
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

    public bool HasMon(string name)
    {
        var idx = monList.IndexOf(name);
        if (idx == -1)
            return false;
        return this.sav.Data[0x896D + idx] == 2;
    }

    private static List<string>  monList = new List<string> { "R-001", "R-002", "R-003", "R-140", "R-141", "R-142", "R-076", "R-077", "R-078", "R-025", "R-101", "R-102", "R-039", "R-040", "R-151", "R-152", "R-023", "R-024", "R-100", "R-058", "R-059", "R-126", "R-127", "R-128", "R-050", "R-051", "R-153", "R-061", "R-062", "R-099", "R-121", "R-149", "R-150", "R-144", "R-145", "R-052", "R-053", "R-054", "R-029", "R-080", "R-081", "R-082", "R-004", "R-005", "R-006", "R-064", "R-065", "R-066", "R-018", "R-014", "R-056", "R-057", "R-041", "R-042", "R-087", "R-088", "R-094", "R-095", "R-096", "R-090", "R-091", "R-070", "R-079", "R-089", "R-063", "R-069", "R-201", "R-107", "R-108", "R-110", "R-111", "R-112", "R-113", "R-092", "R-043", "R-168", "R-188", "R-137", "R-093", "R-049", "R-124", "R-125", "R-106", "R-183", "R-185", "R-184", "R-103", "R-198", "R-105", "R-038", "R-213", "R-007", "R-008", "R-009", "R-019", "R-020", "R-021", "R-073", "R-074", "R-075", "R-157", "R-158", "R-060", "R-022", "R-055", "R-010", "R-186", "R-187", "R-046", "R-170", "R-160", "R-179", "R-097", "R-044", "R-148", "R-199", "R-138", "R-139", "R-172", "R-173", "R-114", "R-115", "R-116", "R-045", "R-161", "R-109", "R-034", "R-035", "R-203", "R-202", "R-204", "R-193", "R-194", "R-195", "R-212", "R-129", "R-130", "R-131", "R-015", "R-016", "R-017", "R-011", "R-012", "R-013", "R-047", "R-048", "R-154", "R-155", "R-156", "R-132", "R-133", "R-134", "R-180", "R-181", "R-182", "R-036", "R-037", "R-122", "R-123", "R-026", "R-027", "R-028", "R-162", "R-163", "R-164", "R-031", "R-032", "R-033", "R-176", "R-177", "R-178", "R-083", "R-084", "R-098", "R-085", "R-086", "R-146", "R-147", "R-067", "R-068", "R-189", "R-117", "R-118", "R-119", "R-135", "R-136", "R-143", "R-030", "R-159", "R-196", "R-197", "R-169", "R-071", "R-072", "R-104", "R-171", "R-200", "R-174", "R-175", "R-120", "R-165", "R-166", "R-167", "R-190", "R-191", "R-192", "R-205", "R-206", "R-207", "R-208", "R-209", "R-210", "R-211" };


    public override void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        for (int i = 0; i < monList.Count; i++)
        {
            ow[monList[i]] = HasMon(monList[i]);
        }
    }

    public void Generate_mission()
    {
        var ow = new Dictionary<string, bool>();
        owned["mission"] = ow;

        ow["RookieMissionDontFearFailure"] = HasMon("R-036"); // Taillow
        ow["Mission1EscorttheProfessor"] = HasMon("R-004"); // Bellsprout
        ow["Mission2FallCityCaseLog"] = HasMon("R-081"); // Machoke
        ow["Mission3GrimerOutbreak"] = HasMon("R-088"); // Muk
        ow["Mission4WheresPolitoed"] = HasMon("R-055"); // Politoed
        ow["Mission5ClearTheRockfalls"] = HasMon("R-136"); // Camerupt
        ow["Mission6TheWaywardWanderer"] = HasMon("R-046"); // Murkrow
        ow["Mission7InvestigateTheFactory"] = HasMon("R-044"); // Scizor
        ow["Mission8AquamoleToTheNorth"] = HasMon("R-179"); // Steelix
        ow["Mission9HideoutInfiltration"] = HasMon("R-195"); // Tyranitar
        ow["Mission10FioreTemple"] = HasMon("R-204"); // Suicune
        ow["SpecialMissionSearchtheSafraSea"] = HasMon("R-208"); // Gyogre
        ow["SpecialMissionSummerlandRescueDuo"] = HasMon("R-209"); // Groudon
        ow["SpecialMissionTheTemplesSinisterShadows"] = HasMon("R-210"); // Rayquaza
        //ow["RangerNetMissionRecoverthePreciousEgg"] = false; //TODO
        ow["RangerNetMissionGainDeoxyssTrust"] = HasMon("R-211"); //Deoxys
        ow["RangerNetMissionRescueCelebi"] = HasMon("R-212"); // Celebi
        ow["RangerNetMissionFindMewTheMirage"] = HasMon("R-213"); // Mew
    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;

        ow["50000PtsatCaptureArena"] = HasMon("R-040"); // Fearow
        ow["5000PtsatGrasslandCaptureChallenge"] = HasMon("R-042"); // Dodrio
        ow["5000PtsatMarineCaptureChallenge"] = HasMon("R-123"); // Pelipper
        //ow["StylerMaxLevel"] = false; //TODO
    }
}
