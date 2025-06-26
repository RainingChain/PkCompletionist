
using PKHeX.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PkCompletionist.Core;

internal class CompletionValidator4_Ranger : CompletionValidatorX
{
    public CompletionValidator4_Ranger(Command command, SAV4_Ranger sav, Objective objective) : base(command, sav, objective)
    {
        this.sav = sav;

    }
    new SAV4_Ranger sav;

    public override void GenerateAll()
    {
        Generate_pokemon();
        Generate_mission();
        Generate_misc();
    }

    public override void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        for (int i = 0; i < SAV4_Ranger.MON_LIST.Count; i++)
        {
            ow[SAV4_Ranger.MON_LIST[i]] = sav.HasMon(SAV4_Ranger.MON_LIST[i]);
        }
    }

    public void Generate_mission()
    {
        var ow = new Dictionary<string, bool>();
        owned["mission"] = ow;

        ow["RookieMissionDontFearFailure"] = sav.HasMon("R-036"); // Taillow
        ow["Mission1EscorttheProfessor"] = sav.HasMon("R-004"); // Bellsprout
        ow["Mission2FallCityCaseLog"] = sav.HasMon("R-081"); // Machoke
        ow["Mission3GrimerOutbreak"] = sav.HasMon("R-088"); // Muk
        ow["Mission4WheresPolitoed"] = sav.HasMon("R-055"); // Politoed
        ow["Mission5ClearTheRockfalls"] = sav.HasMon("R-136"); // Camerupt
        ow["Mission6TheWaywardWanderer"] = sav.HasMon("R-046"); // Murkrow
        ow["Mission7InvestigateTheFactory"] = sav.HasMon("R-044"); // Scizor
        ow["Mission8AquamoleToTheNorth"] = sav.HasMon("R-179"); // Steelix
        ow["Mission9HideoutInfiltration"] = sav.HasMon("R-195"); // Tyranitar
        ow["Mission10FioreTemple"] = sav.HasMon("R-204"); // Suicune
        ow["SpecialMissionSearchtheSafraSea"] = sav.HasMon("R-208"); // Gyogre
        ow["SpecialMissionSummerlandRescueDuo"] = sav.HasMon("R-209"); // Groudon
        ow["SpecialMissionTheTemplesSinisterShadows"] = sav.HasMon("R-210"); // Rayquaza
        //ow["RangerNetMissionRecoverthePreciousEgg"] = false; //TODO
        ow["RangerNetMissionGainDeoxyssTrust"] = sav.HasMon("R-211"); //Deoxys
        ow["RangerNetMissionRescueCelebi"] = sav.HasMon("R-212"); // Celebi
        ow["RangerNetMissionFindMewTheMirage"] = sav.HasMon("R-213"); // Mew
    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;

        ow["50000PtsatCaptureArena"] = sav.HasMon("R-040"); // Fearow
        ow["5000PtsatGrasslandCaptureChallenge"] = sav.HasMon("R-042"); // Dodrio
        ow["5000PtsatMarineCaptureChallenge"] = sav.HasMon("R-123"); // Pelipper
        //ow["StylerMaxLevel"] = false; //TODO
    }
}
