use crate::CommandResult;
use pkhex_core::{binary::InvalidData, gen3::Save3};

pub fn execute(input: &[u8], _hint: &str) -> CommandResult {
    if !pkhex_core::emerald::is_valid(input) {
        return CommandResult::failure("Sorting this save format has not been migrated.");
    }
    match sort_emerald(input) {
        Ok(save_a) => CommandResult {
            success: true,
            save_a,
            ..CommandResult::default()
        },
        Err(error) => CommandResult::failure(error.to_string()),
    }
}

fn sort_emerald(input: &[u8]) -> Result<Vec<u8>, InvalidData> {
    let mut save = Save3::read(input)?;
    let mut boxes = save.boxes()?;
    sort(&mut boxes, |first, second| {
        compare(
            first.species(),
            first.form(),
            second.species(),
            second.form(),
        )
    });
    for (index, pk) in boxes.iter().enumerate() {
        save.set_box(index, pk);
    }
    Ok(save.write())
}

pub(crate) fn compare(first: u16, first_form: u8, second: u16, second_form: u8) -> i32 {
    if first == 0 {
        1
    } else if second == 0 {
        -1
    } else if first != second {
        i32::from(first) - i32::from(second)
    } else {
        i32::from(first_form) - i32::from(second_form)
    }
}

// Match .NET 10 List.Sort's introsort, including tie and empty-record placement.
// Reference: dotnet/runtime v10.0.0, System.Collections.Generic.ArraySortHelper (MIT).
pub(crate) fn sort<T: Clone>(values: &mut [T], compare: impl Fn(&T, &T) -> i32 + Copy) {
    if values.len() > 1 {
        intro(values, 2 * (values.len().ilog2() + 1), compare);
    }
}

fn swap_if<T>(values: &mut [T], a: usize, b: usize, cmp: impl Fn(&T, &T) -> i32) {
    if cmp(&values[a], &values[b]) > 0 {
        values.swap(a, b);
    }
}

fn intro<T: Clone>(mut values: &mut [T], mut depth: u32, cmp: impl Fn(&T, &T) -> i32 + Copy) {
    while values.len() > 1 {
        let length = values.len();
        if length <= 16 {
            if length == 2 {
                swap_if(values, 0, 1, cmp);
            } else if length == 3 {
                swap_if(values, 0, 1, cmp);
                swap_if(values, 0, 2, cmp);
                swap_if(values, 1, 2, cmp);
            } else {
                for index in 1..length {
                    let value = values[index].clone();
                    let mut target = index;
                    while target > 0 && cmp(&value, &values[target - 1]) < 0 {
                        values[target] = values[target - 1].clone();
                        target -= 1;
                    }
                    values[target] = value;
                }
            }
            return;
        }
        if depth == 0 {
            for root in (1..=length / 2).rev() {
                down_heap(values, root, length, cmp);
            }
            for count in (2..=length).rev() {
                values.swap(0, count - 1);
                down_heap(values, 1, count - 1, cmp);
            }
            return;
        }
        depth -= 1;
        let last = length - 1;
        let middle = last / 2;
        swap_if(values, 0, middle, cmp);
        swap_if(values, 0, last, cmp);
        swap_if(values, middle, last, cmp);
        let pivot = values[middle].clone();
        values.swap(middle, last - 1);
        let (mut left, mut right) = (0, last - 1);
        while left < right {
            left += 1;
            while cmp(&values[left], &pivot) < 0 {
                left += 1;
            }
            right -= 1;
            while cmp(&pivot, &values[right]) < 0 {
                right -= 1;
            }
            if left >= right {
                break;
            }
            values.swap(left, right);
        }
        values.swap(left, last - 1);
        let (lower, upper) = values.split_at_mut(left);
        intro(&mut upper[1..], depth, cmp);
        values = lower;
    }
}

fn down_heap<T: Clone>(
    values: &mut [T],
    mut root: usize,
    count: usize,
    cmp: impl Fn(&T, &T) -> i32,
) {
    let value = values[root - 1].clone();
    while root <= count / 2 {
        let mut child = root * 2;
        if child < count && cmp(&values[child - 1], &values[child]) < 0 {
            child += 1;
        }
        if cmp(&value, &values[child - 1]) >= 0 {
            break;
        }
        values[root - 1] = values[child - 1].clone();
        root = child;
    }
    values[root - 1] = value;
}
