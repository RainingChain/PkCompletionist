using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class FlagAnalyzer : Command
{
    List<int> changeRanges = new List<int>();

    [JSExport]
    public static bool Execute(byte[] savA, byte[] savB, string changeRangesStr)
    {
        List<int> changeRanges = new List<int>();
        foreach (var val in changeRangesStr.Split(','))
        {
            if (val == null)
                continue;
            changeRanges.Add(int.Parse(val));
        }

        var simul = new FlagAnalyzer(changeRanges);
        if (simul.SetSavA(savA) == null)
            return false;
        if (simul.SetSavB(savB) == null)
            return false;
        return simul.Execute();
    }

    public FlagAnalyzer(List<int> changeRanges) 
    {
        this.changeRanges = changeRanges;
    }

    public bool Execute()
    {
        if (savA is SAV4)
            return Execute4();

        if (savA == null || savB == null)
            return false;

        byte[] savAData = savA.Data;
        byte[] savBData = savB.Data;

        if (savAData.Length != savBData.Length)
        {
            AddError("this.sav1.Data.Length != this.sav2.Data.Length");
            return false;
        }

        // ignore play time, checksum, for US version only
        List<int> toIgnore = new();
        if (savA.Version == GameVersion.C)
            toIgnore = new() { 0x1252, 0x1253, 0x1254, 0x1255, 0x1F0D, 0x1F0E, 0x2052, 0x2053, 0x2054, 0x2055, 0x2D0D, 0x2D0E };


        List<int> diffIdx = new List<int>();
        for (var i = 0; i < savAData.Length; i++)
        {
            if (toIgnore.IndexOf(i) != -1)
                continue;

            if (savAData[i] != savBData[i])
                diffIdx.Add(i);
        }
        AddMessage($"Change count + 1: {diffIdx.Count + 1}");

        System.Console.WriteLine(string.Join(", ", diffIdx));

        for (var i = changeRanges[0]; i < changeRanges[1] && i < diffIdx.Count; i++)
        {
            int idx = diffIdx[i];

            //if (changeRangeEnd - changeRangeStart < 15)
                AddMessage($"this.sav1.Data[0x{idx:X}] = 0x{savBData[idx]:X} (old=0x{savAData[idx]:X})");

            savAData[idx] = savBData[idx];
        }

       
        this.savA.SetData(savAData, 0);

        if (this.savA is SAV3)
            ((SAV3)this.savA).WriteBothSaveSlots(savAData);

        return true;
    }

    public void OverwriteBytes(Span<byte> spanA, Span<byte> spanB, int changeRangeStart, int changeRangeEnd, List<int> toIgnore)
    {
        List<int> diffIdx = new List<int>();
        for (var i = 0; i < spanA.Length; i++)
        {
            if (toIgnore.IndexOf(i) != -1)
                continue;

            if (spanA[i] != spanB[i])
                diffIdx.Add(i);
        }
        AddMessage($"Change count + 1: {diffIdx.Count + 1}");

        System.Console.WriteLine(string.Join(", ", diffIdx));

        for (var i = changeRangeStart; i < changeRangeEnd && i < diffIdx.Count; i++)
        {
            int idx = diffIdx[i];

            //if (changeRangeEnd - changeRangeStart < 15)
            AddMessage($"spanA[0x{idx:X}] = 0x{spanB[idx]:X2} (old=0x{spanA[idx]:X2})");

            spanA[idx] = spanB[idx];
        }

    }

    public bool Execute4()
    {
        var sav4A = savA as SAV4;
        var sav4B = savB as SAV4;
        if (sav4A == null || sav4B == null)
            return false;

        List<int> toIgnore = new List<int> { 10661, 10662, 10698, 11018, 11288, 36, 40, 44, 68, 138, 140, 141, 638, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658, 659, 660, 661, 662, 663, 664, 665, 666, 667, 668, 669, 670, 671, 672, 673, 674, 675, 676, 677, 678, 679, 680, 681, 682, 683, 684, 685, 686, 687, 688, 689, 690, 691, 692, 693, 694, 695, 696, 697, 698, 699, 700, 701, 702, 703, 704, 705, 706, 707, 708, 709, 710, 711, 712, 713, 714, 715, 716, 717, 718, 719, 720, 721, 722, 723, 724, 725, 726, 727, 728, 729, 730, 731, 732, 733, 734, 735, 736, 737, 738, 739, 740, 741, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751, 752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 765, 767, 3616, 3648, 4484, 4485, 4884, 5948, 6192, 25008, 25009, 33156, 36172, 36173, 53020, 53034, 53035 };
        OverwriteBytes(sav4A.General, sav4B.General, changeRanges[0], changeRanges[1], toIgnore);

        List<int> toIgnore2 = new List<int>();
        OverwriteBytes(sav4A.Storage, sav4B.Storage, changeRanges[2], changeRanges[3], toIgnore2);

        sav4A.State.Edited = true;

        return true;
    }
}
