PkCompletionist Rust migration

Application code and resources are derived from the corresponding paths in
`PkCompletionist.Core/` in this repository. The repository's GPL v3 license is retained.

The selected PKHeX ports are derived from PKHeX by Kurt and contributors, from bundled
revision `10016c6fe5b732a113c3ca732116ea8976dc1f2c`, recorded in `PkHeX.Core.version.txt`.
The upstream project declares `GPL-3.0-or-later` in `PKHeX.Core.csproj`.
Source files retain their corresponding relative paths under `rust/PKHeX.Core`.
No upstream legality engine or move database is included.

`PkCompletionist.Core/Gen3c/BitBlock.rs` derives from the C# BitBlock implementation,
which credits evandixon/SkyEditor.SaveEditor under MIT. Existing game-specific code
also credits PSSE, rangercrypt, and SkyEditor.SaveEditor; those source credits remain
in the C# reference and the root README.

The introsort implementation in `PkCompletionist.Core/PkSorter.rs` follows .NET 10's
`ArraySortHelper.cs` from dotnet/runtime, copyright .NET Foundation and contributors,
licensed under MIT. It preserves the C# sort's ordering for equal keys.
