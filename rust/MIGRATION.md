Migration constraints:

- Keep the C# implementation, saves, and fixtures as the reference until full parity.
- Mirror every `PkCompletionist.Core/*.cs` path in Rust; do not substitute empty stubs.
- Keep application rules and the PKHeX subset in separate crates.
- WASM is the product target. Native command-line execution is through tests only.
- Exclude PKHeX legality analysis, move metadata, and move logic. Preserve raw entity data.
- Do not modify the original `PKHeX.Core` directory.

Verified feature scope so far:

| Feature | Status / current limits |
| --- | --- |
| Yellow completion (normal/living) | Existing fixtures match; international saves only |
| Crystal completion (normal/living) | Existing fixtures match; international Crystal only |
| Emerald completion (normal/living) | Existing fixtures match |
| Pinball Ruby & Sapphire completion | Existing fixtures match |
| Shuffle completion | Existing fixtures match |
| Ranger completion and decryption | Existing fixtures match |
| Red Rescue Team completion | Existing fixtures match |
| Explorers of Sky completion | Existing fixtures match |
| Original Pinball completion | Ported; requires `Pinball` version hint; no supplied save fixture |
| Events for the above games | Existing event fixtures match, including output hashes |
| Crystal Mystery Gift with two saves | Ported with an injected random source; invariant tests cover item outcomes and early termination |
| Crystal flag analysis | Existing messages and both complete output hashes match |
| Checksum serialization for supported saves | Implemented; exercised by event/flag output hashes |
| Emerald sorting | Species order and complete output-save hash match the C# fixture |
| Platinum, Black2 | Pending save/PKM, completion, and event ports |
| Other sorting and merging | Pending, including cross-generation entity conversion |
| Descriptors | Eight migrated formats match their C# fixtures |
| Other save formats reachable through generic C# commands | Must be inventoried and migrated; not implicitly removed from scope |
| Website loader and publishing cutover | Pending full parity; existing C# publishing remains the reference |

The native regression suite iterates 16 completion fixtures, 28 event fixtures, eight
descriptor fixtures, and the Crystal FlagAnalyzer fixture. Additional tests cover
malformed inputs, Pinball mutation and backup serialization, Shuffle caught-bit agreement,
bit-field behavior, Mystery Gift, all PK3 block permutations, and Emerald sorting.
The JavaScript suite checks the public adapter through the browser-targeted module.
Existing fixtures characterize supplied saves; they do not prove full game, language, or branch coverage.

Next migration gates:

1. Finish the source/member/resource inventory rooted at every command and JS export.
   `SaveUtil` and `EntityConverter` reach more formats than the validators do.
2. Add additional branch fixtures, especially languages, invalid saves,
   empty/full inventory and boxes, and successful two-save operations.
3. Extend the migrated Gen1–3 formats beyond the supplied international-save fixtures.
4. Port Platinum/PK4 and Black2/PK5, their commands and resources, then merging/conversion.
5. Complete every remaining core-file counterpart and reachable generic-command format.
6. Run all baseline cases natively and in WASM, integrate the website loader, then switch
   publishing. Remove the C# runtime only once that gate passes.

The scripts in `tools/port-*-rules.ps1` reproduce literal rule tables from the preserved
C# source. Generated Rust is checked in and compiled directly; building/running Rust
does not invoke C# or require .NET. Existing C# quirks, ordering, messages, and deterministic
output bytes are preserved by the snapshots; intentional behavior changes need separate review.
