#![allow(non_snake_case)]
use crate::{category, completion_validator_x::Checklist, CommandResult};
use pkhex_core::{
    binary::{u16_le, InvalidData},
    gen5::Save5,
    pk5::Pk5,
    record5::Record5,
};

struct CompletionValidator5 {
    sav: Save5,
    pokemon: Vec<Pk5>,
    items: Vec<u16>,
    records: Record5,
    objective: i32,
}

pub fn validate(
    input: &[u8],
    objective: i32,
    result: &mut CommandResult,
) -> Result<(), InvalidData> {
    let sav = Save5::read(input)?;
    let pokemon = sav.pokemon()?;
    let mut items = pkhex_core::player_bag5b2w2::items(&sav);
    items.extend(
        pokemon
            .iter()
            .map(|pk| pk.held_item())
            .filter(|id| *id != 0),
    );
    let records = sav.records();
    CompletionValidator5 {
        sav,
        pokemon,
        items,
        records,
        objective,
    }
    .GenerateAll(result);
    Ok(())
}

#[derive(Clone, Copy)]
enum PokestarMovieEnding {
    Normal,
    Bad,
    Strange,
}

impl CompletionValidator5 {
    fn GenerateAll(&self, result: &mut CommandResult) {
        self.Generate_pokemon(result);
        self.Generate_item(result);
        self.Generate_pokemonForm(result);
        self.Generate_inGameTrade(result);
        self.Generate_inGameGift(result);
        self.Generate_itemInMap(result);
        self.Generate_itemGift(result);
        self.Generate_battle(result);
        self.Generate_joinAvenue(result);
        self.Generate_ribbon(result);
        self.Generate_battleSubway(result);
        self.Generate_pwt(result);
        self.Generate_medal(result);
        self.Generate_trainerStar(result);
        self.Generate_easyChatSystemWord(result);
        self.Generate_pokestarStudios(result);
        self.Generate_props(result);
        self.Generate_pokemonMusical(result);
        self.Generate_funfest(result);
        self.Generate_pcWallpaper(result);
        self.Generate_uiSkin(result);
        self.Generate_record(result);
        self.Generate_misc(result);
    }
    fn HasPkm(&self, species: u16) -> bool {
        self.pokemon.iter().any(|pk| pk.species() == species)
    }
    fn HasPkmForm(&self, species: u16, form: u8) -> bool {
        self.pokemon
            .iter()
            .any(|pk| pk.species() == species && pk.form() == form)
    }
    fn HasOrSeenPkmBasedOnObjective(&self, species: u16) -> bool {
        if self.objective == 0 {
            self.sav.dex().caught(species)
        } else {
            self.HasPkm(species)
        }
    }
    fn GetDexFormIndexBW(species: u16, formCount: u8) -> i32 {
        if formCount < 1 || species > 649 {
            return -1;
        }
        match species {
            201 => 0,
            386 => 28,
            492 => 32,
            487 => 34,
            479 => 36,
            422 => 42,
            423 => 44,
            412 => 46,
            413 => 49,
            351 => 52,
            421 => 56,
            585 => 58,
            586 => 62,
            648 => 66,
            555 => 68,
            550 => 70,
            _ => -1,
        }
    }
    fn GetDexFormIndexB2W2(species: u16, formCount: u8) -> i32 {
        if formCount < 1 || species > 649 {
            return -1;
        }
        match species {
            646 => 72,
            647 => 75,
            642 => 77,
            641 => 79,
            645 => 81,
            _ => Self::GetDexFormIndexBW(species, formCount),
        }
    }
    fn personal(&self, species: u16, offset: usize) -> u8 {
        const PERSONAL: &[u8] =
            include_bytes!("../../../PKHeX.Core/Resources/byte/personal/personal_b2w2");
        PERSONAL[usize::from(species) * 0x4c + offset]
    }
    fn HasSeenPkmForm(&self, species: u16, form: u8) -> bool {
        let index =
            Self::GetDexFormIndexB2W2(species, self.personal(species, 0x20)) + i32::from(form);
        self.sav.dex().form_seen(index, false) || self.sav.dex().form_seen(index, true)
    }
    fn HasOrSeenPkmBasedOnObjective2(&self, species: u16, form: u8) -> bool {
        if self.objective == 0 {
            self.HasSeenPkmForm(species, form)
        } else {
            self.HasPkmForm(species, form)
        }
    }
    fn HasOrSeenPkmBasedOnObjectiveArceus(&self, species: u16, form: u8) -> bool {
        if self.objective == 0 {
            self.HasSeenPkmForm(species, 0)
        } else {
            self.HasPkmForm(species, form)
        }
    }
    fn Generate_pokemon(&self, result: &mut CommandResult) {
        category(
            result,
            "pokemon",
            (1..=649).map(|id| (id.to_string(), self.HasOrSeenPkmBasedOnObjective(id))),
        );
    }
    fn Generate_item(&self, result: &mut CommandResult) {
        category(
            result,
            "item",
            (1..=638)
                .filter(|id| !UNOBTAINABLE.contains(id))
                .map(|id| (id.to_string(), self.items.contains(&id))),
        );
    }
    fn CompletePokemonForms(&self, ow: &mut Checklist, result: &mut CommandResult) {
        let dex = self.sav.dex();
        for (key, prefix, mode) in [
            (
                "AllMaleFemaleForms",
                "Missing Male & Female Forms: Pokemon ",
                0,
            ),
            (
                "1ShinyFormforAllPokemon",
                "Missing Shiny Forms: Pokemon ",
                1,
            ),
            (
                "ShinyMaleFemaleFormsforAllPokemon",
                "Missing Shiny Male & Female Forms: Pokemon ",
                2,
            ),
            (
                "AllFormsforAllPokemon",
                "Missing All Pokedex Forms: Pokemon ",
                3,
            ),
        ] {
            let mut missing = Vec::new();
            for id in if mode == 3 { 1..=649 } else { 0..=649 } {
                let gender = self.personal(id, 0x12);
                if mode <= 1 && !dex.seen(id) {
                    missing.push(format!("#{id}"));
                    continue;
                }
                if mode == 0 && [0, 254, 255].contains(&gender) {
                    continue;
                }
                if mode == 0 {
                    if !dex.seen_region(id, 0, false) && !dex.seen_region(id, 0, true) {
                        missing.push(format!("#{id} (Male)"));
                        continue;
                    }
                    if !dex.seen_region(id, 1, false) && !dex.seen_region(id, 1, true) {
                        missing.push(format!("#{id} (Female)"));
                    }
                } else if mode == 1 {
                    if !dex.seen_region(id, 0, true) && !dex.seen_region(id, 1, true) {
                        missing.push(format!("#{id}"));
                    }
                } else {
                    let variants: &[(u8, bool, &str)] = if mode == 2 {
                        &[(0, true, "Male"), (1, true, "Female")]
                    } else {
                        &[
                            (0, false, "Male Non-Shiny"),
                            (1, false, "Female Non-Shiny"),
                            (0, true, "Male Shiny"),
                            (1, true, "Female Shiny"),
                        ]
                    };
                    for &(sex, shiny, label) in variants {
                        if (sex == 0 && gender == 254) || (sex == 1 && [0, 255].contains(&gender)) {
                            continue;
                        }
                        if !dex.seen_region(id, sex, shiny) {
                            missing.push(format!("#{id} ({label})"));
                            break;
                        }
                    }
                }
            }
            if mode == 3 {
                for &(name, species, form) in SPECIAL_FORMS {
                    let index = Self::GetDexFormIndexB2W2(species, self.personal(species, 0x20));
                    if index < 0 {
                        continue;
                    }
                    for (shiny, label) in [(false, "Non-Shiny"), (true, "Shiny")] {
                        if !dex.form_seen(index + i32::from(form), shiny) {
                            missing.push(format!("{name} ({label})"));
                        }
                    }
                }
            }
            ow.set(key, missing.is_empty());
            if !missing.is_empty() {
                result
                    .completion_hints
                    .push(format!("{prefix}{}", missing.join(", ")));
            }
        }
        let mut missing = Vec::new();
        for id in 1..=493 {
            for (language, label) in ["JP", "EN", "FR", "IT", "GE", "SP"].iter().enumerate() {
                if !dex.language(id, language) {
                    missing.push(format!("#{id} ({label})"));
                }
            }
        }
        ow.set("AllForeignPokedexEntries", missing.is_empty());
        if !missing.is_empty() {
            result.completion_hints.push(format!(
                "Missing Foreign Pokedex Entries: Pokemon {}",
                missing.join(", ")
            ));
        }
    }
    fn HasTradeMon(&self, species: u16, tid: u16, trainer: &str) -> bool {
        self.pokemon.iter().any(|pk| {
            pk.species() == species && pk.trainer_id() == tid && pk.original_trainer() == trainer
        })
    }
    fn has_rank_10_join_avenue_shop(&self, ShopType: i32) -> bool {
        (0..8).any(|index| {
            let bytes =
                i32::from(u16_le(&self.sav.data, 0x23c00 + 0xaac + index * 0xc4 + 0xb4).unwrap());
            if bytes == 0xffff {
                return false;
            }
            let level = if bytes % 0x50 == 0 {
                9
            } else {
                (bytes % 0x50 - 1) % 10
            };
            let shop = if level == 9 {
                (bytes - 1) % 0x50 / 10
            } else {
                bytes % 0x50 / 10
            };
            shop == ShopType && level == 9
        })
    }
    fn HasRibbon(&self, offset: usize, bit: u8) -> bool {
        self.pokemon.iter().any(|pk| pk.ribbon(offset, bit))
    }
    fn EnteredHallOfFame(&self) -> bool {
        self.records.record32(2) > 0
    }
    fn Generate_medal(&self, result: &mut CommandResult) {
        category(
            result,
            "medal",
            (0..255).map(|id| (id.to_string(), self.sav.data[0x25300 + id * 4 + 2] & 7 == 4)),
        );
    }
    fn Generate_trainerStar(&self, result: &mut CommandResult) {
        category(
            result,
            "trainerStar",
            [
                ("HallofFame".into(), self.EnteredHallOfFame()),
                (
                    "NationalPokedex".into(),
                    (0..=649).all(|id| {
                        [
                            151, 249, 250, 251, 385, 386, 489, 490, 491, 492, 493, 494, 647, 648,
                            649,
                        ]
                        .contains(&id)
                            || self.sav.dex().caught(id)
                    }),
                ),
                (
                    "PokestarStudios".into(),
                    (0..=39).all(|id| self.HasCompletedPokestar(id)),
                ),
                ("ChampionsTournament".into(), self.sav.pwt(17) >= 1),
                (
                    "Entralink".into(),
                    u16_le(&self.sav.data, 0x2120e).unwrap() >= 30
                        && u16_le(&self.sav.data, 0x2120c).unwrap() >= 30,
                ),
            ],
        );
    }
    fn GetBit(&self, byte_offset: usize, bit_index: usize) -> bool {
        self.sav.data[byte_offset + bit_index / 8] & (1 << (bit_index % 8)) != 0
    }
    fn HasPokestarEnding(&self, scenarioIdx: usize, ending: PokestarMovieEnding) -> bool {
        self.GetBit(
            0x77000
                + match ending {
                    PokestarMovieEnding::Normal => 0x10,
                    PokestarMovieEnding::Bad => 0x2c4,
                    PokestarMovieEnding::Strange => 0x2b4,
                },
            scenarioIdx,
        )
    }
    fn HasCompletedPokestar(&self, scenarioIdx: usize) -> bool {
        [
            PokestarMovieEnding::Normal,
            PokestarMovieEnding::Bad,
            PokestarMovieEnding::Strange,
        ]
        .iter()
        .any(|ending| self.HasPokestarEnding(scenarioIdx, *ending))
    }
    fn Generate_pokestarStudios(&self, result: &mut CommandResult) {
        category(
            result,
            "pokestarStudios",
            (0..=39).flat_map(|id| {
                [
                    ("N", PokestarMovieEnding::Normal),
                    ("B", PokestarMovieEnding::Bad),
                    ("S", PokestarMovieEnding::Strange),
                ]
                .map(|(suffix, ending)| {
                    (format!("{id}{suffix}"), self.HasPokestarEnding(id, ending))
                })
            }),
        );
    }
    fn Generate_record(&self, result: &mut CommandResult) {
        category(
            result,
            "record",
            (0..=53)
                .map(|id| (id.to_string(), self.records.record32(id) > 0))
                .chain((0..=60).map(|id| (format!("s{id}"), self.records.record16(id) > 0))),
        );
    }
    fn Generate_props(&self, result: &mut CommandResult) {
        category(
            result,
            "props",
            (0..=99).map(|id| (id.to_string(), self.GetBit(0x1f958, id))),
        );
    }
    fn Generate_easyChatSystemWord(&self, result: &mut CommandResult) {
        category(
            result,
            "easyChatSystemWord",
            [
                (
                    "PokemonAllCatchableNoTrade".into(),
                    NO_TRADE.iter().all(|id| self.sav.dex().seen(*id)),
                ),
                (
                    "PokemonOthers".into(),
                    (1..=493).all(|id| self.sav.dex().seen(id)),
                ),
            ],
        );
    }
    fn Generate_pcWallpaper(&self, result: &mut CommandResult) {
        let mut ow = Checklist::default();
        for name in [
            "Forest",
            "City",
            "Desert",
            "Savanna",
            "Crag",
            "Volcano",
            "Snow",
            "Cave",
            "Beach",
            "Seafloor",
            "River",
            "Sky",
            "Checks",
            "PokeCenter",
            "Machine",
            "Simple",
        ] {
            ow.set(name, true);
        }
        for name in ["Monochrome", "TeamPlasma", "Movie", "PWT"] {
            ow.set(name, self.EnteredHallOfFame());
        }
        for name in ["BlackKyurem", "WhiteKyurem", "Reshiram", "Zekrom"] {
            ow.set(name, self.HasPkm(644) && self.HasPkm(646));
        }
        ow.emit(result, "pcWallpaper");
    }
    // These categories are empty in the C# reference too.
    fn Generate_itemInMap(&self, result: &mut CommandResult) {
        Checklist::default().emit(result, "itemInMap");
    }
    fn Generate_itemGift(&self, result: &mut CommandResult) {
        Checklist::default().emit(result, "itemGift");
    }
    fn Generate_battle(&self, result: &mut CommandResult) {
        Checklist::default().emit(result, "battle");
    }
    fn Generate_pokemonMusical(&self, result: &mut CommandResult) {
        Checklist::default().emit(result, "pokemonMusical");
    }
    fn Generate_funfest(&self, result: &mut CommandResult) {
        Checklist::default().emit(result, "funfest");
    }
    fn Generate_uiSkin(&self, result: &mut CommandResult) {
        Checklist::default().emit(result, "uiSkin");
    }
    fn Generate_misc(&self, result: &mut CommandResult) {
        Checklist::default().emit(result, "misc");
    }
}
// Generated literal rules follow. See tools/port-black2-rules.ps1.
impl CompletionValidator5 {
    fn Generate_pokemonForm(&self, result: &mut CommandResult) {
        let mut ow = Checklist::default();
        ow.set("UnownA", self.HasOrSeenPkmBasedOnObjective2(201, 0));
        ow.set("UnownB", self.HasOrSeenPkmBasedOnObjective2(201, 1));
        ow.set("UnownC", self.HasOrSeenPkmBasedOnObjective2(201, 2));
        ow.set("UnownD", self.HasOrSeenPkmBasedOnObjective2(201, 3));
        ow.set("UnownE", self.HasOrSeenPkmBasedOnObjective2(201, 4));
        ow.set("UnownF", self.HasOrSeenPkmBasedOnObjective2(201, 5));
        ow.set("UnownG", self.HasOrSeenPkmBasedOnObjective2(201, 6));
        ow.set("UnownH", self.HasOrSeenPkmBasedOnObjective2(201, 7));
        ow.set("UnownI", self.HasOrSeenPkmBasedOnObjective2(201, 8));
        ow.set("UnownJ", self.HasOrSeenPkmBasedOnObjective2(201, 9));
        ow.set("UnownK", self.HasOrSeenPkmBasedOnObjective2(201, 10));
        ow.set("UnownL", self.HasOrSeenPkmBasedOnObjective2(201, 11));
        ow.set("UnownM", self.HasOrSeenPkmBasedOnObjective2(201, 12));
        ow.set("UnownN", self.HasOrSeenPkmBasedOnObjective2(201, 13));
        ow.set("UnownO", self.HasOrSeenPkmBasedOnObjective2(201, 14));
        ow.set("UnownP", self.HasOrSeenPkmBasedOnObjective2(201, 15));
        ow.set("UnownQ", self.HasOrSeenPkmBasedOnObjective2(201, 16));
        ow.set("UnownR", self.HasOrSeenPkmBasedOnObjective2(201, 17));
        ow.set("UnownS", self.HasOrSeenPkmBasedOnObjective2(201, 18));
        ow.set("UnownT", self.HasOrSeenPkmBasedOnObjective2(201, 19));
        ow.set("UnownU", self.HasOrSeenPkmBasedOnObjective2(201, 20));
        ow.set("UnownV", self.HasOrSeenPkmBasedOnObjective2(201, 21));
        ow.set("UnownW", self.HasOrSeenPkmBasedOnObjective2(201, 22));
        ow.set("UnownX", self.HasOrSeenPkmBasedOnObjective2(201, 23));
        ow.set("UnownY", self.HasOrSeenPkmBasedOnObjective2(201, 24));
        ow.set("UnownZ", self.HasOrSeenPkmBasedOnObjective2(201, 25));
        ow.set(
            "UnownExclamationMark",
            self.HasOrSeenPkmBasedOnObjective2(201, 26),
        );
        ow.set(
            "UnownQuestionMark",
            self.HasOrSeenPkmBasedOnObjective2(201, 27),
        );
        ow.set("CastformNormal", self.HasOrSeenPkmBasedOnObjective2(351, 0));
        ow.set("CastformFire", self.HasOrSeenPkmBasedOnObjective2(351, 1));
        ow.set("CastformWater", self.HasOrSeenPkmBasedOnObjective2(351, 2));
        ow.set("CastformIce", self.HasOrSeenPkmBasedOnObjective2(351, 3));
        ow.set("DeoxysNormal", self.HasOrSeenPkmBasedOnObjective2(386, 0));
        ow.set("DeoxysAttack", self.HasOrSeenPkmBasedOnObjective2(386, 1));
        ow.set("DeoxysDefense", self.HasOrSeenPkmBasedOnObjective2(386, 2));
        ow.set("DeoxysSpeed", self.HasOrSeenPkmBasedOnObjective2(386, 3));
        ow.set(
            "BurmyPlantCloak",
            self.HasOrSeenPkmBasedOnObjective2(412, 0),
        );
        ow.set(
            "BurmySandyCloak",
            self.HasOrSeenPkmBasedOnObjective2(412, 1),
        );
        ow.set(
            "BurmyTrashCloak",
            self.HasOrSeenPkmBasedOnObjective2(412, 2),
        );
        ow.set(
            "WormadamPlantCloak",
            self.HasOrSeenPkmBasedOnObjective2(413, 0),
        );
        ow.set(
            "WormadamSandyCloak",
            self.HasOrSeenPkmBasedOnObjective2(413, 1),
        );
        ow.set(
            "WormadamTrashCloak",
            self.HasOrSeenPkmBasedOnObjective2(413, 2),
        );
        ow.set("CherrimOvercast", self.HasOrSeenPkmBasedOnObjective(421));
        ow.set("CherrimSunshine", self.HasOrSeenPkmBasedOnObjective(421));
        ow.set("ShellosWestSea", self.HasOrSeenPkmBasedOnObjective2(422, 0));
        ow.set("ShellosEastSea", self.HasOrSeenPkmBasedOnObjective2(422, 1));
        ow.set(
            "GastrodonWestSea",
            self.HasOrSeenPkmBasedOnObjective2(423, 0),
        );
        ow.set(
            "GastrodonEastSea",
            self.HasOrSeenPkmBasedOnObjective2(423, 1),
        );
        ow.set("RotomGhost", self.HasOrSeenPkmBasedOnObjective2(479, 0));
        ow.set("RotomHeat", self.HasOrSeenPkmBasedOnObjective2(479, 1));
        ow.set("RotomWash", self.HasOrSeenPkmBasedOnObjective2(479, 2));
        ow.set("RotomFrost", self.HasOrSeenPkmBasedOnObjective2(479, 3));
        ow.set("RotomFan", self.HasOrSeenPkmBasedOnObjective2(479, 4));
        ow.set("RotomMow", self.HasOrSeenPkmBasedOnObjective2(479, 5));
        ow.set(
            "GiratinaAltered",
            self.HasOrSeenPkmBasedOnObjective2(487, 0),
        );
        ow.set("GiratinaOrigin", self.HasSeenPkmForm(487, 1));
        ow.set("ShayminLand", self.HasOrSeenPkmBasedOnObjective2(492, 0));
        ow.set("ShayminSky", self.HasOrSeenPkmBasedOnObjective2(492, 1));
        ow.set("GetShinyPokemon", self.pokemon.iter().any(|pk| pk.shiny()));
        ow.set(
            "ArceusNormal",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 0),
        );
        ow.set(
            "ArceusFighting",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 1),
        );
        ow.set(
            "ArceusFlying",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 2),
        );
        ow.set(
            "ArceusPoison",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 3),
        );
        ow.set(
            "ArceusGround",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 4),
        );
        ow.set(
            "ArceusRock",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 5),
        );
        ow.set("ArceusBug", self.HasOrSeenPkmBasedOnObjectiveArceus(493, 6));
        ow.set(
            "ArceusGhost",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 7),
        );
        ow.set(
            "ArceusSteel",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 8),
        );
        ow.set(
            "ArceusFire",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 9),
        );
        ow.set(
            "ArceusWater",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 10),
        );
        ow.set(
            "ArceusGrass",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 11),
        );
        ow.set(
            "ArceusElectric",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 12),
        );
        ow.set(
            "ArceusPsychic",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 13),
        );
        ow.set(
            "ArceusIce",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 14),
        );
        ow.set(
            "ArceusDragon",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 15),
        );
        ow.set(
            "ArceusDark",
            self.HasOrSeenPkmBasedOnObjectiveArceus(493, 16),
        );
        ow.set(
            "BasculinBlueStripe",
            self.HasOrSeenPkmBasedOnObjective2(550, 0),
        );
        ow.set(
            "BasculinRedStripe",
            self.HasOrSeenPkmBasedOnObjective2(550, 1),
        );
        ow.set(
            "DarmanitanStandard",
            self.HasOrSeenPkmBasedOnObjective2(555, 0),
        );
        ow.set("DarmanitanZen", self.HasOrSeenPkmBasedOnObjective2(555, 1));
        ow.set("DeerlingSpring", self.HasOrSeenPkmBasedOnObjective2(585, 0));
        ow.set("DeerlingSummer", self.HasOrSeenPkmBasedOnObjective2(585, 1));
        ow.set("DeerlingAutumn", self.HasOrSeenPkmBasedOnObjective2(585, 2));
        ow.set("DeerlingWinter", self.HasOrSeenPkmBasedOnObjective2(585, 3));
        ow.set("SawsbuckSpring", self.HasOrSeenPkmBasedOnObjective2(586, 0));
        ow.set("SawsbuckSummer", self.HasOrSeenPkmBasedOnObjective2(586, 1));
        ow.set("SawsbuckAutumn", self.HasOrSeenPkmBasedOnObjective2(586, 2));
        ow.set("SawsbuckWinter", self.HasOrSeenPkmBasedOnObjective2(586, 3));
        ow.set(
            "TornadusIncarnate",
            self.HasOrSeenPkmBasedOnObjective2(641, 0),
        );
        ow.set(
            "TornadusTherian",
            self.HasOrSeenPkmBasedOnObjective2(641, 1),
        );
        ow.set(
            "ThundurusIncarnate",
            self.HasOrSeenPkmBasedOnObjective2(642, 0),
        );
        ow.set(
            "ThundurusTherian",
            self.HasOrSeenPkmBasedOnObjective2(642, 1),
        );
        ow.set(
            "LandorusIncarnate",
            self.HasOrSeenPkmBasedOnObjective2(645, 0),
        );
        ow.set(
            "LandorusTherian",
            self.HasOrSeenPkmBasedOnObjective2(645, 1),
        );
        ow.set("KyuremNormal", self.HasOrSeenPkmBasedOnObjective2(646, 0));
        ow.set("KyuremWhite", self.HasOrSeenPkmBasedOnObjective2(646, 1));
        ow.set("KyuremBlack", self.HasOrSeenPkmBasedOnObjective2(646, 2));
        ow.set("KeldeoOrdinary", self.HasOrSeenPkmBasedOnObjective2(647, 0));
        ow.set("KeldeoResolute", self.HasOrSeenPkmBasedOnObjective2(647, 1));
        ow.set("MeloettaAria", self.HasOrSeenPkmBasedOnObjective2(648, 0));
        ow.set(
            "MeloettaPirouette",
            self.HasOrSeenPkmBasedOnObjective2(648, 1),
        );
        ow.set("GenesectNormal", self.HasOrSeenPkmBasedOnObjective2(649, 0));
        ow.set("GenesectDouse", self.HasOrSeenPkmBasedOnObjective2(649, 1));
        ow.set("GenesectShock", self.HasOrSeenPkmBasedOnObjective2(649, 2));
        ow.set("GenesectBurn", self.HasOrSeenPkmBasedOnObjective2(649, 3));
        ow.set("GenesectChill", self.HasOrSeenPkmBasedOnObjective2(649, 4));
        self.CompletePokemonForms(&mut ow, result);
        ow.emit(result, "pokemonForm");
    }
    fn Generate_inGameTrade(&self, result: &mut CommandResult) {
        let mut ow = Checklist::default();
        ow.set("PetililforCottonee", self.sav.event(412));
        ow.set("EmolgaforGigalith", self.sav.event(256));
        ow.set("MantineforTangrowth", self.sav.event(405));
        ow.set("DittoforRotom", self.sav.event(259));
        ow.set("ExcadrillforAmbipom", self.sav.event(431));
        ow.set("HippowdonforAlakazam", self.sav.event(432));
        ow.set("AnyforMeowthYancy", self.HasTradeMon(52, 10303, "Yancy"));
        ow.set(
            "AnyforWobbuffetYancy",
            self.HasTradeMon(202, 10303, "Yancy"),
        );
        ow.set("AnyforRaltsYancy", self.HasTradeMon(280, 10303, "Yancy"));
        ow.set("AnyforShieldonYancy", self.HasTradeMon(410, 10303, "Yancy"));
        ow.set("AnyforRhyhornYancy", self.HasTradeMon(111, 10303, "Yancy"));
        ow.set("AnyforShellosYancy", self.HasTradeMon(422, 10303, "Yancy"));
        ow.set("AnyforMawileYancy", self.HasTradeMon(303, 10303, "Yancy"));
        ow.set(
            "AnyforSpiritombYancy",
            self.HasTradeMon(442, 10303, "Yancy"),
        );
        ow.set("AnyforSnorlaxYancy", self.HasTradeMon(143, 10303, "Yancy"));
        ow.set(
            "AnyforTeddiursaYancy",
            self.HasTradeMon(216, 10303, "Yancy"),
        );
        ow.set("AnyforSpindaYancy", self.HasTradeMon(327, 10303, "Yancy"));
        ow.set("AnyforTogepiYancy", self.HasTradeMon(175, 10303, "Yancy"));
        ow.set("AnyforMankeyCurtis", self.HasTradeMon(56, 54118, "Curtis"));
        ow.set(
            "AnyforWobbuffetCurtis",
            self.HasTradeMon(202, 54118, "Curtis"),
        );
        ow.set("AnyforRaltsCurtis", self.HasTradeMon(280, 54118, "Curtis"));
        ow.set(
            "AnyforCranidosCurtis",
            self.HasTradeMon(408, 54118, "Curtis"),
        );
        ow.set(
            "AnyforRhyhornCurtis",
            self.HasTradeMon(111, 54118, "Curtis"),
        );
        ow.set(
            "AnyforShellosCurtis",
            self.HasTradeMon(422, 54118, "Curtis"),
        );
        ow.set(
            "AnyforSableyeCurtis",
            self.HasTradeMon(302, 54118, "Curtis"),
        );
        ow.set(
            "AnyforSpiritombCurtis",
            self.HasTradeMon(442, 54118, "Curtis"),
        );
        ow.set(
            "AnyforSnorlaxCurtis",
            self.HasTradeMon(143, 54118, "Curtis"),
        );
        ow.set("AnyforPhanpyCurtis", self.HasTradeMon(231, 54118, "Curtis"));
        ow.set("AnyforSpindaCurtis", self.HasTradeMon(327, 54118, "Curtis"));
        ow.set("AnyforTogepiCurtis", self.HasTradeMon(175, 54118, "Curtis"));
        ow.emit(result, "inGameTrade");
    }
    fn Generate_inGameGift(&self, result: &mut CommandResult) {
        let mut ow = Checklist::default();
        ow.set("StarterPokemon", true);
        ow.set("Zorua", self.sav.event(0x0121));
        ow.set("Deerling", self.sav.event(0x01AA));
        ow.set("Eevee", self.sav.event(0x018A));
        ow.set("Magikarp", self.sav.event(0x00FA));
        ow.set("Happiny Egg", self.sav.event(0x01B5));
        ow.set("TirtougaorArchen", self.sav.event(0x01BD));
        ow.set("Shiny Gible", self.sav.event(0x0121));
        ow.emit(result, "inGameGift");
    }
    fn Generate_pwt(&self, result: &mut CommandResult) {
        let mut ow = Checklist::default();
        ow.set("DriftveilSingle", self.sav.pwt(18) >= 1);
        ow.set("DriftveilDouble", self.sav.pwt(18) >= 2);
        ow.set("DriftveilTriple", self.sav.pwt(18) >= 3);
        ow.set("DriftveilRotation", self.sav.pwt(18) >= 4);
        ow.set("RentalSingle", self.sav.pwt(25) >= 1);
        ow.set("RentalDouble", self.sav.pwt(25) >= 2);
        ow.set("RentalTriple", self.sav.pwt(25) >= 3);
        ow.set("RentalRotation", self.sav.pwt(25) >= 4);
        ow.set("MixSingle", self.sav.pwt(27) >= 1);
        ow.set("MixDouble", self.sav.pwt(27) >= 2);
        ow.set("MixTriple", self.sav.pwt(27) >= 3);
        ow.set("MixRotation", self.sav.pwt(27) >= 4);
        ow.set("UnovaLeadersSingle", self.sav.pwt(19) >= 1);
        ow.set("UnovaLeadersDouble", self.sav.pwt(19) >= 2);
        ow.set("UnovaLeadersTriple", self.sav.pwt(19) >= 3);
        ow.set("UnovaLeadersRotation", self.sav.pwt(19) >= 4);
        ow.set("KantoLeadersSingle", self.sav.pwt(20) >= 1);
        ow.set("KantoLeadersDouble", self.sav.pwt(20) >= 2);
        ow.set("KantoLeadersTriple", self.sav.pwt(20) >= 3);
        ow.set("KantoLeadersRotation", self.sav.pwt(20) >= 4);
        ow.set("JohtoLeadersSingle", self.sav.pwt(21) >= 1);
        ow.set("JohtoLeadersDouble", self.sav.pwt(21) >= 2);
        ow.set("JohtoLeadersTriple", self.sav.pwt(21) >= 3);
        ow.set("JohtoLeadersRotation", self.sav.pwt(21) >= 4);
        ow.set("HoennLeadersSingle", self.sav.pwt(22) >= 1);
        ow.set("HoennLeadersDouble", self.sav.pwt(22) >= 2);
        ow.set("HoennLeadersTriple", self.sav.pwt(22) >= 3);
        ow.set("HoennLeadersRotation", self.sav.pwt(22) >= 4);
        ow.set("SinnohLeadersSingle", self.sav.pwt(23) >= 1);
        ow.set("SinnohLeadersDouble", self.sav.pwt(23) >= 2);
        ow.set("SinnohLeadersTriple", self.sav.pwt(23) >= 3);
        ow.set("SinnohLeadersRotation", self.sav.pwt(23) >= 4);
        ow.set("WorldLeadersSingle", self.sav.pwt(24) >= 1);
        ow.set("WorldLeadersDouble", self.sav.pwt(24) >= 2);
        ow.set("WorldLeadersTriple", self.sav.pwt(24) >= 3);
        ow.set("WorldLeadersRotation", self.sav.pwt(24) >= 4);
        ow.set("ChampionsSingle", self.sav.pwt(17) >= 1);
        ow.set("ChampionsDouble", self.sav.pwt(17) >= 2);
        ow.set("ChampionsTriple", self.sav.pwt(17) >= 3);
        ow.set("ChampionsRotation", self.sav.pwt(17) >= 4);
        ow.set("RentalMastersSingle", self.sav.pwt(26) >= 1);
        ow.set("RentalMastersDouble", self.sav.pwt(26) >= 2);
        ow.set("RentalMastersTriple", self.sav.pwt(26) >= 3);
        ow.set("RentalMastersRotation", self.sav.pwt(26) >= 4);
        ow.set("MixMastersSingle", self.sav.pwt(28) >= 1);
        ow.set("MixMastersDouble", self.sav.pwt(28) >= 2);
        ow.set("MixMastersTriple", self.sav.pwt(28) >= 3);
        ow.set("MixMastersRotation", self.sav.pwt(28) >= 4);
        ow.set("TypeExpertNormalSingle", self.sav.pwt(0) >= 1);
        ow.set("TypeExpertNormalDouble", self.sav.pwt(0) >= 2);
        ow.set("TypeExpertNormalTriple", self.sav.pwt(0) >= 3);
        ow.set("TypeExpertNormalRotation", self.sav.pwt(0) >= 4);
        ow.set("TypeExpertFightingSingle", self.sav.pwt(1) >= 1);
        ow.set("TypeExpertFightingDouble", self.sav.pwt(1) >= 2);
        ow.set("TypeExpertFightingTriple", self.sav.pwt(1) >= 3);
        ow.set("TypeExpertFightingRotation", self.sav.pwt(1) >= 4);
        ow.set("TypeExpertFlyingSingle", self.sav.pwt(2) >= 1);
        ow.set("TypeExpertFlyingDouble", self.sav.pwt(2) >= 2);
        ow.set("TypeExpertFlyingTriple", self.sav.pwt(2) >= 3);
        ow.set("TypeExpertFlyingRotation", self.sav.pwt(2) >= 4);
        ow.set("TypeExpertPoisonSingle", self.sav.pwt(3) >= 1);
        ow.set("TypeExpertPoisonDouble", self.sav.pwt(3) >= 2);
        ow.set("TypeExpertPoisonTriple", self.sav.pwt(3) >= 3);
        ow.set("TypeExpertPoisonRotation", self.sav.pwt(3) >= 4);
        ow.set("TypeExpertGroundSingle", self.sav.pwt(4) >= 1);
        ow.set("TypeExpertGroundDouble", self.sav.pwt(4) >= 2);
        ow.set("TypeExpertGroundTriple", self.sav.pwt(4) >= 3);
        ow.set("TypeExpertGroundRotation", self.sav.pwt(4) >= 4);
        ow.set("TypeExpertRockSingle", self.sav.pwt(5) >= 1);
        ow.set("TypeExpertRockDouble", self.sav.pwt(5) >= 2);
        ow.set("TypeExpertRockTriple", self.sav.pwt(5) >= 3);
        ow.set("TypeExpertRockRotation", self.sav.pwt(5) >= 4);
        ow.set("TypeExpertBugSingle", self.sav.pwt(6) >= 1);
        ow.set("TypeExpertBugDouble", self.sav.pwt(6) >= 2);
        ow.set("TypeExpertBugTriple", self.sav.pwt(6) >= 3);
        ow.set("TypeExpertBugRotation", self.sav.pwt(6) >= 4);
        ow.set("TypeExpertGhostSingle", self.sav.pwt(7) >= 1);
        ow.set("TypeExpertGhostDouble", self.sav.pwt(7) >= 2);
        ow.set("TypeExpertGhostTriple", self.sav.pwt(7) >= 3);
        ow.set("TypeExpertGhostRotation", self.sav.pwt(7) >= 4);
        ow.set("TypeExpertSteelSingle", self.sav.pwt(8) >= 1);
        ow.set("TypeExpertSteelDouble", self.sav.pwt(8) >= 2);
        ow.set("TypeExpertSteelTriple", self.sav.pwt(8) >= 3);
        ow.set("TypeExpertSteelRotation", self.sav.pwt(8) >= 4);
        ow.set("TypeExpertFireSingle", self.sav.pwt(9) >= 1);
        ow.set("TypeExpertFireDouble", self.sav.pwt(9) >= 2);
        ow.set("TypeExpertFireTriple", self.sav.pwt(9) >= 3);
        ow.set("TypeExpertFireRotation", self.sav.pwt(9) >= 4);
        ow.set("TypeExpertWaterSingle", self.sav.pwt(10) >= 1);
        ow.set("TypeExpertWaterDouble", self.sav.pwt(10) >= 2);
        ow.set("TypeExpertWaterTriple", self.sav.pwt(10) >= 3);
        ow.set("TypeExpertWaterRotation", self.sav.pwt(10) >= 4);
        ow.set("TypeExpertGrassSingle", self.sav.pwt(11) >= 1);
        ow.set("TypeExpertGrassDouble", self.sav.pwt(11) >= 2);
        ow.set("TypeExpertGrassTriple", self.sav.pwt(11) >= 3);
        ow.set("TypeExpertGrassRotation", self.sav.pwt(11) >= 4);
        ow.set("TypeExpertElectricSingle", self.sav.pwt(12) >= 1);
        ow.set("TypeExpertElectricDouble", self.sav.pwt(12) >= 2);
        ow.set("TypeExpertElectricTriple", self.sav.pwt(12) >= 3);
        ow.set("TypeExpertElectricRotation", self.sav.pwt(12) >= 4);
        ow.set("TypeExpertPsychicSingle", self.sav.pwt(13) >= 1);
        ow.set("TypeExpertPsychicDouble", self.sav.pwt(13) >= 2);
        ow.set("TypeExpertPsychicTriple", self.sav.pwt(13) >= 3);
        ow.set("TypeExpertPsychicRotation", self.sav.pwt(13) >= 4);
        ow.set("TypeExpertIceSingle", self.sav.pwt(14) >= 1);
        ow.set("TypeExpertIceDouble", self.sav.pwt(14) >= 2);
        ow.set("TypeExpertIceTriple", self.sav.pwt(14) >= 3);
        ow.set("TypeExpertIceRotation", self.sav.pwt(14) >= 4);
        ow.set("TypeExpertDragonSingle", self.sav.pwt(15) >= 1);
        ow.set("TypeExpertDragonDouble", self.sav.pwt(15) >= 2);
        ow.set("TypeExpertDragonTriple", self.sav.pwt(15) >= 3);
        ow.set("TypeExpertDragonRotation", self.sav.pwt(15) >= 4);
        ow.set("TypeExpertDarkSingle", self.sav.pwt(16) >= 1);
        ow.set("TypeExpertDarkDouble", self.sav.pwt(16) >= 2);
        ow.set("TypeExpertDarkTriple", self.sav.pwt(16) >= 3);
        ow.set("TypeExpertDarkRotation", self.sav.pwt(16) >= 4);
        ow.emit(result, "pwt");
    }
    fn Generate_ribbon(&self, result: &mut CommandResult) {
        let mut ow = Checklist::default();
        ow.set("ChampionRibbon", self.HasRibbon(0x3E, 4));
        ow.set("SinnohChampionRibbon", self.HasRibbon(0x24, 0));
        ow.set("CoolRibbonHoenn", self.HasRibbon(0x3C, 0));
        ow.set("CoolRibbonSuperHoenn", self.HasRibbon(0x3C, 1));
        ow.set("CoolRibbonHyperHoenn", self.HasRibbon(0x3C, 2));
        ow.set("CoolRibbonMasterHoenn", self.HasRibbon(0x3C, 3));
        ow.set("BeautyRibbonHoenn", self.HasRibbon(0x3C, 4));
        ow.set("BeautyRibbonSuperHoenn", self.HasRibbon(0x3C, 5));
        ow.set("BeautyRibbonHyperHoenn", self.HasRibbon(0x3C, 6));
        ow.set("BeautyRibbonMasterHoenn", self.HasRibbon(0x3C, 7));
        ow.set("CuteRibbonHoenn", self.HasRibbon(0x3D, 0));
        ow.set("CuteRibbonSuperHoenn", self.HasRibbon(0x3D, 1));
        ow.set("CuteRibbonHyperHoenn", self.HasRibbon(0x3D, 2));
        ow.set("CuteRibbonMasterHoenn", self.HasRibbon(0x3D, 3));
        ow.set("SmartRibbonHoenn", self.HasRibbon(0x3D, 4));
        ow.set("SmartRibbonSuperHoenn", self.HasRibbon(0x3D, 5));
        ow.set("SmartRibbonHyperHoenn", self.HasRibbon(0x3D, 6));
        ow.set("SmartRibbonMasterHoenn", self.HasRibbon(0x3D, 7));
        ow.set("ToughRibbonHoenn", self.HasRibbon(0x3E, 0));
        ow.set("ToughRibbonSuperHoenn", self.HasRibbon(0x3E, 1));
        ow.set("ToughRibbonHyperHoenn", self.HasRibbon(0x3E, 2));
        ow.set("ToughRibbonMasterHoenn", self.HasRibbon(0x3E, 3));
        ow.set("CoolRibbonSinnoh", self.HasRibbon(0x60, 0));
        ow.set("CoolRibbonGreatSinnoh", self.HasRibbon(0x60, 1));
        ow.set("CoolRibbonUltraSinnoh", self.HasRibbon(0x60, 2));
        ow.set("CoolRibbonMasterSinnoh", self.HasRibbon(0x60, 3));
        ow.set("BeautyRibbonSinnoh", self.HasRibbon(0x60, 4));
        ow.set("BeautyRibbonGreatSinnoh", self.HasRibbon(0x60, 5));
        ow.set("BeautyRibbonUltraSinnoh", self.HasRibbon(0x60, 6));
        ow.set("BeautyRibbonMasterSinnoh", self.HasRibbon(0x60, 7));
        ow.set("CuteRibbonSinnoh", self.HasRibbon(0x61, 0));
        ow.set("CuteRibbonGreatSinnoh", self.HasRibbon(0x61, 1));
        ow.set("CuteRibbonUltraSinnoh", self.HasRibbon(0x61, 2));
        ow.set("CuteRibbonMasterSinnoh", self.HasRibbon(0x61, 3));
        ow.set("SmartRibbonSinnoh", self.HasRibbon(0x61, 4));
        ow.set("SmartRibbonGreatSinnoh", self.HasRibbon(0x61, 5));
        ow.set("SmartRibbonUltraSinnoh", self.HasRibbon(0x61, 6));
        ow.set("SmartRibbonMasterSinnoh", self.HasRibbon(0x61, 7));
        ow.set("ToughRibbonSinnoh", self.HasRibbon(0x62, 0));
        ow.set("ToughRibbonGreatSinnoh", self.HasRibbon(0x62, 1));
        ow.set("ToughRibbonUltraSinnoh", self.HasRibbon(0x62, 2));
        ow.set("ToughRibbonMasterSinnoh", self.HasRibbon(0x62, 3));
        ow.set("WinningRibbon", self.HasRibbon(0x3E, 5));
        ow.set("VictoryRibbon", self.HasRibbon(0x3E, 6));
        ow.set("AbilityRibbon", self.HasRibbon(0x24, 1));
        ow.set("GreatAbilityRibbon", self.HasRibbon(0x24, 2));
        ow.set("DoubleAbilityRibbon", self.HasRibbon(0x24, 3));
        ow.set("MultiAbilityRibbon", self.HasRibbon(0x24, 4));
        ow.set("PairAbilityRibbon", self.HasRibbon(0x24, 5));
        ow.set("WorldAbilityRibbon", self.HasRibbon(0x24, 6));
        ow.set("ArtistRibbon", self.HasRibbon(0x3E, 7));
        ow.set("EffortRibbon", self.HasRibbon(0x3F, 0));
        ow.set("AlertRibbon", self.HasRibbon(0x24, 7));
        ow.set("ShockRibbon", self.HasRibbon(0x25, 0));
        ow.set("DowncastRibbon", self.HasRibbon(0x25, 1));
        ow.set("CarelessRibbon", self.HasRibbon(0x25, 2));
        ow.set("RelaxRibbon", self.HasRibbon(0x25, 3));
        ow.set("SnoozeRibbon", self.HasRibbon(0x25, 4));
        ow.set("SmileRibbon", self.HasRibbon(0x25, 5));
        ow.set("GorgeousRibbon", self.HasRibbon(0x25, 6));
        ow.set("RoyalRibbon", self.HasRibbon(0x25, 7));
        ow.set("GorgeousRoyalRibbon", self.HasRibbon(0x26, 0));
        ow.set("FootprintRibbon", self.HasRibbon(0x26, 1));
        ow.set("RecordRibbon", self.HasRibbon(0x26, 2));
        ow.set("CountryRibbon", self.HasRibbon(0x3F, 4));
        ow.set("NationalRibbon", self.HasRibbon(0x3F, 5));
        ow.set("EarthRibbon", self.HasRibbon(0x3F, 6));
        ow.set("WorldRibbon", self.HasRibbon(0x3F, 7));
        ow.set("ClassicRibbon", self.HasRibbon(0x27, 2));
        ow.set("LegendRibbon", self.HasRibbon(0x26, 4));
        ow.set("PremierRibbon", self.HasRibbon(0x27, 3));
        ow.emit(result, "ribbon");
    }
    fn Generate_battleSubway(&self, result: &mut CommandResult) {
        let mut ow = Checklist::default();
        ow.set(
            "Single21wins",
            u16_le(&self.sav.data, 0x21b00 + 0x1a).unwrap() >= 21,
        );
        ow.set(
            "Double21wins",
            u16_le(&self.sav.data, 0x21b00 + 0x1c).unwrap() >= 21,
        );
        ow.set(
            "Multi21wins",
            u16_le(&self.sav.data, 0x21b00 + 0x1e).unwrap() >= 21,
        );
        ow.set(
            "SuperSingle49wins",
            u16_le(&self.sav.data, 0x21b00 + 0x24).unwrap() >= 49,
        );
        ow.set(
            "SuperDouble49wins",
            u16_le(&self.sav.data, 0x21b00 + 0x26).unwrap() >= 49,
        );
        ow.set(
            "SuperMulti49wins",
            u16_le(&self.sav.data, 0x21b00 + 0x28).unwrap() >= 49,
        );
        ow.set("WiFi", u16_le(&self.sav.data, 0x21b00 + 0x2a).unwrap() >= 1);
        ow.emit(result, "battleSubway");
    }
    fn Generate_joinAvenue(&self, result: &mut CommandResult) {
        let mut ow = Checklist::default();
        ow.set(
            "AvenueRank100",
            u16_le(&self.sav.data, 0x24fcc).unwrap() == 100,
        );
        ow.set("RaffleShopRank10", self.has_rank_10_join_avenue_shop(0));
        ow.set("BeautySalonRank10", self.has_rank_10_join_avenue_shop(1));
        ow.set("DojoRank10", self.has_rank_10_join_avenue_shop(4));
        ow.set("MarketRank10", self.has_rank_10_join_avenue_shop(2));
        ow.set("FlowerShopRank10", self.has_rank_10_join_avenue_shop(3));
        ow.set("AntiqueShopRank10", self.has_rank_10_join_avenue_shop(6));
        ow.set("CaféRank10", self.has_rank_10_join_avenue_shop(7));
        ow.set("NurseryRank10", self.has_rank_10_join_avenue_shop(5));
        ow.emit(result, "joinAvenue");
    }
}
const UNOBTAINABLE: &[u16] = &[
    5, 16, 113, 114, 115, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133,
    260, 261, 262, 263, 264, 426, 427, 428, 429, 430, 431, 432, 433, 434, 435, 436, 438, 439, 440,
    441, 443, 444, 445, 446, 448, 449, 451, 452, 454, 455, 456, 457, 459, 460, 461, 462, 463, 464,
    467, 468, 469, 470, 472, 473, 474, 475, 476, 477, 478, 479, 480, 481, 482, 483, 484, 485, 486,
    487, 488, 489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499, 500, 501, 502, 503, 505, 506,
    507, 508, 509, 510, 511, 512, 513, 514, 515, 516, 517, 518, 519, 520, 521, 522, 523, 524, 525,
    526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 574, 579, 592, 593, 594, 595, 596, 597,
    598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 608, 609, 610, 611, 612, 613, 614, 615, 616,
    622, 623, 624, 625,
];
const NO_TRADE: &[u16] = &[
    16, 17, 18, 19, 20, 21, 22, 25, 26, 29, 30, 31, 32, 33, 34, 35, 36, 39, 40, 41, 42, 43, 44, 45,
    46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 60, 61, 62, 63, 64, 66, 67, 69, 70, 71, 72, 73,
    74, 75, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 95, 96, 97, 98, 99,
    100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118,
    119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137,
    138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 161, 162, 163, 164, 165, 166, 167,
    168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 187,
    188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 201, 202, 203, 206, 207, 208, 209, 210, 211,
    214, 215, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 231, 232, 234, 235, 236,
    237, 238, 239, 240, 241, 242, 246, 247, 248, 261, 262, 263, 264, 265, 266, 267, 268, 269, 276,
    277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295,
    296, 297, 298, 299, 300, 301, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316,
    317, 318, 319, 320, 321, 322, 323, 324, 325, 326, 327, 331, 332, 333, 334, 339, 340, 341, 342,
    343, 344, 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361,
    362, 363, 364, 365, 369, 370, 371, 372, 373, 374, 375, 376, 387, 388, 389, 390, 391, 392, 393,
    394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412,
    413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 433, 436, 437,
    438, 439, 440, 441, 443, 444, 445, 446, 447, 448, 449, 450, 451, 452, 453, 454, 455, 456, 457,
    458, 459, 460, 461, 462, 463, 465, 468, 469, 470, 471, 472, 473, 475, 476, 478, 479, 480, 481,
    482, 483, 484, 485, 487, 488,
];
const SPECIAL_FORMS: &[(&str, u16, u8)] = &[
    ("UnownA", 201, 0),
    ("UnownB", 201, 1),
    ("UnownC", 201, 2),
    ("UnownD", 201, 3),
    ("UnownE", 201, 4),
    ("UnownF", 201, 5),
    ("UnownG", 201, 6),
    ("UnownH", 201, 7),
    ("UnownI", 201, 8),
    ("UnownJ", 201, 9),
    ("UnownK", 201, 10),
    ("UnownL", 201, 11),
    ("UnownM", 201, 12),
    ("UnownN", 201, 13),
    ("UnownO", 201, 14),
    ("UnownP", 201, 15),
    ("UnownQ", 201, 16),
    ("UnownR", 201, 17),
    ("UnownS", 201, 18),
    ("UnownT", 201, 19),
    ("UnownU", 201, 20),
    ("UnownV", 201, 21),
    ("UnownW", 201, 22),
    ("UnownX", 201, 23),
    ("UnownY", 201, 24),
    ("UnownZ", 201, 25),
    ("UnownExclamationMark", 201, 26),
    ("UnownQuestionMark", 201, 27),
    ("CastformNormal", 351, 0),
    ("CastformFire", 351, 1),
    ("CastformWater", 351, 2),
    ("CastformIce", 351, 3),
    ("DeoxysNormal", 386, 0),
    ("DeoxysAttack", 386, 1),
    ("DeoxysDefense", 386, 2),
    ("DeoxysSpeed", 386, 3),
    ("BurmyPlantCloak", 412, 0),
    ("BurmySandyCloak", 412, 1),
    ("BurmyTrashCloak", 412, 2),
    ("WormadamPlantCloak", 413, 0),
    ("WormadamSandyCloak", 413, 1),
    ("WormadamTrashCloak", 413, 2),
    ("CherrimOvercast", 421, 0),
    ("CherrimSunshine", 421, 1),
    ("ShellosWestSea", 422, 0),
    ("ShellosEastSea", 422, 1),
    ("GastrodonWestSea", 423, 0),
    ("GastrodonEastSea", 423, 1),
    ("RotomGhost", 479, 0),
    ("RotomHeat", 479, 1),
    ("RotomWash", 479, 2),
    ("RotomFrost", 479, 3),
    ("RotomFan", 479, 4),
    ("RotomMow", 479, 5),
    ("GiratinaAltered", 487, 0),
    ("GiratinaOrigin", 487, 1),
    ("ShayminLand", 492, 0),
    ("ShayminSky", 492, 1),
    ("BasculinBlueStripe", 550, 0),
    ("BasculinRedStripe", 550, 1),
    ("DarmanitanStandard", 555, 0),
    ("DarmanitanZen", 555, 1),
    ("DeerlingSpring", 585, 0),
    ("DeerlingSummer", 585, 1),
    ("DeerlingAutumn", 585, 2),
    ("DeerlingWinter", 585, 3),
    ("SawsbuckSpring", 586, 0),
    ("SawsbuckSummer", 586, 1),
    ("SawsbuckAutumn", 586, 2),
    ("SawsbuckWinter", 586, 3),
    ("TornadusIncarnate", 641, 0),
    ("TornadusTherian", 641, 1),
    ("ThundurusIncarnate", 642, 0),
    ("ThundurusTherian", 642, 1),
    ("LandorusIncarnate", 645, 0),
    ("LandorusTherian", 645, 1),
    ("KyuremNormal", 646, 0),
    ("KyuremWhite", 646, 1),
    ("KyuremBlack", 646, 2),
    ("KeldeoOrdinary", 647, 0),
    ("KeldeoResolute", 647, 1),
    ("MeloettaAria", 648, 0),
    ("MeloettaPirouette", 648, 1),
    ("GenesectNormal", 649, 0),
    ("GenesectDouse", 649, 1),
    ("GenesectShock", 649, 2),
    ("GenesectBurn", 649, 3),
    ("GenesectChill", 649, 4),
];
