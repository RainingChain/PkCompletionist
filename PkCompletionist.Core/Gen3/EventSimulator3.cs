using PKHeX.Core;

namespace PkCompletionist.Core;

enum PK_EVENT3
{
    MysticTicket,
    OldSeaMap,
    AuroraTicket,
    EonTicket,
    PokemonFesta,
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
        var evt = ParseEvtName<PK_EVENT3>(evtName);

        if (evt == PK_EVENT3.EonTicket) // player keeps the item
            return AddItem(275);

        if (evt == PK_EVENT3.MysticTicket)
        {
            if (HasPkmWithTID(249) || HasPkmWithTID(250)) // Lugia or Ho-oh
                return "You already obtained MysticTicket.";
            return AddItem(370);
        }

        if (evt == PK_EVENT3.AuroraTicket)
        {
            if (HasPkmWithTID(386)) // Deoxys
                return "You already obtained AuroraTicket.";
            return AddItem(371);
        }

        if (evt == PK_EVENT3.OldSeaMap)
        {
            if (HasPkmWithTID(151)) // Mew
                return "You already obtained Old Sea Map.";
            return AddItem(376);
        }


        if (evt == PK_EVENT3.PokemonFesta)
        {
            if (this.sav.PartyData.Count == 0)
                return "Your party is empty.";

            var pkm = this.sav.PartyData[0] as G3PKM;
            if (pkm == null)
                return "Pokemon is not generation 3.";

            pkm.RibbonCountry = true;
            pkm.RibbonWorld = true;

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