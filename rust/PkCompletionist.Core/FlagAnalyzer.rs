use crate::{
    command::{load, write, Format},
    CommandResult,
};
use pkhex_core::{binary::InvalidData, gen1::Save1, gen2::CrystalSave};

/// Indices address the ordered difference list; the end is exclusive.
pub fn analyze(before: &[u8], after: &[u8], ranges: &str, hint: &str) -> CommandResult {
    let parsed: Result<Vec<_>, _> = ranges
        .split(',')
        .map(|value| value.trim().parse::<i32>())
        .collect();
    let range = match parsed {
        Ok(values) if values.len() >= 2 && values[0] >= 0 && values[1] >= 0 => values,
        _ => return CommandResult::failure("Invalid change range."),
    };
    let mut first = match FlagSave::read(before, hint) {
        Ok(save) => save,
        Err(error) => return CommandResult::failure(error.to_string()),
    };
    let second = match FlagSave::read(after, hint) {
        Ok(save) => save,
        Err(error) => return CommandResult::failure(error.to_string()),
    };
    if first.data().len() != second.data().len() {
        return finish(
            first,
            second,
            CommandResult::failure("this.sav1.Data.Length != this.sav2.Data.Length"),
        );
    }
    let ignored: &[usize] = if matches!(&first, FlagSave::Crystal(_)) {
        &[
            0x1252, 0x1253, 0x1254, 0x1255, 0x1f0d, 0x1f0e, 0x2052, 0x2053, 0x2054, 0x2055, 0x2d0d,
            0x2d0e,
        ]
    } else {
        &[]
    };
    let differences: Vec<_> = first
        .data()
        .iter()
        .zip(second.data())
        .enumerate()
        .filter(|(offset, (a, b))| a != b && !ignored.contains(offset))
        .map(|(offset, _)| offset)
        .collect();
    let mut result = CommandResult {
        success: true,
        ..CommandResult::default()
    };
    result
        .messages
        .push(format!("Change count + 1: {}", differences.len() + 1));
    for (index, offset) in differences
        .iter()
        .enumerate()
        .take(range[1] as usize)
        .skip(range[0] as usize)
    {
        let old = first.data()[*offset];
        let new = second.data()[*offset];
        result.messages.push(format!("this.sav1.Data[0x{offset:X}] = 0x{new:X} (old=0x{old:X}) (0b{old:08b} -> 0b{new:08b}, xor = 0b{:08b}) #{index}",old ^ new));
        first.data_mut()[*offset] = new;
    }
    finish(first, second, result)
}

fn finish(first: FlagSave, second: FlagSave, mut result: CommandResult) -> CommandResult {
    match first.write() {
        Ok(bytes) => result.save_a = bytes,
        Err(error) => return CommandResult::failure(error.to_string()),
    }
    match second.write() {
        Ok(bytes) => result.save_b = bytes,
        Err(error) => return CommandResult::failure(error.to_string()),
    }
    result
}

// Preserve the constructor-time PKM/bit buffers when FlagAnalyzer replaces Data.
enum FlagSave {
    Crystal(CrystalSave),
    Yellow(Save1),
    Emerald(pkhex_core::gen3::Save3),
    Custom {
        format: Format,
        data: Vec<u8>,
        source: Vec<u8>,
    },
}

impl FlagSave {
    fn read(input: &[u8], hint: &str) -> Result<Self, InvalidData> {
        let (format, data) = load(input, hint)?;
        Ok(match format {
            Format::Crystal => Self::Crystal(CrystalSave::read(&data)?),
            Format::Yellow => Self::Yellow(Save1::read(&data)?),
            Format::Emerald => Self::Emerald(pkhex_core::gen3::Save3::read(&data)?),
            _ => Self::Custom {
                format,
                source: data.clone(),
                data,
            },
        })
    }
    fn data(&self) -> &[u8] {
        match self {
            Self::Crystal(save) => &save.data,
            Self::Yellow(save) => &save.data,
            Self::Emerald(save) => &save.data,
            Self::Custom { data, .. } => data,
        }
    }
    fn data_mut(&mut self) -> &mut [u8] {
        match self {
            Self::Crystal(save) => &mut save.data,
            Self::Yellow(save) => &mut save.data,
            Self::Emerald(save) => &mut save.data,
            Self::Custom { data, .. } => data,
        }
    }
    fn write(self) -> Result<Vec<u8>, InvalidData> {
        match self {
            Self::Crystal(save) => Ok(save.write()),
            Self::Yellow(save) => Ok(save.write()),
            Self::Emerald(save) => Ok(save.write()),
            Self::Custom {
                format: Format::Sky,
                mut data,
                source,
            } => {
                crate::sky_save::write_from(&source, &mut data)?;
                Ok(data)
            }
            Self::Custom { format, data, .. } => write(format, data),
        }
    }
}
