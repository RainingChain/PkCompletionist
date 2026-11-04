use crate::CommandResult;
use pkhex_core::gen2::CrystalSave;

/// Crystal branch of FlagAnalyzer.Execute. Indices address the ordered difference list.
pub fn analyze(before: &[u8], after: &[u8], ranges: &str, _hint: &str) -> CommandResult {
    let parsed: Result<Vec<_>, _> = ranges.split(',').map(|value| value.trim().parse::<i32>()).collect();
    let range = match parsed {
        Ok(values) if values.len() >= 2 && values[0] >= 0 && values[1] >= 0 => values,
        _ => return CommandResult::failure("Invalid change range."),
    };
    let mut first = match CrystalSave::read(before) { Ok(save) => save, Err(error) => return CommandResult::failure(error.to_string()) };
    let second = match CrystalSave::read(after) { Ok(save) => save, Err(error) => return CommandResult::failure(error.to_string()) };
    let ignored = [0x1252,0x1253,0x1254,0x1255,0x1f0d,0x1f0e,0x2052,0x2053,0x2054,0x2055,0x2d0d,0x2d0e];
    let differences: Vec<_> = first.data.iter().zip(&second.data).enumerate().filter(|(offset, (a,b))| a != b && !ignored.contains(offset)).map(|(offset, _)| offset).collect();
    let mut result = CommandResult { success: true, ..CommandResult::default() };
    result.messages.push(format!("Change count + 1: {}", differences.len() + 1));
    for (index, offset) in differences.iter().enumerate().take(range[1] as usize).skip(range[0] as usize) {
        let old = first.data[*offset];
        let new = second.data[*offset];
        result.messages.push(format!("this.sav1.Data[0x{offset:X}] = 0x{new:X} (old=0x{old:X}) (0b{old:08b} -> 0b{new:08b}, xor = 0b{:08b}) #{index}", old ^ new));
        first.data[*offset] = new;
    }
    result.save_a = first.write();
    result.save_b = second.write();
    result
}
