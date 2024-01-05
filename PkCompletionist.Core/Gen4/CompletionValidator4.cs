using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Buffers.Binary.BinaryPrimitives;
using static System.Formats.Asn1.AsnWriter;

namespace PkCompletionist.Core;

/*
 * Modifications to PkHex.Core
 * Geonet
 * 
 * */

internal class CompletionValidator4 : CompletionValidatorX
{
    public CompletionValidator4(Command command, SAV4Pt sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;

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
        Generate_undergroundItem();
        Generate_ribbon();
        Generate_backdrop();

        Generate_pokeballSeal();
        Generate_accessory();
        Generate_poffin();
        Generate_pcWallPaper();

        Generate_villaFurniture();
        Generate_trainerStar();
        Generate_easyChatSystemWord();
        Generate_geonet();
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
        ow["CherrimOvercast"] = HasPkm(421);
        ow["CherrimSunshine"] = HasPkm(421);
        ow["ShellosWestSea"] = HasPkmForm(422, 0);
        ow["ShellosEastSea"] = HasPkmForm(422, 1);
        ow["GastrodonWestSea"] = HasPkmForm(423, 0);
        ow["GastrodonEastSea"] = HasPkmForm(423, 1);
        ow["RotomGhost"] = HasPkmForm(479, 0);
        ow["RotomHeat"] = HasPkmForm(479, 1);
        ow["RotomWash"] = HasPkmForm(479, 2);
        ow["RotomFrost"] = HasPkmForm(479, 3);
        ow["RotomFan"] =  HasPkmForm(479, 4);
        ow["RotomMow"] = HasPkmForm(479, 5);
        ow["GiratinaAltered"] = HasPkmForm(487, 0);
        ow["GiratinaOrigin"] = HasPkmForm(488, 1);
        ow["ShayminLand"] = HasPkmForm(492, 0);
        ow["ShayminSky"] = HasPkmForm(492, 1);

        ow["AllMaleFemaleForms"] = new Func<bool>(() =>
        {
            var genderLess = new HashSet<ushort> { 81, 82, 100, 101, 120, 121, 137, 233, 292, 337, 338, 343, 344, 374, 375, 376, 436, 437, 462, 474, 479, 479, 479, 479, 479, 479, 489, 490, 132, 144, 144, 145, 145, 146, 146, 150, 151, 201, 243, 244, 245, 249, 250, 251, 377, 378, 379, 382, 383, 384, 385, 386, 386, 386, 386, 480, 481, 482, 483, 484, 486, 487, 487, 491, 492, 492, 493 };
            for (ushort i = 0; i <= 493; i++)
            {
                if (!this.sav.Dex.GetSeen(i))
                    return false;
                if (genderLess.Contains(i))
                    continue;
                if (this.sav.Dex.GetSeenGenderFirst(i) == this.sav.Dex.GetSeenGenderSecond(i)) // 01 and 10 is when both genders are seen. 00 and 11 are when 1 is seen.
                    return false;
            }
            return true;
        })();

        ow["AllForeignPokedexEntries"] = new Func<bool>(() =>
        {
            for (ushort i = 0; i <= 493; i++)
            {
                for (int j = 0; j < 6; j++)
                    if (!this.sav.Dex.GetLanguageBitIndex(i, j))
                        return false;
            }
            return true;
        })();
    }

    public override void Generate_item()
    {
        base.Generate_item();

        var ow = this.owned["item"];
    }


    public void Generate_poketch()
    {
        var ow = new Dictionary<string, bool>();
        owned["poketch"] = ow;

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

        ow["MachopforAbra"] = sav.GetEventFlag(0x0085);
        ow["BuizelforChatot"] = sav.GetEventFlag(0x0086);
        ow["MedichamforHaunter"] = sav.GetEventFlag(0x00F4);
        ow["FinneonforMagikarp"] = sav.GetEventFlag(0x00F5);
    }
    public void Generate_inGameGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameGift"] = ow;

        ow["StarterPokemon"] = true;
        ow["TogepiEgg"] = sav.GetWork(0x007A) >= 5;
        ow["Eevee"] = sav.GetEventFlag(305);
        ow["Porygon"] = sav.GetEventFlag(151); 
        ow["RioluEgg"] = sav.GetEventFlag(0x220);
    }
    public bool HasAccessory(Accessory accessory)
    {
        if (accessory <= Accessory.Confetti)
        {
            var enumIdx = (byte)accessory;
            var val = sav.General[0x4E38 + enumIdx / 2];
            if (enumIdx % 2 == 0)
                return (val & 0x0F) != 0;
            return (val & 0xF0) != 0;
        }

        var diff = (byte)accessory - (byte)Accessory.Confetti;
        return FlagUtil.GetFlag(sav.General, 0x4E58 + diff / 8, diff % 8);
    }

    public void Generate_accessory()
    {
        var ow = new Dictionary<string, bool>();
        owned["accessory"] = ow;

        ow["WhiteFluff"] = HasAccessory(Accessory.WhiteFluff);
        ow["YellowFluff"] = HasAccessory(Accessory.YellowFluff);
        ow["PinkFluff"] = HasAccessory(Accessory.PinkFluff);
        ow["BrownFluff"] = HasAccessory(Accessory.BrownFluff);
        ow["BlackFluff"] = HasAccessory(Accessory.BlackFluff);
        ow["OrangeFluff"] = HasAccessory(Accessory.OrangeFluff);
        ow["RoundPebble"] = HasAccessory(Accessory.RoundPebble);
        ow["GlitterBoulder"] = HasAccessory(Accessory.GlitterBoulder);
        ow["SnaggyPebble"] = HasAccessory(Accessory.SnaggyPebble);
        ow["JaggedBoulder"] = HasAccessory(Accessory.JaggedBoulder);
        ow["BlackPebble"] = HasAccessory(Accessory.BlackPebble);
        ow["MiniPebble"] = HasAccessory(Accessory.MiniPebble);
        ow["PinkScale"] = HasAccessory(Accessory.PinkScale);
        ow["BlueScale"] = HasAccessory(Accessory.BlueScale);
        ow["GreenScale"] = HasAccessory(Accessory.GreenScale);
        ow["PurpleScale"] = HasAccessory(Accessory.PurpleScale);
        ow["BigScale"] = HasAccessory(Accessory.BigScale);
        ow["NarrowScale"] = HasAccessory(Accessory.NarrowScale);
        ow["BlueFeather"] = HasAccessory(Accessory.BlueFeather);
        ow["RedFeather"] = HasAccessory(Accessory.RedFeather);
        ow["YellowFeather"] = HasAccessory(Accessory.YellowFeather);
        ow["WhiteFeather"] = HasAccessory(Accessory.WhiteFeather);
        ow["BlackMoustache"] = HasAccessory(Accessory.BlackMoustache);
        ow["WhiteMoustache"] = HasAccessory(Accessory.WhiteMoustache);
        ow["BlackBeard"] = HasAccessory(Accessory.BlackBeard);
        ow["WhiteBeard"] = HasAccessory(Accessory.WhiteBeard);
        ow["SmallLeaf"] = HasAccessory(Accessory.SmallLeaf);
        ow["BigLeaf"] = HasAccessory(Accessory.BigLeaf);
        ow["NarrowLeaf"] = HasAccessory(Accessory.NarrowLeaf);
        ow["ShedClaw"] = HasAccessory(Accessory.ShedClaw);
        ow["ShedHorn"] = HasAccessory(Accessory.ShedHorn);
        ow["ThinMushroom"] = HasAccessory(Accessory.ThinMushroom);
        ow["ThickMushroom"] = HasAccessory(Accessory.ThickMushroom);
        ow["Stump"] = HasAccessory(Accessory.Stump);
        ow["PrettyDewdrop"] = HasAccessory(Accessory.PrettyDewdrop);
        ow["SnowCrystal"] = HasAccessory(Accessory.SnowCrystal);
        ow["Sparks"] = HasAccessory(Accessory.Sparks);
        ow["ShimmeringFire"] = HasAccessory(Accessory.ShimmeringFire);
        ow["MysticFire"] = HasAccessory(Accessory.MysticFire);
        ow["Determination"] = HasAccessory(Accessory.Determination);
        ow["PeculiarSpoon"] = HasAccessory(Accessory.PeculiarSpoon);
        ow["PuffySmoke"] = HasAccessory(Accessory.PuffySmoke);
        ow["PoisonExtract"] = HasAccessory(Accessory.PoisonExtract);
        ow["WealthyCoin"] = HasAccessory(Accessory.WealthyCoin);
        ow["EerieThing"] = HasAccessory(Accessory.EerieThing);
        ow["Spring"] = HasAccessory(Accessory.Spring);
        ow["Seashell"] = HasAccessory(Accessory.Seashell);
        ow["HummingNote"] = HasAccessory(Accessory.HummingNote);
        ow["ShinyPowder"] = HasAccessory(Accessory.ShinyPowder);
        ow["GlitterPowder"] = HasAccessory(Accessory.GlitterPowder);
        ow["RedFlower"] = HasAccessory(Accessory.RedFlower);
        ow["PinkFlower"] = HasAccessory(Accessory.PinkFlower);
        ow["WhiteFlower"] = HasAccessory(Accessory.WhiteFlower);
        ow["BlueFlower"] = HasAccessory(Accessory.BlueFlower);
        ow["OrangeFlower"] = HasAccessory(Accessory.OrangeFlower);
        ow["YellowFlower"] = HasAccessory(Accessory.YellowFlower);
        ow["GooglySpecs"] = HasAccessory(Accessory.GooglySpecs);
        ow["BlackSpecs"] = HasAccessory(Accessory.BlackSpecs);
        ow["GorgeousSpecs"] = HasAccessory(Accessory.GorgeousSpecs);
        ow["SweetCandy"] = HasAccessory(Accessory.SweetCandy);
        ow["Confetti"] = HasAccessory(Accessory.Confetti);
        ow["ColoredParasol"] = HasAccessory(Accessory.ColoredParasol);
        ow["OldUmbrella"] = HasAccessory(Accessory.OldUmbrella);
        ow["Spotlight"] = HasAccessory(Accessory.Spotlight);
        ow["Cape"] = HasAccessory(Accessory.Cape);
        ow["StandingMike"] = HasAccessory(Accessory.StandingMike);
        ow["Surfboard"] = HasAccessory(Accessory.Surfboard);
        ow["Carpet"] = HasAccessory(Accessory.Carpet);
        ow["RetroPipe"] = HasAccessory(Accessory.RetroPipe);
        ow["FluffyBed"] = HasAccessory(Accessory.FluffyBed);
        ow["MirrorBall"] = HasAccessory(Accessory.MirrorBall);
        ow["PhotoBoard"] = HasAccessory(Accessory.PhotoBoard);
        ow["PinkBarrette"] = HasAccessory(Accessory.PinkBarrette);
        ow["RedBarrette"] = HasAccessory(Accessory.RedBarrette);
        ow["BlueBarrette"] = HasAccessory(Accessory.BlueBarrette);
        ow["YellowBarrette"] = HasAccessory(Accessory.YellowBarrette);
        ow["GreenBarrette"] = HasAccessory(Accessory.GreenBarrette);
        ow["PinkBalloon"] = HasAccessory(Accessory.PinkBalloon);
        ow["RedBalloons"] = HasAccessory(Accessory.RedBalloons);
        ow["BlueBalloons"] = HasAccessory(Accessory.BlueBalloons);
        ow["YellowBalloon"] = HasAccessory(Accessory.YellowBalloon);
        ow["GreenBalloons"] = HasAccessory(Accessory.GreenBalloons);
        ow["LaceHeadress"] = HasAccessory(Accessory.LaceHeadress);
        ow["TopHat"] = HasAccessory(Accessory.TopHat);
        ow["SilkVeil"] = HasAccessory(Accessory.SilkVeil);
        ow["HeroicHeadband"] = HasAccessory(Accessory.HeroicHeadband);
        ow["ProfessorHat"] = HasAccessory(Accessory.ProfessorHat);
        ow["FlowerStage"] = HasAccessory(Accessory.FlowerStage);
        ow["GoldPedestal"] = HasAccessory(Accessory.GoldPedestal);
        ow["GlassStage"] = HasAccessory(Accessory.GlassStage);
        ow["AwardPodium"] = HasAccessory(Accessory.AwardPodium);
        ow["CubeStage"] = HasAccessory(Accessory.CubeStage);
        ow["TurtwigMask"] = HasAccessory(Accessory.TURTWIGMask);
        ow["ChimcharMask"] = HasAccessory(Accessory.CHIMCHARMask);
        ow["PiplupMask"] = HasAccessory(Accessory.PIPLUPMask);
        ow["BigTree"] = HasAccessory(Accessory.BigTree);
        ow["Flag"] = HasAccessory(Accessory.Flag);
        ow["Crown"] = HasAccessory(Accessory.Crown);
        ow["Tiara"] = HasAccessory(Accessory.Tiara);
        ow["Comet"] = HasAccessory(Accessory.Comet);

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

    public bool GetWallpaperUnlocked(byte id)
    {
        return FlagUtil.GetFlag(sav.Storage, 0x121C6 + id / 8, id % 8);
    }
    public void Generate_pcWallPaper()
    {
        var ow = new Dictionary<string, bool>();
        owned["pcWallPaper"] = ow;

        var HasWallPaper = (byte id) =>
        {
            if (!this.living)
                return GetWallpaperUnlocked(id);

            for (int i = 0; i < this.sav.BoxCount; i++)
                if (this.sav.GetBoxWallpaper(i) == id)
                    return true;

            return false;
        };

        ow["Distortion"] = HasWallPaper(0);
        ow["Contest"] = HasWallPaper(1);
        ow["Nostalgic"] = HasWallPaper(2);
        ow["Croagunk"] = HasWallPaper(3);
        ow["Trio"] = HasWallPaper(4);
        ow["PikaPika"] = HasWallPaper(5);
        ow["Legend"] = HasWallPaper(6);
        ow["TeamGalactic"] = HasWallPaper(7);
    }


    public bool GetVillaFurniturePurchased(VillaFurniture index)
    {
        return FlagUtil.GetFlag(sav.General, 0x111F + (byte)index / 8, (byte)index % 8);
    }

    public void Generate_villaFurniture()
    {
        var ow = new Dictionary<string, bool>();
        owned["villaFurniture"] = ow;

        ow["Table"] = true;
        ow["BigSofa"] = GetVillaFurniturePurchased(VillaFurniture.BigSofa);
        ow["SmallSofa"] = GetVillaFurniturePurchased(VillaFurniture.SmallSofa);
        ow["Bed"] = GetVillaFurniturePurchased(VillaFurniture.Bed);
        ow["NightTable"] = GetVillaFurniturePurchased(VillaFurniture.NightTable);
        ow["TV"] = GetVillaFurniturePurchased(VillaFurniture.TV);
        ow["AudioSystem"] = GetVillaFurniturePurchased(VillaFurniture.AudioSystem);
        ow["Bookshelf"] = GetVillaFurniturePurchased(VillaFurniture.Bookshelf);
        ow["Rack"] = GetVillaFurniturePurchased(VillaFurniture.Rack);
        ow["Houseplant"] = GetVillaFurniturePurchased(VillaFurniture.Houseplant);
        ow["PCDesk"] = GetVillaFurniturePurchased(VillaFurniture.PCDesk);
        ow["MusicBox"] = GetVillaFurniturePurchased(VillaFurniture.MusicBox);
        ow["PokemonBust1"] = GetVillaFurniturePurchased(VillaFurniture.PokemonBust1);
        ow["PokemonBust2"] = GetVillaFurniturePurchased(VillaFurniture.PokemonBust2);
        ow["Piano"] = GetVillaFurniturePurchased(VillaFurniture.Piano);
        ow["GuestSet"] = GetVillaFurniturePurchased(VillaFurniture.GuestSet);
        ow["WallClock"] = GetVillaFurniturePurchased(VillaFurniture.WallClock);
        ow["Masterpiece"] = GetVillaFurniturePurchased(VillaFurniture.Masterpiece);
        ow["TeaSet"] = GetVillaFurniturePurchased(VillaFurniture.TeaSet);
        ow["Chandelier"] = GetVillaFurniturePurchased(VillaFurniture.Chandelier);
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

    public bool GetBackdropUnlocked(Backdrop backdrop)
    {
        return sav.General[0x4E60 + (byte)backdrop] != 0x12;
    }

    public void Generate_backdrop()
    {
        var ow = new Dictionary<string, bool>();
        owned["backdrop"] = ow;

        ow["DressUp"] = GetBackdropUnlocked(Backdrop.DressUp);
        ow["Ranch"] = GetBackdropUnlocked(Backdrop.Ranch);
        ow["CityatNight"] = GetBackdropUnlocked(Backdrop.CityatNight);
        ow["SnowyTown"] = GetBackdropUnlocked(Backdrop.SnowyTown);
        ow["Fiery"] = GetBackdropUnlocked(Backdrop.Fiery);
        ow["OuterSpace"] = GetBackdropUnlocked(Backdrop.OuterSpace);
        ow["Desert"] = GetBackdropUnlocked(Backdrop.Desert);
        ow["CumulusCloud"] = GetBackdropUnlocked(Backdrop.CumulusCloud);
        ow["FlowerPatch"] = GetBackdropUnlocked(Backdrop.FlowerPatch);
        ow["FutureRoom"] = GetBackdropUnlocked(Backdrop.FutureRoom);
        ow["OpenSea"] = GetBackdropUnlocked(Backdrop.OpenSea);
        ow["TotalDarkness"] = GetBackdropUnlocked(Backdrop.TotalDarkness);
        ow["TatamiRoom"] = GetBackdropUnlocked(Backdrop.TatamiRoom);
        ow["GingerbreadRoom"] = GetBackdropUnlocked(Backdrop.GingerbreadRoom);
        ow["Seafloor"] = GetBackdropUnlocked(Backdrop.Seafloor);
        ow["Underground"] = GetBackdropUnlocked(Backdrop.Underground);
        ow["Sky"] = GetBackdropUnlocked(Backdrop.Sky);
        ow["Theater"] = GetBackdropUnlocked(Backdrop.Theater);
    }


    public bool IsBattleFrontierPrintObtained(int index, int wantedVal)
    {
        var currentVal = ReadInt32LittleEndian(sav.General[(0xE4A + (index << 1))..]);
        return currentVal >= wantedVal;
    }

    public void Generate_battleFrontier()
    {
        var ow = new Dictionary<string, bool>();
        owned["battleFrontier"] = ow;


        ow["BattleTowerSilver"] = IsBattleFrontierPrintObtained(0, 1);
        ow["BattleFactorySilver"] = IsBattleFrontierPrintObtained(1, 1);
        ow["BattleHallSilver"] = IsBattleFrontierPrintObtained(2, 1);
        ow["BattleCastleSilver"] = IsBattleFrontierPrintObtained(3, 1);
        ow["BattleArcadeSilver"] = IsBattleFrontierPrintObtained(4, 1);
        ow["BattleTowerGold"] = IsBattleFrontierPrintObtained(0, 2);
        ow["BattleFactoryGold"] = IsBattleFrontierPrintObtained(1, 2);
        ow["BattleHallGold"] = IsBattleFrontierPrintObtained(2, 2);
        ow["BattleCastleGold"] = IsBattleFrontierPrintObtained(3, 2);
        ow["BattleArcadeGold"] = IsBattleFrontierPrintObtained(4, 2);
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

        ow["ChampionRibbon"] = HasRibbon(pk4 => pk4.RibbonChampionG3);
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

        ow["HallofFame"] = sav.GetEventFlag(2404);
        ow["PokemonContest"] = sav.GetEventFlag(2408) && sav.GetEventFlag(2409) && sav.GetEventFlag(2410) && sav.GetEventFlag(2411) && sav.GetEventFlag(2412);
        ow["Underground"] = sav.UG_Flags >= 50;
        ow["BattleTower"] = IsBattleFrontierPrintObtained(0, 2);

    }
    
    public void Generate_easyChatSystemWord()
    {
        var ow = new Dictionary<string, bool>();
        owned["easyChatSystemWord"] = ow;

        //0xCEB4 is start

        ow["ToughWordsArtery"] = false; //TODO
        ow["ToughWordsBoneDensity"] = false; //TODO
        ow["ToughWordsCadenza"] = false; //TODO
        ow["ToughWordsConductivity"] = false; //TODO
        ow["ToughWordsContour"] = false; //TODO
        ow["ToughWordsCopyright"] = false; //TODO
        ow["ToughWordsCrossStitch"] = false; //TODO
        ow["ToughWordsCubism"] = false; //TODO
        ow["ToughWordsEarthTones"] = false; //TODO
        ow["ToughWordsEducation"] = false; //TODO
        ow["ToughWordsFlambe"] = false; //TODO
        ow["ToughWordsFractals"] = false; //TODO
        ow["ToughWordsGMT"] = false; //TODO
        ow["ToughWordsGoldenRatio"] = false; //TODO
        ow["ToughWordsGommage"] = false; //TODO
        ow["ToughWordsHowling"] = false; //TODO
        ow["ToughWordsImplant"] = false; //TODO
        ow["ToughWordsIrritability"] = false; //TODO
        ow["ToughWordsMoneyRate"] = false; //TODO
        ow["ToughWordsNeutrino"] = false; //TODO
        ow["ToughWordsOmnibus"] = false; //TODO
        ow["ToughWordsPH Balance"] = false; //TODO
        ow["ToughWordsPolyphenol"] = false; //TODO
        ow["ToughWordsREMSleep"] = false; //TODO
        ow["ToughWordsResolution"] = false; //TODO
        ow["ToughWordsSpreadsheet"] = false; //TODO
        ow["ToughWordsStarboard"] = false; //TODO
        ow["ToughWordsStockPrices"] = false; //TODO
        ow["ToughWordsStreaming"] = false; //TODO
        ow["ToughWordsTwoStep"] = false; //TODO
        ow["ToughWordsUbiquitous"] = false; //TODO
        ow["ToughWordsVector"] = false; //TODO
    }

    public void Generate_geonet()
    {
        var ow = new Dictionary<string, bool>();
        owned["geonet"] = ow;
        Geonet4 geonet = new (sav);

        foreach (var country in Geonet4.LegalCountries)
        {
            var subRegionCount = Geonet4.GetSubregionCount(country);
            if (subRegionCount == 0)
                ow[$"{country}-0"] = geonet.GetCountrySubregion(country, 0) != Geonet4.Point.None;
            else 
            { 
                for (byte i = 1; i <= subRegionCount; i++)
                    ow[$"{country}-{i}"] = geonet.GetCountrySubregion(country, i) != Geonet4.Point.None;
            }
        }
    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;

        ow["RegisteredaGeonetlocation"] = sav.GeonetGlobalFlag;
        ow["DefeatEliteFourAfterNationalDex"] = sav.GetEventFlag(0x08B5); // TR_CHAMPION_02
        ow["DefeatRivalLevel85Rematch"] = sav.GetEventFlag(0x08EA) || sav.GetEventFlag(0x08EB) || sav.GetEventFlag(0x08EC); // TR_RIVAL_31-33
        ow["3500ScoreinCatchingShow"] = sav.GetWork(0224) >= 3500;
        ow["UnlockPokedexForeignEntries"] = sav.GetAllPKM().Any(pk => pk.Language != this.sav.Language);
        ow["UnlockMysteryGift"] = (sav.General[72] & 1) == 1;
        ow["SignyourTrainerCard"] = sav.General[0x61A7] != 0; // not really accurate
    }
}
