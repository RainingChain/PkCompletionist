using PKHeX.Core;
using System.Collections.Generic;
using System;
using System.Buffers.Binary;

namespace PkCompletionist.Core;
partial class SAV3_Pinball : SAV_Dummy
{
    public SAV3_Pinball(byte[] data) : base(data)
    {
    }

    static public SAV3_Pinball? NewIfValid(byte[] data)
    {
        if (data.Length < 0x400)
            return null;

        // at offset 0x268, it contains the string "POKEPINAGB"
        var POKEPINAGB = new List<byte> { 0x50, 0x4F, 0x4B, 0x45, 0x50, 0x49, 0x4E, 0x41, 0x47, 0x42 };
        for (int i = 0; i < POKEPINAGB.Count; i++)
        {
            if (data[i + 0x268] != POKEPINAGB[i])
                return null;
        }
        return new SAV3_Pinball(data);
    }
    public override ushort MaxSpeciesID => 205;
    public override GameVersion Version => GameVersion.Unknown;

    static public int OFF_SAVE_START = 0x4;
    static public int OFF_POKEDEX_START = 0x4;
    static public int OFF_CHECKSUM = 0x272;
    static public int OFF_BACKUP = 0x2A0;
    static public int SZ_SAVE = 0x274;
    public override string OT => "PRS";

    protected override void SetChecksums()
    {
        var dbg_oldChecksum = BinaryPrimitives.ReadUInt16LittleEndian(Data.AsSpan(OFF_CHECKSUM, 2));

        // update checksum for save1
        BinaryPrimitives.WriteUInt16LittleEndian(Data.AsSpan(OFF_CHECKSUM, 2), 0);

        uint checksum = 0;
        for (var i = 0; i < SZ_SAVE; i += 2)
        {
            var off = OFF_SAVE_START + i;
            var val = BinaryPrimitives.ReadUInt16LittleEndian(Data.AsSpan(off, 2));
            checksum += val;
        }
        checksum = (checksum & 0xFFFF) + (checksum >> 16);
        checksum = ~((checksum >> 16) + checksum);

        ushort checksum_u16 = (ushort)checksum;
        BinaryPrimitives.WriteUInt16LittleEndian(Data.AsSpan(OFF_CHECKSUM, 2), checksum_u16);

        // copy save1 to saveBackup
        for (var i = 0; i < SZ_SAVE; i++)
            Data[i + OFF_BACKUP + OFF_SAVE_START] = Data[i + OFF_SAVE_START];
    }

}
