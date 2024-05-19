//#define VERIFY_GEN
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static PKHeX.Core.EncounterTypeGroup;

namespace PKHeX.Core;

/// <summary>
/// Generates weakly matched <see cref="IEncounterable"/> objects for an input <see cref="PKM"/> (and/or criteria).
/// </summary>
public static class EncounterMovesetGenerator
{
    /// <summary>
    /// Order in which <see cref="IEncounterable"/> objects are yielded from the <see cref="GenerateVersionEncounters"/> generator.
    /// </summary>
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public static IReadOnlyCollection<EncounterTypeGroup> PriorityList { get; set; } = PriorityList = (EncounterTypeGroup[])Enum.GetValues(typeof(EncounterTypeGroup));

    /// <summary>
    /// Resets the <see cref="PriorityList"/> to the default values.
    /// </summary>
    public static void ResetFilters() => PriorityList = (EncounterTypeGroup[])Enum.GetValues(typeof(EncounterTypeGroup));

    /// <summary>
    /// Gets possible <see cref="PKM"/> objects that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="info">Trainer information of the receiver.</param>
    /// <param name="moves">Moves that the resulting <see cref="IEncounterable"/> must be able to learn.</param>
    /// <param name="versions">Any specific version(s) to iterate for. If left blank, all will be checked.</param>
    /// <returns>A consumable <see cref="PKM"/> list of possible results.</returns>
    /// <remarks>When updating, update the sister <see cref="GenerateEncounters(PKM,ITrainerInfo,ReadOnlyMemory{ushort},GameVersion[])"/> method.</remarks>
    public static IEnumerable<PKM> GeneratePKMs(PKM pk, ITrainerInfo info, ReadOnlyMemory<ushort> moves, params GameVersion[] versions)
    {
        if (!IsSane(pk, moves.Span))
            yield break;

        OptimizeCriteria(pk, info);
        var vers = versions.Length >= 1 ? versions : GameUtil.GetVersionsWithinRange(pk, pk.Format);
        foreach (var ver in vers)
        {
            var encounters = GenerateVersionEncounters(pk, moves, ver);
            foreach (var enc in encounters)
            {
                var result = enc.ConvertToPKM(info);
#if VERIFY_GEN
                    var la = new LegalityAnalysis(result);
                    if (!la.Valid)
                        throw new Exception("Legality analysis of generated Pokémon is invalid");
#endif
                yield return result;
            }
        }
    }

    /// <summary>
    /// Gets possible <see cref="IEncounterable"/> objects that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="info">Trainer information of the receiver.</param>
    /// <param name="moves">Moves that the resulting <see cref="IEncounterable"/> must be able to learn.</param>
    /// <param name="versions">Any specific version(s) to iterate for. If left blank, all will be checked.</param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible results.</returns>
    /// <remarks>When updating, update the sister <see cref="GeneratePKMs(PKM,ITrainerInfo,ReadOnlyMemory{ushort},GameVersion[])"/> method.</remarks>
    public static IEnumerable<IEncounterable> GenerateEncounters(PKM pk, ITrainerInfo info, ReadOnlyMemory<ushort> moves, params GameVersion[] versions)
    {
        if (!IsSane(pk, moves.Span))
            yield break;

        OptimizeCriteria(pk, info);
        var vers = versions.Length >= 1 ? versions : GameUtil.GetVersionsWithinRange(pk, pk.Format);
        foreach (var ver in vers)
        {
            var encounters = GenerateVersionEncounters(pk, moves, ver);
            foreach (var enc in encounters)
                yield return enc;
        }
    }

    /// <summary>
    /// Adapts the input <see cref="pk"/> so that it may match as many encounters as possible (indications of trades to other game pairs, etc).
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="info">Trainer information of the receiver.</param>
    public static void OptimizeCriteria(PKM pk, ITrainerID16 info)
    {
        pk.TID16 = info.TID16; // Necessary for Gen2 Headbutt encounters.
        var htTrash = pk.HT_Trash;
        if (htTrash.Length != 0)
            htTrash[0] = 1; // Fake Trash to indicate trading.
    }

    /// <summary>
    /// Gets possible <see cref="PKM"/> objects that allow all moves requested to be learned within a specific generation.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="info">Trainer information of the receiver.</param>
    /// <param name="generation">Specific generation to iterate versions for.</param>
    /// <param name="moves">Moves that the resulting <see cref="IEncounterable"/> must be able to learn.</param>
    public static IEnumerable<PKM> GeneratePKMs(PKM pk, ITrainerInfo info, int generation, ReadOnlyMemory<ushort> moves)
    {
        var vers = GameUtil.GetVersionsInGeneration(generation, pk.Version);
        return GeneratePKMs(pk, info, moves, vers);
    }

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="generation">Specific generation to iterate versions for.</param>
    /// <param name="moves">Moves that the resulting <see cref="IEncounterable"/> must be able to learn.</param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    public static IEnumerable<IEncounterable> GenerateEncounter(PKM pk, int generation, ReadOnlyMemory<ushort> moves)
    {
        var vers = GameUtil.GetVersionsInGeneration(generation, pk.Version);
        return GenerateEncounters(pk, moves, vers);
    }

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="moves">Moves that the resulting <see cref="IEncounterable"/> must be able to learn.</param>
    /// <param name="versions">Any specific version(s) to iterate for. If left blank, all will be checked.</param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    public static IEnumerable<IEncounterable> GenerateEncounters(PKM pk, ReadOnlyMemory<ushort> moves, params GameVersion[] versions)
    {
        if (!IsSane(pk, moves.Span))
            yield break;

        if (versions.Length > 0)
        {
            foreach (var enc in GenerateEncounters(pk, moves, (IReadOnlyList<GameVersion>)versions))
                yield return enc;
            yield break;
        }

        var vers = GameUtil.GetVersionsWithinRange(pk, pk.Format);
        foreach (var ver in vers)
        {
            foreach (var enc in GenerateVersionEncounters(pk, moves, ver))
                yield return enc;
        }
    }

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="moves">Moves that the resulting <see cref="IEncounterable"/> must be able to learn.</param>
    /// <param name="vers">Any specific version(s) to iterate for. If left blank, all will be checked.</param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    public static IEnumerable<IEncounterable> GenerateEncounters(PKM pk, ReadOnlyMemory<ushort> moves, IReadOnlyList<GameVersion> vers)
    {
        if (!IsSane(pk, moves.Span))
            yield break;

        foreach (var ver in vers)
        {
            foreach (var enc in GenerateVersionEncounters(pk, moves, ver))
                yield return enc;
        }
    }

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="moves">Moves that the resulting <see cref="IEncounterable"/> must be able to learn.</param>
    /// <param name="version">Specific version to iterate for.</param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    private static IEnumerable<IEncounterable> GenerateVersionEncounters(PKM pk, ReadOnlyMemory<ushort> moves, GameVersion version)
    {
        pk.Version = (int)version;

        var generation = (byte)version.GetGeneration();
        if (version is GameVersion.GO)
            generation = (byte)pk.Format;
        if (pk.Context is EntityContext.Gen2 && version is GameVersion.RD or GameVersion.GN or GameVersion.BU or GameVersion.YW)
            generation = 1; // try excluding baby pokemon from our evolution chain, for move learning purposes.

        var origin = new EvolutionOrigin(pk.Species, (byte)version, generation, 1, 100, true);
        var chain = EvolutionChain.GetOriginChain(pk, origin);
        if (chain.Length == 0)
            yield break;

        ReadOnlyMemory<ushort> needs = GetNeededMoves(pk, moves.Span, version, generation);
        var generator = EncounterGenerator.GetGenerator(version);

        foreach (var type in PriorityList)
        {
            foreach (var enc in GetPossibleOfType(pk, needs, version, type, chain, generator))
                yield return enc;
        }
    }

    private static bool IsSane(PKM pk, ReadOnlySpan<ushort> moves)
    {
        var species = pk.Species;
        if ((uint)(species - 1) >= pk.MaxSpeciesID) // can enter this method after failing to set a species ID that cannot exist in the format
            return false;
        if (AnyMoveOutOfRange(moves, pk.MaxMoveID))
            return false;
        if (species is (int)Species.Smeargle && !IsPlausibleSmeargleMoveset(pk.Context, moves))
            return false;

        return true;
    }

    private static bool AnyMoveOutOfRange(ReadOnlySpan<ushort> moves, ushort max)
    {
        foreach (var move in moves)
        {
            if (move > max)
                return true;
        }
        return false;
    }

    private static bool IsPlausibleSmeargleMoveset(EntityContext context, ReadOnlySpan<ushort> moves)
    {
        foreach (var move in moves)
        {
            if (!MoveInfo.IsSketchValid(move, context))
                return false;
        }
        return true;
    }

    private static ushort[] GetNeededMoves(PKM pk, ReadOnlySpan<ushort> moves, GameVersion ver, int generation)
    {
        if (pk.Species == (int)Species.Smeargle)
            return Array.Empty<ushort>();

        var length = pk.MaxMoveID + 1;
        var rent = ArrayPool<bool>.Shared.Rent(length);
        var permitted = rent.AsSpan(0, length);
        var enc = new EvolutionOrigin(pk.Species, (byte)ver, (byte)generation, 1, 100, true);
        var history = EvolutionChain.GetEvolutionChainsSearch(pk, enc, ver.GetContext(), 0);
        var e = EncounterInvalid.Default; // default empty
        LearnPossible.Get(pk, e, history, permitted);

        int ctr = 0; // count of moves that can be learned
        Span<ushort> result = stackalloc ushort[moves.Length];
        foreach (var move in moves)
        {
            if (move == 0)
                continue;
            if (!permitted[move])
                result[ctr++] = move;
        }

        permitted.Clear();
        ArrayPool<bool>.Shared.Return(rent);

        if (ctr == 0)
            return Array.Empty<ushort>();
        return result[..ctr].ToArray();
    }

    private static IEnumerable<IEncounterable> GetPossibleOfType(PKM pk, ReadOnlyMemory<ushort> needs, GameVersion version, EncounterTypeGroup type, EvoCriteria[] chain, IEncounterGenerator generator)
        => type switch
    {
        Egg => GetEggs(pk, needs, chain, version, generator),
        Mystery => GetGifts(pk, needs, chain, version, generator),
        Static => GetStatic(pk, needs, chain, version, generator),
        Trade => GetTrades(pk, needs, chain, version, generator),
        Slot => GetSlots(pk, needs, chain, version, generator),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="needs">Moves which cannot be taught by the player.</param>
    /// <param name="chain">Origin possible evolution chain</param>
    /// <param name="version">Specific version to iterate for. Necessary for retrieving possible Egg Moves.</param>
    /// <param name="generator"></param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    private static IEnumerable<IEncounterable> GetEggs(PKM pk, ReadOnlyMemory<ushort> needs, EvoCriteria[] chain, GameVersion version, IEncounterGenerator generator)
    {
        if (!Breeding.CanGameGenerateEggs(version))
            yield break; // no eggs from these games

        var eggs = generator.GetPossible(pk, chain, version, Egg);
        foreach (var egg in eggs)
        {
            if (needs.Length == 0 || HasAllNeededMovesEgg(needs.Span, egg))
                yield return egg;
        }
    }

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="needs">Moves which cannot be taught by the player.</param>
    /// <param name="chain">Origin possible evolution chain</param>
    /// <param name="version">Specific version to iterate for.</param>
    /// <param name="generator">Generator</param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    private static IEnumerable<IEncounterable> GetGifts(PKM pk, ReadOnlyMemory<ushort> needs, EvoCriteria[] chain, GameVersion version, IEncounterGenerator generator)
    {
        var context = pk.Context;
        var gifts = generator.GetPossible(pk, chain, version, Mystery);
        foreach (var enc in gifts)
        {
            if (!IsSane(chain, enc, context))
                continue;
            if (needs.Length == 0 || GetHasAllNeededMoves(needs.Span, enc))
                yield return enc;
        }
    }

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="needs">Moves which cannot be taught by the player.</param>
    /// <param name="chain">Origin possible evolution chain</param>
    /// <param name="version">Specific version to iterate for.</param>
    /// <param name="generator"></param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    private static IEnumerable<IEncounterable> GetStatic(PKM pk, ReadOnlyMemory<ushort> needs, EvoCriteria[] chain, GameVersion version, IEncounterGenerator generator)
    {
        var context = pk.Context;
        var encounters = generator.GetPossible(pk, chain, version, Static);
        foreach (var enc in encounters)
        {
            if (!IsSane(chain, enc, context))
                continue;
            if (needs.Length == 0 || GetHasAllNeededMovesConsiderGen2(needs.Span, enc))
                yield return enc;
        }
    }

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="needs">Moves which cannot be taught by the player.</param>
    /// <param name="chain">Origin possible evolution chain</param>
    /// <param name="version">Specific version to iterate for.</param>
    /// <param name="generator"></param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    private static IEnumerable<IEncounterable> GetTrades(PKM pk, ReadOnlyMemory<ushort> needs, EvoCriteria[] chain, GameVersion version, IEncounterGenerator generator)
    {
        var context = pk.Context;
        var trades = generator.GetPossible(pk, chain, version, Trade);
        foreach (var enc in trades)
        {
            if (!IsSane(chain, enc, context))
                continue;
            if (needs.Length == 0 || GetHasAllNeededMovesConsiderGen2(needs.Span, enc))
                yield return enc;
        }
    }

    /// <summary>
    /// Gets possible encounters that allow all moves requested to be learned.
    /// </summary>
    /// <param name="pk">Rough Pokémon data which contains the requested species, gender, and form.</param>
    /// <param name="needs">Moves which cannot be taught by the player.</param>
    /// <param name="chain">Origin possible evolution chain</param>
    /// <param name="version">Origin version</param>
    /// <param name="generator"></param>
    /// <returns>A consumable <see cref="IEncounterable"/> list of possible encounters.</returns>
    private static IEnumerable<IEncounterable> GetSlots(PKM pk, ReadOnlyMemory<ushort> needs, EvoCriteria[] chain, GameVersion version, IEncounterGenerator generator)
    {
        var context = pk.Context;
        var slots = generator.GetPossible(pk, chain, version, Slot);
        foreach (var slot in slots)
        {
            if (!IsSane(chain, slot, context))
                continue;
            if (needs.Length == 0 || HasAllNeededMovesSlot(needs.Span, slot))
                yield return slot;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSane(ReadOnlySpan<EvoCriteria> chain, IEncounterTemplate enc, EntityContext current)
    {
        foreach (var evo in chain)
        {
            if (evo.Species != enc.Species)
                continue;
            if (evo.Form == enc.Form)
                return true;
            if (FormInfo.IsFormChangeable(enc.Species, enc.Form, evo.Form, enc.Context, current))
                return true;
            if (enc is IEncounterFormRandom { IsRandomUnspecificForm: true })
                return true;
            if (enc is EncounterStatic7 {IsTotem: true} && evo.Form == 0 && current.Generation() > 7) // totems get form wiped
                return true;
            break;
        }
        return false;
    }

    private static int GetMoveMask(ReadOnlySpan<ushort> needs, IEncounterTemplate enc)
    {
        var flags = 0;
        if (enc is IMoveset { Moves: { HasMoves: true } m })
            flags = m.BitOverlap(needs);
        if (enc is IRelearn { Relearn: { HasMoves: true } r })
            flags |= r.BitOverlap(needs);
        return flags;
    }

    private static int GetMoveMaskConsiderGen2(ReadOnlySpan<ushort> needs, IEncounterTemplate enc)
    {
        int flags = GetMoveMask(needs, enc);
        if (enc.Generation <= 2)
            flags |= GetMoveMaskGen2(needs, enc);
        return flags;
    }

    private static int GetMoveMaskGen2(ReadOnlySpan<ushort> needs, IEncounterTemplate enc)
    {
        Span<ushort> moves = stackalloc ushort[4];
        var source = GameData.GetLearnSource(enc.Version);
        source.SetEncounterMoves(enc.Species, 0, enc.LevelMin, moves);
        return Moveset.BitOverlap(moves, needs);
    }

    private static int GetMoveMaskEgg(ReadOnlySpan<ushort> needs, IEncounterTemplate egg)
    {
        var source = GameData.GetLearnSource(egg.Version);
        var eggMoves = source.GetEggMoves(egg.Species, egg.Form);
        int flags = Moveset.BitOverlap(eggMoves, needs);
        var vt = needs.IndexOf((ushort)Move.VoltTackle);
        if (vt != -1 && egg is EncounterEgg { CanHaveVoltTackle: true })
            flags |= 1 << vt;
        else if (egg.Generation <= 2)
            flags |= GetMoveMaskGen2(needs, egg);
        return flags;
    }

    private static bool HasAllNeededMovesSlot(ReadOnlySpan<ushort> needs, IEncounterTemplate slot)
    {
        if (slot is IMoveset m)
            return m.Moves.ContainsAll(needs);
        if (needs.Length == 1 && slot is EncounterSlot6AO dn)
            return dn.CanDexNav && dn.CanBeDexNavMove(needs[0]);
        if (needs.Length == 1 && slot is EncounterSlot8b ug)
            return ug.IsUnderground && ug.CanBeUndergroundMove(needs[0]);
        if (slot.Generation <= 2)
            return HasAllNeededMovesEncounter2(needs, slot);
        return false;
    }

    private static bool HasAllNeededMovesEgg(ReadOnlySpan<ushort> needs, IEncounterTemplate egg)
    {
        int flags = GetMoveMaskEgg(needs, egg);
        return flags == (1 << needs.Length) - 1;
    }

    private static bool GetHasAllNeededMoves(ReadOnlySpan<ushort> needs, IEncounterTemplate enc)
    {
        int flags = GetMoveMask(needs, enc);
        return flags == (1 << needs.Length) - 1;
    }

    private static bool GetHasAllNeededMovesConsiderGen2(ReadOnlySpan<ushort> needs, IEncounterTemplate enc)
    {
        // Some rare encounters have special moves hidden in the Relearn section (Gen7 Wormhole Ho-Oh). Include relearn moves
        int flags = GetMoveMaskConsiderGen2(needs, enc);
        return flags == (1 << needs.Length) - 1;
    }

    private static bool HasAllNeededMovesEncounter2(ReadOnlySpan<ushort> needs, IEncounterTemplate enc)
    {
        int flags = GetMoveMaskGen2(needs, enc);
        return flags == (1 << needs.Length) - 1;
    }
}
