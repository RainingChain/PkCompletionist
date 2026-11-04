using System.Text.Json;
using Xunit;

namespace PkCompletionist.Tests;

public sealed class SavDescriptorRegressionTests
{
    public static IEnumerable<object[]> Cases => DescriptorSnapshot.Cases.Select(c => new object[] { c.Id });

    [Theory]
    [MemberData(nameof(Cases))]
    public void DescriptionsAndMessagesMatchFixture(string id)
    {
        var testCase = DescriptorSnapshot.Cases.Single(c => c.Id == id);
        var path = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Descriptors", id + ".json");
        Assert.True(File.Exists(path), $"Missing descriptor fixture: {path}. Generate it explicitly; tests never create it.");
        var expected = JsonSerializer.Deserialize<DescriptorResult>(File.ReadAllText(path));
        Assert.NotNull(expected);
        var actual = DescriptorSnapshot.Capture(Path.Combine(AppContext.BaseDirectory, "Saves"), testCase);
        Assert.Equal(JsonSerializer.Serialize(expected, ValidatorSnapshot.JsonOptions),
            JsonSerializer.Serialize(actual, ValidatorSnapshot.JsonOptions));
    }
}
