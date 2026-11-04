$ErrorActionPreference = 'Stop'
$repoDirectory = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$source = Get-Content (Join-Path $repoDirectory 'PkCompletionist.Core/Gen4/Enum4.cs') -Raw
$source = [regex]::Replace($source,'//[^\r\n]*','')
$output = [Collections.Generic.List[string]]::new()
$output.Add('// Constants ported from Gen4/Enum4.cs; aliases and source spellings are retained.')
$output.Add('#![allow(non_snake_case, non_upper_case_globals)]')
foreach ($enum in [regex]::Matches($source,'public enum (\w+)(?:\s*:\s*\w+)?\s*\{([^}]+)\}','Singleline')) {
    $output.Add('pub mod ' + $enum.Groups[1].Value + ' {')
    $index = 0; $values = @{}
    foreach ($entry in $enum.Groups[2].Value.Split(',')) {
        $entry = $entry.Trim(); if (!$entry) {continue}
        $parts = $entry.Split('='); $name = $parts[0].Trim()
        if ($parts.Length -gt 1) { $value = $parts[1].Trim(); $index = if ($values.ContainsKey($value)) {$values[$value]} else {[int]$value} }
        $output.Add('    pub const ' + $name + ': usize = ' + $index + ';')
        $values[$name] = $index; $index++
    }
    $output.Add('}')
}
$destination = Join-Path $repoDirectory 'rust/PkCompletionist.Core/Gen4/Enum4.rs'
[IO.Directory]::CreateDirectory((Split-Path $destination -Parent)) | Out-Null
$output | Set-Content $destination
