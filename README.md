## Pokemon Completionist Utilities

Utilities for the Pokemon 100% Checklist Challenge that can be embedded in a website or ran as standalone .exe.

### Features
- Extract obtained elements from .sav
- Trigger events
- Sort PC boxes
- Simulate Mystery Gift (Generation 2)
- Tools to research flags

### Contributing

#### Setup in Visual Studio Code

1. Install [Visual Studio Code](https://code.visualstudio.com/) and the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
2. Open the repository root folder in VS Code so that `.vscode/tasks.json` is loaded.
3. Open **Terminal > New Terminal** and use PowerShell for the commands below.
4. For browser builds, install the [WebAssembly build tools](https://learn.microsoft.com/en-us/aspnet/core/client-side/dotnet-interop/wasm-browser-app):

   ```powershell
   dotnet workload install wasm-tools
   ```

Build the root `PkCompletionist.csproj`. The tasks select the runtime automatically; no project-file edits are needed to switch between desktop and browser builds.

#### Compile the Windows executable

Open the Command Palette (`Ctrl+Shift+P`), select **Tasks: Run Task**, then select **compile: exe**.
This builds the Debug version for Windows x64 and writes the executable to:

```text
bin/Debug/net10.0/win-x64/publish/PkCompletionist.exe
```

The executable requires the .NET 10 runtime on the machine running it. The SDK installed above includes the runtime for local use.

Run it from the VS Code terminal, replacing the save path with your own:

```powershell
& "./bin/Debug/net10.0/win-x64/publish/PkCompletionist.exe" validate "C:\path\to\Pokemon Black2.sav"
```

See `Program.cs` for the supported commands and arguments.

#### Compile the browser version (WASM)

Use **Tasks: Run Task > compile: wasm** to build the Release browser version. The output is written to:

```text
bin/Release/net10.0/browser-wasm/AppBundle/
```

To copy the browser build into the companion website, first update the paths in `publish.ps1` for your checkout. The script copies the AppBundle to the website's `PkCompletionist` folder and updates `dotnetBuildId.ts`.

- **compil & publish: wasm** builds the browser version, then runs `publish.ps1`.
- **publish: wasm** runs `publish.ps1` using the existing build; it does not compile first.

#### Run the C# regression tests

In the VS Code terminal, run:

```powershell
dotnet test tests/PkCompletionist.Tests/PkCompletionist.Tests.csproj
```

See [the test README](tests/PkCompletionist.Tests/README.md) for coverage and fixture-generation instructions.

### Credits:
- RainingChain
- Kurt for [PKHeX](https://github.com/kwsch/PKHeX)
- FabioAttard for [event flag research](https://github.com/fattard/MissingEventFlagsCheckerPlugin)
- supercarotte for [Pokemon Shuffle Save Editor](https://github.com/supercarotte/PSSE)
- Bl4ckSh4rk for [rangercrypt](https://github.com/Bl4ckSh4rk/rangercrypt)
- evandixon for [SkyEditor.SaveEditor](https://github.com/evandixon/SkyEditor.SaveEditor)
 