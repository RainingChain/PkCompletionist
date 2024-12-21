using PKHeX.Core;
using System;
using System.Collections.Generic;

namespace PkCompletionist.Core;

//https://github.com/evandixon/SkyEditor.SaveEditor/blob/master/SkyEditor.SaveEditor/MysteryDungeon/Explorers/SkyActivePokemon.cs

public class SkyOffsets
{
    public int BackupSaveStart => 0xC800;
    public int ChecksumEnd => 0xB65A;
    public int QuicksaveStart => 0x19000;
    public int QuicksaveChecksumStart => 0x19004;
    public int QuicksaveChecksumEnd => 0x1E7FF;
    public int TeamNameStart => 0x994E * 8;
    public int TeamNameLength => 10;
    public int HeldMoney => 0x990C * 8 + 6;
    public int SpEpisodeHeldMoney => 0x990F * 8 + 6;
    public int StoredMoney => 0x9915 * 8 + 6;
    public int NumberOfAdventures => 0x8B70 * 8;
    public int ExplorerRank => 0x9958 * 8;
    public int StoredItemOffset => 0x8E0C * 8 + 6;
    public int HeldItemOffset => 0x8BA2 * 8;
    public int StoredPokemonOffset => 0x464 * 8;
    public int StoredPokemonLength => 362;
    public int StoredPokemonCount => 720;
    public int ActivePokemon1RosterIndexOffset => 0x83D1 * 8 + 1;
    public int ActivePokemon2RosterIndexOffset => 0x83D3 * 8 + 1;
    public int ActivePokemon3RosterIndexOffset => 0x83D5 * 8 + 1;
    public int ActivePokemon4RosterIndexOffset => 0x83D7 * 8 + 1;
    public int ActivePokemonOffset => 0x83D9 * 8 + 1;
    public int SpActivePokemonOffset => 0x84F4 * 8 + 2;
    public int ActivePokemonLength => 546;
    public int ActivePokemonCount => 4;
    public int QuicksavePokemonCount => 20;
    public int QuicksavePokemonLength => 429 * 8;
    public int QuicksavePokemonOffset => 0x19000 * 8 + (0x3170 * 8);
    public int OriginalPlayerID => 0xBE * 8;
    public int OriginalPartnerID => 0xC0 * 8;
    public int OriginalPlayerName => 0x13F * 8;
    public int OriginalPartnerName => 0x149 * 8;
    public int WindowFrameType => 0x995F * 8 + 5;
    public int ItemShop1Offset => 0x98CA * 8 + 6;
    public int ItemShopLength => 22;
    public int ItemShop1Number => 8;
    public int ItemShop2Offset => 0x98E0 * 8 + 6;
    public int ItemShop2Number => 4;
    public int AdventureLogOffset => 0x9958 * 8;
    public int AdventureLogLength => 447;
    public int CroagunkShopOffset => 0xB475 * 8;
    public int CroagunkShopLength => 11;
    public int CroagunkShopNumber => 8;
}

class SAV4_PmdSky : SAV_Dummy
{
    public SAV4_PmdSky(byte[] data) : base(data)
    {
        Bits = new BitBlock(data);
    }
    public BitBlock Bits;
    public SkyOffsets Offsets = new SkyOffsets();

    public BitBlock bitBlock_SlowCopy
    {
        get { return new BitBlock(this.Data); }
        set { this.Data = value.ToByteArray(); }
    }

    public List<int> GetStoredPokemon(int baseOffset)
    {
        var storedPokemon = new List<int>();
        for (int i = 0; i < Offsets.StoredPokemonCount; i++)
        {
            var range = Bits.GetRange(baseOffset * 8 + Offsets.StoredPokemonOffset + i * Offsets.StoredPokemonLength, Offsets.StoredPokemonLength);
            //if (!range[0]) //invalid
            //    break;

            var genderAndId = range.GetInt(0, 8, 11);
            var id = genderAndId >= 600 ? genderAndId - 600 : genderAndId;

            storedPokemon.Add(id);
        }
        return storedPokemon;
    }

    public List<int> GetOwnedItems(int baseOffset)
    {
        var items = new List<int>();

        //stored
        var ids = Bits.GetRange(baseOffset * 8 + Offsets.StoredItemOffset, 11 * 1000);
        var parameters = Bits.GetRange(baseOffset * 8 + Offsets.StoredItemOffset + (11 * 1000), 11 * 1000);
        for (int i = 0; i < 1000; i++)
        {
            var id = ids.GetNextInt(11);
            var param = parameters.GetNextInt(11);
            if (id > 0)
                items.Add(id);
            else
                break;
        }

        // inventory
        for (int i = 0; i < 50; i++)
        {
            var range = Bits.GetRange(baseOffset * 8 + Offsets.HeldItemOffset + (i * 33), 33);
            range.Position = 0;
            if (!range[0])
                break;
            var id = range.GetInt(0, 19, 11);
            if (id > 0)
                items.Add(id);
        }

        return items;
    }

    public override string OT => "PmdSky";
    public bool IsEU = false;

    static public SAV4_PmdSky? NewIfValid(byte[] data, string Hint)
    {
        if (data.Length < 0x410)
            return null;

        // at offset 0x004, it contains the string "POKE_DUN_SORA"
        var POKE_DUN_SORA = new List<byte> { 0x50, 0x4F, 0x4B, 0x45, 0x5F, 0x44, 0x55, 0x4E, 0x5F, 0x53, 0x4F, 0x52, 0x41};
        for (int i = 0; i < POKE_DUN_SORA.Count; i++)
        {
            if (data[i + 0x004] != POKE_DUN_SORA[i])
                return null;
        }

        return new SAV4_PmdSky(data);
    }

    protected override void SetChecksums()
    {
        var PrimaryChecksum = Calculate32BitChecksum(Bits, 4, Offsets.ChecksumEnd);
        var SecondaryChecksum = Calculate32BitChecksum(Bits, Offsets.BackupSaveStart + 4, Offsets.BackupSaveStart + Offsets.ChecksumEnd);
        var QuicksaveChecksum = Calculate32BitChecksum(Bits, Offsets.QuicksaveChecksumStart, Offsets.QuicksaveChecksumEnd);

        var bits = this.bitBlock_SlowCopy;
        /*
        var list = new List<int>();
        for (var i = 0; i < 447; i += 2)
            list.Add(bits.GetInt(Offsets.AdventureLogOffset / 8 + i, 0, 16));

        var oldPrimaryChecksum = bits.GetUInt(0, 0, 32);
        var oldSecondaryChecksum = bits.GetUInt(Offsets.BackupSaveStart, 0, 32);
        var oldQuicksaveChecksum = bits.GetUInt(Offsets.QuicksaveStart, 0, 32);
        Console.WriteLine("oldPrimaryChecksum={0} . PrimaryChecksum={1}", oldPrimaryChecksum, PrimaryChecksum);
        Console.WriteLine("oldSecondaryChecksum={0} . SecondaryChecksum={1}", oldSecondaryChecksum, SecondaryChecksum);
        Console.WriteLine("oldQuicksaveChecksum={0} . QuicksaveChecksum={1}", oldQuicksaveChecksum, QuicksaveChecksum);
        */

        bits.SetUInt(0, 0, 32, PrimaryChecksum);
        bits.SetUInt(Offsets.BackupSaveStart, 0, 32, SecondaryChecksum);
        bits.SetUInt(Offsets.QuicksaveStart, 0, 32, QuicksaveChecksum);

        this.bitBlock_SlowCopy = bits;
    }

    private static uint Calculate32BitChecksum(BitBlock bits, int startIndex, int endIndex)
    {
        ulong sum = 0;
        for (int i = startIndex; i <= endIndex; i += 4)
            sum += bits.GetUInt(i, 0, 32) & 0xFFFFFFFF;
        return (uint)(sum & 0xFFFFFFFF);
    }
}
