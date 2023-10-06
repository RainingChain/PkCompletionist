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
    public SaveFile? savA = null;
    public SaveFile? savB = null;

    public void AddMessage(string msg)
    {
        Messages.Add(msg);
    }

    public void AddError(string msg)
    {
        AddMessage(msg);
    }

    public SaveFile? SetSavA(byte[] data)
    {
        savA = SaveUtil.GetVariantSAV(data);
        if (savA == null)
            Messages.Add("Error: Failed to parse the content of the save file.");
        return savA;
    }
    public SaveFile? SetSavB(byte[] data)
    {
        savB = SaveUtil.GetVariantSAV(data);
        if (savB == null)
            Messages.Add("Error: Failed to parse the content of the save file.");
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
