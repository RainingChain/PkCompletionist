use crate::{
    command::{load, write, Format},
    pinball_events as pinball, shuffle_events as shuffle, CommandResult,
};
pub fn event(
    name: &str,
    data: &[u8],
    hint: &str,
    second: Option<&[u8]>,
    random: &mut dyn FnMut() -> u8,
) -> CommandResult {
    let (format, mut data) = match load(data, hint) {
        Ok(save) => save,
        Err(error) => return CommandResult::failure(error.to_string()),
    };
    if format == Format::Crystal {
        return crate::crystal_events::execute(name, &data, second, random);
    }
    if format == Format::Yellow {
        return crate::yellow_events::execute(name, &data);
    }
    if format == Format::Emerald {
        return crate::emerald_events::execute(name, &data);
    }
    let mut result = CommandResult::default();
    if let Some(second) = second {
        match load(second, hint).and_then(|(format, bytes)| write(format, bytes)) {
            Ok(bytes) => result.save_b = bytes,
            Err(error) => return CommandResult::failure(error.to_string()),
        }
    }
    let error = match format {
        Format::PinballRs => pinball::event(name, &mut data),
        Format::Shuffle => shuffle::event(name, &mut data),
        _ => crate::event_simulator_x::unsupported(),
    };
    result.success = error.is_none();
    if let Some(error) = error {
        result.messages.push(error.clone());
        result.error = Some(error);
    }
    match write(format, data) {
        Ok(bytes) => result.save_a = bytes,
        Err(error) => return CommandResult::failure(error.to_string()),
    }
    result
}
