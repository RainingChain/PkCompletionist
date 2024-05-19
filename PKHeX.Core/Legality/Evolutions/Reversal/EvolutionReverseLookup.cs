using System.Collections.Generic;

namespace PKHeX.Core;

/// <summary>
/// Object storing a reversal path for evolution nodes.
/// </summary>
public sealed class EvolutionReverseLookup : IEvolutionLookup
{
    private readonly EvolutionNode[] Nodes;
    private readonly Dictionary<int, int> KeyLookup;
    private readonly ushort MaxSpecies;

    public EvolutionReverseLookup(ushort maxSpecies)
    {
        Nodes = new EvolutionNode[maxSpecies * 2];
        KeyLookup = new Dictionary<int, int>(maxSpecies);
        MaxSpecies = maxSpecies;
    }

    private void Register(in EvolutionLink link, int index)
    {
        ref var node = ref Nodes[index];
        node.Add(link);
    }

    public void Register(in EvolutionLink link, ushort species, byte form)
    {
        if (form == 0)
        {
            Register(link, species);
            return;
        }

        int index = GetOrAppendIndex(species, form);
        Register(link, index);
    }

    private int GetOrAppendIndex(ushort species, byte form)
    {
        int key = GetKey(species, form);
        if (KeyLookup.TryGetValue(key, out var index))
            return index;

        index = Nodes.Length - KeyLookup.Count - 1;
        KeyLookup.Add(key, index);
        return index;
    }

    private int GetIndex(ushort species, byte form)
    {
        if (species > MaxSpecies)
            return 0;
        if (form == 0)
            return species;
        int key = GetKey(species, form);
        return KeyLookup.TryGetValue(key, out var index) ? index : 0;
    }

    private static int GetKey(ushort species, byte form) => species | form << 11;
    public ref readonly EvolutionNode this[ushort species, byte form] => ref Nodes[GetIndex(species, form)];
}
