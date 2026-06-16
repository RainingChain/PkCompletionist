## Build And Run

- To compile the Windows exe, use the VS Code task `compile: exe`.
- Equivalent command:

```powershell
dotnet publish ./PkCompletionist.csproj --configuration Debug --runtime win-x64 --self-contained false
```

- The compiled exe is written to:

```text
bin/Debug/net10.0/win-x64/publish/PkCompletionist.exe
```

- Use this exe path for command-line testing after compiling. Example:

```powershell
& "./bin/Debug/net10.0/win-x64/publish/PkCompletionist.exe" validate "C:\Users\Samuel\Game\DS\ROM\Pokemon Black2.sav"
```

The list of possible arguments are listed in Program.cs. For example: validate, event.

## WASM Tasks

- `compile: wasm` publishes `PkCompletionist.csproj` in `Release` for `browser-wasm`.
- `publish: wasm` depends on `compile: wasm` and copies the AppBundle with `robocopy` to:

```text
C:/Users/Samuel/source/repos/pokemoncompletion/src/pokemonCompletion/PkCompletionist
```
which is another project that depends on PkCompletionist.

## Project Notes

- The main build target is `PkCompletionist.csproj` at the repository root.
- The project targets `net10.0`.
- Do not casually change the target framework to `net8.0`; the project file notes that browser WASM fetch behavior breaks the current JS fetch override.
- `AllowUnsafeBlocks` is enabled in the root project and required for generated JS export code.
- `PkCompletionist.Core/PkCompletionist.Core.csproj` is not the preferred compile target for the application exe.

## Editing Notes

- Prefer small, focused changes.
- Preserve user changes in the working tree.
- Use `PkCompletionist.Core` for core command and save-manipulation code.
- Don't modify `PKHeX.Core`. If it's absolutely necessary to adapt it, ask for permission first.
