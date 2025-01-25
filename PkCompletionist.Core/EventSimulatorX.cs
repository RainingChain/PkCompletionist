using PKHeX.Core;
using System.Collections.Generic;
using System.Linq;

namespace PkCompletionist.Core;
partial class EventSimulatorX
{
    protected SaveFile sav;
    protected SaveFile? savB;
    protected Command command;

    public EventSimulatorX(Command command, SaveFile sav, SaveFile? savB)
    {
        this.command = command;
        this.sav = sav;
        this.savB = savB;
    }


    public void AddMessage(string msg)
    {
        this.command.AddMessage(msg);
    }
    public string? AddPkm(string file, bool OnlyIfNotOwned = true)
    {
        return SavUtils.AddPkm(this.sav, file, OnlyIfNotOwned);
    }

    public string? AddItem(ushort item, bool OnlyIfNotOwned = true)
    {
        return SavUtils.AddItem(this.sav, item, OnlyIfNotOwned);
    }

    public T? ParseEvtName<T>(string evtName) where T : struct
    {
        if (!System.Enum.TryParse<T>(evtName, out var evt))
            return null;

        return evt;
    }

    public bool HasPkmWithTID(ushort species)
    {
        return SavUtils.HasPkmWithTID(sav, species);
    }    

    public virtual string? ExecEvent(string evtName)
    {
        return "Unsupported generation.";
    }
}
