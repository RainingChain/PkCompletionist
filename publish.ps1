robocopy "C:\Users\Samuel\source\repos\PkCompletionist\bin\Release\net10.0\browser-wasm\AppBundle" "C:\Users\Samuel\source\repos\pokemoncompletion\src\pokemonCompletion\PkCompletionist" "/E"
$dotnetBuildIdPath = "C:\Users\Samuel\source\repos\pokemoncompletion\src\pokemonCompletion\dotnetBuildId.ts"
$dotnetBuildId = [DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()
Set-Content -Path $dotnetBuildIdPath -Value "export const dotnetBuildId = ""$dotnetBuildId"";"
