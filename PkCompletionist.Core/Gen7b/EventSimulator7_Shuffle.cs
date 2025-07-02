using PKHeX.Core;
using System.Collections.Generic;
using System.Linq;

namespace PkCompletionist.Core;

enum PK_EVENT7_Shuffle
{
    PikachuCelebration,
    AddHearts,
    AddJewels,
    AddCoins,
    ResetLimitedShop,
}

internal class EventSimulator7_Shuffle : EventSimulatorX
{

    public EventSimulator7_Shuffle(Command command, SAV7_Shuffle sav, SaveFile? savB) : base(command, sav, savB)
    {
        this.sav = sav;
    }

    new SAV7_Shuffle sav;

    public override string? ExecEvent(string evtName)
    {
        var evt = ParseEvtName<PK_EVENT7_Shuffle>(evtName);

        if (evt == PK_EVENT7_Shuffle.AddHearts)
        {
            this.sav.Data[0x2D4A] = 0x90;
            this.sav.Data[0x2D4A + 1] = 0x31;
            return null;
        }
        if (evt == PK_EVENT7_Shuffle.AddJewels)
        {
            //Jewel bits are split in 2 bytes. Goal is to set 0x96
            this.sav.Data[0x6A] &= 0x0F; // clear 4 first bits
            this.sav.Data[0x6A] |= 0x60;

            this.sav.Data[0x6B] &= 0xF0; // clear 4 last bits
            this.sav.Data[0x6B] |= 0x09;
            return null;
        }
        if (evt == PK_EVENT7_Shuffle.AddCoins)
        {
            this.sav.Data[0x68] = 0xF8;
            this.sav.Data[0x69] = 0x34;
            this.sav.Data[0x6A] &= 0xF0; // clear 4 last bits
            this.sav.Data[0x6A] |= 0x0C;
            return null;
        }
        if (evt == PK_EVENT7_Shuffle.ResetLimitedShop)
        {
            for (int i = 0; i < 10; i++)
                this.sav.Data[0x2EC7 + i] = 0x00;
            return null;
        }

        if (evt == PK_EVENT7_Shuffle.PikachuCelebration)
        {
            var list = new List<int> { 888, 889, 890, 879, 880, 881, 882, 883, 884, 885, 886, 887 };
            foreach (var monId in list)
                this.sav.SetCaughtMon(monId);
            return null;
        }
        return $"Internal error : Invalid event name \"{evt}\".";
    }
}