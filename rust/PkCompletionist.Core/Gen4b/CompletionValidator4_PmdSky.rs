use crate::{category, ordered_category, CommandResult};
use pkhex_core::binary::{bits, InvalidData};
const FORMS: &[&[u32]] = &[
    &[201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224,225,226,227,228],
    &[379,380,381,382], &[418,419,420,421], &[447,448,449], &[450,451,452], &[460,461], &[462,463], &[464,465], &[529,536], &[534,535],
];

pub fn validate(data: &[u8], result: &mut CommandResult) -> Result<(), InvalidData> {
    let mut owned = Vec::new();
    for index in 0..720 {
        let id = bits(data, 0x464 * 8 + index * 362 + 8, 11)?;
        owned.push(if id >= 600 { id - 600 } else { id });
    }
    let mut items = Vec::new();
    for index in 0..1000 {
        let id = bits(data, 0x8e0c * 8 + 6 + index * 11, 11)?;
        if id == 0 { break; }
        items.push(id);
    }
    for index in 0..50 {
        let offset = 0x8ba2 * 8 + index * 33;
        if bits(data, offset, 1)? == 0 { break; }
        let id = bits(data, offset + 19, 11)?;
        if id > 0 { items.push(id); }
    }
    let has = |id| owned.contains(&id);
    let item = |id| items.contains(&id);
    let points = bits(data, 0x9958 * 8, 32)? as i32;
    category(result, "pokemon", (1..=534).filter(|id| !FORMS.iter().any(|forms| forms[1..].contains(id))).map(|id| (id.to_string(), FORMS.iter().find(|forms| forms[0] == id).map_or_else(|| has(id), |forms| forms.iter().any(|form| has(*form))))));
    category(result, "pokemonForm", FORMS.iter().flat_map(|forms| forms.iter()).filter(|id| ![380,381,382,461,536,535].contains(id)).map(|id| (id.to_string(), has(*id))));
    const BOXES: &[u32] = &[364,367,370,373,376,379,382,385,388,391,394,397];
    category(result, "item", (1..=1399).filter(|id| !UNOBTAINABLE.contains(id) && !BOXES.iter().any(|b| *id == b + 1 || *id == b + 2)).map(|id| (id.to_string(), item(id) || BOXES.contains(&id) && (item(id + 1) || item(id + 2)))));
    category(result, "MarowakDojo", []);
    populate(result, has, item, points);
    Ok(())
}
// Ported rule expressions; regenerate with rust/tools/port-pmd-rules.ps1.
#[allow(clippy::vec_init_then_push, clippy::too_many_arguments)]
fn populate(result: &mut CommandResult, has: impl Fn(u32) -> bool + Copy, item: impl Fn(u32) -> bool + Copy, points: i32) {
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
    let mut entries = Vec::new();
    entries.push(("BeachCave", has(532)));
    entries.push(("DrenchedBluff", has(532)));
    entries.push(("MtBristle", has(532)));
    entries.push(("WaterfallCave", has(532)));
    entries.push(("AppleWoods", has(532)));
    entries.push(("SidePath", has(532)));
    entries.push(("CraggyCoast", has(532)));
    entries.push(("RockPath", has(532)));
    entries.push(("MtHorn", has(532)));
    entries.push(("ForestPath", has(532)));
    entries.push(("FoggyForest", has(532)));
    entries.push(("SteamCave", has(532)));
    entries.push(("AmpPlains", has(532)));
    entries.push(("NorthernDesert", has(532)));
    entries.push(("QuicksandCave", has(532)));
    entries.push(("CrystalCave", has(532)));
    entries.push(("CrystalCrossing", has(532)));
    entries.push(("ChasmCave", has(532)));
    entries.push(("DarkHill", has(532)));
    entries.push(("SealedRuin", has(532)));
    entries.push(("DuskForest", has(532)));
    entries.push(("DeepDuskForest", has(532)));
    entries.push(("TreeshroudForest", has(532)));
    entries.push(("BrineCave", has(532)));
    entries.push(("HiddenLand", has(532)));
    entries.push(("TemporalTower", has(532)));
    entries.push(("MystifyingForest", has(532)));
    entries.push(("BlizzardIsland", has(532)));
    entries.push(("CreviceCave", has(532)));
    entries.push(("SurroundedSea", has(532)));
    entries.push(("MiracleSea", has(532)));
    entries.push(("AegisCave", has(532)));
    entries.push(("MtTravail", has(532)));
    entries.push(("TheNightmare", has(532)));
    entries.push(("SpacialRift", has(532)));
    entries.push(("DarkCrater", has(532)));
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
    if points < 17000 { entries.push(("OblivionForest", false)); }
    entries.push(("TreacherousWaters", has(272)));
    entries.push(("SoutheasternIslands", has(270)));
    entries.push(("InfernoCave", has(271)));
    entries.push(("StarCave", has(417)));
    entries.push(("SkyPeakMountainPath", has(532)));
    ordered_category(result, "dungeon", &entries);
    let mut entries = Vec::new();
    entries.push(("PokemonthatjoinedyouX", true));
    entries.push(("KindsofPokemonbattledX", true));
    entries.push(("MoveslearnedX", true));
    entries.push(("KindsofitemsacquiredX", true));
    entries.push(("DungeonsclearedX", true));
    entries.push(("RecordofonefloorvictoriesX", true));
    entries.push(("RecycledX", has(485) || has(449) || has(7) || item(169) || item(170) || item(171) || item(172)));
    entries.push(("DiscoveredtheHotSpring", has(532)));
    entries.push(("DiscoveredtheFogboundLake", has(532)));
    entries.push(("Returnedfromthefuture", has(532)));
    entries.push(("Preventedtheplanetsparalysis", has(532)));
    entries.push(("GraduatedfromWigglytuffsGuild", has(532)));
    entries.push(("ClimbedtotheSkyPeakSummit", has(532)));
    entries.push(("DiscoveredthesecretofAegisCave", has(532)));
    entries.push(("FoiledDarkraisdastardlyplot", has(532)));
    entries.push(("ReceivedtheIcyFlute", item(59)));
    entries.push(("ReceivedtheFieryDrum", item(60)));
    entries.push(("ReceivedtheTerraCymbal", item(61)));
    entries.push(("ReceivedtheAquaMonica", item(62)));
    entries.push(("ReceivedtheRockHorn", item(63)));
    entries.push(("ReceivedtheGrassCornet", item(64)));
    entries.push(("ReceivedtheSkyMelodica", item(65)));
    entries.push(("AcceptedallSpecialChallenges", has(417) && has(271) && has(270) && has(272) && has(150)));
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
    entries.push(("Deoxysjoinedtheteam", has(418) || has(419) || has(420) || has(421)));
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
    entries.push(("Shayminjoinedtheteam", has(534) || has(535)));
    ordered_category(result, "adventureLog", &entries);
    let entries = Vec::new();
    ordered_category(result, "specialEpisode", &entries);
    let mut entries = Vec::new();
    entries.push(("Drowzee", has(532)));
    entries.push(("Groudon", has(532)));
    entries.push(("Manectric", has(532)));
    entries.push(("Celebi", has(532)));
    entries.push(("Dusknoir", has(532)));
    entries.push(("Palkia", has(532)));
    entries.push(("Wigglytuff", has(532)));
    entries.push(("Shaymin", has(532)));
    entries.push(("Scizor", has(532)));
    entries.push(("Phione", has(532)));
    entries.push(("Regigigas", has(532)));
    entries.push(("Darkrai", has(532)));
    entries.push(("GoldenChest", (59..=65).all(item)));
    ordered_category(result, "progressIcon", &entries);
}
const UNOBTAINABLE: &[u32] = &[ 11, 12, 98, 113, 114, 138, 166, 175, 176, 177, 181, 184, 185, 194, 198, 205, 219, 224, 226, 236, 258, 259, 293, 294, 295, 296, 
            297, 298, 299, 300, 324, 339, 345, 349, 353, 360, 361, 363, 769, 825, 1352, 1353, 1354, 1355, 1356, 1357, 1358, 1359, 1360, 1361, 1362, 1363, 1364, 1365, 1366, 1367,
            1368, 1369, 1370, 1371, 1372, 1373, 1374, 1375, 1376, 1377, 1378, 1379, 1380, 1381, 1382, 1383, 1384, 1385, 1386, 1387, 1388, 1389, 1390, 1391, 1392, 1393, 1394,
            1395, 1396, 1397, 1398, 1399, ];
