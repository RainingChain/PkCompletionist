using System.Text.Json;
using Xunit;

namespace PkCompletionist.Tests;

public sealed class FlagAnalyzerRegressionTests
{
    [Fact]
    public void CrystalMessagesAndResultingSaveHashesMatchFixture()
    {
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", FlagAnalyzerSnapshot.FixtureFile);
        Assert.True(File.Exists(fixturePath), $"Missing flag analyzer fixture: {fixturePath}. Generate it explicitly; tests never create it.");
        var expected = JsonSerializer.Deserialize<FlagAnalyzerResult>(File.ReadAllText(fixturePath));
        Assert.NotNull(expected);
        var actual = FlagAnalyzerSnapshot.Capture(Path.Combine(AppContext.BaseDirectory, "Saves"));
        Assert.True(actual.Success, $"Crystal flag analysis failed: {string.Join("; ", actual.Messages)}");
        Assert.Equal(JsonSerializer.Serialize(expected, ValidatorSnapshot.JsonOptions),
            JsonSerializer.Serialize(actual, ValidatorSnapshot.JsonOptions));
    }
}
