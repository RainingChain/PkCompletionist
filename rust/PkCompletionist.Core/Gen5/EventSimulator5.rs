#![allow(non_snake_case)]
use crate::{
    command::{load, write},
    CommandResult,
};
use pkhex_core::{gen5::Save5, pk5::Pk5};

const EVENTS: &[&str] = &[
    "Arceus_BW_Event",
    "Victini_BW_Event",
    "Deoxys_B2_Event",
    "Genesect_B2_Event",
    "Genesect_Shiny_B2_Event",
    "DarkraiClassicRibbonEnigmaBerry_BW_Event",
    "Shaymin_BW_Event",
    "KeldeoWishingRibbon_B2_Event",
    "MeloettaWishingRibbon_B2_Event",
    "CustapBerryWishingRibbon_B2_Event",
    "PremierRibbon_BW_Event",
    "BirthdayRibbon_B2_Event",
    "LightBallEventRibbon_B2_Event",
    "BattleChampionRibbon_B2_Event",
    "SouvenirRibbon_B2_Event",
    "PokemonFestaRibbons_RSE_Event",
    "Mew_MyPokemonRanch",
];

struct EventSimulator5<'a> {
    sav: &'a mut Save5,
    second_count: usize,
}
impl EventSimulator5<'_> {
    fn ExecEvent(&mut self, name: &str) -> Result<(), String> {
        let event = crate::event_simulator_x::parse_event(name, EVENTS);
        match event {
            Some(0) => crate::sav_utils::add_pokemon5(
                self.sav,
                include_bytes!("Resources/Global Link Arceus.pkm"),
            )
            .map_err(str::to_string),
            Some(1..=14) => {
                // SAV5 exposes IMysteryGiftStorageProvider, whereas the C# SavUtils
                // requires IMysteryGiftStorage (and only unwraps the SAV4 provider).
                // All the fixed PGF resources pass its preliminary compatibility checks.
                Err("Error: This save file does not support mystery gift storage.".into())
            }
            Some(15) => {
                if self.sav.data[0x18e04] == 0 {
                    return Err("Your party is empty.".into());
                }
                let mut pk = Pk5::read_encrypted(&self.sav.data[0x18e08..0x18ee4])
                    .map_err(|err| err.to_string())?;
                pk.data[0x3f] |= 0x90; // Country and World ribbons.
                self.sav.set_party(0, &pk);
                Ok(())
            }
            Some(16) => {
                let count =
                    self.sav.pokemon().map_err(|err| err.to_string())?.len() + self.second_count;
                if count < 999 {
                    return Err(format!("Error: You must own at least 999 Pokémon within both savefiles to obtain Mew from My Pokémon Ranch. You currently only own {count} Pokémon."));
                }
                if !self
                    .sav
                    .boxes()
                    .map_err(|err| err.to_string())?
                    .iter()
                    .any(|pk| pk.species() == 0)
                {
                    return Err(
                        "Error: All pokemon boxes are full. Free a box slot to fix the issue."
                            .into(),
                    );
                }
                // The reference inserts the PK4 resource directly and throws this
                // format error; retain the message as a recoverable command failure.
                Err("PKM Format needs to be PKHeX.Core.PK5 when setting to this Save File.".into())
            }
            _ => {
                let label = if event.is_some() {
                    crate::event_simulator_x::event_name(name, EVENTS).into_owned()
                } else {
                    String::new()
                };
                Err(format!("Internal error : Invalid event name \"{label}\"."))
            }
        }
    }
}

pub fn execute(name: &str, input: &[u8], second: Option<&[u8]>, hint: &str) -> CommandResult {
    let mut save = match Save5::read(input) {
        Ok(save) => save,
        Err(err) => return CommandResult::failure(err.to_string()),
    };
    let mut save_b = Vec::new();
    let mut second_count = 0;
    if let Some(input) = second {
        let parsed = load(input, hint).and_then(|(format, data)| {
            let count = crate::sav_utils::pokemon_count(format, &data)?;
            Ok((write(format, data)?, count))
        });
        match parsed {
            Ok((bytes, count)) => {
                save_b = bytes;
                second_count = count;
            }
            Err(err) => return CommandResult::failure(err.to_string()),
        }
    }
    let error = EventSimulator5 {
        sav: &mut save,
        second_count,
    }
    .ExecEvent(name)
    .err();
    let mut result = CommandResult {
        success: error.is_none(),
        save_a: save.write(),
        save_b,
        ..CommandResult::default()
    };
    if let Some(error) = error {
        result.messages.push(error.clone());
        result.error = Some(error);
    }
    result
}
