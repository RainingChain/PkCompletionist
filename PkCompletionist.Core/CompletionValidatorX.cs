using PKHeX.Core;
using System.Collections.Generic;
using System.Linq;

namespace PkCompletionist.Core;
partial class CompletionValidatorX
{
    protected SaveFile sav;
    public Dictionary<string, Dictionary<string, bool>> owned = new ();
    protected bool living = false;
    protected List<int> unobtainableItems = new ();
    Command command;

    protected List<ushort> OwnedPkms = new List<ushort>();
    protected List<ushort> OwnedPkmsTID = new List<ushort>();
    protected List<int> OwnedItems = new List<int>();

    public CompletionValidatorX(Command command, SaveFile sav, bool living) 
    {
        this.command = command;
        this.sav = sav;
        this.living = living;

        var pkms = sav.GetAllPKM();
        foreach (var pkm in pkms)
        {
            OwnedPkms.Add(pkm.Species);
            if (pkm.TID16 == sav.TID16)
                OwnedPkmsTID.Add(pkm.Species);
            if (pkm.HeldItem != 0)
                OwnedItems.Add(pkm.HeldItem);
        }

        foreach (var pouch in sav.Inventory)
        {
            foreach(var item in pouch.Items)
            {
                if (item.Index != 0)
                    OwnedItems.Add(item.Index);
            }
        }
    }


    public void AddMessage(string msg)
    {
        this.command.AddMessage(msg);
    }

    public virtual void GenerateAll()
    {
        Generate_pokemon();
        Generate_item();
    }

    public virtual void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        for (ushort i = 1; i <= sav.MaxSpeciesID; i++)
            ow[i.ToString()] = this.living ? HasPkm(i) : sav.GetCaught(i);
    }

    public virtual void Generate_item()
    {
        var ow = new Dictionary<string, bool>();
        owned["item"] = ow;

        for (ushort i = 1; i <= sav.MaxItemID; i++)
        {
            if (this.unobtainableItems.Contains(i))
                continue;
            ow[i.ToString()] = HasItem(i);
        }
    }
    public bool HasPkmForm(ushort species, byte form)
    {
        if (!HasPkm(species))
            return false;

        var pkms = sav.GetAllPKM();
        return pkms.FirstOrDefault(pkm =>
        {
            if (pkm.Species != species)
                return false;
            return pkm.Form == form;
        }) != null;
    }

    public bool HasPkm(ushort speciesId)
    {
        return OwnedPkms.Contains(speciesId);
    }

    public bool HasPkmWithTID(ushort speciesId)
    {
        return OwnedPkmsTID.Contains(speciesId);
    }

    public bool HasItem(ushort itemId)
    {
        return OwnedItems.Contains(itemId);
    }

}
