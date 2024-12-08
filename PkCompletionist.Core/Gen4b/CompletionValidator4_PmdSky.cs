using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers.Binary;
using System.Reflection.Emit;
using System.IO;
using System.Security.Cryptography;

namespace PkCompletionist.Core;


internal class CompletionValidator4_PmdSky : CompletionValidatorX
{
    public CompletionValidator4_PmdSky(Command command, SAV4_PmdSky sav, Objective objective) : base(command, sav, objective)
    {
        this.sav = sav;
        this.ownedMonList = this.GetObtainedPokemons();
    }
    new SAV4_PmdSky sav;

    private List<int> ownedMonList = new();

    public override void GenerateAll()
    {
        Generate_pokemon();
        Generate_pokemonForm();
        Generate_item();
        Generate_marowakDojo();
        Generate_rank();
        Generate_dungeon();
        Generate_adventureLog();
        Generate_specialEpisode();
        Generate_progressIcon();
    }
    public List<int> GetObtainedPokemons()
    {
        var ownedList = new List<int>();
        return ownedList;
    }
    public override void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        var unobtainabled = new List<int> { 377, 378, 379 };
        for (ushort i = 1; i <= 416; i++)
        {
            if (unobtainabled.Contains(i))
                continue;

            ow[i.ToString()] = this.ownedMonList.Contains(i);
        }
    }
    public void Generate_pokemonForm()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemonForm"] = ow;
    }

    public List<int> GetObtainedItems()
    {
        var ownedList = new List<int>();

        return ownedList;
    }
    public override void Generate_item()
    {
        var ow = new Dictionary<string, bool>();
        owned["item"] = ow;

    }

    public bool HasCompletedMarowakDojo(MarowakDojo dojo)
    {
        var byteIdx = (int)dojo / 8;
        var bitIdx = (int)dojo % 8;
        return false;
    }

    public void Generate_marowakDojo()
    {
        var ow = new Dictionary<string, bool>();
        owned["MarowakDojo"] = ow;

    }

    public void Generate_dungeon()
    {
        var ow = new Dictionary<string, bool>();
        owned["dungeon"] = ow;
    }
    public void Generate_adventureLog()
    {
        var ow = new Dictionary<string, bool>();
        owned["adventureLog"] = ow;
    }
    
    public void Generate_specialEpisode()
    {
        var ow = new Dictionary<string, bool>();
        owned["specialEpisode"] = ow;
    }

    public void Generate_progressIcon()
    {
        var ow = new Dictionary<string, bool>();
        owned["progressIcon"] = ow;
    }

    public void Generate_rank()
    {
        var ow = new Dictionary<string, bool>();
        owned["rank"] = ow;

    }
}

enum MarowakDojo
{
}