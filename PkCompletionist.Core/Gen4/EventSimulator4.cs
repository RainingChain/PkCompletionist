using PKHeX.Core;
using System.Linq;

namespace PkCompletionist.Core;

enum PK_EVENT4
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

        if (evt == PK_EVENT4.ArceusRowap_Event)
            return SavUtils.AddMysteryGift(sav, "Arceus.pcd");

        if (evt == PK_EVENT4.ShayminMicle_Event)
            return SavUtils.AddMysteryGift(sav, "ShayminMicleBerry.pcd");

        if (evt == PK_EVENT4.EnteiCustap_Event)
            return SavUtils.AddMysteryGift(sav, "EnteiCustapBerry.pcd");
        
        if (evt == PK_EVENT4.CelebiJaboca_Event)
            return SavUtils.AddMysteryGift(sav, "CelebiJabocaBerry.pcd");

        if (evt == PK_EVENT4.Jirachi_Event)
            return SavUtils.AddMysteryGift(sav, "Jirachi.pcd");

        if (evt == PK_EVENT4.SecretKey_Event)
            return SavUtils.AddMysteryGift(sav, "SecretKey.pcd");

        if (evt == PK_EVENT4.OaksLetter_Event)
            return SavUtils.AddMysteryGift(sav, "OaksLetter.pcd");

        if (evt == PK_EVENT4.MemberCard_Event)
            return SavUtils.AddMysteryGift(sav, "MemberCard.pcd");

        if (evt == PK_EVENT4.LucarioDoll_Event)
            return SavUtils.AddMysteryGift(sav, "LucarioDoll.pcd");
        
        if (evt == PK_EVENT4.MewPremierRibbon_HGSS_Event)
        {
            //if (!(savB is SAV4HGSS))
            //    return "Error: This event is exclusive to HeartGold and SoulSilver. Load a HeartGold or SoulSilver savefile in .sav #2.";

            return SavUtils.AddMysteryGift(sav, "Mew_HGSS.pcd");
        }
        if (evt == PK_EVENT4.Darkrai_DP_Event)
        {
            //if (!(savB is SAV4DP))
            //    return "Error: This event is exclusive to Diamond and Pearl. Load a Diamond or Pearl savefile in .sav #2.";
            return SavUtils.AddMysteryGift(sav, "Darkrai_DP.pcd");
        }
        
        if (evt == PK_EVENT4.Manaphy_DP_Event)
        {
            //if (!(savB is SAV4DP))
            //    return "Error: This event is exclusive to Diamond and Pearl. Load a Diamond or Pearl savefile in .sav #2.";
            return SavUtils.AddMysteryGift(sav, "Manaphy_DP.pcd");
        }
        if (evt == PK_EVENT4.Deoxys_DP_Event)
        {
            //if (!(savB is SAV4DP))
            //    return "Error: This event is exclusive to Diamond and Pearl. Load a Diamond or Pearl savefile in .sav #2.";
            return SavUtils.AddMysteryGift(sav, "Deoxys_DP.pcd");
        }

        if (evt == PK_EVENT4.Phione_MyPokemonRanch)
        {
            if (sav.GetAllPKM().Count() < 250)
                return "Error: You must own at least 250 Pokémon to obtain Phione from My Pokémon Ranch.";
            return AddPkm("Phione_MyPokemonRanch.pk4");
        }

        if (evt == PK_EVENT4.Mew_MyPokemonRanch)
        {
            var monCount = sav.GetAllPKM().Count() + (savB == null ? 0 : savB.GetAllPKM().Count());
            if (monCount < 999)
                return $"Error: You must own at least 999 Pokémon within both savefiles to obtain Mew from My Pokémon Ranch. You currently only own {monCount} Pokémon.";
            return AddPkm("Mew_MyPokemonRanch.pk4");
        }

        if (evt == PK_EVENT4.BrickMail_DP_Event)
        {
            //if (!(savB is SAV4DP))
            //    return "Error: The Brick Mail trade was exclusive to Diamond and Pearl, as Platinum was not yet released. Load a Diamond or Pearl savefile in .sav #2.";

            for (var slot = 0; slot < sav.PartyData.Count; slot++)
            {
                var pk = sav.PartyData[slot];
                if (pk.Species == 315 && pk.HeldItem == 146 && pk.CurrentLevel >= 10 && pk.Language == (int)LanguageID.Japanese)
                {
                    var pkm = SavUtils.LoadPk("BrickMail_DP.pk4");
                    if (pkm == null)
                        return "Internal error: invalid pkmFile BrickMail_DP.pk4";
                    sav.SetPartySlotAtIndex(pkm, slot);

                    return null;
                }
            }

            return "Error: You don't own a Japanese Lvl10+ Roselia holding Air Mail.";
        }

        if (evt == PK_EVENT4.PokemonFestaRibbons_RSE_Event)
        {
            if (savB is not SAV3RS && savB is not SAV3E)
                return "Error: Pokemon Festa Ribbons are exclusive to Ruby, Sapphire and Emerald. Load a Ruby, Sapphire or Emerald savefile in .sav #2.";

            var sav3B = (SAV3)savB;
            return EventSimulator3.PokemonFestaRibbons(sav3B);
        }
        if (evt == PK_EVENT4.AuroraTicketEvent_Gen3)
        {
            if (savB is not SAV3E && savB is not SAV3FRLG)
                return "Error: Aurora Ticket Event is exclusive to Emerald, FireRed, LeafGreen. Load a Emerald savefile in .sav #2. (Only Emerald is supported by the tool.)";

            if (savB is not SAV3E)
                return "Error: This tool only supports giving Aurora Ticket to Emerald.";

            var sav3B = (SAV3E)savB;
            return EventSimulator3.AuroraTicketEvent(sav3B);
        }

        return $"Internal error : Invalid event name \"{evt}\".";
    }
}