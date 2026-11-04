use crate::{
    command::{load, write, Format},
    crystal_validator, pinball1_validator, pinball_validator, ranger_validator as ranger,
    rescue_validator as rescue, shuffle_validator as shuffle, sky_validator as sky, CommandResult,
};
/// Objective numbering remains compatible with the C# JS exports (0 = normal).
pub fn validate(data: &[u8], hint: &str, objective: i32) -> CommandResult {
    let (format, data) = match load(data, hint) {
        Ok(save) => save,
        Err(error) => return CommandResult::failure(error.to_string()),
    };
    let mut result = CommandResult {
        success: true,
        ..CommandResult::default()
    };
    match format {
        Format::Emerald => {
            if let Err(error) = crate::emerald_validator::validate(&data, objective, &mut result) {
                return CommandResult::failure(error.to_string());
            }
        }
        Format::Yellow => {
            if let Err(error) = crate::yellow_validator::validate(&data, objective, &mut result) {
                return CommandResult::failure(error.to_string());
            }
        }
        Format::PinballRs => pinball_validator::validate(&data, &mut result),
        Format::Pinball => pinball1_validator::validate(&data, &mut result),
        Format::Shuffle => shuffle::validate(&data, &mut result),
        Format::Ranger => ranger::validate(&data, &mut result),
        Format::Rescue => {
            if let Err(error) = rescue::validate(&data, objective, &mut result) {
                return CommandResult::failure(error.to_string());
            }
        }
        Format::Sky => {
            if let Err(error) = sky::validate(&data, &mut result) {
                return CommandResult::failure(error.to_string());
            }
        }
        Format::Crystal => {
            if let Err(error) = crystal_validator::validate(&data, objective, &mut result) {
                return CommandResult::failure(error.to_string());
            }
        }
    }
    match write(format, data) {
        Ok(bytes) => result.save_a = bytes,
        Err(error) => return CommandResult::failure(error.to_string()),
    }
    result
}
