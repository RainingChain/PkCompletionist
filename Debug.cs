using PkCompletionist.Core;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkCompletionist;

internal class Debug
{
    public static void OnStart()
    {
        var file = "C:\\Users\\samue\\Game\\Gameboy\\ROMS\\pmdTest2\\Pokemon Mystery Dungeon - Red Rescue Team (U)";
        var versionHint = "PmdRescueTeam";
        var bytes = Program.TryReadAllBytes($"{file}.sav");
        if (bytes == null)
            return;
        
        var sav = (SAV3_PmdRescueTeam)Command.GetVariantSAV(bytes, versionHint)!;
        for (var i = 0x70; i <= 0x80; i++)
            sav.Data[i] = 0;
        /*
        sav.Data[sav.off.MakuhitaDojoCompletedOffset] = 0xFF;
        sav.Data[sav.off.MakuhitaDojoCompletedOffset + 1] = 0xFF;
        sav.Data[sav.off.MakuhitaDojoCompletedOffset + 2] = 0xFF;
        File.WriteAllBytes($"{file}_2.sav", sav.Write());
        return;*/
        /*
        int byteStart = 0x4ED8;
        int byteEnd = 0x4EDF;

        for (int i = byteStart; i <= byteEnd; i++)
        {
            for (int k = 0; k < 8; k++) // for every bit
            {
                // Clear existing
                for (int j = byteStart; j <= byteEnd; j++)
                    sav.Data[j] = 0;

                sav.Data[i] = (byte)(1 << k); // set new byte
                File.WriteAllBytes($"{file} 0x{(i):X} {k}.sav", sav.Write());
            }
        }
        */
        File.WriteAllBytes($"{file}_2.sav", sav.Write());
        return;

        /* For debug:
        var file = "";
        var mySav = (SAV2)SaveUtil.GetVariantSAV(TryReadAllBytes(file));
        File.WriteAllBytes(file + "2", mySav.Write());
        */
        /*

            var sav = (SAV4Pt)SaveUtil.GetVariantSAV(file);
            sav.State.Edited = true;
                       

            File.WriteAllBytes($"{file}.sav", sav.Write());

            for (int i = 0; i <= 10; i++)
            {
                for (int k = 0; k < 8; k++) // for every bit
                {
                    for (int j = 0; j <= 10; j++) // clear existing
                        sav.General[52910 + j] = 0;

                    sav.General[52910 + i] = (byte)(1 << k); // set new byte
                    File.WriteAllBytes($"{file} 0x{(52910 + i):X} {k}.sav", sav.Write());
                }

            }
        */

        //for (int j = 0; j <= 5; j++) // clear existing
        //    sav.General[0x4E5C + j] = (byte)0xff;

        /*for (int i = 0; i <= 255; i++)
        {
            sav.State.Edited = true;
            sav.General[0x4E61] = (byte)i;
            File.WriteAllBytes($"{file} - {i:X2}.sav", sav.Write());
        }*/
    }
    public static void Main2()
    {
    }
}
