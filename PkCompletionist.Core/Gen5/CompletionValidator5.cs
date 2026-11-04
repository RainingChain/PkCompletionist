using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Buffers.Binary.BinaryPrimitives;
namespace PkCompletionist.Core;

/*
not working

Yes	
Emolga for Gigalith		Route 7
	Yes	
Mantine for Tangrowth		Humilau City
	Yes	
Ditto for Rotom		Route 15
	Yes	
Excadrill for Ambipom		Accumula Town
	Yes	
Hippowdon for Alakazam		Accumula Town

--------------------
to check in old save

yancy trades



*/

internal class CompletionValidator5 : CompletionValidatorX
{
    public CompletionValidator5(Command command, SAV5B2W2 sav, Objective objective) : base(command, sav, objective)
    {
        this.sav = sav;

        // a.list.filter(a => a.obtainable === -1).map(a => +a.id).toString()
        this.unobtainableItems = new List<int>() { 5,16,113,114,115,120,121,122,123,124,125,126,127,128,129,130,131,132,133,260,261,262,263,264,426,427,428,429,430,431,432,433,434,435,436,438,439,440,441,443,444,445,446,448,449,451,452,454,455,456,457,459,460,461,462,463,464,467,468,469,470,472,473,474,475,476,477,478,479,480,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,497,498,499,500,501,502,503,505,506,507,508,509,510,511,512,513,514,515,516,517,518,519,520,521,522,523,524,525,526,527,528,529,530,531,532,533,534,535,536,574,579,592,593,594,595,596,597,598,599,600,601,602,603,604,605,606,607,608,609,610,611,612,613,614,615,616,622,623,624,625 };
    }

    new SAV5B2W2 sav;

    public override void GenerateAll()
    {
        base.GenerateAll(); // pokemon, item

        Generate_pokemonForm();
        Generate_inGameTrade();
        Generate_inGameGift();

        Generate_itemInMap(); 
        Generate_itemGift();
        Generate_battle();

        Generate_joinAvenue();

        Generate_ribbon();
        Generate_battleSubway();
        Generate_pwt();
        Generate_medal();

        Generate_trainerStar();
        Generate_easyChatSystemWord();
        Generate_pokestarStudios();
        Generate_props();
        Generate_pokemonMusical();
        Generate_funfest();
        Generate_pcWallpaper();
        Generate_uiSkin();

        Generate_record();
        Generate_misc();
    }

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
                
                if (!this.sav.Zukan.GetSeen(i, 0, false) && 
                    !this.sav.Zukan.GetSeen(i, 0, true))
                {
                    retStr.Add($"#{i} (Male)");
                    continue;
                }
                if (!this.sav.Zukan.GetSeen(i, 1, false) && 
                    !this.sav.Zukan.GetSeen(i, 1, true))
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

                if (!this.sav.Zukan.GetSeen(i, 0, true) && 
                    !this.sav.Zukan.GetSeen(i, 1, true))
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
                    !this.sav.Zukan.GetSeen(i, 0, true))
                {
                    retStr.Add($"#{i} (Male)");
                    continue;
                }

                if (!(pi.OnlyMale || pi.Genderless) && 
                    !this.sav.Zukan.GetSeen(i, 1, true))
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

            for (ushort i = 1; i <= 649; i++)
            {
                var pi = this.sav.Personal[i];

                if (!pi.OnlyFemale && 
                    !this.sav.Zukan.GetSeen(i, 0, false))
                {
                    retStr.Add($"#{i} (Male Non-Shiny)");
                    continue;
                }

                if (!(pi.OnlyMale || pi.Genderless) && 
                    !this.sav.Zukan.GetSeen(i, 1, false))
                {
                    retStr.Add($"#{i} (Female Non-Shiny)");
                    continue;
                }

                if (!pi.OnlyFemale && 
                    !this.sav.Zukan.GetSeen(i, 0, true))
                {
                    retStr.Add($"#{i} (Male Shiny)");
                    continue;
                }

                if (!(pi.OnlyMale || pi.Genderless) && 
                    !this.sav.Zukan.GetSeen(i, 1, true))
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
                if (!this.sav.Zukan.GetFormSeen(formIndex, false))
                    retStr.Add($"{name} (Non-Shiny)");
                if (!this.sav.Zukan.GetFormSeen(formIndex, true))
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
        short formIndex = (short)GetDexFormIndexB2W2(species, this.sav.Personal[species].FormCount);

        return this.sav.Zukan.GetFormSeen(formIndex + form, false) || 
                this.sav.Zukan.GetFormSeen(formIndex + form, true);
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

        ow["DriftveilSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Driftveil) >= 1;
        ow["DriftveilDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Driftveil) >= 2;
        ow["DriftveilTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Driftveil) >= 3;
        ow["DriftveilRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Driftveil) >= 4;
        ow["RentalSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Rental) >= 1;
        ow["RentalDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Rental) >= 2;
        ow["RentalTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Rental) >= 3;
        ow["RentalRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Rental) >= 4;
        ow["MixSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Mix) >= 1;
        ow["MixDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Mix) >= 2;
        ow["MixTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Mix) >= 3;
        ow["MixRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Mix) >= 4;
        ow["UnovaLeadersSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Unova) >= 1;
        ow["UnovaLeadersDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Unova) >= 2;
        ow["UnovaLeadersTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Unova) >= 3;
        ow["UnovaLeadersRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Unova) >= 4;
        ow["KantoLeadersSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Kanto) >= 1;
        ow["KantoLeadersDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Kanto) >= 2;
        ow["KantoLeadersTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Kanto) >= 3;
        ow["KantoLeadersRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Kanto) >= 4;
        ow["JohtoLeadersSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Johto) >= 1;
        ow["JohtoLeadersDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Johto) >= 2;
        ow["JohtoLeadersTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Johto) >= 3;
        ow["JohtoLeadersRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Johto) >= 4;
        ow["HoennLeadersSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Hoenn) >= 1;
        ow["HoennLeadersDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Hoenn) >= 2;
        ow["HoennLeadersTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Hoenn) >= 3;
        ow["HoennLeadersRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Hoenn) >= 4;
        ow["SinnohLeadersSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Sinnoh) >= 1;
        ow["SinnohLeadersDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Sinnoh) >= 2;
        ow["SinnohLeadersTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Sinnoh) >= 3;
        ow["SinnohLeadersRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Sinnoh) >= 4;
        ow["WorldLeadersSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.World) >= 1;
        ow["WorldLeadersDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.World) >= 2;
        ow["WorldLeadersTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.World) >= 3;
        ow["WorldLeadersRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.World) >= 4;
        ow["ChampionsSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Champion) >= 1;
        ow["ChampionsDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Champion) >= 2;
        ow["ChampionsTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Champion) >= 3;
        ow["ChampionsRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Champion) >= 4;
        ow["RentalMastersSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.RentalMaster) >= 1;
        ow["RentalMastersDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.RentalMaster) >= 2;
        ow["RentalMastersTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.RentalMaster) >= 3;
        ow["RentalMastersRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.RentalMaster) >= 4;
        ow["MixMastersSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.MixMaster) >= 1;
        ow["MixMastersDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.MixMaster) >= 2;
        ow["MixMastersTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.MixMaster) >= 3;
        ow["MixMastersRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.MixMaster) >= 4;
        ow["TypeExpertNormalSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Normal) >= 1;
        ow["TypeExpertNormalDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Normal) >= 2;
        ow["TypeExpertNormalTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Normal) >= 3;
        ow["TypeExpertNormalRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Normal) >= 4;
        ow["TypeExpertFightingSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Fighting) >= 1;
        ow["TypeExpertFightingDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Fighting) >= 2;
        ow["TypeExpertFightingTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Fighting) >= 3;
        ow["TypeExpertFightingRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Fighting) >= 4;
        ow["TypeExpertFlyingSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Flying) >= 1;
        ow["TypeExpertFlyingDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Flying) >= 2;
        ow["TypeExpertFlyingTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Flying) >= 3;
        ow["TypeExpertFlyingRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Flying) >= 4;
        ow["TypeExpertPoisonSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Poison) >= 1;
        ow["TypeExpertPoisonDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Poison) >= 2;
        ow["TypeExpertPoisonTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Poison) >= 3;
        ow["TypeExpertPoisonRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Poison) >= 4;
        ow["TypeExpertGroundSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Ground) >= 1;
        ow["TypeExpertGroundDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Ground) >= 2;
        ow["TypeExpertGroundTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Ground) >= 3;
        ow["TypeExpertGroundRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Ground) >= 4;
        ow["TypeExpertRockSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Rock) >= 1;
        ow["TypeExpertRockDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Rock) >= 2;
        ow["TypeExpertRockTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Rock) >= 3;
        ow["TypeExpertRockRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Rock) >= 4;
        ow["TypeExpertBugSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Bug) >= 1;
        ow["TypeExpertBugDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Bug) >= 2;
        ow["TypeExpertBugTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Bug) >= 3;
        ow["TypeExpertBugRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Bug) >= 4;
        ow["TypeExpertGhostSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Ghost) >= 1;
        ow["TypeExpertGhostDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Ghost) >= 2;
        ow["TypeExpertGhostTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Ghost) >= 3;
        ow["TypeExpertGhostRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Ghost) >= 4;
        ow["TypeExpertSteelSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Steel) >= 1;
        ow["TypeExpertSteelDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Steel) >= 2;
        ow["TypeExpertSteelTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Steel) >= 3;
        ow["TypeExpertSteelRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Steel) >= 4;
        ow["TypeExpertFireSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Fire) >= 1;
        ow["TypeExpertFireDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Fire) >= 2;
        ow["TypeExpertFireTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Fire) >= 3;
        ow["TypeExpertFireRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Fire) >= 4;
        ow["TypeExpertWaterSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Water) >= 1;
        ow["TypeExpertWaterDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Water) >= 2;
        ow["TypeExpertWaterTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Water) >= 3;
        ow["TypeExpertWaterRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Water) >= 4;
        ow["TypeExpertGrassSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Grass) >= 1;
        ow["TypeExpertGrassDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Grass) >= 2;
        ow["TypeExpertGrassTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Grass) >= 3;
        ow["TypeExpertGrassRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Grass) >= 4;
        ow["TypeExpertElectricSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Electric) >= 1;
        ow["TypeExpertElectricDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Electric) >= 2;
        ow["TypeExpertElectricTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Electric) >= 3;
        ow["TypeExpertElectricRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Electric) >= 4;
        ow["TypeExpertPsychicSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Psychic) >= 1;
        ow["TypeExpertPsychicDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Psychic) >= 2;
        ow["TypeExpertPsychicTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Psychic) >= 3;
        ow["TypeExpertPsychicRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Psychic) >= 4;
        ow["TypeExpertIceSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Ice) >= 1;
        ow["TypeExpertIceDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Ice) >= 2;
        ow["TypeExpertIceTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Ice) >= 3;
        ow["TypeExpertIceRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Ice) >= 4;
        ow["TypeExpertDragonSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Dragon) >= 1;
        ow["TypeExpertDragonDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Dragon) >= 2;
        ow["TypeExpertDragonTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Dragon) >= 3;
        ow["TypeExpertDragonRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Dragon) >= 4;
        ow["TypeExpertDarkSingle"] = sav.PWT.GetPWTRecord(PWTRecordID.Dark) >= 1;
        ow["TypeExpertDarkDouble"] = sav.PWT.GetPWTRecord(PWTRecordID.Dark) >= 2;
        ow["TypeExpertDarkTriple"] = sav.PWT.GetPWTRecord(PWTRecordID.Dark) >= 3;
        ow["TypeExpertDarkRotation"] = sav.PWT.GetPWTRecord(PWTRecordID.Dark) >= 4;
    }

    
    public void Generate_medal()
    {
        var ow = new Dictionary<string, bool>();
        owned["medal"] = ow;

        var medals = MedalList5.GetMedals(this.sav.Medals.Data.ToArray());

        for (int i = 0; i < medals.Length; i++)
            ow[i.ToString()] = medals[i].IsObtained;        
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
            var val = sav.EventWork.GetEventFlag(id);
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
            ow[id.ToString()] = sav.EventWork.GetEventFlag(id);
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
        // wb_flag_define.h
        var ow = new Dictionary<string, bool>();
        owned["inGameTrade"] = ow;

        ow["PetililforCottonee"] = sav.EventWork.GetEventFlag(412);
        ow["EmolgaforGigalith"] = sav.EventWork.GetEventFlag(256);
        ow["MantineforTangrowth"] = sav.EventWork.GetEventFlag(405);
        ow["DittoforRotom"] = sav.EventWork.GetEventFlag(259);
        ow["ExcadrillforAmbipom"] = sav.EventWork.GetEventFlag(431);
        ow["HippowdonforAlakazam"] = sav.EventWork.GetEventFlag(432);

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

        ow["AnyforMeowthYancy"] = yancySpecies.Contains(52);
        ow["AnyforWobbuffetYancy"] = yancySpecies.Contains(202);
        ow["AnyforRaltsYancy"] = yancySpecies.Contains(280);
        ow["AnyforShieldonYancy"] = yancySpecies.Contains(410);
        ow["AnyforRhyhornYancy"] = yancySpecies.Contains(111);
        ow["AnyforShellosYancy"] = yancySpecies.Contains(422);
        ow["AnyforMawileYancy"] = yancySpecies.Contains(303);
        ow["AnyforSpiritombYancy"] = yancySpecies.Contains(442);
        ow["AnyforSnorlaxYancy"] = yancySpecies.Contains(143);
        ow["AnyforTeddiursaYancy"] = yancySpecies.Contains(216);
        ow["AnyforSpindaYancy"] = yancySpecies.Contains(327);
        ow["AnyforTogepiYancy"] = yancySpecies.Contains(175);
        ow["AnyforMankeyCurtis"] = curtisSpecies.Contains(56);
        ow["AnyforWobbuffetCurtis"] = curtisSpecies.Contains(202);
        ow["AnyforRaltsCurtis"] = curtisSpecies.Contains(280);
        ow["AnyforCranidosCurtis"] = curtisSpecies.Contains(408);
        ow["AnyforRhyhornCurtis"] = curtisSpecies.Contains(111);
        ow["AnyforShellosCurtis"] = curtisSpecies.Contains(422);
        ow["AnyforSableyeCurtis"] = curtisSpecies.Contains(302);
        ow["AnyforSpiritombCurtis"] = curtisSpecies.Contains(442);
        ow["AnyforSnorlaxCurtis"] = curtisSpecies.Contains(143);
        ow["AnyforPhanpyCurtis"] = curtisSpecies.Contains(231);
        ow["AnyforSpindaCurtis"] = curtisSpecies.Contains(327);
        ow["AnyforTogepiCurtis"] = curtisSpecies.Contains(175);

    }
    public void Generate_inGameGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameGift"] = ow;

        ow["StarterPokemon"] = true;
        ow["Zorua"] = sav.EventWork.GetEventFlag(0x0121);
        
        ow["Deerling"] = sav.EventWork.GetEventFlag(0x01AA);
        ow["Eevee"] = sav.EventWork.GetEventFlag(0x018A);
        ow["Magikarp"] = sav.EventWork.GetEventFlag(0x00FA);
        ow["Happiny Egg"] = sav.EventWork.GetEventFlag(0x01B5);
        ow["TirtougaorArchen"] = sav.EventWork.GetEventFlag(0x01BD);
        // ow["Shiny Dratini"] = false; // White2
        ow["Shiny Gible"] = sav.EventWork.GetEventFlag(0x0121);
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
        return sav.Records.GetRecord32(2) > 0;
    }


    public void Generate_trainerStar()
    {
        var ow = new Dictionary<string, bool>();
        owned["trainerStar"] = ow;

        ow["HallofFame"] = EnteredHallOfFame();

        var NationalPokedex = () =>
        {
            var exclude = new HashSet<ushort> {151, 249,250,251,385,386,489,490,491,492,493, 494, 647, 648, 649};
            for (ushort i = 0; i <= sav.MaxSpeciesID; i++)
            {
                if (exclude.Contains(i))
                    continue;
                if (!sav.Zukan.GetCaught(i))
                    return false;
            }
            return true;
        };    
        ow["NationalPokedex"] = NationalPokedex();
        
        var Pokestar = () => {
            for (int i = 0; i <= 39; i++) {
                if (!HasCompletedPokestar(i))
                    return false;
            }
            return true;
        };    
        ow[fmt("Pokéstar Studios")] = Pokestar();

        ow[fmt("Champions Tournament")] = sav.PWT.GetPWTRecord(PWTRecordID.Champion) >= 1;
        
        ow[fmt("Entralink")] = this.sav.Entralink.BlackCityLevel >= 30 && this.sav.Entralink.WhiteForestLevel >= 30;
    }
    

    enum PokestarMovieEnding {
        Normal,
        Bad,
        Strange,
    }

    private bool GetBit(int byte_offset, int bit_index)
    {
        int offset = bit_index / 8;
        int bit_index_in_byte = bit_index % 8;
        return ((sav.Data[byte_offset + offset] >> bit_index_in_byte) & 1) != 0;
    }

    bool HasPokestarEnding(int scenarioIdx, PokestarMovieEnding ending) {
        const int success_flag_offset = 0x10;
        const int bad_flag_offset = 0x2C4;
        const int weird_flag_offset = 0x2B4;
        int flagOffsetToUse = ending switch
        {
            PokestarMovieEnding.Normal => success_flag_offset,
            PokestarMovieEnding.Bad => bad_flag_offset,
            PokestarMovieEnding.Strange => weird_flag_offset,
            _ => success_flag_offset,
        };
       return GetBit(SAV5B2W2.PokestarOffset + flagOffsetToUse, scenarioIdx);        
    }

    bool HasCompletedPokestar(int scenarioIdx) {
        return HasPokestarEnding(scenarioIdx, PokestarMovieEnding.Normal)
             || HasPokestarEnding(scenarioIdx, PokestarMovieEnding.Bad) 
             || HasPokestarEnding(scenarioIdx, PokestarMovieEnding.Strange);
    }

    void Generate_pokestarStudios(){
        var ow = new Dictionary<string, bool>();
        owned["pokestarStudios"] = ow;

        for (int scenarioIdx = 0; scenarioIdx <= 39; scenarioIdx++)
        {
            ow[$"{scenarioIdx}N"] = HasPokestarEnding(scenarioIdx, PokestarMovieEnding.Normal);
            ow[$"{scenarioIdx}B"] = HasPokestarEnding(scenarioIdx, PokestarMovieEnding.Bad);
            ow[$"{scenarioIdx}S"] = HasPokestarEnding(scenarioIdx, PokestarMovieEnding.Strange);
        }
    }

    public void Generate_record()
    {
        var ow = new Dictionary<string, bool>();
        owned["record"] = ow;

        for (int i = 0; i <= 53; i++)
            ow[i.ToString()] = sav.Records.GetRecord32(i) > 0;

        for (int i = 0; i <= 60; i++)
            ow["s" + i.ToString()] = sav.Records.GetRecord16(i) > 0;
    }

    public void Generate_props()
    {
        var ow = new Dictionary<string, bool>();
        owned["props"] = ow;

        for (int i = 0; i <= 99; i++)
            ow[i.ToString()] = sav.Musical.GetHasProp(i);
    }

    public void Generate_uiSkin()
    {
        var ow = new Dictionary<string, bool>();
        owned["uiSkin"] = ow;

        // can't be tracked easily. in the save file, its a bitmap image.
    }


// Musical5GetHasProp

    public void Generate_easyChatSystemWord()
    {
        //TODO in pokecompletion
        var ow = new Dictionary<string, bool>();
        owned["easyChatSystemWord"] = ow;

        //this.sav1.Data[0x1DC00] is address for bonjour
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
            var got = sav.EventWork.GetEventFlag(0x550 + i);
            if (!got && untrackable.Contains(i))
                continue;
            ow[i.ToString()] = got;
        }*/

    }

    public void Generate_pokemonMusical()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemonMusical"] = ow;

        // not trackable
    }

    public void Generate_funfest()
    {
        var ow = new Dictionary<string, bool>();
        owned["funfest"] = ow;

        // TODO
    }

    public void Generate_pcWallpaper()
    {
        var ow = new Dictionary<string, bool>();
        owned["pcWallpaper"] = ow;

        var autoGot = new List<string> {"Forest", "City", "Desert", "Savanna", "Crag", "Volcano", "Snow", "Cave", "Beach", "Seafloor", "River", "Sky", "Checks", "Poké Center", "Machine", "Simple"};

        foreach (var pcWallpaper in autoGot){
            ow[fmt(pcWallpaper)] = true;
        }

        var onHallOfFame = new List<string> {"Monochrome", "Team Plasma", "Movie", "PWT"};
        foreach (var pcWallpaper in onHallOfFame){
            ow[fmt(pcWallpaper)] = EnteredHallOfFame();
        }
        
        var onPk = new List<string> {"Black Kyurem", "White Kyurem", "Reshiram", "Zekrom", };
        var zekKyu = HasPkm(644) && HasPkm(646);
        foreach (var pcWallpaper in onPk){
            ow[fmt(pcWallpaper)] = zekKyu;
        }
    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;
        //TODO

        // Misc.PokeTransferMinigameScore
    }

}
