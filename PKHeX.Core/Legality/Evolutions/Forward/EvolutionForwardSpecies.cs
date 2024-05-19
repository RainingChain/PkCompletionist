using System;
using System.Collections.Generic;

namespace PKHeX.Core;

/// <summary>
/// Provides forward evolution pathways based only on species.
/// </summary>
public sealed class EvolutionForwardSpecies : IEvolutionForward
{
    private readonly EvolutionMethod[][] Entries;

    public EvolutionForwardSpecies(EvolutionMethod[][] entries) => Entries = entries;

    public IEnumerable<(ushort Species, byte Form)> GetEvolutions(ushort species, byte form)
    {
        var methods = GetForward(species, form);
        return GetEvolutions(methods, form);
    }

    public ReadOnlyMemory<EvolutionMethod> GetForward(ushort species, byte form)
    {
        var arr = Entries;
        if (species >= arr.Length)
            return Array.Empty<EvolutionMethod>();
        return arr[species];
    }

    private IEnumerable<(ushort Species, byte Form)> GetEvolutions(ReadOnlyMemory<EvolutionMethod> evos, byte form)
    {
        for (int i = 0; i < evos.Length; i++)
        {
            var method = evos.Span[i];
            var s = method.Species;
            if (s == 0)
                continue;
            var f = method.GetDestinationForm(form);
            yield return (s, f);
            var nextEvolutions = GetEvolutions(s, f);
            foreach (var nextEvo in nextEvolutions)
                yield return nextEvo;
        }
    }

    public bool TryEvolve(ISpeciesForm head, ISpeciesForm next, PKM pk, byte currentMaxLevel, byte levelMin, bool skipChecks, out EvoCriteria result)
    {
        var methods = GetForward(head.Species, head.Form);
        foreach (var method in methods.Span)
        {
            if (method.Species != next.Species)
                continue;
            var expectForm = method.GetDestinationForm(head.Form);
            if (next.Form != expectForm)
                continue;

            var chk = method.Check(pk, currentMaxLevel, levelMin, skipChecks);
            if (chk != EvolutionCheckResult.Valid)
                continue;

            result = Create(next.Species, next.Form, method, currentMaxLevel, levelMin);
            return true;
        }

        result = default;
        return false;
    }

    private static EvoCriteria Create(ushort species, byte form, EvolutionMethod method, byte currentMaxLevel, byte min) => new()
    {
        Species = species,
        Form = form,
        LevelMax = currentMaxLevel,
        Method = method.Method,

        // Temporarily store these and overwrite them when we clean the list.
        LevelMin = Math.Max(min, method.Level),
        LevelUpRequired = method.RequiresLevelUp ? (byte)1 : (byte)0,
    };
}
