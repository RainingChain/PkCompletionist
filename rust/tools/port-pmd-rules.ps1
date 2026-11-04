# Reproduce the literal rule tables from the preserved C# reference.
# This handles only the two named validators; unsupported expressions fail review/build.
$ErrorActionPreference = 'Stop'
$repoDirectory = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$coreDirectory = Join-Path $repoDirectory 'rust/PkCompletionist.Core'
function Get-EnumIndices([string]$source, [string]$name) {
    $body = [regex]::Match($source, ('enum ' + $name + '\s*\{(.*?)\}'), 'Singleline').Groups[1].Value
    $body = [regex]::Replace($body, '//[^\r\n]*', '')
    $index = 0; $values = @{}
    foreach ($entry in $body.Split(',')) {
        $entry = $entry.Trim()
        if (!$entry -or $entry.StartsWith('MAX')) { continue }
        $parts = $entry.Split('='); if ($parts.Count -gt 1) { $index = [int]$parts[1].Trim() }
        $values[$parts[0].Trim()] = $index; $index++
    }
    return $values
}
$rescue = Get-Content (Join-Path $repoDirectory 'PkCompletionist.Core/Gen3c/CompletionValidator3_PmdRescueTeam.cs') -Raw
$friends = Get-EnumIndices $rescue 'FriendArea'
$dojos = Get-EnumIndices $rescue 'MakuhitaDojo'
$sky = Get-Content (Join-Path $repoDirectory 'PkCompletionist.Core/Gen4b/CompletionValidator4_PmdSky.cs') -Raw
function Convert-Rule([string]$expression) {
    $expression = [regex]::Replace($expression, '\s+', ' ').Trim()
    $expression = $expression.Replace('this.ownedMonList.Contains(', 'has(').Replace('ownedMonList.Contains(', 'has(')
    $expression = $expression.Replace('this.ownedItemList.Contains(', 'item(').Replace('ownedItemList.Contains(', 'item(')
    $expression = $expression.Replace('Has2(', 'item(').Replace('Has(', 'has(')
    $expression = $expression.Replace('HasCompletedSkyTower()', 'story(3)').Replace('HasCompletedStory(', 'story(')
    $expression = [regex]::Replace($expression, 'HasFriendArea\(FriendArea\.(\w+)\)', { param($m) 'friend(' + $friends[$m.Groups[1].Value] + ')' })
    $expression = [regex]::Replace($expression, 'HasCompletedMakuhitaDojo\(MakuhitaDojo\.(\w+)\)', { param($m) 'dojo(' + $dojos[$m.Groups[1].Value] + ')' })
    $expression = $expression.Replace('this.sav.Data[', 'data[').Replace('sav.Data[', 'data[')
    $expression = $expression.Replace('UnownRelic()', '(201..=226).any(has)').Replace('HasUnown()', '(201..=228).any(has)')
    $expression = $expression.Replace('ExplorerRankPoints', 'points').Replace('pts', 'points')
    $expression = $expression.Replace('DidStory', 'has(532)').Replace('didSomething', 'true')
    $expression = $expression.Replace('GoldenChest()', '(59..=65).all(item)').Replace('HasTreasure()', '(59..=65).any(item)')
    return $expression
}
function Write-Rules([string]$source, [string[]]$methods, [string]$header, [string]$destination) {
    $clean = [regex]::Replace($source, '//[^\r\n]*', '')
    $lines = [System.Collections.Generic.List[string]]::new()
    $existing = Get-Content $destination -Raw
    $marker = $existing.IndexOf('// Ported rule expressions;')
    if ($marker -lt 0) { throw "Missing generated-rule boundary: $destination" }
    $lines.Add($existing.Substring(0, $marker).TrimEnd())
    $lines.Add('// Ported rule expressions; regenerate with rust/tools/port-pmd-rules.ps1.')
    $lines.Add('#[allow(clippy::vec_init_then_push, clippy::too_many_arguments)]')
    $lines.Add($header)
    foreach ($method in $methods) {
        $body = [regex]::Match($clean, ('public void Generate_' + $method + '\(\)\s*\{(.*?)\n    \}'), 'Singleline').Groups[1].Value
        if (!$body) { throw "Method not found: $method" }
        $category = [regex]::Match($body, 'owned\["([^"]+)"\]').Groups[1].Value
        $hasRules = [regex]::IsMatch($body, 'ow\["')
        $lines.Add($(if ($hasRules) { '    let mut entries = Vec::new();' } else { '    let entries = Vec::new();' }))
        foreach ($rule in [regex]::Matches($body, 'ow\["([^"]+)"\]\s*=\s*(.*?);', 'Singleline')) {
            $key = $rule.Groups[1].Value; $expression = Convert-Rule $rule.Groups[2].Value
            $line = 'entries.push(("' + $key + '", ' + $expression + '));'
            if ($key -eq 'OblivionForest') { $line = 'if points < 17000 { ' + $line + ' }' }
            $lines.Add('    ' + $line)
        }
        $lines.Add('    ordered_category(result, "' + $category + '", &entries);')
    }
    $lines.Add('}')
    $lines | Set-Content $destination
}
Write-Rules $rescue @('friendArea','makuhitaDojo','rank','dungeon','adventureLog','progressIcon','teamBaseFigure') 'fn populate(data: &[u8], result: &mut CommandResult, has: impl Fn(u32) -> bool + Copy, item: impl Fn(u32) -> bool + Copy, story: impl Fn(usize) -> bool, friend: impl Fn(usize) -> bool, dojo: impl Fn(usize) -> bool, points: i32) {' (Join-Path $coreDirectory 'Gen3c/CompletionValidator3_PmdRescueTeam.rs')
Write-Rules $sky @('rank','dungeon','adventureLog','specialEpisode','progressIcon') 'fn populate(result: &mut CommandResult, has: impl Fn(u32) -> bool + Copy, item: impl Fn(u32) -> bool + Copy, points: i32) {' (Join-Path $coreDirectory 'Gen4b/CompletionValidator4_PmdSky.rs')
$unobtainable = [regex]::Match($sky, 'this.unobtainableItems = new List<int>\s*\{(.*?)\};', 'Singleline').Groups[1].Value
Add-Content (Join-Path $coreDirectory 'Gen4b/CompletionValidator4_PmdSky.rs') ('const UNOBTAINABLE: &[u32] = &[' + $unobtainable + '];')
