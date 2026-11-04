# Literal Emerald rules and item tables, ported from the preserved C# source.
$ErrorActionPreference = 'Stop'
$repoDirectory = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$targetDirectory = Join-Path $repoDirectory 'rust/PkCompletionist.Core/Gen3'
$source = Get-Content (Join-Path $repoDirectory 'PkCompletionist.Core/Gen3/CompletionValidator3.cs') -Raw
$source = [regex]::Replace($source,'/\*.*?\*/','','Singleline')
$source = [regex]::Replace($source,'//[^\r\n]*','')
function Read-Enum([string]$source,[string]$name) {
    $body = [regex]::Match($source,('enum ' + $name + '(?:\s*:\s*\w+)?\s*\{(.*?)\}'),'Singleline').Groups[1].Value
    $index = 0; $values = @{}
    foreach ($entry in $body.Split(',')) {
        $entry = $entry.Trim(); if (!$entry) {continue}
        $parts = $entry.Split('='); if ($parts.Length -gt 1) {$index = [int]$parts[1].Trim()}
        $values[$parts[0].Trim()] = $index; $index++
    }
    return $values
}
$decorations = Read-Enum ([regex]::Replace((Get-Content (Join-Path $repoDirectory 'PKHeX.Core/Saves/Substructures/Gen3/Decoration3.cs') -Raw),'//[^\r\n]*','')) 'Decoration3'
$colors = Read-Enum (Get-Content (Join-Path $repoDirectory 'PKHeX.Core/Saves/Substructures/Gen3/PokeBlock3Color.cs') -Raw) 'PokeBlock3Color'
$trendy = Read-Enum $source 'TrendyWord'
$pkSource = Get-Content (Join-Path $repoDirectory 'PKHeX.Core/PKM/PK3.cs') -Raw
$ribbonFlags = @{}; $contestShifts = @{}
foreach ($match in [regex]::Matches($pkSource,'public override bool (\w+)\s*\{ get => \(RIB0 & \(1 << (\d+)\)')) {$ribbonFlags[$match.Groups[1].Value] = [int]$match.Groups[2].Value}
foreach ($match in [regex]::Matches($pkSource,'public override byte (RibbonCount\w+)\s*\{ get => \(byte\)\(\(RIB0 >> (\d+)\)')) {$contestShifts[$match.Groups[1].Value] = [int]$match.Groups[2].Value}
function Convert-Expression([string]$expression) {
    $expression = [regex]::Replace($expression,'\s+',' ').Trim().Replace('this.sav','sav')
    $expression = $expression.Replace('sav.GetEventFlag(','ctx.save.event(').Replace('sav.GetWork(','ctx.save.work(').Replace('sav.GetSeen(','ctx.save.seen(')
    $expression = $expression.Replace('HasPkmWithTID(','ctx.own_trainer(').Replace('HasPkmForm(','ctx.form(').Replace('HasOrSeenPkmBasedOnObjective(','ctx.objective(').Replace('HasPkm(','ctx.has(').Replace('HasItem(','ctx.item(')
    $expression = $expression.Replace('sav.GetAllPKM().Count > 0','!ctx.pokemon.is_empty()').Replace('sav.Gender','ctx.save.small[8]')
    $expression = $expression.Replace('HasShinyMon()','ctx.pokemon.iter().any(|pk| pk.is_shiny())').Replace('GetInfectedbyPokerus()','ctx.pokemon.iter().any(|pk| pk.data[0x44] != 0)')
    $expression = [regex]::Replace($expression,'HasDeco\(Decoration3\.(\w+)\)',{param($m) 'ctx.decoration(' + $decorations[$m.Groups[1].Value] + ')'})
    $expression = [regex]::Replace($expression,'HasPokeblock\(PokeBlock3Color\.(\w+)\)',{param($m) 'ctx.block(' + $colors[$m.Groups[1].Value] + ')'})
    $expression = [regex]::Replace($expression,'GetTrendyWordUnlocked\(TrendyWord\.(\w+)\)',{param($m) 'ctx.trendy(' + $trendy[$m.Groups[1].Value] + ')'})
    $expression = [regex]::Replace($expression,'HasRibbon\(p => p\.(\w+)\)',{param($m) 'ctx.pokemon.iter().any(|pk| pk.ribbons() & (1 << ' + $ribbonFlags[$m.Groups[1].Value] + ') != 0)'})
    $expression = [regex]::Replace($expression,'HasContestRibbon\(p => p\.(\w+),\s*(\d+)\)',{param($m) 'ctx.pokemon.iter().any(|pk| ((pk.ribbons() >> ' + $contestShifts[$m.Groups[1].Value] + ') & 7) >= ' + $m.Groups[2].Value + ')'})
    $expression = $expression.Replace('HoennPokedex()','HOENN.iter().all(|id| [385,386].contains(id) || ctx.save.caught(*id))').Replace('PokemonRegionalNoTrade()','ctx.regional_no_trade()').Replace('HoennListSeeTrade.All(id => ctx.save.seen(id))','HOENN_TRADE.iter().all(|id| ctx.save.seen(*id))')
    $expression = $expression.Replace('sav.LargeBlock.WaldaUnlocked','ctx.save.large[0x3d86] != 0').Replace('TrainerHill(','ctx.trainer_hill(').Replace('BerryBlenderRecord(','ctx.blender(')
    foreach ($pair in @(@('JoyfulJumpScore','0x208'),@('JoyfulJump5InRow','0x200'),@('JoyfulBerriesScore','0x20c'),@('JoyfulBerries5InRow','0x212'))) {$expression = $expression.Replace(('sav.SmallBlock.' + $pair[0]),('u16_le(&ctx.save.small,' + $pair[1] + ').unwrap()'))}
    if ($expression -match '\bsav\.|\bHas[A-Z]|\bGet[A-Z]|HoennList|PokemonRegionalNoTrade') {throw ('Unconverted expression: ' + $expression)}
    $expression = $expression.Replace('pk.ribbons() >> 0', 'pk.ribbons()')
    $expression = [regex]::Replace($expression, '\s+\+\s+0\b', '')
    return $expression
}
$destination = Join-Path $targetDirectory 'CompletionValidator3.rs'
$existing = Get-Content $destination -Raw
$marker = $existing.IndexOf('// Generated literal tables and simple rules follow.')
$output = [Collections.Generic.List[string]]::new()
$output.Add($existing.Substring(0,$marker).TrimEnd())
$output.Add('// Generated literal tables and simple rules follow. See tools/port-emerald-rules.ps1.')
$output.Add('impl CompletionValidator3 {')
foreach ($method in @('pokemonForm','decoration','inGameTrade','inGameGift','battleFrontier','ribbon','phone','trainerStar','pokeblock','easyChatSystemWord','misc')) {
    $body = [regex]::Match($source,('void Generate_' + $method + '\(\)\s*\{(.*?)\n    \}'),'Singleline').Groups[1].Value
    if (!$body) {throw ('Missing method ' + $method)}
    $category = [regex]::Match($body,'owned\["([^"]+)"\]').Groups[1].Value
    $methodName = [regex]::Replace($method,'([A-Z])','_$1').ToLower()
    $output.Add('fn Generate_' + $method + '(&self,result: &mut CommandResult) {')
    $output.Add('    let ctx = self;')
    $output.Add('    let mut entries = Checklist::default();')
    foreach ($rule in [regex]::Matches($body,'ow\["([^"]+)"\]\s*=\s*([^;]+);')) {
        $output.Add('    entries.set("' + $rule.Groups[1].Value + '",' + (Convert-Expression $rule.Groups[2].Value) + ');')
    }
    $output.Add('    entries.emit(result,"' + $category + '");'); $output.Add('}')
}
$output.Add('}')
foreach ($pair in @(@('HoennListSeeNoTrade','HOENN_NO_TRADE'),@('HoennListSeeTrade','HOENN_TRADE'),@('HoennList','HOENN'))) {
    $body = [regex]::Match($source,($pair[0] + ' = new List<ushort>\s*\{([^}]+)\}'),'Singleline').Groups[1].Value
    $output.Add('const ' + $pair[1] + ': &[u16] = &[' + $body + '];')
}
$body = [regex]::Match($source,'unobtainableItems = new List<int>\(\)\s*\{([^}]+)\}','Singleline').Groups[1].Value
$output.Add('const UNOBTAINABLE: &[u16] = &[' + $body + '];')
foreach ($pair in @(@('itemInMap','list','MAP_FLAGS','usize'),@('itemGift','list','GIFT_FLAGS','usize'),@('itemInMap','berryIdxList','BERRY_INDICES','usize'),@('itemInMap','defaultBerryIdList','DEFAULT_BERRIES','u8'),@('battle','monBattle','MON_BATTLES','usize'))) {
    $body = [regex]::Match($source,('void Generate_' + $pair[0] + '\(\)\s*\{(.*?)\n    \}'),'Singleline').Groups[1].Value
    $list = [regex]::Match($body,($pair[1] + ' = new List<int>\s*\{([^}]+)\}'),'Singleline').Groups[1].Value
    if (!$list) {throw ('Missing list ' + $pair[1])}
    $output.Add('const ' + $pair[2] + ': &[' + $pair[3] + '] = &[' + $list + '];')
}
$itemBody = [regex]::Match($source,'override void Generate_item\(\)\s*\{(.*?)\n    \}','Singleline').Groups[1].Value
$output.Add('const ITEM_INFERENCES: &[(u16,&[usize])] = &[')
foreach ($match in [regex]::Matches($itemBody,'if \(([^\r\n]+)\)\s*\n\s*ow\["(\d+)"\] = true;')) {
    $condition = $match.Groups[1].Value
    if ($condition -notmatch 'GetEventFlag' -or $condition -match 'HasPkm') {continue}
    $flags = [regex]::Matches($condition,'sav.GetEventFlag\(([^)]+)\)') | ForEach-Object {$_.Groups[1].Value}
    $output.Add('    (' + $match.Groups[2].Value + ',&[' + ($flags -join ',') + ']),')
}
$output.Add('];')
$output | Set-Content $destination
$itemSource = Get-Content (Join-Path $repoDirectory 'PKHeX.Core/Items/ItemStorage3RS.cs') -Raw
$itemSource = [regex]::Replace($itemSource,'//[^\r\n]*','')
$keySource = Get-Content (Join-Path $repoDirectory 'PKHeX.Core/Items/ItemStorage3E.cs') -Raw
$keySource = [regex]::Replace($keySource,'//[^\r\n]*','')
$output = [Collections.Generic.List[string]]::new()
$output.Add('// Emerald item identifiers, ported from ItemStorage3E/3RS. No legality or move data.')
foreach ($pair in @(@('General','GENERAL'),@('Key','KEY'),@('Balls','BALLS'),@('Machine','MACHINE'),@('Berry','BERRY'))) {
    $text = if ($pair[0] -eq 'Key') {$keySource} else {$itemSource}
    $body = [regex]::Match($text,($pair[0] + '\s*=>\s*\[(.*?)\]'),'Singleline').Groups[1].Value
    $output.Add('pub const ' + $pair[1] + ': &[u16] = &[' + $body + '];')
}
$output | Set-Content (Join-Path $repoDirectory 'rust/PKHeX.Core/Items/ItemStorage3E.rs')
