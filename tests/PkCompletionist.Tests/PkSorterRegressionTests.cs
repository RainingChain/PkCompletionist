using Xunit;

namespace PkCompletionist.Tests;

public sealed class PkSorterRegressionTests
{
    [Fact]
    public void EmeraldWrittenSaveBoxOrderMatchesFixture()
    {
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", SorterSnapshot.FixtureFile);
        Assert.True(File.Exists(fixturePath), $"Missing sorter fixture: {fixturePath}. Generate it explicitly; tests never create it.");
        var expected = File.ReadAllText(fixturePath);
        var actual = SorterSnapshot.Capture(Path.Combine(AppContext.BaseDirectory, "Saves"));
        Assert.Equal(expected, actual);
    }
}
