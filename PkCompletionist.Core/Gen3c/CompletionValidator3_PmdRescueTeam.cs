using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers.Binary;
using System.Reflection.Emit;
using System.IO;

namespace PkCompletionist.Core;


internal class CompletionValidator3_PmdRescueTeam : CompletionValidatorX
{
    public CompletionValidator3_PmdRescueTeam(Command command, SAV3_PmdRescueTeam sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;
        this.bitBlocks = this.sav.bitBlock_SlowCopy;
        this.ownedMonList = this.GetObtainedPokemons();
    }
    new SAV3_PmdRescueTeam sav;

    BitBlock bitBlocks;
    private List<int> ownedMonList = new();

    public override void GenerateAll()
    {
        Generate_pokemon();
        Generate_item();
        Generate_friendArea();
        Generate_makuhitaDojo();
        Generate_dungeon();
        Generate_adventureLog();
        Generate_teamBaseFigure();
        Generate_rank();
    }
    public List<int> GetObtainedPokemons()
    {
        var ownedList = new List<int>();
        for (int i = 0; i < sav.off.StoredPokemonCount; i++)
        {
            var bits = this.bitBlocks.GetRange(sav.off.StoredPokemonOffset + i * sav.off.StoredPokemonLength, sav.off.StoredPokemonLength);
            int pokeId = bits.GetInt(0, 7, 9);
            if (pokeId == 0)
                continue;
               
            // Castform
            if (pokeId == 376 || pokeId == 377 || pokeId == 378 || pokeId == 379)
                ownedList.Add(376);
            else if (pokeId == 414 || pokeId == 417 || pokeId == 418 || pokeId == 419)
                ownedList.Add(414);
            else 
                ownedList.Add(pokeId);
        }
        return ownedList;
    }
    public override void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        var unobtainabled = new List<int> { 377, 378, 379 };
        for (ushort i = 1; i <= 416; i++)
        {
            if (unobtainabled.Contains(i))
                continue;

            ow[i.ToString()] = this.ownedMonList.Contains(i);
        }
    }


    public List<int> GetObtainedItems()
    {
        var ownedList = new List<int>();

        // Stored items
        var block = this.bitBlocks.GetRange(this.sav.off.StoredItemOffset, this.sav.off.StoredItemCount * 10);
        for (int i = 0; i < this.sav.off.StoredItemCount; i++)
        {
            var quantity = block.GetNextInt(10);
            if (quantity > 0)
                ownedList.Add(i + 1 /*itemId*/);
        }

        // Inventory items
        for (int i = 0; i < 50; i++)
        {
            var bits = this.bitBlocks.GetRange(this.sav.off.HeldItemOffset + (i * 33), 33);
            if (bits.Bits[0])
                ownedList.Add(bits.GetInt(0, 15, 8));
        }

        // TODO: Held item of Pokemon

        return ownedList;
    }
    public override void Generate_item()
    {
        var ow = new Dictionary<string, bool>();
        owned["item"] = ow;

        var ownedList = GetObtainedItems();
        for (ushort i = 1; i <= 239; i++)
            ow[i.ToString()] = ownedList.Contains(i);
    }

    public bool HasFriendArea(FriendArea fa)
    {
        var byteIdx = (int)fa / 8;
        var bitIdx = (int)fa % 8;
        return (sav.Data[sav.off.FriendAreaUnlockedOffset + byteIdx] & (1 << bitIdx)) != 0;
    }

    public void Generate_friendArea()
    {
        var ow = new Dictionary<string, bool>();
        owned["friendArea"] = ow;

        ow["AgedChamberAN"] = HasFriendArea(FriendArea.AGED_CHAMBER_AN);
        ow["AgedChamberO"] = HasFriendArea(FriendArea.AGED_CHAMBER_O);
        ow["AncientRelic"] = HasFriendArea(FriendArea.ANCIENT_RELIC);
        ow["BeauPlains"] = HasFriendArea(FriendArea.BEAU_PLAINS);
        ow["BoulderCave"] = HasFriendArea(FriendArea.BOULDER_CAVE);
        ow["BountifulSea"] = HasFriendArea(FriendArea.BOUNTIFUL_SEA);
        ow["Crater"] = HasFriendArea(FriendArea.CRATER);
        ow["CrypticCave"] = HasFriendArea(FriendArea.CRYPTIC_CAVE);
        ow["DarknessRidge"] = HasFriendArea(FriendArea.DARKNESS_RIDGE);
        ow["DecrepitLab"] = HasFriendArea(FriendArea.DECREPIT_LAB);
        ow["DeepSeaCurrent"] = HasFriendArea(FriendArea.DEEP_SEA_CURRENT);
        ow["DeepSeaFloor"] = HasFriendArea(FriendArea.DEEP_SEA_FLOOR);
        ow["DragonCave"] = HasFriendArea(FriendArea.DRAGON_CAVE);
        ow["EchoCave"] = HasFriendArea(FriendArea.ECHO_CAVE);
        ow["EnclosedIsland"] = HasFriendArea(FriendArea.ENCLOSED_ISLAND);
        ow["EnergeticForest"] = HasFriendArea(FriendArea.ENERGETIC_FOREST);
        ow["FinalIsland"] = HasFriendArea(FriendArea.FINAL_ISLAND);
        ow["FlyawayForest"] = HasFriendArea(FriendArea.FLYAWAY_FOREST);
        ow["FrigidCavern"] = HasFriendArea(FriendArea.FRIGID_CAVERN);
        ow["FurnaceDesert"] = HasFriendArea(FriendArea.FURNACE_DESERT);
        ow["HealingForest"] = HasFriendArea(FriendArea.HEALING_FOREST);
        ow["IceFloeBeach"] = HasFriendArea(FriendArea.ICE_FLOE_BEACH);
        ow["Jungle"] = HasFriendArea(FriendArea.JUNGLE);
        ow["LegendaryIsland"] = HasFriendArea(FriendArea.LEGENDARY_ISLAND);
        ow["MagneticQuarry"] = HasFriendArea(FriendArea.MAGNETIC_QUARRY);
        ow["MistRiseForest"] = HasFriendArea(FriendArea.MIST_RISE_FOREST);
        ow["MtCleft"] = HasFriendArea(FriendArea.MT_CLEFT);
        ow["MtDeepgreen"] = HasFriendArea(FriendArea.MT_DEEPGREEN);
        ow["MtDiscipline"] = HasFriendArea(FriendArea.MT_DISCIPLINE);
        ow["MtMoonview"] = HasFriendArea(FriendArea.MT_MOONVIEW);
        ow["MushroomForest"] = HasFriendArea(FriendArea.MUSHROOM_FOREST);
        ow["MysticLake"] = HasFriendArea(FriendArea.MYSTIC_LAKE);
        ow["OvergrownForest"] = HasFriendArea(FriendArea.OVERGROWN_FOREST);
        ow["PeanutSwamp"] = HasFriendArea(FriendArea.PEANUT_SWAMP);
        ow["PoisonSwamp"] = HasFriendArea(FriendArea.POISON_SWAMP);
        ow["PowerPlant"] = HasFriendArea(FriendArea.POWER_PLANT);
        ow["RainbowPeak"] = HasFriendArea(FriendArea.RAINBOW_PEAK);
        ow["RavagedField"] = HasFriendArea(FriendArea.RAVAGED_FIELD);
        ow["RubaDubRiver"] = HasFriendArea(FriendArea.RUB_A_DUB_RIVER);
        ow["SacredField"] = HasFriendArea(FriendArea.SACRED_FIELD);
        ow["Safari"] = HasFriendArea(FriendArea.SAFARI);
        ow["ScorchedPlains"] = HasFriendArea(FriendArea.SCORCHED_PLAINS);
        ow["SeafloorCave"] = HasFriendArea(FriendArea.SEAFLOOR_CAVE);
        ow["SecretiveForest"] = HasFriendArea(FriendArea.SECRETIVE_FOREST);
        ow["SereneSea"] = HasFriendArea(FriendArea.SERENE_SEA);
        ow["ShallowBeach"] = HasFriendArea(FriendArea.SHALLOW_BEACH);
        ow["SkyBluePlains"] = HasFriendArea(FriendArea.SKY_BLUE_PLAINS);
        ow["SouthernIsland"] = HasFriendArea(FriendArea.SOUTHERN_ISLAND);
        ow["StratosLookout"] = HasFriendArea(FriendArea.STRATOS_LOOKOUT);
        ow["TadpolePond"] = HasFriendArea(FriendArea.TADPOLE_POND);
        ow["ThunderMeadow"] = HasFriendArea(FriendArea.THUNDER_MEADOW);
        ow["TransformForest"] = HasFriendArea(FriendArea.TRANSFORM_FOREST);
        ow["TreasureSea"] = HasFriendArea(FriendArea.TREASURE_SEA);
        ow["TurtleshellPond"] = HasFriendArea(FriendArea.TURTLESHELL_POND);
        ow["VolcanicPit"] = HasFriendArea(FriendArea.VOLCANIC_PIT);
        ow["WaterfallLake"] = HasFriendArea(FriendArea.WATERFALL_LAKE);
        ow["WildPlains"] = HasFriendArea(FriendArea.WILD_PLAINS);
    }

    public bool HasCompletedMakuhitaDojo(MakuhitaDojo dojo)
    {
        var byteIdx = (int)dojo / 8;
        var bitIdx = (int)dojo % 8;
        return (sav.Data[sav.off.MakuhitaDojoCompletedOffset + byteIdx] & (1 << bitIdx)) != 0;
    }
    
    public void Generate_makuhitaDojo()
    {
        var ow = new Dictionary<string, bool>();
        owned["makuhitaDojo"] = ow;

        ow["NormalMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.NormalMaze);
        ow["FireMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.FireMaze);
        ow["WaterMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.WaterMaze);
        ow["GrassMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.GrassMaze);
        ow["ElectricMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.ElectricMaze);
        ow["IceMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.IceMaze);
        ow["FightingMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.FightingMaze);
        ow["GroundMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.GroundMaze);
        ow["FlyingMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.FlyingMaze);
        ow["PsychicMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.PsychicMaze);
        ow["PoisonMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.PoisonMaze);
        ow["BugMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.BugMaze);
        ow["RockMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.RockMaze);
        ow["GhostMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.GhostMaze);
        ow["DragonMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.DragonMaze);
        ow["DarkMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.DarkMaze);
        ow["SteelMaze"] = HasCompletedMakuhitaDojo(MakuhitaDojo.SteelMaze);
        ow["TeamShifty"] = HasCompletedMakuhitaDojo(MakuhitaDojo.TeamShifty);
        ow["TeamConstrictor"] = HasCompletedMakuhitaDojo(MakuhitaDojo.TeamConstrictor);
        ow["TeamHydro"] = HasCompletedMakuhitaDojo(MakuhitaDojo.TeamHydro);
        ow["TeamRumblerock"] = HasCompletedMakuhitaDojo(MakuhitaDojo.TeamRumblerock);

    }

    public void Generate_dungeon()
    {
        var ow = new Dictionary<string, bool>();
        owned["dungeon"] = ow;

        ow["TinyWoods"] = true;
        ow["ThunderwaveCave"] = HasCompletedSkyTower();
        ow["MtSteel"] = HasCompletedSkyTower();
        ow["SinisterWoods"] = HasCompletedSkyTower();
        ow["SilentChasm"] = HasCompletedSkyTower();
        ow["MtThunder"] = HasCompletedSkyTower();
        ow["GreatCanyon"] = HasCompletedSkyTower();
        ow["LapisCave"] = HasCompletedSkyTower();
        ow["RockPath"] = HasCompletedSkyTower();
        ow["MtBlaze"] = HasCompletedSkyTower();
        ow["SnowPath"] = HasCompletedSkyTower();
        ow["FrostyForest"] = HasCompletedSkyTower();
        ow["MtFreeze"] = HasCompletedSkyTower();
        ow["UproarForest"] = HasCompletedSkyTower();
        ow["MagmaCavern"] = HasCompletedSkyTower();
        ow["SkyTower"] = HasCompletedSkyTower();

        // TODO: use real flag instead of checking caught Pokemon
        ow["HowlingForest"] = ownedMonList.Contains(260); // Smeargle
        ow["StormySea"] = ownedMonList.Contains(410); // Kyogre
        ow["SilverTrench"] = ownedMonList.Contains(274);  // Lugia
        ow["MeteorCave"] = ownedMonList.Contains(414);  // Deoxys
        ow["FieryField"] = ownedMonList.Contains(269);  // Entei
        ow["LightningField"] = ownedMonList.Contains(268);  // Raikou
        ow["NorthwindField"] = ownedMonList.Contains(270);  // Suicune

        ow["MtFaraway"] = ownedMonList.Contains(275);  // Ho-Oh
        ow["WesternCave"] = ownedMonList.Contains(150);  // Mewtwo
        ow["NorthernRange"] = ownedMonList.Contains(409);  // Latios
        ow["PitfallValley"] = ownedMonList.Contains(409);  // Latios

        ow["BuriedRelic"] = ownedMonList.Contains(151) || ownedMonList.Contains(405) || ownedMonList.Contains(406) || ownedMonList.Contains(406);  // Mew, Regi*
        ow["WishCave"] = ownedMonList.Contains(413);  // Jirachi

        ow["MurkyCave"] = HasCompletedStory(4);

        ow["DesertRegion"] = HasFriendArea(FriendArea.FURNACE_DESERT);
        ow["SouthernCavern"] = HasFriendArea(FriendArea.BOULDER_CAVE);
        ow["WyvernHill"] = HasFriendArea(FriendArea.DRAGON_CAVE);

        var ownedItemList = GetObtainedItems();
        ow["SolarCave"] = ownedMonList.Contains(228) || ownedItemList.Contains(230) || ownedItemList.Contains(226);  // Girafarig, Waterfall, Surf

        ow["DarknightRelic"] = ownedMonList.Contains(327);  // Sableye
        ow["GrandSea"] = ownedMonList.Contains(251);  // Mantine

        ow["WaterfallPond"] = ownedMonList.Contains(118);  // Goldeen

        var UnownRelic = () =>
        {
            for (var i = 201; i <= 226; i++) // Unown
                if (ownedMonList.Contains(i))
                    return true;
            return false;
        };
        ow["UnownRelic"] = UnownRelic();

        ow["JoyousTower"] = ownedMonList.Contains(77);  // Ponyta
        ow["FarOffSea"] = ownedItemList.Contains(209); // Vacuum-Cut
        ow["PurityForest"] = ownedMonList.Contains(276);  // Celebi

        //untrackable
        //ow["OddityCave"] 
        //ow["RemainsIsland"]
        //ow["MarvelousSea"]
        //ow["FantasyStrait"]
    }
    public bool HasCompletedSkyTower()
    {
        return HasCompletedStory(3);
    }
    public bool HasCompletedStory(int idx)
    {
        var byteIdx = idx / 8;
        var bitIdx = idx % 8;
        return (sav.Data[0x4EEC + byteIdx] & (1 << bitIdx)) != 0;
    }
    public void Generate_adventureLog()
    {
        var ow = new Dictionary<string, bool>();
        owned["adventureLog"] = ow;

        ow["ReachedtheHilloftheAncients"] = HasCompletedStory(1);
        ow["Tooktotheroadasfugitives"] = HasCompletedStory(2);
        ow["Preventedthemeteorscollision"] = HasCompletedStory(3);
        ow["BrokethecurseonGardevoir"] = HasCompletedStory(4);
        ow["Therescueteambasewascompleted"] = HasCompletedStory(5);
        ow["RescuedSmeargle"] = HasCompletedStory(6);
        ow["SpottedMunchlax"] = HasCompletedStory(7);
        ow["Xmoveswerelearned"] = HasCompletedStory(8);
        ow["Xfriendrescuesweresuccessful"] = HasCompletedStory(9);
        ow["XPokemonevolved"] = HasCompletedStory(10);
        ow["XPokemonjoinedtheteam"] = HasCompletedStory(11);
        ow["Xthievingattemptssucceeded"] = HasCompletedStory(12);
        ow["Xfloorswereexplored"] = HasCompletedStory(13);
        ow["AllFriendAreaswereobtained"] = HasCompletedStory(14);
        ow["AllFriendAreaswereobtained"] = HasCompletedStory(15);
        ow["AllPokemonweremadeleaders"] = HasCompletedStory(16);
        ow["AllPokemonjoinedtheteam"] = HasCompletedStory(17);

        ow["Moltresjoinedtheteam"] = this.ownedMonList.Contains(146);
        ow["Zapdosjoinedtheteam"] = this.ownedMonList.Contains(145);
        ow["Articunojoinedtheteam"] = this.ownedMonList.Contains(144);
        ow["Deoxysjoinedtheteam"] = this.ownedMonList.Contains(414);
        ow["Enteijoinedtheteam"] = this.ownedMonList.Contains(269);
        ow["Raikoujoinedtheteam"] = this.ownedMonList.Contains(268);
        ow["Suicunejoinedtheteam"] = this.ownedMonList.Contains(270);
        ow["HoOhjoinedtheteam"] = this.ownedMonList.Contains(275);
        ow["Kyogrejoinedtheteam"] = this.ownedMonList.Contains(410);
        ow["Groudonjoinedtheteam"] = this.ownedMonList.Contains(411);
        ow["Rayquazajoinedtheteam"] = this.ownedMonList.Contains(412);
        ow["Lugiajoinedtheteam"] = this.ownedMonList.Contains(274);
        ow["Celebijoinedtheteam"] = this.ownedMonList.Contains(276);
        ow["Mewjoinedtheteam"] = this.ownedMonList.Contains(151);
        ow["Mewtwojoinedtheteam"] = this.ownedMonList.Contains(150);
        ow["Jirachijoinedtheteam"] = this.ownedMonList.Contains(413);
    }
    public void Generate_progressIcon()
    {
        var ow = new Dictionary<string, bool>();
        owned["progressIcon"] = ow;

        ow["Zapdos"] = HasCompletedSkyTower();
        ow["Moltres"] = HasCompletedSkyTower();
        ow["Articuno"] = HasCompletedSkyTower();
        ow["Groudon"] = HasCompletedSkyTower();
        ow["Rayquaza"] = HasCompletedSkyTower();

        //TODO: check real flag instead of caught Pokemon
        ow["Kyogre"] = this.ownedMonList.Contains(410);
        ow["Mew"] = this.ownedMonList.Contains(151);
        ow["HoOh"] = this.ownedMonList.Contains(275);
        ow["Celebi"] = this.ownedMonList.Contains(276);
        ow["Lugia"] = this.ownedMonList.Contains(274);
        ow["Mewtwo"] = this.ownedMonList.Contains(150);
        ow["Deoxys"] = this.ownedMonList.Contains(414);
    }

    public void Generate_teamBaseFigure()
    {
        var ow = new Dictionary<string, bool>();
        owned["teamBaseFigure"] = ow;

        ow["BonslyFigure"] = HasCompletedMakuhitaDojo(MakuhitaDojo.NormalMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.FireMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.WaterMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.GrassMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.ElectricMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.IceMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.FightingMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.GroundMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.FlyingMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.PsychicMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.PoisonMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.BugMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.RockMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.GhostMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.DragonMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.DarkMaze) &&
                              HasCompletedMakuhitaDojo(MakuhitaDojo.SteelMaze);

        ow["MimeJrFigure"] = (sav.Data[0xDB] & 0b01) != 0;
        ow["WeavileFigure"] = (sav.Data[0xDB] & 0b10) != 0;

        var pts = this.bitBlocks.GetInt(0, sav.off.RescuePointsOffset, sav.off.RescuePointsLength);
        ow["LucarioFigure"] = pts >= 15000;
    }


    public void Generate_rank()
    {
        var ow = new Dictionary<string, bool>();
        owned["rank"] = ow;

        var pts = this.bitBlocks.GetInt(0, sav.off.RescuePointsOffset, sav.off.RescuePointsLength);
        ow["NormalRank"] = true;
        ow["BronzeRank"] = pts >= 50;
        ow["SilverRank"] = pts >= 500;
        ow["GoldRank"] = pts >= 1500;
        ow["PlatinumRank"] = pts >= 3000;
        ow["DiamondRank"] = pts >= 7500;
        ow["LucarioRank"] = pts >= 15000;
    }
}



enum FriendArea
{
    //0x4ED8
    _UNUSED = 0,
    BOUNTIFUL_SEA,
    TREASURE_SEA,
    SERENE_SEA,
    DEEP_SEA_FLOOR,
    DEEP_SEA_CURRENT,
    SEAFLOOR_CAVE,
    SHALLOW_BEACH,

    //0x4ED9
    MT_DEEPGREEN,
    MT_CLEFT,
    MT_MOONVIEW,
    RAINBOW_PEAK,
    WILD_PLAINS,
    BEAU_PLAINS,
    SKY_BLUE_PLAINS,
    SAFARI,

    //0x4EDA
    SCORCHED_PLAINS,
    SACRED_FIELD,
    MIST_RISE_FOREST,
    FLYAWAY_FOREST,
    OVERGROWN_FOREST,
    ENERGETIC_FOREST,
    MUSHROOM_FOREST,
    HEALING_FOREST,

    //0x4EDB
    TRANSFORM_FOREST,
    SECRETIVE_FOREST,
    RUB_A_DUB_RIVER,
    TADPOLE_POND,
    TURTLESHELL_POND,
    MYSTIC_LAKE,
    WATERFALL_LAKE,
    PEANUT_SWAMP,

    //0x4EDC
    POISON_SWAMP,
    ECHO_CAVE,
    CRYPTIC_CAVE,
    DRAGON_CAVE,
    BOULDER_CAVE,
    JUNGLE,
    DECREPIT_LAB,
    MT_DISCIPLINE,

    //0x4EDD
    THUNDER_MEADOW,
    POWER_PLANT,
    CRATER,
    FURNACE_DESERT,
    AGED_CHAMBER_AN,
    AGED_CHAMBER_O,
    ANCIENT_RELIC,
    DARKNESS_RIDGE,

    //0x4EDE
    FRIGID_CAVERN,
    ICE_FLOE_BEACH,
    VOLCANIC_PIT,
    STRATOS_LOOKOUT,
    RAVAGED_FIELD,
    MAGNETIC_QUARRY,
    LEGENDARY_ISLAND,
    SOUTHERN_ISLAND,

    //0x4EDF
    ENCLOSED_ISLAND,
    FINAL_ISLAND,
    MAX = FINAL_ISLAND,
}

enum MakuhitaDojo
{
    NormalMaze,
    FireMaze,
    WaterMaze,
    GrassMaze,
    ElectricMaze,
    IceMaze,
    FightingMaze,
    GroundMaze,
    FlyingMaze,
    PsychicMaze,
    PoisonMaze,
    BugMaze,
    RockMaze,
    GhostMaze,
    DragonMaze,
    DarkMaze,
    SteelMaze,
    TeamShifty,
    TeamConstrictor,
    TeamHydro,
    TeamRumblerock,
}