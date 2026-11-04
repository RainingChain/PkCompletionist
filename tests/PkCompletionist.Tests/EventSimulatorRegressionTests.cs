using System.Text.Json;
using Xunit;

namespace PkCompletionist.Tests;

public sealed class EventSimulatorRegressionTests
{
    public static IEnumerable<object[]> Cases => EventCase.All().Select(c => new object[] { c.Id });

    [Theory]
    [MemberData(nameof(Cases))]
    public void MessagesAndResultingSaveHashesMatchFixtures(string id)
    {
        var testCase = EventCase.All().Single(c => c.Id == id);
        var fixtureDirectory = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Events", id);
        var expectedResult = JsonSerializer.Deserialize<EventResult>(ReadFixture(Path.Combine(fixtureDirectory, "result.json")));
        Assert.NotNull(expectedResult);
        var actual = EventSnapshot.Capture(Path.Combine(AppContext.BaseDirectory, "Saves"), testCase);
        // Serialize for a structural comparison that includes ordered messages and input metadata.
        Assert.Equal(JsonSerializer.Serialize(expectedResult, ValidatorSnapshot.JsonOptions),
            JsonSerializer.Serialize(actual, ValidatorSnapshot.JsonOptions));
    }

    private static byte[] ReadFixture(string path)
    {
        Assert.True(File.Exists(path), $"Missing event fixture: {path}. Generate event fixtures explicitly; tests never create them.");
        return File.ReadAllBytes(path);
    }

}
