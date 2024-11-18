## Pokemon Completionist Utilities

Utilities for the Pokemon 100% Checklist Challenge that can be embedded in a website or ran as standalone .exe.

### Features
- Extract obtained elements from .sav
- Trigger events
- Sort PC boxes
- Simulate Mystery Gift (Generation 2)
- Tools to research flags

### Credits:
- RainingChain
- Kurt for [PKHeX](https://github.com/kwsch/PKHeX)
- FabioAttard for [event flag research](https://github.com/fattard/MissingEventFlagsCheckerPlugin)

### Contributing
- Install Visual Studio 2022 
- Add Visual Studio support for .NET (C#) projects
- To compile browser version (wasm):
	- dotnet workload install wasm-tools-net7
	- dotnet publish --configuration Release
- To compile desktop version (exe):
	- Install .NET 7.0 Runtime (v7.0.20)
	- Modify PkCompletionist.csproj <When Condition="'1' == '0'">
	- dotnet publish --configuration Release