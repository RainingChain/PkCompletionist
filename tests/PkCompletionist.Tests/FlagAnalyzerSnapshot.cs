using System.Security.Cryptography;
using PkCompletionist.Core;

namespace PkCompletionist.Tests;

internal sealed record FlagAnalyzerResult(
    string SaveFile,
    string SaveSha256,
    string SecondSaveFile,
    string SecondSaveSha256,
    string ChangeRanges,
    string VersionHint,
    bool Success,
    string? Error,
    string[] Messages,
    string OutputSaveASha256,
    string OutputSaveBSha256);

internal static class FlagAnalyzerSnapshot
{
    public const string FixtureFile = "crystal.flag-analyzer.json";

    public static FlagAnalyzerResult Capture(string savesDirectory)
    {
        const string saveFile = "Pokemon Crystal - FlagBefore.sav";
        const string secondSaveFile = "Pokemon Crystal - FlagAfter.sav";
        // The end is exclusive and capped at the number of detected differences.
        const string changeRanges = "0,2147483647";
        var inputA = File.ReadAllBytes(Path.Combine(savesDirectory, saveFile));
        var inputB = File.ReadAllBytes(Path.Combine(savesDirectory, secondSaveFile));
        var hashA = Convert.ToHexString(SHA256.HashData(inputA));
        var hashB = Convert.ToHexString(SHA256.HashData(inputB));
        Command.LastCommand = null;
        try
        {
            var success = FlagAnalyzer.Execute(inputA, inputB, changeRanges, "");
            return new(saveFile, hashA, secondSaveFile, hashB, changeRanges, "", success,
                Command.LastCommand?.LastError, Command.GetMessages(),
                Convert.ToHexString(SHA256.HashData(Command.GetSavA())),
                Convert.ToHexString(SHA256.HashData(Command.GetSavB())));
        }
        finally
        {
            Command.LastCommand = null;
        }
    }
}
