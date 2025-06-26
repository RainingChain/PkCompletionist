using PKHeX.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PkCompletionist.Core;

internal class CompletionValidator7_Shuffle : CompletionValidatorX
{
    public CompletionValidator7_Shuffle(Command command, SAV7_Shuffle sav, Objective objective) : base(command, sav, objective)
    {
        this.sav = sav;

    }
    new SAV7_Shuffle sav;

    public override void GenerateAll()
    {
        Generate_pokemon();
        Generate_missionCard();
        Generate_misc();
    }

    public override void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        var unused = new List<int> { 201, 202, 204, 206, 207, 208, 210, 211, 212, 213, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 789, 793, 794, 802, 813, 825, 828, 829, 838, 859, 862, 912, 913 };
        for (int i = 1; i <= 1022; i++) 
        {
            if (unused.Contains(i))
                continue;
            ow[i.ToString()] = sav.HasMon(i);
        }
    }

    public void Generate_missionCard()
    {
        var ow = new Dictionary<string, bool>();
        owned["missionCard"] = ow;
    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;
    }
}
