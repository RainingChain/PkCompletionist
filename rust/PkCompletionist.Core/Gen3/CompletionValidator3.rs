#![allow(non_snake_case)]
use crate::{category, completion_validator_x::Checklist, CommandResult};
use pkhex_core::{
    binary::{u16_le, u32_le, InvalidData},
    gen3::Save3,
    pk3::Pk3,
};

struct CompletionValidator3 {
    save: Save3,
    pokemon: Vec<Pk3>,
    items: Vec<u16>,
    objective: i32,
}
impl CompletionValidator3 {
    fn has(&self, id: u16) -> bool {
        self.pokemon.iter().any(|pk| pk.species() == id)
    }
    fn own_trainer(&self, id: u16) -> bool {
        let trainer = u16_le(&self.save.small, 0xa).unwrap();
        self.pokemon
            .iter()
            .any(|pk| pk.species() == id && pk.trainer_id() == trainer)
    }
    fn item(&self, id: u16) -> bool {
        self.items.contains(&id)
    }
    fn objective(&self, id: u16) -> bool {
        if self.objective == 0 {
            self.save.caught(id)
        } else {
            self.has(id)
        }
    }
    fn form(&self, id: u16, form: u8) -> bool {
        self.pokemon
            .iter()
            .any(|pk| pk.species() == id && pk.form() == form)
    }
    fn decoration(&self, id: u8) -> bool {
        self.save.large[0x2734..0x2734 + 150].contains(&id)
    }
    fn block(&self, id: u8) -> bool {
        (0..40).any(|index| self.save.large[0x848 + index * 8] == id)
    }
    fn trendy(&self, id: usize) -> bool {
        self.save.large[0x2e20 + id / 8] & (1 << (id % 8)) != 0
    }
    fn regional_no_trade(&self) -> bool {
        (252..=260).filter(|id| self.save.seen(*id)).count() >= 5
            && (self.save.seen(380) || self.save.seen(381))
            && HOENN_NO_TRADE.iter().all(|id| self.save.seen(*id))
    }
    fn trainer_hill(&self, offset: usize) -> bool {
        let value = u32_le(&self.save.large, offset).unwrap();
        value != 0 && value < 12 * 60 * 60
    }
    fn blender(&self, offset: usize) -> bool {
        u16_le(&self.save.large, offset).unwrap() != 0
    }
}

pub fn validate(
    input: &[u8],
    objective: i32,
    result: &mut CommandResult,
) -> Result<(), InvalidData> {
    let save = Save3::read(input)?;
    let pokemon = save.pokemon()?;
    let mut items = pkhex_core::player_bag3e::items(&save);
    items.extend(pokemon.iter().map(|pk| pk.held_item()));
    CompletionValidator3 {
        save,
        pokemon,
        items,
        objective,
    }
    .GenerateAll(result);
    Ok(())
}

impl CompletionValidator3 {
    fn GenerateAll(&self, result: &mut CommandResult) {
        self.Generate_pokemon(result);
        self.Generate_item(result);
        self.Generate_pokemonForm(result);
        self.Generate_itemInMap(result);
        self.Generate_itemGift(result);
        self.Generate_itemInMap(result);
        self.Generate_itemGift(result);
        self.Generate_decoration(result);
        self.Generate_inGameTrade(result);
        self.Generate_inGameGift(result);
        self.Generate_battleFrontier(result);
        self.Generate_ribbon(result);
        self.Generate_phone(result);
        self.Generate_battle(result);
        self.Generate_battle(result);
        self.Generate_trainerStar(result);
        self.Generate_pokeblock(result);
        self.Generate_easyChatSystemWord(result);
        self.Generate_gameStat(result);
        self.Generate_misc(result);
    }

    fn Generate_pokemon(&self, result: &mut CommandResult) {
        category(
            result,
            "pokemon",
            (1..=386).map(|id| (id.to_string(), self.objective(id))),
        );
    }

    fn Generate_itemInMap(&self, result: &mut CommandResult) {
        let mut map = Checklist::default();
        for id in MAP_FLAGS {
            map.set(id, self.save.event(*id));
        }
        for (index, default) in BERRY_INDICES.iter().zip(DEFAULT_BERRIES) {
            map.set(
                format!("b{index}"),
                self.HasPickedBerry(*index, default + 1),
            );
        }
        map.emit(result, "itemInMap");
    }

    fn HasPickedBerry(&self, berryIdx: usize, defaultBerry: u8) -> bool {
        self.save.large[0x169c + 8 * berryIdx] != defaultBerry
    }

    fn Generate_itemGift(&self, result: &mut CommandResult) {
        category(
            result,
            "itemGift",
            GIFT_FLAGS
                .iter()
                .map(|id| (id.to_string(), self.save.event(*id))),
        );
    }

    fn Generate_battle(&self, result: &mut CommandResult) {
        let mut battle = Checklist::default();
        for id in 1..=854 {
            battle.set(id, self.save.event(0x500 + id));
        }
        for (id, flag) in [
            (734, 2148),
            (514, 2148),
            (809, 2252),
            (808, 2250),
            (806, 2246),
            (810, 2254),
            (807, 2248),
            (811, 2256),
            (805, 2244),
        ] {
            battle.set(id, self.save.event(flag));
        }
        battle.set(56, self.save.large[0x2bad] >= 6);
        for id in MON_BATTLES {
            battle.set(format!("p{id}"), self.save.event(*id));
        }
        battle.set("p763", self.save.event(0x2fc) && self.save.event(763));
        battle.emit(result, "battle");
    }

    fn Generate_gameStat(&self, result: &mut CommandResult) {
        category(
            result,
            "gameStat",
            (0..=51).map(|id| (id.to_string(), self.save.record(id) > 0)),
        );
    }
    fn Generate_item(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut items = Checklist::default();
        for id in (1..=376).filter(|id| !UNOBTAINABLE.contains(id)) {
            items.set(id, ctx.item(id));
        }
        items.set(375, ctx.item(375));
        for (id, flag) in [
            (281, 0x213),
            (282, 0x214),
            (283, 0x215),
            (284, 0x215),
            (274, 0xbd),
        ] {
            items.set(id, ctx.save.event(flag));
        }
        if ctx.own_trainer(25) || ctx.own_trainer(26) {
            items.set(5, true);
        }
        items.set(269, ctx.save.event(0x95));
        if ctx.save.event(0x45f) {
            items.set(272, true);
        }
        if ctx.save.event(0x446) {
            items.set(259, true);
        }
        if ctx.objective == 0 {
            for (id, flags) in ITEM_INFERENCES {
                if flags.iter().any(|id| ctx.save.event(*id)) {
                    items.set(id, true);
                }
            }
            if ctx.own_trainer(241) {
                items.set(29, true);
            }
            if ctx.has(367) && ctx.save.event(0x436) {
                items.set(192, true);
            }
            if ctx.has(368) && ctx.save.event(0x436) {
                items.set(193, true);
            }
        }
        items.emit(result, "item");
    }
}
// Generated literal tables and simple rules follow. See tools/port-emerald-rules.ps1.
impl CompletionValidator3 {
    fn Generate_pokemonForm(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("UnownA", ctx.form(201, 0));
        entries.set("UnownB", ctx.form(201, 1));
        entries.set("UnownC", ctx.form(201, 2));
        entries.set("UnownD", ctx.form(201, 3));
        entries.set("UnownE", ctx.form(201, 4));
        entries.set("UnownF", ctx.form(201, 5));
        entries.set("UnownG", ctx.form(201, 6));
        entries.set("UnownH", ctx.form(201, 7));
        entries.set("UnownI", ctx.form(201, 8));
        entries.set("UnownJ", ctx.form(201, 9));
        entries.set("UnownK", ctx.form(201, 10));
        entries.set("UnownL", ctx.form(201, 11));
        entries.set("UnownM", ctx.form(201, 12));
        entries.set("UnownN", ctx.form(201, 13));
        entries.set("UnownO", ctx.form(201, 14));
        entries.set("UnownP", ctx.form(201, 15));
        entries.set("UnownQ", ctx.form(201, 16));
        entries.set("UnownR", ctx.form(201, 17));
        entries.set("UnownS", ctx.form(201, 18));
        entries.set("UnownT", ctx.form(201, 19));
        entries.set("UnownU", ctx.form(201, 20));
        entries.set("UnownV", ctx.form(201, 21));
        entries.set("UnownW", ctx.form(201, 22));
        entries.set("UnownX", ctx.form(201, 23));
        entries.set("UnownY", ctx.form(201, 24));
        entries.set("UnownZ", ctx.form(201, 25));
        entries.set("UnownExclamationMark", ctx.form(201, 26));
        entries.set("UnownQuestionMark", ctx.form(201, 27));
        entries.set("CastformNormal", ctx.objective(351));
        entries.set("CastformFire", ctx.objective(351));
        entries.set("CastformWater", ctx.objective(351));
        entries.set("CastformIce", ctx.objective(351));
        entries.set(
            "GetShinyPokemon",
            ctx.pokemon.iter().any(|pk| pk.is_shiny()),
        );
        entries.emit(result, "pokemonForm");
    }
    fn Generate_decoration(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("SmallChair", ctx.decoration(10));
        entries.set("PokemonChair", ctx.decoration(11));
        entries.set("HeavyChair", ctx.decoration(12));
        entries.set("RaggedChair", ctx.decoration(15));
        entries.set("ComfortChair", ctx.decoration(14));
        entries.set("BrickChair", ctx.decoration(16));
        entries.set("CampChair", ctx.decoration(17));
        entries.set("HardChair", ctx.decoration(18));
        entries.set("PrettyChair", ctx.decoration(13));
        entries.set("PikaCushion", ctx.decoration(101));
        entries.set("RoundCushion", ctx.decoration(102));
        entries.set("ZigzagCushion", ctx.decoration(104));
        entries.set("SpinCushion", ctx.decoration(105));
        entries.set("DiamondCushion", ctx.decoration(106));
        entries.set("BallCushion", ctx.decoration(107));
        entries.set("GrassCushion", ctx.decoration(108));
        entries.set("FireCushion", ctx.decoration(109));
        entries.set("WaterCushion", ctx.decoration(110));
        entries.set("KissCushion", ctx.decoration(103));
        entries.set("SmallDesk", ctx.decoration(1));
        entries.set("PokemonDesk", ctx.decoration(2));
        entries.set("HeavyDesk", ctx.decoration(3));
        entries.set("RaggedDesk", ctx.decoration(4));
        entries.set("ComfortDesk", ctx.decoration(5));
        entries.set("BrickDesk", ctx.decoration(7));
        entries.set("CampDesk", ctx.decoration(8));
        entries.set("HardDesk", ctx.decoration(9));
        entries.set("PrettyDesk", ctx.decoration(6));
        entries.set("CLowNoteMat", ctx.decoration(51));
        entries.set("DNoteMat", ctx.decoration(52));
        entries.set("ENoteMat", ctx.decoration(53));
        entries.set("FNoteMat", ctx.decoration(54));
        entries.set("GNoteMat", ctx.decoration(55));
        entries.set("ANoteMat", ctx.decoration(56));
        entries.set("BNoteMat", ctx.decoration(57));
        entries.set("CHighNoteMat", ctx.decoration(58));
        entries.set("GlitterMat", ctx.decoration(48));
        entries.set("JumpMat", ctx.decoration(49));
        entries.set("SpinMat", ctx.decoration(50));
        entries.set("SurfMat", ctx.decoration(59));
        entries.set("ThunderMat", ctx.decoration(60));
        entries.set("FireBlastMat", ctx.decoration(61));
        entries.set("PowderSnowMat", ctx.decoration(62));
        entries.set("AttractMat", ctx.decoration(63));
        entries.set("FissureMat", ctx.decoration(64));
        entries.set("SpikesMat", ctx.decoration(65));
        entries.set("RedPlant", ctx.decoration(19));
        entries.set("TropicalPlant", ctx.decoration(20));
        entries.set("PrettyFlowers", ctx.decoration(21));
        entries.set("ColorfulPlant", ctx.decoration(22));
        entries.set("BigPlant", ctx.decoration(23));
        entries.set("GorgeousPlant", ctx.decoration(24));
        entries.set("MudBall", ctx.decoration(39));
        entries.set("BreakableDoor", ctx.decoration(40));
        entries.set("SandOrnament", ctx.decoration(41));
        entries.set("FenceLength", ctx.decoration(35));
        entries.set("FenceWidth", ctx.decoration(36));
        entries.set("TV", ctx.decoration(45));
        entries.set("RoundTV", ctx.decoration(46));
        entries.set("CuteTV", ctx.decoration(47));
        entries.set("SilverShield", ctx.decoration(42));
        entries.set("GoldShield", ctx.decoration(43));
        entries.set("GlassOrnament", ctx.decoration(44));
        entries.set("Tire", ctx.decoration(37));
        entries.set("SolidBoard", ctx.decoration(33));
        entries.set("Stand", ctx.decoration(38));
        entries.set("Slide", ctx.decoration(34));
        entries.set("RedBrick", ctx.decoration(25));
        entries.set("YellowBrick", ctx.decoration(26));
        entries.set("BlueBrick", ctx.decoration(27));
        entries.set("RedBalloon", ctx.decoration(28));
        entries.set("YellowBalloon", ctx.decoration(30));
        entries.set("BlueBalloon", ctx.decoration(29));
        entries.set("RedTent", ctx.decoration(31));
        entries.set("BlueTent", ctx.decoration(32));
        entries.set("AzurillDoll", ctx.decoration(95));
        entries.set("BaltoyDoll", ctx.decoration(93));
        entries.set("ChikoritaDoll", ctx.decoration(81));
        entries.set("ClefairyDoll", ctx.decoration(85));
        entries.set("CyndaquilDoll", ctx.decoration(80));
        entries.set("DittoDoll", ctx.decoration(86));
        entries.set("DuskullDoll", ctx.decoration(91));
        entries.set("GulpinDoll", ctx.decoration(98));
        entries.set("JigglypuffDoll", ctx.decoration(83));
        entries.set("KecleonDoll", ctx.decoration(94));
        entries.set("LotadDoll", ctx.decoration(99));
        entries.set("MarillDoll", ctx.decoration(78));
        entries.set("MeowthDoll", ctx.decoration(84));
        entries.set("MudkipDoll", ctx.decoration(90));
        entries.set("PichuDoll", ctx.decoration(76));
        entries.set("PikachuDoll", ctx.decoration(77));
        entries.set("SeedotDoll", ctx.decoration(100));
        entries.set("SkittyDoll", ctx.decoration(96));
        entries.set("SmoochumDoll", ctx.decoration(87));
        entries.set("SwabluDoll", ctx.decoration(97));
        entries.set("TogepiDoll", ctx.decoration(79));
        entries.set("TorchicDoll", ctx.decoration(89));
        entries.set("TotodileDoll", ctx.decoration(82));
        entries.set("TreeckoDoll", ctx.decoration(88));
        entries.set("WynautDoll", ctx.decoration(92));
        entries.set("BlastoiseDoll", ctx.decoration(116));
        entries.set("CharizardDoll", ctx.decoration(115));
        entries.set("LaprasDoll", ctx.decoration(113));
        entries.set("RhydonDoll", ctx.decoration(112));
        entries.set("SnorlaxDoll", ctx.decoration(111));
        entries.set("VenusaurDoll", ctx.decoration(114));
        entries.set("WailmerDoll", ctx.decoration(117));
        entries.set("RegirockDoll", ctx.decoration(118));
        entries.set("RegiceDoll", ctx.decoration(119));
        entries.set("RegisteelDoll", ctx.decoration(120));
        entries.set("BallPoster", ctx.decoration(66));
        entries.set("BluePoster", ctx.decoration(69));
        entries.set("CutePoster", ctx.decoration(70));
        entries.set("GreenPoster", ctx.decoration(67));
        entries.set("RedPoster", ctx.decoration(68));
        entries.set("LongPoster", ctx.decoration(72));
        entries.set("PikaPoster", ctx.decoration(71));
        entries.set("SeaPoster", ctx.decoration(73));
        entries.set("SkyPoster", ctx.decoration(74));
        entries.set("KissPoster", ctx.decoration(75));
        entries.emit(result, "decoration");
    }
    fn Generate_inGameTrade(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("RaltsforSeedot", ctx.save.event(0x0099));
        entries.set("VolbeatforPlusle", ctx.save.event(0x009B));
        entries.set("BagonforHorsea", ctx.save.event(0x009A));
        entries.set("SkittyforMeowth", ctx.save.event(0x009C));
        entries.emit(result, "inGameTrade");
    }
    fn Generate_inGameGift(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("StarterPokemon", !ctx.pokemon.is_empty());
        entries.set("WynautEgg", ctx.save.event(0x010A));
        entries.set(
            "LileeporAnorith",
            ctx.save.event(0x010B) || ctx.item(286) || ctx.item(287),
        );
        entries.set("Castform", ctx.save.event(0x0097));
        entries.set("Beldum", ctx.save.event(0x012A));
        entries.set(
            "ChikoritaorCyndaquilorTotodile",
            ctx.save.event(2148) && ctx.save.work(0xD3) >= 6,
        );
        entries.emit(result, "inGameGift");
    }
    fn Generate_battleFrontier(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("SilverKnowledgeSymbol", ctx.save.event(2252));
        entries.set("SilverGutsSymbol", ctx.save.event(2250));
        entries.set("SilverTacticsSymbol", ctx.save.event(2246));
        entries.set("SilverLuckSymbol", ctx.save.event(2254));
        entries.set("SilverSpiritsSymbol", ctx.save.event(2248));
        entries.set("SilverBraveSymbol", ctx.save.event(2256));
        entries.set("SilverAbilitySymbol", ctx.save.event(2244));
        entries.set("GoldKnowledgeSymbol", ctx.save.event(2253));
        entries.set("GoldGutsSymbol", ctx.save.event(2251));
        entries.set("GoldTacticsSymbol", ctx.save.event(2247));
        entries.set("GoldLuckSymbol", ctx.save.event(2255));
        entries.set("GoldSpiritsSymbol", ctx.save.event(2249));
        entries.set("GoldBraveSymbol", ctx.save.event(2257));
        entries.set("GoldAbilitySymbol", ctx.save.event(2245));
        entries.emit(result, "battleFrontier");
    }
    fn Generate_ribbon(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set(
            "ChampionRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 15) != 0),
        );
        entries.set(
            "CoolRibbon",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons()) & 7) >= 1),
        );
        entries.set(
            "CoolRibbonSuper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons()) & 7) >= 2),
        );
        entries.set(
            "CoolRibbonHyper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons()) & 7) >= 3),
        );
        entries.set(
            "CoolRibbonMaster",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons()) & 7) >= 4),
        );
        entries.set(
            "BeautyRibbon",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 3) & 7) >= 1),
        );
        entries.set(
            "BeautyRibbonSuper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 3) & 7) >= 2),
        );
        entries.set(
            "BeautyRibbonHyper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 3) & 7) >= 3),
        );
        entries.set(
            "BeautyRibbonMaster",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 3) & 7) >= 4),
        );
        entries.set(
            "CuteRibbon",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 6) & 7) >= 1),
        );
        entries.set(
            "CuteRibbonSuper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 6) & 7) >= 2),
        );
        entries.set(
            "CuteRibbonHyper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 6) & 7) >= 3),
        );
        entries.set(
            "CuteRibbonMaster",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 6) & 7) >= 4),
        );
        entries.set(
            "SmartRibbon",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 9) & 7) >= 1),
        );
        entries.set(
            "SmartRibbonSuper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 9) & 7) >= 2),
        );
        entries.set(
            "SmartRibbonHyper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 9) & 7) >= 3),
        );
        entries.set(
            "SmartRibbonMaster",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 9) & 7) >= 4),
        );
        entries.set(
            "ToughRibbon",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 12) & 7) >= 1),
        );
        entries.set(
            "ToughRibbonSuper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 12) & 7) >= 2),
        );
        entries.set(
            "ToughRibbonHyper",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 12) & 7) >= 3),
        );
        entries.set(
            "ToughRibbonMaster",
            ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> 12) & 7) >= 4),
        );
        entries.set(
            "WinningRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 16) != 0),
        );
        entries.set(
            "VictoryRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 17) != 0),
        );
        entries.set(
            "ArtistRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 18) != 0),
        );
        entries.set(
            "EffortRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 19) != 0),
        );
        entries.set(
            "CountryRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 23) != 0),
        );
        entries.set(
            "NationalRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 24) != 0),
        );
        entries.set(
            "EarthRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 25) != 0),
        );
        entries.set(
            "WorldRibbon",
            ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << 26) != 0),
        );
        entries.emit(result, "ribbon");
    }
    fn Generate_phone(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("PKMNLoverWally", ctx.save.event(0x0D6));
        entries.set("ElusiveEyesScott", ctx.save.event(0x0D7));
        entries.set("CalmKindMom", ctx.save.event(0x0D8));
        entries.set(
            "RadNeighborBrendan",
            ctx.save.event(0x0FD) && ctx.save.small[8] != 0,
        );
        entries.set(
            "RadNeighborMay",
            ctx.save.event(0x0FD) && ctx.save.small[8] == 0,
        );
        entries.set("PKMNProfProfBirch", ctx.save.event(0x119));
        entries.set("HardasRockSteven", ctx.save.event(0x131));
        entries.set("ReliableOneDad", ctx.save.event(0x132));
        entries.set("DevonPresMrStone", ctx.save.event(0x158));
        entries.set("AromaLadyRose", ctx.save.event(0x15D - 1));
        entries.set("RuinManiacAndres", ctx.save.event(0x15E - 1));
        entries.set("RuinManiacDusty", ctx.save.event(0x15F - 1));
        entries.set("TuberLola", ctx.save.event(0x160 - 1));
        entries.set("TuberRicky", ctx.save.event(0x161 - 1));
        entries.set("SisandBroLilaRoy", ctx.save.event(0x162 - 1));
        entries.set("CooltrainerCristin", ctx.save.event(0x163 - 1));
        entries.set("CooltrainerBrooke", ctx.save.event(0x164 - 1));
        entries.set("CooltrainerWilton", ctx.save.event(0x165 - 1));
        entries.set("HexManiacValerie", ctx.save.event(0x166 - 1));
        entries.set("LadyCindy", ctx.save.event(0x167 - 1));
        entries.set("BeautyThalia", ctx.save.event(0x168 - 1));
        entries.set("BeautyJessica", ctx.save.event(0x169 - 1));
        entries.set("RichBoyWinston", ctx.save.event(0x16A - 1));
        entries.set("PokeManiacSteve", ctx.save.event(0x16B - 1));
        entries.set("SwimmerTony", ctx.save.event(0x16C - 1));
        entries.set("BlackBeltNob", ctx.save.event(0x16D - 1));
        entries.set("BlackBeltKoji", ctx.save.event(0x16E - 1));
        entries.set("GuitaristFernando", ctx.save.event(0x16F - 1));
        entries.set("GuitaristDalton", ctx.save.event(0x170 - 1));
        entries.set("KindlerBernie", ctx.save.event(0x171 - 1));
        entries.set("CamperEthan", ctx.save.event(0x172 - 1));
        entries.set("OldCoupleJohnJay", ctx.save.event(0x173 - 1));
        entries.set("ManiacJeffrey", ctx.save.event(0x174 - 1));
        entries.set("PsychicCameron", ctx.save.event(0x175 - 1));
        entries.set("PsychicJacki", ctx.save.event(0x176 - 1));
        entries.set("GentlemanWalter", ctx.save.event(0x177 - 1));
        entries.set("SchoolKidKaren", ctx.save.event(0x178 - 1));
        entries.set("SchoolKidJerry", ctx.save.event(0x179 - 1));
        entries.set("SrandJrAnnaMeg", ctx.save.event(0x17A - 1));
        entries.set("PokefanIsabel", ctx.save.event(0x17B - 1));
        entries.set("PokefanMiguel", ctx.save.event(0x17C - 1));
        entries.set("ExpertTimothy", ctx.save.event(0x17D - 1));
        entries.set("ExpertShelby", ctx.save.event(0x17E - 1));
        entries.set("YoungsterCalvin", ctx.save.event(0x17F - 1));
        entries.set("FishermanElliot", ctx.save.event(0x180 - 1));
        entries.set("TriathleteIsaiah", ctx.save.event(0x181 - 1));
        entries.set("TriathleteMaria", ctx.save.event(0x182 - 1));
        entries.set("TriathleteAbigail", ctx.save.event(0x183 - 1));
        entries.set("TriathleteDylan", ctx.save.event(0x184 - 1));
        entries.set("TriathleteKatelyn", ctx.save.event(0x185 - 1));
        entries.set("TriathleteBenjamin", ctx.save.event(0x186 - 1));
        entries.set("TriathletePablo", ctx.save.event(0x187 - 1));
        entries.set("DragonTamerNicolas", ctx.save.event(0x188 - 1));
        entries.set("BirdKeeperRobert", ctx.save.event(0x189 - 1));
        entries.set("NinjaBoyLao", ctx.save.event(0x18A - 1));
        entries.set("BattleGirlCyndy", ctx.save.event(0x18B - 1));
        entries.set("ParasolLadyMadeline", ctx.save.event(0x18C - 1));
        entries.set("SwimmerJenny", ctx.save.event(0x18D - 1));
        entries.set("PicnickerDiana", ctx.save.event(0x18E - 1));
        entries.set("TwinsAmyLiv", ctx.save.event(0x18F - 1));
        entries.set("SailorErnest", ctx.save.event(0x190 - 1));
        entries.set("SailorCory", ctx.save.event(0x191 - 1));
        entries.set("CollectorEdwin", ctx.save.event(0x192 - 1));
        entries.set("PKMNBreederLydia", ctx.save.event(0x193 - 1));
        entries.set("PKMNBreederIsaac", ctx.save.event(0x194 - 1));
        entries.set("PKMNBreederGabrielle", ctx.save.event(0x195 - 1));
        entries.set("PKMNRangerCatherine", ctx.save.event(0x196 - 1));
        entries.set("PKMNRangerJackson", ctx.save.event(0x197 - 1));
        entries.set("LassHaley", ctx.save.event(0x198 - 1));
        entries.set("BugCatcherJames", ctx.save.event(0x199 - 1));
        entries.set("HikerTrent", ctx.save.event(0x19A - 1));
        entries.set("HikerSawyer", ctx.save.event(0x19B - 1));
        entries.set("YoungCoupleKiraDan", ctx.save.event(0x19C - 1));
        entries.set("RockinWhizRoxanne", ctx.save.event(0x1D3));
        entries.set("TheBigHitBrawly", ctx.save.event(0x1D4));
        entries.set("SwellShockWattson", ctx.save.event(0x1D5));
        entries.set("PassionBurnFlannery", ctx.save.event(0x1D6));
        entries.set("SkyTamerWinona", ctx.save.event(0x1D7));
        entries.set("MysticDuoTateLiza", ctx.save.event(0x1D8));
        entries.set("EliteFourSidney", ctx.save.event(0x1A5 - 1));
        entries.set("EliteFourPhoebe", ctx.save.event(0x1A6 - 1));
        entries.set("EliteFourGlacia", ctx.save.event(0x1A7 - 1));
        entries.set("EliteFourDrake", ctx.save.event(0x1A8 - 1));
        entries.set("ChampionWallace", ctx.save.event(0x1A9 - 1));
        entries.set("DandyCharmJuan", ctx.save.event(0x1D9));
        entries.emit(result, "phone");
    }
    fn Generate_trainerStar(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("HallofFame", ctx.save.event(2148));
        entries.set(
            "HoennPokedex",
            HOENN
                .iter()
                .all(|id| [385, 386].contains(id) || ctx.save.caught(*id)),
        );
        entries.set("PokemonContests", ctx.decoration(44));
        entries.set("BattleTower", ctx.save.event(0x01D2));
        entries.emit(result, "trainerStar");
    }
    fn Generate_pokeblock(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("BlackPokeblock", ctx.block(12));
        entries.set("RedPokeblock", ctx.block(1));
        entries.set("BluePokeblock", ctx.block(2));
        entries.set("PinkPokeblock", ctx.block(3));
        entries.set("GreenPokeblock", ctx.block(4));
        entries.set("YellowPokeblock", ctx.block(5));
        entries.set("GoldPokeblock", ctx.block(14));
        entries.set("PurplePokeblock", ctx.block(6));
        entries.set("IndigoPokeblock", ctx.block(7));
        entries.set("LiteBluePokeblock", ctx.block(9));
        entries.set("BrownPokeblock", ctx.block(8));
        entries.set("OlivePokeblock", ctx.block(10));
        entries.set("GrayPokeblock", ctx.block(11));
        entries.set("WhitePokeblock", ctx.block(13));
        entries.emit(result, "pokeblock");
    }
    fn Generate_easyChatSystemWord(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("PokemonRegionalNoTrade", ctx.regional_no_trade());
        entries.set("PokemonGolem", ctx.save.seen(76));
        entries.set("PokemonAlakazam", ctx.save.seen(65));
        entries.set("PokemonJirachi", ctx.save.seen(385));
        entries.set("PokemonDeoxys", ctx.save.seen(386));
        entries.set(
            "PokemonRegionalTrade",
            HOENN_TRADE.iter().all(|id| ctx.save.seen(*id)),
        );
        entries.set("PokemonNonRegional", ctx.save.event(0x0896));
        entries.set("Events", ctx.save.event(2148));
        entries.set("Move1", ctx.save.event(2148));
        entries.set("Move2", ctx.save.event(2148));
        entries.set("TrendyKTHXBYE", ctx.trendy(0));
        entries.set("TrendyYESSIR", ctx.trendy(1));
        entries.set("TrendyAVANTGARDE", ctx.trendy(2));
        entries.set("TrendyCOUPLE", ctx.trendy(3));
        entries.set("TrendyMUCHOBLIGED", ctx.trendy(4));
        entries.set("TrendyYEEHAW", ctx.trendy(5));
        entries.set("TrendyMEGA", ctx.trendy(6));
        entries.set("Trendy1HITKO", ctx.trendy(7));
        entries.set("TrendyDESTINY", ctx.trendy(8));
        entries.set("TrendyCANCEL", ctx.trendy(9));
        entries.set("TrendyNEW", ctx.trendy(10));
        entries.set("TrendyFLATTEN", ctx.trendy(11));
        entries.set("TrendyKIDDING", ctx.trendy(12));
        entries.set("TrendyLOSER", ctx.trendy(13));
        entries.set("TrendyLOSING", ctx.trendy(14));
        entries.set("TrendyHAPPENING", ctx.trendy(15));
        entries.set("TrendyHIPAND", ctx.trendy(16));
        entries.set("TrendySHAKE", ctx.trendy(17));
        entries.set("TrendySHADY", ctx.trendy(18));
        entries.set("TrendyUPBEAT", ctx.trendy(19));
        entries.set("TrendyMODERN", ctx.trendy(20));
        entries.set("TrendySMELLYA", ctx.trendy(21));
        entries.set("TrendyBANG", ctx.trendy(22));
        entries.set("TrendyKNOCKOUT", ctx.trendy(23));
        entries.set("TrendyHASSLE", ctx.trendy(24));
        entries.set("TrendyWINNER", ctx.trendy(25));
        entries.set("TrendyFEVER", ctx.trendy(26));
        entries.set("TrendyWANNABE", ctx.trendy(27));
        entries.set("TrendyBABY", ctx.trendy(28));
        entries.set("TrendyHEART", ctx.trendy(29));
        entries.set("TrendyOLD", ctx.trendy(30));
        entries.set("TrendyYOUNG", ctx.trendy(31));
        entries.set("TrendyUGLY", ctx.trendy(32));
        entries.emit(result, "easyChatSystemWord");
    }
    fn Generate_misc(&self, result: &mut CommandResult) {
        let ctx = self;
        let mut entries = Checklist::default();
        entries.set("DefeatSteven", ctx.save.event(0x04F8));
        entries.set("UnlockMysteryGiftSystem", ctx.save.event(0x08DB));
        entries.set("UnlockNationalPokedex", ctx.save.event(0x0896));
        entries.set("UnlockWaldaCustomWallpaper", ctx.save.large[0x3d86] != 0);
        entries.set(
            "WinBestPrizeTrainerHillNormalMode",
            ctx.trainer_hill(0x3718),
        );
        entries.set(
            "WinBestPrizeTrainerHillVarietyMode",
            ctx.trainer_hill(0x3718 + 4),
        );
        entries.set(
            "WinBestPrizeTrainerHillUniqueMode",
            ctx.trainer_hill(0x3718 + 8),
        );
        entries.set(
            "WinBestPrizeTrainerHillExpertMode",
            ctx.trainer_hill(0x3718 + 12),
        );
        entries.set("SetBerryBlenderRecord2Players", ctx.blender(0x9BC));
        entries.set("SetBerryBlenderRecord3Players", ctx.blender(0x9BC + 2));
        entries.set("SetBerryBlenderRecord4Players", ctx.blender(0x9BC + 4));
        entries.set(
            "SetPokemonJumpRecord",
            u16_le(&ctx.save.small, 0x208).unwrap() > 0,
        );
        entries.set(
            "SetPokemonJumpExcellentsinaRowRecord",
            u16_le(&ctx.save.small, 0x200).unwrap() > 0,
        );
        entries.set(
            "SetBerryPickingRecord",
            u16_le(&ctx.save.small, 0x20c).unwrap() > 0,
        );
        entries.set(
            "SetBerryPickingInrowwith5playersRecord",
            u16_le(&ctx.save.small, 0x212).unwrap() > 0,
        );
        entries.set("NewLotadSizeRecord", ctx.save.work(0x4F) > 0x8000);
        entries.set("NewSeedotSizeRecord", ctx.save.work(0x47) > 0x8000);
        entries.set(
            "GetInfectedbyPokerus",
            ctx.pokemon.iter().any(|pk| pk.data[0x44] != 0),
        );
        entries.emit(result, "misc");
    }
}
const HOENN_NO_TRADE: &[u16] = &[
    25, 26, 27, 28, 37, 38, 39, 40, 41, 42, 43, 44, 45, 54, 55, 63, 64, 66, 67, 68, 72, 73, 74, 75,
    81, 82, 84, 85, 88, 89, 100, 101, 109, 110, 111, 112, 116, 117, 118, 119, 120, 121, 127, 129,
    130, 169, 170, 171, 172, 174, 177, 178, 182, 183, 184, 202, 203, 214, 218, 219, 222, 227, 230,
    231, 232, 261, 262, 263, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277,
    278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296,
    297, 298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315,
    316, 317, 318, 319, 320, 321, 322, 323, 324, 325, 326, 327, 328, 329, 330, 331, 332, 333, 334,
    335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350, 351, 352, 353,
    354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 369, 370, 371, 372, 373, 374,
    375, 376, 377, 378, 379, 382, 383, 384,
];
const HOENN_TRADE: &[u16] = &[
    252, 253, 254, 255, 256, 257, 258, 259, 260, 367, 368, 380, 381,
];
const HOENN: &[u16] = &[
    25, 26, 27, 28, 37, 38, 39, 40, 41, 42, 43, 44, 45, 54, 55, 63, 64, 65, 66, 67, 68, 72, 73, 74,
    75, 76, 81, 82, 84, 85, 88, 89, 100, 101, 109, 110, 111, 112, 116, 117, 118, 119, 120, 121,
    127, 129, 130, 169, 170, 171, 172, 174, 177, 178, 182, 183, 184, 202, 203, 214, 218, 219, 222,
    227, 230, 231, 232, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266,
    267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285,
    286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304,
    305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 318, 319, 320, 321, 322, 323,
    324, 325, 326, 327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342,
    343, 344, 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361,
    362, 363, 364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374, 375, 376, 377, 378, 379, 380,
    381, 382, 383, 384, 385, 386,
];
const UNOBTAINABLE: &[u16] = &[
    44, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 72, 82, 87, 88, 89, 90, 91, 92, 99, 100, 101,
    102, 105, 112, 113, 114, 115, 116, 117, 118, 119, 120, 176, 177, 178, 226, 227, 228, 229, 230,
    231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249,
    250, 251, 252, 253, 266, 267, 276, 277, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357,
    358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 373, 374,
];
const MAP_FLAGS: &[usize] = &[
    0x1F4, 0x1F5, 0x1F6, 0x1F7, 0x1F8, 0x1F9, 0x1FA, 0x1FB, 0x1FC, 0x1FD, 0x1FE, 0x1FF, 0x200,
    0x201, 0x202, 0x203, 0x204, 0x205, 0x206, 0x207, 0x208, 0x209, 0x20A, 0x20B, 0x20C, 0x20D,
    0x20E, 0x20F, 0x210, 0x211, 0x212, 0x213, 0x214, 0x215, 0x216, 0x217, 0x218, 0x219, 0x21A,
    0x21B, 0x21C, 0x21D, 0x21E, 0x21F, 0x220, 0x221, 0x222, 0x223, 0x224, 0x225, 0x226, 0x227,
    0x228, 0x229, 0x22A, 0x22B, 0x22C, 0x22D, 0x22E, 0x22F, 0x230, 0x231, 0x232, 0x233, 0x234,
    0x235, 0x236, 0x237, 0x238, 0x239, 0x23A, 0x23B, 0x23C, 0x23D, 0x23E, 0x23F, 0x240, 0x241,
    0x242, 0x243, 0x244, 0x245, 0x246, 0x247, 0x248, 0x249, 0x24A, 0x24B, 0x24C, 0x24D, 0x24E,
    0x24F, 0x250, 0x251, 0x252, 0x253, 0x254, 0x255, 0x256, 0x257, 0x258, 0x259, 0x25A, 0x25B,
    0x25C, 0x25D, 0x25E, 0x25F, 0x260, 0x261, 0x262, 0x263, 0x3E8, 0x3E9, 0x3EA, 0x3EB, 0x3EC,
    0x3ED, 0x3EE, 0x3EF, 0x3F0, 0x3F1, 0x3F2, 0x3F3, 0x3F4, 0x3F5, 0x3F6, 0x3F7, 0x3F8, 0x3F9,
    0x3FA, 0x3FB, 0x3FC, 0x3FD, 0x3FE, 0x3FF, 0x400, 0x401, 0x402, 0x403, 0x404, 0x405, 0x406,
    0x407, 0x408, 0x40A, 0x40B, 0x40C, 0x40D, 0x40E, 0x40F, 0x410, 0x411, 0x412, 0x413, 0x414,
    0x415, 0x416, 0x417, 0x418, 0x419, 0x41A, 0x41B, 0x41C, 0x41D, 0x41E, 0x41F, 0x420, 0x421,
    0x422, 0x423, 0x424, 0x425, 0x426, 0x427, 0x428, 0x429, 0x42A, 0x42B, 0x42C, 0x42D, 0x42E,
    0x42F, 0x430, 0x431, 0x432, 0x433, 0x434, 0x435, 0x436, 0x437, 0x438, 0x439, 0x43A, 0x43B,
    0x43C, 0x43D, 0x43E, 0x43F, 0x440, 0x441, 0x442, 0x443, 0x444, 0x445, 0x446, 0x447, 0x448,
    0x449, 0x44A, 0x44B, 0x44C, 0x44D, 0x44E, 0x44F, 0x450, 0x451, 0x452, 0x453, 0x454, 0x455,
    0x456, 0x457, 0x458, 0x459, 0x45A, 0x45B, 0x45C, 0x45D, 0x45E, 0x45F, 0x460, 0x461, 0x462,
    0x463, 0x464, 0x469, 0x46A, 0x46B, 0x46C, 0x46E, 0x46F, 0x471, 0x473, 0x474, 0x475, 0x476,
    0x477, 0x478, 0x47A, 0x47B, 0x47C, 0x47D, 0x47E, 0x47F, 0x480, 0x481, 0x482, 0x483, 0x484,
    0x485, 0x486, 0x487, 0x488, 0x489, 0x48A, 0x48B, 0x48C, 0x48D, 0x48E, 0x48F, 0x490, 0x491,
    0x492,
];
const GIFT_FLAGS: &[usize] = &[
    0x05A, 0x05E, 0x05F, 0x060, 0x06A, 0x06B, 0x06D, 0x06E, 0x073, 0x079, 0x07A, 0x07B, 0x083,
    0x084, 0x085, 0x089, 0x08C, 0x098, 0x0A5, 0x0A6, 0x0A7, 0x0A8, 0x0A9, 0x0AA, 0x0AB, 0x0AC,
    0x0C0, 0x0C8, 0x0C9, 0x0CA, 0x0CB, 0x0CC, 0x0D0, 0x0D1, 0x0D4, 0x0D5, 0x0DD, 0x0DF, 0x0E1,
    0x0E2, 0x0E3, 0x0E5, 0x0E6, 0x0E7, 0x0E8, 0x0EA, 0x0EB, 0x0EC, 0x0ED, 0x0EE, 0x0F5, 0x0F6,
    0x0F8, 0x0F9, 0x0FA, 0x0FB, 0x0FC, 0x0FE, 0x100, 0x101, 0x102, 0x104, 0x105, 0x106, 0x108,
    0x109, 0x10B, 0x10D, 0x110, 0x113, 0x114, 0x115, 0x116, 0x117, 0x118, 0x11A, 0x11B, 0x11D,
    0x121, 0x123, 0x129, 0x138, 0x151, 0x1D1,
];
const BERRY_INDICES: &[usize] = &[
    1, 2, 4, 5, 6, 7, 8, 10, 11, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
    29, 30, 31, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 52, 53, 55,
    56, 58, 59, 60, 61, 62, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 76, 77, 78, 79, 80, 81, 82,
    83, 84, 85, 86, 87, 88, 89,
];
const DEFAULT_BERRIES: &[u8] = &[
    2, 6, 6, 0, 5, 0, 0, 5, 6, 2, 22, 20, 17, 17, 17, 15, 15, 3, 2, 2, 3, 19, 1, 18, 18, 18, 20, 9,
    9, 20, 20, 20, 4, 4, 4, 2, 2, 2, 15, 17, 19, 18, 7, 4, 3, 1, 17, 17, 16, 16, 20, 20, 24, 24, 5,
    5, 24, 1, 19, 7, 21, 21, 21, 24, 22, 22, 0, 7, 7, 22, 6, 6, 35, 23, 23, 9, 5, 2, 9, 3,
];
const MON_BATTLES: &[usize] = &[
    0x1BB, 0x1BC, 0x1BD, 0x1BE, 0x1BF, 0x1C0, 0x1C9, 0x2CE, 0x2FB, 0x320, 0x321, 0x34A, 0x3CE,
    0x3CF, 0x3D0, 0x3D1, 0x3D2, 0x3D9, 0x3DA, 0x3DB, 0x3DC, 0x3DD, 0x3DE, 0x3CA,
];
const ITEM_INFERENCES: &[(u16, &[usize])] = &[
    (272, &[0x045F]),
    (259, &[0x0446]),
    (376, &[0x2CE]),
    (280, &[0x0E5]),
    (47, &[0x03BC, 0x03BD, 0x03BE, 0x03BF]),
    (48, &[0x0444, 0x020C]),
    (49, &[0x0445, 0x0200]),
    (49, &[0x0451, 0x01FD]),
    (93, &[0x00C0]),
    (94, &[0x0416]),
    (95, &[0x457]),
    (98, &[0x0403]),
    (123, &[0x042B]),
    (127, &[0x042D]),
    (129, &[0x042C]),
    (132, &[0x009C]),
    (187, &[0x114]),
    (278, &[0x0436]),
    (285, &[0x044C, 0x00EF]),
    (286, &[0x014F, 0x03C3]),
    (287, &[0x0150, 0x03C4]),
    (291, &[0x00AC]),
    (295, &[0x0458]),
    (332, &[0x00EA]),
    (334, &[0x010D]),
];
