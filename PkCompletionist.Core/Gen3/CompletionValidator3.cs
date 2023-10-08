using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;

namespace PkCompletionist.Core;

internal class CompletionValidator3 : CompletionValidatorX
{
    public CompletionValidator3(Command command, SAV3E sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;

        // Note that in US game, there are more unobtainable items than that
        this.unobtainableItems = new List<int>() { 44, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 72, 82, 87, 88, 89, 90, 91, 92, 99, 100, 101, 102, 105, 112, 113, 114, 115, 116, 117, 118, 119, 120, 176, 177, 178, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 266, 267, 276, 277, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 373, 374 };
    }

    new SAV3E sav;

    public override void GenerateAll()
    {
        base.GenerateAll();

        Generate_pokemonForm();
        Generate_decoration();
        Generate_inGameTrade();
        Generate_inGameGift();
        Generate_battleFrontier();
        Generate_ribbon();
        Generate_phone();
        Generate_trainerStar();
        Generate_eReaderBattles();
        Generate_pokeblock();
        Generate_easyChatSystemWord();
        Generate_misc();
    }

    public void Generate_pokemonForm()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemonForm"] = ow;

        ow["UnownA"] = HasForm(201, 0);
        ow["UnownB"] = HasForm(201, 1);
        ow["UnownC"] = HasForm(201, 2);
        ow["UnownD"] = HasForm(201, 3);
        ow["UnownE"] = HasForm(201, 4);
        ow["UnownF"] = HasForm(201, 5);
        ow["UnownG"] = HasForm(201, 6);
        ow["UnownH"] = HasForm(201, 7);
        ow["UnownI"] = HasForm(201, 8);
        ow["UnownJ"] = HasForm(201, 9);
        ow["UnownK"] = HasForm(201, 10);
        ow["UnownL"] = HasForm(201, 11);
        ow["UnownM"] = HasForm(201, 12);
        ow["UnownN"] = HasForm(201, 13);
        ow["UnownO"] = HasForm(201, 14);
        ow["UnownP"] = HasForm(201, 15);
        ow["UnownQ"] = HasForm(201, 16);
        ow["UnownR"] = HasForm(201, 17);
        ow["UnownS"] = HasForm(201, 18);
        ow["UnownT"] = HasForm(201, 19);
        ow["UnownU"] = HasForm(201, 20);
        ow["UnownV"] = HasForm(201, 21);
        ow["UnownW"] = HasForm(201, 22);
        ow["UnownX"] = HasForm(201, 23);
        ow["UnownY"] = HasForm(201, 24);
        ow["UnownZ"] = HasForm(201, 25);
        ow["UnownExclamationMark"] = HasForm(201, 26);
        ow["UnownQuestionMark"] = HasForm(201, 27);

        ow["CastformNormal"] = HasPkm(351);
        ow["CastformFire"] = HasPkm(351);
        ow["CastformWater"] = HasPkm(351);
        ow["CastformIce"] = HasPkm(351);
    }
    
    public bool HasDeco(Decoration3 deco)
    {
        var CheckSpan = (Span<Decoration3> span) => {
            return span.ToArray().Contains(deco);
        };
        return
            CheckSpan(sav.Decorations.Chair) ||
            CheckSpan(sav.Decorations.Cushion) ||
            CheckSpan(sav.Decorations.Desk) ||
            CheckSpan(sav.Decorations.Doll) ||
            CheckSpan(sav.Decorations.Mat) ||
            CheckSpan(sav.Decorations.Ornament) ||
            CheckSpan(sav.Decorations.Plant) ||
            CheckSpan(sav.Decorations.Poster);
    }

    public void Generate_decoration()
    {
        var ow = new Dictionary<string, bool>();
        owned["decoration"] = ow;

        ow["SmallChair"] = HasDeco(Decoration3.SMALL_CHAIR);
        ow["PokemonChair"] = HasDeco(Decoration3.POKEMON_CHAIR);
        ow["HeavyChair"] = HasDeco(Decoration3.HEAVY_CHAIR);
        ow["RaggedChair"] = HasDeco(Decoration3.RAGGED_CHAIR);
        ow["ComfortChair"] = HasDeco(Decoration3.COMFORT_CHAIR);
        ow["BrickChair"] = HasDeco(Decoration3.BRICK_CHAIR);
        ow["CampChair"] = HasDeco(Decoration3.CAMP_CHAIR);
        ow["HardChair"] = HasDeco(Decoration3.HARD_CHAIR);
        ow["PrettyChair"] = HasDeco(Decoration3.PRETTY_CHAIR);
        ow["PikaCushion"] = HasDeco(Decoration3.PIKA_CUSHION);
        ow["RoundCushion"] = HasDeco(Decoration3.ROUND_CUSHION);
        ow["ZigzagCushion"] = HasDeco(Decoration3.ZIGZAG_CUSHION);
        ow["SpinCushion"] = HasDeco(Decoration3.SPIN_CUSHION);
        ow["DiamondCushion"] = HasDeco(Decoration3.DIAMOND_CUSHION);
        ow["BallCushion"] = HasDeco(Decoration3.BALL_CUSHION);
        ow["GrassCushion"] = HasDeco(Decoration3.GRASS_CUSHION);
        ow["FireCushion"] = HasDeco(Decoration3.FIRE_CUSHION);
        ow["WaterCushion"] = HasDeco(Decoration3.WATER_CUSHION);
        ow["KissCushion"] = HasDeco(Decoration3.KISS_CUSHION);
        ow["SmallDesk"] = HasDeco(Decoration3.SMALL_DESK);
        ow["PokemonDesk"] = HasDeco(Decoration3.POKEMON_DESK);
        ow["HeavyDesk"] = HasDeco(Decoration3.HEAVY_DESK);
        ow["RaggedDesk"] = HasDeco(Decoration3.RAGGED_DESK);
        ow["ComfortDesk"] = HasDeco(Decoration3.COMFORT_DESK);
        ow["BrickDesk"] = HasDeco(Decoration3.BRICK_DESK);
        ow["CampDesk"] = HasDeco(Decoration3.CAMP_DESK);
        ow["HardDesk"] = HasDeco(Decoration3.HARD_DESK);
        ow["PrettyDesk"] = HasDeco(Decoration3.PRETTY_DESK);
        ow["CLowNoteMat"] = HasDeco(Decoration3.C_LOW_NOTE_MAT);
        ow["DNoteMat"] = HasDeco(Decoration3.D_NOTE_MAT);
        ow["ENoteMat"] = HasDeco(Decoration3.E_NOTE_MAT);
        ow["FNoteMat"] = HasDeco(Decoration3.F_NOTE_MAT);
        ow["GNoteMat"] = HasDeco(Decoration3.G_NOTE_MAT);
        ow["ANoteMat"] = HasDeco(Decoration3.A_NOTE_MAT);
        ow["BNoteMat"] = HasDeco(Decoration3.B_NOTE_MAT);
        ow["CHighNoteMat"] = HasDeco(Decoration3.C_HIGH_NOTE_MAT);
        ow["GlitterMat"] = HasDeco(Decoration3.GLITTER_MAT);
        ow["JumpMat"] = HasDeco(Decoration3.JUMP_MAT);
        ow["SpinMat"] = HasDeco(Decoration3.SPIN_MAT);
        ow["SurfMat"] = HasDeco(Decoration3.SURF_MAT);
        ow["ThunderMat"] = HasDeco(Decoration3.THUNDER_MAT);
        ow["FireBlastMat"] = HasDeco(Decoration3.FIRE_BLAST_MAT);
        ow["PowderSnowMat"] = HasDeco(Decoration3.POWDER_SNOW_MAT);
        ow["AttractMat"] = HasDeco(Decoration3.ATTRACT_MAT);
        ow["FissureMat"] = HasDeco(Decoration3.FISSURE_MAT);
        ow["SpikesMat"] = HasDeco(Decoration3.SPIKES_MAT);
        ow["RedPlant"] = HasDeco(Decoration3.RED_PLANT);
        ow["TropicalPlant"] = HasDeco(Decoration3.TROPICAL_PLANT);
        ow["PrettyFlowers"] = HasDeco(Decoration3.PRETTY_FLOWERS);
        ow["ColorfulPlant"] = HasDeco(Decoration3.COLORFUL_PLANT);
        ow["BigPlant"] = HasDeco(Decoration3.BIG_PLANT);
        ow["GorgeousPlant"] = HasDeco(Decoration3.GORGEOUS_PLANT);
        ow["MudBall"] = HasDeco(Decoration3.MUD_BALL);
        ow["BreakableDoor"] = HasDeco(Decoration3.BREAKABLE_DOOR);
        ow["SandOrnament"] = HasDeco(Decoration3.SAND_ORNAMENT);
        ow["FenceLength"] = HasDeco(Decoration3.FENCE_LENGTH);
        ow["FenceWidth"] = HasDeco(Decoration3.FENCE_WIDTH);
        ow["TV"] = HasDeco(Decoration3.TV);
        ow["RoundTV"] = HasDeco(Decoration3.ROUND_TV);
        ow["CuteTV"] = HasDeco(Decoration3.CUTE_TV);
        ow["SilverShield"] = HasDeco(Decoration3.SILVER_SHIELD);
        ow["GoldShield"] = HasDeco(Decoration3.GOLD_SHIELD);
        ow["GlassOrnament"] = HasDeco(Decoration3.GLASS_ORNAMENT);
        ow["Tire"] = HasDeco(Decoration3.TIRE);
        ow["SolidBoard"] = HasDeco(Decoration3.SOLID_BOARD);
        ow["Stand"] = HasDeco(Decoration3.STAND);
        ow["Slide"] = HasDeco(Decoration3.SLIDE);
        ow["RedBrick"] = HasDeco(Decoration3.RED_BRICK);
        ow["YellowBrick"] = HasDeco(Decoration3.YELLOW_BRICK);
        ow["BlueBrick"] = HasDeco(Decoration3.BLUE_BRICK);
        ow["RedBalloon"] = HasDeco(Decoration3.RED_BALLOON);
        ow["YellowBalloon"] = HasDeco(Decoration3.YELLOW_BALLOON);
        ow["BlueBalloon"] = HasDeco(Decoration3.BLUE_BALLOON);
        ow["RedTent"] = HasDeco(Decoration3.RED_TENT);
        ow["BlueTent"] = HasDeco(Decoration3.BLUE_TENT);
        ow["AzurillDoll"] = HasDeco(Decoration3.AZURILL_DOLL);
        ow["DollBaltoy"] = HasDeco(Decoration3.BALTOY_DOLL);
        ow["DollChikorita"] = HasDeco(Decoration3.CHIKORITA_DOLL);
        ow["DollClefairy"] = HasDeco(Decoration3.CLEFAIRY_DOLL);
        ow["DollCyndaquil"] = HasDeco(Decoration3.CYNDAQUIL_DOLL);
        ow["DollDitto"] = HasDeco(Decoration3.DITTO_DOLL);
        ow["DollDuskull"] = HasDeco(Decoration3.DUSKULL_DOLL);
        ow["DollGulpin"] = HasDeco(Decoration3.GULPIN_DOLL);
        ow["DollJigglypuff"] = HasDeco(Decoration3.JIGGLYPUFF_DOLL);
        ow["DollKecleon"] = HasDeco(Decoration3.KECLEON_DOLL);
        ow["DollLotad"] = HasDeco(Decoration3.LOTAD_DOLL);
        ow["DollMarill"] = HasDeco(Decoration3.MARILL_DOLL);
        ow["DollMeowth"] = HasDeco(Decoration3.MEOWTH_DOLL);
        ow["DollMudkip"] = HasDeco(Decoration3.MUDKIP_DOLL);
        ow["DollPichu"] = HasDeco(Decoration3.PICHU_DOLL);
        ow["DollPikachu"] = HasDeco(Decoration3.PIKACHU_DOLL);
        ow["DollSeedot"] = HasDeco(Decoration3.SEEDOT_DOLL);
        ow["DollSkitty"] = HasDeco(Decoration3.SKITTY_DOLL);
        ow["DollSmoochum"] = HasDeco(Decoration3.SMOOCHUM_DOLL);
        ow["DollSwablu"] = HasDeco(Decoration3.SWABLU_DOLL);
        ow["DollTogepi"] = HasDeco(Decoration3.TOGEPI_DOLL);
        ow["DollTorchic"] = HasDeco(Decoration3.TORCHIC_DOLL);
        ow["DollTotodile"] = HasDeco(Decoration3.TOTODILE_DOLL);
        ow["DollTreecko"] = HasDeco(Decoration3.TREECKO_DOLL);
        ow["DollWynaut"] = HasDeco(Decoration3.WYNAUT_DOLL);
        ow["DollBlastoise"] = HasDeco(Decoration3.BLASTOISE_DOLL);
        ow["DollCharizard"] = HasDeco(Decoration3.CHARIZARD_DOLL);
        ow["DollLapras"] = HasDeco(Decoration3.LAPRAS_DOLL);
        ow["DollRhydon"] = HasDeco(Decoration3.RHYDON_DOLL);
        ow["DollSnorlax"] = HasDeco(Decoration3.SNORLAX_DOLL);
        ow["DollVenusaur"] = HasDeco(Decoration3.VENUSAUR_DOLL);
        ow["DollWailmer"] = HasDeco(Decoration3.WAILMER_DOLL);
        ow["RegirockDoll"] = HasDeco(Decoration3.REGIROCK_DOLL);
        ow["RegiceDoll"] = HasDeco(Decoration3.REGICE_DOLL);
        ow["RegisteelDoll"] = HasDeco(Decoration3.REGISTEEL_DOLL);
        ow["BallPoster"] = HasDeco(Decoration3.BALL_POSTER);
        ow["BluePoster"] = HasDeco(Decoration3.BLUE_POSTER);
        ow["CutePoster"] = HasDeco(Decoration3.CUTE_POSTER);
        ow["GreenPoster"] = HasDeco(Decoration3.GREEN_POSTER);
        ow["RedPoster"] = HasDeco(Decoration3.RED_POSTER);
        ow["LongPoster"] = HasDeco(Decoration3.LONG_POSTER);
        ow["PikaPoster"] = HasDeco(Decoration3.PIKA_POSTER);
        ow["SeaPoster"] = HasDeco(Decoration3.SEA_POSTER);
        ow["SkyPoster"] = HasDeco(Decoration3.SKY_POSTER);
        ow["KissPoster"] = HasDeco(Decoration3.KISS_POSTER);
    }

    public void Generate_inGameTrade()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameTrade"] = ow;

        ow["RaltsforSeedot"] = sav.GetEventFlag(0x0099);
        ow["VolbeatforPlusle"] = sav.GetEventFlag(0x009B);
        ow["BagonforHorsea"] = sav.GetEventFlag(0x009A);
        ow["SkittyforMeowth"] = sav.GetEventFlag(0x009C);

    }
    public void Generate_inGameGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameGift"] = ow;
        
        ow["StarterPokemon"] = true;
        ow["WynautEgg"] = sav.GetEventFlag(0x010A);
        ow["LileeporAnorith"] = sav.GetEventFlag(0x010B);

        ow["Castform"] = sav.GetEventFlag(0x0097);
        ow["Beldum"] = sav.GetEventFlag(0x012A);
        ow["ChikoritaCyndaquilorTotodile"] = sav.GetEventFlag(0x0346) || sav.GetEventFlag(0x032C) || sav.GetEventFlag(0x032B);
    }
    public void Generate_battleFrontier()
    {
        var ow = new Dictionary<string, bool>();
        owned["battleFrontier"] = ow;

        ow["SilverKnowledgeSymbol"] = sav.GetEventFlag(2252);
        ow["SilverGutsSymbol"] = sav.GetEventFlag(2250);
        ow["SilverTacticsSymbol"] = sav.GetEventFlag(2246);
        ow["SilverLuckSymbol"] = sav.GetEventFlag(2254);
        ow["SilverSpiritsSymbol"] = sav.GetEventFlag(2248);
        ow["SilverBraveSymbol"] = sav.GetEventFlag(2256);
        ow["SilverAbilitySymbo"] = sav.GetEventFlag(2244);

        ow["GoldKnowledgeSymbol"] = sav.GetEventFlag(2253);
        ow["GoldGutsSymbol"] = sav.GetEventFlag(2251);
        ow["GoldTacticsSymbol"] = sav.GetEventFlag(2247);
        ow["GoldLuckSymbol"] = sav.GetEventFlag(2255);
        ow["GoldSpiritsSymbol"] = sav.GetEventFlag(2249);
        ow["GoldBraveSymbol"] = sav.GetEventFlag(2257);
        ow["GoldAbilitySymbol"] = sav.GetEventFlag(2245);
    }

    public bool HasRibbon(Func<PK3, bool> func)
    {
        var pkms = sav.GetAllPKM();
        return pkms.Any(pk =>
        {
            var pk3 = (PK3?)pk;
            if (pk3 == null)
                return false;
            return func(pk3);
        });
    }
    public bool HasContestRibbon(Func<PK3, byte> func, byte wantedVal)
    {
        return HasRibbon(pk =>
        {
            var savVal = func(pk);
            if (this.living)
                return savVal == wantedVal;
            return savVal >= wantedVal;
        });
    }

    public void Generate_ribbon()
    {
        var ow = new Dictionary<string, bool>();
        owned["ribbon"] = ow;

        ow["ChampionRibbon"] = HasRibbon(p => p.RibbonChampionBattle);
        ow["CoolRibbon"] = HasContestRibbon(p => p.RibbonCountG3Cool, 1);
        ow["CoolRibbonSuper"] = HasContestRibbon(p => p.RibbonCountG3Cool, 2);
        ow["CoolRibbonHyper"] = HasContestRibbon(p => p.RibbonCountG3Cool, 3);
        ow["CoolRibbonMaster"] = HasContestRibbon(p => p.RibbonCountG3Cool, 4);
        ow["BeautyRibbon"] = HasContestRibbon(p => p.RibbonCountG3Beauty, 1);
        ow["BeautyRibbonSuper"] = HasContestRibbon(p => p.RibbonCountG3Beauty, 2);
        ow["BeautyRibbonHyper"] = HasContestRibbon(p => p.RibbonCountG3Beauty, 3);
        ow["BeautyRibbonMaster"] = HasContestRibbon(p => p.RibbonCountG3Beauty, 4);
        ow["CuteRibbon"] = HasContestRibbon(p => p.RibbonCountG3Cute, 1);
        ow["CuteRibbonSuper"] = HasContestRibbon(p => p.RibbonCountG3Cute, 2);
        ow["CuteRibbonHyper"] = HasContestRibbon(p => p.RibbonCountG3Cute, 3);
        ow["CuteRibbonMaster"] = HasContestRibbon(p => p.RibbonCountG3Cute, 4);
        ow["SmartRibbon"] = HasContestRibbon(p => p.RibbonCountG3Smart, 1);
        ow["SmartRibbonSuper"] = HasContestRibbon(p => p.RibbonCountG3Smart, 2);
        ow["SmartRibbonHyper"] = HasContestRibbon(p => p.RibbonCountG3Smart, 3);
        ow["SmartRibbonMaster"] = HasContestRibbon(p => p.RibbonCountG3Smart, 4);
        ow["ToughRibbon"] = HasContestRibbon(p => p.RibbonCountG3Tough, 1);
        ow["ToughRibbonSuper"] = HasContestRibbon(p => p.RibbonCountG3Tough, 2);
        ow["ToughRibbonHyper"] = HasContestRibbon(p => p.RibbonCountG3Tough, 3);
        ow["ToughRibbonMaster"] = HasContestRibbon(p => p.RibbonCountG3Tough, 4);
        ow["WinningRibbon"] = HasRibbon(p => p.RibbonWinning);
        ow["VictoryRibbon"] = HasRibbon(p => p.RibbonVictory);
        ow["ArtistRibbon"] = HasRibbon(p => p.RibbonArtist);
        ow["EffortRibbon"] = HasRibbon(p => p.RibbonEffort);
        ow["CountryRibbon"] = HasRibbon(p => p.RibbonCountry);
        ow["NationalRibbon"] = HasRibbon(p => p.RibbonNational);
        ow["EarthRibbon"] = HasRibbon(p => p.RibbonEarth);
        ow["WorldRibbon"] = HasRibbon(p => p.RibbonWorld);
    }

    public bool HasPhone()
    {
        return true;
        //NO_PROD
    }

    public void Generate_phone()
    {
        var ow = new Dictionary<string, bool>();
        owned["phone"] = ow;

        ow["RadNeighborBrendan"] = HasPhone();
        ow["RadNeighborMay"] = HasPhone();
        ow["TriathleteAbigail"] = HasPhone();
        ow["TwinsAmyLiv"] = HasPhone();
        ow["RuinManiacAndres"] = HasPhone();
        ow["SrandJrAnnaMeg"] = HasPhone();
        ow["TriathleteBenjamin"] = HasPhone();
        ow["KindlerBernie"] = HasPhone();
        ow["ManiacJeffrey"] = HasPhone();
        ow["TheBigHitBrawly"] = HasPhone();
        ow["CooltrainerBrooke"] = HasPhone();
        ow["YoungsterCalvin"] = HasPhone();
        ow["PsychicCameron"] = HasPhone();
        ow["PKMNRangerCatherine"] = HasPhone();
        ow["LadyCindy"] = HasPhone();
        ow["SailorCory"] = HasPhone();
        ow["CooltrainerCristin"] = HasPhone();
        ow["BattleGirlCyndy"] = HasPhone();
        ow["GuitaristDalton"] = HasPhone();
        ow["PicnickerDiana"] = HasPhone();
        ow["EliteFourDrake"] = HasPhone();
        ow["RuinManiacDusty"] = HasPhone();
        ow["TriathleteDylan"] = HasPhone();
        ow["CollectorEdwin"] = HasPhone();
        ow["FishermanElliot"] = HasPhone();
        ow["SailorErnest"] = HasPhone();
        ow["CamperEthan"] = HasPhone();
        ow["GuitaristFernando"] = HasPhone();
        ow["PassionBurnFlannery"] = HasPhone();
        ow["PKMNBreederGabrielle"] = HasPhone();
        ow["EliteFourGlacia"] = HasPhone();
        ow["LassHaley"] = HasPhone();
        ow["PKMNBreederIsaac"] = HasPhone();
        ow["PokefanIsabel"] = HasPhone();
        ow["TriathleteIsaiah"] = HasPhone();
        ow["PsychicJacki"] = HasPhone();
        ow["PKMNRangerJackson"] = HasPhone();
        ow["BugCatcherJames"] = HasPhone();
        ow["SwimmerJenny"] = HasPhone();
        ow["SchoolKidJerry"] = HasPhone();
        ow["BeautyJessica"] = HasPhone();
        ow["OldCoupleJohnJay"] = HasPhone();
        ow["DandyCharmJuan"] = HasPhone();
        ow["SchoolKidKaren"] = HasPhone();
        ow["TriathleteKatelyn"] = HasPhone();
        ow["BlackBeltKoji"] = HasPhone();
        ow["NinjaBoyLao"] = HasPhone();
        ow["MysticDuoTateLiza"] = HasPhone();
        ow["YoungCoupleKiraDan"] = HasPhone();
        ow["TuberLola"] = HasPhone();
        ow["PKMNBreederLydia"] = HasPhone();
        ow["ParasolLadyMadeline"] = HasPhone();
        ow["TriathleteMaria"] = HasPhone();
        ow["PokefanMiguel"] = HasPhone();
        ow["CalmKindMom"] = HasPhone();
        ow["DevonPresMrStone"] = HasPhone();
        ow["DragonTamerNicolas"] = HasPhone();
        ow["BlackBeltNob"] = HasPhone();
        ow["ReliableOneDad"] = HasPhone();
        ow["TriathletePablo"] = HasPhone();
        ow["EliteFourPhoebe"] = HasPhone();
        ow["PKMNProfProfBirch"] = HasPhone();
        ow["TuberRicky"] = HasPhone();
        ow["SisandBroLilaRoy"] = HasPhone();
        ow["BirdKeeperRobert"] = HasPhone();
        ow["AromaLadyRose"] = HasPhone();
        ow["RockinWhizRoxanne"] = HasPhone();
        ow["HikerSawyer"] = HasPhone();
        ow["ElusiveEyesScott"] = HasPhone();
        ow["ExpertShelby"] = HasPhone();
        ow["EliteFourSidney"] = HasPhone();
        ow["PokeManiacSteve"] = HasPhone();
        ow["HardasRockSteven"] = HasPhone();
        ow["BeautyThaliaE"] = HasPhone();
        ow["ExpertTimothy"] = HasPhone();
        ow["SwimmerTony"] = HasPhone();
        ow["HikerTrent"] = HasPhone();
        ow["HexManiacValerie"] = HasPhone();
        ow["ChampionWallace"] = HasPhone();
        ow["PKMNLoverWally"] = HasPhone();
        ow["GentlemanWalter"] = HasPhone();
        ow["SwellShockWattson"] = HasPhone();
        ow["CooltrainerWilton"] = HasPhone();
        ow["SkyTamerWinona"] = HasPhone();
        ow["RichBoyWinston"] = HasPhone();
    }
    public void Generate_trainerStar()
    {
        var ow = new Dictionary<string, bool>();
        owned["trainerStar"] = ow;

        ow["HallofFame"] = sav.GetEventFlag(2148);
        ow["HoennPokedex"] = true; //NO_PROD

        ow["PokemonContests"] = HasDeco(Decoration3.GLASS_ORNAMENT);
        ow["BattleTower"] = sav.GetEventFlag(0x01D2);
    }

    
    public void Generate_eReaderBattles()
    {
        var ow = new Dictionary<string, bool>();
        owned["eReaderBattles"] = ow;

        // None for US version
    }
    public bool HasPokeblock(PokeBlock3Color color)
    {
        return sav.PokeBlocks.Blocks.Any(b => b.Color == color);
    }

    public void Generate_pokeblock()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokeblock"] = ow;

        ow["BlackPokeblock"] = HasPokeblock(PokeBlock3Color.Black);
        ow["RedPokeblock"] = HasPokeblock(PokeBlock3Color.Red);
        ow["BluePokeblock"] = HasPokeblock(PokeBlock3Color.Blue);
        ow["PinkPokeblock"] = HasPokeblock(PokeBlock3Color.Pink);
        ow["GreenPokeblock"] = HasPokeblock(PokeBlock3Color.Green);
        ow["YellowPokeblock"] = HasPokeblock(PokeBlock3Color.Yellow);
        ow["GoldPokeblock"] = HasPokeblock(PokeBlock3Color.Gold);
        ow["PurplePokeblock"] = HasPokeblock(PokeBlock3Color.Purple);
        ow["IndigoPokeblock"] = HasPokeblock(PokeBlock3Color.Indigo);
        ow["LiteBluePokeblock"] = HasPokeblock(PokeBlock3Color.LightBlue);
        ow["BrownPokeblock"] = HasPokeblock(PokeBlock3Color.Brown);
        ow["OlivePokeblock"] = HasPokeblock(PokeBlock3Color.Olive);
        ow["GrayPokeblock"] = HasPokeblock(PokeBlock3Color.Gray);
        ow["WhitePokeblock"] = HasPokeblock(PokeBlock3Color.White);
    }

    public void Generate_easyChatSystemWord()
    {
        var ow = new Dictionary<string, bool>();
        owned["easyChatSystemWord"] = ow;

        ow["RegionalPokemonNames"] = true;
        ow["NonRegionalPokemonNames"] = true;
        ow["Events"] = true;
        ow["Move1"] = true;
        ow["Move2"] = true;
        ow["TrendyKTHXBYE"] = true;
        ow["TrendyYESSIR"] = true;
        ow["TrendyAVANTGARDE"] = true;
        ow["TrendyCOUPLE"] = true;
        ow["TrendyMUCHOBLIGED"] = true;
        ow["TrendyYEEHAW"] = true;
        ow["TrendyMEGA"] = true;
        ow["Trendy1HITKO"] = true;
        ow["TrendyDESTINY"] = true;
        ow["TrendyCANCEL"] = true;
        ow["TrendyNEW"] = true;
        ow["TrendyFLATTEN"] = true;
        ow["TrendyKIDDING"] = true;
        ow["TrendyLOSER"] = true;
        ow["TrendyLOSING"] = true;
        ow["TrendyHAPPENING"] = true;
        ow["TrendyHIPAND"] = true;
        ow["TrendySHAKE"] = true;
        ow["TrendySHADY"] = true;
        ow["TrendyUPBEAT"] = true;
        ow["TrendyMODERN"] = true;
        ow["TrendySMELLYA"] = true;
        ow["TrendyBANG"] = true;
        ow["TrendyKNOCKOUT"] = true;
        ow["TrendyHASSLE"] = true;
        ow["TrendyWINNER"] = true;
        ow["TrendyFEVER"] = true;
        ow["TrendyWANNABE"] = true;
        ow["TrendyBABY"] = true;
        ow["TrendyHEART"] = true;
        ow["TrendyOLD"] = true;
        ow["TrendyYOUNG"] = true;
        ow["TrendyUGLY"] = true;
    }


    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;

        ow["DefeatSteven"] = true;
        ow["BeatSlateportBattleTent"] = true;
        ow["BeatVerdanturfBattleTent"] = true;
        ow["BeatFallarborBattleTent"] = true;
        ow["BeatTrainerHillNormalMode"] = true;
        ow["BeatTrainerHillVarietyMode"] = true;
        ow["BeatTrainerHillUniqueMode"] = true;
        ow["BeatTrainerHillExpertMode"] = true;
        ow["UnlockMysteryGiftsystem"] = true;
    }
}