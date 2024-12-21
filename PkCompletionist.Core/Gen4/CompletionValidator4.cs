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
    public CompletionValidator4(Command command, SAV4Pt sav, Objective objective) : base(command, sav, objective)
    {
        this.sav = sav;

        this.unobtainableItems = new List<int>() { };

    }

    new SAV4Pt sav;

    public override void GenerateAll()
    {
        base.GenerateAll();

        Generate_pokemonForm();
        Generate_itemInMap();
        Generate_itemGift();
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
        Generate_jubilifeTvRankings();

        //Generate_geonet();
        Generate_battle();
        Generate_misc();
    }

    public void Generate_pokemonForm()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemonForm"] = ow;

        ow["UnownA"] = HasPkmForm2(201, 0);
        ow["UnownB"] = HasPkmForm2(201, 1);
        ow["UnownC"] = HasPkmForm2(201, 2);
        ow["UnownD"] = HasPkmForm2(201, 3);
        ow["UnownE"] = HasPkmForm2(201, 4);
        ow["UnownF"] = HasPkmForm2(201, 5);
        ow["UnownG"] = HasPkmForm2(201, 6);
        ow["UnownH"] = HasPkmForm2(201, 7);
        ow["UnownI"] = HasPkmForm2(201, 8);
        ow["UnownJ"] = HasPkmForm2(201, 9);
        ow["UnownK"] = HasPkmForm2(201, 10);
        ow["UnownL"] = HasPkmForm2(201, 11);
        ow["UnownM"] = HasPkmForm2(201, 12);
        ow["UnownN"] = HasPkmForm2(201, 13);
        ow["UnownO"] = HasPkmForm2(201, 14);
        ow["UnownP"] = HasPkmForm2(201, 15);
        ow["UnownQ"] = HasPkmForm2(201, 16);
        ow["UnownR"] = HasPkmForm2(201, 17);
        ow["UnownS"] = HasPkmForm2(201, 18);
        ow["UnownT"] = HasPkmForm2(201, 19);
        ow["UnownU"] = HasPkmForm2(201, 20);
        ow["UnownV"] = HasPkmForm2(201, 21);
        ow["UnownW"] = HasPkmForm2(201, 22);
        ow["UnownX"] = HasPkmForm2(201, 23);
        ow["UnownY"] = HasPkmForm2(201, 24);
        ow["UnownZ"] = HasPkmForm2(201, 25);
        ow["UnownExclamationMark"] = HasPkmForm2(201, 26);
        ow["UnownQuestionMark"] = HasPkmForm2(201, 27);

        ow["CastformNormal"] = HasOrSeenPkmBasedOnObjective(351);
        ow["CastformFire"] = HasOrSeenPkmBasedOnObjective(351);
        ow["CastformWater"] = HasOrSeenPkmBasedOnObjective(351);
        ow["CastformIce"] = HasOrSeenPkmBasedOnObjective(351);

        ow["DeoxysNormal"] = HasPkmForm2(386, 0);
        ow["DeoxysAttack"] = HasPkmForm2(386, 1);
        ow["DeoxysDefense"] = HasPkmForm2(386, 2);
        ow["DeoxysSpeed"] = HasPkmForm2(386, 3);
        ow["BurmyPlantCloak"] = HasPkmForm2(412, 0);
        ow["BurmySandyCloak"] = HasPkmForm2(412, 1);
        ow["BurmyTrashCloak"] = HasPkmForm2(412, 2);
        ow["WormadamPlantCloak"] = HasPkmForm2(413, 0);
        ow["WormadamSandyCloak"] = HasPkmForm2(413, 1);
        ow["WormadamTrashCloak"] = HasPkmForm2(413, 2);
        ow["CherrimOvercast"] = HasOrSeenPkmBasedOnObjective(421);
        ow["CherrimSunshine"] = HasOrSeenPkmBasedOnObjective(421);
        ow["ShellosWestSea"] = HasPkmForm2(422, 0);
        ow["ShellosEastSea"] = HasPkmForm2(422, 1);
        ow["GastrodonWestSea"] = HasPkmForm2(423, 0);
        ow["GastrodonEastSea"] = HasPkmForm2(423, 1);
        ow["RotomGhost"] = HasPkmForm2(479, 0);
        ow["RotomHeat"] = HasPkmForm2(479, 1);
        ow["RotomWash"] = HasPkmForm2(479, 2);
        ow["RotomFrost"] = HasPkmForm2(479, 3);
        ow["RotomFan"] = HasPkmForm2(479, 4);
        ow["RotomMow"] = HasPkmForm2(479, 5);
        ow["GiratinaAltered"] = HasPkmForm2(487, 0);
        ow["GiratinaOrigin"] = HasPkmForm2(488, 1);
        ow["ShayminLand"] = HasPkmForm2(492, 0);
        ow["ShayminSky"] = HasPkmForm2(492, 1);

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
    public bool HasPkmForm2(ushort species, byte form)
    {
        if (HasPkmForm(species, form))
            return true;

        return this.sav.Dex.GetForms(species).Contains(form);
    }

    public override void Generate_item()
    {
        base.Generate_item();

        var ow = this.owned["item"];

        //TODO: add exception if not living
        /*
         * 
        impossible to keep:
        Yes	5	Safari Ball		Great Marsh
        Yes	439	Old Charm		Route 210 (from Cynthia after curing the Psyduck)
        Yes	451	Suite Key		Route 213 (immediately northwest of the northern reception entrance of Hotel Grand Lake)
        Yes	453	Lunar Wing		Fullmoon Island (left behind by Cresselia when it leaves)
        Yes	459	Parcel		Twinleaf Town (from Barry's mother after returning from Sandgem Town)
        Yes	460	Coupon 1		Jubilife City
        Yes	461	Coupon 2		Jubilife City
        Yes	462	Coupon 3		Jubilife City
        Yes	500	Park Ball		Pal Park
        */
    }

    public void Generate_itemInMap()
    {
        var ow = new Dictionary<string, bool>();
        owned["itemInMap"] = ow;

        var list = new List<int> { 1051, 1264, 1032, 1138, 1268, 746, 750, 764, 772, 779, 864, 885, 903, 910, 944, 993, 759, 778, 857, 915, 948, 1183, 1282, 1126, 798, 819, 939, 1082, 1279, 1289, 1295,
    1307, 1323, 1047, 771, 790, 872, 1102, 1136, 1237, 1256, 1328, 773, 841, 921, 1062, 1141, 1189, 1221, 1331, 1222, 756, 1061, 1217, 1210, 1274, 1193, 732, 1334, 1261, 1216, 1312, 1154, 740,
    1125, 1206, 1020, 1333, 808, 1070, 1152, 1290, 792, 1001, 999, 1000, 1021, 1050, 1150, 1163, 1187, 1227, 734, 1064, 1099, 1283, 766, 998, 1137, 1118, 869, 755, 769, 824, 929, 955, 1073, 1121,
    1173, 1226, 1257, 1123, 816, 832, 839, 846, 874, 909, 1168, 1190, 1201, 1205, 1229, 1245, 1311, 1251, 733, 739, 762, 881, 918, 1063, 1084, 1086, 1100, 797, 835, 838, 892, 1035, 1085, 1288, 1297,
    1313, 1321, 1056, 1336, 1037, 1250, 1112, 1175, 760, 787, 852, 1024, 1157, 1214, 1260, 1285, 1339, 1030, 828, 834, 842, 848, 853, 856, 865, 914, 916, 987, 995, 996, 997, 788, 789, 897, 917,
    949, 950, 951, 952, 754, 780, 882, 919, 943, 953, 1077, 1101, 1117, 1119, 1147, 1276, 731, 802, 871, 913, 1059, 1176, 1239, 1287, 1330, 1231, 786, 1003, 1220, 1215, 933, 942, 1248, 1211, 1225,
    1166, 1026, 1106, 1053, 1072, 1332, 1303, 730, 830, 849, 1169, 1182, 1200, 1224, 1232, 749, 1054, 1120, 806, 820, 837, 928, 1155, 1305, 753, 813, 867, 925, 936, 945, 1114, 1128, 1142, 1151,
    1186, 1196, 803, 825, 845, 896, 904, 905, 922, 957, 1092, 1181, 1184, 1236, 774, 1172, 1337, 1109, 1160, 818, 958, 1088, 1116, 1034, 1338, 793, 809, 843, 876, 930, 1004, 1005, 1006, 1111, 1207,
    1233, 1246, 1110, 931, 1036, 1105, 863, 937, 1319, 737, 770, 801, 804, 861, 956, 1045, 1094, 1228, 1284, 1315, 1033, 1096, 747, 758, 817, 855, 894, 895, 900, 988, 994, 1044, 821, 1016, 1057,
    1081, 1083, 1249, 805, 1014, 1022, 1263, 1265, 1266, 1272, 1149, 1238, 776, 893, 927, 1060, 1095, 1144, 1244, 1310, 1115, 1314, 742, 751, 799, 807, 831, 836, 847, 854, 859, 860, 884, 889, 908,
    935, 940, 1055, 1124, 1129, 1146, 1164, 1197, 1209, 1213, 1234, 1247, 1281, 1291, 1300, 1309, 1316, 1329, 810, 1199, 1208, 898, 1212, 1243, 1324, 738, 795, 811, 1087, 1127, 1293, 1296, 1301,
    1326, 1318, 1015, 1140, 924, 990, 992, 765, 911, 954, 986, 1002, 1049, 1058, 1079, 1107, 1170, 1218, 1292, 1304, 1306, 1135, 1069, 1023, 1241, 775, 1158, 1240, 1043, 850, 1131, 1165, 1139,
    1067, 744, 777, 781, 782, 783, 784, 785, 796, 800, 812, 814, 822, 866, 868, 873, 906, 989, 1286, 741, 743, 763, 794, 815, 826, 833, 870, 891, 947, 829, 946, 761, 823, 880, 938, 1040, 1159,
    1267, 883, 1046, 1113, 1017, 1188, 1027, 1089, 1078, 1174, 1028, 1068, 1041, 1171, 1255, 1194, 1104, 1180, 1153, 1029, 1254, 1052, 1108, 1130, 1161, 1133, 1018, 1048, 1122, 1134, 1178, 1242,
    1253, 1025, 1090, 1195, 1066, 1065, 1038, 1103, 1179, 1230, 1219, 1204, 1075, 1162, 1019, 1203, 1202, 1185, 1145, 1042, 1074, 1097, 1071, 1148, 1262, 768, 875, 1191, 1335, 735, 745, 862, 887,
    890, 941, 748, 752, 844, 851, 858, 877, 879, 886, 888, 902, 907, 912, 920, 923, 926, 932, 934, 991, 1098, 1143, 1156, 1167, 1177, 1192, 1235, 1299, 1308, 1320, 1325, 1327, 1280, 1317, 767,
    878, 1093, 1132, 1271, 1273, 1270, 1278, 1031, 1277, 736, 791, 827, 1080, 1091, 1275, 1298, 1302, 1322, 840, 757, 899, 901, 1076, 1198, 1223, 1258, 1294, };

        foreach (var id in list)
            ow[id.ToString()] = sav.GetEventFlag(id);

        var defaultBerryIdList = new List<int> { 7, 1, 2, 3, 7, 3, 16, 17, 1, 7, 10, 19, 19, 22, 1, 3, 7, 7, 4, 4, 16, 16, 1, 7, 7, 17, 18, 16, 17, 20, 6, 2, 16, 16, 8, 18, 18, 11, 5, 5, 16, 20, 10, 2,
    12, 14, 3, 5, 15, 25, 10, 5, 26, 9, 3, 20, 20, 20, 8, 8, 18, 18, 14, 15, 4, 4, 1, 10, 2, 21, 3, 17, 12, 13, 4, 8, 11, 20, 6, 3, 13, 24, 12, 13, 14, 23, 10, 17, 18, 19, 21, 21, 24, 24, 22, 22,
    26, 26, 23, 23, 21, 21, 24, 24, 26, 26, 25, 25, 23, 23, 9, 6, 23, 25, 22, 22, 25, 25 };

        for (var i = 0; i < defaultBerryIdList.Count(); i++)
        {
            var berryIdx = i;
            var defaultBerry = defaultBerryIdList[i];
            ow["b" + berryIdx.ToString()] = HasPickedBerry(berryIdx, defaultBerry + 1);
        }
    }

    public bool HasPickedBerry(int berryIdx, int defaultBerry)
    {
        var berry = sav.General[0x20c4 + 14 * berryIdx];
        return berry != defaultBerry;
    }

    public void Generate_itemGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["itemGift"] = ow;
        var list = new List<int> { 108, 116, 117, 124, 125, 131, 140, 146, 152, 156, 157, 158, 160, 161, 182, 193, 194, 197, 198, 199, 201, 202, 203, 204, 205, 206, 213, 217, 218, 261, 265, 266,
    267, 278, 281, 284, 285, 302, 303, 308, 309, 310, 312, 313, 314, 315, 319, 320, 321, 322, 323, 324, 352, 2721, };

        foreach (var id in list)
            ow[id.ToString()] = sav.GetEventFlag(id);
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
        if (accessory < Accessory.ColoredParasol)
        {
            var enumIdx = (byte)accessory;
            var val = sav.General[0x4E38 + enumIdx / 2];
            if (enumIdx % 2 == 0)
                return (val & 0x0F) != 0;
            return (val & 0xF0) != 0;
        }

        var diff = (byte)accessory - (byte)Accessory.ColoredParasol;
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
            if (this.objective != Objective.living)
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

    public bool GetThoughWordUnlocked(ThoughWord word)
    {
        var idx = (byte)word;
        return FlagUtil.GetFlag(sav.General, 0xCEB4 + idx / 8, idx % 8);
    }
    public void Generate_easyChatSystemWord()
    {
        var ow = new Dictionary<string, bool>();
        owned["easyChatSystemWord"] = ow;

        ow["ToughWordsArtery"] = GetThoughWordUnlocked(ThoughWord.Artery);
        ow["ToughWordsBoneDensity"] = GetThoughWordUnlocked(ThoughWord.BoneDensity);
        ow["ToughWordsCadenza"] = GetThoughWordUnlocked(ThoughWord.Cadenza);
        ow["ToughWordsConductivity"] = GetThoughWordUnlocked(ThoughWord.Conductivity);
        ow["ToughWordsContour"] = GetThoughWordUnlocked(ThoughWord.Contour);
        ow["ToughWordsCopyright"] = GetThoughWordUnlocked(ThoughWord.Copyright);
        ow["ToughWordsCrossStitch"] = GetThoughWordUnlocked(ThoughWord.CrossStitch);
        ow["ToughWordsCubism"] = GetThoughWordUnlocked(ThoughWord.Cubism);
        ow["ToughWordsEarthTones"] = GetThoughWordUnlocked(ThoughWord.EarthTones);
        ow["ToughWordsEducation"] = GetThoughWordUnlocked(ThoughWord.Education);
        ow["ToughWordsFlambe"] = GetThoughWordUnlocked(ThoughWord.Flambe);
        ow["ToughWordsFractals"] = GetThoughWordUnlocked(ThoughWord.Fractals);
        ow["ToughWordsGMT"] = GetThoughWordUnlocked(ThoughWord.GMT);
        ow["ToughWordsGoldenRatio"] = GetThoughWordUnlocked(ThoughWord.GoldenRatio);
        ow["ToughWordsGommage"] = GetThoughWordUnlocked(ThoughWord.Gommage);
        ow["ToughWordsHowling"] = GetThoughWordUnlocked(ThoughWord.Howling);
        ow["ToughWordsImplant"] = GetThoughWordUnlocked(ThoughWord.Implant);
        ow["ToughWordsIrritability"] = GetThoughWordUnlocked(ThoughWord.Irritability);
        ow["ToughWordsMoneyRate"] = GetThoughWordUnlocked(ThoughWord.MoneyRate);
        ow["ToughWordsNeutrino"] = GetThoughWordUnlocked(ThoughWord.Neutrino);
        ow["ToughWordsOmnibus"] = GetThoughWordUnlocked(ThoughWord.Omnibus);
        ow["ToughWordsPHBalance"] = GetThoughWordUnlocked(ThoughWord.PHBalance);
        ow["ToughWordsPolyphenol"] = GetThoughWordUnlocked(ThoughWord.Polyphenol);
        ow["ToughWordsREMSleep"] = GetThoughWordUnlocked(ThoughWord.REMSleep);
        ow["ToughWordsResolution"] = GetThoughWordUnlocked(ThoughWord.Resolution);
        ow["ToughWordsSpreadsheet"] = GetThoughWordUnlocked(ThoughWord.Spreadsheet);
        ow["ToughWordsStarboard"] = GetThoughWordUnlocked(ThoughWord.Starboard);
        ow["ToughWordsStockPrices"] = GetThoughWordUnlocked(ThoughWord.StockPrices);
        ow["ToughWordsStreaming"] = GetThoughWordUnlocked(ThoughWord.Streaming);
        ow["ToughWordsTwoStep"] = GetThoughWordUnlocked(ThoughWord.TwoStep);
        ow["ToughWordsUbiquitous"] = GetThoughWordUnlocked(ThoughWord.Ubiquitous);
        ow["ToughWordsVector"] = GetThoughWordUnlocked(ThoughWord.Vector);

        var HasSeenMonList = () =>
        {
            var list = new List<ushort> { 16, 17, 18, 19, 20, 21, 22, 25, 26, 29, 30, 31, 32, 33, 34, 35, 36, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 60, 61, 62, 63, 64, 66, 67, 69, 70, 71, 72, 73, 74, 75, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 201, 202, 203, 206, 207, 208, 209, 210, 211, 214, 215, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 231, 232, 234, 235, 236, 237, 238, 239, 240, 241, 242, 246, 247, 248, 261, 262, 263, 264, 265, 266, 267, 268, 269, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 318, 319, 320, 321, 322, 323, 324, 325, 326, 327, 331, 332, 333, 334, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 369, 370, 371, 372, 373, 374, 375, 376, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 433, 436, 437, 438, 439, 440, 441, 443, 444, 445, 446, 447, 448, 449, 450, 451, 452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463, 465, 468, 469, 470, 471, 472, 473, 475, 476, 478, 479, 480, 481, 482, 483, 484, 485, 487, 488 };
            foreach (var mon in list)
                if (!this.sav.Dex.GetSeen(mon))
                    return false;
            return true;
        };

        var HasSeenMon = (ushort mon) =>
        {
            return this.sav.Dex.GetSeen(mon);
        };

        var HasSeenMonAll = () =>
        {
            for (ushort mon = 1; mon <= 493; mon++)
                if (!this.sav.Dex.GetSeen(mon))
                    return false;
            return true;
        };

        ow["PokemonAllCatchableNoTrade"] = HasSeenMonList();
        ow["PokemonButterfree"] = HasSeenMon(12);
        ow["PokemonArbok"] = HasSeenMon(24);
        ow["PokemonSandslash"] = HasSeenMon(28);
        ow["PokemonNinetales"] = HasSeenMon(38);
        ow["PokemonArcanine"] = HasSeenMon(59);
        ow["PokemonAlakazam"] = HasSeenMon(65);
        ow["PokemonMachamp"] = HasSeenMon(68);
        ow["PokemonGolem"] = HasSeenMon(76);
        ow["PokemonGengar"] = HasSeenMon(94);
        ow["PokemonPolitoed"] = HasSeenMon(186);
        ow["PokemonMurkrow"] = HasSeenMon(198);
        ow["PokemonSlowking"] = HasSeenMon(199);
        ow["PokemonMisdreavus"] = HasSeenMon(200);
        ow["PokemonForretress"] = HasSeenMon(205);
        ow["PokemonScizor"] = HasSeenMon(212);
        ow["PokemonShuckle"] = HasSeenMon(213);
        ow["PokemonUrsaring"] = HasSeenMon(217);
        ow["PokemonKingdra"] = HasSeenMon(230);
        ow["PokemonPorygon2"] = HasSeenMon(233);
        ow["PokemonLombre"] = HasSeenMon(271);
        ow["PokemonLudicolo"] = HasSeenMon(272);
        ow["PokemonNuzleaf"] = HasSeenMon(274);
        ow["PokemonShiftry"] = HasSeenMon(275);
        ow["PokemonTrapinch"] = HasSeenMon(328);
        ow["PokemonVibrava"] = HasSeenMon(329);
        ow["PokemonFlygon"] = HasSeenMon(330);
        ow["PokemonZangoose"] = HasSeenMon(335);
        ow["PokemonSeviper"] = HasSeenMon(336);
        ow["PokemonLunatone"] = HasSeenMon(337);
        ow["PokemonSolrock"] = HasSeenMon(338);
        ow["PokemonOthers"] = HasSeenMonAll();
        ow["Move"] = sav.GetEventFlag(0x08B5); // DefeatEliteFourAfterNationalDex
    }

    public void Generate_geonet()
    {
        var ow = new Dictionary<string, bool>();
        owned["geonet"] = ow;
        Geonet4 geonet = new(sav);

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
    public void Generate_jubilifeTvRankings()
    {
        var ow = new Dictionary<string, bool>();
        owned["jubilifeTvRankings"] = ow;

        ow["PokemonDefeated"] = true;
        ow["PokemonCaught"] = true;
        ow["PokemonEggs"] = sav.GetEventFlag(2404); // HallOfFame, kinda bad
        ow["EncounteredwhileFishing"] = true;

        ow["SingleBattles"] = sav.General[0x68e0] > 0;
        ow["DoubleBattles"] = sav.General[0x68e4] > 0;
        ow["MultiBattles"] = sav.General[0x68e8] > 0;
        ow["LinkMultiBattles"] = sav.General[0x68ec] > 0;
        ow["WiFiBattles"] = sav.General[0x68f0] > 0;
        ow["AvgWinStreak"] = sav.General[0x68e0] > 0;

        var HasBeatenContest = HasRibbon(pk4 =>
        {
            return pk4.RibbonG4Cool || pk4.RibbonG4CoolGreat || pk4.RibbonG4CoolUltra || pk4.RibbonG4CoolMaster || pk4.RibbonG4Beauty || pk4.RibbonG4BeautyGreat || pk4.RibbonG4BeautyUltra
                || pk4.RibbonG4BeautyMaster || pk4.RibbonG4Cute || pk4.RibbonG4CuteGreat || pk4.RibbonG4CuteUltra || pk4.RibbonG4CuteMaster || pk4.RibbonG4Smart || pk4.RibbonG4SmartGreat ||
                pk4.RibbonG4SmartUltra || pk4.RibbonG4SmartMaster || pk4.RibbonG4Tough || pk4.RibbonG4ToughGreat || pk4.RibbonG4ToughUltra || pk4.RibbonG4ToughMaster;
        });

        ow["ContestWins"] = HasBeatenContest;
        ow["ContestWinsPct"] = HasBeatenContest;
        ow["Ribbons"] = HasRibbon(pk4 =>
        {
            return pk4.RibbonChampionG3 || pk4.RibbonChampionSinnoh || pk4.RibbonG3Cool || pk4.RibbonG3CoolSuper || pk4.RibbonG3CoolHyper || pk4.RibbonG3CoolMaster || pk4.RibbonG3Beauty ||
            pk4.RibbonG3BeautySuper || pk4.RibbonG3BeautyHyper || pk4.RibbonG3BeautyMaster || pk4.RibbonG3Cute || pk4.RibbonG3CuteSuper || pk4.RibbonG3CuteHyper || pk4.RibbonG3CuteMaster ||
            pk4.RibbonG3Smart || pk4.RibbonG3SmartSuper || pk4.RibbonG3SmartHyper || pk4.RibbonG3SmartMaster || pk4.RibbonG3Tough || pk4.RibbonG3ToughSuper || pk4.RibbonG3ToughHyper ||
            pk4.RibbonG3ToughMaster || pk4.RibbonG4Cool || pk4.RibbonG4CoolGreat || pk4.RibbonG4CoolUltra || pk4.RibbonG4CoolMaster || pk4.RibbonG4Beauty || pk4.RibbonG4BeautyGreat ||
            pk4.RibbonG4BeautyUltra || pk4.RibbonG4BeautyMaster || pk4.RibbonG4Cute || pk4.RibbonG4CuteGreat || pk4.RibbonG4CuteUltra || pk4.RibbonG4CuteMaster || pk4.RibbonG4Smart ||
            pk4.RibbonG4SmartGreat || pk4.RibbonG4SmartUltra || pk4.RibbonG4SmartMaster || pk4.RibbonG4Tough || pk4.RibbonG4ToughGreat || pk4.RibbonG4ToughUltra || pk4.RibbonG4ToughMaster
            || pk4.RibbonWinning || pk4.RibbonVictory || pk4.RibbonAbility || pk4.RibbonAbilityGreat || pk4.RibbonAbilityDouble || pk4.RibbonAbilityMulti || pk4.RibbonAbilityPair ||
            pk4.RibbonAbilityWorld || pk4.RibbonArtist || pk4.RibbonEffort || pk4.RibbonAlert || pk4.RibbonShock || pk4.RibbonDowncast || pk4.RibbonCareless || pk4.RibbonRelax || pk4.RibbonSnooze
            || pk4.RibbonSmile || pk4.RibbonGorgeous || pk4.RibbonRoyal || pk4.RibbonGorgeousRoyal || pk4.RibbonFootprint || pk4.RibbonRecord || pk4.RibbonCountry || pk4.RibbonNational ||
            pk4.RibbonEarth || pk4.RibbonWorld || pk4.RibbonClassic;
        });
    }

    public void Generate_battle()
    {
        var ow = new Dictionary<string, bool>();
        owned["battle"] = ow;
        Geonet4 geonet = new(sav);

        for (var i = 1; i < 928; i++)
            ow[i.ToString()] = sav.GetEventFlag(0x550 + i);

        var list = new List<int> { 208, 209, 288, 291, 329, 344, 479, 480, 481, 579, 591, 592 };

        foreach (var id in list)
            ow["p" + id.ToString()] = sav.GetEventFlag(id);
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
