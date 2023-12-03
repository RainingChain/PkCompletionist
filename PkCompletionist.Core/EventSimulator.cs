using PKHeX.Core;
using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

partial class EventSimulator : Command
{
    [JSExport]
    public static bool Execute(string evtStr, byte[] savABytes, byte[]? savBBytes)
    {
        var simul = new EventSimulator();
        var savA = simul.SetSavA(savABytes);
        if (savA == null)
            return false;

        if (savBBytes != null)
        {
            var savB = simul.SetSavB(savBBytes);
            if (savB == null)
                return false;
        }

        var simulX = simul.CreateSimulatorX(savA, savB);
        var err = simulX.ExecEvent(evtStr);
        if (err != null)
        {
            simul.AddError(err);
            return false;
        }
        return true;
    }


    EventSimulatorX CreateSimulatorX(SaveFile savA, SaveFile savB)
    {
        if (savA is SAV1)
            return new EventSimulator1(this, (savA as SAV1)!);

        if (savA is SAV2)
            return new EventSimulator2(this, (savA as SAV2)!, savB);

        if (savA is SAV3E)
            return new EventSimulator3(this, (savA as SAV3E)!);

        if (savA is SAV4)
            return new EventSimulator4(this, (savA as SAV4)!, savB);

        return new EventSimulatorX(this, savA, savB);
    }
}
