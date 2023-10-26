using PKHeX.Core;
using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class EventSimulator : Command
{
    [JSExport]
    public static bool Execute(string evtStr, byte[] savABytes)
    {
        var simul = new EventSimulator();
        var savA = simul.SetSavA(savABytes);
        if (savA == null)
            return false;

        var simulX = simul.CreateSimulatorX(savA);
        var err = simulX.ExecEvent(evtStr);
        if (err != null)
        {
            simul.AddError(err);
            return false;
        }
        return true;
    }


    EventSimulatorX CreateSimulatorX(SaveFile savA)
    {
        if (savA is SAV1)
            return new EventSimulator1(this, (savA as SAV1)!);

        if (savA is SAV2)
            return new EventSimulator2(this, (savA as SAV2)!);

        if (savA is SAV3E)
            return new EventSimulator3(this, (savA as SAV3E)!);

        return new EventSimulatorX(this, savA);
    }
}
