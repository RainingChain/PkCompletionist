using System.Collections.Generic;

namespace PkCompletionist.Core;

public class RBOffsets
{
    // Checksums
    public virtual int ChecksumEnd => 0x57D0;
    public virtual int BackupSaveStart => 0x6000;

    // General
    public virtual int TeamNameStart => 0x4EC8 * 8;
    public virtual int TeamNameLength => 10;
    public virtual int BaseTypeOffset => 0x67 * 8;
    public virtual int HeldMoneyOffset => 0x4E6C * 8;
    public virtual int HeldMoneyLength => 24;
    public virtual int StoredMoneyOffset => 0x4E6F * 8;
    public virtual int StoredMoneyLength => 24;
    public virtual int RescuePointsOffset => 0x4ED3 * 8;
    public virtual int RescuePointsLength => 32;

    public virtual int FriendAreaUnlockedOffset => 0x4ED8;
    public virtual int MakuhitaDojoCompletedOffset => 0xAA;

    // Stored Items
    public virtual int StoredItemOffset => 0x4D2B * 8 - 2;
    public virtual int StoredItemCount => 239;

    // Held Items
    public virtual int HeldItemOffset => 0x4CF0 * 8;
    public virtual int HeldItemCount => 20;
    public virtual int HeldItemLength => 23;

    // Stored Pokemon
    public virtual int StoredPokemonOffset => (0x5B3 * 8 + 3) - (323 * 9);
    public virtual int StoredPokemonLength => 323;
    public virtual int StoredPokemonCount => 407 + 6;

    // Story
    public virtual int StoryOffset => 0x74;

    public virtual int PokemonEvolvedCountOffset => 0x4EEA;
}


public class RBEUOffsets : RBOffsets
{
    // General
    public override int TeamNameStart => 0x4ECC * 8;
    public override int HeldMoneyOffset => 0x4E70 * 8;
    public override int StoredMoneyOffset => 0x4E73 * 8;
    public override int RescuePointsOffset => 0x4ED7 * 8;

    // Stored Items
    public override int StoredItemOffset => 0x4D2F * 8 - 2;

    // Held Items
    public override int HeldItemOffset => 0x4CF4 * 8;

    // Stored Pokemon
    public override int StoredPokemonOffset => (0x5B7 * 8 + 3) - (323 * 9);
}


class SAV3_PmdRescueTeam : SAV_Dummy
{
    public SAV3_PmdRescueTeam(byte[] data) : base(data)
    {
        var bits = new BitBlock(data);
        var checkSum = Calculate32BitChecksum(bits, 4, 0x57D0);
        this.IsEU = checkSum - 1 == bits.GetUInt(0, 0, 32); // EU checksum has -1
        this.off = IsEU ? new RBEUOffsets() : new RBOffsets();
    }
    public override string OT => "PmdRescueTeam";
    public bool IsEU = false;

    static public SAV3_PmdRescueTeam? NewIfValid(byte[] data, string Hint)
    {

        if (data.Length < 0x410)
            return null;

        // at offset 0x404, it contains the string "POKE_DUNGEON"
        var POKE_DUNGEON = new List<byte> { 0x50, 0x4F, 0x4B, 0x45, 0x5F, 0x44, 0x55, 0x4E, 0x47, 0x45, 0x4F, 0x4E};
        for (int i = 0; i < POKE_DUNGEON.Count; i++)
        {
            if (data[i + 0x404] != POKE_DUNGEON[i])
                return null;
        }

        return new SAV3_PmdRescueTeam(data);
    }

    public BitBlock bitBlock_SlowCopy
    {
        get { return new BitBlock(this.Data); }
        set { this.Data = value.ToByteArray(); }
    }

    public RBOffsets off;



    protected override void SetChecksums()
    {
        var bits = this.bitBlock_SlowCopy;
        var PrimaryChecksum = Calculate32BitChecksum(bits, 4, off.ChecksumEnd);
        bits.SetUInt(0, 0, 32, PrimaryChecksum);

        this.bitBlock_SlowCopy = bits;
    }

    public static uint Calculate32BitChecksum(BitBlock bits, int startIndex, int endIndex)
    {
        ulong sum = 0;
        for (int i = startIndex; i <= endIndex; i += 4)
        {
            sum += bits.GetUInt(i, 0, 32) & 0xFFFFFFFF;
        }
        return (uint)(sum & 0xFFFFFFFF);
    }    
}
