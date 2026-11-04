using System.Security.Cryptography;
using System.Text.Json;
using PkCompletionist.Core;

namespace PkCompletionist.Tests;

internal sealed record ValidatorCase(string Id, string SaveFile, string Objective, int ObjectiveValue)
{
    public string FixtureFile => $"{Id}.{Objective}.json";
    public string VersionHint => "";

    public static IEnumerable<ValidatorCase> All()
    {
        (string Id, string Save)[] saves =
        [
            ("yellow", "Pokemon Yellow.sav"),
            ("crystal", "Pokemon Crystal.sav"),
            ("emerald", "Pokemon Emerald.sav"),
            ("platinum", "Pokemon Platinum.sav"),
            ("black2", "Pokemon Black2.sav"),
            ("red-rescue-team", "Pokemon Mystery Dungeon - Red Rescue Team.sav"),
            ("pinball-ruby-sapphire", "Pokemon Pinball - Ruby & Sapphire.sav"),
            ("ranger", "Pokemon Ranger.dsv"),
            ("shuffle", "Pokemon Shuffle.bin"),
        ];
        foreach (var (id, save) in saves)
        {
            yield return new(id, save, "normal", (int)Core.Objective.normal);
            yield return new(id, save, "living", (int)Core.Objective.living);
        }
    }
}

internal sealed record ValidatorSnapshot(
    string SaveFile,
    string SaveSha256,
    string VersionHint,
    string Objective,
    bool Success,
    string[] ObtainedStatus,
    string[] CompletionHints,
    string[] Messages)
{
    internal static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static ValidatorSnapshot Capture(string savesDirectory, ValidatorCase testCase)
    {
        var bytes = File.ReadAllBytes(Path.Combine(savesDirectory, testCase.SaveFile));
        var hash = Convert.ToHexString(SHA256.HashData(bytes));
        CompletionValidator.LastValidatorX = null;
        Command.LastCommand = null;
        try
        {
            var success = CompletionValidator.Execute(bytes, testCase.VersionHint, testCase.ObjectiveValue);
            return new(testCase.SaveFile, hash, testCase.VersionHint, testCase.Objective, success,
                CompletionValidator.GetLastObtainedStatus(),
                CompletionValidator.GetLastCompletionHints(), Command.GetMessages());
        }
        finally
        {
            CompletionValidator.LastValidatorX = null;
            Command.LastCommand = null;
        }
    }
}
