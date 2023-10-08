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

    public CompletionValidatorX(Command command, SaveFile sav, bool living) 
    {
        this.command = command;
        this.sav = sav;
        this.living = living;
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

        if (this.living)
        {
            var pkms = sav.GetAllPKM();
            for (ushort i = 1; i <= sav.MaxSpeciesID; i++)
                ow[i.ToString()] = pkms.Any(pkm => pkm.Species == i);
        }
        else
        {
            for (ushort i = 1; i <= sav.MaxSpeciesID; i++)
                ow[i.ToString()] = sav.GetCaught(i);
        }
    }

    public virtual void Generate_item()
    {
        var ow = new Dictionary<string, bool>();
        owned["item"] = ow;

        for (ushort i = 1; i <= sav.MaxItemID; i++)
        {
            if (this.unobtainableItems.Contains(i))
                continue;
            ow[i.ToString()] = SavUtils.HasItem(sav, i);
        }
    }
    public bool HasForm(int species, byte form)
    {
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
        return SavUtils.HasPkm(sav, speciesId);
    }


}
