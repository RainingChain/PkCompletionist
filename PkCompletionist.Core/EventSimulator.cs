using PKHeX.Core;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace PkCompletionist.Core;

enum PK_EVENT
{
    Gen1_Mew, //Classic: Toys "R" Us Mew ᅠ https://projectpokemon.org/home/files/file/1791-classic-toys-r-us-mew/
    Gen1_PikachuSurf, // manually created using info from bulbapedia
    Gen2_Mew, //Celebi Present Campaign, Shiny Mew, https://projectpokemon.org/home/files/file/4073-pcny-distribution-machine-savefiles/
    Gen2_Celebi, //Celebi Tour Spain 2001, Celebi https://projectpokemon.org/home/forums/topic/40507-gen-2-celebi-tour-celebi/
    Gen2_GSBall,
    Gen2_EggTicket,
    Gen2_BlueskyMail,
    Gen2_MirageMail,
}

partial class EventSimulator : Command
{
    [JSExport]
    public static bool Execute(string evtStr, byte[] savA)
    {
        var simul = new EventSimulator();
        if (simul.SetSavA(savA) == null)
            return false;

        if (evtStr == "*")
            simul.ExecAllEvents();
        else
        {
            if (!System.Enum.TryParse<PK_EVENT>(evtStr, out var evt))
            {
                simul.AddError("Error: Invalid event name.");
                return false;
            }
            var err = simul.ExecEvent(evt);
            if (err != null)
            {
                simul.AddError(err);
                return false;
            }
        }
        return true;
    }

    void ExecAllEvents()
    {
        foreach (PK_EVENT evt in System.Enum.GetValues(typeof(PK_EVENT)))
        { 
            var err = ExecEvent(evt);
            if (err != null)
                AddError(err);
        }
    }

    string? ExecEvent(PK_EVENT evt)
    {
        if (savA?.Generation == 1)
        {
            if (evt == PK_EVENT.Gen1_Mew)
                return SavUtils.AddPkm(savA, "Gen1_Mew.pk1");
            if (evt == PK_EVENT.Gen1_PikachuSurf)
                return SavUtils.AddPkm(savA, "Gen1_PikachuSurf.pk1");
        }

        if (savA?.Generation == 2)
        {
            if (evt == PK_EVENT.Gen2_Mew)
                return SavUtils.AddPkm(savA, "Gen2_Mew.pk2");

            if (evt == PK_EVENT.Gen2_Celebi)
                return SavUtils.AddPkm(savA, "Gen2_Celebi.pk2");
            
            if (evt == PK_EVENT.Gen2_GSBall)
            {
                SAV2? sav2 = savA as SAV2;
                if (sav2 == null)
                    return "Error: The save file provided is not Generation 2.";

                var allPkms = sav2.GetAllPKM();
                var celebi = allPkms.Any(pk =>
                {
                    return pk.Species == 251 && pk.TID16 == sav2.TID16 && pk.OT_Name == sav2.OT;
                });

                if (celebi || sav2.GetEventFlag(832) || sav2.GetEventFlag(190) || sav2.GetEventFlag(191) || sav2.GetEventFlag(192))
                    return "Error: You already obtained GS Ball.";

                var err = SavUtils.AddItem(sav2, 115 /*GS Ball*/);
                if (err != null)
                    return err;

                sav2.SetEventFlag(832, true);
            }

            if (evt == PK_EVENT.Gen2_EggTicket)
                return SavUtils.AddItem(savA, 129 /*Egg Ticket*/);

            if (evt == PK_EVENT.Gen2_BlueskyMail)
                return SavUtils.AddItem(savA, 187 /*Bluesky Mail*/);

            if (evt == PK_EVENT.Gen2_MirageMail)
                return SavUtils.AddItem(savA, 189 /*Mirage Mail*/);
        }
        return null;
    }
}
