using PKHeX.Core;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class CompletionValidator : Command
{
    static public CompletionValidatorX? LastValidatorX = null;

    [JSExport]
    static public bool Execute(byte[] savData, bool living)
    {
        var validator = new CompletionValidator();
        var savA = validator.SetSavA(savData);
        if (savA == null)
            return false;

        var validatorX =  validator.CreateValidatorX(savA, living);
        CompletionValidator.LastValidatorX = validatorX;

        validatorX.GenerateAll();

        return true;
    }

    CompletionValidatorX CreateValidatorX(SaveFile savA, bool living)
    {
        if (savA is SAV1)
            return new CompletionValidator1(this, (savA as SAV1)!, living);

        if (savA is SAV2)
            return new CompletionValidator2(this, (savA as SAV2)!, living);

        if (savA is SAV3)
            return new CompletionValidator3(this, (savA as SAV3)!, living);

        return new CompletionValidatorX(this, savA, living);
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
}
