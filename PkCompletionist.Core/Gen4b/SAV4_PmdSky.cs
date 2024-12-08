using System.Collections.Generic;

namespace PkCompletionist.Core;

//https://github.com/evandixon/SkyEditor.SaveEditor/blob/master/SkyEditor.SaveEditor/MysteryDungeon/Explorers/SkyActivePokemon.cs

class SAV4_PmdSky : SAV_Dummy
{
    public SAV4_PmdSky(byte[] data) : base(data)
    {

    }
    public override string OT => "PmdSky";
    public bool IsEU = false;

    static public SAV3_PmdRescueTeam? NewIfValid(byte[] data, string Hint)
    {
        //NO_PROD
        if (data.Length < 0x410)
            return null;

        // at offset 0x404, it contains the string "POKE_DUNGEON"
        var POKE_DUNGEON = new List<byte> { 0x50, 0x4F, 0x4B, 0x45, 0x5F, 0x44, 0x55, 0x4E, 0x47, 0x45, 0x4F, 0x4E };
        for (int i = 0; i < POKE_DUNGEON.Count; i++)
        {
            if (data[i + 0x404] != POKE_DUNGEON[i])
                return null;
        }

        return new SAV3_PmdRescueTeam(data);
    }
}
