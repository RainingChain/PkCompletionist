#![allow(non_snake_case, non_camel_case_types)]
use crate::bit_block::bits;
use crate::{category, ordered_category, CommandResult};
use pkhex_core::binary::{sum32, u32_le, InvalidData};
struct CompletionValidator3_PmdRescueTeam<'a> {
    sav: &'a [u8],
    ownedMonList: Vec<u32>,
    ownedItemList: Vec<u32>,
    points: i32,
    objective: i32,
}
pub fn validate(
    data: &[u8],
    objective: i32,
    result: &mut CommandResult,
) -> Result<(), InvalidData> {
    CompletionValidator3_PmdRescueTeam::new(data, objective)?.GenerateAll(result);
    Ok(())
}
impl<'a> CompletionValidator3_PmdRescueTeam<'a> {
    fn new(data: &'a [u8], objective: i32) -> Result<Self, InvalidData> {
        let eu = sum32(data, 4, 0x57d0)?.wrapping_sub(1) == u32_le(data, 0)?;
        let shift = if eu { 4 } else { 0 };
        Ok(Self {
            sav: data,
            ownedMonList: Self::GetObtainedPokemons(data, shift)?,
            ownedItemList: Self::GetObtainedItems(data, shift)?,
            points: bits(data, (0x4ed3 + shift) * 8, 32)? as i32,
            objective,
        })
    }
    fn GetObtainedPokemons(data: &[u8], shift: usize) -> Result<Vec<u32>, InvalidData> {
        let mut owned = Vec::new();
        let mon_offset = (0x5b3 + shift) * 8 + 3 - 323 * 9;
        for index in 0..413 {
            let id = bits(data, mon_offset + index * 323 + 7, 9)?;
            let id = match id {
                376..=379 => 376,
                414 | 417..=419 => 414,
                _ => id,
            };
            if id != 0 {
                owned.push(id);
            }
        }
        Ok(owned)
    }
    fn GetObtainedItems(data: &[u8], shift: usize) -> Result<Vec<u32>, InvalidData> {
        let mut items = Vec::new();
        for index in 0..239 {
            if bits(data, (0x4d2b + shift) * 8 - 2 + index * 10, 10)? > 0 {
                items.push((index + 1) as u32);
            }
        }
        for index in 0..20 {
            let offset = (0x4cf0 + shift) * 8 + index * 33;
            if bits(data, offset, 1)? != 0 {
                items.push(bits(data, offset + 15, 8)?);
            }
        }
        Ok(items)
    }
    fn HasMon(&self, id: u32) -> bool {
        self.ownedMonList.contains(&id)
    }
    fn HasItem(&self, id: u32) -> bool {
        self.ownedItemList.contains(&id)
    }
    fn GetFlag(&self, offset: usize, index: usize) -> bool {
        self.sav[offset + index / 8] & (1 << (index % 8)) != 0
    }
    fn HasCompletedStory(&self, index: usize) -> bool {
        self.GetFlag(0x4eec, index)
    }
    fn HasCompletedSkyTower(&self) -> bool {
        self.HasCompletedStory(3)
    }
    fn HasFriendArea(&self, index: usize) -> bool {
        self.GetFlag(0x4ed8, index)
    }
    fn HasCompletedMakuhitaDojo(&self, index: usize) -> bool {
        self.GetFlag(0xaa, index)
    }
    fn GenerateAll(&self, result: &mut CommandResult) {
        self.Generate_pokemon(result);
        self.Generate_item(result);
        self.Generate_friendArea(result);
        self.Generate_makuhitaDojo(result);
        self.Generate_rank(result);
        self.Generate_dungeon(result);
        self.Generate_adventureLog(result);
        self.Generate_progressIcon(result);
        self.Generate_teamBaseFigure(result);
    }
    fn Generate_pokemon(&self, result: &mut CommandResult) {
        category(
            result,
            "pokemon",
            (1..=416)
                .filter(|id| !(377..=379).contains(id))
                .map(|id| (id.to_string(), self.HasMon(id))),
        );
    }
    fn Generate_item(&self, result: &mut CommandResult) {
        category(
            result,
            "item",
            (1..=239).map(|id| {
                (
                    id.to_string(),
                    self.HasItem(id)
                        || id == 105
                        || self.objective != 1
                            && match id {
                                47 => self.HasMon(375),
                                48 => self.HasMon(196),
                                49 => self.HasMon(197),
                                124 => self.HasCompletedStory(8),
                                _ => false,
                            },
                )
            }),
        );
    }
}
// Ported rule expressions; regenerate with rust/tools/port-pmd-rules.ps1.
impl CompletionValidator3_PmdRescueTeam<'_> {
    #[allow(clippy::vec_init_then_push)]
    fn Generate_friendArea(&self, result: &mut CommandResult) {
        let friend = |id| self.HasFriendArea(id);
        let mut entries = Vec::new();
        entries.push(("AgedChamberAN", friend(44)));
        entries.push(("AgedChamberO", friend(45)));
        entries.push(("AncientRelic", friend(46)));
        entries.push(("BeauPlains", friend(13)));
        entries.push(("BoulderCave", friend(36)));
        entries.push(("BountifulSea", friend(1)));
        entries.push(("Crater", friend(42)));
        entries.push(("CrypticCave", friend(34)));
        entries.push(("DarknessRidge", friend(47)));
        entries.push(("DecrepitLab", friend(38)));
        entries.push(("DeepSeaCurrent", friend(5)));
        entries.push(("DeepSeaFloor", friend(4)));
        entries.push(("DragonCave", friend(35)));
        entries.push(("EchoCave", friend(33)));
        entries.push(("EnclosedIsland", friend(56)));
        entries.push(("EnergeticForest", friend(21)));
        entries.push(("FinalIsland", friend(57)));
        entries.push(("FlyawayForest", friend(19)));
        entries.push(("FrigidCavern", friend(48)));
        entries.push(("FurnaceDesert", friend(43)));
        entries.push(("HealingForest", friend(23)));
        entries.push(("IceFloeBeach", friend(49)));
        entries.push(("Jungle", friend(37)));
        entries.push(("LegendaryIsland", friend(54)));
        entries.push(("MagneticQuarry", friend(53)));
        entries.push(("MistRiseForest", friend(18)));
        entries.push(("MtCleft", friend(9)));
        entries.push(("MtDeepgreen", friend(8)));
        entries.push(("MtDiscipline", friend(39)));
        entries.push(("MtMoonview", friend(10)));
        entries.push(("MushroomForest", friend(22)));
        entries.push(("MysticLake", friend(29)));
        entries.push(("OvergrownForest", friend(20)));
        entries.push(("PeanutSwamp", friend(31)));
        entries.push(("PoisonSwamp", friend(32)));
        entries.push(("PowerPlant", friend(41)));
        entries.push(("RainbowPeak", friend(11)));
        entries.push(("RavagedField", friend(52)));
        entries.push(("RubaDubRiver", friend(26)));
        entries.push(("SacredField", friend(17)));
        entries.push(("Safari", friend(15)));
        entries.push(("ScorchedPlains", friend(16)));
        entries.push(("SeafloorCave", friend(6)));
        entries.push(("SecretiveForest", friend(25)));
        entries.push(("SereneSea", friend(3)));
        entries.push(("ShallowBeach", friend(7)));
        entries.push(("SkyBluePlains", friend(14)));
        entries.push(("SouthernIsland", friend(55)));
        entries.push(("StratosLookout", friend(51)));
        entries.push(("TadpolePond", friend(27)));
        entries.push(("ThunderMeadow", friend(40)));
        entries.push(("TransformForest", friend(24)));
        entries.push(("TreasureSea", friend(2)));
        entries.push(("TurtleshellPond", friend(28)));
        entries.push(("VolcanicPit", friend(50)));
        entries.push(("WaterfallLake", friend(30)));
        entries.push(("WildPlains", friend(12)));
        ordered_category(result, "friendArea", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_makuhitaDojo(&self, result: &mut CommandResult) {
        let dojo = |id| self.HasCompletedMakuhitaDojo(id);
        let mut entries = Vec::new();
        entries.push(("NormalMaze", dojo(0)));
        entries.push(("FireMaze", dojo(1)));
        entries.push(("WaterMaze", dojo(2)));
        entries.push(("GrassMaze", dojo(3)));
        entries.push(("ElectricMaze", dojo(4)));
        entries.push(("IceMaze", dojo(5)));
        entries.push(("FightingMaze", dojo(6)));
        entries.push(("GroundMaze", dojo(7)));
        entries.push(("FlyingMaze", dojo(8)));
        entries.push(("PsychicMaze", dojo(9)));
        entries.push(("PoisonMaze", dojo(10)));
        entries.push(("BugMaze", dojo(11)));
        entries.push(("RockMaze", dojo(12)));
        entries.push(("GhostMaze", dojo(13)));
        entries.push(("DragonMaze", dojo(14)));
        entries.push(("DarkMaze", dojo(15)));
        entries.push(("SteelMaze", dojo(16)));
        entries.push(("TeamShifty", dojo(17)));
        entries.push(("TeamConstrictor", dojo(18)));
        entries.push(("TeamHydro", dojo(19)));
        entries.push(("TeamRumblerock", dojo(20)));
        ordered_category(result, "makuhitaDojo", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_rank(&self, result: &mut CommandResult) {
        let points = self.points;
        let mut entries = Vec::new();
        entries.push(("NormalRank", true));
        entries.push(("BronzeRank", points >= 50));
        entries.push(("SilverRank", points >= 500));
        entries.push(("GoldRank", points >= 1500));
        entries.push(("PlatinumRank", points >= 3000));
        entries.push(("DiamondRank", points >= 7500));
        entries.push(("LucarioRank", points >= 15000));
        ordered_category(result, "rank", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_dungeon(&self, result: &mut CommandResult) {
        let has = |id| self.HasMon(id);
        let item = |id| self.HasItem(id);
        let story = |id| self.HasCompletedStory(id);
        let friend = |id| self.HasFriendArea(id);
        let data = self.sav;
        let mut entries = Vec::new();
        entries.push(("TinyWoods", true));
        entries.push(("ThunderwaveCave", self.HasCompletedSkyTower()));
        entries.push(("MtSteel", self.HasCompletedSkyTower()));
        entries.push(("SinisterWoods", self.HasCompletedSkyTower()));
        entries.push(("SilentChasm", self.HasCompletedSkyTower()));
        entries.push(("MtThunder", self.HasCompletedSkyTower()));
        entries.push(("GreatCanyon", self.HasCompletedSkyTower()));
        entries.push(("LapisCave", self.HasCompletedSkyTower()));
        entries.push(("RockPath", self.HasCompletedSkyTower()));
        entries.push(("MtBlaze", self.HasCompletedSkyTower()));
        entries.push(("SnowPath", self.HasCompletedSkyTower()));
        entries.push(("FrostyForest", self.HasCompletedSkyTower()));
        entries.push(("MtFreeze", self.HasCompletedSkyTower()));
        entries.push(("UproarForest", self.HasCompletedSkyTower()));
        entries.push(("MagmaCavern", self.HasCompletedSkyTower()));
        entries.push(("SkyTower", self.HasCompletedSkyTower()));
        entries.push(("HowlingForest", has(260)));
        entries.push(("StormySea", has(410)));
        entries.push(("SilverTrench", has(274)));
        entries.push(("MeteorCave", has(414)));
        entries.push(("FieryField", has(269)));
        entries.push(("LightningField", has(268)));
        entries.push(("NorthwindField", has(270)));
        entries.push(("MtFaraway", has(275)));
        entries.push(("WesternCave", has(150)));
        entries.push(("NorthernRange", has(409)));
        entries.push(("PitfallValley", has(409)));
        entries.push(("BuriedRelic", has(151) || has(405) || has(406) || has(406)));
        entries.push(("WishCave", has(413)));
        entries.push(("MurkyCave", story(4)));
        entries.push(("DesertRegion", friend(43)));
        entries.push(("SouthernCavern", friend(36)));
        entries.push(("WyvernHill", friend(35)));
        entries.push(("SolarCave", has(228) || item(230) || item(226)));
        entries.push(("DarknightRelic", has(327)));
        entries.push(("GrandSea", has(251)));
        entries.push(("WaterfallPond", has(118)));
        entries.push(("UnownRelic", (201..=226).any(has)));
        entries.push(("JoyousTower", has(77)));
        entries.push(("FarOffSea", item(209)));
        entries.push(("PurityForest", has(276)));
        entries.push(("FantasyStrait", (data[0xA7] & 0b0010_0000) != 0));
        entries.push(("OddityCave", (data[0xA7] & 0b0000_0100) != 0));
        entries.push(("RemainsIsland", (data[0xA7] & 0b0000_1000) != 0));
        entries.push(("MarvelousSea", (data[0xA7] & 0b0001_0000) != 0));
        ordered_category(result, "dungeon", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_adventureLog(&self, result: &mut CommandResult) {
        let has = |id| self.HasMon(id);
        let story = |id| self.HasCompletedStory(id);
        let mut entries = Vec::new();
        entries.push(("ReachedtheHilloftheAncients", story(1)));
        entries.push(("Tooktotheroadasfugitives", story(2)));
        entries.push(("Preventedthemeteorscollision", story(3)));
        entries.push(("BrokethecurseonGardevoir", story(4)));
        entries.push(("Therescueteambasewascompleted", story(5)));
        entries.push(("RescuedSmeargle", story(6)));
        entries.push(("SpottedMunchlax", story(7)));
        entries.push(("Xmoveswerelearned", story(8)));
        entries.push(("Xfriendrescuesweresuccessful", story(9)));
        entries.push(("XPokemonevolved", story(10)));
        entries.push(("XPokemonjoinedtheteam", story(11)));
        entries.push(("Xthievingattemptssucceeded", story(12)));
        entries.push(("Xfloorswereexplored", story(13)));
        entries.push(("AllFriendAreaswereobtained", story(14)));
        entries.push(("AllFriendAreaswereobtained", story(15)));
        entries.push(("AllPokemonweremadeleaders", story(16)));
        entries.push(("AllPokemonjoinedtheteam", story(17)));
        entries.push(("Moltresjoinedtheteam", has(146)));
        entries.push(("Zapdosjoinedtheteam", has(145)));
        entries.push(("Articunojoinedtheteam", has(144)));
        entries.push(("Deoxysjoinedtheteam", has(414)));
        entries.push(("Enteijoinedtheteam", has(269)));
        entries.push(("Raikoujoinedtheteam", has(268)));
        entries.push(("Suicunejoinedtheteam", has(270)));
        entries.push(("HoOhjoinedtheteam", has(275)));
        entries.push(("Kyogrejoinedtheteam", has(410)));
        entries.push(("Groudonjoinedtheteam", has(411)));
        entries.push(("Rayquazajoinedtheteam", has(412)));
        entries.push(("Lugiajoinedtheteam", has(274)));
        entries.push(("Celebijoinedtheteam", has(276)));
        entries.push(("Mewjoinedtheteam", has(151)));
        entries.push(("Mewtwojoinedtheteam", has(150)));
        entries.push(("Jirachijoinedtheteam", has(413)));
        ordered_category(result, "adventureLog", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_progressIcon(&self, result: &mut CommandResult) {
        let has = |id| self.HasMon(id);
        let mut entries = Vec::new();
        entries.push(("Zapdos", self.HasCompletedSkyTower()));
        entries.push(("Moltres", self.HasCompletedSkyTower()));
        entries.push(("Articuno", self.HasCompletedSkyTower()));
        entries.push(("Groudon", self.HasCompletedSkyTower()));
        entries.push(("Rayquaza", self.HasCompletedSkyTower()));
        entries.push(("Kyogre", has(410)));
        entries.push(("Mew", has(151)));
        entries.push(("HoOh", has(275)));
        entries.push(("Celebi", has(276)));
        entries.push(("Lugia", has(274)));
        entries.push(("Mewtwo", has(150)));
        entries.push(("Deoxys", has(414)));
        ordered_category(result, "progressIcon", &entries);
    }
    #[allow(clippy::vec_init_then_push)]
    fn Generate_teamBaseFigure(&self, result: &mut CommandResult) {
        let dojo = |id| self.HasCompletedMakuhitaDojo(id);
        let data = self.sav;
        let points = self.points;
        let mut entries = Vec::new();
        entries.push((
            "BonslyFigure",
            dojo(0)
                && dojo(1)
                && dojo(2)
                && dojo(3)
                && dojo(4)
                && dojo(5)
                && dojo(6)
                && dojo(7)
                && dojo(8)
                && dojo(9)
                && dojo(10)
                && dojo(11)
                && dojo(12)
                && dojo(13)
                && dojo(14)
                && dojo(15)
                && dojo(16),
        ));
        entries.push(("MimeJrFigure", (data[0xDB] & 0b01) != 0));
        entries.push(("WeavileFigure", (data[0xDB] & 0b10) != 0));
        entries.push(("LucarioFigure", points >= 15000));
        ordered_category(result, "teamBaseFigure", &entries);
    }
}
