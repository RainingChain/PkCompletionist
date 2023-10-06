using PKHeX.Core;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class FlagAnalyzer : Command
{
    int changeRangeStart;
    int changeRangeEnd;

    [JSExport]
    public static bool Execute(byte[] savA, byte[] savB, int changeRangeStart, int changeRangeEnd)
    {
        var simul = new FlagAnalyzer(changeRangeStart, changeRangeEnd);
        if (simul.SetSavA(savA) == null)
            return false;
        if (simul.SetSavB(savB) == null)
            return false;
        return simul.Execute();
    }

    public FlagAnalyzer(int changeRangeStart, int changeRangeEnd) 
    {
        this.changeRangeStart = changeRangeStart;
        this.changeRangeEnd = changeRangeEnd;
    }

    public bool Execute()
    {
        if(savA == null || savB == null) 
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
        if(savA.Version == GameVersion.C)
            toIgnore = new() { 0x1252, 0x1253, 0x1254, 0x1255, 0x1F0D, 0x1F0E, 0x2052, 0x2053, 0x2054, 0x2055, 0x2D0D, 0x2D0E };

        List<int> diffIdx = new List<int>();
        for (var i = 0; i < savAData.Length; i++)
        {
            if (savA.Version == GameVersion.C && toIgnore.IndexOf(i) != -1)
                continue;

            if (savAData[i] != savBData[i])
                diffIdx.Add(i);
        }
        AddMessage($"Change count + 1: {diffIdx.Count + 1}");

        for (var i = changeRangeStart; i < changeRangeEnd; i++)
        {
            if (i >= diffIdx.Count)
                continue;

            int idx = diffIdx[i];

            if (changeRangeEnd - changeRangeStart < 15)
                AddMessage($"this.sav1.Data[0x{idx:X}] = 0x{savBData[idx]:X} (old=0x{savAData[idx]:X})");

            savAData[idx] = savBData[idx];
        }
        this.savA.SetData(savAData, 0);
        return true;
    }
}
