using PKHeX.Core;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class PkMerger : Command
{
    [JSExport]
    public static bool Execute(byte[] savA, byte[] savB, string VersionHint)
    {
        var sorter = new PkMerger();
        if (sorter.SetSavA(savA, VersionHint) == null)
            return false;

        if (sorter.SetSavB(savB, "") == null)
            return false;

        List<PKM> boxes = new(sorter.savA!.BoxData);
        HashSet<(ushort Species, byte Form)> owned = new();
        foreach (var pkm in sorter.savA.GetAllPKM())
        {
            if (pkm.Species != 0)
                owned.Add((pkm.Species, pkm.Form));
        }

        int emptySlot = 0;
        foreach (var pkm in sorter.savB!.GetAllPKM())
        {
            if (pkm.Species == 0 || owned.Contains((pkm.Species, pkm.Form)))
                continue;

            while (emptySlot < boxes.Count && boxes[emptySlot].Species != 0)
                emptySlot++;
            if (emptySlot >= boxes.Count)
                break;

            var converted = EntityConverter.ConvertToType(pkm, sorter.savA.PKMType, out _);
            if (converted == null)
                continue;

            boxes[emptySlot++] = converted;
            owned.Add((converted.Species, converted.Form));
        }

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
