using PKHeX.Core;
using System.Collections.Generic;
using System;
using System.Buffers.Binary;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace PkCompletionist.Core;

enum Enhancement
{
    MegaSpeedup,
    RaiseMaxLevel,
    LevelUp,
    ExpBoosterS,
    ExpBoosterM,
    ExpBoosterL,
    SkillBoosterS,
    SkillBoosterM,
    SkillBoosterL,
    SkillSwapper,

}

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
        var bit_index = MonIdToIndex(id) % 8;
        var byte_offset = MonIdToIndex(id) / 8;

        foreach (int offset in new[] { 0xE6, 0x546, 0x5E6 })
        {
            if (!GetBit(offset + byte_offset, bit_index))
                return false;
        }
        return true;
    }

    static List<int> MISSION_COUNT_BY_CARD = new List<int>{ 
        3, 8, 8, 5, 5, 7, 5, 7, 7, 5, 5, 6, 7, 3, 5, 7, 3, 4, 4
    };


    public bool HasCompletedMissionCard(int card)
    {

        for (int missionIdx = 0; missionIdx < MISSION_COUNT_BY_CARD[card]; missionIdx++)
        {
            var byte_offset = 0xB6FC + CardIdToIndex(card, missionIdx) / 8;
            var bit_index = CardIdToIndex(card, missionIdx) % 8;
            if (((this.Data[byte_offset] >> bit_index) & 1) == 0)
                return false;
        }
        return true;
    }
    
    public bool SetCaughtMon(int id)
    {
        var bit_index = MonIdToIndex(id) % 8;
        var byte_offset = MonIdToIndex(id) / 8;

        foreach (int offset in new[] { 0xE6, 0x546, 0x5E6 })
        {
            SetBit(offset + byte_offset, bit_index);
        }
        return true;
    }

    public int GetEnhancementCount(Enhancement id)
    {
        return (Data[0x2D4C + (int)id] >> 1) & 0x7F;
    }

    public bool HasMegaStone(int monId, int stoneIdx = 0)
    {
        var byte_index = 0x406 + (monId + 2) / 4;
        var bit_index = (5 + (monId * 2)) % 8;

        var owned_stones = (Data[byte_index] >> bit_index);
        var wanted_stone_mask = stoneIdx == 0 ? 0b01 : 0b10;
        return (owned_stones & wanted_stone_mask) != 0;
    }

    private bool GetBit(int byte_offset, int bit_index)
    {
        return ((Data[byte_offset] >> bit_index) & 1) != 0;
    }
    private void SetBit(int byte_offset, int bit_index)
    {
        Data[byte_offset] |= (byte)(1 << bit_index);
    }

    private int MonIdToIndex(int id)
    {
        return id + 5;
    }
    private int CardIdToIndex(int id, int mission)
    {
        return id * 10 + mission;
    }
}