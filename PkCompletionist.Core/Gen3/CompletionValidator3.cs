using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace PkCompletionist.Core;

internal class CompletionValidator3 : CompletionValidatorX
{
    public CompletionValidator3(Command command, SAV3 sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;
        this.unobtainableItems = new List<int>() { 44, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 72, 82, 87, 88, 89, 90, 91, 92, 99, 100, 101, 102, 105, 112, 113, 114, 115, 116, 117, 118, 119, 120, 176, 177, 178, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 266, 267, 276, 277, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 373, 374 };
        // more than that if us-only
    }

    new SAV3 sav;

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

    public void Generate_decoration()
    {
        var ow = new Dictionary<string, bool>();
        owned["decoration"] = ow;

        /*
        ow["SmallChair"] = sav.GetEventFlag();
        ow["PokmonChair"] = sav.GetEventFlag();
        ow["HeavyChair"] = sav.GetEventFlag();
        ow["RaggedChair"] = sav.GetEventFlag();
        ow["ComfortChair"] = sav.GetEventFlag();
        ow["BrickChair"] = sav.GetEventFlag();
        ow["CampChair"] = sav.GetEventFlag();
        ow["HardChair"] = sav.GetEventFlag();
        ow["PrettyChair"] = sav.GetEventFlag();
        ow["PikaCushion"] = sav.GetEventFlag();
        ow["RoundCushion"] = sav.GetEventFlag();
        ow["ZigzagCushion"] = sav.GetEventFlag();
        ow["SpinCushion"] = sav.GetEventFlag();
        ow["DiamondCushion"] = sav.GetEventFlag();
        ow["BallCushion"] = sav.GetEventFlag();
        ow["GrassCushion"] = sav.GetEventFlag();
        ow["FireCushion"] = sav.GetEventFlag();
        ow["WaterCushion"] = sav.GetEventFlag();
        ow["KissCushion"] = sav.GetEventFlag();
        ow["SmallDesk"] = sav.GetEventFlag();
        ow["PokmonDesk"] = sav.GetEventFlag();
        ow["HeavyDesk"] = sav.GetEventFlag();
        ow["RaggedDesk"] = sav.GetEventFlag();
        ow["ComfortDesk"] = sav.GetEventFlag();
        ow["BrickDesk"] = sav.GetEventFlag();
        ow["CampDesk"] = sav.GetEventFlag();
        ow["HardDesk"] = sav.GetEventFlag();
        ow["PrettyDesk"] = sav.GetEventFlag();
        ow["CLowNoteMat"] = sav.GetEventFlag();
        ow["DNoteMat"] = sav.GetEventFlag();
        ow["ENoteMat"] = sav.GetEventFlag();
        ow["FNoteMat"] = sav.GetEventFlag();
        ow["GNoteMat"] = sav.GetEventFlag();
        ow["ANoteMat"] = sav.GetEventFlag();
        ow["BNoteMat"] = sav.GetEventFlag();
        ow["CHighNoteMat"] = sav.GetEventFlag();
        ow["GlitterMat"] = sav.GetEventFlag();
        ow["JumpMat"] = sav.GetEventFlag();
        ow["SpinMat"] = sav.GetEventFlag();
        ow["SurfMat"] = sav.GetEventFlag();
        ow["ThunderMat"] = sav.GetEventFlag();
        ow["FireBlastMat"] = sav.GetEventFlag();
        ow["PowderSnowMat"] = sav.GetEventFlag();
        ow["AttractMat"] = sav.GetEventFlag();
        ow["FissureMat"] = sav.GetEventFlag();
        ow["SpikesMat"] = sav.GetEventFlag();
        ow["RedPlant"] = sav.GetEventFlag();
        ow["TropicalPlant"] = sav.GetEventFlag();
        ow["PrettyFlowers"] = sav.GetEventFlag();
        ow["ColorfulPlant"] = sav.GetEventFlag();
        ow["BigPlant"] = sav.GetEventFlag();
        ow["GorgeousPlant"] = sav.GetEventFlag();
        ow["MudBall"] = sav.GetEventFlag();
        ow["BreakableDoor"] = sav.GetEventFlag();
        ow["SandOrnament"] = sav.GetEventFlag();
        ow["FenceLength"] = sav.GetEventFlag();
        ow["FenceWidth"] = sav.GetEventFlag();
        ow["TV"] = sav.GetEventFlag();
        ow["RoundTV"] = sav.GetEventFlag();
        ow["CuteTV"] = sav.GetEventFlag();
        ow["SilverShield"] = sav.GetEventFlag();
        ow["GoldShield"] = sav.GetEventFlag();
        ow["GlassOrnament"] = sav.GetEventFlag();
        ow["Tire"] = sav.GetEventFlag();
        ow["SolidBoard"] = sav.GetEventFlag();
        ow["Stand"] = sav.GetEventFlag();
        ow["Slide"] = sav.GetEventFlag();
        ow["RedBrick"] = sav.GetEventFlag();
        ow["YellowBrick"] = sav.GetEventFlag();
        ow["BlueBrick"] = sav.GetEventFlag();
        ow["RedBalloon"] = sav.GetEventFlag();
        ow["YellowBalloon"] = sav.GetEventFlag();
        ow["BlueBalloon"] = sav.GetEventFlag();
        ow["RedTent"] = sav.GetEventFlag();
        ow["BlueTent"] = sav.GetEventFlag();
        ow["AzurillDoll"] = sav.GetEventFlag();
        ow["DollBaltoy"] = sav.GetEventFlag();
        ow["DollChikorita"] = sav.GetEventFlag();
        ow["DollClefairy"] = sav.GetEventFlag();
        ow["DollCyndaquil"] = sav.GetEventFlag();
        ow["DollDitto"] = sav.GetEventFlag();
        ow["DollDuskull"] = sav.GetEventFlag();
        ow["DollGulpin"] = sav.GetEventFlag();
        ow["DollJigglypuff"] = sav.GetEventFlag();
        ow["DollKecleon"] = sav.GetEventFlag();
        ow["DollLotad"] = sav.GetEventFlag();
        ow["DollMarill"] = sav.GetEventFlag();
        ow["DollMeowth"] = sav.GetEventFlag();
        ow["DollMudkip"] = sav.GetEventFlag();
        ow["DollPichu"] = sav.GetEventFlag();
        ow["DollPikachu"] = sav.GetEventFlag();
        ow["DollSeedot"] = sav.GetEventFlag();
        ow["DollSkitty"] = sav.GetEventFlag();
        ow["DollSmoochum"] = sav.GetEventFlag();
        ow["DollSwablu"] = sav.GetEventFlag();
        ow["DollTogepi"] = sav.GetEventFlag();
        ow["DollTorchic"] = sav.GetEventFlag();
        ow["DollTotodile"] = sav.GetEventFlag();
        ow["DollTreecko"] = sav.GetEventFlag();
        ow["DollWynaut"] = sav.GetEventFlag();
        ow["DollBlastoise"] = sav.GetEventFlag();
        ow["DollCharizard"] = sav.GetEventFlag();
        ow["DollLapras"] = sav.GetEventFlag();
        ow["DollRhydon"] = sav.GetEventFlag();
        ow["DollSnorlax"] = sav.GetEventFlag();
        ow["DollVenusaur"] = sav.GetEventFlag();
        ow["DollWailmer"] = sav.GetEventFlag();
        ow["RegirockDoll"] = sav.GetEventFlag();
        ow["RegiceDoll"] = sav.GetEventFlag();
        ow["RegisteelDoll"] = sav.GetEventFlag();
        ow["BallPoster"] = sav.GetEventFlag();
        ow["BluePoster"] = sav.GetEventFlag();
        ow["CutePoster"] = sav.GetEventFlag();
        ow["GreenPoster"] = sav.GetEventFlag();
        ow["RedPoster"] = sav.GetEventFlag();
        ow["LongPoster"] = sav.GetEventFlag();
        ow["PikaPoster"] = sav.GetEventFlag();
        ow["SeaPoster"] = sav.GetEventFlag();
        ow["SkyPoster"] = sav.GetEventFlag();
        ow["KissPoster"] = sav.GetEventFlag();
        */
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
}