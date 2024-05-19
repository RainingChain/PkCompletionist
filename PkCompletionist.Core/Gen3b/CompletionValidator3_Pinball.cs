using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers.Binary;
using System.Reflection.Emit;

namespace PkCompletionist.Core;

internal class CompletionValidator3_Pinball : CompletionValidatorX
{
    public CompletionValidator3_Pinball(Command command, SAV3_Pinball sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;
    }

    static public SAV3_Pinball? NewIfValid(byte[] data)
    {
        if (data.Length < 0x400)
            return null;

        // at offset 0x260, it contains the string "POKEPINAGB"
        var POKEPINAGB = new List<byte> { 0x50, 0x4F, 0x4B, 0x45, 0x50, 0x49, 0x4E, 0x4, 0x47, 0x42 };
        for (int i = 0; i < POKEPINAGB.Count; i++)
        {
            if (data[i + 0x260] != POKEPINAGB[i])
                return null;
        }
        return new SAV3_Pinball(data);
    }

    public override void GenerateAll()
    {
        Generate_pokemon();
    }
    public override void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        for (ushort i = 1; i <= sav.MaxSpeciesID; i++)
            ow[i.ToString()] = this.sav.Data[SAV3_Pinball.OFF_POKEDEX_START + i - 1] == 4;
    }
}
