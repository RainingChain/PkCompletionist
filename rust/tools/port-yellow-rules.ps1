# Reproduce Generation 1 rule and species/experience tables from the C# reference.
$ErrorActionPreference = 'Stop'
$repoDirectory = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$coreDirectory = Join-Path $repoDirectory 'rust/PkCompletionist.Core/Gen1'
$source = Get-Content (Join-Path $repoDirectory 'PkCompletionist.Core/Gen1/CompletionValidator1.cs') -Raw
$source = [regex]::Replace($source, '/\*.*?\*/', '', 'Singleline')
$source = [regex]::Replace($source, '//[^\r\n]*', '')
$destination = Join-Path $coreDirectory 'CompletionValidator1.rs'
$existing = Get-Content $destination -Raw
$marker = $existing.IndexOf('// Generated literal tables and simple rules follow.')
$output = [Collections.Generic.List[string]]::new()
$output.Add($existing.Substring(0,$marker).TrimEnd())
$output.Add('// Generated literal tables and simple rules follow. See tools/port-yellow-rules.ps1.')
$output.Add('impl CompletionValidator1 {')
foreach ($pair in @(@('inGameGift','in_game_gift'), @('inGameTrade','in_game_trade'))) {
    $body = [regex]::Match($source, ('void Generate_' + $pair[0] + '\(\)\s*\{(.*?)\n    \}'), 'Singleline').Groups[1].Value
    $category = [regex]::Match($body, 'owned\["([^"]+)"\]').Groups[1].Value
    $output.Add('fn Generate_' + $pair[0] + '(&self,result: &mut CommandResult) {')
    $output.Add('    let save = &self.save;')
    $output.Add('    let mut entries = Checklist::default();')
    foreach ($rule in [regex]::Matches($body, 'ow\["([^"]+)"\]\s*=\s*([^;]+);')) {
        $expression = $rule.Groups[2].Value.Trim().Replace('this.sav','sav').Replace('sav.GetEventFlag(', 'save.event(').Replace('sav.GetFlag(', 'save.flag(')
        $expression = [regex]::Replace($expression,'sav.EventSpawnFlags\[([^\]]+)\]', 'save.flag(0x2852,$1)')
        $output.Add('    entries.set("' + $rule.Groups[1].Value + '", ' + $expression + ');')
    }
    $output.Add('    entries.emit(result,"' + $category + '");'); $output.Add('}')
}
$output.Add('}')
$output.Add('const ITEM_FLAGS: &[(usize,usize,usize)] = &[')
foreach ($rule in [regex]::Matches($source, 'Add\((\d+),\s*(0x[\dA-Fa-f]+|\d+),\s*(\d+)\)')) { $output.Add('    (' + $rule.Groups[1].Value + ',' + $rule.Groups[2].Value + ',' + $rule.Groups[3].Value + '),') }
$output.Add('];')
$onlyFlags = [regex]::Match($source,'itemOnlyWithFlags = new int\[\]\s*\{([^}]+)\}').Groups[1].Value
$output.Add('const ITEM_ONLY_WITH_FLAGS: &[usize] = &[' + $onlyFlags + '];')
foreach ($pair in @(@('fieldAddr','FIELD_ITEMS'), @('hiddenAddr','HIDDEN_ITEMS'), @('hiddenCoinAddr','HIDDEN_COINS'))) {
    $body = [regex]::Match($source, ($pair[0] + ' = new List<int>\s*\{([^}]+)\}')).Groups[1].Value
    if (!$body) { throw ('Missing list ' + $pair[0]) }
    $output.Add('const ' + $pair[1] + ': &[usize] = &[' + $body + '];')
}
$battle = [regex]::Match($source, 'void Generate_battle\(\)\s*\{(.*?)\n    \}', 'Singleline').Groups[1].Value
$body = [regex]::Match($battle,'var list = new List<int>\s*\{([^}]+)\}').Groups[1].Value
$output.Add('const BATTLES: &[usize] = &[' + $body + '];')
$output | Set-Content $destination
$species = Get-Content (Join-Path $repoDirectory 'PKHeX.Core/PKM/Util/Conversion/SpeciesConverter.cs') -Raw
$body = [regex]::Match($species,'Table1InternalToNational\s*=>\s*\[([^\]]+)\]').Groups[1].Value
$target = Join-Path $repoDirectory 'rust/PKHeX.Core/PKM/Util/Conversion/SpeciesConverter.rs'
[IO.Directory]::CreateDirectory((Split-Path $target -Parent)) | Out-Null
@('// Ported from SpeciesConverter.cs; Gen1 species identifiers only.', 'pub fn national1(raw: u8) -> u8 { TABLE1_INTERNAL_TO_NATIONAL[raw as usize] }', ('const TABLE1_INTERNAL_TO_NATIONAL: &[u8] = &[' + $body + '];')) | Set-Content $target
$experience = Get-Content (Join-Path $repoDirectory 'PKHeX.Core/PKM/Util/Experience.cs') -Raw
$experience = [regex]::Replace($experience, '//[^\r\n]*', '')
$output = [Collections.Generic.List[string]]::new()
$output.Add('// Experience thresholds ported from Experience.cs, indexed by level minus one.')
$output.Add('pub fn level(exp: u32, growth: u8) -> u8 { TABLES.get(growth as usize).unwrap_or(&TABLES[0]).iter().take_while(|threshold| **threshold <= exp).count().clamp(1,100) as u8 }')
$output.Add('const TABLES: [&[u32];6] = [')
foreach ($index in 0..5) {
    $body = [regex]::Match($experience, ('Growth' + $index + '\s*=>\s*\[([^\]]+)\]'), 'Singleline').Groups[1].Value
    if (!$body) { throw 'Missing growth table' }
    $body = [regex]::Replace($body, '\b0[0-9]+\b', {param($m) ([int]$m.Value).ToString()})
    $output.Add('&[' + $body + '],')
}
$output.Add('];')
$output | Set-Content (Join-Path $repoDirectory 'rust/PKHeX.Core/PKM/Util/Experience.rs')
$personalDirectory = Join-Path $repoDirectory 'rust/PKHeX.Core/Resources/byte/personal'
[IO.Directory]::CreateDirectory($personalDirectory) | Out-Null
Copy-Item -LiteralPath (Join-Path $repoDirectory 'PKHeX.Core/Resources/byte/personal/personal_y') -Destination $personalDirectory
$species = [regex]::Replace($species, '//[^\r\n]*', '')
$output = [Collections.Generic.List[string]]::new()
$output.Add('pub fn national3(raw: u16) -> u16 { if raw < 252 { return raw; } let Some(index) = raw.checked_sub(277) else { return 0; }; TABLE3_INTERNAL_TO_NATIONAL.get(index as usize).map_or(0, |delta| (i32::from(raw) + i32::from(*delta)) as u16) }')
$output.Add('pub fn internal3(species: u16) -> u16 { let Some(index) = species.checked_sub(252) else { return species; }; TABLE3_NATIONAL_TO_INTERNAL.get(index as usize).map_or(species, |delta| (i32::from(species) + i32::from(*delta)) as u16) }')
foreach ($pair in @(@('Table3InternalToNational','TABLE3_INTERNAL_TO_NATIONAL'), @('Table3NationalToInternal','TABLE3_NATIONAL_TO_INTERNAL'))) {
    $body = [regex]::Match($species, ($pair[0] + '\s*=>\s*\[([^\]]+)\]'), 'Singleline').Groups[1].Value
    if (!$body) { throw ('Missing Gen3 species table ' + $pair[0]) }
    $numbers = [regex]::Matches($body,'-?\d+') | ForEach-Object { ([int]$_.Value).ToString() }
    $output.Add('const ' + $pair[1] + ': &[i16] = &[' + ($numbers -join ',') + '];')
}
$output | Add-Content (Join-Path $repoDirectory 'rust/PKHeX.Core/PKM/Util/Conversion/SpeciesConverter.rs')
