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
    $expression = $expression.Replace('HasCompletedSkyTower()', 'self.HasCompletedSkyTower()').Replace('HasCompletedStory(', 'story(')
    $expression = [regex]::Replace($expression, 'HasFriendArea\(FriendArea\.(\w+)\)', { param($m) 'friend(' + $friends[$m.Groups[1].Value] + ')' })
    $expression = [regex]::Replace($expression, 'HasCompletedMakuhitaDojo\(MakuhitaDojo\.(\w+)\)', { param($m) 'dojo(' + $dojos[$m.Groups[1].Value] + ')' })
    $expression = $expression.Replace('this.sav.Data[', 'data[').Replace('sav.Data[', 'data[')
    $expression = $expression.Replace('UnownRelic()', '(201..=226).any(has)').Replace('HasUnown()', '(201..=228).any(has)')
    $expression = $expression.Replace('ExplorerRankPoints', 'points').Replace('pts', 'points')
    $expression = $expression.Replace('DidStory', 'self.HasFinishedPostCreditStory()').Replace('didSomething', 'true')
    $expression = $expression.Replace('GoldenChest()', '(59..=65).all(item)').Replace('HasTreasure()', '(59..=65).any(item)')
    $expression = $expression.Replace('has(534) || has(535)', 'self.GotShaymin()')
    return $expression
}
function Write-Rules([string]$source, [string[]]$methods, [string]$typeName, [string]$destination) {
    $clean = [regex]::Replace($source, '//[^\r\n]*', '')
    $lines = [Collections.Generic.List[string]]::new()
    $existing = Get-Content $destination -Raw
    $marker = $existing.IndexOf('// Ported rule expressions;')
    if ($marker -lt 0) {throw "Missing generated-rule boundary: $destination"}
    $lines.Add($existing.Substring(0,$marker).TrimEnd())
    $lines.Add('// Ported rule expressions; regenerate with rust/tools/port-pmd-rules.ps1.')
    $lines.Add('impl ' + $typeName + ' {')
    foreach ($method in $methods) {
        $body = [regex]::Match($clean,('public void Generate_' + $method + '\(\)\s*\{(.*?)\n    \}'),'Singleline').Groups[1].Value
        if (!$body) {throw "Method not found: $method"}
        $category = [regex]::Match($body,'owned\["([^"]+)"\]').Groups[1].Value
        $lines.Add('    #[allow(clippy::vec_init_then_push)]')
        $lines.Add('    fn Generate_' + $method + '(&self,result: &mut CommandResult) {')
        $rules = @([regex]::Matches($body,'ow\["([^"]+)"\]\s*=\s*(.*?);','Singleline') | ForEach-Object { [pscustomobject]@{Key=$_.Groups[1].Value;Expression=(Convert-Rule $_.Groups[2].Value)} })
        $expressions = ($rules | ForEach-Object {$_.Expression}) -join ' '
        foreach ($binding in @(@('has','|id| self.HasMon(id)'),@('item','|id| self.HasItem(id)'),@('story','|id| self.HasCompletedStory(id)'),@('friend','|id| self.HasFriendArea(id)'),@('dojo','|id| self.HasCompletedMakuhitaDojo(id)'))) {
            if ($expressions -match ('\b' + $binding[0] + '\b')) {$lines.Add('        let ' + $binding[0] + ' = ' + $binding[1] + ';')}
        }
        if ($expressions -match '\bdata\b') {$lines.Add('        let data = self.sav;')}
        if ($expressions -match '\bpoints\b') {$lines.Add('        let points = self.points;')}
        $lines.Add($(if ($rules.Count -gt 0) {'        let mut entries = Vec::new();'} else {'        let entries = Vec::new();'}))
        foreach ($rule in $rules) {
            $line = 'entries.push(("' + $rule.Key + '", ' + $rule.Expression + '));'
            if ($rule.Key -eq 'OblivionForest') {$line = 'if self.points < 17000 { ' + $line + ' }'}
            $lines.Add('        ' + $line)
        }
        $lines.Add('        ordered_category(result,"' + $category + '",&entries);')
        $lines.Add('    }')
    }
    $lines.Add('}')
    $lines | Set-Content $destination
}
Write-Rules $rescue @('friendArea','makuhitaDojo','rank','dungeon','adventureLog','progressIcon','teamBaseFigure') 'CompletionValidator3_PmdRescueTeam<''_>' (Join-Path $coreDirectory 'Gen3c/CompletionValidator3_PmdRescueTeam.rs')
Write-Rules $sky @('rank','dungeon','adventureLog','specialEpisode','progressIcon') 'CompletionValidator4_PmdSky' (Join-Path $coreDirectory 'Gen4b/CompletionValidator4_PmdSky.rs')
$unobtainable = [regex]::Match($sky, 'this.unobtainableItems = new List<int>\s*\{(.*?)\};', 'Singleline').Groups[1].Value
Add-Content (Join-Path $coreDirectory 'Gen4b/CompletionValidator4_PmdSky.rs') ('const UNOBTAINABLE: &[u32] = &[' + $unobtainable + '];')
