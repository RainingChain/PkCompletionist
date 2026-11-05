# Reproduce literal Black2 rules and the required upstream block/personal tables.
$ErrorActionPreference = 'Stop'
$repoDirectory = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
Set-Location $repoDirectory
$source = Get-Content 'PkCompletionist.Core/Gen5/CompletionValidator5.cs' -Raw
$source = [regex]::Replace($source, '/\*.*?\*/', '', 'Singleline')
$source = [regex]::Replace($source, '//[^\r\n]*', '')
$blocks = [regex]::Matches((Get-Content 'PKHeX.Core/Saves/Access/SaveBlockAccessor5B2W2.cs' -Raw), 'new\((0x[\da-fA-F]+),\s*(0x[\da-fA-F]+),\s*(0x[\da-fA-F]+),\s*(0x[\da-fA-F]+)\)')
$blockText = "// Literal block table from SaveBlockAccessor5B2W2.cs.`npub const BLOCKS: &[(usize, usize, usize, usize)] = &[`n"
foreach ($m in $blocks) { $blockText += '    (' + (($m.Groups | Select-Object -Skip 1 | ForEach-Object Value) -join ', ') + "),`n" }
$blockText += "];`n"
New-Item -ItemType Directory 'rust/PKHeX.Core/Saves/Access' -Force | Out-Null
[IO.File]::WriteAllText((Join-Path $PWD 'rust/PKHeX.Core/Saves/Access/SaveBlockAccessor5B2W2.rs'), $blockText)
Copy-Item 'PKHeX.Core/Resources/byte/personal/personal_b2w2' 'rust/PKHeX.Core/Resources/byte/personal/personal_b2w2'
$storage = Get-Content 'PKHeX.Core/Items/ItemStorage5.cs' -Raw
$storage = [regex]::Replace($storage, '//[^\r\n]*', '')
$itemText = "// Inventory identifiers from ItemStorage5.cs; no legality rules.`n"
foreach ($name in @('General','Machine','Medicine','Berry')) {
    $values = [regex]::Match($storage, ($name + ' =>\s*\[(.*?)\]'), 'Singleline').Groups[1].Value
    $values = [regex]::Replace($values, '\b0+(\d+)', '$1')
    $itemText += 'pub const ' + $name.ToUpperInvariant() + ": &[u16] = &[$values];`n"
}
[IO.File]::WriteAllText((Join-Path $PWD 'rust/PKHeX.Core/Items/ItemStorage5.rs'), $itemText)
$keys = [regex]::Match((Get-Content 'PKHeX.Core/Items/ItemStorage5B2W2.cs' -Raw), 'Key =>\s*\[(.*?)\]', 'Singleline').Groups[1].Value
[IO.File]::WriteAllText((Join-Path $PWD 'rust/PKHeX.Core/Items/ItemStorage5B2W2.rs'), "pub const KEY: &[u16] = &[$keys];`n")
$pwtNames = [regex]::Match((Get-Content 'PKHeX.Core/Saves/Substructures/Gen5/PWTRecordID.cs' -Raw), '\{(.*?)\}', 'Singleline').Groups[1].Value.Split(',') | ForEach-Object { $_.Trim() } | Where-Object { $_ }
$pwt = @{}; for ($i = 0; $i -lt $pwtNames.Count; $i++) { $pwt[$pwtNames[$i]] = $i }
$pkSource = Get-Content 'PKHeX.Core/PKM/PK5.cs' -Raw
$ribOffsets = @{}
foreach ($m in [regex]::Matches($pkSource, 'private byte (RIB\w) \{ get => Data\[(0x\w+)\]')) { $ribOffsets[$m.Groups[1].Value] = $m.Groups[2].Value }
$ribbons = @{}
foreach ($m in [regex]::Matches($pkSource, 'public bool (Ribbon\w+)\s+\{ get => \((RIB\w) & \(1 << (\d+)\)')) { $ribbons[$m.Groups[1].Value] = @($ribOffsets[$m.Groups[2].Value], $m.Groups[3].Value) }
function Body([string]$name) {
    $m = [regex]::Match($source, ('\b' + $name + '\([^)]*\)\s*\{'))
    if (!$m.Success) { throw "Missing method $name" }
    $start = $m.Index + $m.Length; $depth = 1; $end = $start
    while ($depth -gt 0) { if ($source[$end] -eq '{') {$depth++}; if ($source[$end] -eq '}') {$depth--}; $end++ }
    $source.Substring($start, $end - $start - 1)
}
function Convert-Rule([string]$expression) {
    $expression = $expression.Trim().Replace('this.', '')
    $expression = [regex]::Replace($expression, 'sav.PWT.GetPWTRecord\(PWTRecordID\.(\w+)\)', {param($m) 'self.sav.pwt(' + $pwt[$m.Groups[1].Value] + ')' })
    $expression = [regex]::Replace($expression, 'HasRibbon\(pk4 => pk4\.(\w+)\)', {param($m) $r=$ribbons[$m.Groups[1].Value]; if (!$r) { throw "Unknown ribbon $m" }; 'self.HasRibbon(' + $r[0] + ', ' + $r[1] + ')' })
    $expression = $expression.Replace('sav.EventWork.GetEventFlag(', 'self.sav.event(')
    foreach ($pair in @(@('SingleRecord','0x1a'),@('DoubleRecord','0x1c'),@('MultiNPCRecord','0x1e'),@('SuperSingleRecord','0x24'),@('SuperDoubleRecord','0x26'),@('SuperMultiNPCRecord','0x28'),@('SuperMultiFriendsRecord','0x2a'))) {
        $expression = $expression.Replace(('sav.BattleSubway.' + $pair[0]), ('u16_le(&self.sav.data, 0x21b00 + ' + $pair[1] + ').unwrap()'))
    }
    $expression = $expression.Replace('HasShinyMon()', 'self.pokemon.iter().any(|pk| pk.shiny())')
    $expression = [regex]::Replace($expression, '\byancySpecies.Contains\((\d+)\)', 'self.HasTradeMon($1, 10303, "Yancy")')
    $expression = [regex]::Replace($expression, '\bcurtisSpecies.Contains\((\d+)\)', 'self.HasTradeMon($1, 54118, "Curtis")')
    $expression = [regex]::Replace($expression, '\b(HasOrSeenPkmBasedOnObjectiveArceus|HasOrSeenPkmBasedOnObjective2|HasOrSeenPkmBasedOnObjective|HasSeenPkmForm|has_rank_10_join_avenue_shop)\(', 'self.$1(')
    $expression = $expression.Replace('lvl == 100', 'u16_le(&self.sav.data, 0x24fcc).unwrap() == 100')
    if ($expression.Replace('self.sav.', '') -match '\bsav\.|nextForm|Species.Contains|HasRibbon\(pk4') { throw "Unconverted rule: $expression" }
    $expression
}
$destination = 'rust/PkCompletionist.Core/Gen5/CompletionValidator5.rs'
$prefix = (Get-Content $destination -Raw).Split('// Generated literal rules follow.')[0]
$generated = "// Generated literal rules follow. See tools/port-black2-rules.ps1.`nimpl CompletionValidator5 {`n"
foreach ($name in @('pokemonForm','inGameTrade','inGameGift','pwt','ribbon','battleSubway','joinAvenue')) {
    $body = Body "Generate_$name"
    if ($name -eq 'pokemonForm') { $body = $body.Substring(0, $body.IndexOf('ow["AllMaleFemaleForms"]')) }
    $category = [regex]::Match($body, 'owned\["([^"]+)"\]').Groups[1].Value
    $generated += "fn Generate_$name(&self, result: &mut CommandResult) {`nlet mut ow = Checklist::default();`n"
    $form = 0
    foreach ($m in [regex]::Matches($body, 'ow\[(?:"([^"]+)"|fmt\("([^"]+)"\))\]\s*=\s*([^;]+);')) {
        $key = $m.Groups[1].Value
        if (!$key) { $key = [regex]::Replace($m.Groups[2].Value.Replace('é','e'), '[^A-Za-z0-9]', '') }
        $expression = $m.Groups[3].Value
        if ($expression.Contains('nextForm++')) { $expression = $expression.Replace('nextForm++', "$form"); $form++ }
        $generated += 'ow.set("' + $key + '", ' + (Convert-Rule $expression) + ");`n"
    }
    if ($name -eq 'pokemonForm') { $generated += "self.CompletePokemonForms(&mut ow, result);`n" }
    $generated += 'ow.emit(result, "' + $category + '");' + "`n}`n"
}
$generated += "}`n"
$unobtainable = [regex]::Match($source, 'unobtainableItems = new List<int>\(\) \{([^}]+)\}').Groups[1].Value
$generated += "const UNOBTAINABLE: &[u16] = &[$unobtainable];`n"
$notrade = [regex]::Match((Body 'Generate_easyChatSystemWord'), 'new List<ushort> \{([^}]+)\}').Groups[1].Value
$generated += "const NO_TRADE: &[u16] = &[$notrade];`n"
$forms = [regex]::Match($source, 'var specialForms = .*?\{(.*?)\};', 'Singleline').Groups[1].Value
$generated += "const SPECIAL_FORMS: &[(&str, u16, u8)] = &[$forms];`n"
[IO.File]::WriteAllText((Join-Path $PWD $destination), $prefix + $generated)
