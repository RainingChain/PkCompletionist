# Reproduce Crystal literal tables while preserving C# source order.
$ErrorActionPreference = 'Stop'
$repoDirectory = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$coreDirectory = Join-Path $repoDirectory 'rust/PkCompletionist.Core/Gen2'
$source = Get-Content (Join-Path $repoDirectory 'PkCompletionist.Core/Gen2/CompletionValidator2.cs') -Raw
$source = [regex]::Replace($source, '//[^\r\n]*', '')
$constants = Get-Content (Join-Path $repoDirectory 'PkCompletionist.Core/Gen2/Cnst2.cs') -Raw
$constants = [regex]::Replace($constants, '//[^\r\n]*', '')
$maps = @{}
$output = [Collections.Generic.List[string]]::new()
$output.Add('// Literal tables ported from Gen2/Cnst2.cs. Regenerate with tools/port-crystal-rules.ps1.')
foreach ($pair in @(@('EventNameToAddress','EVENT_NAME_TO_ADDRESS'), @('EventWorkNameToAddress','EVENT_WORK_NAME_TO_ADDRESS'), @('TrainerToFlag','TRAINER_TO_FLAG'))) {
    $body = [regex]::Match($constants, ($pair[0] + '\s*=\s*new Dictionary<string,\s*int>\s*\{(.*?)\};'), 'Singleline').Groups[1].Value
    if (!$body) { throw ('Missing dictionary ' + $pair[0]) }
    $lookup = @{}
    $output.Add('pub const ' + $pair[1] + ': &[(&str, usize)] = &[')
    foreach ($match in [regex]::Matches($body, '\{\s*"([^"]+)",\s*(0x[0-9A-Fa-f]+|\d+)\s*\}')) {
        $lookup[$match.Groups[1].Value] = $match.Groups[2].Value
        $output.Add('    ("' + $match.Groups[1].Value + '", ' + $match.Groups[2].Value + '),')
    }
    $output.Add('];'); $maps[$pair[0]] = $lookup
}
$untrackable = [regex]::Match($constants, 'UntrackableTrainers.*?\{([^}]+)\}', 'Singleline').Groups[1].Value
$output.Add('pub const UNTRACKABLE_TRAINERS: &[&str] = &[' + $untrackable + '];')
$output | Set-Content (Join-Path $coreDirectory 'Cnst2.rs')
$destination = Join-Path $coreDirectory 'CompletionValidator2.rs'
$existing = Get-Content $destination -Raw
$marker = $existing.IndexOf('// Generated literal tables and simple rules follow.')
if ($marker -lt 0) { throw 'Missing generated boundary' }
$output = [Collections.Generic.List[string]]::new()
$output.Add($existing.Substring(0, $marker).TrimEnd())
$output.Add('// Generated literal tables and simple rules follow. See tools/port-crystal-rules.ps1.')
foreach ($pair in @(@('inGameGift','in_game_gift'), @('inGameTrade','in_game_trade'), @('decoration','decoration'), @('phone','phone'), @('pokegear','pokegear'))) {
    $body = [regex]::Match($source, ('void Generate_' + $pair[0] + '\(\)\s*\{(.*?)\n    \}'), 'Singleline').Groups[1].Value
    if (!$body) { throw ('Missing method ' + $pair[0]) }
    $category = [regex]::Match($body, 'owned\["([^"]+)"\]').Groups[1].Value
    $output.Add('fn simple_' + $pair[1] + '(save: &CrystalSave, result: &mut CommandResult) {')
    if ($pair[0] -eq 'pokegear') { $output.Add('    let _ = save;') }
    $output.Add('    let mut entries = Checklist::default();')
    foreach ($rule in [regex]::Matches($body, 'ow\["([^"]+)"\]\s*=\s*([^;]+);')) {
        $expression = $rule.Groups[2].Value.Trim().Replace('sav.GetEventFlag(', 'save.event(')
        $expression = [regex]::Replace($expression, 'sav.GetFlag\(([^,]+),\s*([^\)]+)\)', 'save.data[$1] & (1 << $2) != 0')
        $output.Add('    entries.set("' + $rule.Groups[1].Value + '", ' + $expression + ');')
    }
    $output.Add('    entries.emit(result,"' + $category + '");'); $output.Add('}')
}
foreach ($pair in @(@('itemInMap','MAP_ITEMS'), @('itemGift','GIFT_ITEMS'))) {
    $body = [regex]::Match($source, ('void Generate_' + $pair[0] + '\(\)\s*\{(.*?)\n    \}'), 'Singleline').Groups[1].Value
    $list = [regex]::Match($body, 'var list = new List<int>\s*\{([^}]+)\}').Groups[1].Value
    if (!$list) { throw 'Missing item list' }
    $output.Add('const ' + $pair[1] + ': &[usize] = &[' + $list + '];')
}
$output.Add('type Rematch = (&''static [&''static str], usize, usize, Option<usize>);')
$output.Add('const REMATCHES: &[Rematch] = &[')
foreach ($match in [regex]::Matches($source, 'func(2?)\(new List<string>\s*\{([^}]+)\},\s*"([^"]+)",\s*"([^"]+)"(?:,\s*"([^"]+)")?\)')) {
    $flag = $maps['EventNameToAddress'][$match.Groups[3].Value]
    $work = $maps['EventWorkNameToAddress'][$match.Groups[4].Value]
    if (!$flag -or !$work) { throw 'Missing rematch address' }
    $last = 'None'
    if ($match.Groups[1].Value -eq '2') { $last = 'Some(' + $maps['EventNameToAddress'][$match.Groups[5].Value] + ')' }
    $output.Add('    (&[' + $match.Groups[2].Value + '], ' + $flag + ', ' + $work + ', ' + $last + '),')
}
$output.Add('];')
$output | Set-Content $destination
$itemSource = Get-Content (Join-Path $repoDirectory 'PKHeX.Core/Items/ItemStorage2.cs') -Raw
$itemSource = [regex]::Replace($itemSource, '//[^\r\n]*', '')
$output = [Collections.Generic.List[string]]::new()
$output.Add('// Item identifiers only, ported from ItemStorage2.cs; no move data or legality analysis.')
foreach ($pair in @(@('General','GENERAL'), @('Balls','BALLS'), @('KeyCrystal','KEY_CRYSTAL'), @('Machine','MACHINE'))) {
    $body = [regex]::Match($itemSource, ($pair[0] + '\s*=>\s*\[(.*?)\]'), 'Singleline').Groups[1].Value
    $numbers = [regex]::Matches($body, '\d+') | ForEach-Object { ([int]$_.Value).ToString() }
    $output.Add('pub const ' + $pair[1] + ': &[u16] = &[' + ($numbers -join ',') + '];')
}
$target = Join-Path $repoDirectory 'rust/PKHeX.Core/Items/ItemStorage2.rs'
[IO.Directory]::CreateDirectory((Split-Path $target -Parent)) | Out-Null
$output | Set-Content $target
