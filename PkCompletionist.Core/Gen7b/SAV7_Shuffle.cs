using PKHeX.Core;
using System.Collections.Generic;
using System;
using System.Buffers.Binary;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PkCompletionist.Core;
partial class SAV7_Shuffle : SAV_Dummy
{
    public SAV7_Shuffle(byte[] data) : base(data)
    {
    }

    static public SAV7_Shuffle? NewIfValid(byte[] data)
    {
        if (data.Length != 74807)
            return null;

        var START = new List<byte> { 0x09, 0x00, 0x00, 0x00, 0x40 };
        for (int i = 0; i < START.Count; i++)
        {
            if (data[i] != START[i])
                return null;
        }
        return new SAV7_Shuffle(data);
    }

    public override ushort MaxSpeciesID => 802;
    public override GameVersion Version => GameVersion.Unknown;

    static public int OFF_SAVE_START = 0x4;
    static public int OFF_POKEDEX_START = 0x4;
    static public int OFF_CHECKSUM = 0x272;
    static public int OFF_BACKUP = 0x2A0;
    static public int SZ_SAVE = 0x274;
    public override string OT => "SHUFFLE";

    public bool HasMon(int id)
    {
        var bit_index = IdToIndex(id) % 8;
        var byte_offset = IdToIndex(id) / 8;

        foreach (int offset in new[] { 0xE6, 0x546, 0x5E6 })
        {
            if (!GetBit(offset + byte_offset, bit_index))
                return false;
        }
        return true;
    }

    public bool SetCaughtMon(int id)
    {
        var bit_index = IdToIndex(id) % 8;
        var byte_offset = IdToIndex(id) / 8;

        foreach (int offset in new[] { 0xE6, 0x546, 0x5E6 })
        {
            SetBit(offset + byte_offset, bit_index);
        }
        return true;
    }

    private bool GetBit(int byte_offset, int bit_index)
    {
        return ((Data[byte_offset] >> bit_index) & 1) != 0;
    }
    private void SetBit(int byte_offset, int bit_index)
    {
        Data[byte_offset] |= (byte)(1 << bit_index);
    }

    private int IdToIndex(int id)
    {
        return id + 5;
    }
}