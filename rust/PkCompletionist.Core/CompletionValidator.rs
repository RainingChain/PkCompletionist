use crate::{CommandResult, command::{load, write, Format}, pinball_validator, pinball1_validator, shuffle_validator as shuffle, ranger_validator as ranger, rescue_validator as rescue, sky_validator as sky, crystal_validator};
/// Objective numbering remains compatible with the C# JS exports (0 = normal).
pub fn validate(data: &[u8], hint: &str, objective: i32) -> CommandResult {
    let (format, data) = match load(data, hint) { Ok(save) => save, Err(error) => return CommandResult::failure(error.to_string()) };
    let mut result = CommandResult { success: true, ..CommandResult::default() };
    match format {
        Format::PinballRs => pinball_validator::validate(&data, &mut result),
        Format::Pinball => pinball1_validator::validate(&data, &mut result),
        Format::Shuffle => shuffle::validate(&data, &mut result),
        Format::Ranger => ranger::validate(&data, &mut result),
        Format::Rescue => if let Err(error) = rescue::validate(&data, objective, &mut result) { return CommandResult::failure(error.to_string()); },
        Format::Sky => if let Err(error) = sky::validate(&data, &mut result) { return CommandResult::failure(error.to_string()); },
        Format::Crystal => if let Err(error) = crystal_validator::validate(&data, objective, &mut result) { return CommandResult::failure(error.to_string()); },
    }
    match write(format, data) {
        Ok(bytes) => result.save_a = bytes,
        Err(error) => return CommandResult::failure(error.to_string()),
    }
    result
}

