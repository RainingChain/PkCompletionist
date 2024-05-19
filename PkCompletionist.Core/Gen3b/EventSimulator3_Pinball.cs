using PKHeX.Core;
using System.Collections.Generic;

namespace PkCompletionist.Core;

enum PK_EVENT3_PINBALL
{
    TransmitDexWithEReaderMons
}

internal class EventSimulator3_Pinball : EventSimulatorX
{

    public EventSimulator3_Pinball(Command command, SAV3_Pinball sav) : base(command, sav, null)
    {
        this.sav = sav;
    }

    public override string? ExecEvent(string evtName)
    {
        var evt = ParseEvtName<PK_EVENT3_PINBALL>(evtName);

        if (evt == PK_EVENT3_PINBALL.TransmitDexWithEReaderMons)
        {
            var pokes = new List<int> { 202,203,204,205 };
            foreach (var poke in pokes)
            {
                var off = SAV3_Pinball.OFF_POKEDEX_START + poke - 1;
                var oldValue = this.sav.Data[off];
                if (oldValue < 2)
                    this.sav.Data[off] = 2;
            }
            return null;
        }

        return "Invalid event name.";
    }

}