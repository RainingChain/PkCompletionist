using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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

        // a.list.filter(a => a.obtainable === -1).map(a => +a.id).toString()
        this.unobtainableItems = new List<int>() { 5,16,113,114,115,120,121,122,123,124,125,126,127,128,129,130,131,132,133,260,261,262,263,264,426,427,428,429,430,431,432,433,434,435,436,438,439,440,441,443,444,445,446,448,449,451,452,454,455,456,457,459,460,461,462,463,464,467,468,469,470,472,473,474,475,476,477,478,479,480,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,497,498,499,500,501,502,503,505,506,507,508,509,510,511,512,513,514,515,516,517,518,519,520,521,522,523,524,525,526,527,528,529,530,531,532,533,534,535,536,574,579,592,593,594,595,596,597,598,599,600,601,602,603,604,605,606,607,608,609,610,611,612,613,614,615,616,622,623,624,625 };
    }

    new SAV5B2W2 sav;

    private static int GetDexFormIndexBW(ushort species, byte formCount)
    {
        if (formCount < 1 || species > Legal.MaxSpeciesID_5)
            return -1;

        return species switch
        {
            201 => 000, // 28 Unown
            386 => 028, // 4 Deoxys
            492 => 032, // 2 Shaymin
            487 => 034, // 2 Giratina
            479 => 036, // 6 Rotom
            422 => 042, // 2 Shellos
            423 => 044, // 2 Gastrodon
            412 => 046, // 3 Burmy
            413 => 049, // 3 Wormadam
            351 => 052, // 4 Castform
            421 => 056, // 2 Cherrim
            585 => 058, // 4 Deerling
            586 => 062, // 4 Sawsbuck
            648 => 066, // 2 Meloetta
            555 => 068, // 2 Darmanitan
            550 => 070, // 2 Basculin
            _ => -1,
        };
    }

    private static int GetDexFormIndexB2W2(ushort species, byte formCount)
    {
        if (formCount < 1 || species > Legal.MaxSpeciesID_5)
            return -1;

        return species switch
        {
            646 => 072, // 3 Kyurem
            647 => 075, // 2 Keldeo
            642 => 077, // 2 Thundurus
            641 => 079, // 2 Tornadus
            645 => 081, // 2 Landorus
            _ => GetDexFormIndexBW(species, formCount),
        };
    }

    public override void GenerateAll()
    {
        base.GenerateAll(); // pokemon, item

        Generate_pokemonForm();
        Generate_inGameTrade();
        Generate_inGameGift();

        Generate_itemInMap();
        Generate_itemGift();

        Generate_joinAvenue();

        Generate_ribbon();
        Generate_battleSubway();
        Generate_pwt();
        Generate_medal();

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

        ow["CastformNormal"] = HasOrSeenPkmBasedOnObjective2(351,0);
        ow["CastformFire"] = HasOrSeenPkmBasedOnObjective2(351,1);
        ow["CastformWater"] = HasOrSeenPkmBasedOnObjective2(351,2);
        ow["CastformIce"] = HasOrSeenPkmBasedOnObjective2(351,3);

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
        byte nextForm = 0;
        ow["ArceusNormal"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusFighting"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusFlying"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusPoison"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusGround"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusRock"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusBug"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusGhost"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusSteel"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusFire"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusWater"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusGrass"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusElectric"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusPsychic"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusIce"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusDragon"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);
        ow["ArceusDark"] = HasOrSeenPkmBasedOnObjectiveArceus(493, nextForm++);

        ow["BasculinBlueStripe"] = HasOrSeenPkmBasedOnObjective2(550,0);
        ow["BasculinRedStripe"] = HasOrSeenPkmBasedOnObjective2(550,1);
        ow["DarmanitanStandard"] = HasOrSeenPkmBasedOnObjective2(555,0);
        ow["DarmanitanZen"] = HasOrSeenPkmBasedOnObjective2(555,1);
        ow["DeerlingSpring"] = HasOrSeenPkmBasedOnObjective2(585,0);
        ow["DeerlingSummer"] = HasOrSeenPkmBasedOnObjective2(585,1);
        ow["DeerlingAutumn"] = HasOrSeenPkmBasedOnObjective2(585,2);
        ow["DeerlingWinter"] = HasOrSeenPkmBasedOnObjective2(585,3);
        ow["SawsbuckSpring"] = HasOrSeenPkmBasedOnObjective2(586,0);
        ow["SawsbuckSummer"] = HasOrSeenPkmBasedOnObjective2(586,1);
        ow["SawsbuckAutumn"] = HasOrSeenPkmBasedOnObjective2(586,2);
        ow["SawsbuckWinter"] = HasOrSeenPkmBasedOnObjective2(586,3);
        ow["TornadusIncarnate"] = HasOrSeenPkmBasedOnObjective2(641,0);
        ow["TornadusTherian"] = HasOrSeenPkmBasedOnObjective2(641,1);
        ow["ThundurusIncarnate"] = HasOrSeenPkmBasedOnObjective2(642,0);
        ow["ThundurusTherian"] = HasOrSeenPkmBasedOnObjective2(642,1);
        ow["LandorusIncarnate"] = HasOrSeenPkmBasedOnObjective2(645,0);
        ow["LandorusTherian"] = HasOrSeenPkmBasedOnObjective2(645,1);
        ow["KyuremNormal"] = HasOrSeenPkmBasedOnObjective2(646,0);
        ow["KyuremWhite"] = HasOrSeenPkmBasedOnObjective2(646,1);
        ow["KyuremBlack"] = HasOrSeenPkmBasedOnObjective2(646,2);
        ow["KeldeoOrdinary"] = HasOrSeenPkmBasedOnObjective2(647,0);
        ow["KeldeoResolute"] = HasOrSeenPkmBasedOnObjective2(647,1);
        ow["MeloettaAria"] = HasOrSeenPkmBasedOnObjective2(648,0);
        ow["MeloettaPirouette"] = HasOrSeenPkmBasedOnObjective2(648,1);
        ow["GenesectNormal"] = HasOrSeenPkmBasedOnObjective2(649,0);
        ow["GenesectDouse"] = HasOrSeenPkmBasedOnObjective2(649,1);
        ow["GenesectShock"] = HasOrSeenPkmBasedOnObjective2(649,2);
        ow["GenesectBurn"] = HasOrSeenPkmBasedOnObjective2(649,3);
        ow["GenesectChill"] = HasOrSeenPkmBasedOnObjective2(649,4);

        /*
        Seen: 0 Male, 1 Female, 2 Shiny Male, 3 Shiny Female, 0 Genderless, 1 Shiny Genderless.
            Dex.GetSeen(species, 0-4);
        Displayed: Only 1 can be active at one. it indicates the image displayed for that species. 
        Owned: Dex.GetCaught

        GetLanguageFlag(species - 1, LANG);
        */
        const int MALE_GENDERLESS_NON_SHINY = 0;
        const int FEMALE_NON_SHINY = 1;
        const int MALE_GENDERLESS_SHINY = 2;
        const int FEMALE_SHINY = 3;

        
        ow["AllMaleFemaleForms"] = new Func<bool>(() =>
        {
            var retStr = new List<string>();
            for (ushort i = 0; i <= 649; i++)
            {
                if (!this.sav.Zukan.GetSeen(i))
                {
                    retStr.Add("#" + i.ToString());
                    continue;
                }

                var pi = this.sav.Personal[i];
                if (pi.OnlyFemale || pi.OnlyMale || pi.Genderless){
                    continue;
                }
                
                if (!this.sav.Zukan.GetSeen(i, MALE_GENDERLESS_NON_SHINY) && 
                    !this.sav.Zukan.GetSeen(i, MALE_GENDERLESS_SHINY))
                {
                    retStr.Add($"#{i} (Male)");
                    continue;
                }
                if (!this.sav.Zukan.GetSeen(i, FEMALE_NON_SHINY) && 
                    !this.sav.Zukan.GetSeen(i, FEMALE_SHINY))
                {
                    retStr.Add($"#{i} (Female)");
                    continue;
                }
            }

            if (retStr.Count > 0)
                this.incompleteHints.Add("Missing Male & Female Forms: Pokemon " + String.Join(", ", retStr.ToArray()));
            
            return retStr.Count == 0;
        })();

        ow["1ShinyFormforAllPokemon"] = new Func<bool>(() =>
        {
            var retStr = new List<string>();
            for (ushort i = 0; i <= 649; i++)
            {
                if (!this.sav.Zukan.GetSeen(i))
                {
                    retStr.Add("#" + i.ToString());
                    continue;
                }

                if (!this.sav.Zukan.GetSeen(i, MALE_GENDERLESS_SHINY) && 
                    !this.sav.Zukan.GetSeen(i, FEMALE_SHINY))
                {
                    retStr.Add($"#{i}");
                    continue;
                }
            }

            if (retStr.Count > 0)
                this.incompleteHints.Add("Missing Shiny Forms: Pokemon " + String.Join(", ", retStr.ToArray()));
            
            return retStr.Count == 0;
        })();

        ow["ShinyMaleFemaleFormsforAllPokemon"] = new Func<bool>(() =>
        {
            var retStr = new List<string>();
            for (ushort i = 0; i <= 649; i++)
            {
                var pi = this.sav.Personal[i];
                
                if (!pi.OnlyFemale && 
                    !this.sav.Zukan.GetSeen(i, MALE_GENDERLESS_SHINY))
                {
                    retStr.Add($"#{i} (Male)");
                    continue;
                }

                if (!(pi.OnlyMale || pi.Genderless) && 
                    !this.sav.Zukan.GetSeen(i, FEMALE_SHINY))
                {
                    retStr.Add($"#{i} (Female)");
                    continue;
                }
            }

            if (retStr.Count > 0)
                this.incompleteHints.Add("Missing Shiny Male & Female Forms: Pokemon " + String.Join(", ", retStr.ToArray()));
            
            return retStr.Count == 0;
        })();

        ow["AllFormsforAllPokemon"] = new Func<bool>(() =>
        {
            var retStr = new List<string>();
            var specialForms = new (string Name, ushort Species, byte Form)[]
            {
                ("UnownA", 201, 0), ("UnownB", 201, 1), ("UnownC", 201, 2), ("UnownD", 201, 3),
                ("UnownE", 201, 4), ("UnownF", 201, 5), ("UnownG", 201, 6), ("UnownH", 201, 7),
                ("UnownI", 201, 8), ("UnownJ", 201, 9), ("UnownK", 201, 10), ("UnownL", 201, 11),
                ("UnownM", 201, 12), ("UnownN", 201, 13), ("UnownO", 201, 14), ("UnownP", 201, 15),
                ("UnownQ", 201, 16), ("UnownR", 201, 17), ("UnownS", 201, 18), ("UnownT", 201, 19),
                ("UnownU", 201, 20), ("UnownV", 201, 21), ("UnownW", 201, 22), ("UnownX", 201, 23),
                ("UnownY", 201, 24), ("UnownZ", 201, 25), ("UnownExclamationMark", 201, 26),
                ("UnownQuestionMark", 201, 27), ("CastformNormal", 351, 0), ("CastformFire", 351, 1),
                ("CastformWater", 351, 2), ("CastformIce", 351, 3), ("DeoxysNormal", 386, 0),
                ("DeoxysAttack", 386, 1), ("DeoxysDefense", 386, 2), ("DeoxysSpeed", 386, 3),
                ("BurmyPlantCloak", 412, 0), ("BurmySandyCloak", 412, 1), ("BurmyTrashCloak", 412, 2),
                ("WormadamPlantCloak", 413, 0), ("WormadamSandyCloak", 413, 1), ("WormadamTrashCloak", 413, 2),
                ("CherrimOvercast", 421, 0), ("CherrimSunshine", 421, 1), ("ShellosWestSea", 422, 0),
                ("ShellosEastSea", 422, 1), ("GastrodonWestSea", 423, 0), ("GastrodonEastSea", 423, 1),
                ("RotomGhost", 479, 0), ("RotomHeat", 479, 1), ("RotomWash", 479, 2), ("RotomFrost", 479, 3),
                ("RotomFan", 479, 4), ("RotomMow", 479, 5), ("GiratinaAltered", 487, 0),
                ("GiratinaOrigin", 487, 1), ("ShayminLand", 492, 0), ("ShayminSky", 492, 1),
                ("BasculinBlueStripe", 550, 0), ("BasculinRedStripe", 550, 1), ("DarmanitanStandard", 555, 0),
                ("DarmanitanZen", 555, 1), ("DeerlingSpring", 585, 0), ("DeerlingSummer", 585, 1),
                ("DeerlingAutumn", 585, 2), ("DeerlingWinter", 585, 3), ("SawsbuckSpring", 586, 0),
                ("SawsbuckSummer", 586, 1), ("SawsbuckAutumn", 586, 2), ("SawsbuckWinter", 586, 3),
                ("TornadusIncarnate", 641, 0), ("TornadusTherian", 641, 1), ("ThundurusIncarnate", 642, 0),
                ("ThundurusTherian", 642, 1), ("LandorusIncarnate", 645, 0), ("LandorusTherian", 645, 1),
                ("KyuremNormal", 646, 0), ("KyuremWhite", 646, 1), ("KyuremBlack", 646, 2),
                ("KeldeoOrdinary", 647, 0), ("KeldeoResolute", 647, 1), ("MeloettaAria", 648, 0),
                ("MeloettaPirouette", 648, 1), ("GenesectNormal", 649, 0), ("GenesectDouse", 649, 1),
                ("GenesectShock", 649, 2), ("GenesectBurn", 649, 3), ("GenesectChill", 649, 4),
            };

            const int FORM_NON_SHINY = 0;
            const int FORM_SHINY = 1;
            for (ushort i = 1; i <= 649; i++)
            {
                var pi = this.sav.Personal[i];

                if (!pi.OnlyFemale && 
                    !this.sav.Zukan.GetSeen(i, MALE_GENDERLESS_NON_SHINY))
                {
                    retStr.Add($"#{i} (Male Non-Shiny)");
                    continue;
                }

                if (!(pi.OnlyMale || pi.Genderless) && 
                    !this.sav.Zukan.GetSeen(i, FEMALE_NON_SHINY))
                {
                    retStr.Add($"#{i} (Female Non-Shiny)");
                    continue;
                }

                if (!pi.OnlyFemale && 
                    !this.sav.Zukan.GetSeen(i, MALE_GENDERLESS_SHINY))
                {
                    retStr.Add($"#{i} (Male Shiny)");
                    continue;
                }

                if (!(pi.OnlyMale || pi.Genderless) && 
                    !this.sav.Zukan.GetSeen(i, FEMALE_SHINY))
                {
                    retStr.Add($"#{i} (Female Shiny)");
                    continue;
                }
            }

            for (int i = 0; i < specialForms.Length; i++)
            {
                var (name, species, form) = specialForms[i];
                int formIndex = GetDexFormIndexB2W2(species, this.sav.Personal[species].FormCount);
                if (formIndex < 0)
                    continue;

                formIndex += form;
                if (!this.sav.Zukan.GetFormFlag(formIndex, FORM_NON_SHINY))
                    retStr.Add($"{name} (Non-Shiny)");
                if (!this.sav.Zukan.GetFormFlag(formIndex, FORM_SHINY))
                    retStr.Add($"{name} (Shiny)");
            }

            if (retStr.Count > 0)
                this.incompleteHints.Add("Missing All Pokedex Forms: Pokemon " + String.Join(", ", retStr.ToArray()));
            
            return retStr.Count == 0;
        })();

        ow["AllForeignPokedexEntries"] = new Func<bool>(() =>
        {
            var languages = new List<string> { "JP", "EN", "FR", "IT", "GE", "SP", "KO" };
            var retStr = new List<string>();
            var ret = true;
            for (ushort i = 1; i <= 493; i++) // gen5 don't have language flags.
            {
                for (int j = 0; j < 6; j++)
                    if (!this.sav.Zukan.GetLanguageFlag(i, j))
                    {
                        retStr.Add("#" + i.ToString() + " (" + languages[j] + ")");
                        ret = false;
                    }
            }

            if (!ret)
                this.incompleteHints.Add("Missing Foreign Pokedex Entries: Pokemon " + String.Join(", ", retStr.ToArray()));
            return ret;
        })();

    }

    public bool HasSeenPkmForm(ushort species, byte form)
    {
        var pi = this.sav.Personal[species];

        var fc = pi.FormCount;
        int f = GetDexFormIndexB2W2(species, fc);

        const int NON_SHINY = 0;
        const int SHINY = 0;

        return this.sav.Zukan.GetFormFlag(f + form, NON_SHINY) || this.sav.Zukan.GetFormFlag(f + form, SHINY);
    }

    public bool HasOrSeenPkmBasedOnObjective2(ushort species, byte form)
    {
        return this.objective != 0 ? HasPkmForm(species, form) : HasSeenPkmForm(species, form);
    }
    public bool HasOrSeenPkmBasedOnObjectiveArceus(ushort species, byte form)
    {
        // Arceus forms are not in the pokdex in gen5
        return this.objective != 0 ? HasPkmForm(species, form) : HasSeenPkmForm(species, 0);
    }

    public void Generate_pwt()
    {
        var ow = new Dictionary<string, bool>();
        owned["pwt"] = ow;

        // Kinda bad. It doesn't track each variant of the challenge.
        var val = this.sav.PWT.GetFlags(0) == 65535 && this.sav.PWT.GetFlags(1) == 65535;
        ow["DriftveilSingle"] = val;
        ow["DriftveilDouble"] = val;
        ow["DriftveilTriple"] = val;
        ow["DriftveilRotation"] = val;
        ow["RentalSingle"] = val;
        ow["RentalDouble"] = val;
        ow["RentalTriple"] = val;
        ow["RentalRotation"] = val;
        ow["MixSingle"] = val;
        ow["MixDouble"] = val;
        ow["MixTriple"] = val;
        ow["MixRotation"] = val;
        ow["UnovaLeadersSingle"] = val;
        ow["UnovaLeadersDouble"] = val;
        ow["UnovaLeadersTriple"] = val;
        ow["UnovaLeadersRotation"] = val;
        ow["KantoLeadersSingle"] = val;
        ow["KantoLeadersDouble"] = val;
        ow["KantoLeadersTriple"] = val;
        ow["KantoLeadersRotation"] = val;
        ow["JohtoLeadersSingle"] = val;
        ow["JohtoLeadersDouble"] = val;
        ow["JohtoLeadersTriple"] = val;
        ow["JohtoLeadersRotation"] = val;
        ow["HoennLeadersSingle"] = val;
        ow["HoennLeadersDouble"] = val;
        ow["HoennLeadersTriple"] = val;
        ow["HoennLeadersRotation"] = val;
        ow["SinnohLeadersSingle"] = val;
        ow["SinnohLeadersDouble"] = val;
        ow["SinnohLeadersTriple"] = val;
        ow["SinnohLeadersRotation"] = val;
        ow["WorldLeadersSingle"] = val;
        ow["WorldLeadersDouble"] = val;
        ow["WorldLeadersTriple"] = val;
        ow["WorldLeadersRotation"] = val;
        ow["ChampionsSingle"] = val;
        ow["ChampionsDouble"] = val;
        ow["ChampionsTriple"] = val;
        ow["ChampionsRotation"] = val;
        ow["RentalMastersSingle"] = val;
        ow["RentalMastersDouble"] = val;
        ow["RentalMastersTriple"] = val;
        ow["RentalMastersRotation"] = val;
        ow["MixMastersSingle"] = val;
        ow["MixMastersDouble"] = val;
        ow["MixMastersTriple"] = val;
        ow["MixMastersRotation"] = val;
        ow["TypeExpertNormalSingle"] = val;
        ow["TypeExpertNormalDouble"] = val;
        ow["TypeExpertNormalTriple"] = val;
        ow["TypeExpertNormalRotation"] = val;
        ow["TypeExpertFightingSingle"] = val;
        ow["TypeExpertFightingDouble"] = val;
        ow["TypeExpertFightingTriple"] = val;
        ow["TypeExpertFightingRotation"] = val;
        ow["TypeExpertFlyingSingle"] = val;
        ow["TypeExpertFlyingDouble"] = val;
        ow["TypeExpertFlyingTriple"] = val;
        ow["TypeExpertFlyingRotation"] = val;
        ow["TypeExpertPoisonSingle"] = val;
        ow["TypeExpertPoisonDouble"] = val;
        ow["TypeExpertPoisonTriple"] = val;
        ow["TypeExpertPoisonRotation"] = val;
        ow["TypeExpertGroundSingle"] = val;
        ow["TypeExpertGroundDouble"] = val;
        ow["TypeExpertGroundTriple"] = val;
        ow["TypeExpertGroundRotation"] = val;
        ow["TypeExpertRockSingle"] = val;
        ow["TypeExpertRockDouble"] = val;
        ow["TypeExpertRockTriple"] = val;
        ow["TypeExpertRockRotation"] = val;
        ow["TypeExpertBugSingle"] = val;
        ow["TypeExpertBugDouble"] = val;
        ow["TypeExpertBugTriple"] = val;
        ow["TypeExpertBugRotation"] = val;
        ow["TypeExpertGhostSingle"] = val;
        ow["TypeExpertGhostDouble"] = val;
        ow["TypeExpertGhostTriple"] = val;
        ow["TypeExpertGhostRotation"] = val;
        ow["TypeExpertSteelSingle"] = val;
        ow["TypeExpertSteelDouble"] = val;
        ow["TypeExpertSteelTriple"] = val;
        ow["TypeExpertSteelRotation"] = val;
        ow["TypeExpertFireSingle"] = val;
        ow["TypeExpertFireDouble"] = val;
        ow["TypeExpertFireTriple"] = val;
        ow["TypeExpertFireRotation"] = val;
        ow["TypeExpertWaterSingle"] = val;
        ow["TypeExpertWaterDouble"] = val;
        ow["TypeExpertWaterTriple"] = val;
        ow["TypeExpertWaterRotation"] = val;
        ow["TypeExpertGrassSingle"] = val;
        ow["TypeExpertGrassDouble"] = val;
        ow["TypeExpertGrassTriple"] = val;
        ow["TypeExpertGrassRotation"] = val;
        ow["TypeExpertElectricSingle"] = val;
        ow["TypeExpertElectricDouble"] = val;
        ow["TypeExpertElectricTriple"] = val;
        ow["TypeExpertElectricRotation"] = val;
        ow["TypeExpertPsychicSingle"] = val;
        ow["TypeExpertPsychicDouble"] = val;
        ow["TypeExpertPsychicTriple"] = val;
        ow["TypeExpertPsychicRotation"] = val;
        ow["TypeExpertIceSingle"] = val;
        ow["TypeExpertIceDouble"] = val;
        ow["TypeExpertIceTriple"] = val;
        ow["TypeExpertIceRotation"] = val;
        ow["TypeExpertDragonSingle"] = val;
        ow["TypeExpertDragonDouble"] = val;
        ow["TypeExpertDragonTriple"] = val;
        ow["TypeExpertDragonRotation"] = val;
        ow["TypeExpertDarkSingle"] = val;
        ow["TypeExpertDarkDouble"] = val;
        ow["TypeExpertDarkTriple"] = val;
        ow["TypeExpertDarkRotation"] = val;
    }

    
    public void Generate_medal()
    {
        
    }


    public void Generate_itemInMap()
    {
        //TODO
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
        //TODO
        var ow = new Dictionary<string, bool>();
        owned["itemGift"] = ow;
        var list = new List<int> {};

        foreach (var id in list)
            ow[id.ToString()] = sav.GetEventFlag(id);
    }
    public bool has_rank_10_join_avenue_shop(int ShopType)
    {
        const int AVENUE_BLOCK_OFFSET_B2W2 = 0x23C00;
        const int OCCUPANT_OFFSET = 0xAAC;
        const int OCCUPANT_SIZE = 0xC4;
        const int SHOP_BYTES_OFFSET = 0xB4;
        const int SHOP_COUNT = 8;
        const int SHOP_LEVEL_10 = 9;

        for (int i = 0; i < SHOP_COUNT; i++)
        {
            int shopBytesOffset = AVENUE_BLOCK_OFFSET_B2W2 + OCCUPANT_OFFSET + (OCCUPANT_SIZE * i) + SHOP_BYTES_OFFSET;

            ushort shopBytes = (ushort)(sav.Data[shopBytesOffset] | (sav.Data[shopBytesOffset + 1] << 8));
            if (shopBytes == 0xFFFF)
                continue;

            int shopLevel = shopBytes % 0x50 == 0
                ? 9
                : (((shopBytes % 0x50) - 1) % 0xA);
            int shopType = shopLevel == 9
                ? (shopBytes - 1) % 0x50 / 0xA
                : shopBytes % 0x50 / 0xA;

            if (shopType == ShopType && shopLevel == SHOP_LEVEL_10)
                return true;
        }

        return false;
    }
    
    public void Generate_joinAvenue()
    {
        var ow = new Dictionary<string, bool>();
        owned["joinAvenue"] = ow;

        const int JOIN_AVENUE_BLK = 0x23C00;
        
        var lvl = ReadUInt16LittleEndian(sav.Data[(JOIN_AVENUE_BLK + 0x13CC)..]);

        ow["AvenueRank100"] = lvl == 100;

        ow["RaffleShopRank10"] = has_rank_10_join_avenue_shop(0);
        ow["BeautySalonRank10"] = has_rank_10_join_avenue_shop(1);
        ow["DojoRank10"] = has_rank_10_join_avenue_shop(4);
        ow["MarketRank10"] = has_rank_10_join_avenue_shop(2);
        ow["FlowerShopRank10"] = has_rank_10_join_avenue_shop(3);
        ow["AntiqueShopRank10"] = has_rank_10_join_avenue_shop(6);
        ow["CaféRank10"] = has_rank_10_join_avenue_shop(7);
        ow["NurseryRank10"] = has_rank_10_join_avenue_shop(5);
    }



    public void Generate_inGameTrade()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameTrade"] = ow;

        ow["PetililforCottonee"] = sav.GetEventFlag(0x0412);
        ow["EmolgaforGigalith"] = sav.GetEventFlag(0x0256);
        ow["MantineforTangrowth"] = sav.GetEventFlag(0x0405);
        ow["DittoforRotom"] = sav.GetEventFlag(0x0259);
        ow["ExcadrillforAmbipom"] = sav.GetEventFlag(0x0431);
        ow["HippowdonforAlakazam"] = sav.GetEventFlag(0x0432);

        var yancySpecies = new HashSet<int>();
        var curtisSpecies = new HashSet<int>();
        
        // Note: not ideal, because it forces the player to keep the pokemon.
        foreach (var pk in sav.GetAllPKM())
        {
            if (pk.DisplayTID == 10303 && pk.OriginalTrainerName == "Yancy")
                yancySpecies.Add(pk.Species);
            if (pk.DisplayTID == 54118 && pk.OriginalTrainerName == "Curtis")
                curtisSpecies.Add(pk.Species);
        }

        ow["AnyforMeowthYancymaleplayercharacteronly"] = yancySpecies.Contains(52);
        ow["AnyforWobbuffetYancymaleplayercharacteronly"] = yancySpecies.Contains(202);
        ow["AnyforRaltsYancymaleplayercharacteronly"] = yancySpecies.Contains(280);
        ow["AnyforShieldonYancymaleplayercharacteronly"] = yancySpecies.Contains(410);
        ow["AnyforRhyhornYancymaleplayercharacteronly"] = yancySpecies.Contains(111);
        ow["AnyforShellosYancymaleplayercharacteronly"] = yancySpecies.Contains(422);
        ow["AnyforMawileYancymaleplayercharacteronly"] = yancySpecies.Contains(303);
        ow["AnyforSpiritombYancymaleplayercharacteronly"] = yancySpecies.Contains(442);
        ow["AnyforSnorlaxYancymaleplayercharacteronly"] = yancySpecies.Contains(143);
        ow["AnyforTeddiursaYancymaleplayercharacteronly"] = yancySpecies.Contains(216);
        ow["AnyforSpindaYancymaleplayercharacteronly"] = yancySpecies.Contains(327);
        ow["AnyforTogepiYancymaleplayercharacteronly"] = yancySpecies.Contains(175);
        ow["AnyforMankeyCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(56);
        ow["AnyforWobbuffetCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(202);
        ow["AnyforRaltsCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(280);
        ow["AnyforCranidosCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(408);
        ow["AnyforRhyhornCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(111);
        ow["AnyforShellosCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(422);
        ow["AnyforSableyeCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(302);
        ow["AnyforSpiritombCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(442);
        ow["AnyforSnorlaxCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(143);
        ow["AnyforPhanpyCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(231);
        ow["AnyforSpindaCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(327);
        ow["AnyforTogepiCurtisfemaleplayercharacteronly"] = curtisSpecies.Contains(175);

    }
    public void Generate_inGameGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameGift"] = ow;

        ow["StarterPokemon"] = true;
        ow["Zorua"] = sav.GetEventFlag(0x0121);
        
        ow["Deerling"] = sav.GetEventFlag(0x01AA);
        ow["Eevee"] = sav.GetEventFlag(0x018A);
        ow["Magikarp"] = sav.GetEventFlag(0x00FA);
        ow["Happiny Egg"] = sav.GetEventFlag(0x01B5);
        ow["TirtougaorArchen"] = sav.GetEventFlag(0x01BD);
        // ow["Shiny Dratini"] = false; // White2
        ow["Shiny Gible"] = sav.GetEventFlag(0x0121);
    }

    public void Generate_pcWallPaper()
    {
        var ow = new Dictionary<string, bool>();
        owned["pcWallPaper"] = ow;

        //TODO
    }


    public string fmt(string name)
    {
        return Regex.Replace(name.Replace('é', 'e'), "[^A-Za-z0-9]", "");
    }

    public void Generate_battleSubway()
    {
        var ow = new Dictionary<string, bool>();
        owned["battleSubway"] = ow;

        ow[fmt("Single - 21 wins")] = this.sav.BattleSubway.SingleRecord >= 21;;
        ow[fmt("Double - 21 wins")] = this.sav.BattleSubway.DoubleRecord >= 21;
        ow[fmt("Multi - 21 wins")] = this.sav.BattleSubway.MultiNPCRecord >= 21;
        ow[fmt("Super Single - 49 wins")] = this.sav.BattleSubway.SuperSingleRecord >= 49;
        ow[fmt("Super Double - 49 wins")] = this.sav.BattleSubway.SuperDoubleRecord >= 49;
        ow[fmt("Super Multi - 49 wins")] = this.sav.BattleSubway.SuperMultiNPCRecord >= 49;
        ow[fmt("Wi-Fi")] = this.sav.BattleSubway.SuperMultiFriendsRecord >= 1;

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
        //ow["Moves"] = UnlockedNationalDex;
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
