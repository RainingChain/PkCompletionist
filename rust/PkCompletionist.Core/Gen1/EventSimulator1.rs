use crate::CommandResult;
use pkhex_core::gen1::Save1;

pub fn execute(name: &str, data: &[u8]) -> CommandResult {
    let normalized = crate::event_simulator_x::event_name(name, &["Mew", "PikachuSurf"]);
    let name = normalized.as_ref();
    let mut save = match Save1::read(data) {
        Ok(save) => save,
        Err(error) => return CommandResult::failure(error.to_string()),
    };
    let error = match name.trim() {
        "Mew" | "0" => save.add_pokemon(include_bytes!("Resources/Mew.pk1")).err(),
        "PikachuSurf" | "1" => save
            .add_pokemon(include_bytes!("Resources/PikachuSurf.pk1"))
            .err(),
        _ => Some("Invalid event name."),
    };
    let mut result = CommandResult {
        success: error.is_none(),
        save_a: save.write(),
        ..CommandResult::default()
    };
    if let Some(error) = error {
        result.error = Some(error.to_string());
        result.messages.push(error.to_string());
    }
    result
}
