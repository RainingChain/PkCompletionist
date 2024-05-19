using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers.Binary;
using System.Reflection.Emit;

namespace PkCompletionist.Core;

class SAV1_Pinball : SAV_Dummy
{
    public SAV1_Pinball(byte[] data) : base(data)
    {
    }
    public override string OT => "PINBALL";

    static public SAV1_Pinball? NewIfValid(byte[] data, string Hint)
    {
        if (Hint != "Pinball")
            return null;

        return new SAV1_Pinball(data);
    }
}

internal class CompletionValidator1_Pinball : CompletionValidatorX
{
    public CompletionValidator1_Pinball(Command command, SAV1_Pinball sav, bool living) : base(command, sav, living)
    {
    }


    public override void GenerateAll()
    {
        Generate_pokemon();
    }
    public override void Generate_pokemon()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemon"] = ow;

        for (ushort i = 1; i <= 151; i++)
            ow[i.ToString()] = this.sav.Data[0x10C + i - 1] == 3;
    }
}
