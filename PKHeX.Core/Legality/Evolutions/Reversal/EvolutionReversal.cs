using System;

namespace PKHeX.Core;

/// <summary>
/// Evolution Reversal logic
/// </summary>
public static class EvolutionReversal
{
    /// <summary>
    /// Reverses from current state to see what evolutions the <see cref="pk"/> may have existed as.
    /// </summary>
    /// <param name="lineage">Evolution Method lineage reversal object</param>
    /// <param name="result">Result storage</param>
    /// <param name="species">Species to devolve from</param>
    /// <param name="form">Form to devolve from</param>
    /// <param name="pk">Entity reference to sanity check evolutions with</param>
    /// <param name="levelMin">Minimum level the entity may exist as</param>
    /// <param name="levelMax">Maximum the entity may exist as</param>
    /// <param name="stopSpecies">Species ID that should be the last node, if at all. Provide 0 to fully devolve.</param>
    /// <param name="skipChecks">Option to bypass some evolution criteria</param>
    /// <returns>Reversed evolution lineage, with the lowest index being the current species-form.</returns>
    public static int Devolve(this IEvolutionLookup lineage, Span<EvoCriteria> result, ushort species, byte form,
        PKM pk, byte levelMin, byte levelMax, ushort stopSpecies, bool skipChecks)
    {
        // Store our results -- trim at the end when we place it on the heap.
        var head = result[0] = new EvoCriteria { Species = species, Form = form, LevelMax = levelMax };
        int ctr = Devolve(lineage, result, head, pk, levelMax, levelMin, skipChecks, stopSpecies);
        EvolutionUtil.CleanDevolve(result[..ctr], levelMin);
        return ctr;
    }

    private static int Devolve(IEvolutionLookup lineage, Span<EvoCriteria> result, EvoCriteria head,
        PKM pk, byte currentMaxLevel, byte levelMin, bool skipChecks, ushort stopSpecies)
    {
        // There aren't any circular evolution paths, and all lineages have at most 3 evolutions total.
        // There aren't any convergent evolution paths, so only yield the first connection.
        int ctr = 1; // count in the 'evos' span
        while (head.Species != stopSpecies)
        {
            ref readonly var node = ref lineage[head.Species, head.Form];
            if (!node.TryDevolve(pk, currentMaxLevel, levelMin, skipChecks, out var x))
                return ctr;

            result[ctr++] = x;
            currentMaxLevel -= x.LevelUpRequired;
        }
        return ctr;
    }

    public static bool TryDevolve(this EvolutionNode node, PKM pk, byte currentMaxLevel, byte levelMin, bool skipChecks, out EvoCriteria result)
    {
        // Multiple methods can exist to devolve to the same species-form.
        // The first method is less restrictive (no LevelUp req), if two {level/other} methods exist.
        ref readonly var link = ref node.First;
        if (link.IsEmpty)
        {
            result = default;
            return false;
        }

        var chk = link.Method.Check(pk, currentMaxLevel, levelMin, skipChecks);
        if (chk == EvolutionCheckResult.Valid)
        {
            result = Create(link, currentMaxLevel);
            return true;
        }

        link = ref node.Second;
        if (link.IsEmpty)
        {
            result = default;
            return false;
        }

        chk = link.Method.Check(pk, currentMaxLevel, levelMin, skipChecks);
        if (chk == EvolutionCheckResult.Valid)
        {
            result = Create(link, currentMaxLevel);
            return true;
        }

        result = default;
        return false;
    }

    private static EvoCriteria Create(in EvolutionLink link, byte currentMaxLevel) => new()
    {
        Species = link.Species,
        Form = link.Form,
        LevelMax = currentMaxLevel,
        Method = link.Method.Method,

        // Temporarily store these and overwrite them when we clean the list.
        LevelMin = link.Method.Level,
        LevelUpRequired = link.Method.RequiresLevelUp ? (byte)1 : (byte)0,
    };
}
