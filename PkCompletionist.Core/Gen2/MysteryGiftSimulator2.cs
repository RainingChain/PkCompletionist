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

internal class MysteryGiftSimulator2 : Command
{
    MysteryGiftSimulator2(bool onlyPrintDecoOfPlayer1)
    {
        this.onlyPrintDecoOfPlayer1 = onlyPrintDecoOfPlayer1;
    }

    bool onlyPrintDecoOfPlayer1 = false;

    public static bool Execute(byte[] savA, byte[] savB, bool onlyPrintDecoOfPlayer1, int count)
    {
        var simul = new MysteryGiftSimulator2(onlyPrintDecoOfPlayer1);
        if (simul.SetSavA(savA) == null)
            return false;
        if (simul.SetSavA(savB) == null)
            return false;

        if (count <= 1)
            return simul.Execute(false);

        for (int i = 0; i < count; i++)
        {
            if (!simul.Execute(true))
                return false;
        }
        return true;
    }

    byte RandomByte()
    {
        System.Random rnd = new();
        return (byte)rnd.Next(0, 256);
    }

    int GetRandomItemIdx(ushort tid)
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
    int GetRandomDecoIdx(ushort tid)
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

    public bool Execute(bool OverwriteItems)
    {
        SAV2? sav1 = (SAV2?)savA;
        SAV2? sav2 = (SAV2?)savB;
        if (sav1 == null)
        {
            AddError("First save file is not generation 2.");
            return false;
        }
        if (sav2 == null)
        {
            AddError("Second save file is not generation 2.");
            return false;
        }

        if (sav1.Data[0xBE3] != 0x00)
        {
            AddError("Player 1 has not unlocked Mystery Gift.");
            return false;
        }

        if (sav2.Data[0xBE3] != 0x00)
        {
            AddError("Player 2 has not unlocked Mystery Gift.");
            return false;
        }

        if (OverwriteItems)
        {
            sav1.SetEventFlag(1809, true);
            sav2.SetEventFlag(1809, true);
            // Assumes international version
            sav1.Data[0xBE4] = 0;
            sav2.Data[0xBE4] = 0;
        }

        if (!sav1.GetEventFlag(1809))
        {
            AddMessage("Player 1 has an item waiting at PK Center.");
            return false;
        }

        if (!sav2.GetEventFlag(1809))
        {
            AddMessage("Player 2 has an item waiting at PK Center.");
            return false;
        }

        var decoIdx1 = GetRandomDecoIdx(sav2.TID16);
        var decoId1 = MysteryGiftDecoList[decoIdx1];
        if (RandomByte() % 2 == 0 && !sav1.GetEventFlag(decoId1))
        {
            var name = MysteryGiftDecoNameList[decoIdx1];
            sav1.SetEventFlag(decoId1, true);
            AddMessage($"Player 1 received: {name} (Idx {decoIdx1})");
        }
        else
        {
            var idx = GetRandomItemIdx(sav2.TID16);
            var id = MysteryGiftItemList[idx];
            var name = MysteryGiftItemNameList[idx];
            sav1.SetEventFlag(1809, false);
            sav1.Data[0xBE4] = id;
            if (!onlyPrintDecoOfPlayer1)
                AddMessage($"Player 1 received: {name} (Idx {idx})");
        }


        var decoIdx2 = GetRandomDecoIdx(sav1.TID16);
        var decoId2 = MysteryGiftDecoList[decoIdx2];

        if (RandomByte() % 2 == 0 && !sav2.GetEventFlag(decoId2))
        {
            var name = MysteryGiftDecoNameList[decoIdx2];
            sav2.SetEventFlag(decoId2, true);
            if (!onlyPrintDecoOfPlayer1)
                AddMessage($"Player 2 received: {name} (Idx {decoIdx2})");
        }
        else
        {
            var idx = GetRandomItemIdx(sav1.TID16);
            var id = MysteryGiftItemList[idx];
            var name = MysteryGiftItemNameList[idx];
            sav2.SetEventFlag(1809, false);
            sav2.Data[0xBE4] = id;
            if (!onlyPrintDecoOfPlayer1)
                AddMessage($"Player 2 received: {name} (Idx {idx})");
        }

        return true;
    }

    readonly List<byte> MysteryGiftItemList = new List<byte>
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


    readonly List<string> MysteryGiftItemNameList = new List<string>
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

    readonly List<int> MysteryGiftDecoList = new List<int>
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

    readonly List<string> MysteryGiftDecoNameList = new List<string>
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
