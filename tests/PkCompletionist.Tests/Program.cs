using System.Text.Json;

namespace PkCompletionist.Tests;

internal static class Program
{
    // This entry point runs only through dotnet run; dotnet test uses the xUnit adapter.
    private static int Main(string[] args)
    {
        if (args.Length != 2 || (args[0] != "--generate-fixtures" && args[0] != "--generate-sorter-fixture" && args[0] != "--generate-event-fixtures" && args[0] != "--generate-flag-analyzer-fixture"))
        {
            Console.Error.WriteLine("Usage: dotnet run --project tests/PkCompletionist.Tests -- <--generate-fixtures|--generate-sorter-fixture|--generate-event-fixtures|--generate-flag-analyzer-fixture> <test-project-directory>");
            return 1;
        }

        var projectDirectory = Path.GetFullPath(args[1]);
        if (!File.Exists(Path.Combine(projectDirectory, "PkCompletionist.Tests.csproj")))
            throw new ArgumentException("The destination must be the PkCompletionist.Tests project directory.");

        if (args[0] == "--generate-flag-analyzer-fixture")
        {
            var snapshot = FlagAnalyzerSnapshot.Capture(Path.Combine(projectDirectory, "Saves"));
            if (!snapshot.Success)
                throw new InvalidOperationException($"Crystal flag analysis failed: {string.Join("; ", snapshot.Messages)}");
            Directory.CreateDirectory(Path.Combine(projectDirectory, "Fixtures"));
            File.WriteAllText(Path.Combine(projectDirectory, "Fixtures", FlagAnalyzerSnapshot.FixtureFile),
                JsonSerializer.Serialize(snapshot, ValidatorSnapshot.JsonOptions) + "\n");
            Console.WriteLine($"Generated {FlagAnalyzerSnapshot.FixtureFile}");
            return 0;
        }

        if (args[0] == "--generate-event-fixtures")
        {
            var eventSnapshots = EventCase.All().Select(testCase =>
                (testCase.Id, Snapshot: EventSnapshot.Capture(Path.Combine(projectDirectory, "Saves"), testCase))).ToArray();
            foreach (var (id, snapshot) in eventSnapshots)
            {
                var directory = Path.Combine(projectDirectory, "Fixtures", "Events", id);
                Directory.CreateDirectory(directory);
                File.WriteAllText(Path.Combine(directory, "result.json"),
                    JsonSerializer.Serialize(snapshot, ValidatorSnapshot.JsonOptions) + "\n");
                Console.WriteLine($"Generated {id}");
            }
            return 0;
        }

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

        var fixturesDirectory = Path.Combine(projectDirectory, "Fixtures", "Completion");
        Directory.CreateDirectory(fixturesDirectory);
        foreach (var (file, json) in snapshots)
        {
            File.WriteAllText(Path.Combine(fixturesDirectory, file), json);
            Console.WriteLine($"Generated {file}");
        }
        return 0;
    }
}
