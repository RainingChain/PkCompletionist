using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers.Binary;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using static PkCompletionist.Core.CompletionValidator4;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PkCompletionist.Core;

internal class CompletionValidator4 : CompletionValidatorX
{
    public CompletionValidator4(Command command, SAV4Pt sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;

        // Note that in US game, there are more unobtainable items than that
        this.unobtainableItems = new List<int>() { };

    }

    new SAV4Pt sav;

    public override void GenerateAll()
    {
        base.GenerateAll();

        Generate_pokemonForm();
        Generate_poketch();
        Generate_inGameTrade();
        Generate_inGameGift();
        Generate_battleFrontier();

        Generate_decoration();
        Generate_undergroundSphere();
        Generate_ribbon();
        Generate_backdrop();

        Generate_pokeballSeal();
        Generate_accessory();
        Generate_poffin();
        Generate_pcWallPaper();

        Generate_villaFurniture();
        Generate_trainerStar();
        Generate_easyChatSystemWord();
        Generate_misc();
    }

    public void Generate_pokemonForm()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemonForm"] = ow;

        ow["UnownA"] = HasPkmForm(201, 0);
        ow["UnownB"] = HasPkmForm(201, 1);
        ow["UnownC"] = HasPkmForm(201, 2);
        ow["UnownD"] = HasPkmForm(201, 3);
        ow["UnownE"] = HasPkmForm(201, 4);
        ow["UnownF"] = HasPkmForm(201, 5);
        ow["UnownG"] = HasPkmForm(201, 6);
        ow["UnownH"] = HasPkmForm(201, 7);
        ow["UnownI"] = HasPkmForm(201, 8);
        ow["UnownJ"] = HasPkmForm(201, 9);
        ow["UnownK"] = HasPkmForm(201, 10);
        ow["UnownL"] = HasPkmForm(201, 11);
        ow["UnownM"] = HasPkmForm(201, 12);
        ow["UnownN"] = HasPkmForm(201, 13);
        ow["UnownO"] = HasPkmForm(201, 14);
        ow["UnownP"] = HasPkmForm(201, 15);
        ow["UnownQ"] = HasPkmForm(201, 16);
        ow["UnownR"] = HasPkmForm(201, 17);
        ow["UnownS"] = HasPkmForm(201, 18);
        ow["UnownT"] = HasPkmForm(201, 19);
        ow["UnownU"] = HasPkmForm(201, 20);
        ow["UnownV"] = HasPkmForm(201, 21);
        ow["UnownW"] = HasPkmForm(201, 22);
        ow["UnownX"] = HasPkmForm(201, 23);
        ow["UnownY"] = HasPkmForm(201, 24);
        ow["UnownZ"] = HasPkmForm(201, 25);
        ow["UnownExclamationMark"] = HasPkmForm(201, 26);
        ow["UnownQuestionMark"] = HasPkmForm(201, 27);

        ow["CastformNormal"] = HasPkm(351);
        ow["CastformFire"] = HasPkm(351);
        ow["CastformWater"] = HasPkm(351);
        ow["CastformIce"] = HasPkm(351);

        ow["DeoxysNormal"] = HasPkmForm(386, 0);
        ow["DeoxysAttack"] = HasPkmForm(386, 1); 
        ow["DeoxysDefense"] = HasPkmForm(386, 2);
        ow["DeoxysSpeed"] = HasPkmForm(386, 3);
        ow["BurmyPlantCloak"] = HasPkmForm(412, 0); 
        ow["BurmySandyCloak"] = HasPkmForm(412, 1);
        ow["BurmyTrashCloak"] = HasPkmForm(412, 2);
        ow["WormadamPlantCloak"] = HasPkmForm(413, 0);
        ow["WormadamSandyCloak"] = HasPkmForm(413, 1);
        ow["WormadamTrashCloak"] = HasPkmForm(413, 2);
        ow["CherrimOvercast"] = false; // TODO
        ow["CherrimSunshine"] = false; // TODO
        ow["ShellosWestSea"] = false; // TODO
        ow["ShellosEastSea"] = false; // TODO
        ow["GastrodonWestSea"] = false; // TODO
        ow["GastrodonEastSea"] = false; // TODO
        ow["RotomGhost"] = false; // TODO
        ow["RotomHeat"] = false; // TODO
        ow["RotomWash"] = false; // TODO
        ow["RotomFrost"] = false; // TODO
        ow["RotomFan"] = false; // TODO
        ow["RotomMow"] = false; // TODO
        ow["GiratinaAltered"] = false; // TODO
        ow["GiratinaOrigin"] = false; // TODO
        ow["ShayminLand"] = false; // TODO
        ow["ShayminSky"] = false; // TODO
    }

    public override void Generate_item()
    {
        base.Generate_item();

        var ow = this.owned["item"];
    }


    public void Generate_poketch()
    {
        var ow = this.owned["poketch"];

        ow["DigitalWatch"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Digital_Watch);
        ow["Calculator"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Calculator);
        ow["MemoPad"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Memo_Pad);
        ow["Pedometer"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Pedometer);
        ow["Party"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Party);
        ow["FriendshipChecker"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Friendship_Checker);
        ow["DowsingMachine"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Dowsing_Machine);
        ow["BerrySearcher"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Berry_Searcher);
        ow["Daycare"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Daycare);
        ow["History"] = this.sav.GetPoketchAppUnlocked(PoketchApp.History);
        ow["Counter"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Counter);
        ow["AnalogWatch"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Analog_Watch);
        ow["MarkingMap"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Marking_Map);
        ow["LinkSearcher"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Link_Searcher);
        ow["CoinToss"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Coin_Toss);
        ow["MoveTester"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Move_Tester);
        ow["Calendar"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Calendar);
        ow["DotArtist"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Dot_Artist);
        ow["Roulette"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Roulette);
        ow["TrainerCounter"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Trainer_Counter);
        ow["KitchenTimer"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Kitchen_Timer);
        ow["ColorChanger"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Color_Changer);
        ow["MatchupChecker"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Matchup_Checker);
        ow["Stopwatch"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Stopwatch);
        ow["AlarmClock"] = this.sav.GetPoketchAppUnlocked(PoketchApp.Alarm_Clock);
    }


    public enum DECO4 : byte
    {
        None,
        unk0,
        unk1,
        unk2,
        unk3,
        unk4,
        unk5,
        YellowCushion,
        BlueCushion,
        WoodenChair,
        BigTable,
        PlainTable,
        SmallTable,
        LongTable,
        WideTable,
        PokeTable,
        BigBookshelf,
        SmallBookshelf,
        ResearchShelf,
        BikeRack,
        ShopShelf,
        DisplayShelf,
        Cupboard,
        WoodDresser,
        PinkDresser,
        TV,
        Refrigerator,
        PrettySink,
        FeatheryBed,
        TrashCan,
        CardboardBox,
        Crate,
        Container,
        OilDrum,
        BigOilDrum,
        IronBeam,
        PottedPlant,
        PokeFlower,
        HealingMachine,
        LabMachine,
        TestMachine,
        GameSystem,
        VendingMachine,
        RedBike,
        GreenBike,
        Binoculars,
        Globe,
        GymStatue,
        RedTent,
        BlueTent,
        ClearTent,
        MazeBlock1,
        MazeBlock2,
        MazeBlock3,
        MazeBlock4,
        MazeBlock5,
        HoleTool,
        PitTool,
        SmokeTool,
        BigSmokeTool,
        RockTool,
        RockfallTool,
        FoamTool,
        BubbleTool,
        AlertTool1,
        AlertTool2,
        AlertTool3,
        AlertTool4,
        LeafTool,
        FlowerTool,
        EmberTool,
        FireTool,
        CuteCup,
        CoolCup,
        BeautyCup,
        ToughCup,
        CleverCup,
        BlueCrystal,
        PinkCrystal,
        RedCrystal,
        YellowCrystal,
        PrettyGem,
        ShinyGem,
        MysticGem,
        GlitterGem,
        BronzeTrophy,
        SilverTrophy,
        GoldTrophy,
        GreatTrophy,
        BallOrnament,
        RoundOrnament,
        ClearOrnament,
        CHARMANDERDoll,
        BULBASAURDoll,
        SQUIRTLEDoll,
        CYNDAQUILDoll,
        CHIKORITADoll,
        TOTODILEDoll,
        TORCHICDoll,
        TREECKODoll,
        MUDKIPDoll,
        CHIMCHARDoll,
        TURTWIGDoll,
        PIPLUPDoll,
        PIKACHUDoll,
        PLUSLEDoll,
        MINUNDoll,
        CLEFAIRYDoll,
        JIGGLYPUFFDoll,
        WOBBUFFETDoll,
        MEOWTHDoll,
        SKITTYDoll,
        GLAMEOWDoll,
        BUNEARYDoll,
        WEAVILEDoll,
        MUNCHLAXDoll,
        BONSLYDoll,
        MIMEJRDoll,
        LUCARIODoll,
        MANTYKEDoll,
        BUIZELDoll,
        CHATOTDoll,
        MANAPHYDoll,
        SNORLAXDoll,
        WAILORDDoll,
        MAGNEZONEDoll,
        DRIFLOONDoll,
        HAPPINYDoll,
        PACHIRISUDoll,
        unk6,
        unk7,
        unk8,
        unk9,
        WideSofa,
        Bonsai,
        DaintyFlowers,
        LovelyFlowers,
        PrettyFlowers,
        LavishFlowers
    }
    public enum TRAP4
    {
        None,
        MoveTrapUp,
        MoveTrapRight,
        MoveTrapDown,
        MoveTrapLeft,
        HurlTrapUp,
        HurlTrapRight,
        HurlTrapDown,
        HurlTrapLeft,
        WarpTrap,
        HiWarpTrap,
        HoleTrap,
        PitTrap,
        ReverseTrap,
        ConfuseTrap,
        RunTrap,
        FadeTrap,
        SlowTrap,
        SmokeTrap,
        BigSmokeTrap,
        RockTrap,
        RockfallTrap,
        FoamTrap,
        BubbleTrap,
        AlertTrap1,
        AlertTrap2,
        AlertTrap3,
        AlertTrap4,
        LeafTrap,
        FlowerTrap,
        EmberTrap,
        FireTrap,
        RadarTrap,
        DiggerDrill
    }

    public void Generate_decoration()
    {
        var ow = new Dictionary<string, bool>();
        owned["decoration"] = ow;

        var ownedGoods = new HashSet<DECO4>();
        foreach (var good in this.sav.GetUGI_Goods())
            ownedGoods.Add((DECO4)good);

        ow["BlueCushion"] = ownedGoods.Contains(DECO4.BlueCushion);
        ow["TrashCan"] = ownedGoods.Contains(DECO4.TrashCan);
        ow["WoodenChair"] = ownedGoods.Contains(DECO4.WoodenChair);
        ow["YellowCushion"] = ownedGoods.Contains(DECO4.YellowCushion);
        ow["RockTool"] = ownedGoods.Contains(DECO4.RockTool);
        ow["SmokeTool"] = ownedGoods.Contains(DECO4.SmokeTool);
        ow["EmberTool"] = ownedGoods.Contains(DECO4.EmberTool);
        ow["MeowthDoll"] = ownedGoods.Contains(DECO4.MEOWTHDoll);
        ow["CardboardBox"] = ownedGoods.Contains(DECO4.CardboardBox);
        ow["PottedPlant"] = ownedGoods.Contains(DECO4.PottedPlant);
        ow["CharmanderDoll"] = ownedGoods.Contains(DECO4.CHARMANDERDoll);
        ow["CyndaquilDoll"] = ownedGoods.Contains(DECO4.CYNDAQUILDoll);
        ow["RockfallTool"] = ownedGoods.Contains(DECO4.RockfallTool);
        ow["TorchicDoll"] = ownedGoods.Contains(DECO4.TORCHICDoll);
        ow["ChimcharDoll"] = ownedGoods.Contains(DECO4.CHIMCHARDoll);
        ow["BigSmokeTool"] = ownedGoods.Contains(DECO4.BigSmokeTool);
        ow["Crate"] = ownedGoods.Contains(DECO4.Crate);
        ow["FireTool"] = ownedGoods.Contains(DECO4.FireTool);
        ow["RedBike"] = ownedGoods.Contains(DECO4.RedBike);
        ow["PlusleDoll"] = ownedGoods.Contains(DECO4.PLUSLEDoll);
        ow["SnorlaxDoll"] = ownedGoods.Contains(DECO4.SNORLAXDoll);
        ow["PlainTable"] = ownedGoods.Contains(DECO4.PlainTable);
        ow["SmallTable"] = ownedGoods.Contains(DECO4.SmallTable);
        ow["HoleTool"] = ownedGoods.Contains(DECO4.HoleTool);
        ow["FoamTool"] = ownedGoods.Contains(DECO4.FoamTool);
        ow["BigTable"] = ownedGoods.Contains(DECO4.BigTable);
        ow["LongTable"] = ownedGoods.Contains(DECO4.LongTable);
        ow["WideTable"] = ownedGoods.Contains(DECO4.WideTable);
        ow["GlameowDoll"] = ownedGoods.Contains(DECO4.GLAMEOWDoll);
        ow["PokeTable"] = ownedGoods.Contains(DECO4.PokeTable);
        ow["WideSofa"] = ownedGoods.Contains(DECO4.WideSofa);
        ow["MudkipDoll"] = ownedGoods.Contains(DECO4.MUDKIPDoll);
        ow["PitTool"] = ownedGoods.Contains(DECO4.PitTool);
        ow["SquirtleDoll"] = ownedGoods.Contains(DECO4.SQUIRTLEDoll);
        ow["TotodileDoll"] = ownedGoods.Contains(DECO4.TOTODILEDoll);
        ow["BubbleTool"] = ownedGoods.Contains(DECO4.BubbleTool);
        ow["DrifloonDoll"] = ownedGoods.Contains(DECO4.DRIFLOONDoll);
        ow["FeatheryBed"] = ownedGoods.Contains(DECO4.FeatheryBed);
        ow["PiplupDoll"] = ownedGoods.Contains(DECO4.PIPLUPDoll);
        ow["MinunDoll"] = ownedGoods.Contains(DECO4.MINUNDoll);
        ow["WailordDoll"] = ownedGoods.Contains(DECO4.WAILORDDoll);
        ow["Bonsai"] = ownedGoods.Contains(DECO4.Bonsai);
        ow["DaintyFlowers"] = ownedGoods.Contains(DECO4.DaintyFlowers);
        ow["LavishFlowers"] = ownedGoods.Contains(DECO4.LavishFlowers);
        ow["LovelyFlowers"] = ownedGoods.Contains(DECO4.LovelyFlowers);
        ow["PrettyFlowers"] = ownedGoods.Contains(DECO4.PrettyFlowers);
        ow["SmallBookshelf"] = ownedGoods.Contains(DECO4.SmallBookshelf);
        ow["VendingMachine"] = ownedGoods.Contains(DECO4.VendingMachine);
        ow["DisplayShelf"] = ownedGoods.Contains(DECO4.DisplayShelf);
        ow["Refrigerator"] = ownedGoods.Contains(DECO4.Refrigerator);
        ow["ResearchShelf"] = ownedGoods.Contains(DECO4.ResearchShelf);
        ow["SkittyDoll"] = ownedGoods.Contains(DECO4.SKITTYDoll);
        ow["BigBookshelf"] = ownedGoods.Contains(DECO4.BigBookshelf);
        ow["Cupboard"] = ownedGoods.Contains(DECO4.Cupboard);
        ow["WoodDresser"] = ownedGoods.Contains(DECO4.WoodDresser);
        ow["BulbasaurDoll"] = ownedGoods.Contains(DECO4.BULBASAURDoll);
        ow["ChikoritaDoll"] = ownedGoods.Contains(DECO4.CHIKORITADoll);
        ow["TreeckoDoll"] = ownedGoods.Contains(DECO4.TREECKODoll);
        ow["LeafTool"] = ownedGoods.Contains(DECO4.LeafTool);
        ow["GreenBike"] = ownedGoods.Contains(DECO4.GreenBike);
        ow["TurtwigDoll"] = ownedGoods.Contains(DECO4.TURTWIGDoll);
        ow["BikeRack"] = ownedGoods.Contains(DECO4.BikeRack);
        ow["ShopShelf"] = ownedGoods.Contains(DECO4.ShopShelf);
        ow["PinkDresser"] = ownedGoods.Contains(DECO4.PinkDresser);
        ow["FlowerTool"] = ownedGoods.Contains(DECO4.FlowerTool);
        ow["WobbuffetDoll"] = ownedGoods.Contains(DECO4.WOBBUFFETDoll);
        ow["IronBeam"] = ownedGoods.Contains(DECO4.IronBeam);
        ow["PrettySink"] = ownedGoods.Contains(DECO4.PrettySink);
        ow["TV"] = ownedGoods.Contains(DECO4.TV);
        ow["LabMachine"] = ownedGoods.Contains(DECO4.LabMachine);
        ow["TestMachine"] = ownedGoods.Contains(DECO4.TestMachine);
        ow["WeavileDoll"] = ownedGoods.Contains(DECO4.WEAVILEDoll);
        ow["GameSystem"] = ownedGoods.Contains(DECO4.GameSystem);
        ow["MazeBlock1"] = ownedGoods.Contains(DECO4.MazeBlock1);
        ow["MazeBlock2"] = ownedGoods.Contains(DECO4.MazeBlock2);
        ow["MazeBlock3"] = ownedGoods.Contains(DECO4.MazeBlock3);
        ow["MazeBlock4"] = ownedGoods.Contains(DECO4.MazeBlock4);
        ow["MazeBlock5"] = ownedGoods.Contains(DECO4.MazeBlock5);
        ow["HealingMachine"] = ownedGoods.Contains(DECO4.HealingMachine);
        ow["BigOilDrum"] = ownedGoods.Contains(DECO4.BigOilDrum);
        ow["Binoculars"] = ownedGoods.Contains(DECO4.Binoculars);
        ow["Container"] = ownedGoods.Contains(DECO4.Container);
        ow["OilDrum"] = ownedGoods.Contains(DECO4.OilDrum);
        ow["PokeFlower"] = ownedGoods.Contains(DECO4.PokeFlower);
        ow["BunearyDoll"] = ownedGoods.Contains(DECO4.BUNEARYDoll);
        ow["AlertTool1"] = ownedGoods.Contains(DECO4.AlertTool1);
        ow["AlertTool2"] = ownedGoods.Contains(DECO4.AlertTool2);
        ow["AlertTool3"] = ownedGoods.Contains(DECO4.AlertTool3);
        ow["AlertTool4"] = ownedGoods.Contains(DECO4.AlertTool4);
        ow["BlueTent"] = ownedGoods.Contains(DECO4.BlueTent);
        ow["RedTent"] = ownedGoods.Contains(DECO4.RedTent);
        ow["ClefairyDoll"] = ownedGoods.Contains(DECO4.CLEFAIRYDoll);
        ow["HappinyDoll"] = ownedGoods.Contains(DECO4.HAPPINYDoll);
        ow["JigglypuffDoll"] = ownedGoods.Contains(DECO4.JIGGLYPUFFDoll);
        ow["ClearTent"] = ownedGoods.Contains(DECO4.ClearTent);
        ow["PachirisuDoll"] = ownedGoods.Contains(DECO4.PACHIRISUDoll);
        ow["PikachuDoll"] = ownedGoods.Contains(DECO4.PIKACHUDoll);
        ow["BallOrnament"] = ownedGoods.Contains(DECO4.BallOrnament);
        ow["BeautyCup"] = ownedGoods.Contains(DECO4.BeautyCup);
        ow["BlueCrystal"] = ownedGoods.Contains(DECO4.BlueCrystal);
        ow["BonslyDoll"] = ownedGoods.Contains(DECO4.BONSLYDoll);
        ow["BronzeTrophy"] = ownedGoods.Contains(DECO4.BronzeTrophy);
        ow["BuizelDoll"] = ownedGoods.Contains(DECO4.BUIZELDoll);
        ow["ChatotDoll"] = ownedGoods.Contains(DECO4.CHATOTDoll);
        ow["ClearOrnament"] = ownedGoods.Contains(DECO4.ClearOrnament);
        ow["CleverCup"] = ownedGoods.Contains(DECO4.CleverCup);
        ow["CoolCup"] = ownedGoods.Contains(DECO4.CoolCup);
        ow["CuteCup"] = ownedGoods.Contains(DECO4.CuteCup);
        ow["GlitterGem"] = ownedGoods.Contains(DECO4.GlitterGem);
        ow["Globe"] = ownedGoods.Contains(DECO4.Globe);
        ow["GoldTrophy"] = ownedGoods.Contains(DECO4.GoldTrophy);
        ow["GreatTrophy"] = ownedGoods.Contains(DECO4.GreatTrophy);
        ow["GymStatue"] = ownedGoods.Contains(DECO4.GymStatue);
        ow["LucarioDoll"] = ownedGoods.Contains(DECO4.LUCARIODoll);
        ow["MagnezoneDoll"] = ownedGoods.Contains(DECO4.MAGNEZONEDoll);
        ow["ManaphyDoll"] = ownedGoods.Contains(DECO4.MANAPHYDoll);
        ow["MantykeDoll"] = ownedGoods.Contains(DECO4.MANTYKEDoll);
        ow["MimeJrDoll"] = ownedGoods.Contains(DECO4.MIMEJRDoll);
        ow["MunchlaxDoll"] = ownedGoods.Contains(DECO4.MUNCHLAXDoll);
        ow["MysticGem"] = ownedGoods.Contains(DECO4.MysticGem);
        ow["PinkCrystal"] = ownedGoods.Contains(DECO4.PinkCrystal);
        ow["PrettyGem"] = ownedGoods.Contains(DECO4.PrettyGem);
        ow["RedCrystal"] = ownedGoods.Contains(DECO4.RedCrystal);
        ow["RoundOrnament"] = ownedGoods.Contains(DECO4.RoundOrnament);
        ow["ShinyGem"] = ownedGoods.Contains(DECO4.ShinyGem);
        ow["SilverTrophy"] = ownedGoods.Contains(DECO4.SilverTrophy);
        ow["ToughCup"] = ownedGoods.Contains(DECO4.ToughCup);
        ow["YellowCrystal"] = ownedGoods.Contains(DECO4.YellowCrystal);
    }

    public enum SPHERE4
    {
        None,
        PrismSphereS,
        PaleSphereS,
        RedSphereS,
        BlueSphereS,
        GreenSphereS,
        PrismSphereL,
        PaleSphereL,
        RedSphereL,
        BlueSphereL,
        GreenSphereL,
    }

    public void Generate_undergroundItem()
    {
        var ow = new Dictionary<string, bool>();
        owned["undergroundItem"] = ow;

        var ownedSpheres = new HashSet<SPHERE4>();
        var sphereBytes = this.sav.GetUGI_Spheres();
        for (var i = 0; i < (sphereBytes.Length / 2); i++) // split in 2 (all types, then all sizes)
            ownedSpheres.Add((SPHERE4)sphereBytes[i]);

        var ownedTraps = new HashSet<TRAP4>();
        foreach (var good in this.sav.GetUGI_Traps())
            ownedTraps.Add((TRAP4)good);

        ow["PrismSphereS"] = ownedSpheres.Contains(SPHERE4.PrismSphereS);
        ow["PaleSphereS"] = ownedSpheres.Contains(SPHERE4.PaleSphereS);
        ow["RedSphereS"] = ownedSpheres.Contains(SPHERE4.RedSphereS);
        ow["BlueSphereS"] = ownedSpheres.Contains(SPHERE4.BlueSphereS);
        ow["GreenSphereS"] = ownedSpheres.Contains(SPHERE4.GreenSphereS);
        ow["PrismSphereL"] = ownedSpheres.Contains(SPHERE4.PrismSphereL);
        ow["PaleSphereL"] = ownedSpheres.Contains(SPHERE4.PaleSphereL);
        ow["RedSphereL"] = ownedSpheres.Contains(SPHERE4.RedSphereL);
        ow["BlueSphereL"] = ownedSpheres.Contains(SPHERE4.BlueSphereL);
        ow["GreenSphereL"] = ownedSpheres.Contains(SPHERE4.GreenSphereL);

        ow["MoveTrapUp"] = ownedTraps.Contains(TRAP4.MoveTrapUp);
        ow["MoveTrapRight"] = ownedTraps.Contains(TRAP4.MoveTrapRight);
        ow["MoveTrapDown"] = ownedTraps.Contains(TRAP4.MoveTrapDown);
        ow["MoveTrapLeft"] = ownedTraps.Contains(TRAP4.MoveTrapLeft);
        ow["HurlTrapUp"] = ownedTraps.Contains(TRAP4.HurlTrapUp);
        ow["HurlTrapRight"] = ownedTraps.Contains(TRAP4.HurlTrapRight);
        ow["HurlTrapDown"] = ownedTraps.Contains(TRAP4.HurlTrapDown);
        ow["HurlTrapLeft"] = ownedTraps.Contains(TRAP4.HurlTrapLeft);
        ow["WarpTrap"] = ownedTraps.Contains(TRAP4.WarpTrap);
        ow["HiWarpTrap"] = ownedTraps.Contains(TRAP4.HiWarpTrap);
        ow["HoleTrap"] = ownedTraps.Contains(TRAP4.HoleTrap);
        ow["PitTrap"] = ownedTraps.Contains(TRAP4.PitTrap);
        ow["ReverseTrap"] = ownedTraps.Contains(TRAP4.ReverseTrap);
        ow["ConfuseTrap"] = ownedTraps.Contains(TRAP4.ConfuseTrap);
        ow["RunTrap"] = ownedTraps.Contains(TRAP4.RunTrap);
        ow["FadeTrap"] = ownedTraps.Contains(TRAP4.FadeTrap);
        ow["SlowTrap"] = ownedTraps.Contains(TRAP4.SlowTrap);
        ow["SmokeTrap"] = ownedTraps.Contains(TRAP4.SmokeTrap);
        ow["BigSmokeTrap"] = ownedTraps.Contains(TRAP4.BigSmokeTrap);
        ow["RockTrap"] = ownedTraps.Contains(TRAP4.RockTrap);
        ow["RockfallTrap"] = ownedTraps.Contains(TRAP4.RockfallTrap);
        ow["FoamTrap"] = ownedTraps.Contains(TRAP4.FoamTrap);
        ow["BubbleTrap"] = ownedTraps.Contains(TRAP4.BubbleTrap);
        ow["AlertTrap1"] = ownedTraps.Contains(TRAP4.AlertTrap1);
        ow["AlertTrap2"] = ownedTraps.Contains(TRAP4.AlertTrap2);
        ow["AlertTrap3"] = ownedTraps.Contains(TRAP4.AlertTrap3);
        ow["AlertTrap4"] = ownedTraps.Contains(TRAP4.AlertTrap4);
        ow["LeafTrap"] = ownedTraps.Contains(TRAP4.LeafTrap);
        ow["FlowerTrap"] = ownedTraps.Contains(TRAP4.FlowerTrap);
        ow["EmberTrap"] = ownedTraps.Contains(TRAP4.EmberTrap);
        ow["FireTrap"] = ownedTraps.Contains(TRAP4.FireTrap);
        ow["RadarTrap"] = ownedTraps.Contains(TRAP4.RadarTrap);
        ow["DiggerDrill"] = ownedTraps.Contains(TRAP4.DiggerDrill);


    }

    public void Generate_inGameTrade()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameTrade"] = ow;

        ow["MachopforAbra"] = false; // TODO
        ow["BuizelforChatot"] = false; // TODO
        ow["MedichamforHaunter"] = false; // TODO
        ow["FinneonforMagikarp"] = false; // TODO
    }
    public void Generate_inGameGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameGift"] = ow;

        ow["StarterPokemon"] = false; // TODO
        ow["TogepiEgg"] = false; // TODO
        ow["Eevee"] = false; // TODO
        ow["Porygon"] = false; // TODO
        ow["RioluEgg"] = false; // TODO
    }

    public void Generate_accessory()
    {
        var ow = new Dictionary<string, bool>();
        owned["accessory"] = ow;

        ow["BlackFluff"] = false; // TODO
        ow["BrownFluff"] = false; // TODO
        ow["OrangeFluff"] = false; // TODO
        ow["PinkFluff"] = false; // TODO
        ow["WhiteFluff"] = false; // TODO
        ow["YellowFluff"] = false; // TODO
        ow["RoundPebble"] = false; // TODO
        ow["GlitterBoulder"] = false; // TODO
        ow["SnaggyPebble"] = false; // TODO
        ow["JaggedBoulder"] = false; // TODO
        ow["BlackPebble"] = false; // TODO
        ow["MiniPebble"] = false; // TODO
        ow["PinkScale"] = false; // TODO
        ow["BlueScale"] = false; // TODO
        ow["GreenScale"] = false; // TODO
        ow["PurpleScale"] = false; // TODO
        ow["BigScale"] = false; // TODO
        ow["NarrowScale"] = false; // TODO
        ow["BlueFeather"] = false; // TODO
        ow["RedFeather"] = false; // TODO
        ow["YellowFeather"] = false; // TODO
        ow["WhiteFeather"] = false; // TODO
        ow["BlackMoustache"] = false; // TODO
        ow["WhiteMoustache"] = false; // TODO
        ow["BlackBeard"] = false; // TODO
        ow["WhiteBeard"] = false; // TODO
        ow["SmallLeaf"] = false; // TODO
        ow["BigLeaf"] = false; // TODO
        ow["NarrowLeaf"] = false; // TODO
        ow["ShedClaw"] = false; // TODO
        ow["ShedHorn"] = false; // TODO
        ow["ThinMushroom"] = false; // TODO
        ow["ThickMushroom"] = false; // TODO
        ow["Stump"] = false; // TODO
        ow["PrettyDewdrop"] = false; // TODO
        ow["SnowCrystal"] = false; // TODO
        ow["Sparks"] = false; // TODO
        ow["ShimmeringFire"] = false; // TODO
        ow["MysticFire"] = false; // TODO
        ow["Determination"] = false; // TODO
        ow["PeculiarSpoon"] = false; // TODO
        ow["PuffySmoke"] = false; // TODO
        ow["PoisonExtract"] = false; // TODO
        ow["WealthyCoin"] = false; // TODO
        ow["EerieThing"] = false; // TODO
        ow["Spring"] = false; // TODO
        ow["Seashell"] = false; // TODO
        ow["HummingNote"] = false; // TODO
        ow["ShinyPowder"] = false; // TODO
        ow["GlitterPowder"] = false; // TODO
        ow["RedFlower"] = false; // TODO
        ow["PinkFlower"] = false; // TODO
        ow["WhiteFlower"] = false; // TODO
        ow["BlueFlower"] = false; // TODO
        ow["OrangeFlower"] = false; // TODO
        ow["YellowFlower"] = false; // TODO
        ow["GooglySpecs"] = false; // TODO
        ow["BlackSpecs"] = false; // TODO
        ow["GorgeousSpecs"] = false; // TODO
        ow["SweetCandy"] = false; // TODO
        ow["Confetti"] = false; // TODO
        ow["ColoredParasol"] = false; // TODO
        ow["OldUmbrella"] = false; // TODO
        ow["Spotlight"] = false; // TODO
        ow["Cape"] = false; // TODO
        ow["StandingMike"] = false; // TODO
        ow["Surfboard"] = false; // TODO
        ow["Carpet"] = false; // TODO
        ow["RetroPipe"] = false; // TODO
        ow["FluffyBed"] = false; // TODO
        ow["MirrorBall"] = false; // TODO
        ow["PhotoBoard"] = false; // TODO
        ow["PinkBarrette"] = false; // TODO
        ow["RedBarrette"] = false; // TODO
        ow["BlueBarrette"] = false; // TODO
        ow["YellowBarrette"] = false; // TODO
        ow["GreenBarrette"] = false; // TODO
        ow["PinkBalloon"] = false; // TODO
        ow["RedBalloons"] = false; // TODO
        ow["BlueBalloons"] = false; // TODO
        ow["YellowBalloon"] = false; // TODO
        ow["GreenBalloons"] = false; // TODO
        ow["LaceHeadress"] = false; // TODO
        ow["TopHat"] = false; // TODO
        ow["SilkVeil"] = false; // TODO
        ow["HeroicHeadband"] = false; // TODO
        ow["ProfessorHat"] = false; // TODO
        ow["FlowerStage"] = false; // TODO
        ow["GoldPedestal"] = false; // TODO
        ow["GlassStage"] = false; // TODO
        ow["AwardPodium"] = false; // TODO
        ow["CubeStage"] = false; // TODO
        ow["TurtwigMask"] = false; // TODO
        ow["ChimcharMask"] = false; // TODO
        ow["PiplupMask"] = false; // TODO
        ow["BigTree"] = false; // TODO
        ow["Flag"] = false; // TODO
        ow["Crown"] = false; // TODO
        ow["Tiara"] = false; // TODO
        ow["Comet"] = false; // TODO

    }
    
    public void Generate_poffin()
    {
        var ow = new Dictionary<string, bool>();
        owned["poffin"] = ow;

        var poffinCase4 = new PoffinCase4(this.sav);
        var HasPoffin = (PoffinFlavor4 flavour) =>
        {
            return poffinCase4.Poffins.Any(pof => pof.Type == flavour);
        };

        ow["FoulPoffin"] = HasPoffin(PoffinFlavor4.Foul);
        ow["RichPoffin"] = HasPoffin(PoffinFlavor4.Rich);
        ow["OverripePoffin"] = HasPoffin(PoffinFlavor4.Overripe);
        ow["MildPoffin"] = HasPoffin(PoffinFlavor4.Mild);
        ow["BitterPoffin"] = HasPoffin(PoffinFlavor4.Bitter);
        ow["BitterDryPoffin"] = HasPoffin(PoffinFlavor4.Bitter_Dry);
        ow["BitterSourPoffin"] = HasPoffin(PoffinFlavor4.Bitter_Sour);
        ow["BitterSpicyPoffin"] = HasPoffin(PoffinFlavor4.Bitter_Spicy);
        ow["DryPoffin"] = HasPoffin(PoffinFlavor4.Dry);
        ow["DryBitterPoffin"] = HasPoffin(PoffinFlavor4.Dry_Bitter);
        ow["DrySourPoffin"] = HasPoffin(PoffinFlavor4.Dry_Sour);
        ow["DrySweetPoffin"] = HasPoffin(PoffinFlavor4.Dry_Sweet);
        ow["SourPoffin"] = HasPoffin(PoffinFlavor4.Sour);
        ow["SourDryPoffin"] = HasPoffin(PoffinFlavor4.Sour_Dry);
        ow["SourSpicyPoffin"] = HasPoffin(PoffinFlavor4.Sour_Spicy);
        ow["SourSweetPoffin"] = HasPoffin(PoffinFlavor4.Sour_Sweet);
        ow["SpicyPoffin"] = HasPoffin(PoffinFlavor4.Spicy);
        ow["SpicyBitterPoffin"] = HasPoffin(PoffinFlavor4.Spicy_Bitter);
        ow["SpicyDryPoffin"] = HasPoffin(PoffinFlavor4.Spicy_Dry);
        ow["SpicySourPoffin"] = HasPoffin(PoffinFlavor4.Spicy_Sour);
        ow["SpicySweetPoffin"] = HasPoffin(PoffinFlavor4.Spicy_Sweet);
        ow["SweetPoffin"] = HasPoffin(PoffinFlavor4.Sweet);
        ow["SweetBitterPoffin"] = HasPoffin(PoffinFlavor4.Sweet_Bitter);
        ow["SweetSourPoffin"] = HasPoffin(PoffinFlavor4.Sweet_Sour);
        ow["SweetSpicyPoffin"] = HasPoffin(PoffinFlavor4.Sweet_Spicy);
    }
    public void Generate_pcWallPaper()
    {
        var ow = new Dictionary<string, bool>();
        owned["pcWallPaper"] = ow;

        var HasWallPaper = (int id) =>
        {
            for (int i = 0; i < this.sav.BoxCount; i++)
                if (this.sav.GetBoxWallpaper(i) == id)
                    return true;
            return false;
        };
        //TODO if not living, only get if unlocked
        ow["Distortion"] = HasWallPaper(0); // TODO
        ow["Contest"] = HasWallPaper(1);
        ow["Nostalgic"] = HasWallPaper(2);
        ow["Croagunk"] = HasWallPaper(3);
        ow["Trio"] = HasWallPaper(4);
        ow["PikaPika"] = HasWallPaper(5);
        ow["Legend"] = HasWallPaper(6);
        ow["TeamGalactic"] = HasWallPaper(7);
    }


    public void Generate_villaFurniture()
    {
        var ow = new Dictionary<string, bool>();
        owned["villaFurniture"] = ow;

        ow["Table"] = false; // TODO
        ow["BigSofa"] = false; // TODO
        ow["SmallSofa"] = false; // TODO
        ow["Bed"] = false; // TODO
        ow["NightTable"] = false; // TODO
        ow["TV"] = false; // TODO
        ow["AudioSystem"] = false; // TODO
        ow["Bookshelf"] = false; // TODO
        ow["Rack"] = false; // TODO
        ow["Houseplant"] = false; // TODO
        ow["PCDesk"] = false; // TODO
        ow["MusicBox"] = false; // TODO
        ow["PokemonBust"] = false; // TODO
        ow["Piano"] = false; // TODO
        ow["GuestSet"] = false; // TODO
        ow["WallClock"] = false; // TODO
        ow["Masterpiece"] = false; // TODO
        ow["TeaSet"] = false; // TODO
        ow["Chandelier"] = false; // TODO

    }


    public void Generate_pokeballSeal()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokeballSeal"] = ow;

        var HasSeal = (Seal4 seal) => this.sav.GetSealCount(seal) > 0;

        ow["HeartSealA"] = HasSeal(Seal4.HeartA);
        ow["HeartSealB"] = HasSeal(Seal4.HeartB);
        ow["HeartSealC"] = HasSeal(Seal4.HeartC);
        ow["HeartSealD"] = HasSeal(Seal4.HeartD);
        ow["HeartSealE"] = HasSeal(Seal4.HeartE);
        ow["HeartSealF"] = HasSeal(Seal4.HeartF);
        ow["StarSealA"] = HasSeal(Seal4.StarA);
        ow["StarSealB"] = HasSeal(Seal4.StarB);
        ow["StarSealC"] = HasSeal(Seal4.StarC);
        ow["StarSealD"] = HasSeal(Seal4.StarD);
        ow["StarSealE"] = HasSeal(Seal4.StarE);
        ow["StarSealF"] = HasSeal(Seal4.StarF);
        ow["LineSealA"] = HasSeal(Seal4.LineA);
        ow["LineSealB"] = HasSeal(Seal4.LineB);
        ow["LineSealC"] = HasSeal(Seal4.LineC);
        ow["LineSealD"] = HasSeal(Seal4.LineD);
        ow["SmokeSealA"] = HasSeal(Seal4.SmokeA);
        ow["SmokeSealB"] = HasSeal(Seal4.SmokeB);
        ow["SmokeSealC"] = HasSeal(Seal4.SmokeC);
        ow["SmokeSealD"] = HasSeal(Seal4.SmokeD);
        ow["EleSealA"] = HasSeal(Seal4.ElectricA);
        ow["EleSealB"] = HasSeal(Seal4.ElectricB);
        ow["EleSealC"] = HasSeal(Seal4.ElectricC);
        ow["EleSealD"] = HasSeal(Seal4.ElectricD);
        ow["FoamySealA"] = HasSeal(Seal4.FoamyA);
        ow["FoamySealB"] = HasSeal(Seal4.FoamyB);
        ow["FoamySealC"] = HasSeal(Seal4.FoamyC);
        ow["FoamySealD"] = HasSeal(Seal4.FoamyD);
        ow["FireSealA"] = HasSeal(Seal4.FireA);
        ow["FireSealB"] = HasSeal(Seal4.FireB);
        ow["FireSealC"] = HasSeal(Seal4.FireC);
        ow["FireSealD"] = HasSeal(Seal4.FireD);
        ow["PartySealA"] = HasSeal(Seal4.PartyA);
        ow["PartySealB"] = HasSeal(Seal4.PartyB);
        ow["PartySealC"] = HasSeal(Seal4.PartyC);
        ow["PartySealD"] = HasSeal(Seal4.PartyD);
        ow["FloraSealA"] = HasSeal(Seal4.FloraA);
        ow["FloraSealB"] = HasSeal(Seal4.FloraB);
        ow["FloraSealC"] = HasSeal(Seal4.FloraC);
        ow["FloraSealD"] = HasSeal(Seal4.FloraD);
        ow["FloraSealE"] = HasSeal(Seal4.FloraE);
        ow["FloraSealF"] = HasSeal(Seal4.FloraF);
        ow["SongSealA"] = HasSeal(Seal4.SongA);
        ow["SongSealB"] = HasSeal(Seal4.SongB);
        ow["SongSealC"] = HasSeal(Seal4.SongC);
        ow["SongSealD"] = HasSeal(Seal4.SongD);
        ow["SongSealE"] = HasSeal(Seal4.SongE);
        ow["SongSealF"] = HasSeal(Seal4.SongF);
        ow["SongSealG"] = HasSeal(Seal4.SongG);
        ow["ASeal"] = HasSeal(Seal4.LetterA);
        ow["BSeal"] = HasSeal(Seal4.LetterB);
        ow["CSeal"] = HasSeal(Seal4.LetterC);
        ow["DSeal"] = HasSeal(Seal4.LetterD);
        ow["ESeal"] = HasSeal(Seal4.LetterE);
        ow["FSeal"] = HasSeal(Seal4.LetterF);
        ow["GSeal"] = HasSeal(Seal4.LetterG);
        ow["HSeal"] = HasSeal(Seal4.LetterH);
        ow["ISeal"] = HasSeal(Seal4.LetterI);
        ow["JSeal"] = HasSeal(Seal4.LetterJ);
        ow["KSeal"] = HasSeal(Seal4.LetterK);
        ow["LSeal"] = HasSeal(Seal4.LetterL);
        ow["MSeal"] = HasSeal(Seal4.LetterM);
        ow["NSeal"] = HasSeal(Seal4.LetterN);
        ow["OSeal"] = HasSeal(Seal4.LetterO);
        ow["PSeal"] = HasSeal(Seal4.LetterP);
        ow["QSeal"] = HasSeal(Seal4.LetterQ);
        ow["RSeal"] = HasSeal(Seal4.LetterR);
        ow["SSeal"] = HasSeal(Seal4.LetterS);
        ow["TSeal"] = HasSeal(Seal4.LetterT);
        ow["USeal"] = HasSeal(Seal4.LetterU);
        ow["VSeal"] = HasSeal(Seal4.LetterV);
        ow["WSeal"] = HasSeal(Seal4.LetterW);
        ow["XSeal"] = HasSeal(Seal4.LetterX);
        ow["YSeal"] = HasSeal(Seal4.LetterY);
        ow["ZSeal"] = HasSeal(Seal4.LetterZ);
        ow["ShockSeal"] = HasSeal(Seal4.Shock);
        ow["MysterySeal"] = HasSeal(Seal4.Mystery);
        ow["LiquidSeal"] = HasSeal(Seal4.Liquid);
        ow["BurstSeal"] = HasSeal(Seal4.Burst);
        ow["TwinkleSeal"] = HasSeal(Seal4.Twinkle);
    }


    public void Generate_backdrop()
    {
        var ow = new Dictionary<string, bool>();
        owned["backdrop"] = ow;

        ow["DressUp"] = false; // TODO
        ow["Ranch"] = false; // TODO
        ow["CityatNight"] = false; // TODO
        ow["SnowyTown"] = false; // TODO
        ow["Fiery"] = false; // TODO
        ow["OuterSpace"] = false; // TODO
        ow["Desert"] = false; // TODO
        ow["CumulusCloud"] = false; // TODO
        ow["FlowerPatch"] = false; // TODO
        ow["FutureRoom"] = false; // TODO
        ow["OpenSea"] = false; // TODO
        ow["TotalDarkness"] = false; // TODO
        ow["TatamiRoom"] = false; // TODO
        ow["GingerbreadRoom"] = false; // TODO
        ow["Seafloor"] = false; // TODO
        ow["Underground"] = false; // TODO
        ow["Sky"] = false; // TODO
        ow["Theater"] = false; // TODO
    }



    public void Generate_battleFrontier()
    {
        var ow = new Dictionary<string, bool>();
        owned["battleFrontier"] = ow;

        ow["BattleTowerSilver"] = false; // TODO
        ow["BattleFactorySilver"] = false; // TODO
        ow["BattleArcadeSilver"] = false; // TODO
        ow["BattleCastleSilver"] = false; // TODO
        ow["BattleHallSilver"] = false; // TODO
        ow["BattleTowerGold"] = false; // TODO
        ow["BattleFactoryGold"] = false; // TODO
        ow["BattleArcadeGold"] = false; // TODO
        ow["BattleCastleGold"] = false; // TODO
        ow["BattleHallGold"] = false; // TODO
    }

    public bool HasRibbon(Func<PK4, bool> func)
    {
        var pkms = sav.GetAllPKM();
        return pkms.Any(pk =>
        {
            var pk4 = (PK4?)pk;
            if (pk4 == null)
                return false;
            return func(pk4);
        });
    }
    public bool HasContestRibbon(Func<PK4, byte> func, byte wantedVal)
    {
        return HasRibbon(pk => func(pk) >= wantedVal);
    }

    public void Generate_ribbon()
    {
        var ow = new Dictionary<string, bool>();
        owned["ribbon"] = ow;

        ow["ChampionRibbon"] = HasRibbon(pk4 => pk4.RibbonChampionBattle); //TODO
        ow["SinnohChampionRibbon"] = HasRibbon(pk4 => pk4.RibbonChampionSinnoh);
        ow["CoolRibbonHoenn"] = HasRibbon(pk4 => pk4.RibbonG3Cool);
        ow["CoolRibbonSuperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3CoolSuper);
        ow["CoolRibbonHyperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3CoolHyper);
        ow["CoolRibbonMasterHoenn"] = HasRibbon(pk4 => pk4.RibbonG3CoolMaster);
        ow["BeautyRibbonHoenn"] = HasRibbon(pk4 => pk4.RibbonG3Beauty);
        ow["BeautyRibbonSuperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3BeautySuper);
        ow["BeautyRibbonHyperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3BeautyHyper);
        ow["BeautyRibbonMasterHoenn"] = HasRibbon(pk4 => pk4.RibbonG3BeautyMaster);
        ow["CuteRibbonHoenn"] = HasRibbon(pk4 => pk4.RibbonG3Cute);
        ow["CuteRibbonSuperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3CuteSuper);
        ow["CuteRibbonHyperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3CuteHyper);
        ow["CuteRibbonMasterHoenn"] = HasRibbon(pk4 => pk4.RibbonG3CuteMaster);
        ow["SmartRibbonHoenn"] = HasRibbon(pk4 => pk4.RibbonG3Smart);
        ow["SmartRibbonSuperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3SmartSuper);
        ow["SmartRibbonHyperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3SmartHyper);
        ow["SmartRibbonMasterHoenn"] = HasRibbon(pk4 => pk4.RibbonG3SmartMaster);
        ow["ToughRibbonHoenn"] = HasRibbon(pk4 => pk4.RibbonG3Tough);
        ow["ToughRibbonSuperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3ToughSuper);
        ow["ToughRibbonHyperHoenn"] = HasRibbon(pk4 => pk4.RibbonG3ToughHyper);
        ow["ToughRibbonMasterHoenn"] = HasRibbon(pk4 => pk4.RibbonG3ToughMaster);
        ow["CoolRibbonSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4Cool);
        ow["CoolRibbonGreatSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4CoolGreat);
        ow["CoolRibbonUltraSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4CoolUltra);
        ow["CoolRibbonMasterSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4CoolMaster);
        ow["BeautyRibbonSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4Beauty);
        ow["BeautyRibbonGreatSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4BeautyGreat);
        ow["BeautyRibbonUltraSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4BeautyUltra);
        ow["BeautyRibbonMasterSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4BeautyMaster);
        ow["CuteRibbonSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4Cute);
        ow["CuteRibbonGreatSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4CuteGreat);
        ow["CuteRibbonUltraSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4CuteUltra);
        ow["CuteRibbonMasterSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4CuteMaster);
        ow["SmartRibbonSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4Smart);
        ow["SmartRibbonGreatSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4SmartGreat);
        ow["SmartRibbonUltraSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4SmartUltra);
        ow["SmartRibbonMasterSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4SmartMaster);
        ow["ToughRibbonSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4Tough);
        ow["ToughRibbonGreatSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4ToughGreat);
        ow["ToughRibbonUltraSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4ToughUltra);
        ow["ToughRibbonMasterSinnoh"] = HasRibbon(pk4 => pk4.RibbonG4ToughMaster);
        ow["WinningRibbon"] = HasRibbon(pk4 => pk4.RibbonWinning);
        ow["VictoryRibbon"] = HasRibbon(pk4 => pk4.RibbonVictory);
        ow["AbilityRibbon"] = HasRibbon(pk4 => pk4.RibbonAbility);
        ow["GreatAbilityRibbon"] = HasRibbon(pk4 => pk4.RibbonAbilityGreat);
        ow["DoubleAbilityRibbon"] = HasRibbon(pk4 => pk4.RibbonAbilityDouble);
        ow["MultiAbilityRibbon"] = HasRibbon(pk4 => pk4.RibbonAbilityMulti);
        ow["PairAbilityRibbon"] = HasRibbon(pk4 => pk4.RibbonAbilityPair);
        ow["WorldAbilityRibbon"] = HasRibbon(pk4 => pk4.RibbonAbilityWorld);
        ow["ArtistRibbon"] = HasRibbon(pk4 => pk4.RibbonArtist);
        ow["EffortRibbon"] = HasRibbon(pk4 => pk4.RibbonEffort);
        ow["AlertRibbon"] = HasRibbon(pk4 => pk4.RibbonAlert);
        ow["ShockRibbon"] = HasRibbon(pk4 => pk4.RibbonShock);
        ow["DowncastRibbon"] = HasRibbon(pk4 => pk4.RibbonDowncast);
        ow["CarelessRibbon"] = HasRibbon(pk4 => pk4.RibbonCareless);
        ow["RelaxRibbon"] = HasRibbon(pk4 => pk4.RibbonRelax);
        ow["SnoozeRibbon"] = HasRibbon(pk4 => pk4.RibbonSnooze);
        ow["SmileRibbon"] = HasRibbon(pk4 => pk4.RibbonSmile);
        ow["GorgeousRibbon"] = HasRibbon(pk4 => pk4.RibbonGorgeous);
        ow["RoyalRibbon"] = HasRibbon(pk4 => pk4.RibbonRoyal);
        ow["GorgeousRoyalRibbon"] = HasRibbon(pk4 => pk4.RibbonGorgeousRoyal);
        ow["FootprintRibbon"] = HasRibbon(pk4 => pk4.RibbonFootprint);
        ow["RecordRibbon"] = HasRibbon(pk4 => pk4.RibbonRecord);
        ow["CountryRibbon"] = HasRibbon(pk4 => pk4.RibbonCountry);
        ow["NationalRibbon"] = HasRibbon(pk4 => pk4.RibbonNational);
        ow["EarthRibbon"] = HasRibbon(pk4 => pk4.RibbonEarth);
        ow["WorldRibbon"] = HasRibbon(pk4 => pk4.RibbonWorld);
        ow["ClassicRibbon"] = HasRibbon(pk4 => pk4.RibbonClassic);
    }

    public void Generate_trainerStar()
    {
        var ow = new Dictionary<string, bool>();
        owned["trainerStar"] = ow;

        ow["HallofFame"] = false; // TODO
        ow["PokemonContest"] = false; // TODO
        ow["Underground"] = false; // TODO
        ow["BattleTower"] = false; // TODO

    }


    public void Generate_easyChatSystemWord()
    {
        var ow = new Dictionary<string, bool>();
        owned["easyChatSystemWord"] = ow;

        ow["ToughWordsArtery"] = false; //TODO
        ow["ToughWordsBone Density"] = false; //TODO
        ow["ToughWordsCadenza"] = false; //TODO
        ow["ToughWordsConductivity"] = false; //TODO
        ow["ToughWordsContour"] = false; //TODO
        ow["ToughWordsCopyright"] = false; //TODO
        ow["ToughWordsCross-Stitch"] = false; //TODO
        ow["ToughWordsCubism"] = false; //TODO
        ow["ToughWordsEarth Tones"] = false; //TODO
        ow["ToughWordsEducation"] = false; //TODO
        ow["ToughWordsFlambe"] = false; //TODO
        ow["ToughWordsFractals"] = false; //TODO
        ow["ToughWordsGMT"] = false; //TODO
        ow["ToughWordsGolden Ratio"] = false; //TODO
        ow["ToughWordsGommage"] = false; //TODO
        ow["ToughWordsHowling"] = false; //TODO
        ow["ToughWordsImplant"] = false; //TODO
        ow["ToughWordsIrritability"] = false; //TODO
        ow["ToughWordsMoney Rate"] = false; //TODO
        ow["ToughWordsNeutrino"] = false; //TODO
        ow["ToughWordsOmnibus"] = false; //TODO
        ow["ToughWordsPH Balance"] = false; //TODO
        ow["ToughWordsPolyphenol"] = false; //TODO
        ow["ToughWordsREM Sleep"] = false; //TODO
        ow["ToughWordsResolution"] = false; //TODO
        ow["ToughWordsSpreadsheet"] = false; //TODO
        ow["ToughWordsStarboard"] = false; //TODO
        ow["ToughWordsStock Prices"] = false; //TODO
        ow["ToughWordsStreaming"] = false; //TODO
        ow["ToughWordsTwo-Step"] = false; //TODO
        ow["ToughWordsUbiquitous"] = false; //TODO
        ow["ToughWordsVector"] = false; //TODO
    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;

        ow["DefeatEliteFourAfterNationalDex"] = false; // TODO
        ow["DefeatRivalLevel85Rematch"] = false; // TODO
        ow["UnlockPokedexForeignEntries"] = false; // TODO
    }
}