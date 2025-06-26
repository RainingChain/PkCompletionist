using PKHeX.Core;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class CompletionValidator : Command
{
    static public CompletionValidatorX? LastValidatorX = null;

    [JSExport]
    static public bool Execute(byte[] savData, string VersionHint, int objective)
    {
        var validator = new CompletionValidator();
        var savA = validator.SetSavA(savData, VersionHint);
        if (savA == null)
            return false;

        var validatorX = validator.CreateValidatorX(savA, (Objective)objective);
        CompletionValidator.LastValidatorX = validatorX;

        validatorX.GenerateAll();

        return true;
    }

    CompletionValidatorX CreateValidatorX(SaveFile savA, Objective objective)
    {
        if (savA is SAV1)
            return new CompletionValidator1(this, (savA as SAV1)!, objective);

        if (savA is SAV2)
            return new CompletionValidator2(this, (savA as SAV2)!, objective);

        if (savA is SAV3E)
            return new CompletionValidator3(this, (savA as SAV3E)!, objective);

        if (savA is SAV4Pt)
            return new CompletionValidator4(this, (savA as SAV4Pt)!, objective);

        if (savA is SAV3_Pinball)
            return new CompletionValidator3_Pinball(this, (savA as SAV3_Pinball)!, objective);

        if (savA is SAV1_Pinball)
            return new CompletionValidator1_Pinball(this, (savA as SAV1_Pinball)!, objective);

        if (savA is SAV3_PmdRescueTeam)
            return new CompletionValidator3_PmdRescueTeam(this, (savA as SAV3_PmdRescueTeam)!, objective);

        if (savA is SAV4_PmdSky)
            return new CompletionValidator4_PmdSky(this, (savA as SAV4_PmdSky)!, objective);

        if (savA is SAV4_Ranger)
            return new CompletionValidator4_Ranger(this, (savA as SAV4_Ranger)!, objective);

        if (savA is SAV7_Shuffle)
            return new CompletionValidator7_Shuffle(this, (savA as SAV7_Shuffle)!, objective);

        return new CompletionValidatorX(this, savA, objective);
    }

    [JSExport]
    static public string[] GetLastObtainedStatus()
    {
        if (LastValidatorX == null)
            return System.Array.Empty<string>();

        string[] Res = new string[LastValidatorX.owned.Count * 3];

        int i = 0;
        foreach (var Info in LastValidatorX.owned)
        {
            Res[i] = Info.Key;
            List<string> obtained = new();
            List<string> notObtained = new();

            foreach (var Info2 in Info.Value)
            {
                if (Info2.Value)
                    obtained.Add(Info2.Key);
                else
                    notObtained.Add(Info2.Key);
            }
            Res[i + 1] = System.String.Join(',', obtained);
            Res[i + 2] = System.String.Join(',', notObtained);
            i += 3;
        }
        return Res;
    }

    [JSExport]
    static public string[] GetLastCompletionHints()
    {
        if (LastValidatorX == null)
            return System.Array.Empty<string>();
        return LastValidatorX.incompleteHints.ToArray();
    }

}
