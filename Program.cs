using System;
using System.IO;
using PkCompletionist.Core;
using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PkCompletionist;

// dotnet workload install wasm-tools-net7
// https://www.meziantou.net/using-dotnet-code-from-javascript-using-webassembly.htm
// To generate .exe instead of wasm, check PkCompletionist settings (Right-Click -> Properties).

internal class Program
{
    public static byte[]? TryReadAllBytes(string path)
    {
        try
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int numBytesToRead = Convert.ToInt32(fs.Length);
                byte[] oFileBytes = new byte[(numBytesToRead)];
                fs.Read(oFileBytes, 0, numBytesToRead);
                return oFileBytes;
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: The file {path} doesn't exist.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while reading file: " + path);
            Console.WriteLine(ex.ToString());
            return null;
        }
    }

    public static string? TryReadAllText(string path)
    {
        try
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                    return sr.ReadToEnd();
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: The file {path} doesn't exist.");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while reading file: " + path);
            Console.WriteLine(ex.ToString());
            return null;
        }
    }

    public static bool ValidateArgLength(string[] args, int Length)
    {
        if (args.Length < Length)
        {
            Console.WriteLine("No enough arguments were provided.");
            return false;
        }
        return true;
    }


    static bool TmpDebug()
    {
        var a = TryReadAllBytes("C:\\Users\\Samuel\\Game\\DS\\ROM\\cmp\\tmp2\\after.sav")!;
        var b = TryReadAllBytes("C:\\Users\\Samuel\\Game\\DS\\ROM\\cmp\\tmp2\\before.sav")!;
        var b2 = TryReadAllBytes("C:\\Users\\Samuel\\Game\\DS\\ROM\\cmp\\tmp2\\before2.sav")!;
        var b3 = TryReadAllBytes("C:\\Users\\Samuel\\Game\\DS\\ROM\\cmp\\tmp2\\before3.sav")!;

        for (int i = 0; i < a.Length; i++)
        {
            if (b[i] != b2[i])
                continue;
            if (b[i] != b3[i])
                continue;
            if (b2[i] != b3[i])
                continue;
            if (a[i] == b[i])
                continue;
            Console.WriteLine("Hex: {0:X}", i);
        }
        return true;
    }

    static bool TmpDebug2()
    {
        var savData_ranger = TryReadAllBytes("C:\\Users\\Samuel\\Game\\DS\\ROM\\cmp\\tmp2\\encrypted\\before.sav");
        if (savData_ranger == null)
            return true;
        var sav_r = (SAV4_Ranger)Command.GetVariantSAV(savData_ranger, "")!;

        sav_r.Decrypt();
        File.WriteAllBytes("C:\\Users\\Samuel\\Game\\DS\\ROM\\cmp\\tmp2\\before_csharp.sav", sav_r.Write());
        return true;
    }

    /*
    //File.WriteAllBytes("C:\\Users\\Samuel\\Downloads\\Pokemon Platinum_online_mod.sav", sav8.Write());
    var a = 0;
    if (a != 0 || a == 0)
        return;
    */
    /*var bytes = sav8.Data;
    var size = bytes.Length / sizeof(int);
    var ints = new UInt32[size];
    for (var index = 0; index < size; index++)
    {
        ints[index] = BitConverter.ToUInt32(bytes, index * sizeof(int));

        if (ints[index] == 0xE6F6591A || ints[index] == 0x1A59F6E6)
        {
            ints[index] = 0x1256477;
            //File.WriteAllBytes("C:\\Users\\Samuel\\Downloads\\plat_play_wk_7051E4B1.sav", sav8.Write());
            return;
        }
    }*/

    static bool TmpDebug3()
    {
        var file_name = "C:\\Users\\Samuel\\source\\repos\\pk_battle_facilities_rng_pt\\utils\\search\\video.sav";
        var savData = TryReadAllBytes(file_name);
        if (savData == null)
           return true;
        var sav = (SAV4Pt)Command.GetVariantSAV(savData, "")!;

        var wantedSameDaySeed = 0x7a7aad82;
        sav.Data[0x7238 + 0] = (byte)((wantedSameDaySeed >> 0) & 0xFF);
        sav.Data[0x7238 + 1] = (byte)((wantedSameDaySeed >> 8) & 0xFF);
        sav.Data[0x7238 + 2] = (byte)((wantedSameDaySeed >> 16) & 0xFF); 
        sav.Data[0x7238 + 3] = (byte)((wantedSameDaySeed >> 24) & 0xFF);
        File.WriteAllBytes(file_name.Replace(".sav", "2.sav"), sav.Write());
        return true;
        /*
        // C:\Users\samuel\source\repos\PkCompletionist\bin\Debug\net7.0\PkCompletionist.exe platinum_setBattleTowerSeeds "C:\Users\Samuel\Game\DS\ROM\14wins.sav" "C:\Users\Samuel\Game\DS\ROM\14wins_after.sav" 0xFDF06E9C 0x0
        // usage: use it once, load savefile, game says it's corrupted. save in-game. run the .exe again. load savefile => works
        if (args[0] == "platinum_setBattleTowerSeeds")
        {
            if (!ValidateArgLength(args, 5))
                return;

            var savData = TryReadAllBytes(args[1]);
            if (savData == null)
                return;
            var sav = (SAV4Pt)Command.GetVariantSAV(savData, "")!;

            var wantedSameDaySeed = Convert.ToUInt32(args[3], 16);
            sav.Data[0x7238 + 0] = (byte)((wantedSameDaySeed >> 0) & 0xFF);
            sav.Data[0x7238 + 1] = (byte)((wantedSameDaySeed >> 8) & 0xFF);
            sav.Data[0x7238 + 2] = (byte)((wantedSameDaySeed >> 16) & 0xFF);
            sav.Data[0x7238 + 3] = (byte)((wantedSameDaySeed >> 24) & 0xFF);

            var wantedDiffDaySeed = Convert.ToUInt32(args[4], 16);
            if (wantedDiffDaySeed != 0)
            {
                sav.Data[0x5664 + 0] = (byte)((wantedDiffDaySeed >> 0) & 0xFF);
                sav.Data[0x5664 + 1] = (byte)((wantedDiffDaySeed >> 8) & 0xFF);
                sav.Data[0x5664 + 2] = (byte)((wantedDiffDaySeed >> 16) & 0xFF);
                sav.Data[0x5664 + 3] = (byte)((wantedDiffDaySeed >> 24) & 0xFF);
            }
            File.WriteAllBytes(args[2], sav.Write());
            return;
        }
        */
    }


    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Debug.OnStart();
            return;
        }

        if (!ValidateArgLength(args, 1))
            return;

        var versionHint = "PmdRescueTeam";

        /*
        C:\Users\samuel\source\repos\PkCompletionist\bin\Debug\net7.0\PkCompletionist.exe overwriteFlags before.sav after.sav "Pokemon Platinum.sav" "0,0"
        */
        if (args[0] == "overwriteFlags")
        {
            Console.WriteLine(System.String.Join(' ', args));

            if (!ValidateArgLength(args, 5))
                return;

            var sav1 = TryReadAllBytes(args[1]);
            var sav2 = TryReadAllBytes(args[2]);

            if (sav1 == null || sav2 == null)
                return;

            if (FlagAnalyzer.Execute(sav1, sav2, args[4], versionHint))
                LastCommandSave(args[3]);

            LastCommandPrintMsgs();
        }

        //validate input.sav --living
        if (args[0] == "validate")
        {
            if (!ValidateArgLength(args, 2))
                return;

            var savData = TryReadAllBytes(args[1]);
            if (savData == null)
                return;

            Objective objective = args.Length > 2 && args[2] == "--living" ? Objective.living : Objective.normal;
            CompletionValidator.Execute(savData, versionHint, (int)objective);

            foreach (var msg in CompletionValidator.GetLastObtainedStatus())
                Console.WriteLine(msg); 
                
            LastCommandPrintMsgs();
        }

        if (args[0] == "fixChecksum")
        {
            if (!ValidateArgLength(args, 2))
                return;

            var savData = TryReadAllBytes(args[1]);
            if (savData == null)
                return;

            var sav = Command.GetVariantSAV(savData, versionHint)!;
            File.WriteAllBytes(args[1], sav.Write());
            return;
        }

        //sortPkms input.sav output.sav
        if (args[0] == "sortPkms")
        {
            if (!ValidateArgLength(args, 3))
                return;

            var savData = TryReadAllBytes(args[1]);
            if (savData == null)
                return;

            if (PkSorter.Execute(savData, versionHint))
                LastCommandSave(args[2]);

            LastCommandPrintMsgs();                    
        }


        if (args[0] == "gen2_mysteryGift")
        {
            if (!ValidateArgLength(args, 4))
                return;

            var sav1 = TryReadAllBytes(args[1]);
            var sav2 = TryReadAllBytes(args[2]);
            if (sav1 == null || sav2 == null)
                return;

            var onlyPrintDecoOfPlayer1 = args.Length >= 6 ? args[5] == "true" : false;
            var count = args.Length >= 7 ? TryParseInt(args[6]) : 1;
            if (count == null)
                return;

            if (MysteryGiftSimulatorCmd2.Execute(sav1, sav2, onlyPrintDecoOfPlayer1, (int)count))
            {
                LastCommandSave(args[3], true);
                LastCommandSave(args[4], false);
            }
            
            LastCommandPrintMsgs();
        }

        //event EVT input.sav output.sav inputB.sav outputB.sav
        if (args[0] == "event")
        {
            if (!ValidateArgLength(args, 3))
                return;

            var savData = TryReadAllBytes(args[2]);
            if (savData == null)
                return;

            byte[]? savBData = null;
            if (args.Length >= 6)
                savBData = TryReadAllBytes(args[4]);

            if (EventSimulator.Execute(args[1], savData, versionHint, savBData))
            {
                LastCommandSave(args[3]);
                if (savBData != null)
                    LastCommandSave(args[5], false);
            }

            LastCommandPrintMsgs();
        }


        if (args[0] == "exportEventFlags")
        {
            ExportEventFlags(args[1]);
        }

        if (args[0] == "flagWatcher")
        {
            StartFlagWatcher(args[1]);
        }
    }
    static int? TryParseInt(string str)
    {
        int myInt;
        if (!Int32.TryParse(str, out myInt))
        {
            Console.WriteLine($"Error. {str} is not an integer.");
            return null;
        }
        return myInt;
    }

    static void LastCommandPrintMsgs()
    {
        foreach (var msg in Command.GetMessages())
            Console.WriteLine(msg);
    }

    static void LastCommandSave(string dest, bool savA = true)
    {
        var sav = savA ? Command.GetSavA() : Command.GetSavB();
        if (sav == null)
        {
            Console.WriteLine("Error: There is no save file to save.");
            return;
        }
        File.WriteAllBytes(dest, sav);
    }

    static void ExportEventFlags(string file)
    {
        var savData = TryReadAllBytes(file);
        if (savData == null)
            return;

        var savA = (SAV3E?)SaveUtil.GetVariantSAV(savData);
        if (savA == null)
            return;
        //for (var i = 0; i < savA.EventFlagCount; i++)
        //    Console.WriteLine($"0x{i:X} {savA.GetEventFlag(i)}");

        //  /*0x2B50*/ PokeNews pokeNews[POKE_NEWS_COUNT];
        for (var i = 0; i < 16; i++)
        {
            var kind = savA.Large[0x2B50 + i * 4 + 0];
            var state = savA.Large[0x2B50 + i * 4 + 1];
            var dayCountdown = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(savA.Large.AsSpan(0x2B50 + i * 4 + 2));
            Console.WriteLine($"kind={kind}, state={state}, dayCountdown={dayCountdown}");
        }
        savA.Large[0x2B50 + 0 * 4 + 0] = 3;
        File.WriteAllBytes(@"C:\Users\samue\Game\Gameboy\ROMS\Pokemon Emerald - Copy2.sav", savA.Write());
    }

    static void StartFlagWatcher(string file)
    {
        bool[] oldFlags = new bool[0];
        bool[] oldSpawnFlags = new bool[0];
        while (true)
        {
            System.Threading.Thread.Sleep(2000);
            var savData = TryReadAllBytes(file);
            if (savData == null)
                continue;

            var savA = (SAV1?)SaveUtil.GetVariantSAV(savData);
            if (savA == null)
                continue;

            bool[] newFlags = savA.GetEventFlags();
            bool[] newSpawnFlags = savA.EventSpawnFlags;
            if (oldFlags.Length == 0)
                oldFlags = newFlags;

            if (oldSpawnFlags.Length == 0)
                oldSpawnFlags = newSpawnFlags;

            if (newFlags.Length == oldFlags.Length)
            {
                for (int i = 0; i < newFlags.Length; i++)
                {
                    if (newFlags[i] == oldFlags[i])
                        continue;

                    Console.WriteLine($"EventFlags[0x{i:X}] = {newFlags[i]}");
                }
                oldFlags = newFlags;
            }

            if (newSpawnFlags.Length == oldSpawnFlags.Length)
            {
                for (int i = 0; i < newSpawnFlags.Length; i++)
                {
                    if (newSpawnFlags[i] == oldSpawnFlags[i])
                        continue;

                    Console.WriteLine($"EventSpawnFlags[0x{i:X}] = {newSpawnFlags[i]}");
                }
                oldSpawnFlags = newFlags;
            }
        }
    }

}