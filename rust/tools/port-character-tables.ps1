# Copy the actual English character tables, resolving the shared C# char constants.
$ErrorActionPreference = 'Stop'
$repoDirectory = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$constants = @{}
function Get-Character([string]$token) {
    $token = $token.Trim()
    if ($token.StartsWith("'")) {
        $literal = $token.Substring(1,$token.Length - 2)
        if ($literal -eq '\0') { return [char]0 }
        if ($literal -eq "\'") { return [char]39 }
        if ($literal -eq '\\') { return [char]92 }
        if ($literal.Length -ne 1) { throw ('Unsupported char literal ' + $token) }
        return $literal[0]
    }
    $key = ($token -split '\.')[-1]
    if (!$constants.ContainsKey($key)) { throw ('Unresolved character ' + $token) }
    return $constants[$key]
}
foreach ($generation in 1..2) {
    $relative = 'PKHeX.Core/PKM/Strings/StringConverter' + $generation + '.cs'
    $source = Get-Content (Join-Path $repoDirectory $relative) -Raw
    $source = [regex]::Replace($source,'//[^\r\n]*','')
    foreach ($constant in [regex]::Matches($source,'const char (\w+) = ([^;]+);')) { $constants[$constant.Groups[1].Value] = Get-Character $constant.Groups[2].Value }
    $table = [regex]::Match($source,'TableEN\s*=>\s*\[(.*?)\];','Singleline').Groups[1].Value
    $tokens = [regex]::Matches($table, "'(?:\\.|[^'\\])*'|[A-Z][A-Z0-9]*")
    if ($tokens.Count -ne 256) { throw ('Wrong table size: ' + $tokens.Count) }
    $output = [Collections.Generic.List[string]]::new()
    $output.Add('// English character decoding table ported from StringConverter' + $generation + '.cs.')
    $output.Add('pub fn decode(bytes: &[u8]) -> String {')
    $output.Add('    if bytes.first() == Some(&0x5d) { return "*".to_string(); }')
    $output.Add('    bytes.iter().map(|byte| TABLE_EN[*byte as usize]).take_while(|c| *c != ''\0'').collect()')
    $output.Add('}')
    $output.Add('const TABLE_EN: [char;256] = [')
    foreach ($token in $tokens) { $character = Get-Character $token.Value; $output.Add("    '\u{" + ([int]$character).ToString('x') + "}',") }
    $output.Add('];')
    $destination = Join-Path $repoDirectory ('rust/PKHeX.Core/PKM/Strings/StringConverter' + $generation + '.rs')
    [IO.Directory]::CreateDirectory((Split-Path $destination -Parent)) | Out-Null
    $output | Set-Content $destination
}
# Generation 3 uses its own terminator and gender symbols.
$source = Get-Content (Join-Path $repoDirectory 'PKHeX.Core/PKM/Strings/StringConverter3.cs') -Raw
$source = [regex]::Replace($source,'//[^\r\n]*','')
$constants['Terminator'] = [char]0xff
$constants['FGM'] = [char]0x2642
$constants['FGF'] = [char]0x2640
$constants['HGM'] = [char]0x246d
$constants['HGF'] = [char]0x246e
$output = [Collections.Generic.List[string]]::new()
$output.Add('// StringConverter3 decoding subset. English/Japanese tables preserve source order.')
$output.Add('pub fn decode(bytes: &[u8], japanese: bool) -> String {')
$output.Add('    let table = if japanese { &TABLE_JP } else { &TABLE_EN };')
$output.Add('    bytes.iter().map(|byte| table[*byte as usize]).take_while(|c| *c != ''\u{ff}'').map(|c| match c {''\u{246d}'' => ''♂'', ''\u{246e}'' => ''♀'', _ => c}).collect()')
$output.Add('}')
foreach ($pair in @(@('G3_EN','TABLE_EN'),@('G3_JP','TABLE_JP'))) {
    $table = [regex]::Match($source,($pair[0] + '\s*=>\s*\[(.*?)\];'),'Singleline').Groups[1].Value
    $tokens = [regex]::Matches($table,"'(?:\\.|[^'\\])*'|[A-Za-z][A-Za-z0-9]*")
    if ($tokens.Count -ne 256) {throw ('Wrong Gen3 table size ' + $tokens.Count)}
    $output.Add('const ' + $pair[1] + ': [char;256] = [')
    foreach ($token in $tokens) {$character = Get-Character $token.Value; $output.Add("    '\u{" + ([int]$character).ToString('x') + "}',")}
    $output.Add('];')
}
$output | Set-Content (Join-Path $repoDirectory 'rust/PKHeX.Core/PKM/Strings/StringConverter3.rs')
