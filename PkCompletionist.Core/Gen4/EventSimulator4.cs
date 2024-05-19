using PKHeX.Core;
using System.Linq;

namespace PkCompletionist.Core;

enum PK_EVENT4
{
    //pcd pt
    SecretKey,
    OaksLetter,
    MemberCard,
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

    public EventSimulator4(Command command, SAV4 sav, SaveFile? savB) : base(command, sav, savB)
    {
        this.sav = sav;
    }

    new SAV4 sav;

    public override string? ExecEvent(string evtName)
    {
        var evt = ParseEvtName<PK_EVENT4>(evtName);

        if (evt == PK_EVENT4.Arceus)
            return AddPcd("Arceus.pcd");

        if (evt == PK_EVENT4.SuicuneRowapBerry)
            return AddPcd("SuicuneRowapBerry.pcd");

        if (evt == PK_EVENT4.ShayminMicleBerry)
            return AddPcd("ShayminMicleBerry.pcd");

        if (evt == PK_EVENT4.CelebiJabocaBerry)
            return AddPcd("CelebiJabocaBerry.pcd");

        if (evt == PK_EVENT4.Jirachi)
            return AddPcd("Jirachi.pcd");

        if (evt == PK_EVENT4.SecretKey)
            return AddPcd("SecretKey.pcd");

        if (evt == PK_EVENT4.OaksLetter)
            return AddPcd("OaksLetter.pcd");

        if (evt == PK_EVENT4.MemberCard)
            return AddPcd("MemberCard.pcd");

        if (evt == PK_EVENT4.Mew_HGSS)
        {
            if (!(savB is SAV4HGSS))
                return "Error: Mew Mystery Gift is exclusive to HeartGold and SoulSilver.";

            return AddPcd("Mew_HGSS.pcd", (SAV4HGSS) savB);
        }

        if (evt == PK_EVENT4.BrickMail_DP)
        {
            if (!(savB is SAV4DP))
                return "Error: The Brick Mail trade was exclusive to Diamond and Pearl, as Platinum was not yet released.";

            int slot = savB.FindSlotIndex(pk =>
            {
                //Japanese Lvl10+ Roselia holding Air Mail
                return pk.Species == 315 && pk.HeldItem == 146 && pk.CurrentLevel >= 10;
            }, savB.SlotCount);

            if (slot == -1)
                return "Error: You don't own a Japanese Lvl10+ Roselia holding Air Mail.";

            var pkm = SavUtils.LoadPk("BrickMail_DP.pk4");
            if (pkm == null)
                return "Internal error: invalid pkmFile BrickMail_DP.pk4";

            savB.SetBoxSlotAtIndex(pkm, slot);

            return null;
        }

        if (evt == PK_EVENT4.PokemonFestaRibbons_RSE)
        {
            if (!(savB is SAV3RS) && !(savB is SAV3E))
                return "Error: Pokemon Festa Ribbons are exclusive to Ruby, Sapphire and Emerald.";

            var sav3B = (SAV3)savB;
            return EventSimulator3.PokemonFestaRibbons(sav3B);
        }


        return "Invalid event name.";
    }

    public string? AddPcd(string path, SAV4? savArg = null)
    {
        var sav = savArg ?? this.sav;
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