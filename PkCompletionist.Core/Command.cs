using PKHeX.Core;
using System.Runtime.InteropServices.JavaScript;
using System.Collections.Generic;

namespace PkCompletionist.Core;
partial class Command
{
    public Command()
    {
        LastCommand = this;
    }

    public static Command? LastCommand = null;

    List<string> Messages = new();
    public string? LastError = null;
    public SaveFile? savA = null;
    public SaveFile? savB = null;

    public void AddMessage(string msg)
    {
        Messages.Add(msg);
    }

    public void AddError(string msg)
    {
        if (LastError == msg)
            return;
        AddMessage(msg);
        LastError = msg;
    }
    public static SaveFile? GetVariantSAV(byte[] data, string VersionHint)
    {
        var shuffle = SAV7_Shuffle.NewIfValid(data);
        if (shuffle != null)
            return shuffle;

        var pmdRanger = SAV4_Ranger.NewIfValid(data, VersionHint);
        if (pmdRanger != null)
            return pmdRanger;

        var pinballRs = SAV3_Pinball.NewIfValid(data);
        if (pinballRs != null)
            return pinballRs;

        var pinball = SAV1_Pinball.NewIfValid(data, VersionHint);
        if (pinball != null)
            return pinball;

        var pmdRescueTeam = SAV3_PmdRescueTeam.NewIfValid(data, VersionHint);
        if (pmdRescueTeam != null)
            return pmdRescueTeam;

        var pmdSky = SAV4_PmdSky.NewIfValid(data, VersionHint);
        if (pmdSky != null)
            return pmdSky;

        return SaveUtil.GetVariantSAV(data);
    }

    public SaveFile? SetSavA(byte[] data, string VersionHint)
    {
        savA = GetVariantSAV(data, VersionHint);
        if (savA == null)
            Messages.Add("Error: Failed to parse the content of the save file #1.");
        return savA;
    }
    public SaveFile? SetSavB(byte[] data, string VersionHint)
    {
        savB = GetVariantSAV(data, VersionHint);
        if (savB == null)
            Messages.Add("Error: Failed to parse the content of the save file #2.");
        return savB;
    }

    [JSExport]
    public static string[] GetMessages()
    {
        if (LastCommand == null)
            return System.Array.Empty<string>();
        return LastCommand.Messages.ToArray();
    }

    [JSExport]
    public static byte[] GetSavA()
    {
        if (LastCommand?.savA == null)
            return System.Array.Empty<byte>();
        return LastCommand.savA.Write();
    }

    [JSExport]
    public static byte[] GetSavB()
    {
        if (LastCommand?.savB == null)
            return System.Array.Empty<byte>();
        return LastCommand.savB.Write();
    }

}
