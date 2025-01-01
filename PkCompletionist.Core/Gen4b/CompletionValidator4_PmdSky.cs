
using System.Collections.Generic;

namespace PkCompletionist.Core;

/* TODO:
    start new game
        marowak dungeon flag
    
        // TODO: spinda cafe unlocked
        ow["TinyMeadow"] = false; // TODO: recycle 25
        ow["LabyrinthCave"] = false; 

    with current game:
        special episode flag
   

    //ow["FaintedindungeonsX"] = true;  //TODO
    //ow["SuccessfulFriendRescuesX"] = true; //TODO
    //ow["PokemonevolvedX"] = true; //TODO
    //ow["PokemonEggshatchedX"] = true; //TODO
    //ow["WonbigatBigTreasureX"] = true; //TODO
    //ow["SkyGiftssentX"] = true; //TODO

     ow["ClearedallofZeroIsle"] = true; //TODO


*/

internal class CompletionValidator4_PmdSky : CompletionValidatorX
{
    public CompletionValidator4_PmdSky(Command command, SAV4_PmdSky sav, Objective objective) : base(command, sav, objective)
    {
        this.sav = sav;
        this.ownedMonList = sav.GetStoredPokemon(0);
        this.ownedItemList = sav.GetOwnedItems(0);

        this.unobtainableItems = new List<int> { 11, 12, 98, 113, 114, 138, 166, 175, 176, 177, 181, 184, 185, 194, 198, 205, 219, 224, 226, 236, 258, 259, 293, 294, 295, 296, 
            297, 298, 299, 300, 324, 339, 345, 349, 353, 360, 361, 363, 769, 825, 1352, 1353, 1354, 1355, 1356, 1357, 1358, 1359, 1360, 1361, 1362, 1363, 1364, 1365, 1366, 1367,
            1368, 1369, 1370, 1371, 1372, 1373, 1374, 1375, 1376, 1377, 1378, 1379, 1380, 1381, 1382, 1383, 1384, 1385, 1386, 1387, 1388, 1389, 1390, 1391, 1392, 1393, 1394,
            1395, 1396, 1397, 1398, 1399, };

        foreach(var box in boxList)
        {
            this.unobtainableItems.Add(box + 1);
            this.unobtainableItems.Add(box + 2);
        }

    }
    new SAV4_PmdSky sav;

    private List<int> boxList = new List<int> { 364, 367,370,373,376,379,382,385,388,391,394,397 };
    private List<int> ownedMonList = new();
    private List<int> ownedItemList = new();


    static List<List<int>> PokemonForms = new List<List<int>> {
        new List<int>{ 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228 },
        new List<int>{379,380,381,382 },
        new List<int>{418,419,420,421 },
        new List<int>{ 447, 448, 449, },
        new List<int>{ 450, 451, 452, },
        new List<int>{ 460, 461, },
        new List<int>{ 462, 463, },
        new List<int>{ 464, 465, },
        new List<int>{ 529, 536, },
        new List<int>{ 534, 535, },
    };

    public override void GenerateAll()
    {
        Generate_pokemon();
        Generate_pokemonForm();
        Generate_item();
        Generate_marowakDojo();
        Generate_rank();
        Generate_dungeon();
        Generate_adventureLog();
        Generate_specialEpisode();
        Generate_progressIcon();
    }

    public override void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        var toIgnore = new List<int>();
        var mainFormToForms = new Dictionary<int,List<int>>();
        foreach (var forms in PokemonForms)
        {
            mainFormToForms.Add(forms[0], forms);
            for (int i = 1; i < forms.Count; i++)
                toIgnore.Add(forms[i]);
        }

        for (ushort i = 1; i <= 534; i++)
        {
            if (toIgnore.Contains(i))
                continue;
            if (mainFormToForms.TryGetValue(i, out var forms))
            {
                var owned = false;
                foreach (var form in forms)
                    if (this.ownedMonList.Contains(form))
                        owned = true;
                ow[i.ToString()] = owned;
            } else 
                ow[i.ToString()] = this.ownedMonList.Contains(i);
        }
    }
    public void Generate_pokemonForm()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemonForm"] = ow;

        var untrackable = new List<int> { 380, 381, 382, 461, 536, 535 };
        foreach (var forms in PokemonForms)
        {
            foreach(var form in forms)
            {
                if (untrackable.Contains(form))
                    continue;
                ow[form.ToString()] = this.ownedMonList.Contains(form);
            }
        }
    }

    public override void Generate_item()
    {
        var ow = new Dictionary<string, bool>();
        owned["item"] = ow;

        for (var i = 1; i <= 1399; i++)
        {
            if (this.unobtainableItems.Contains(i))
                continue;
            if (this.boxList.Contains(i))
                ow[i.ToString()] = this.ownedItemList.Contains(i) || this.ownedItemList.Contains(i + 1) || this.ownedItemList.Contains(i + 2);
            else
                ow[i.ToString()] = this.ownedItemList.Contains(i);
        }
    }

    public bool HasCompletedMarowakDojo(MarowakDojo dojo)
    {
        var byteIdx = (int)dojo / 8;
        var bitIdx = (int)dojo % 8;
        return false; //TODO
    }

    public void Generate_marowakDojo()
    {
        var ow = new Dictionary<string, bool>();
        owned["MarowakDojo"] = ow;
        // ow["NormalFlyMaze"] = false; //TODO
        // ow["DarkFireMaze"] = false; //TODO
        // ow["RockWaterMaze"] = false; //TODO
        // ow["GrassMaze"] = false; //TODO
        // ow["ElecSteelMaze"] = false; //TODO
        // ow["IceGroundMaze"] = false; //TODO
        // ow["FightPsychMaze"] = false; //TODO
        // ow["PoisonBugMaze"] = false; //TODO
        // ow["DragonMaze"] = false; //TODO
        // ow["GhostMaze"] = false; //TODO
        // ow["FinalMaze"] = false; //TODO
        // ow["ExplorerMaze"] = false; //TODO
    }

    public void Generate_dungeon()
    {
        var ow = new Dictionary<string, bool>();
        owned["dungeon"] = ow;

        var Has = (int id) => this.ownedMonList.Contains(id);
        var Has2 = (int id) => this.ownedItemList.Contains(id);

        var DidStory = HasFinishedPostCreditStory();
        ow["BeachCave"] = DidStory; 
        ow["DrenchedBluff"] = DidStory; 
        ow["MtBristle"] = DidStory; 
        ow["WaterfallCave"] = DidStory; 
        ow["AppleWoods"] = DidStory; 
        ow["SidePath"] = DidStory; 
        ow["CraggyCoast"] = DidStory; 
        ow["RockPath"] = DidStory; 
        ow["MtHorn"] = DidStory; 
        ow["ForestPath"] = DidStory; 
        ow["FoggyForest"] = DidStory; 
        ow["SteamCave"] = DidStory; 
        ow["AmpPlains"] = DidStory; 
        ow["NorthernDesert"] = DidStory; 
        ow["QuicksandCave"] = DidStory; 
        ow["CrystalCave"] = DidStory; 
        ow["CrystalCrossing"] = DidStory; 
        ow["ChasmCave"] = DidStory; 
        ow["DarkHill"] = DidStory; 
        ow["SealedRuin"] = DidStory; 
        ow["DuskForest"] = DidStory; 
        ow["DeepDuskForest"] = DidStory; 
        ow["TreeshroudForest"] = DidStory; 
        ow["BrineCave"] = DidStory; 
        ow["HiddenLand"] = DidStory; 
        ow["TemporalTower"] = DidStory; 
        ow["MystifyingForest"] = DidStory; 
        ow["BlizzardIsland"] = DidStory; 
        ow["CreviceCave"] = DidStory; 
        ow["SurroundedSea"] = DidStory; 
        ow["MiracleSea"] = DidStory; 
        ow["AegisCave"] = DidStory; 
        ow["MtTravail"] = DidStory; 
        ow["TheNightmare"] = DidStory; 
        ow["SpacialRift"] = DidStory; 
        ow["DarkCrater"] = DidStory;
        ow["ConcealedRuins"] = Has(385); // Shuppet
        ow["MarineResort"] = Has(392); // Wynaut
        ow["BottomlessSea"] = Has2(62); // 	Aqua-Monica
        ow["ShimmerDesert"] = Has2(61); // Terra Cymbal
        ow["MtAvalanche"] = Has2(59); // Icy Flute
        ow["GiantVolcano"] = Has2(60); // Fiery Drum
        ow["WorldAbyss"] = Has2(63); // Rock Horn
        ow["SkyStairway"] = Has2(65); // Sky Melodica
        ow["MysteryJungle"] = Has2(64); // Grass Cornet
        ow["SerenityRiver"] = Has(312); // Masquerain. BAD because can evolve Surskit
        ow["LandslideCave"] = Has(485); // Gible
        ow["LushPrairie"] = Has(448); // Burmy - Plant	
        //ow["TinyMeadow"] = false; // TODO: recycle 25
        //ow["LabyrinthCave"] = false; // TODO: spinda cafe unlocked
        ow["OranForest"] = Has(449); // Burmy - Trash
        ow["LakeAfar"] = Has(7); // Squirtle
        ow["HappyOutlook"] = Has(175); // Togepi
        ow["MtMistral"] = Has(277); // Ho-Oh
        ow["ShimmerHill"] = Has(418) || Has(419) || Has(420) || Has(421); // Deoxys
        ow["LostWilderness"] = Has(324); // Makuhita
        ow["MidnightForest"] = Has(521); // Rotom
        //ow["ZeroIsleNorth"] = false; // TODO: complete all zero
        //ow["ZeroIsleEast"] = false; // TODO: complete all zero
        //ow["ZeroIsleWest"] = false; // TODO: complete all zero
        //ow["ZeroIsleSouth"] = false; // TODO: complete all zero
        //ow["FinalMaze"] = false; // TODO complete all maze
        //ow["ZeroIsleCenter"] = false; // TODO: complete all zero
        ow["DestinyTower"] = Has2(43); // Space Globe

        var ExplorerRankPoints = this.sav.Bits.GetInt(0, this.sav.Offsets.ExplorerRank, 32);
        if (ExplorerRankPoints < 17000)
            ow["OblivionForest"] = false; //untrackable

        ow["TreacherousWaters"] = Has(272); // Suicune
        ow["SoutheasternIslands"] = Has(270); // Raikou
        ow["InfernoCave"] = Has(271); // 271
        ow["StarCave"] = Has(417); // Jirachi
        ow["SkyPeakMountainPath"] = DidStory;
        //ow["MurkyForest"] = false; // TODO special episode 2 iggly
        //ow["EasternCave"] = false; // TODO special episode 2 iggly
        //ow["FortuneRavine"] = false; // TODO special episode 2 iggly
        //ow["SpringCave"] = false; // TODO third Special Episode, Today's "Oh My Gosh".
        //ow["SouthernJungle"] = false; // TODO  Special Episode Here Comes Team Charm!.
        //ow["BoulderQuarry"] = false; // TODO  Special Episode Here Comes Team Charm!
        //ow["LeftCavePath"] = false; // TODO Special Episode, Here Comes Team Charm!. 
        //ow["RightCavePath"] = false; // TODO Special Episode, Here Comes Team Charm!. 
        //ow["LimestoneCavern"] = false; // TODO Special Episode, Here Comes Team Charm!. 
        //ow["BarrenValley"] = false; // TODO  In the Future of Darkness i
        //ow["DarkWasteland"] = false; // TODO  In the Future of Darkness i
        //ow["SpacialCliffs"] = false; // TODO In the Future of Darkness i
        //ow["DarkIceMountain"] = false; // TODO In the Future of Darkness i
        //ow["IcicleForest"] = false; // TODO In the Future of Darkness i
        //ow["VastIceMountain"] = false; // TODO In the Future of Darkness i
    }

    public bool GotShaymin()
    {
        return this.ownedMonList.Contains(534) || this.ownedMonList.Contains(535);
    }
    public bool HasFinishedPostCreditStory()
    {
        return this.ownedMonList.Contains(532);
    }

    public void Generate_adventureLog()
    {
        var ow = new Dictionary<string, bool>();
        owned["adventureLog"] = ow;

        var didSomething = this.ownedMonList.Count >= 3;
        ow["PokemonthatjoinedyouX"] = didSomething;
        ow["KindsofPokemonbattledX"] = didSomething;
        ow["MoveslearnedX"] = didSomething;
        ow["KindsofitemsacquiredX"] = didSomething;
        ow["DungeonsclearedX"] = didSomething;
        //ow["FaintedindungeonsX"] = true;  //TODO
        //ow["SuccessfulFriendRescuesX"] = true; //TODO
        //ow["PokemonevolvedX"] = true; //TODO
        //ow["PokemonEggshatchedX"] = true; //TODO
        ow["RecordofonefloorvictoriesX"] = didSomething;
        //ow["WonbigatBigTreasureX"] = true; //TODO
        ow["RecycledX"] = this.ownedMonList.Contains(485) // Gible for Landslide Cave
                            || this.ownedMonList.Contains(449) // Burmy - Trash for Oran Forest;
                            || this.ownedMonList.Contains(7) // Squirtle for Lake Afar
                            || this.ownedItemList.Contains(169) // Prize Ticket
                            || this.ownedItemList.Contains(170)  // Silver Ticket
                            || this.ownedItemList.Contains(171)  // Gold Ticket 
                            || this.ownedItemList.Contains(172); // Prism Ticket
        //ow["SkyGiftssentX"] = true; //TODO

        var DidStory = HasFinishedPostCreditStory();
        ow["DiscoveredtheHotSpring"] = DidStory;
        ow["DiscoveredtheFogboundLake"] = DidStory;
        ow["Returnedfromthefuture"] = DidStory;
        ow["Preventedtheplanetsparalysis"] = DidStory;
        ow["GraduatedfromWigglytuffsGuild"] = DidStory;
        ow["ClimbedtotheSkyPeakSummit"] = DidStory;
        ow["DiscoveredthesecretofAegisCave"] = DidStory;
        ow["FoiledDarkraisdastardlyplot"] = DidStory;
        // ow["BidoofaskedJirachiforawish"] = true; //TODO
        // ow["WigglytuffreminiscedabouthisMaster"] = true; //TODO
        // ow["SunfloracaughttheHauntergroup"] = true; //TODO
        // ow["TeamCharmfoundaTimeGear"] = true; //TODO
        // ow["ThePokemoninthefuturecamethrough"] = true; //TODO
        ow["ReceivedtheIcyFlute"] = this.ownedItemList.Contains(59);
        ow["ReceivedtheFieryDrum"] = this.ownedItemList.Contains(60);
        ow["ReceivedtheTerraCymbal"] = this.ownedItemList.Contains(61);
        ow["ReceivedtheAquaMonica"] = this.ownedItemList.Contains(62);
        ow["ReceivedtheRockHorn"] = this.ownedItemList.Contains(63);
        ow["ReceivedtheGrassCornet"] = this.ownedItemList.Contains(64);
        ow["ReceivedtheSkyMelodica"] = this.ownedItemList.Contains(65);
        // ow["Completedalltrainingprograms"] = true; //TODO
        ow["AcceptedallSpecialChallenges"] = this.ownedMonList.Contains(417) //Jirachi
                                              && this.ownedMonList.Contains(271) //Entei
                                              && this.ownedMonList.Contains(270) //Raikou
                                              && this.ownedMonList.Contains(272) //Suicune
                                              && this.ownedMonList.Contains(150); //Mewtwo
        // ow["ClearedallofZeroIsle"] = true; //TODO
        ow["Articunojoinedtheteam"] = this.ownedMonList.Contains(144);
        ow["Zapdosjoinedtheteam"] = this.ownedMonList.Contains(145);
        ow["Moltresjoinedtheteam"] = this.ownedMonList.Contains(146);
        ow["Mewtwojoinedtheteam"] = this.ownedMonList.Contains(150);
        ow["Mewjoinedtheteam"] = this.ownedMonList.Contains(151);

        var HasUnown = () =>
        {
            var list = new List<int> { 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228 };
            foreach (var i in list)
                if (this.ownedMonList.Contains(i))
                    return true;
            return false;
        };

        ow["Unownjoinedtheteam"] = HasUnown();
        ow["Raikoujoinedtheteam"] = this.ownedMonList.Contains(270);
        ow["Enteijoinedtheteam"] = this.ownedMonList.Contains(271);
        ow["Suicunejoinedtheteam"] = this.ownedMonList.Contains(272);
        ow["Lugiajoinedtheteam"] = this.ownedMonList.Contains(276);
        ow["HoOhjoinedtheteam"] = this.ownedMonList.Contains(277);
        ow["Celebijoinedtheteam"] = this.ownedMonList.Contains(278);
        ow["Regirockjoinedtheteam"] = this.ownedMonList.Contains(409);
        ow["Regicejoinedtheteam"] = this.ownedMonList.Contains(410);
        ow["Registeeljoinedtheteam"] = this.ownedMonList.Contains(411);
        ow["Latiasjoinedtheteam"] = this.ownedMonList.Contains(412);
        ow["Latiosjoinedtheteam"] = this.ownedMonList.Contains(413);
        ow["Kyogrejoinedtheteam"] = this.ownedMonList.Contains(414);
        ow["Groudonjoinedtheteam"] = this.ownedMonList.Contains(415);
        ow["Rayquazajoinedtheteam"] = this.ownedMonList.Contains(416);
        ow["Jirachijoinedtheteam"] = this.ownedMonList.Contains(417);
        ow["Deoxysjoinedtheteam"] = this.ownedMonList.Contains(418) || this.ownedMonList.Contains(419) || this.ownedMonList.Contains(420) || this.ownedMonList.Contains(421);
        ow["Rotomjoinedtheteam"] = this.ownedMonList.Contains(521);
        ow["Uxiejoinedtheteam"] = this.ownedMonList.Contains(522);
        ow["Mespritjoinedtheteam"] = this.ownedMonList.Contains(523);
        ow["Azelfjoinedtheteam"] = this.ownedMonList.Contains(524);
        ow["Dialgajoinedtheteam"] = this.ownedMonList.Contains(525);
        ow["Palkiajoinedtheteam"] = this.ownedMonList.Contains(526);
        ow["Heatranjoinedtheteam"] = this.ownedMonList.Contains(527);
        ow["Regigigasjoinedtheteam"] = this.ownedMonList.Contains(528);
        ow["Giratinajoinedtheteam"] = this.ownedMonList.Contains(529) || this.ownedMonList.Contains(536);
        ow["Cresseliajoinedtheteam"] = this.ownedMonList.Contains(530);
        ow["Phionejoinedtheteam"] = this.ownedMonList.Contains(531);
        ow["Manaphyjoinedtheteam"] = this.ownedMonList.Contains(532);
        ow["Darkraijoinedtheteam"] = this.ownedMonList.Contains(533);
        ow["Shayminjoinedtheteam"] = this.ownedMonList.Contains(534) || this.ownedMonList.Contains(535);
    }

    public void Generate_specialEpisode()
    {
        var ow = new Dictionary<string, bool>();
        owned["specialEpisode"] = ow;
        // ow["BidoofsWish"] = false; // TODO
        // ow["IgglybufftheProdigy"] = false; // TODO
        // ow["TodaysOhMyGosh"] = false; // TODO
        // ow["HereComesTeamCharm"] = false; // TODO
        // ow["IntheFutureofDarkness"] = false; // TODO
    }

    public void Generate_progressIcon()
    {
        var ow = new Dictionary<string, bool>();
        owned["progressIcon"] = ow;

        var DidStory = HasFinishedPostCreditStory();
        ow["Drowzee"] = DidStory;
        ow["Groudon"] = DidStory;
        ow["Manectric"] = DidStory;
        ow["Celebi"] = DidStory;
        ow["Dusknoir"] = DidStory;
        ow["Palkia"] = DidStory;
        ow["Wigglytuff"] = DidStory;
        ow["Shaymin"] = DidStory;
        ow["Scizor"] = DidStory;
        ow["Phione"] = DidStory;
        ow["Regigigas"] = DidStory;
        ow["Darkrai"] = DidStory;

        var GoldenChest = () =>
        {
            var list = new List<int> { 59, 60, 61, 62, 63, 64, 65 };
            foreach (var item in list)
                if (!this.ownedItemList.Contains(item))
                    return false;
            return true;
        };
        ow["GoldenChest"] = GoldenChest();
    }

    public void Generate_rank()
    {
        var ow = new Dictionary<string, bool>();
        owned["rank"] = ow;

        var ExplorerRankPoints = this.sav.Bits.GetInt(0, this.sav.Offsets.ExplorerRank, 32);

        ow["NormalRank"] = true;
        ow["BronzeRank"] = ExplorerRankPoints >= 100;
        ow["SilverRank"] = ExplorerRankPoints >= 400;
        ow["GoldRank"] = ExplorerRankPoints >= 1600;
        ow["DiamondRank"] = ExplorerRankPoints >= 3200;
        ow["SuperRank"] = ExplorerRankPoints >= 5000;
        ow["UltraRank"] = ExplorerRankPoints >= 7500;
        ow["HyperRank"] = ExplorerRankPoints >= 10500;
        ow["MasterRank"] = ExplorerRankPoints >= 13500;
        ow["MasterRank1Star"] = ExplorerRankPoints >= 17000;
        ow["MasterRank2Stars"] = ExplorerRankPoints >= 21000;
        ow["MasterRank3Stars"] = ExplorerRankPoints >= 25000;
        ow["GuildmasterRank"] = ExplorerRankPoints >= 100000;

        var HasTreasure = () =>
        {
            var list = new List<int> { 59, 60, 61, 62, 63, 64, 65 };
            foreach (var item in list)
                if (this.ownedItemList.Contains(item))
                    return true;
            return false;
        };
        ow["SecretRank"] = HasTreasure();
    }
}

enum MarowakDojo
{
}