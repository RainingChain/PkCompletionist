using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers.Binary;
using System.Reflection.Emit;

namespace PkCompletionist.Core;

internal class CompletionValidator3 : CompletionValidatorX
{
    public CompletionValidator3(Command command, SAV3E sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;

        // Note that in US game, there are more unobtainable items than that
        this.unobtainableItems = new List<int>() {
            44, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 72, 82, 87, 88, 89, 90, 91, 92, 99, 100, 101, 102, 105, 112, 113, 114, 115, 116, 117, 118, 119, 120, 176, 177, 178, 226, 227, 228, 
            229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 266, 267, 276, 277, 347, 348, 349, 350, 351, 352, 353, 
            354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 373, 374 };

    }

    new SAV3E sav;

    //Removed starters, latios, latias, gorebyss, huntail + jirachi, deoxys, golem, alakazam
    List<ushort> HoennListSeeNoTrade = new List<ushort> { 25,26,27,28,37,38,39,40,41,42,43,44,45,54,55,63,64,66,67,68,72,73,74,75,81,82,84,85,88,89,100,101,109,110,111,112,116,117,118,119,
        120,121,127,129,130,169,170,171,172,174,177,178,182,183,184,202,203,214,218,219,222,227,230,231,232,261,262,263,264,265,266,267,268,269,270,271,272,273,274,275,276,277,278,279,280,
        281,282,283,284,285,286,287,288,289,290,291,292,293,294,295,296,297,298,299,300,301,302,303,304,305,306,307,308,309,310,311,312,313,314,315,316,317,318,319,320,321,322,323,324,325,
        326,327,328,329,330,331,332,333,334,335,336,337,338,339,340,341,342,343,344,345,346,347,348,349,350,351,352,353,354,355,356,357,358,359,360,361,362,363,364,365,366,369,370,371,372,
        373,374,375,376,377,378,379,382,383,384};

    // starters, latios, latias, gorebyss, huntail
    List<ushort> HoennListSeeTrade = new List<ushort> { 252, 253, 254, 255, 256, 257, 258, 259, 260, 367, 368, 380, 381 };

    List<ushort> HoennList = new List<ushort> { 25, 26, 27, 28, 37, 38, 39, 40, 41, 42, 43, 44, 45, 54, 55, 63, 64, 65, 66, 67, 68, 72, 73, 74, 75, 76, 81, 82, 84, 85, 88, 89, 100, 101, 
        109, 110, 111, 112, 116, 117, 118, 119, 120, 121, 127, 129, 130, 169, 170, 171, 172, 174, 177, 178, 182, 183, 184, 202, 203, 214, 218, 219, 222, 227, 230, 231, 232, 252, 253, 254, 
        255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 
        291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 318, 319, 320, 321, 322, 323, 324, 325, 326, 
        327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 
        363, 364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374, 375, 376, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386 };

    List<ushort> StarterList = new List<ushort> { 252, 253, 254, 255, 256, 257, 258, 259, 260 };

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
        Generate_trainerBattle();
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
    }

    public override void Generate_item()
    {
        base.Generate_item();

        var ow = this.owned["item"];
        ow["375"] = HasItem(375); // Fix bug with PkHeX. https://github.com/kwsch/PKHeX/issues/4033

        ow["281"] = sav.GetEventFlag(0x0213); // Rm. 1 Key
        ow["282"] = sav.GetEventFlag(0x0214); // Rm. 2 Key
        ow["283"] = sav.GetEventFlag(0x0215); // Rm. 4 Key
        ow["284"] = sav.GetEventFlag(0x0215); // Rm. 6 Key

        ow["274"] = sav.GetEventFlag(0x00BD); // Letter -  FLAG_DELIVERED_STEVEN_LETTER

        if (HasPkmWithTID(25) || HasPkmWithTID(26)) // Pikachu or Raichu
            ow["5"] = true; // Safari Ball

        ow["269"] = sav.GetEventFlag(0x0095); // Devon Goods - FLAG_DELIVERED_DEVON_GOODS

        if (sav.GetEventFlag(0x045F)) // FLAG_ITEM_SAFARI_ZONE_NORTH_CALCIUM
            ow["272"] = true; // Acro Bike

        if (sav.GetEventFlag(0x0446)) // FLAG_ITEM_SAFARI_ZONE_NORTH_WEST_TM_22
            ow["259"] = true; // Mach Bike

        if (this.living)
            return;

        if (HasPkmWithTID(241)) // Miltank
            ow["29"] = true; // Moomoo Milk

        if (sav.GetEventFlag(0x03BC) || sav.GetEventFlag(0x03BD) || sav.GetEventFlag(0x03BE) || sav.GetEventFlag(0x03BF))
            ow["47"] = true; // Shoal Shell

        if (sav.GetEventFlag(0x0444) || sav.GetEventFlag(0x020C))
            ow["48"] = true; // Red Shard

        if (sav.GetEventFlag(0x0445) || sav.GetEventFlag(0x0200))
            ow["49"] = true; // Blue Shard

        if (sav.GetEventFlag(0x0451) || sav.GetEventFlag(0x01FD))
            ow["49"] = true; // Green Shard

        if (sav.GetEventFlag(0x00C0))
            ow["93"] = true; // Sun Stone

        if (sav.GetEventFlag(0x0416))
            ow["94"] = true; // Moon Stone

        if (sav.GetEventFlag(0x0403))
            ow["98"] = true; // Leaf Stone

        if (sav.GetEventFlag(0x042B))
            ow["123"] = true; // Glitter Mail

        if (sav.GetEventFlag(0x042D))
            ow["127"] = true; // Bed Mail

        if (sav.GetEventFlag(0x042C))
            ow["129"] = true; // Glitter Mail

        if (sav.GetEventFlag(0x009C)) // Skitty for Meowth trade
            ow["132"] = true; // Retro Mail

        if (HasPkm(367) && sav.GetEventFlag(0x0126)) // Huntail, FLAG_EXCHANGED_SCANNER
            ow["192"] = true; // DeepSeaTooth

        if (HasPkm(368) && sav.GetEventFlag(0x0126)) // Gorebyss, FLAG_EXCHANGED_SCANNER
            ow["193"] = true; // DeepSeaScale

        if (sav.GetEventFlag(0x0436))
            ow["278"] = true; // Scanner

        if (sav.GetEventFlag(0x044C) || sav.GetEventFlag(0x00EF))
            ow["285"] = true; // Storage Key

        if (sav.GetEventFlag(0x014F) || sav.GetEventFlag(0x03C3))
            ow["286"] = true; // Root Fossil

        if (sav.GetEventFlag(0x0150) || sav.GetEventFlag(0x03C4))
            ow["287"] = true; // Claw Fossil

        if (sav.GetEventFlag(0x00AC))
            ow["291"] = true; // TM03 - Water Pulse

        if (sav.GetEventFlag(0x0458))
            ow["295"] = true; // TM07 - Hail

        if (sav.GetEventFlag(0x00EA))
            ow["332"] = true; // TM44 - Rest

        if (sav.GetEventFlag(0x010D))
            ow["334"] = true; // TM46 - Thief
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
        ow["BaltoyDoll"] = HasDeco(Decoration3.BALTOY_DOLL);
        ow["ChikoritaDoll"] = HasDeco(Decoration3.CHIKORITA_DOLL);
        ow["ClefairyDoll"] = HasDeco(Decoration3.CLEFAIRY_DOLL);
        ow["CyndaquilDoll"] = HasDeco(Decoration3.CYNDAQUIL_DOLL);
        ow["DittoDoll"] = HasDeco(Decoration3.DITTO_DOLL);
        ow["DuskullDoll"] = HasDeco(Decoration3.DUSKULL_DOLL);
        ow["GulpinDoll"] = HasDeco(Decoration3.GULPIN_DOLL);
        ow["JigglypuffDoll"] = HasDeco(Decoration3.JIGGLYPUFF_DOLL);
        ow["KecleonDoll"] = HasDeco(Decoration3.KECLEON_DOLL);
        ow["LotadDoll"] = HasDeco(Decoration3.LOTAD_DOLL);
        ow["MarillDoll"] = HasDeco(Decoration3.MARILL_DOLL);
        ow["MeowthDoll"] = HasDeco(Decoration3.MEOWTH_DOLL);
        ow["MudkipDoll"] = HasDeco(Decoration3.MUDKIP_DOLL);
        ow["PichuDoll"] = HasDeco(Decoration3.PICHU_DOLL);
        ow["PikachuDoll"] = HasDeco(Decoration3.PIKACHU_DOLL);
        ow["SeedotDoll"] = HasDeco(Decoration3.SEEDOT_DOLL);
        ow["SkittyDoll"] = HasDeco(Decoration3.SKITTY_DOLL);
        ow["SmoochumDoll"] = HasDeco(Decoration3.SMOOCHUM_DOLL);
        ow["SwabluDoll"] = HasDeco(Decoration3.SWABLU_DOLL);
        ow["TogepiDoll"] = HasDeco(Decoration3.TOGEPI_DOLL);
        ow["TorchicDoll"] = HasDeco(Decoration3.TORCHIC_DOLL);
        ow["TotodileDoll"] = HasDeco(Decoration3.TOTODILE_DOLL);
        ow["TreeckoDoll"] = HasDeco(Decoration3.TREECKO_DOLL);
        ow["WynautDoll"] = HasDeco(Decoration3.WYNAUT_DOLL);
        ow["BlastoiseDoll"] = HasDeco(Decoration3.BLASTOISE_DOLL);
        ow["CharizardDoll"] = HasDeco(Decoration3.CHARIZARD_DOLL);
        ow["LaprasDoll"] = HasDeco(Decoration3.LAPRAS_DOLL);
        ow["RhydonDoll"] = HasDeco(Decoration3.RHYDON_DOLL);
        ow["SnorlaxDoll"] = HasDeco(Decoration3.SNORLAX_DOLL);
        ow["VenusaurDoll"] = HasDeco(Decoration3.VENUSAUR_DOLL);
        ow["WailmerDoll"] = HasDeco(Decoration3.WAILMER_DOLL);
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

        ow["StarterPokemon"] = sav.GetAllPKM().Count > 0;
        ow["WynautEgg"] = sav.GetEventFlag(0x010A);
        ow["LileeporAnorith"] = sav.GetEventFlag(0x010B) || HasItem(286 /*Root Fossil*/) || HasItem(287 /*Claw Fossil*/);

        ow["Castform"] = sav.GetEventFlag(0x0097);
        ow["Beldum"] = sav.GetEventFlag(0x012A);
        ow["ChikoritaorCyndaquilorTotodile"] = sav.GetEventFlag(2148) && (sav.GetEventFlag(0x0346) || sav.GetEventFlag(0x032C) || sav.GetEventFlag(0x032B)); //HallOfFame && (picked Chiko or Cynd or Toto)
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
        ow["SilverAbilitySymbol"] = sav.GetEventFlag(2244);

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
        return HasRibbon(pk => func(pk) >= wantedVal);
    }

    public void Generate_ribbon()
    {
        var ow = new Dictionary<string, bool>();
        owned["ribbon"] = ow;

        ow["ChampionRibbon"] = HasRibbon(p => p.RibbonChampionG3);
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

    public void Generate_phone()
    {
        var ow = new Dictionary<string, bool>();
        owned["phone"] = ow;

        ow["PKMNLoverWally"] = sav.GetEventFlag(0x0D6);
        ow["ElusiveEyesScott"] = sav.GetEventFlag(0x0D7);
        ow["CalmKindMom"] = sav.GetEventFlag(0x0D8);
        ow["RadNeighborBrendan"] = sav.GetEventFlag(0x0FD) && sav.Gender != 0;
        ow["RadNeighborMay"] = sav.GetEventFlag(0x0FD) && sav.Gender == 0;
        ow["PKMNProfProfBirch"] = sav.GetEventFlag(0x119); // FLAG_ENABLE_PROF_BIRCH_MATCH_CALL
        ow["HardasRockSteven"] = sav.GetEventFlag(0x131); // FLAG_REGISTERED_STEVEN_POKENAV
        ow["ReliableOneDad"] = sav.GetEventFlag(0x132); // FLAG_ENABLE_NORMAN_MATCH_CALL
        ow["DevonPresMrStone"] = sav.GetEventFlag(0x158); // FLAG_ENABLE_MR_STONE_POKENAV
        ow["AromaLadyRose"] = sav.GetEventFlag(0x15D - 1);
        ow["RuinManiacAndres"] = sav.GetEventFlag(0x15E - 1);
        ow["RuinManiacDusty"] = sav.GetEventFlag(0x15F - 1);
        ow["TuberLola"] = sav.GetEventFlag(0x160 - 1);
        ow["TuberRicky"] = sav.GetEventFlag(0x161 - 1);
        ow["SisandBroLilaRoy"] = sav.GetEventFlag(0x162 - 1);
        ow["CooltrainerCristin"] = sav.GetEventFlag(0x163 - 1);
        ow["CooltrainerBrooke"] = sav.GetEventFlag(0x164 - 1);
        ow["CooltrainerWilton"] = sav.GetEventFlag(0x165 - 1);
        ow["HexManiacValerie"] = sav.GetEventFlag(0x166 - 1);
        ow["LadyCindy"] = sav.GetEventFlag(0x167 - 1);
        ow["BeautyThalia"] = sav.GetEventFlag(0x168 - 1);
        ow["BeautyJessica"] = sav.GetEventFlag(0x169 - 1);
        ow["RichBoyWinston"] = sav.GetEventFlag(0x16A - 1);
        ow["PokeManiacSteve"] = sav.GetEventFlag(0x16B - 1);
        ow["SwimmerTony"] = sav.GetEventFlag(0x16C - 1);
        ow["BlackBeltNob"] = sav.GetEventFlag(0x16D - 1);
        ow["BlackBeltKoji"] = sav.GetEventFlag(0x16E - 1);
        ow["GuitaristFernando"] = sav.GetEventFlag(0x16F - 1);
        ow["GuitaristDalton"] = sav.GetEventFlag(0x170 - 1);
        ow["KindlerBernie"] = sav.GetEventFlag(0x171 - 1);
        ow["CamperEthan"] = sav.GetEventFlag(0x172 - 1);
        ow["OldCoupleJohnJay"] = sav.GetEventFlag(0x173 - 1);
        ow["ManiacJeffrey"] = sav.GetEventFlag(0x174 - 1);
        ow["PsychicCameron"] = sav.GetEventFlag(0x175 - 1);
        ow["PsychicJacki"] = sav.GetEventFlag(0x176 - 1);
        ow["GentlemanWalter"] = sav.GetEventFlag(0x177 - 1);
        ow["SchoolKidKaren"] = sav.GetEventFlag(0x178 - 1);
        ow["SchoolKidJerry"] = sav.GetEventFlag(0x179 - 1);
        ow["SrandJrAnnaMeg"] = sav.GetEventFlag(0x17A - 1);
        ow["PokefanIsabel"] = sav.GetEventFlag(0x17B - 1);
        ow["PokefanMiguel"] = sav.GetEventFlag(0x17C - 1);
        ow["ExpertTimothy"] = sav.GetEventFlag(0x17D - 1);
        ow["ExpertShelby"] = sav.GetEventFlag(0x17E - 1);
        ow["YoungsterCalvin"] = sav.GetEventFlag(0x17F - 1);
        ow["FishermanElliot"] = sav.GetEventFlag(0x180 - 1);
        ow["TriathleteIsaiah"] = sav.GetEventFlag(0x181 - 1);
        ow["TriathleteMaria"] = sav.GetEventFlag(0x182 - 1);
        ow["TriathleteAbigail"] = sav.GetEventFlag(0x183 - 1);
        ow["TriathleteDylan"] = sav.GetEventFlag(0x184 - 1);
        ow["TriathleteKatelyn"] = sav.GetEventFlag(0x185 - 1);
        ow["TriathleteBenjamin"] = sav.GetEventFlag(0x186 - 1);
        ow["TriathletePablo"] = sav.GetEventFlag(0x187 - 1);
        ow["DragonTamerNicolas"] = sav.GetEventFlag(0x188 - 1);
        ow["BirdKeeperRobert"] = sav.GetEventFlag(0x189 - 1);
        ow["NinjaBoyLao"] = sav.GetEventFlag(0x18A - 1);
        ow["BattleGirlCyndy"] = sav.GetEventFlag(0x18B - 1);
        ow["ParasolLadyMadeline"] = sav.GetEventFlag(0x18C - 1);
        ow["SwimmerJenny"] = sav.GetEventFlag(0x18D - 1);
        ow["PicnickerDiana"] = sav.GetEventFlag(0x18E - 1);
        ow["TwinsAmyLiv"] = sav.GetEventFlag(0x18F - 1);
        ow["SailorErnest"] = sav.GetEventFlag(0x190 - 1);
        ow["SailorCory"] = sav.GetEventFlag(0x191 - 1);
        ow["CollectorEdwin"] = sav.GetEventFlag(0x192 - 1);
        ow["PKMNBreederLydia"] = sav.GetEventFlag(0x193 - 1);
        ow["PKMNBreederIsaac"] = sav.GetEventFlag(0x194 - 1);
        ow["PKMNBreederGabrielle"] = sav.GetEventFlag(0x195 - 1);
        ow["PKMNRangerCatherine"] = sav.GetEventFlag(0x196 - 1);
        ow["PKMNRangerJackson"] = sav.GetEventFlag(0x197 - 1);
        ow["LassHaley"] = sav.GetEventFlag(0x198 - 1);
        ow["BugCatcherJames"] = sav.GetEventFlag(0x199 - 1);
        ow["HikerTrent"] = sav.GetEventFlag(0x19A - 1);
        ow["HikerSawyer"] = sav.GetEventFlag(0x19B - 1);
        ow["YoungCoupleKiraDan"] = sav.GetEventFlag(0x19C - 1);
        ow["RockinWhizRoxanne"] = sav.GetEventFlag(0x19E - 1);
        ow["TheBigHitBrawly"] = sav.GetEventFlag(0x19F - 1);
        ow["SwellShockWattson"] = sav.GetEventFlag(0x1A0 - 1);
        ow["PassionBurnFlannery"] = sav.GetEventFlag(0x1A1 - 1);
        ow["SkyTamerWinona"] = sav.GetEventFlag(0x1A3 - 1);
        ow["MysticDuoTateLiza"] = sav.GetEventFlag(0x1A4 - 1);
        ow["EliteFourSidney"] = sav.GetEventFlag(0x1A5 - 1);
        ow["EliteFourPhoebe"] = sav.GetEventFlag(0x1A6 - 1);
        ow["EliteFourGlacia"] = sav.GetEventFlag(0x1A7 - 1);
        ow["EliteFourDrake"] = sav.GetEventFlag(0x1A8 - 1);
        ow["ChampionWallace"] = sav.GetEventFlag(0x1A9 - 1);
        ow["DandyCharmJuan"] = sav.GetEventFlag(0x1D9); // special. FLAG_ENABLE_JUAN_MATCH_CALL
    }

    public void Generate_trainerBattle()
    {
        var ow = new Dictionary<string, bool>();
        owned["trainerBattle"] = ow;

        var TRAINER_FLAGS_START = 0x500;
        for (var trainerId = 0; trainerId <= 854; trainerId++)
            ow[trainerId.ToString()] = sav.GetEventFlag(TRAINER_FLAGS_START + trainerId);
    }

    public void Generate_trainerStar()
    {
        var ow = new Dictionary<string, bool>();
        owned["trainerStar"] = ow;

        ow["HallofFame"] = sav.GetEventFlag(2148);

        var HoennPokedex = () =>
        {
            return HoennList.All(id => {
                if (id == 385)  // Jirachi
                    return true;
                if (id == 386)  // Deoxys
                    return true;
                return sav.GetCaught(id);
             });
        };
        ow["HoennPokedex"] = HoennPokedex();

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

        var PokemonRegionalNoTrade = () =>
        {
            var starterCount = 0;
            foreach (var id in StarterList)
            {
                if (sav.GetSeen(id))
                    starterCount++;
            }
            if (starterCount < 5)
                return false;

            if (!sav.GetSeen(380) && !sav.GetSeen(381)) //Lati@s
                return false;

            return HoennListSeeNoTrade.All(id => sav.GetSeen(id));
        };

        ow["PokemonRegionalNoTrade"] = PokemonRegionalNoTrade();
        ow["PokemonGolem"] = sav.GetSeen(76);
        ow["PokemonAlakazam"] = sav.GetSeen(65);
        ow["PokemonJirachi"] = sav.GetSeen(385);
        ow["PokemonDeoxys"] = sav.GetSeen(386);
        ow["PokemonRegionalTrade"] = HoennListSeeTrade.All(id => sav.GetSeen(id));
        ow["PokemonNonRegional"] = sav.GetEventFlag(0x0896);
        ow["Events"] = sav.GetEventFlag(2148); // Hall of Fame
        ow["Move1"] = sav.GetEventFlag(2148); // Hall of Fame
        ow["Move2"] = sav.GetEventFlag(2148); // Hall of Fame

        var IsTrendyUnlocked = (int bitIdx) =>
        {
            // 0x2E20 u8 unlockedTrendySayings[NUM_TRENDY_SAYING_BYTES];
            var byteIdx = bitIdx / 8;
            var bitoffset = bitIdx % 8;
            var span = sav.Large.AsSpan(0x2E20, 5);
            return ((span[byteIdx] >> bitoffset) & 1) != 0;
        };
        ow["TrendyKTHXBYE"] = IsTrendyUnlocked(0);
        ow["TrendyYESSIR"] = IsTrendyUnlocked(1);
        ow["TrendyAVANTGARDE"] = IsTrendyUnlocked(2);
        ow["TrendyCOUPLE"] = IsTrendyUnlocked(3);
        ow["TrendyMUCHOBLIGED"] = IsTrendyUnlocked(4);
        ow["TrendyYEEHAW"] = IsTrendyUnlocked(5);
        ow["TrendyMEGA"] = IsTrendyUnlocked(6);
        ow["Trendy1HITKO"] = IsTrendyUnlocked(7);
        ow["TrendyDESTINY"] = IsTrendyUnlocked(8);
        ow["TrendyCANCEL"] = IsTrendyUnlocked(9);
        ow["TrendyNEW"] = IsTrendyUnlocked(10);
        ow["TrendyFLATTEN"] = IsTrendyUnlocked(11);
        ow["TrendyKIDDING"] = IsTrendyUnlocked(12);
        ow["TrendyLOSER"] = IsTrendyUnlocked(13);
        ow["TrendyLOSING"] = IsTrendyUnlocked(14);
        ow["TrendyHAPPENING"] = IsTrendyUnlocked(15);
        ow["TrendyHIPAND"] = IsTrendyUnlocked(16);
        ow["TrendySHAKE"] = IsTrendyUnlocked(17);
        ow["TrendySHADY"] = IsTrendyUnlocked(18);
        ow["TrendyUPBEAT"] = IsTrendyUnlocked(19);
        ow["TrendyMODERN"] = IsTrendyUnlocked(20);
        ow["TrendySMELLYA"] = IsTrendyUnlocked(21);
        ow["TrendyBANG"] = IsTrendyUnlocked(22);
        ow["TrendyKNOCKOUT"] = IsTrendyUnlocked(23);
        ow["TrendyHASSLE"] = IsTrendyUnlocked(24);
        ow["TrendyWINNER"] = IsTrendyUnlocked(25);
        ow["TrendyFEVER"] = IsTrendyUnlocked(26);
        ow["TrendyWANNABE"] = IsTrendyUnlocked(27);
        ow["TrendyBABY"] = IsTrendyUnlocked(28);
        ow["TrendyHEART"] = IsTrendyUnlocked(29);
        ow["TrendyOLD"] = IsTrendyUnlocked(30);
        ow["TrendyYOUNG"] = IsTrendyUnlocked(31);
        ow["TrendyUGLY"] = IsTrendyUnlocked(32);
    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;

        ow["DefeatSteven"] = sav.GetEventFlag(0x04F8);

        var large = sav.Large;
        var toFind = new List<byte> { 204, 201, 187, 197 };

        for(var i = 0; i < large.Length - 4; i++) { 
            if (large[i] == toFind[0] && large[i + 1] == toFind[1] && large[i + 2] == toFind[2] && large[i +3] == toFind[3])
                Console.WriteLine(i);
        }

        //ow["BattleviaGameLinkCable"] = sav.BattleVideo

        // ow["BeatSlateportBattleTent"] = true; // Not trackable
        // ow["BeatVerdanturfBattleTent"] = true; // Not trackable
        // ow["BeatFallarborBattleTent"] = true; // Not trackable
        ow["UnlockMysteryGiftSystem"] = sav.GetEventFlag(0x08DB);
        ow["UnlockNationalPokedex"] = sav.GetEventFlag(0x0896);
        ow["UnlockWaldaCustomWallpaper"] = sav.WaldaUnlocked;
        
        var TrainerHill = (int offset) =>
        {
            var timer = BinaryPrimitives.ReadUInt32LittleEndian(sav.Large.AsSpan(offset, 4));
            return timer != 0 && timer < 12 * 60 * 60;
        };
        ow["WinBestPrizeTrainerHillNormalMode"] = TrainerHill(0x3718 + 0);
        ow["WinBestPrizeTrainerHillVarietyMode"] = TrainerHill(0x3718 + 4); ///uint32
        ow["WinBestPrizeTrainerHillUniqueMode"] = TrainerHill(0x3718 + 8);
        ow["WinBestPrizeTrainerHillExpertMode"] = TrainerHill(0x3718 + 12);

        var BerryBlenderRecord = (int offset) =>
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(sav.Large.AsSpan(offset, 2)) != 0;
        };

        ow["SetBerryBlenderRecord2Players"] = BerryBlenderRecord(0x9BC + 0);
        ow["SetBerryBlenderRecord3Players"] = BerryBlenderRecord(0x9BC + 2);
        ow["SetBerryBlenderRecord4Players"] = BerryBlenderRecord(0x9BC + 4);

        ow["SetPokemonJumpRecord"] = sav.JoyfulJumpScore > 0;
        ow["SetPokemonJumpExcellentsinaRowRecord"] = sav.JoyfulJump5InRow > 0;
        ow["SetBerryPickingRecord"] = sav.JoyfulBerriesScore > 0;
        ow["SetBerryPickingInrowwith5playersRecord"] = sav.JoyfulBerries5InRow > 0;
    }
}