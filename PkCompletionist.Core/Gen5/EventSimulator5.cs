using PKHeX.Core;
using System.Linq;

namespace PkCompletionist.Core;

enum PK_EVENT5
{
    Arceus_BW_Event,
    Victini_BW_Event,
    Deoxys_B2_Event,
    Genesect_B2_Event,
    Genesect_Shiny_B2_Event,
    DarkraiClassicRibbonEnigmaBerry_BW_Event,
    Shaymin_BW_Event,
    KeldeoWishingRibbon_B2_Event,
    MeloettaWishingRibbon_B2_Event,
    CustapBerryWishingRibbon_B2_Event,
    PremierRibbon_BW_Event,
    BirthdayRibbon_B2_Event,
    LightBallEventRibbon_B2_Event,
    BattleChampionRibbon_B2_Event,
    SouvenirRibbon_B2_Event,
    PokemonFestaRibbons_RSE_Event,

    //dual
    Mew_MyPokemonRanch
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

        if (evt == PK_EVENT5.Arceus_BW_Event)
            return AddPkm("Global Link Arceus.pkm");

        if (evt == PK_EVENT5.Victini_BW_Event)
            return SavUtils.AddMysteryGift(this.sav, "Movie 14 Victini.pgf");

        if (evt == PK_EVENT5.Deoxys_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "Plasma Deoxys.pgf");

        if (evt == PK_EVENT5.Genesect_Shiny_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "Cinema Genesect.pgf");

        if (evt == PK_EVENT5.Genesect_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "Plasma Genesect.pgf");

        if (evt == PK_EVENT5.DarkraiClassicRibbonEnigmaBerry_BW_Event)
            return SavUtils.AddMysteryGift(this.sav, "Winter 2011 Darkrai.pgf");

        if (evt == PK_EVENT5.Shaymin_BW_Event)
            return SavUtils.AddMysteryGift(this.sav, "Pokémon Center Shaymin.pgf");

        if (evt == PK_EVENT5.KeldeoWishingRibbon_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "Winter 2013 Keldeo.pgf");

        if (evt == PK_EVENT5.MeloettaWishingRibbon_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "Spring 2013 Meloetta.pgf");
        if (evt == PK_EVENT5.CustapBerryWishingRibbon_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "Pokémon Hills Mewtwo.pgf");
        if (evt == PK_EVENT5.PremierRibbon_BW_Event)
            return SavUtils.AddMysteryGift(this.sav, "February 2012 Mewtwo.pgf");
        if (evt == PK_EVENT5.BirthdayRibbon_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "2012 Birthday Audino.pgf");
        if (evt == PK_EVENT5.LightBallEventRibbon_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "Strongest Class Pikachu.pgf");
        if (evt == PK_EVENT5.BattleChampionRibbon_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "Yamamoto's Politoed.pgf");
        if (evt == PK_EVENT5.SouvenirRibbon_B2_Event)
            return SavUtils.AddMysteryGift(this.sav, "2013 Summer Shiny Giratina.pgf");


        if (evt == PK_EVENT5.Mew_MyPokemonRanch)
        {
            var monCount = sav.GetAllPKM().Count() + (savB == null ? 0 : savB.GetAllPKM().Count());
            if (monCount < 999)
                return $"Error: You must own at least 999 Pokémon within both savefiles to obtain Mew from My Pokémon Ranch. You currently only own {monCount} Pokémon.";
            return AddPkm("Mew_MyPokemonRanch.pk4");
        }

        if (evt == PK_EVENT5.PokemonFestaRibbons_RSE_Event)
            return EventSimulator3.PokemonFestaRibbons(sav);

        return $"Internal error : Invalid event name \"{evt}\".";
    }


}