﻿using System;
using System.IO;
using PkCompletionist.Core;
using PKHeX.Core;

namespace PkCompletionist;

// dotnet workload install wasm-tools
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


    static void Main(string[] args)
    {
        /* For debug:
        var file = "";
        var mySav = (SAV2)SaveUtil.GetVariantSAV(TryReadAllBytes(file));
        File.WriteAllBytes(file + "2", mySav.Write());
        */
        /*
        var file = "C:\\Users\\samue\\Game\\DS\\ROM\\Pokemon Platinum";
        var sav = (SAV4Pt)SaveUtil.GetVariantSAV(TryReadAllBytes(file + ".sav"));
        sav.State.Edited = true;

        for (int i = 0; i < 100; i += 2)
        {
            for (int j = 0; j < 50; j++)
                sav.General[0x4E38 + j] = 0;

            var addr = 0x4E38 + i / 2;
            sav.General[addr] = 0x2F;
            File.WriteAllBytes($"{file} 0x{addr:X}.sav", sav.Write());
        }

        return;
        */

        if (!ValidateArgLength(args, 1))
            return;

        /*
        overwriteFlags
        "C:\Users\samue\Game\Gameboy\ROMS\Pokemon Emerald - before.sav"
        "C:\Users\samue\Game\Gameboy\ROMS\Pokemon Emerald - after.sav"
        "C:\Users\samue\Game\Gameboy\ROMS\Pokemon Emerald.sav"
        0
        0
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

            if (FlagAnalyzer.Execute(sav1, sav2, args[4]))
                LastCommandSave(args[3]);

            LastCommandPrintMsgs();
        }

        if (args[0] == "validate")
        {
            if (!ValidateArgLength(args, 2))
                return;

            var savData = TryReadAllBytes(args[1]);
            if (savData == null)
                return;

            bool living = args.Length > 2 && args[2] == "--living";
            CompletionValidator.Execute(savData, living);

            LastCommandPrintMsgs();
        }

        if (args[0] == "sortPkms")
        {
            if (!ValidateArgLength(args, 3))
                return;

            var savData = TryReadAllBytes(args[1]);
            if (savData == null)
                return;

            if (PkSorter.Execute(savData))
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


        if (args[0] == "event")
        {
            if (!ValidateArgLength(args, 3))
                return;

            var savData = TryReadAllBytes(args[2]);
            if (savData == null)
                return;

            byte[]? savBData = null;
            if (args.Length  >= 6)
                savBData = TryReadAllBytes(args[4]);

            if (EventSimulator.Execute(args[1], savData, savBData))
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