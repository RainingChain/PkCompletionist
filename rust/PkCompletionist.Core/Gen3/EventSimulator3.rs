use crate::{
    sav_utils::{add_item3, add_pokemon3},
    CommandResult,
};
use pkhex_core::gen3::Save3;

pub fn execute(name: &str, input: &[u8]) -> CommandResult {
    let normalized = crate::event_simulator_x::event_name(
        name,
        &[
            "MysticTicket",
            "OldSeaMap",
            "AuroraTicket",
            "EonTicket",
            "PokemonFestaRibbons",
            "Deoxys",
            "Jirachi",
            "Celebi",
            "Mew",
        ],
    );
    let name = normalized.as_ref();
    let mut save = match Save3::read(input) {
        Ok(save) => save,
        Err(error) => return CommandResult::failure(error.to_string()),
    };
    let error = match name.trim() {
        "MysticTicket" | "0" => ticket(
            &mut save,
            &[0x13b, 0x91, 0x92],
            &[0x8e0, 0x13b],
            370,
            "You already obtained MysticTicket.",
        ),
        "OldSeaMap" | "1" => ticket(
            &mut save,
            &[0x13c, 0x1ca],
            &[0x8d6, 0x13c],
            376,
            "You already obtained Old Sea Map.",
        ),
        "AuroraTicket" | "2" => ticket(
            &mut save,
            &[0x13a, 0x1ad],
            &[0x8d5, 0x13a],
            371,
            "You already obtained AuroraTicket.",
        ),
        "EonTicket" | "3" => ticket(
            &mut save,
            &[0x8b3],
            &[0x8b3],
            275,
            "You already obtained Eon Ticket.",
        ),
        "PokemonFestaRibbons" | "4" => match save.party() {
            Ok(party) if party.is_empty() => Some("Your party is empty."),
            Ok(mut party) => {
                party[0].data[0x4e] |= 0x80;
                party[0].data[0x4f] |= 4;
                save.set_party(0, &party[0]);
                None
            }
            Err(_) => Some("Pokemon is not generation 3."),
        },
        "Deoxys" | "5" => add_pokemon3(&mut save, include_bytes!("Resources/Deoxys.pk3")).err(),
        "Jirachi" | "6" => add_pokemon3(&mut save, include_bytes!("Resources/Jirachi.pk3")).err(),
        "Celebi" | "7" => add_pokemon3(&mut save, include_bytes!("Resources/Celebi.pk3")).err(),
        "Mew" | "8" => add_pokemon3(&mut save, include_bytes!("Resources/Mew.pk3")).err(),
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

fn ticket(
    save: &mut Save3,
    already: &[usize],
    set: &[usize],
    item: u16,
    error: &'static str,
) -> Option<&'static str> {
    if already.iter().any(|id| save.event(*id)) {
        return Some(error);
    }
    for id in set {
        save.set_event(*id, true);
    }
    add_item3(save, item).err()
}
