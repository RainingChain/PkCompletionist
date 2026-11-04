use crate::{category, CommandResult, completion_validator_x::Checklist, cnst2};
use pkhex_core::{binary::InvalidData, gen2::CrystalSave};

pub fn validate(data: &[u8], objective: i32, result: &mut CommandResult) -> Result<(), InvalidData> {
    let save = CrystalSave::read(data)?;
    let data = &save.data;
    let pokemon = save.pokemon();
    let trainer_id = u16::from_be_bytes([data[0x2009],data[0x200a]]);
    let own_trainer = |species| pokemon.iter().any(|pk| pk.species == species && pk.trainer_id == trainer_id);
    category(result, "pokemon", (1_u16..=251).map(|id| (id.to_string(), if objective == 0 { data[0x2a27 + (id as usize - 1) / 8] & (1 << ((id - 1) % 8)) != 0 } else { pokemon.iter().any(|pk| pk.species == id) })));
    let mut owned_items = pkhex_core::player_bag2::items(data);
    owned_items.extend(pokemon.iter().map(|pk| pk.held_item));
    for index in 0..16 {
        let offset = if index < 6 { 0x600 + index * 0x2f } else { 0x600 + 12 * 0x2f + 1 + (index - 6) * 0x2f };
        let mail_type = data[offset + 0x2e];
        if mail_type == 0x9e || (0xb5..=0xbd).contains(&mail_type) { owned_items.push(u16::from(mail_type)); }
    }
    const UNOBTAINABLE: &[u16] = &[6,25,45,50,56,90,100,120,135,136,137,141,142,145,147,148,149,153,154,155,162,171,176,179,190,195,220,250,251,252,253,254,255];
    let mut items = Checklist::default();
    for id in (1..=249).filter(|id| !UNOBTAINABLE.contains(id)) { items.set(id, owned_items.contains(&id)); }
    if data[0x23e5..0x23e7] == [0xff,0xff] { for id in [66,67,128] { items.set(id, true); } }
    if items.get("134") { items.set(130,true); }
    if save.event(0x1e) { items.set(69,true); }
    if own_trainer(123) { items.set(177,true); }
    if objective != 1 {
        for (flag,id) in [(718,167),(717,168),(0xde,172),(0x73,82),(0x71,143),(0x683,151),(0x324,23),(0x321,34)] { if save.event(flag) { items.set(id,true); } }
        if own_trainer(251) { items.set(115,true); }
    }
    items.emit(result,"item");
    simple_in_game_gift(&save, result);
    let unown = |form| pokemon.iter().any(|pk| pk.species == 201 && pk.form() == form);
    let mut misc = Checklist::default();
    if (0x2c..=0x2f).any(|id| save.work(id) == 0) { misc.set("BeatBattleTower",false); }
    misc.set("UnlockMysteryGiftoption",data[0xbe3] == 0);
    misc.set("NewMagikarpSizeRecord",data[0x2b76] >= 4);
    misc.set("PokedexUnownMode",(0..26).filter(|form| unown(*form)).count() >= 3);
    if !save.event(0x762) && save.event(0x44) { misc.set("DefeatRed",true); }
    misc.set("GetInfectedbyPokerus",pokemon.iter().any(|pk| pk.pokerus != 0));
    misc.emit(result,"misc");
    category(result,"itemInMap", MAP_ITEMS.iter().filter(|id| **id != 236 || save.event(236)).map(|id| (id.to_string(),save.event(*id))));
    let mut gifts = Checklist::default();
    for id in GIFT_ITEMS { let value = save.event(*id); if value || ![101,103,105,107,109,111,99].contains(id) { gifts.set(id,value); } }
    gifts.set("custom0",save.event(0x1a) && save.work(0x15) != 5);
    gifts.set("custom2",save.event(0x1f) && save.work(0x15) != 6);
    gifts.emit(result,"itemGift");
    simple_in_game_trade(&save,result);
    simple_decoration(&save,result);
    simple_phone(&save,result);
    battle(&save,result);
    simple_pokegear(&save,result);
    let mut forms = Checklist::default();
    for (form, letter) in ('A'..='Z').enumerate() { forms.set(format!("Unown{letter}"),unown(form as u8)); }
    forms.set("GetShinyPokemon",pokemon.iter().any(|pk| pk.is_shiny()));
    forms.emit(result,"pokemonForm");
    Ok(())
}

fn battle(save: &CrystalSave, result: &mut CommandResult) {
    let mut battles = Checklist::default();
    for id in [1760,1761,1762,42,791,792,1872,1873] { battles.set(id,save.event(id)); }
    for (names, flag, work, final_flag) in REMATCHES {
        battles.set(names[0],save.event(*flag));
        for (index,name) in names.iter().enumerate().take(names.len() - 1).skip(1) { battles.set(name,save.work(*work) as usize >= index + 1); }
        if let Some(flag) = final_flag { battles.set(names[names.len() - 1],save.event(*flag)); }
        else if (save.work(*work) as usize) < names.len() - 1 { battles.set(names[names.len() - 1],false); }
    }
    for (name,flag) in cnst2::TRAINER_TO_FLAG { let value = save.event(*flag); if value || !cnst2::UNTRACKABLE_TRAINERS.contains(name) { battles.set(name,value); } }
    if save.event(0x44) {
        battles.set("GRUNTM_31",true);
        for (flag,starter) in [(0x1b,"TOTODILE"),(0x1c,"CHIKORITA"),(0x1d,"CYNDAQUIL")] {
            if save.event(flag) {
                for index in 1..=5 { battles.set(format!("RIVAL1_{index}_{starter}"),true); }
                for index in 1..=2 { battles.set(format!("RIVAL2_{index}_{starter}"),true); }
            }
        }
    }
    battles.emit(result,"battle");
}
// Generated literal tables and simple rules follow. See tools/port-crystal-rules.ps1.
fn simple_in_game_gift(save: &CrystalSave, result: &mut CommandResult) {
    let mut entries = Checklist::default();
    entries.set("Starter", true);
    entries.set("Togepi", save.event(0x0054));
    entries.set("OddEgg", save.event(0x033E));
    entries.set("Spearow", save.event(0x0050));
    entries.set("Eevee", save.event(0x004F));
    entries.set("Dratini", save.event(0x00BD));
    entries.set("Shuckle", save.event(0x0045));
    entries.set("Tyrogue", save.event(0x0061));
    entries.emit(result,"inGameGift");
}
fn simple_in_game_trade(save: &CrystalSave, result: &mut CommandResult) {
    let mut entries = Checklist::default();
    entries.set("AbraforMachop", save.data[0x24EE] & (1 << 0) != 0);
    entries.set("BellsproutforOnix", save.data[0x24EE] & (1 << 1) != 0);
    entries.set("KrabbyforVoltorb", save.data[0x24EE] & (1 << 2) != 0);
    entries.set("DragonairforDodrio", save.data[0x24EE] & (1 << 3) != 0);
    entries.set("HaunterforXatu", save.data[0x24EE] & (1 << 4) != 0);
    entries.set("ChanseyforAerodactyl", save.data[0x24EE] & (1 << 5) != 0);
    entries.set("DugtrioforMagneton", save.data[0x24EE] & (1 << 6) != 0);
    entries.emit(result,"inGameTrade");
}
fn simple_decoration(save: &CrystalSave, result: &mut CommandResult) {
    let mut entries = Checklist::default();
    entries.set("TownMap", true);
    entries.set("FeatheryBed", true);
    entries.set("PinkBed", save.event(677));
    entries.set("PolkadotBed", save.event(678));
    entries.set("RedCarpet", save.event(680));
    entries.set("BlueCarpet", save.event(681));
    entries.set("YellowCarpet", save.event(682));
    entries.set("GreenCarpet", save.event(683));
    entries.set("Magnaplant", save.event(684));
    entries.set("Tropicplant", save.event(685));
    entries.set("Jumboplant", save.event(686));
    entries.set("PikachuPoster", save.event(688));
    entries.set("ClefairyPoster", save.event(689));
    entries.set("JigglypuffPoster", save.event(690));
    entries.set("NES", save.event(691));
    entries.set("SuperNES", save.event(692));
    entries.set("Nintendo64", save.event(693));
    entries.set("VirtualBoy", save.event(694));
    entries.set("BulbasaurDoll", save.event(699));
    entries.set("CharmanderDoll", save.event(700));
    entries.set("ClefairyDoll", save.event(697));
    entries.set("DiglettDoll", save.event(703));
    entries.set("GengarDoll", save.event(707));
    entries.set("GeodudeDoll", save.event(713));
    entries.set("GrimerDoll", save.event(709));
    entries.set("JigglypuffDoll", save.event(698));
    entries.set("MachopDoll", save.event(714));
    entries.set("MagikarpDoll", save.event(705));
    entries.set("OddishDoll", save.event(706));
    entries.set("PikachuDoll", save.event(695));
    entries.set("PoliwagDoll", save.event(702));
    entries.set("ShellderDoll", save.event(708));
    entries.set("SquirtleDoll", save.event(701));
    entries.set("StaryuDoll", save.event(704));
    entries.set("SurfPikachuDoll", save.event(696));
    entries.set("VoltorbDoll", save.event(710));
    entries.set("WeedleDoll", save.event(711));
    entries.set("BigLaprasDoll", save.event(721));
    entries.set("BigOnixDoll", save.event(720));
    entries.set("BigSnorlaxDoll", save.event(719));
    entries.set("SilverTrophy", save.event(718));
    entries.set("GoldTrophy", save.event(717));
    entries.emit(result,"decoration");
}
fn simple_phone(save: &CrystalSave, result: &mut CommandResult) {
    let mut entries = Checklist::default();
    entries.set("Mom", true);
    entries.set("ProfessorElm", true);
    entries.set("Bill", save.event(0x004F));
    entries.set("Buena", save.event(0x033C) || save.event(0x029E));
    entries.set("PokefanBeverly", save.event(0x0261));
    entries.set("PokefanDerek", save.event(0x028D));
    entries.set("SchoolKidJack", save.event(0x025F));
    entries.set("SailorHuey", save.event(0x0263));
    entries.set("AceTrainerGaven", save.event(0x026B));
    entries.set("CooltrainerBeth", save.event(0x026D));
    entries.set("BirdKeeperJose", save.event(0x026F));
    entries.set("AceTrainerReena", save.event(0x0271));
    entries.set("YoungsterJoey", save.event(0x0273));
    entries.set("BugCatcherWade", save.event(0x0275));
    entries.set("FishermanRalph", save.event(0x0277));
    entries.set("PicnickerLiz", save.event(0x0279));
    entries.set("HikerAnthony", save.event(0x027B));
    entries.set("CamperTodd", save.event(0x027D));
    entries.set("PicnickerGina", save.event(0x027F));
    entries.set("JugglerIrwin", save.event(0x0281));
    entries.set("BugCatcherArnie", save.event(0x0283));
    entries.set("SchoolKidAlan", save.event(0x0285));
    entries.set("LassDana", save.event(0x0289));
    entries.set("SchoolKidChad", save.event(0x028B));
    entries.set("FishermanTully", save.event(0x028F));
    entries.set("PokeManiacBrent", save.event(0x0291));
    entries.set("PicnickerTiffany", save.event(0x0293));
    entries.set("BirdKeeperVance", save.event(0x0295));
    entries.set("FishermanWilton", save.event(0x0297));
    entries.set("BlackBeltKenji", save.event(0x0299));
    entries.set("HikerParry", save.event(0x029B));
    entries.set("PicnickerErin", save.event(0x029D));
    entries.emit(result,"phone");
}
fn simple_pokegear(save: &CrystalSave, result: &mut CommandResult) {
    let _ = save;
    let mut entries = Checklist::default();
    entries.set("TownMap", true);
    entries.set("Radio", true);
    entries.emit(result,"pokegear");
}
const MAP_ITEMS: &[usize] = &[ 1654, 1976, 1978, 141, 1629, 1944, 250, 1655, 1983, 1928, 244, 1683, 1986, 1671, 149, 1925, 1682, 127, 1943, 1650, 1998, 1923, 154, 1704, 1667, 1632, 1668, 1605, 174, 1722, 157, 243, 1918, 1926, 1947, 1955, 1610, 1661, 1669, 1685, 1614, 1691, 1979, 136, 1653, 169, 128, 1636, 1659, 242, 1917, 177, 138, 132, 135, 1647, 1675, 1652, 1695, 159, 1702, 252, 238, 1611, 146, 1994, 181, 1701, 229, 239, 153, 1681, 1686, 126, 1619, 184, 1693, 1952, 1940, 1712, 165, 1626, 1643, 1672, 1622, 1703, 1927, 1991, 1946, 1954, 176, 1633, 180, 133, 171, 144, 1663, 1670, 1641, 1696, 1648, 150, 1992, 1674, 235, 1920, 249, 1984, 162, 1706, 247, 1688, 1618, 1689, 172, 179, 1995, 1717, 1657, 246, 237, 173, 1958, 1664, 183, 148, 1676, 142, 161, 1723, 158, 233, 1924, 125, 1990, 156, 145, 1981, 1662, 147, 1718, 1700, 228, 1687, 1616, 1692, 1953, 1705, 236, 1941, 1948, 1949, 1678, 1980, 170, 1959, 1642, 1720, 1929, 1684, 1617, 1603, 1996, 1673, 175, 253, 1930, 1921, 1613, 1988, 1607, 1624, 139, 1711, 1709, 164, 1710, 1609, 1628, 1694, 248, 234, 1645, 1982, 1931, 1690, 1945, 1604, 167, 1637, 1665, 182, 1708, 254, 151, 1615, 163, 1713, 1942, 1649, 178, 1660, 134, 143, 160, 1721, 1697, 245, 1922, 240, 1651, 1951, 1950, 166, 1634, 137, 168, 1640, 1716, 140, 1638, 1714, 1698, 1635, 1623, 1707, 1699, 1625, 1639, 1658, 1630, 1666, 1606, 1677, 1644, 1919, 1656, 255, 1715, 1993, 1620, 1719, 1621, 1997, 241, 1679, 1680, 152, 1612, 1985, 155, 1987, 1608, 231, 1627, 1977, 1631, 232, 1646, 230, 1724 ];
const GIFT_ITEMS: &[usize] = &[ 10, 101, 103, 105, 107, 109, 11, 111, 112, 113, 114, 115, 116, 117, 12, 122, 124, 13, 14, 15, 200, 206, 212, 216, 218, 219, 220, 221, 222, 223, 224, 226, 227, 23, 24, 25, 256, 257, 258, 259, 260, 39, 53, 613, 614, 615, 616, 617, 62, 71, 72, 75, 77, 78, 8, 800, 801, 802, 803, 804, 82, 83, 86, 87, 88, 89, 9, 93, 94, 95, 99 ];
type Rematch = (&'static [&'static str], usize, usize, Option<usize>);
const REMATCHES: &[Rematch] = &[
    (&["ALAN1", "ALAN2", "ALAN3", "ALAN4", "ALAN5"], 0x046E, 0x0090, None),
    (&["ANTHONY2", "ANTHONY1", "ANTHONY3", "ANTHONY4", "ANTHONY5"], 0x0528, 0x008B, None),
    (&["ARNIE1", "ARNIE2", "ARNIE3", "ARNIE4", "ARNIE5"], 0x053F, 0x008F, None),
    (&["BETH1", "BETH2", "BETH3"], 0x0563, 0x0084, None),
    (&["BRENT1", "BRENT2", "BRENT3", "BRENT4"], 0x04E9, 0x0095, None),
    (&["CHAD1", "CHAD2", "CHAD3", "CHAD4", "CHAD5"], 0x0475, 0x0092, None),
    (&["DANA1", "DANA2", "DANA3", "DANA4", "DANA5"], 0x051E, 0x0091, None),
    (&["GINA1", "GINA2", "GINA3", "GINA4", "GINA5"], 0x047F, 0x008D, None),
    (&["HUEY1", "HUEY2", "HUEY3", "HUEY4"], 0x0576, 0x0082, Some(0x0265)),
    (&["JACK1", "JACK2", "JACK3", "JACK4", "JACK5"], 0x046C, 0x0080, None),
    (&["JOEY1", "JOEY2", "JOEY3", "JOEY4", "JOEY5"], 0x05A9, 0x0087, Some(0x0266)),
    (&["LIZ1", "LIZ2", "LIZ3", "LIZ4", "LIZ5"], 0x047E, 0x008A, None),
    (&["TIFFANY3", "TIFFANY1", "TIFFANY2", "TIFFANY4"], 0x0491, 0x0096, None),
    (&["TODD1", "TODD2", "TODD3", "TODD4", "TODD5"], 0x041B, 0x008C, None),
    (&["TULLY3", "TULLY1", "TULLY2", "TULLY4"], 0x0454, 0x0094, None),
    (&["WADE1", "WADE2", "WADE3", "WADE4", "WADE5"], 0x053B, 0x0088, None),
    (&["ERIN1", "ERIN2", "ERIN3"], 0x0487, 0x009B, Some(0x0269)),
    (&["GAVEN3", "GAVEN1", "GAVEN2"], 0x0551, 0x0083, None),
    (&["JOSE2", "JOSE1", "JOSE3"], 0x0408, 0x0085, None),
    (&["PARRY3", "PARRY1", "PARRY2"], 0x052C, 0x009A, Some(0x0268)),
    (&["REENA1", "REENA2", "REENA3"], 0x0564, 0x0086, None),
    (&["RALPH1", "RALPH2", "RALPH3", "RALPH4", "RALPH5"], 0x044F, 0x0089, None),
    (&["VANCE1", "VANCE2", "VANCE3"], 0x0401, 0x0097, Some(0x0267)),
    (&["WILTON1", "WILTON2", "WILTON3"], 0x0457, 0x0098, None),
];
