use crate::{sav_utils, CommandResult};
use pkhex_core::gen2::CrystalSave;

pub fn execute(
    name: &str,
    data: &[u8],
    second: Option<&[u8]>,
    random: &mut dyn FnMut() -> u8,
) -> CommandResult {
    let normalized = crate::event_simulator_x::event_name(
        name,
        &[
            "Mew",
            "Celebi",
            "GSBall",
            "EggTicket",
            "BlueskyMail",
            "MirageMail",
            "MysteryGift",
            "MysteryGiftx1000",
        ],
    );
    let name = normalized.as_ref();
    let mut save = match CrystalSave::read(data) {
        Ok(save) => save,
        Err(error) => return CommandResult::failure(error.to_string()),
    };
    let mut result = CommandResult::default();
    let mut other = match second.map(CrystalSave::read).transpose() {
        Ok(save) => save,
        Err(error) => return CommandResult::failure(error.to_string()),
    };
    let error = match name.trim() {
        "Mew" | "0" => save.add_pokemon(include_bytes!("Resources/Mew.pk2")).err(),
        "Celebi" | "1" => save
            .add_pokemon(include_bytes!("Resources/Celebi.pk2"))
            .err(),
        "GSBall" | "2" => {
            if sav_utils::has_pokemon_with_trainer(&save, 251)
                || [832, 190, 191, 192].into_iter().any(|id| save.event(id))
            {
                Some("Error: You already obtained GS Ball.")
            } else {
                let error = sav_utils::add_item(&mut save, 115).err();
                if error.is_none() {
                    save.set_event(832, true);
                }
                error
            }
        }
        "EggTicket" | "3" => sav_utils::add_item(&mut save, 129).err(),
        "BlueskyMail" | "4" => sav_utils::add_item(&mut save, 187).err(),
        "MirageMail" | "5" => sav_utils::add_item(&mut save, 189).err(),
        "MysteryGift" | "6" | "MysteryGiftx1000" | "7" => {
            if let Some(second) = &mut other {
                let many = matches!(name.trim(), "MysteryGiftx1000" | "7");
                crate::mystery_gift2::execute(
                    &mut save,
                    second,
                    if many { 1000 } else { 1 },
                    many,
                    random,
                    &mut result.messages,
                )
            } else {
                Some("The second savefile is not generation 2.")
            }
        }
        _ => Some("Invalid event name."),
    };
    result.success = error.is_none();
    if let Some(error) = error {
        result.error = Some(error.to_string());
        result.messages.push(error.to_string());
    }
    result.save_a = save.write();
    if let Some(other) = other {
        result.save_b = other.write();
    }
    result
}
