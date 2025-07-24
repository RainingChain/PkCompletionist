using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using static System.Buffers.Binary.BinaryPrimitives;
using static System.Formats.Asn1.AsnWriter;

namespace PkCompletionist.Core;

/*
 * Modifications to PkHex.Core
 * Geonet
 * 
 * */

internal class CompletionValidator5 : CompletionValidatorX
{
    public CompletionValidator5(Command command, SAV5B2W2 sav, Objective objective) : base(command, sav, objective)
    {
        this.sav = sav;

        this.unobtainableItems = new List<int>() { 16, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 147, 429, 430, 432, 436, 441, 455, 456, 457, 458, 463, 468, 469, 470, 471, 472, 473, 474, 475, 476, 477, 478, 479, 480, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499, 501, 502, 503, 504, 532, 533, 534, 535, 536 };


        var UnlockedNationalDex = () =>
        {
            var list = new List<ushort> { 108, 111, 112, 113, 114, 118, 119, 122, 123, 125, 126, 129, 130, 133, 134, 135, 136, 137, 143, 163, 164, 169, 172, 173, 175, 176, 183, 184, 185, 190, 193, 194, 195, 196, 197, 198, 200, 201, 203, 207, 208, 212, 214, 215, 220, 221, 223, 224, 226, 228, 229, 233, 239, 240, 242, 25, 26, 265, 266, 267, 268, 269, 278, 279, 280, 281, 282, 298, 299, 307, 308, 315, 333, 334, 339, 340, 349, 35, 350, 355, 356, 357, 358, 359, 36, 361, 362, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 41, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 42, 420, 421, 422, 423, 424, 425, 426, 427, 428, 429, 430, 431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449, 450, 451, 452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474, 475, 476, 477, 478, 479, 480, 481, 482, 483, 484, 487, 490, 54, 55, 63, 64, 65, 66, 67, 68, 72, 73, 74, 75, 76, 77, 78, 81, 82, 92, 93, 94, 95 };
            return list.All(i => sav.Zukan.GetSeen(i));
        };
        this.UnlockedNationalDex = UnlockedNationalDex();

    }
    bool UnlockedNationalDex = false;

    new SAV5B2W2 sav;
    static public int NumberOfSetBits(int i)
    {
        i = i - ((i >> 1) & 0x55555555);
        i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
        return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
    }

    public override void GenerateAll()
    {
        base.GenerateAll();

        Generate_pokemonForm();
        Generate_itemInMap();
        Generate_itemGift();
        Generate_inGameTrade();
        Generate_inGameGift();

        //Generate_joinAvenue();

        Generate_ribbon();
        Generate_battleSubway();
        //Generate_pwt();
        //Generate_medal();
        Generate_trainerStar();
        //Generate_pokestarStudios();
        //Generate_props();
        //Generate_pokemonMusical();
        //Generate_funfest();

        Generate_battle();
        Generate_misc();
    }

    public void Generate_pokemonForm()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemonForm"] = ow;

        ow["UnownA"] = HasOrSeenPkmBasedOnObjective2(201, 0);
        ow["UnownB"] = HasOrSeenPkmBasedOnObjective2(201, 1);
        ow["UnownC"] = HasOrSeenPkmBasedOnObjective2(201, 2);
        ow["UnownD"] = HasOrSeenPkmBasedOnObjective2(201, 3);
        ow["UnownE"] = HasOrSeenPkmBasedOnObjective2(201, 4);
        ow["UnownF"] = HasOrSeenPkmBasedOnObjective2(201, 5);
        ow["UnownG"] = HasOrSeenPkmBasedOnObjective2(201, 6);
        ow["UnownH"] = HasOrSeenPkmBasedOnObjective2(201, 7);
        ow["UnownI"] = HasOrSeenPkmBasedOnObjective2(201, 8);
        ow["UnownJ"] = HasOrSeenPkmBasedOnObjective2(201, 9);
        ow["UnownK"] = HasOrSeenPkmBasedOnObjective2(201, 10);
        ow["UnownL"] = HasOrSeenPkmBasedOnObjective2(201, 11);
        ow["UnownM"] = HasOrSeenPkmBasedOnObjective2(201, 12);
        ow["UnownN"] = HasOrSeenPkmBasedOnObjective2(201, 13);
        ow["UnownO"] = HasOrSeenPkmBasedOnObjective2(201, 14);
        ow["UnownP"] = HasOrSeenPkmBasedOnObjective2(201, 15);
        ow["UnownQ"] = HasOrSeenPkmBasedOnObjective2(201, 16);
        ow["UnownR"] = HasOrSeenPkmBasedOnObjective2(201, 17);
        ow["UnownS"] = HasOrSeenPkmBasedOnObjective2(201, 18);
        ow["UnownT"] = HasOrSeenPkmBasedOnObjective2(201, 19);
        ow["UnownU"] = HasOrSeenPkmBasedOnObjective2(201, 20);
        ow["UnownV"] = HasOrSeenPkmBasedOnObjective2(201, 21);
        ow["UnownW"] = HasOrSeenPkmBasedOnObjective2(201, 22);
        ow["UnownX"] = HasOrSeenPkmBasedOnObjective2(201, 23);
        ow["UnownY"] = HasOrSeenPkmBasedOnObjective2(201, 24);
        ow["UnownZ"] = HasOrSeenPkmBasedOnObjective2(201, 25);
        ow["UnownExclamationMark"] = HasOrSeenPkmBasedOnObjective2(201, 26);
        ow["UnownQuestionMark"] = HasOrSeenPkmBasedOnObjective2(201, 27);

        ow["CastformNormal"] = HasOrSeenPkmBasedOnObjective(351);
        ow["CastformFire"] = HasOrSeenPkmBasedOnObjective(351);
        ow["CastformWater"] = HasOrSeenPkmBasedOnObjective(351);
        ow["CastformIce"] = HasOrSeenPkmBasedOnObjective(351);

        ow["DeoxysNormal"] = HasOrSeenPkmBasedOnObjective2(386, 0);
        ow["DeoxysAttack"] = HasOrSeenPkmBasedOnObjective2(386, 1);
        ow["DeoxysDefense"] = HasOrSeenPkmBasedOnObjective2(386, 2);
        ow["DeoxysSpeed"] = HasOrSeenPkmBasedOnObjective2(386, 3);
        ow["BurmyPlantCloak"] = HasOrSeenPkmBasedOnObjective2(412, 0);
        ow["BurmySandyCloak"] = HasOrSeenPkmBasedOnObjective2(412, 1);
        ow["BurmyTrashCloak"] = HasOrSeenPkmBasedOnObjective2(412, 2);
        ow["WormadamPlantCloak"] = HasOrSeenPkmBasedOnObjective2(413, 0);
        ow["WormadamSandyCloak"] = HasOrSeenPkmBasedOnObjective2(413, 1);
        ow["WormadamTrashCloak"] = HasOrSeenPkmBasedOnObjective2(413, 2);
        ow["CherrimOvercast"] = HasOrSeenPkmBasedOnObjective(421);
        ow["CherrimSunshine"] = HasOrSeenPkmBasedOnObjective(421); //kinda bad
        ow["ShellosWestSea"] = HasOrSeenPkmBasedOnObjective2(422, 0);
        ow["ShellosEastSea"] = HasOrSeenPkmBasedOnObjective2(422, 1);
        ow["GastrodonWestSea"] = HasOrSeenPkmBasedOnObjective2(423, 0);
        ow["GastrodonEastSea"] = HasOrSeenPkmBasedOnObjective2(423, 1);
        ow["RotomGhost"] = HasOrSeenPkmBasedOnObjective2(479, 0);
        ow["RotomHeat"] = HasOrSeenPkmBasedOnObjective2(479, 1);
        ow["RotomWash"] = HasOrSeenPkmBasedOnObjective2(479, 2);
        ow["RotomFrost"] = HasOrSeenPkmBasedOnObjective2(479, 3);
        ow["RotomFan"] = HasOrSeenPkmBasedOnObjective2(479, 4);
        ow["RotomMow"] = HasOrSeenPkmBasedOnObjective2(479, 5);
        ow["GiratinaAltered"] = HasOrSeenPkmBasedOnObjective2(487, 0);
        ow["GiratinaOrigin"] = HasSeenPkmForm(487, 1);
        ow["ShayminLand"] = HasOrSeenPkmBasedOnObjective2(492, 0);
        ow["ShayminSky"] = HasOrSeenPkmBasedOnObjective2(492, 1);
        ow["GetShinyPokemon"] = HasShinyMon();

        var HasArceus = this.OwnedPkms.Contains(493);
        ow["ArceusNormal"] = HasArceus;
        ow["ArceusFire"] = HasArceus;
        ow["ArceusWater"] = HasArceus;
        ow["ArceusElectric"] = HasArceus;
        ow["ArceusGrass"] = HasArceus;
        ow["ArceusIce"] = HasArceus;
        ow["ArceusFighting"] = HasArceus;
        ow["ArceusPoison"] = HasArceus;
        ow["ArceusGround"] = HasArceus;
        ow["ArceusFly"] = HasArceus;
        ow["ArceusPsychic"] = HasArceus;
        ow["ArceusBug"] = HasArceus;
        ow["ArceusRock"] = HasArceus;
        ow["ArceusGhost"] = HasArceus;
        ow["ArceusDragon"] = HasArceus;
        ow["ArceusDark"] = HasArceus;
        ow["ArceusSteel"] = HasArceus;

        ow["AllMaleFemaleForms"] = new Func<bool>(() =>
        {
            var retStr = new List<string>();
            var ret = true;
            var genderLess = new HashSet<ushort> { 81, 82, 100, 101, 120, 121, 137, 233, 292, 337, 338, 343, 344, 374, 375, 376, 436, 437, 462, 474, 479, 479, 479, 479, 479, 479, 489, 490, 132, 144, 144, 145, 145, 146, 146, 150, 151, 201, 243, 244, 245, 249, 250, 251, 377, 378, 379, 382, 383, 384, 385, 386, 386, 386, 386, 480, 481, 482, 483, 484, 486, 487, 487, 491, 492, 492, 493 };
            for (ushort i = 0; i <= 493; i++)
            {
                if (!this.sav.Zukan.GetSeen(i))
                {
                    ret = false;
                    retStr.Add("#" + i.ToString());
                    continue;
                }
                if (genderLess.Contains(i))
                    continue;
                //TODO
                /*if (this.sav.Zukan.GetSeenGenderFirst(i) == this.sav.Zukan.GetSeenGenderSecond(i)) // 01 and 10 is when both genders are seen. 00 and 11 are when 1 is seen.
                {
                    ret = false;
                    retStr.Add("#" + i.ToString());
                    continue;
                }*/
            }

            if (!ret)
                this.incompleteHints.Add("Missing Male & Female Forms: Pokemon " + String.Join(", ", retStr.ToArray()));
            return ret;
        })();

        //TODO
        /*
        ow["AllForeignPokedexEntries"] = new Func<bool>(() =>
        {
            var languages = new List<string> { "JP", "EN", "FR", "IT", "GE", "SP", "KO" };
            var retStr = new List<string>();
            var ret = true;
            for (ushort i = 1; i <= 493; i++)
            {
                for (int j = 0; j < 6; j++)
                    if (!this.sav.Zukan.GetLanguageBitIndex(i, j))
                    {
                        retStr.Add("#" + i.ToString() + " (" + languages[j] + ")");
                        ret = false;
                    }
            }

            if (!ret)
                this.incompleteHints.Add("Missing Foreign Pokedex Entries: Pokemon " + String.Join(", ", retStr.ToArray()));
            return ret;
        })();*/
    }

    public bool HasSeenPkmForm(ushort species, byte form)
    {
        //TODO
        return false; // this.sav.Zukan.GetForms(species).Contains(form);
    }

    public bool HasOrSeenPkmBasedOnObjective2(ushort species, byte form)
    {
        return this.objective != 0 ? HasPkmForm(species, form) : HasSeenPkmForm(species, form);
    }

    public override void Generate_item()
    {
        base.Generate_item();

        var ow = this.owned["item"];
        //TODO
    }

    public void Generate_itemInMap()
    {
        var ow = new Dictionary<string, bool>();
        owned["itemInMap"] = ow;

        var list = new List<int> {  };

        var untrackable = new HashSet<int> { };

        foreach (var id in list)
        {
            var val = sav.GetEventFlag(id);
            if (!val && untrackable.Contains(id))
                continue;
            ow[id.ToString()] = val;
        }
    }

    public void Generate_itemGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["itemGift"] = ow;
        var list = new List<int> {};

        foreach (var id in list)
            ow[id.ToString()] = sav.GetEventFlag(id);
    }

    public void Generate_inGameTrade()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameTrade"] = ow;

        //TODO
        /*ow["MachopforAbra"] = sav.GetEventFlag(0x0085);
        ow["BuizelforChatot"] = sav.GetEventFlag(0x0086);
        ow["MedichamforHaunter"] = sav.GetEventFlag(0x00F4);
        ow["FinneonforMagikarp"] = sav.GetEventFlag(0x00F5);*/
    }
    public void Generate_inGameGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameGift"] = ow;

        //TODO
        /*ow["StarterPokemon"] = true;
        ow["TogepiEgg"] = sav.GetWork(0x007A) >= 5;
        ow["Eevee"] = sav.GetEventFlag(305);
        ow["Porygon"] = sav.GetEventFlag(151);
        ow["RioluEgg"] = HasPkmWithTID(447) || HasPkmWithTID(448); //bad*/
    }

    public void Generate_pcWallPaper()
    {
        var ow = new Dictionary<string, bool>();
        owned["pcWallPaper"] = ow;

        //TODO
    }



    public void Generate_battleSubway()
    {
        var ow = new Dictionary<string, bool>();
        owned["battleSubway"] = ow;
        //TODO
    }

    public bool HasRibbon(Func<PK5, bool> func)
    {
        var pkms = sav.GetAllPKM();
        return pkms.Any(pk =>
        {
            var pk4 = (PK5?)pk;
            if (pk4 == null)
                return false;
            return func(pk4);
        });
    }
    public bool HasContestRibbon(Func<PK5, byte> func, byte wantedVal)
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
        ow["LegendRibbon"] = HasRibbon(pk4 => pk4.RibbonLegend);
        ow["PremierRibbon"] = HasRibbon(pk4 => pk4.RibbonPremier);

        //TODO
    }
    public bool EnteredHallOfFame()
    {
        return false; //TODO sav.GetEventFlag(2404);
    }


    public void Generate_trainerStar()
    {
        var ow = new Dictionary<string, bool>();
        owned["trainerStar"] = ow;

        ow["HallofFame"] = EnteredHallOfFame();

        var NationalPokedex = () =>
        {
            for (ushort i = 0; i <= 493; i++)
            {
                //TODO
                if (i == 151 || i == 249 || i == 250 || i == 251 || i == 385 || i == 386 || i == 489 || i == 490 || i == 491 || i == 492 || i == 493)
                    continue;
                if (!sav.Zukan.GetCaught(i))
                    return false;
            }
            return true;
        };
        ow["NationalPokedex"] = NationalPokedex();
        //TODO
    }

    public bool GetThoughWordUnlocked(ThoughWord word)
    {
        var idx = (byte)word;
        return false; // TODO FlagUtil.GetFlag(sav.General, 0xCEB4 + idx / 8, idx % 8);
    }
    public void Generate_easyChatSystemWord()
    {
        var ow = new Dictionary<string, bool>();
        owned["easyChatSystemWord"] = ow;


        var HasSeenAllCatchableNoTrade = () =>
        {
            var list = new List<ushort> { 16, 17, 18, 19, 20, 21, 22, 25, 26, 29, 30, 31, 32, 33, 34, 35, 36, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 60, 61, 62, 63, 64, 66, 67, 69, 70, 71, 72, 73, 74, 75, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 201, 202, 203, 206, 207, 208, 209, 210, 211, 214, 215, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 231, 232, 234, 235, 236, 237, 238, 239, 240, 241, 242, 246, 247, 248, 261, 262, 263, 264, 265, 266, 267, 268, 269, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 318, 319, 320, 321, 322, 323, 324, 325, 326, 327, 331, 332, 333, 334, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 369, 370, 371, 372, 373, 374, 375, 376, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 433, 436, 437, 438, 439, 440, 441, 443, 444, 445, 446, 447, 448, 449, 450, 451, 452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463, 465, 468, 469, 470, 471, 472, 473, 475, 476, 478, 479, 480, 481, 482, 483, 484, 485, 487, 488 };
            return list.All(mon => this.sav.Zukan.GetSeen(mon));
        };

        var HasSeenMon = (ushort mon) =>
        {
            return this.sav.Zukan.GetSeen(mon);
        };

        var HasSeenMonAll = () =>
        {
            for (ushort mon = 1; mon <= 493; mon++)
                if (!this.sav.Zukan.GetSeen(mon))
                    return false;
            return true;
        };

        ow["PokemonAllCatchableNoTrade"] = HasSeenAllCatchableNoTrade();
        ow["PokemonOthers"] = HasSeenMonAll();
        ow["Moves"] = UnlockedNationalDex;
    }

    public void Generate_battle()
    {
        var ow = new Dictionary<string, bool>();
        owned["battle"] = ow;

        var untrackable = new HashSet<int> {};
        /* TODO
        for (var i = 1; i < 928; i++)
        {
            var got = sav.GetEventFlag(0x550 + i);
            if (!got && untrackable.Contains(i))
                continue;
            ow[i.ToString()] = got;
        }*/

    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;
        //TODO
    }
}
