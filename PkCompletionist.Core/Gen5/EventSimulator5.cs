using PKHeX.Core;
using System.Linq;

namespace PkCompletionist.Core;

enum PK_EVENT5
{
    //pcd pt
    SecretKey_Event,
    OaksLetter_Event,
    MemberCard_Event,
    ArceusRowap_Event,
    ShayminMicle_Event,
    EnteiCustap_Event,
    CelebiJaboca_Event,
    Jirachi_Event,
    LucarioDoll_Event,

    // dual save
    BrickMail_DP_Event,
    PokemonFestaRibbons_RSE_Event,
    MewPremierRibbon_HGSS_Event,
    Darkrai_DP_Event,
    Manaphy_DP_Event,
    Deoxys_DP_Event,
    AuroraTicketEvent_Gen3,
    Phione_MyPokemonRanch,
    Mew_MyPokemonRanch,

    Manaphy_RangerShadow_Event,
    Darkrai_RangerShadow_Event,
    Manaphy_RangerGuardian_Event,
    Deoxys_RangerGuardian_Event,
    Shaymin_RangerGuardian_Event,
}

internal class EventSimulator5 : EventSimulatorX
{

    public EventSimulator5(Command command, SAV5 sav, SaveFile? savB) : base(command, sav, savB)
    {
        this.sav = sav;
    }

    new SAV5 sav;

    public override string? ExecEvent(string evtName)
    {
        var evt = ParseEvtName<PK_EVENT5>(evtName);

        if (evt == PK_EVENT5.ArceusRowap_Event)
            return AddPcd("Arceus.pcd");

        if (evt == PK_EVENT5.ShayminMicle_Event)
            return AddPcd("ShayminMicleBerry.pcd");

        if (evt == PK_EVENT5.EnteiCustap_Event)
            return AddPcd("EnteiCustapBerry.pcd");

        if (evt == PK_EVENT5.CelebiJaboca_Event)
            return AddPcd("CelebiJabocaBerry.pcd");

        if (evt == PK_EVENT5.Jirachi_Event)
            return AddPcd("Jirachi.pcd");

        if (evt == PK_EVENT5.SecretKey_Event)
            return AddPcd("SecretKey.pcd");

        if (evt == PK_EVENT5.OaksLetter_Event)
            return AddPcd("OaksLetter.pcd");

        if (evt == PK_EVENT5.MemberCard_Event)
            return AddPcd("MemberCard.pcd");

        if (evt == PK_EVENT5.LucarioDoll_Event)
            return AddPcd("LucarioDoll.pcd");
        /*
        if (evt == PK_EVENT5.MewPremierRibbon_HGSS_Event)
        {
            if (!(savB is SAV5HGSS))
                return "Error: This event is exclusive to HeartGold and SoulSilver. Load a HeartGold or SoulSilver savefile in .sav #2.";

            return AddPcd("Mew_HGSS.pcd", (SAV5HGSS)savB);
        }
        if (evt == PK_EVENT5.Darkrai_DP_Event)
        {
            if (!(savB is SAV5DP))
                return "Error: This event is exclusive to Diamond and Pearl. Load a Diamond or Pearl savefile in .sav #2.";
            return AddPcd("Darkrai_DP.pcd");
        }

        if (evt == PK_EVENT5.Manaphy_DP_Event)
        {
            if (!(savB is SAV5DP))
                return "Error: This event is exclusive to Diamond and Pearl. Load a Diamond or Pearl savefile in .sav #2.";
            return AddPcd("Manaphy_DP.pcd");
        }
        if (evt == PK_EVENT5.Deoxys_DP_Event)
        {
            if (!(savB is SAV5DP))
                return "Error: This event is exclusive to Diamond and Pearl. Load a Diamond or Pearl savefile in .sav #2.";
            return AddPcd("Deoxys_DP.pcd");
        }

        if (evt == PK_EVENT5.Phione_MyPokemonRanch)
        {
            if (sav.GetAllPKM().Count() < 250)
                return "Error: You must own at least 250 Pokémon to obtain Phione from My Pokémon Ranch.";
            return AddPkm("Phione_MyPokemonRanch.pk4");
        }

        if (evt == PK_EVENT5.Mew_MyPokemonRanch)
        {
            var monCount = sav.GetAllPKM().Count() + (savB == null ? 0 : savB.GetAllPKM().Count());
            if (monCount < 999)
                return $"Error: You must own at least 999 Pokémon within both savefiles to obtain Mew from My Pokémon Ranch. You currently only own {monCount} Pokémon.";
            return AddPkm("Mew_MyPokemonRanch.pk4");
        }
        if (evt == PK_EVENT5.BrickMail_DP_Event)
        {
            if (!(savB is SAV5DP))
                return "Error: The Brick Mail trade was exclusive to Diamond and Pearl, as Platinum was not yet released. Load a Diamond or Pearl savefile in .sav #2.";

            for (var slot = 0; slot < savB.PartyData.Count; slot++)
            {
                var pk = savB.PartyData[slot];
                if (pk.Species == 315 && pk.HeldItem == 146 && pk.CurrentLevel >= 10 && pk.Language == (int)LanguageID.Japanese)
                {
                    var pkm = SavUtils.LoadPk("BrickMail_DP.pk4");
                    if (pkm == null)
                        return "Internal error: invalid pkmFile BrickMail_DP.pk4";
                    savB.SetPartySlotAtIndex(pkm, slot);

                    return null;
                }
            }

            return "Error: You don't own a Japanese Lvl10+ Roselia holding Air Mail.";
        }
        
        */
        if (evt == PK_EVENT5.PokemonFestaRibbons_RSE_Event)
        {
            if (!(savB is SAV3RS) && !(savB is SAV3E))
                return "Error: Pokemon Festa Ribbons are exclusive to Ruby, Sapphire and Emerald. Load a Ruby, Sapphire or Emerald savefile in .sav #2.";

            var sav3B = (SAV3)savB;
            return EventSimulator3.PokemonFestaRibbons(sav3B);
        }
        if (evt == PK_EVENT5.AuroraTicketEvent_Gen3)
        {
            if (!(savB is SAV3E) && !(savB is SAV3FRLG))
                return "Error: Aurora Ticket Event is exclusive to Emerald, FireRed, LeafGreen. Load a Emerald savefile in .sav #2. (Only Emerald is supported by the tool.)";

            if (!(savB is SAV3E))
                return "Error: This tool only supports giving Aurora Ticket to Emerald.";

            var sav3B = (SAV3E)savB;
            return EventSimulator3.AuroraTicketEvent(sav3B);
        }

        if (evt == PK_EVENT5.Manaphy_RangerShadow_Event ||
            evt == PK_EVENT5.Darkrai_RangerShadow_Event ||
            evt == PK_EVENT5.Manaphy_RangerGuardian_Event ||
            evt == PK_EVENT5.Deoxys_RangerGuardian_Event ||
            evt == PK_EVENT5.Shaymin_RangerGuardian_Event)
        {
            return "Error: Ranger events are not supported yet this by tool.";
        }

        return $"Internal error : Invalid event name \"{evt}\".";
    }

    public string? AddPcd(string path, SAV5? savArg = null)
    {
        var sav = savArg ?? this.sav;
        var pcd = (PCD?)SavUtils.LoadMysteryGift(path);
        if (pcd == null)
            return $"Internal error: invalid PCD. ({path})";

        if (!sav.CanReceiveGift(pcd))
            return "Error: The mystery gift can't be received in this game.";

        if (!pcd.IsCardCompatible(sav, out string msg))
            return msg;

        var pcdIdx = -1;
        var pgtIdx = -1;
        var album = sav.GiftAlbum;
        var gifts = album.Gifts;
        for (var i = 0; i < gifts.Length; i++)
        {
            var gift = gifts[i];
            if (gift is PGT && pgtIdx == -1)
            {
                if (gift.Empty || gift.CardID == -1)
                    pgtIdx = i;
            }

            if (gift is PCD && pcdIdx == -1)
            {
                if (gift.Empty || ((PCD)gift).Gift.CardType == 0) // unused
                    pcdIdx = i;
            }
        }
        if (pcdIdx == -1 || pgtIdx == -1)
            return "Error: There are no free Wonder Card slot.";

        gifts[pcdIdx] = pcd;
        gifts[pgtIdx] = pcd.Gift;

        var flags = album.Flags;
        flags[pcd.CardID] = true;
        sav.GiftAlbum = new(gifts, flags);

        return null;
    }

}