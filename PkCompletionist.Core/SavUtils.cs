using PKHeX.Core;
using System.Linq;
using System;
using System.Reflection;

namespace PkCompletionist.Core;

internal class SavUtils
{
    public static bool HasPkm(SaveFile sav, ushort speciesId)
    {
        var pkms = sav.GetAllPKM();
        if (pkms == null)
            return false;
        return pkms.Any(pkm => pkm.Species == speciesId);
    }

    public static bool HasPkmWithTID(SaveFile sav, ushort speciesId)
    {
        var pkms = sav.GetAllPKM();
        if (pkms == null)
            return false;
        return pkms.Any(pkm => pkm.Species == speciesId && pkm.TID16 == sav.TID16);
    }

    public static bool HasItem(SaveFile sav, ushort itemId)
    {
        return HasItemInPouch(sav, itemId) || HasItemOnPkm(sav, itemId);
    }

    public static bool HasItemOnPkm(SaveFile sav, ushort itemId)
    {
        var pkms = sav.GetAllPKM();
        return pkms.Any(pkm => pkm.HeldItem == itemId);
    }

    public static bool HasItemInPouch(SaveFile sav, ushort itemId)
    {
        var pouches = sav.Inventory;
        foreach (var pouch in pouches)
        {
            int alreadyThereIdx = System.Array.FindIndex(pouch.Items, z => z.Index == itemId);
            if (alreadyThereIdx >= 0)
                return true;
        }
        return false;
    }

    public static string? AddItem(SaveFile sav, ushort itemId, bool AddOnlyIfNotThere = true)
    {
        if (AddOnlyIfNotThere && HasItem(sav, itemId))
            return "Error: You already have that item.";

        var pouches = sav.Inventory;
        var pouch = pouches.FirstOrDefault(pouch =>
        {
            return pouch.GetAllItems().Contains(itemId);
        });
        if (pouch == null)
            return "Error: No valid pouch found for that item.";

        int addedCount = pouch.GiveItem(sav, itemId, 1);
        if (addedCount <= 0)
            return "Error: Your item pouch is full.";

        sav.Inventory = pouches;
        return null;
    }

    public static MysteryGift? LoadMysteryGift(string name)
    {
        byte[] data = Util.GetBinaryResource(name);
        string[] exts = name.Split('.');
        if (exts.Length != 2)
            return null;
        return MysteryGift.GetMysteryGift(data, "." + exts[1]);
    }

    public static PKM? LoadPk(string name)
    {
        byte[] data = Util.GetBinaryResource(name);
        string[] exts = name.Split('.');
        if (exts.Length != 2)
            return null;
        if (FileUtil.TryGetPKM(data, out var pkm, "." + exts[1]))
            return pkm;
        return null;
    }
    public static string? AddPkm(SaveFile sav, string pkmFile, bool onlyIfNotOwned = true)
    {
        var pkm = LoadPk(pkmFile);
        if (pkm == null)
            return "Internal error: invalid pkmFile " + pkmFile;

        var isEmpty = (PKM pkm) => pkm.Species <= 0;
        int slot = sav.FindSlotIndex(isEmpty, sav.SlotCount);

        if (slot < 0)
            return "Error: All pokemon boxes are full. Free a box slot to fix the issue.";

        if (onlyIfNotOwned)
        {
            var ivs1 = new int[6];
            pkm.GetIVs(ivs1);

            var ivs2 = new int[6];

            var allPkms = sav.GetAllPKM();
            var alreadyOwned = allPkms.Any(pkm2 =>
            {
                pkm2.GetIVs(ivs2);
                return pkm.Species == pkm2.Species && pkm.Form == pkm2.Form && pkm.TID16 == pkm2.TID16 && pkm.OT_Name == pkm2.OT_Name && Enumerable.SequenceEqual(ivs1, ivs2);
            });
            if (alreadyOwned)
                return "Error: You already have that pokemon.";
        }

        sav.SetBoxSlotAtIndex(pkm, slot);
        return null;
    }

    static public string? AddMysteryGift(SaveFile sav, string path)
    {
        var giftToAdd = LoadMysteryGift(path);
        if (giftToAdd == null || giftToAdd is not DataMysteryGift)
            return $"Internal error: invalid mystery gift. ({path})";

        var gift = (DataMysteryGift)giftToAdd;

        if (!sav.CanReceiveGift(gift))
            return "Error: The mystery gift can't be received in this game.";

        if (!gift.IsCardCompatible(sav, out string msg))
            return msg;

        var gifts = sav.GiftAlbum.Gifts;
        int lastUnfilled = GetLastUnfilledByType(gift, gifts);
        int index = 0;
        if (lastUnfilled > -1 && lastUnfilled < index)
            index = lastUnfilled;
        if (gift is PCD { IsLockCapsule: true })
            index = 11;

        var other = gifts[index];
        if (gift is PCD { CanConvertToPGT: true } pcd && other is PGT)
        {
            gift = pcd.Gift;
        }
        else if (gift.Type != other.Type)
        {
            return $"Internal error: {gift.Type} != {other.Type}";
        }
        else if (gift is PCD g && (g is { IsLockCapsule: true } != (index == 11)))
        {
            return "Internal error: slot not valid.";
        }

        gifts[index] = (DataMysteryGift)gift.Clone();

        var flags = sav.GiftAlbum.Flags;
        flags[giftToAdd.CardID] = true;
        sav.GiftAlbum = new(gifts, flags);

        return null;
    }
    private static int GetLastUnfilledByType(DataMysteryGift gift, ReadOnlySpan<DataMysteryGift> album)
    {
        for (int i = 0; i < album.Length; i++)
        {
            var exist = album[i];
            if (!exist.Empty)
                continue;
            if (exist.Type != gift.Type)
                continue;
            return i;
        }
        return -1;
    }
}
