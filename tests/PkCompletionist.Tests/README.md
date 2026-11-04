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
and fixtures together. The supplied ten saves cover Yellow, Crystal, Emerald,
Platinum, Black2, Red Rescue Team, Explorers of Sky, Pinball Ruby & Sapphire,
Ranger, and Shuffle (20 regression cases). Other validators require additional saves.

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

## EventSimulator regression tests

The suite enumerates every member of the seven `PK_EVENT*` enums and invokes
`EventSimulator.Execute` on fresh bytes from the corresponding supplied save.
There are 60 named-event cases, plus the missing-secondary-save Aurora Ticket
case and unsupported-generation cases for Red Rescue Team, Explorers of Sky,
and Ranger (64 event cases; 85 tests in the full suite).

Each `Fixtures/Events/<game>.<event>/result.json` contains the event name,
input filenames and SHA-256 hashes, version hint, return value, exact error
(including null on success), and ordered messages. `OutputSaveASha256` is the
SHA-256 of the complete bytes returned by `Command.GetSavA()`, including on
errors. `OutputSaveBSha256` hashes `Command.GetSavB()` when a second input is
supplied, and is null otherwise. Resulting save binaries are not stored.

The tests compare the output SHA-256 hashes along with the messages and metadata.
Input files are never overwritten. Static command state is reset around each
capture and the suite runs serially. Fixture generation uses the same capture
helper and captures all cases before writing any baseline files.

Generate only the event fixtures explicitly:

```powershell
dotnet run --project tests/PkCompletionist.Tests -- --generate-event-fixtures tests/PkCompletionist.Tests
```

Existing saves determine whether each event succeeds or returns an error; these
tests capture current behavior rather than force every event to succeed. All
enum names automatically become cases, so adding an event without generating
its fixtures fails the tests.

Emerald event cases use `Pokemon Emerald - Events.sav`. Platinum's Aurora Ticket
event uses that same save as save B and checks both output hashes. Completion
and sorter tests continue to use `Pokemon Emerald.sav`.
Platinum event cases use `Pokemon Platinum - Events.sav`, including both
independently loaded inputs for Ranch Mew. The outcome depends on that save's
Pokemon count. Completion tests use `Pokemon Platinum.sav`.
Black2's Ranch Mew case uses a single save
and captures the count requirement error: supplying two copies currently throws
a PK4/PK5 format exception, which is outside this snapshot case.

Crystal's `MysteryGift` and `MysteryGiftx1000` cases capture the missing-save-B
error and resulting save A. Successful two-player gifts use an unseeded random
generator, so their random outcomes are not covered by output hash fixtures.
No production behavior is changed to generate these baselines.
