using System.Text.Json;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace PkCompletionist.Tests;

public sealed class CompletionValidatorRegressionTests
{
    public static IEnumerable<object[]> Cases => ValidatorCase.All().Select(c => new object[] { c.Id, c.Objective });

    [Theory]
    [MemberData(nameof(Cases))]
    public void ResultsMatchFixture(string id, string objective)
    {
        var testCase = ValidatorCase.All().Single(c => c.Id == id && c.Objective == objective);
        var context = $"{testCase.SaveFile} ({objective})";
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", testCase.FixtureFile);
        Assert.True(File.Exists(fixturePath), $"{context}: missing fixture {fixturePath}. Generate fixtures explicitly; tests never create them.");
        var expected = JsonSerializer.Deserialize<ValidatorSnapshot>(File.ReadAllText(fixturePath));
        Assert.NotNull(expected);
        var actual = ValidatorSnapshot.Capture(Path.Combine(AppContext.BaseDirectory, "Saves"), testCase);

        Match(context, nameof(actual.SaveFile), expected.SaveFile, actual.SaveFile);
        Match(context, nameof(actual.SaveSha256), expected.SaveSha256, actual.SaveSha256);
        Match(context, nameof(actual.VersionHint), expected.VersionHint, actual.VersionHint);
        Match(context, nameof(actual.Objective), expected.Objective, actual.Objective);
        Assert.True(actual.Success, $"{context}: Execute returned false. Messages: {string.Join("; ", actual.Messages)}");
        Match(context, nameof(actual.Success), expected.Success, actual.Success);
        MatchArray(context, nameof(actual.ObtainedStatus), expected.ObtainedStatus, actual.ObtainedStatus);
        MatchArray(context, nameof(actual.CompletionHints), expected.CompletionHints, actual.CompletionHints);
        MatchArray(context, nameof(actual.Messages), expected.Messages, actual.Messages);
    }

    private static void Match<T>(string context, string field, T expected, T actual) =>
        Assert.True(EqualityComparer<T>.Default.Equals(expected, actual),
            $"{context}: {field} mismatch. Expected: {JsonSerializer.Serialize(expected)}; actual: {JsonSerializer.Serialize(actual)}");

    private static void MatchArray(string context, string field, string[] expected, string[] actual)
    {
        Match(context, $"{field}.Length", expected.Length, actual.Length);
        for (var i = 0; i < expected.Length; i++)
            Match(context, $"{field}[{i}]", expected[i], actual[i]);
    }
}
