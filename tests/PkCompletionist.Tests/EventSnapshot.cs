using System.Security.Cryptography;
using PkCompletionist.Core;

namespace PkCompletionist.Tests;

internal sealed record EventCase(string Game, string Event, string SaveFile, string? SecondSaveFile = null)
{
    public string Id => $"{Game}.{Event}";

    public static IEnumerable<EventCase> All()
    {
        foreach (var testCase in For<PK_EVENT1>("yellow", "Pokemon Yellow.sav")) yield return testCase;
        foreach (var testCase in For<PK_EVENT2>("crystal", "Pokemon Crystal.sav")) yield return testCase;
        foreach (var testCase in For<PK_EVENT3>("emerald", "Pokemon Emerald - Events.sav")) yield return testCase;
        foreach (var testCase in For<PK_EVENT3_PINBALL>("pinball-ruby-sapphire", "Pokemon Pinball - Ruby & Sapphire.sav")) yield return testCase;
        foreach (var testCase in For<PK_EVENT4>("platinum", "Pokemon Platinum - Events.sav"))
            yield return testCase with
            {
                SecondSaveFile = testCase.Event switch
                {
                    nameof(PK_EVENT4.AuroraTicketEvent_Gen3) => "Pokemon Emerald - Events.sav",
                    nameof(PK_EVENT4.Mew_MyPokemonRanch) => "Pokemon Platinum - Events.sav",
                    _ => null,
                },
            };
        foreach (var testCase in For<PK_EVENT5>("black2", "Pokemon Black2.sav")) yield return testCase;
        foreach (var testCase in For<PK_EVENT7_Shuffle>("shuffle", "Pokemon Shuffle.bin")) yield return testCase;

        yield return new("platinum-no-second-save", nameof(PK_EVENT4.AuroraTicketEvent_Gen3), "Pokemon Platinum - Events.sav");
        yield return new("red-rescue-team", "Unsupported", "Pokemon Mystery Dungeon - Red Rescue Team.sav");
        yield return new("explorers-of-sky", "Unsupported", "Pokemon Mystery Dungeon - Explorers of Sky.sav");
        yield return new("ranger", "Unsupported", "Pokemon Ranger.dsv");
    }

    private static IEnumerable<EventCase> For<T>(string game, string save) where T : struct, Enum =>
        Enum.GetNames<T>().Select(evt => new EventCase(game, evt, save));
}

internal sealed record EventResult(
    string Event,
    string SaveFile,
    string SaveSha256,
    string? SecondSaveFile,
    string? SecondSaveSha256,
    string VersionHint,
    bool Success,
    string? Error,
    string[] Messages,
    string OutputSaveASha256,
    string? OutputSaveBSha256);

internal static class EventSnapshot
{
    public static EventResult Capture(string savesDirectory, EventCase testCase)
    {
        var inputA = File.ReadAllBytes(Path.Combine(savesDirectory, testCase.SaveFile));
        var inputB = testCase.SecondSaveFile is null ? null
            : File.ReadAllBytes(Path.Combine(savesDirectory, testCase.SecondSaveFile));
        var hashA = Convert.ToHexString(SHA256.HashData(inputA));
        var hashB = inputB is null ? null : Convert.ToHexString(SHA256.HashData(inputB));
        Command.LastCommand = null;
        try
        {
            var success = EventSimulator.Execute(testCase.Event, inputA, "", inputB);
            return new EventResult(testCase.Event, testCase.SaveFile, hashA, testCase.SecondSaveFile,
                hashB, "", success, Command.LastCommand?.LastError, Command.GetMessages(),
                Convert.ToHexString(SHA256.HashData(Command.GetSavA())),
                inputB is null ? null : Convert.ToHexString(SHA256.HashData(Command.GetSavB())));
        }
        finally
        {
            Command.LastCommand = null;
        }
    }
}
