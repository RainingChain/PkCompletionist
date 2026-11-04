#![allow(non_snake_case, non_camel_case_types)]
use crate::bit_block::bits;
use crate::{category, ordered_category, sky_save, CommandResult};
use pkhex_core::binary::InvalidData;
struct CompletionValidator4_PmdSky {
    ownedMonList: Vec<u32>,
    ownedItemList: Vec<u32>,
    points: i32,
}
pub fn validate(data: &[u8], result: &mut CommandResult) -> Result<(), InvalidData> {
    CompletionValidator4_PmdSky {
        ownedMonList: sky_save::GetStoredPokemon(data, 0)?,
        ownedItemList: sky_save::GetOwnedItems(data, 0)?,
        points: bits(data, 0x9958 * 8, 32)? as i32,
    }
    .GenerateAll(result);
    Ok(())
}
impl CompletionValidator4_PmdSky {
    fn HasMon(&self, id: u32) -> bool {
        self.ownedMonList.contains(&id)
    }
    fn HasItem(&self, id: u32) -> bool {
        self.ownedItemList.contains(&id)
    }
    fn GotShaymin(&self) -> bool {
        self.HasMon(534) || self.HasMon(535)
    }
    fn HasFinishedPostCreditStory(&self) -> bool {
        self.HasMon(532)
    }
    fn GenerateAll(&self, result: &mut CommandResult) {
        self.Generate_pokemon(result);
        self.Generate_pokemonForm(result);
        self.Generate_item(result);
        self.Generate_marowakDojo(result);
        self.Generate_rank(result);
        self.Generate_dungeon(result);
        self.Generate_adventureLog(result);
        self.Generate_specialEpisode(result);
        self.Generate_progressIcon(result);
    }
    fn Generate_pokemon(&self, result: &mut CommandResult) {
        category(
            result,
            "pokemon",
            (1..=534)
                .filter(|id| !FORMS.iter().any(|forms| forms[1..].contains(id)))
                .map(|id| {
                    (
                        id.to_string(),
                        FORMS.iter().find(|forms| forms[0] == id).map_or_else(
                            || self.HasMon(id),
                            |forms| forms.iter().any(|form| self.HasMon(*form)),
                        ),
                    )
                }),
        );
    }
    fn Generate_pokemonForm(&self, result: &mut CommandResult) {
        category(
            result,
            "pokemonForm",
            FORMS
                .iter()
                .flat_map(|forms| forms.iter())
                .filter(|id| ![380, 381, 382, 461, 536, 535].contains(id))
                .map(|id| (id.to_string(), self.HasMon(*id))),
        );
    }
    fn Generate_item(&self, result: &mut CommandResult) {
        const BOXES: &[u32] = &[364, 367, 370, 373, 376, 379, 382, 385, 388, 391, 394, 397];
        category(
            result,
            "item",
            (1..=1399)
                .filter(|id| {
                    !UNOBTAINABLE.contains(id)
                        && !BOXES.iter().any(|b| *id == b + 1 || *id == b + 2)
                })
                .map(|id| {
                    (
                        id.to_string(),
                        self.HasItem(id)
                            || BOXES.contains(&id)
                                && (self.HasItem(id + 1) || self.HasItem(id + 2)),
                    )
                }),
        );
    }
    fn Generate_marowakDojo(&self, result: &mut CommandResult) {
        category(result, "MarowakDojo", []);
    }
}
const FORMS: &[&[u32]] = &[
    &[
        201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218,
        219, 220, 221, 222, 223, 224, 225, 226, 227, 228,
    ],
    &[379, 380, 381, 382],
    &[418, 419, 420, 421],
    &[447, 448, 449],
    &[450, 451, 452],
    &[460, 461],
    &[462, 463],
    &[464, 465],
    &[529, 536],
    &[534, 535],
];
// Ported rule expressions; regenerate with rust/tools/port-pmd-rules.ps1.
impl CompletionValidator4_PmdSky {
    #[allow(clippy::vec_init_then_push)]
    fn Generate_rank(&self, result: &mut CommandResult) {
        let item = |id| self.HasItem(id);
        let points = self.points;
        let mut entries = Vec::new();
        entries.push(("NormalRank", true));
        entries.push(("BronzeRank", points >= 100));
        entries.push(("SilverRank", points >= 400));
        entries.push(("GoldRank", points >= 1600));
        entries.push(("DiamondRank", points >= 3200));
        entries.push(("SuperRank", points >= 5000));
        entries.push(("UltraRank", points >= 7500));
        entries.push(("HyperRank", points >= 10500));
        entries.push(("MasterRank", points >= 13500));
        entries.push(("MasterRank1Star", points >= 17000));
        entries.push(("MasterRank2Stars", points >= 21000));
        entries.push(("MasterRank3Stars", points >= 25000));
        entries.push(("GuildmasterRank", points >= 100000));
        entries.push(("SecretRank", (59..=65).any(item)));
        ordered_category(result, "rank", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_dungeon(&self, result: &mut CommandResult) {
        let has = |id| self.HasMon(id);
        let item = |id| self.HasItem(id);
        let mut entries = Vec::new();
        entries.push(("BeachCave", self.HasFinishedPostCreditStory()));
        entries.push(("DrenchedBluff", self.HasFinishedPostCreditStory()));
        entries.push(("MtBristle", self.HasFinishedPostCreditStory()));
        entries.push(("WaterfallCave", self.HasFinishedPostCreditStory()));
        entries.push(("AppleWoods", self.HasFinishedPostCreditStory()));
        entries.push(("SidePath", self.HasFinishedPostCreditStory()));
        entries.push(("CraggyCoast", self.HasFinishedPostCreditStory()));
        entries.push(("RockPath", self.HasFinishedPostCreditStory()));
        entries.push(("MtHorn", self.HasFinishedPostCreditStory()));
        entries.push(("ForestPath", self.HasFinishedPostCreditStory()));
        entries.push(("FoggyForest", self.HasFinishedPostCreditStory()));
        entries.push(("SteamCave", self.HasFinishedPostCreditStory()));
        entries.push(("AmpPlains", self.HasFinishedPostCreditStory()));
        entries.push(("NorthernDesert", self.HasFinishedPostCreditStory()));
        entries.push(("QuicksandCave", self.HasFinishedPostCreditStory()));
        entries.push(("CrystalCave", self.HasFinishedPostCreditStory()));
        entries.push(("CrystalCrossing", self.HasFinishedPostCreditStory()));
        entries.push(("ChasmCave", self.HasFinishedPostCreditStory()));
        entries.push(("DarkHill", self.HasFinishedPostCreditStory()));
        entries.push(("SealedRuin", self.HasFinishedPostCreditStory()));
        entries.push(("DuskForest", self.HasFinishedPostCreditStory()));
        entries.push(("DeepDuskForest", self.HasFinishedPostCreditStory()));
        entries.push(("TreeshroudForest", self.HasFinishedPostCreditStory()));
        entries.push(("BrineCave", self.HasFinishedPostCreditStory()));
        entries.push(("HiddenLand", self.HasFinishedPostCreditStory()));
        entries.push(("TemporalTower", self.HasFinishedPostCreditStory()));
        entries.push(("MystifyingForest", self.HasFinishedPostCreditStory()));
        entries.push(("BlizzardIsland", self.HasFinishedPostCreditStory()));
        entries.push(("CreviceCave", self.HasFinishedPostCreditStory()));
        entries.push(("SurroundedSea", self.HasFinishedPostCreditStory()));
        entries.push(("MiracleSea", self.HasFinishedPostCreditStory()));
        entries.push(("AegisCave", self.HasFinishedPostCreditStory()));
        entries.push(("MtTravail", self.HasFinishedPostCreditStory()));
        entries.push(("TheNightmare", self.HasFinishedPostCreditStory()));
        entries.push(("SpacialRift", self.HasFinishedPostCreditStory()));
        entries.push(("DarkCrater", self.HasFinishedPostCreditStory()));
        entries.push(("ConcealedRuins", has(385)));
        entries.push(("MarineResort", has(392)));
        entries.push(("BottomlessSea", item(62)));
        entries.push(("ShimmerDesert", item(61)));
        entries.push(("MtAvalanche", item(59)));
        entries.push(("GiantVolcano", item(60)));
        entries.push(("WorldAbyss", item(63)));
        entries.push(("SkyStairway", item(65)));
        entries.push(("MysteryJungle", item(64)));
        entries.push(("SerenityRiver", has(312)));
        entries.push(("LandslideCave", has(485)));
        entries.push(("LushPrairie", has(448)));
        entries.push(("OranForest", has(449)));
        entries.push(("LakeAfar", has(7)));
        entries.push(("HappyOutlook", has(175)));
        entries.push(("MtMistral", has(277)));
        entries.push(("ShimmerHill", has(418) || has(419) || has(420) || has(421)));
        entries.push(("LostWilderness", has(324)));
        entries.push(("MidnightForest", has(521)));
        entries.push(("DestinyTower", item(43)));
        if self.points < 17000 {
            entries.push(("OblivionForest", false));
        }
        entries.push(("TreacherousWaters", has(272)));
        entries.push(("SoutheasternIslands", has(270)));
        entries.push(("InfernoCave", has(271)));
        entries.push(("StarCave", has(417)));
        entries.push(("SkyPeakMountainPath", self.HasFinishedPostCreditStory()));
        ordered_category(result, "dungeon", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_adventureLog(&self, result: &mut CommandResult) {
        let has = |id| self.HasMon(id);
        let item = |id| self.HasItem(id);
        let mut entries = Vec::new();
        entries.push(("PokemonthatjoinedyouX", true));
        entries.push(("KindsofPokemonbattledX", true));
        entries.push(("MoveslearnedX", true));
        entries.push(("KindsofitemsacquiredX", true));
        entries.push(("DungeonsclearedX", true));
        entries.push(("RecordofonefloorvictoriesX", true));
        entries.push((
            "RecycledX",
            has(485) || has(449) || has(7) || item(169) || item(170) || item(171) || item(172),
        ));
        entries.push(("DiscoveredtheHotSpring", self.HasFinishedPostCreditStory()));
        entries.push((
            "DiscoveredtheFogboundLake",
            self.HasFinishedPostCreditStory(),
        ));
        entries.push(("Returnedfromthefuture", self.HasFinishedPostCreditStory()));
        entries.push((
            "Preventedtheplanetsparalysis",
            self.HasFinishedPostCreditStory(),
        ));
        entries.push((
            "GraduatedfromWigglytuffsGuild",
            self.HasFinishedPostCreditStory(),
        ));
        entries.push((
            "ClimbedtotheSkyPeakSummit",
            self.HasFinishedPostCreditStory(),
        ));
        entries.push((
            "DiscoveredthesecretofAegisCave",
            self.HasFinishedPostCreditStory(),
        ));
        entries.push((
            "FoiledDarkraisdastardlyplot",
            self.HasFinishedPostCreditStory(),
        ));
        entries.push(("ReceivedtheIcyFlute", item(59)));
        entries.push(("ReceivedtheFieryDrum", item(60)));
        entries.push(("ReceivedtheTerraCymbal", item(61)));
        entries.push(("ReceivedtheAquaMonica", item(62)));
        entries.push(("ReceivedtheRockHorn", item(63)));
        entries.push(("ReceivedtheGrassCornet", item(64)));
        entries.push(("ReceivedtheSkyMelodica", item(65)));
        entries.push((
            "AcceptedallSpecialChallenges",
            has(417) && has(271) && has(270) && has(272) && has(150),
        ));
        entries.push(("Articunojoinedtheteam", has(144)));
        entries.push(("Zapdosjoinedtheteam", has(145)));
        entries.push(("Moltresjoinedtheteam", has(146)));
        entries.push(("Mewtwojoinedtheteam", has(150)));
        entries.push(("Mewjoinedtheteam", has(151)));
        entries.push(("Unownjoinedtheteam", (201..=228).any(has)));
        entries.push(("Raikoujoinedtheteam", has(270)));
        entries.push(("Enteijoinedtheteam", has(271)));
        entries.push(("Suicunejoinedtheteam", has(272)));
        entries.push(("Lugiajoinedtheteam", has(276)));
        entries.push(("HoOhjoinedtheteam", has(277)));
        entries.push(("Celebijoinedtheteam", has(278)));
        entries.push(("Regirockjoinedtheteam", has(409)));
        entries.push(("Regicejoinedtheteam", has(410)));
        entries.push(("Registeeljoinedtheteam", has(411)));
        entries.push(("Latiasjoinedtheteam", has(412)));
        entries.push(("Latiosjoinedtheteam", has(413)));
        entries.push(("Kyogrejoinedtheteam", has(414)));
        entries.push(("Groudonjoinedtheteam", has(415)));
        entries.push(("Rayquazajoinedtheteam", has(416)));
        entries.push(("Jirachijoinedtheteam", has(417)));
        entries.push((
            "Deoxysjoinedtheteam",
            has(418) || has(419) || has(420) || has(421),
        ));
        entries.push(("Rotomjoinedtheteam", has(521)));
        entries.push(("Uxiejoinedtheteam", has(522)));
        entries.push(("Mespritjoinedtheteam", has(523)));
        entries.push(("Azelfjoinedtheteam", has(524)));
        entries.push(("Dialgajoinedtheteam", has(525)));
        entries.push(("Palkiajoinedtheteam", has(526)));
        entries.push(("Heatranjoinedtheteam", has(527)));
        entries.push(("Regigigasjoinedtheteam", has(528)));
        entries.push(("Giratinajoinedtheteam", has(529) || has(536)));
        entries.push(("Cresseliajoinedtheteam", has(530)));
        entries.push(("Phionejoinedtheteam", has(531)));
        entries.push(("Manaphyjoinedtheteam", has(532)));
        entries.push(("Darkraijoinedtheteam", has(533)));
        entries.push(("Shayminjoinedtheteam", self.GotShaymin()));
        ordered_category(result, "adventureLog", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_specialEpisode(&self, result: &mut CommandResult) {
        let entries = Vec::new();
        ordered_category(result, "specialEpisode", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_progressIcon(&self, result: &mut CommandResult) {
        let item = |id| self.HasItem(id);
        let mut entries = Vec::new();
        entries.push(("Drowzee", self.HasFinishedPostCreditStory()));
        entries.push(("Groudon", self.HasFinishedPostCreditStory()));
        entries.push(("Manectric", self.HasFinishedPostCreditStory()));
        entries.push(("Celebi", self.HasFinishedPostCreditStory()));
        entries.push(("Dusknoir", self.HasFinishedPostCreditStory()));
        entries.push(("Palkia", self.HasFinishedPostCreditStory()));
        entries.push(("Wigglytuff", self.HasFinishedPostCreditStory()));
        entries.push(("Shaymin", self.HasFinishedPostCreditStory()));
        entries.push(("Scizor", self.HasFinishedPostCreditStory()));
        entries.push(("Phione", self.HasFinishedPostCreditStory()));
        entries.push(("Regigigas", self.HasFinishedPostCreditStory()));
        entries.push(("Darkrai", self.HasFinishedPostCreditStory()));
        entries.push(("GoldenChest", (59..=65).all(item)));
        ordered_category(result, "progressIcon", &entries);
    }
}
const UNOBTAINABLE: &[u32] = &[
    11, 12, 98, 113, 114, 138, 166, 175, 176, 177, 181, 184, 185, 194, 198, 205, 219, 224, 226,
    236, 258, 259, 293, 294, 295, 296, 297, 298, 299, 300, 324, 339, 345, 349, 353, 360, 361, 363,
    769, 825, 1352, 1353, 1354, 1355, 1356, 1357, 1358, 1359, 1360, 1361, 1362, 1363, 1364, 1365,
    1366, 1367, 1368, 1369, 1370, 1371, 1372, 1373, 1374, 1375, 1376, 1377, 1378, 1379, 1380, 1381,
    1382, 1383, 1384, 1385, 1386, 1387, 1388, 1389, 1390, 1391, 1392, 1393, 1394, 1395, 1396, 1397,
    1398, 1399,
];
