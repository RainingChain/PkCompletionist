using PKHeX.Core;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class PkSorter : Command
{
    [JSExport]
    public static bool Execute(byte[] savA, string VersionHint)
    {
        var sorter = new PkSorter();
        if (sorter.SetSavA(savA, VersionHint) == null)
            return false;

        List<PKM> boxes = new(sorter.savA!.BoxData);
        boxes.Sort((pkm1, pkm2) =>
        {
            if (pkm1.Species == 0)
                return 1;
            if (pkm2.Species == 0)
                return -1;
            if (pkm1.Species != pkm2.Species)
                return pkm1.Species - pkm2.Species;
            return pkm1.Form - pkm2.Form;
        });

        sorter.savA!.BoxData = boxes;
        return true;
    }
}
