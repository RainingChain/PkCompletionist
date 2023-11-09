using PKHeX.Core;

namespace PkCompletionist.Core;

enum PK_EVENT3
{
    MysticTicket,
    OldSeaMap,
    AuroraTicket,
    EonTicket,
    PokemonFestaRibbons,
    Deoxys, // https://projectpokemon.org/home/files/file/1681-space-c-deoxys/ Space Center Houston, to commemorate the 10th Anniversary of Pokémon. The distribution ran daily from March 10 to March 19, 2006, 
    Jirachi, // https://digiex.net/threads/pokemon-gen3-legit-jpn-event-pokemon-pk3-downloads-ruby-sapphire-emerald-firered-leafgreen.15268/ 2005 Tanabata Jirachi - 4x
    Celebi, // https://projectpokemon.org/home/files/file/1684-journey-across-america-celebi/ Pokémon 10th Anniversary Journey Across America, United States back in 2006.
    Mew, // https://projectpokemon.org/home/files/file/1695-mystry-mew/ MYSTRY MEW Toys "R" Us stores across the United States, distributed to commemorate the release of M08: Lucario and the Mystery of Mew.
}

internal class EventSimulator3 : EventSimulatorX
{

    public EventSimulator3(Command command, SAV3E sav) : base(command, sav)
    {
        this.sav = sav;
    }

    new SAV3E sav;

    public override string? ExecEvent(string evtName)
    {
        var SYSTEM_FLAGS = 0x860;

        var evt = ParseEvtName<PK_EVENT3>(evtName);

        if (evt == PK_EVENT3.EonTicket)
        {
            var FLAG_ENABLE_SHIP_SOUTHERN_ISLAND = SYSTEM_FLAGS + 0x53;
            if (sav.GetEventFlag(FLAG_ENABLE_SHIP_SOUTHERN_ISLAND))
                return "You already obtained Eon Ticket.";

            sav.SetEventFlag(FLAG_ENABLE_SHIP_SOUTHERN_ISLAND, true);
            return AddItem(275);
        }

        if (evt == PK_EVENT3.MysticTicket)
        {
            var FLAG_RECEIVED_MYSTIC_TICKET = 0x13B;
            var FLAG_CAUGHT_LUGIA = 0x91;
            var FLAG_CAUGHT_HO_OH = 0x92;
            var FLAG_ENABLE_SHIP_NAVEL_ROCK = SYSTEM_FLAGS + 0x80;

            if (sav.GetEventFlag(FLAG_RECEIVED_MYSTIC_TICKET)
                 || sav.GetEventFlag(FLAG_CAUGHT_LUGIA)
                 || sav.GetEventFlag(FLAG_CAUGHT_HO_OH))
                return "You already obtained MysticTicket.";

            sav.SetEventFlag(FLAG_ENABLE_SHIP_NAVEL_ROCK, true);
            sav.SetEventFlag(FLAG_RECEIVED_MYSTIC_TICKET, true);

            return AddItem(370);
        }

        if (evt == PK_EVENT3.AuroraTicket)
        {
            var FLAG_RECEIVED_AURORA_TICKET = 0x13A;
            var FLAG_BATTLED_DEOXYS = 0x1AD;
            var FLAG_ENABLE_SHIP_BIRTH_ISLAND = SYSTEM_FLAGS + 0x75;

            if (sav.GetEventFlag(FLAG_RECEIVED_AURORA_TICKET) 
                 || sav.GetEventFlag(FLAG_BATTLED_DEOXYS))
                return "You already obtained AuroraTicket.";

            sav.SetEventFlag(FLAG_ENABLE_SHIP_BIRTH_ISLAND, true);
            sav.SetEventFlag(FLAG_RECEIVED_AURORA_TICKET, true);

            return AddItem(371);
        }

        if (evt == PK_EVENT3.OldSeaMap)
        {
            var FLAG_RECEIVED_OLD_SEA_MAP = 0x13C;
            var FLAG_CAUGHT_MEW = 0x1CA;
            var FLAG_ENABLE_SHIP_FARAWAY_ISLAND = SYSTEM_FLAGS + 0x76;

            if (sav.GetEventFlag(FLAG_RECEIVED_OLD_SEA_MAP) 
                 || sav.GetEventFlag(FLAG_CAUGHT_MEW))
                return "You already obtained Old Sea Map.";

            sav.SetEventFlag(FLAG_ENABLE_SHIP_FARAWAY_ISLAND, true);
            sav.SetEventFlag(FLAG_RECEIVED_OLD_SEA_MAP, true);

            return AddItem(376); // OldSeaMap
        }


        if (evt == PK_EVENT3.PokemonFestaRibbons)
        {
            if (this.sav.PartyData.Count == 0)
                return "Your party is empty.";

            var pkm = this.sav.PartyData[0] as G3PKM;
            if (pkm == null)
                return "Pokemon is not generation 3.";

            pkm.RibbonCountry = true;
            pkm.RibbonWorld = true;

            this.sav.SetPartySlotAtIndex(pkm, 0);

            return null;
        }

        if (evt == PK_EVENT3.Mew)
            return AddPkm("Mew.pk3");

        if (evt == PK_EVENT3.Jirachi)
            return AddPkm("Jirachi.pk3");

        if (evt == PK_EVENT3.Celebi)
            return AddPkm("Celebi.pk3");

        if (evt == PK_EVENT3.Deoxys)
            return AddPkm("Deoxys.pk3");
        
        return "Invalid event name.";
    }

}