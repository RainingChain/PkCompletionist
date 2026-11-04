using System.Text.Json;

namespace PkCompletionist.Tests;

internal static class Program
{
    // This entry point runs only through dotnet run; dotnet test uses the xUnit adapter.
    private static int Main(string[] args)
    {
        if (args.Length != 2 || (args[0] != "--generate-fixtures" && args[0] != "--generate-sorter-fixture"))
        {
            Console.Error.WriteLine("Usage: dotnet run --project tests/PkCompletionist.Tests -- <--generate-fixtures|--generate-sorter-fixture> <test-project-directory>");
            return 1;
        }

        var projectDirectory = Path.GetFullPath(args[1]);
        if (!File.Exists(Path.Combine(projectDirectory, "PkCompletionist.Tests.csproj")))
            throw new ArgumentException("The destination must be the PkCompletionist.Tests project directory.");

        if (args[0] == "--generate-sorter-fixture")
        {
            var order = SorterSnapshot.Capture(Path.Combine(projectDirectory, "Saves"));
            Directory.CreateDirectory(Path.Combine(projectDirectory, "Fixtures"));
            File.WriteAllText(Path.Combine(projectDirectory, "Fixtures", SorterSnapshot.FixtureFile), order);
            Console.WriteLine($"Generated {SorterSnapshot.FixtureFile}");
            return 0;
        }

        // Capture every case successfully before replacing any baseline files.
        var snapshots = ValidatorCase.All().Select(testCase =>
        {
            var snapshot = ValidatorSnapshot.Capture(Path.Combine(projectDirectory, "Saves"), testCase);
            if (!snapshot.Success)
                throw new InvalidOperationException($"Validation failed for {testCase.FixtureFile}: {string.Join("; ", snapshot.Messages)}");
            return (testCase.FixtureFile, Json: JsonSerializer.Serialize(snapshot, ValidatorSnapshot.JsonOptions) + "\n");
        }).ToArray();

        var fixturesDirectory = Path.Combine(projectDirectory, "Fixtures");
        Directory.CreateDirectory(fixturesDirectory);
        foreach (var (file, json) in snapshots)
        {
            File.WriteAllText(Path.Combine(fixturesDirectory, file), json);
            Console.WriteLine($"Generated {file}");
        }
        return 0;
    }
}
