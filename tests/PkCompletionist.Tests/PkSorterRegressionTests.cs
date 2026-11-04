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
        var actual = SorterSnapshot.Capture(Path.Combine(AppContext.BaseDirectory, "Saves"), out var outputHash);
        Assert.Equal(expected, actual);
        var hashFixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", SorterSnapshot.HashFixtureFile);
        Assert.True(File.Exists(hashFixturePath), $"Missing sorter hash fixture: {hashFixturePath}. Generate it explicitly.");
        Assert.Equal(File.ReadAllText(hashFixturePath), outputHash);
    }
}
