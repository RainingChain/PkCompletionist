using PKHeX.Core;

namespace PkCompletionist.Core;

enum PK_EVENT1
{
    Mew,         // Classic: Toys "R" Us Mew ᅠ https://projectpokemon.org/home/files/file/1791-classic-toys-r-us-mew/
    PikachuSurf, // manually created using info from bulbapedia
}

internal class EventSimulator1 : EventSimulatorX
{

    public EventSimulator1(Command command, SAV1 sav) : base(command, sav)
    {
        this.sav = sav;
    }

    new SAV1 sav;

    public override string? ExecEvent(string evtName)
    {
        var evt = ParseEvtName<PK_EVENT1>(evtName);

        if (evt == PK_EVENT1.Mew)
            return AddPkm("Mew.pk1");
        if (evt == PK_EVENT1.PikachuSurf)
            return AddPkm("PikachuSurf.pk1");

        return "Invalid event name.";
    }

}