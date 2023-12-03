using PKHeX.Core;
using System.Linq;

namespace PkCompletionist.Core;

enum PK_EVENT4
{
    // item
    SecretKey,
    OaksLetter,
    MemberCard,

    //pcd pt
    Arceus,
    SuicuneRowapBerry,
    ShayminMicleBerry,
    CelebiJabocaBerry,
    Jirachi,

    // dual save
    BrickMail_DP,
    PokemonFestaRibbons_RSE,
    Mew_HGSS,

    // range
    Manaphy_RangerShadow,
    Darkrai_RangerShadow,
    Manaphy_RangerGuardian,
    Deoxys_RangerGuardian,
    Shaymin_RangerGuardian,

}

internal class EventSimulator4 : EventSimulatorX
{

    public EventSimulator4(Command command, SAV4 sav, SAV savB) : base(command, sav, savB)
    {
        this.sav = sav;
    }

    new SAV4 sav;

    public override string? ExecEvent(string evtName)
    {
        var evt = ParseEvtName<PK_EVENT4>(evtName);

        if (evt == PK_EVENT4.ShayminMicleBerry)
            return AddPcd("ShayminMicleBerry.pcd");

        return "Invalid event name.";
    }

    public string? AddPcd(string path)
    {
        var pcd = (PCD?)SavUtils.LoadMysteryGift(path);
        if (pcd == null)
            return $"Internal error: invalid PCD. ({path})";

        if (!sav.CanReceiveGift(pcd))
            return "Error: The mystery gift can't be received in this game.";

        if (!pcd.IsCardCompatible(sav, out string msg))
            return msg;

        var added = false;
        var album = sav.GiftAlbum;
        var gifts = album.Gifts;
        for (var i = 0; i < gifts.Length; i++)
        {
            var gift = gifts[i];
            if (!gift.Empty)
                continue;
            if (!(gift is PCD))
                continue;

            gifts[i] = pcd;
            added = true;
            break;
        }
        if (!added)
            return "Error: There are no free Wonder Card slot.";

        var flags = album.Flags;
        flags[pcd.CardID] = true;
        sav.GiftAlbum = new(gifts, flags);

        return null;
    }

}