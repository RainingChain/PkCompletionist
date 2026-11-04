The Rust application targets WebAssembly. Native execution is through tests; there is no CLI executable.

The directory layout mirrors the C# project:

```text
rust/
  Cargo.toml                 WASM application and Cargo workspace
  Program.rs                 JavaScript bindings
  main.js                    compatibility adapter for the browser
  PkCompletionist.Core/       application commands and game rules
  PKHeX.Core/                 required upstream save/PKM subset
  tests/                     native and JavaScript/WASM regression tests
```

C# filenames retain their spelling and relative directories with `.rs` replacing `.cs`.
`lib.rs` files register those modules explicitly. Application-specific game handling stays
in `PkCompletionist.Core`; PKHeX format handling stays in `PKHeX.Core`.
Validator structs retain the C# class names and `GenerateAll` / `Generate_*` method
boundaries and call order, including `Generate_itemGift`, `Generate_itemInMap`, and
`Generate_battle`. Rule-generation scripts preserve these boundaries when regenerated.
Legality analysis and move data/logic are excluded. Raw move bytes in Pokémon records are preserved.

Build the browser module (requires Rust's `wasm32-unknown-unknown` target and `wasm-pack`):

```powershell
wasm-pack build rust --target web --dev
```

The generated ES module, bindings, TypeScript declarations, and `.wasm` are in `rust/pkg/`.
Serve this directory over HTTP and use `await load()` from `rust/main.js`, which returns
the C#-style export groups already migrated. Each API instance owns its state; call
`dispose()` when finished. The consuming website still uses its C# loader until full parity.

Run tests from the repository root:

```powershell
cargo test --manifest-path rust/Cargo.toml --workspace --locked
node --test rust/tests/WasmRegressionTests.mjs
```

The Node tests execute the **web-targeted WASM artifact**, supplying the WASM bytes where
the browser would fetch them. Rebuild before running them after Rust changes.
Both suites reuse the committed C# save/JSON fixtures. Tests never regenerate baselines.
`cargo fmt --manifest-path rust/Cargo.toml --all -- --check` and
`cargo clippy --manifest-path rust/Cargo.toml --workspace --all-targets -- -D warnings`
check formatting and diagnostics.

See [MIGRATION.md](MIGRATION.md) for current coverage and the remaining work. This is an
incremental migration, not yet a replacement for all C# functionality.
