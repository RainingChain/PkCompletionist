using PKHeX.Core;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class SavDescriptor : Command
{
    [JSExport]
    public static string[] Execute(byte[] savA)
    {
        var descriptor = new SavDescriptor();
        var sav = descriptor.SetSavA(savA);
        if (sav == null)
            return new string[] { "","",""};

        var desc = $"{sav.OT} ({sav.Version.ToString()}) - {sav.PlayTimeString}";
        return new string[] { sav.Version.ToString(), sav.Language.ToString(), desc };
    }
}
