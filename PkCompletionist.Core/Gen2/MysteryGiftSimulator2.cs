using PKHeX.Core;
using System.Collections.Generic;

namespace PkCompletionist.Core;

/*
 gen2_mysteryGift
"C:\Users\samue\Game\Gameboy\ROMS\Pokemon Crystal.sav"
"C:\Users\samue\Game\Gameboy\ROMS\Pokemon Gold.sav"
"C:\Users\samue\Game\Gameboy\ROMS\Pokemon Crystal 2.sav"
"C:\Users\samue\Game\Gameboy\ROMS\Pokemon Gold 2.sav"
true
100
*/

internal class MysteryGiftSimulatorCmd2 : Command
{
    public static bool Execute(byte[] savAData, byte[] savBData, bool onlyPrintDecoOfPlayer1, int count)
    {
        var simulCmd = new MysteryGiftSimulatorCmd2();

        simulCmd.SetSavA(savAData, "");
        simulCmd.SetSavB(savBData, "");

        return MysteryGiftSimulator2.ExecuteXCmd(simulCmd, count, onlyPrintDecoOfPlayer1) == null;
    }
}

internal class MysteryGiftSimulator2 : Command
{

    public static string? ExecuteXCmd(Command cmd, int count, bool onlyPrintDecoOfPlayer1)
    {
        var savA = (SAV2?)cmd.savA;
        var savB = (SAV2?)cmd.savB;

        if (savA == null)
        {
            cmd.AddError("The first savefile is not generation 2.");
            return cmd.LastError;
        }
        if (savB == null)
        {
            cmd.AddError("The second savefile is not generation 2.");
            return cmd.LastError;
        }

        List<string> Msgs = new();

        var err = ExecuteX(count, savA, savB, onlyPrintDecoOfPlayer1, ref Msgs);

        foreach (var Msg in Msgs)
            cmd.AddMessage(Msg);

        if (err != null)
        {
            cmd.AddError(err);
            return cmd.LastError;
        }

        return null;
    }

    public static string? ExecuteX(int count, SAV2 savA, SAV2 savB, bool onlyPrintDecoOfPlayer1, ref List<string> Msgs)
    {
        var overwriteItems = count <= 1;

        for (int i = 0; i < count; i++)
        {
            var err = MysteryGiftSimulator2.Execute(savA, savB, overwriteItems, onlyPrintDecoOfPlayer1, ref Msgs);
            if (err != null)
                return err;
        }
        return null;
    }

    public static string? Execute(SAV2 savA, SAV2 savB, bool OverwriteItems, bool onlyPrintDecoOfPlayer1, ref List<string> Msgs)
    {
        if (savA.Data[0xBE3] != 0x00)
            return "Player 1 has not unlocked Mystery Gift.";

        if (savB.Data[0xBE3] != 0x00)
            return "Player 2 has not unlocked Mystery Gift.";

        if (OverwriteItems)
        {
            savA.SetEventFlag(1809, true);
            savB.SetEventFlag(1809, true);
            // Assumes international version
            //NO_PROD
            savA.Data[0xBE4] = 0;
            savB.Data[0xBE4] = 0;
        }

        if (!savA.GetEventFlag(1809))
            return "Player 1 has an item waiting at PK Center.";

        if (!savB.GetEventFlag(1809))
            return "Player 2 has an item waiting at PK Center.";

        var decoIdx1 = GetRandomDecoIdx(savB.TID16);
        var decoId1 = MysteryGiftDecoList[decoIdx1];
        if (RandomByte() % 2 == 0 && !savA.GetEventFlag(decoId1))
        {
            var name = MysteryGiftDecoNameList[decoIdx1];
            savA.SetEventFlag(decoId1, true);
            Msgs.Add($"Player 1 received: {name} (Idx {decoIdx1})");
        }
        else
        {
            var idx = GetRandomItemIdx(savB.TID16);
            var id = MysteryGiftItemList[idx];
            var name = MysteryGiftItemNameList[idx];
            savA.SetEventFlag(1809, false);
            savA.Data[0xBE4] = id;
            if (!onlyPrintDecoOfPlayer1)
                Msgs.Add($"Player 1 received: {name} (Idx {idx})");
        }

        var decoIdx2 = GetRandomDecoIdx(savA.TID16);
        var decoId2 = MysteryGiftDecoList[decoIdx2];

        if (RandomByte() % 2 == 0 && !savB.GetEventFlag(decoId2))
        {
            var name = MysteryGiftDecoNameList[decoIdx2];
            savB.SetEventFlag(decoId2, true);
            if (!onlyPrintDecoOfPlayer1)
                Msgs.Add($"Player 2 received: {name} (Idx {decoIdx2})");
        }
        else
        {
            var idx = GetRandomItemIdx(savA.TID16);
            var id = MysteryGiftItemList[idx];
            var name = MysteryGiftItemNameList[idx];
            savB.SetEventFlag(1809, false);
            savB.Data[0xBE4] = id;
            if (!onlyPrintDecoOfPlayer1)
                Msgs.Add($"Player 2 received: {name} (Idx {idx})");
        }

        return null;
    }

    static byte RandomByte()
    {
        System.Random rnd = new();
        return (byte)rnd.Next(0, 256);
    }

    static int GetRandomItemIdx(ushort tid)
    {
        if (RandomByte() >= 26)
        {
            var group = RandomByte() % 8;
            var oddBitIdx = new List<int>{7,0,1,2,3,4,5,6}[group];
            var oddIncr = (tid & (1 << oddBitIdx)) != 0 ? 1 : 0;
            return group * 2 + oddIncr;
        }

        if (RandomByte() >= 51)
        {
            var group = RandomByte() % 4;
            var oddBitIdx = new List<int> { 15,8,9,10 }[group];
            var oddIncr = (tid & (1 << oddBitIdx)) != 0 ? 1 : 0;
            return 16 + group * 2 + oddIncr;
        }

        if (RandomByte() >= 51)
        {
            var tidIncr = (tid & 0b111000_000_000_000) >> 12;
            return 16 + 8 + tidIncr;
        }

        return (tid & (1 << 15)) == 0 ? 32 : 33;
    }
    static int GetRandomDecoIdx(ushort tid)
    {
        if (RandomByte() >= 26)
        {
            var group = RandomByte() % 8;
            var oddBitIdx = new List<int> { 15, 8,9,10,11,12,13,14 }[group];
            var oddIncr = (tid & (1 << oddBitIdx)) != 0 ? 1 : 0;
            return group * 2 + oddIncr;
        }

        if (RandomByte() >= 51)
        {
            var group = RandomByte() % 4;
            var oddBitIdx = new List<int> {7,0,1,2 }[group];
            var oddIncr = (tid & (1 << oddBitIdx)) != 0 ? 1 : 0;
            return 16 + group * 2 + oddIncr;
        }

        if (RandomByte() >= 51)
        {
            var tidIncr = (tid & 0b1110000) >> 4;
            return 16 + 8 + tidIncr;
        }

        return (tid & (1 << 7)) == 0 ? 32 : 33;
    }


    static readonly List<byte> MysteryGiftItemList = new List<byte>
    {
        /*Berry*/173,
        /*PRZCureBerry*/78,
        /*MintBerry*/84,
        /*IceBerry*/80,

        /*BurntBerry*/79,
        /*PSNCureBerry*/74,
        /*GuardSpec*/41,
        /*XDefend*/51,

        /*XAttack*/49,
        /*BitterBerry*/83,
        /*DireHit*/44,
        /*XSpecial*/53,

        /*XAccuracy*/33,
        /*EonMail*/185,
        /*MorphMail*/186,
        /*MusicMail*/188,

        /*MiracleBerry*/109,
        /*GoldBerry*/174,
        /*Revive*/39,
        /*GreatBall*/4,

        /*SuperRepel*/42,
        /*MaxRepel*/43,
        /*Elixer*/65,
        /*Ether*/63,

        /*WaterStone*/24,
        /*FireStone*/22,
        /*LeafStone*/34,
        /*Thunderstone*/23,

        /*MaxEther*/64,
        /*MaxElixer*/21,
        /*MaxRevive*/40,
        /*ScopeLens*/140,

        /*HP_Up*/26,
        /*PP_Up*/62
    };


    static readonly List<string> MysteryGiftItemNameList = new List<string>
    {
        "Berry",
        "PRZCureBerry",
        "Mint Berry",
        "Ice Berry",
        "Burnt Berry",
        "PSNCureBerry",
        "Guard Spec",
        "X Defend",
        "X Attack",
        "Bitter Berry",
        "Dire Hit",
        "X Special",
        "X Accuracy",
        "Eon Mail",
        "Morph Mail",
        "Music Mail",
        "MiracleBerry",
        "Gold Berry",
        "Revive",
        "Great Ball",
        "Super Repel",
        "Max Repel",
        "Elixer",
        "Ether",
        "Water Stone",
        "Fire Stone",
        "Leaf Stone",
        "Thunderstone",
        "Max Ether",
        "Max Elixer",
        "Max Revive",
        "Scope Lens",
        "HP Up",
        "PP Up"
    };

    static readonly List<int> MysteryGiftDecoList = new List<int>
    {
        698,
        702,
        703,
        704,
        705,
        706,
        707,
        708,
        709,
        710,
        689,
        690,
        692,
        711,
        713,
        714,   //idx15
        
        684, 
        685,
        691,
        693,
        699,
        701,
        677,
        678, //idx23

        680,
        681,
        682,
        683,
        686,
        694,
        720,
        688,
        721,
        696
    };

    static readonly List<string> MysteryGiftDecoNameList = new List<string>
    {
        "Jigglypuff Doll",
        "Poliwag Doll",
        "Diglett Doll",
        "Staryu Doll",
        "Magikarp Doll",
        "Oddish Doll",
        "Gengar Doll",
        "Shellder Doll",
        "Grimer Doll",
        "Voltorb Doll",
        "Clefairy Poster",
        "Jigglypuff Poster",
        "Super NES",
        "Weedle Doll",
        "Geodude Doll",
        "Machop Doll", //idx15

        "Magnaplant",
        "Tropicplant",
        "NES",
        "Nintendo64",
        "Bulbasaur Doll",
        "Squirtle Doll",
        "Pink Bed",
        "Polkadot Bed", // idx23

        "Red Carpet",
        "Blue Carpet",
        "Yellow Carpet", //idx26
        "Green Carpet",
        "Jumboplant",
        "Virtual Boy",
        "Big Onix Doll",
        "Pikachu Poster",
        "Big Lapras Doll",
        "Surf Pikachu Doll"
    };
}
