cd C:\Users\Samuel\source\repos\PkCompletionist
dotnet publish --configuration Release
robocopy C:\Users\Samuel\source\repos\PkCompletionist\bin\Release\net7.0\browser-wasm\AppBundle C:\Users\Samuel\source\repos\pokemoncompletion\src\pokemonCompletion\PkCompletionist /E
