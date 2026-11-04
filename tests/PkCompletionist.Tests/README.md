# CompletionValidator regression tests

Run from the repository root with the .NET 10 SDK:

```powershell
dotnet test tests/PkCompletionist.Tests/PkCompletionist.Tests.csproj
```

The suite calls `CompletionValidator.Execute` directly for each save in `Saves`,
with both `normal` and `living` objectives and an empty version hint. It compares
the return value, obtained-status array, completion-hints array, and command
messages with committed JSON fixtures. Array order and empty strings are retained.
Each fixture also records the input filename, SHA-256, objective, and version hint.
Mismatch messages identify the save, objective, field, and array index.

Saves and fixtures are copied to the test output directory, so tests do not depend
on the current working directory. Tests reset the command/validator static state
before and after each invocation. Test parallelization is disabled because this
state is shared.

## Updating the baseline

The initial fixtures were generated from the existing implementation, assumed
correct. Tests only read fixtures; missing fixtures fail the test. To explicitly
replace the baseline after an intentional behavior change, run:

```powershell
dotnet run --project tests/PkCompletionist.Tests -- --generate-fixtures tests/PkCompletionist.Tests
dotnet test tests/PkCompletionist.Tests/PkCompletionist.Tests.csproj
```

The executable entry point is a fixture-generation utility; `dotnet test` uses
the xUnit adapter instead. The utility captures every case successfully before
writing fixtures. Review fixture diffs alongside the change that requires them.
Regeneration uses the current implementation, so do not regenerate merely to
make an unexpected regression pass.

To add coverage, place an unchanged save in `Saves`, add its ID and filename to
`ValidatorCase.All`, then generate and review both objective fixtures. Set a
case-specific version hint there if a future save requires it. Commit the save
and fixtures together. The supplied nine saves cover Yellow, Crystal, Emerald,
Platinum, Black2, Red Rescue Team, Pinball Ruby & Sapphire, Ranger, and Shuffle
(18 regression cases). Other validators require additional saves.

The test project references the root application because it compiles both core
source trees and the required resources. The root project excludes `tests/**`
from its default inputs, keeping test sources, data, and build outputs out of
application builds. No changes to `PKHeX.Core` are required.

## PkSorter Emerald regression test

One additional test sorts `Pokemon Emerald.sav` through `PkSorter.Execute`,
writes the result to a unique temporary save, and reloads that file as Emerald.
It compares the national dex numbers in PC box order (box first, then slot)
with `Fixtures/emerald.sorted-boxes.txt`. This fixture is a single comma-separated
string, including `0` for empty slots, with no trailing newline. Party Pokemon
are excluded. The temporary file is deleted after capture, even on failure;
the input save is never overwritten.

Generate only this fixture explicitly using the current implementation:

```powershell
dotnet run --project tests/PkCompletionist.Tests -- --generate-sorter-fixture tests/PkCompletionist.Tests
```

The normal test command runs this test alongside the validator tests.
