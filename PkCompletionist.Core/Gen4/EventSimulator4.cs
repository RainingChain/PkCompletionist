using PKHeX.Core;

namespace PkCompletionist.Core;

enum PK_EVENT4
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

internal class EventSimulator4 : EventSimulatorX
{

    public EventSimulator4(Command command, SAV4Sinnoh sav) : base(command, sav)
    {
        this.sav = sav;
    }

    new SAV4Sinnoh sav; // Not using SAV4Pt because some events are DP-exclusive (ex: Brick Mail)

    public override string? ExecEvent(string evtName)
    {
        var evt = ParseEvtName<PK_EVENT4>(evtName);

        return "Invalid event name.";
    }

}