using PKHeX.Core;
using System.Linq;

namespace PkCompletionist.Core;

enum PK_EVENT2
{
    Mew, //Celebi Present Campaign, Shiny Mew, https://projectpokemon.org/home/files/file/4073-pcny-distribution-machine-savefiles/
    Celebi, //Celebi Tour Spain 2001, Celebi https://projectpokemon.org/home/forums/topic/40507-gen-2-celebi-tour-celebi/
    GSBall,
    EggTicket,
    BlueskyMail,
    MirageMail,
}

internal class EventSimulator2 : EventSimulatorX
{

    public EventSimulator2(Command command, SAV2 sav) : base(command, sav)
    {
        this.sav = sav;
    }

    new SAV2 sav;

    public override string? ExecEvent(string evtName)
    {
        var evt = ParseEvtName<PK_EVENT2>(evtName);

        if (evt == PK_EVENT2.Mew)
            return AddPkm("Mew.pk2");

        if (evt == PK_EVENT2.Celebi)
            return AddPkm("Celebi.pk2");

        if (evt == PK_EVENT2.GSBall)
        {
            if (HasPkmWithTID(251 /*Celebi*/) || sav.GetEventFlag(832) || sav.GetEventFlag(190) || sav.GetEventFlag(191) || sav.GetEventFlag(192))
                return "Error: You already obtained GS Ball.";

            var err = AddItem(115 /*GS Ball*/);
            if (err != null)
                return err;

            sav.SetEventFlag(832, true);
        }

        if (evt == PK_EVENT2.EggTicket)
            return AddItem(129 /*Egg Ticket*/);

        if (evt == PK_EVENT2.BlueskyMail)
            return AddItem(187 /*Bluesky Mail*/);

        if (evt == PK_EVENT2.MirageMail)
            return AddItem(189 /*Mirage Mail*/);

        return "Invalid event name.";
    }

}