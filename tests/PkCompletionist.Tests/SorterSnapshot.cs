using System.Globalization;
using System.Security.Cryptography;
using PKHeX.Core;
using PkCompletionist.Core;

namespace PkCompletionist.Tests;

internal static class SorterSnapshot
{
    public const string FixtureFile = "emerald.sorted-boxes.txt";
    public const string HashFixtureFile = "emerald.sorted-save.sha256.txt";

    public static string Capture(string savesDirectory) => Capture(savesDirectory, out _);

    public static string Capture(string savesDirectory, out string outputSha256)
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"PkCompletionist-sorted-{Guid.NewGuid():N}.sav");
        Command.LastCommand = null;
        try
        {
            var input = File.ReadAllBytes(Path.Combine(savesDirectory, "Pokemon Emerald.sav"));
            if (!PkSorter.Execute(input, ""))
                throw new InvalidOperationException($"Emerald sorting failed: {string.Join("; ", Command.GetMessages())}");

            var output = Command.GetSavA();
            outputSha256 = Convert.ToHexString(SHA256.HashData(output));
            File.WriteAllBytes(outputPath, output);
            var reloaded = Command.GetVariantSAV(File.ReadAllBytes(outputPath), "") as SAV3E
                ?? throw new InvalidOperationException("The written sorted save could not be reloaded as Emerald.");

            // BoxData traverses boxes in order, then slots within each box.
            // Species is the national dex number; retain 0 for every empty slot.
            return string.Join(",", reloaded.BoxData.Select(pkm => pkm.Species.ToString(CultureInfo.InvariantCulture)));
        }
        finally
        {
            Command.LastCommand = null;
            File.Delete(outputPath);
        }
    }
}
