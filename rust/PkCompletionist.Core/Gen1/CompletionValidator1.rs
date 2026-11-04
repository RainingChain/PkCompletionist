#![allow(non_snake_case)]
use crate::{category, completion_validator_x::Checklist, CommandResult};
use pkhex_core::{binary::InvalidData, gen1::Save1};
struct CompletionValidator1 {
    save: Save1,
    objective: i32,
}
pub fn validate(
    data: &[u8],
    objective: i32,
    result: &mut CommandResult,
) -> Result<(), InvalidData> {
    CompletionValidator1 {
        save: Save1::read(data)?,
        objective,
    }
    .GenerateAll(result);
    Ok(())
}
impl CompletionValidator1 {
    fn GenerateAll(&self, result: &mut CommandResult) {
        self.Generate_pokemon(result);
        self.Generate_item(result);
        self.Generate_inGameGift(result);
        self.Generate_misc(result);
        self.Generate_inGameTrade(result);
        self.Generate_itemGift(result);
        self.Generate_itemInMap(result);
        self.Generate_battle(result);
    }
    fn Generate_pokemon(&self, result: &mut CommandResult) {
        category(
            result,
            "pokemon",
            (1_u8..=151).map(|id| {
                (
                    id.to_string(),
                    if self.objective == 0 {
                        self.save.flag(0x25a3, id as usize - 1)
                    } else {
                        self.save.pokemon().any(|pk| pk.species() == id)
                    },
                )
            }),
        );
    }
    fn Generate_misc(&self, result: &mut CommandResult) {
        category(
            result,
            "misc",
            [(
                "PlayPikachusBeachMinigame".into(),
                self.save.data[0x2741] != 0 || self.save.data[0x2742] != 0,
            )],
        );
    }
    fn Generate_itemGift(&self, result: &mut CommandResult) {
        category(
            result,
            "itemGift",
            [36, 442, 443, 444, 960, 1344]
                .into_iter()
                .map(|id| (id.to_string(), self.save.event(id))),
        );
    }
    fn Generate_item(&self, result: &mut CommandResult) {
        let save = &self.save;
        let data = &save.data;
        let objective = self.objective;
        let mut owned_items = Vec::new();
        for (offset, capacity) in [(0x25c9, 20), (0x27e6, 50)] {
            if data[offset] as usize > capacity {
                continue;
            }
            for index in 0..data[offset] as usize {
                owned_items.push(data[offset + 1 + index * 2] as usize);
            }
        }
        let mut items = Checklist::default();
        for id in 1..=255 {
            items.set(id, owned_items.contains(&id));
        }
        for (kind, flag, id) in ITEM_FLAGS {
            if items.get(&id.to_string()) {
                continue;
            }
            let owned = match kind {
                0 => save.flag(0x2852, *flag),
                1 => save.flag(0x299c, *flag),
                _ => save.event(*flag),
            };
            items.set(id, owned);
        }
        if data[0x2602] == 0xff {
            items.set(8, true);
        }
        if objective == 0 {
            for id in 1..=255 {
                if !items.get(&id.to_string()) {
                    items.remove(&id.to_string());
                }
            }
            for id in ITEM_ONLY_WITH_FLAGS {
                if !items.get(&id.to_string()) {
                    items.set(id, false);
                }
            }
        }
        items.emit(result, "item");
    }

    fn Generate_itemInMap(&self, result: &mut CommandResult) {
        let save = &self.save;
        let mut map = Checklist::default();
        for id in FIELD_ITEMS {
            map.set(id, save.flag(0x2852, *id));
        }
        for id in HIDDEN_ITEMS {
            map.set(format!("h{id}"), save.flag(0x299c, *id));
        }
        for id in HIDDEN_COINS {
            map.set(format!("hc{id}"), save.flag(0x29aa, *id));
        }
        map.emit(result, "itemInMap");
    }

    fn Generate_battle(&self, result: &mut CommandResult) {
        let save = &self.save;
        let mut battles = Checklist::default();
        for id in [239, 1856, 1318] {
            battles.set(format!("{id}a"), save.event(id));
        }
        for id in ["2289", "2297", "2273", "2281", "2305a"] {
            battles.set(id, save.flag(0x2852, 9));
        }
        battles.set("115", save.event(1504));
        battles.set("70", save.flag(0x2852, 139));
        for id in BATTLES {
            battles.set(id, save.event(*id));
        }
        battles.emit(result, "battle");
    }
}
// Generated literal tables and simple rules follow. See tools/port-yellow-rules.ps1.
impl CompletionValidator1 {
    fn Generate_inGameGift(&self, result: &mut CommandResult) {
        let save = &self.save;
        let mut entries = Checklist::default();
        entries.set("Magikarp", save.event(0x3FF));
        entries.set("Bulbasaur", save.event(0x0A8));
        entries.set("Charmander", save.event(0x54F));
        entries.set("Squirtle", save.event(0x147));
        entries.set("Eevee", save.flag(0x2852, 0x45));
        entries.set("Hitmonlee", save.event(0x356));
        entries.set("Hitmonchan", save.event(0x357));
        entries.set("Pikachu", true);
        entries.set("Lapras", save.flag(0x29DA, 0));
        entries.emit(result, "inGameGift");
    }
    fn Generate_inGameTrade(&self, result: &mut CommandResult) {
        let save = &self.save;
        let mut entries = Checklist::default();
        entries.set("LickitungforDugtrio", save.flag(0x29E3, 0));
        entries.set("ClefairyforMrMime", save.flag(0x29E3, 1));
        entries.set("KangaskhanforMuk", save.flag(0x29E3, 3));
        entries.set("TangelaforParasect", save.flag(0x29E3, 5));
        entries.set("GolduckforRhydon", save.flag(0x29E3, 7));
        entries.set("GrowlitheforDewgong", save.flag(0x29E4, 0));
        entries.set("CuboneforMachamp", save.flag(0x29E4, 1));
        entries.emit(result, "inGameTrade");
    }
}
const ITEM_FLAGS: &[(usize, usize, usize)] = &[
    (0, 0x07, 228),
    (0, 0x1A, 10),
    (0, 0x1B, 35),
    (0, 0x1C, 204),
    (0, 0x1D, 230),
    (0, 0x1F, 216),
    (0, 0x20, 37),
    (0, 0x21, 220),
    (0, 0x26, 245),
    (0, 0x27, 219),
    (0, 0x32, 53),
    (0, 0x35, 40),
    (0, 0x36, 83),
    (0, 0x37, 54),
    (0, 0x38, 2),
    (0, 0x3A, 29),
    (0, 0x3B, 82),
    (0, 0x3C, 14),
    (0, 0x3D, 35),
    (0, 0x3E, 49),
    (0, 0x3F, 40),
    (0, 0x40, 46),
    (0, 0x47, 40),
    (0, 0x48, 29),
    (0, 0x49, 38),
    (0, 0x56, 38),
    (0, 0x57, 35),
    (0, 0x58, 40),
    (0, 0x59, 225),
    (0, 0x5A, 233),
    (0, 0x5C, 217),
    (0, 0x5D, 52),
    (0, 0x5E, 205),
    (0, 0x5F, 55),
    (0, 0x64, 11),
    (0, 0x65, 20),
    (0, 0x66, 4),
    (0, 0x67, 20),
    (0, 0x68, 10),
    (0, 0x69, 40),
    (0, 0x6A, 29),
    (0, 0x6B, 20),
    (0, 0x6C, 212),
    (0, 0x71, 35),
    (0, 0x72, 201),
    (0, 0x74, 208),
    (0, 0x75, 81),
    (0, 0x76, 40),
    (0, 0x77, 80),
    (0, 0x78, 244),
    (0, 0x79, 17),
    (0, 0x7A, 54),
    (0, 0x7B, 247),
    (0, 0x7D, 29),
    (0, 0x7E, 18),
    (0, 0x7F, 10),
    (0, 0x80, 49),
    (0, 0x81, 207),
    (0, 0x82, 19),
    (0, 0x83, 210),
    (0, 0x84, 40),
    (0, 0x88, 35),
    (0, 0x89, 202),
    (0, 0x8A, 37),
    (0, 0x8B, 72),
    (0, 0x8C, 74),
    (0, 0x94, 18),
    (0, 0x98, 52),
    (0, 0x99, 54),
    (0, 0x9A, 29),
    (0, 0x9F, 209),
    (0, 0xA0, 36),
    (0, 0xA1, 48),
    (0, 0xA5, 35),
    (0, 0xA6, 46),
    (0, 0xAC, 39),
    (0, 0xAD, 203),
    (0, 0xB8, 226),
    (0, 0xB9, 40),
    (0, 0xBA, 38),
    (0, 0xC0, 39),
    (0, 0xC1, 17),
    (0, 0xC2, 37),
    (0, 0xC3, 40),
    (0, 0xC4, 16),
    (0, 0xC5, 214),
    (0, 0xC6, 222),
    (0, 0xC7, 43),
    (0, 0xC8, 16),
    (0, 0xC9, 17),
    (0, 0xCA, 38),
    (0, 0xCB, 237),
    (0, 0xCC, 36),
    (0, 0xCD, 240),
    (0, 0xCE, 17),
    (0, 0xCF, 232),
    (0, 0xD0, 54),
    (0, 0xD1, 64),
    (0, 0xD2, 49),
    (0, 0xD3, 40),
    (0, 0xD4, 2),
    (0, 0xD5, 54),
    (0, 0xD6, 16),
    (0, 0xD8, 2),
    (0, 0xD9, 2),
    (0, 0xDA, 54),
    (0, 0xDB, 83),
    (0, 0xDC, 243),
    (0, 0xDD, 40),
    (1, 0x00, 82),
    (1, 0x01, 17),
    (1, 0x02, 54),
    (1, 0x03, 40),
    (1, 0x04, 53),
    (1, 0x05, 79),
    (1, 0x06, 79),
    (1, 0x08, 49),
    (1, 0x09, 83),
    (1, 0x0A, 2),
    (1, 0x0B, 20),
    (1, 0x0C, 11),
    (1, 0x0D, 10),
    (1, 0x0E, 80),
    (1, 0x0F, 3),
    (1, 0x10, 18),
    (1, 0x11, 16),
    (1, 0x12, 68),
    (1, 0x13, 49),
    (1, 0x14, 82),
    (1, 0x15, 79),
    (1, 0x16, 49),
    (1, 0x17, 19),
    (1, 0x18, 19),
    (1, 0x19, 81),
    (1, 0x1A, 83),
    (1, 0x1B, 79),
    (1, 0x1C, 29),
    (1, 0x1D, 18),
    (1, 0x1E, 79),
    (1, 0x1F, 39),
    (1, 0x20, 40),
    (1, 0x21, 16),
    (1, 0x22, 79),
    (1, 0x23, 54),
    (1, 0x24, 83),
    (1, 0x25, 16),
    (1, 0x26, 2),
    (1, 0x27, 81),
    (1, 0x28, 2),
    (1, 0x29, 16),
    (1, 0x2A, 80),
    (1, 0x2B, 82),
    (1, 0x2C, 3),
    (1, 0x2D, 80),
    (1, 0x2E, 49),
    (1, 0x2F, 20),
    (1, 0x30, 40),
    (1, 0x31, 79),
    (1, 0x32, 82),
    (1, 0x33, 81),
    (1, 0x34, 79),
    (1, 0x36, 10),
    (2, 0x018, 5),
    (2, 0x024, 4),
    (2, 0x025, 9),
    (2, 0x029, 242),
    (2, 0x039, 70),
    (2, 0x050, 227),
    (2, 0x069, 31),
    (2, 0x076, 234),
    (2, 0x0BE, 211),
    (2, 0x0C0, 6),
    (2, 0x128, 73),
    (2, 0x151, 45),
    (2, 0x166, 224),
    (2, 0x180, 241),
    (2, 0x18C, 213),
    (2, 0x18D, 248),
    (2, 0x18E, 249),
    (2, 0x18F, 218),
    (2, 0x1A8, 221),
    (2, 0x238, 199),
    (2, 0x258, 206),
    (2, 0x298, 238),
    (2, 0x2D7, 235),
    (2, 0x340, 231),
    (2, 0x360, 246),
    (2, 0x3B0, 229),
    (2, 0x3D8, 200),
    (2, 0x3C0, 20),
    (2, 0x47F, 71),
    (2, 0x480, 239),
    (2, 0x4B0, 75),
    (2, 0x4CE, 197),
    (2, 0x540, 49),
    (2, 0x55C, 63),
    (2, 0x578, 41),
    (2, 0x57F, 42),
    (2, 0x5E0, 196),
    (2, 0x6FF, 236),
    (2, 0x78D, 1),
    (2, 0x880, 198),
];
const ITEM_ONLY_WITH_FLAGS: &[usize] = &[
    1, 5, 6, 8, 9, 10, 31, 40, 42, 43, 45, 48, 49, 54, 63, 64, 69, 70, 71, 72, 73, 74, 75, 76, 77,
    78, 79, 80, 81, 82, 83, 196, 197, 198, 199, 200, 203, 204, 206, 208, 210, 211, 212, 213, 214,
    216, 219, 220, 221, 222, 224, 225, 226, 227, 228, 229, 230, 231, 234, 235, 236, 238, 239, 240,
    241, 242, 243, 244, 245, 246, 247, 248, 249,
];
const FIELD_ITEMS: &[usize] = &[
    100, 101, 102, 103, 104, 105, 106, 107, 108, 113, 114, 116, 117, 118, 119, 120, 121, 122, 123,
    125, 126, 127, 128, 129, 130, 131, 132, 136, 137, 138, 139, 140, 148, 152, 153, 154, 159, 160,
    161, 165, 166, 172, 173, 184, 185, 186, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202,
    203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 216, 217, 218, 219, 220, 221, 26,
    27, 28, 29, 31, 32, 33, 38, 39, 50, 53, 54, 55, 56, 58, 59, 60, 61, 62, 63, 64, 71, 72, 73, 86,
    87, 88, 89, 90, 92, 93, 94, 95,
];
const HIDDEN_ITEMS: &[usize] = &[
    0, 1, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 2, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 3, 30,
    31, 32, 33, 34, 35, 36, 37, 38, 39, 4, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 5, 50, 51, 52,
    54, 6, 8, 9,
];
const HIDDEN_COINS: &[usize] = &[0, 1, 10, 2, 3, 4, 5, 6, 7, 8, 9];
const BATTLES: &[usize] = &[
    35, 994, 995, 1394, 1346, 1361, 1362, 1365, 1521, 1137, 1138, 1139, 1140, 1090, 1378, 1379,
    1380, 996, 997, 998, 1398, 1399, 1347, 1041, 1042, 1095, 1097, 1381, 999, 1000, 1001, 1010,
    1395, 1397, 1348, 1349, 1364, 1366, 1522, 1537, 1073, 1074, 1075, 1076, 426, 427, 1382, 1476,
    1477, 1553, 1554, 1555, 1556, 1557, 354, 114, 1363, 1350, 1043, 1091, 1154, 1044, 1351, 186,
    1045, 1092, 1089, 1105, 1106, 2481, 2482, 428, 1169, 1170, 1171, 1172, 1281, 1113, 1114, 1115,
    1201, 1202, 1203, 1204, 1282, 1046, 1107, 1108, 2483, 2484, 2485, 1340, 1116, 1396, 1401, 1077,
    1078, 1079, 666, 667, 668, 669, 1393, 1367, 1368, 1369, 1093, 1094, 1109, 1110, 2486, 2487,
    2488, 1117, 1118, 1119, 1096, 1173, 1185, 1205, 1206, 1217, 1218, 1219, 1233, 1234, 1235, 1236,
    1237, 1186, 1187, 1188, 670, 671, 672, 2049, 2065, 2081, 1141, 1142, 1538, 1558, 1155, 1156,
    1157, 1158, 1297, 1298, 1299, 1300, 1159, 187, 1265, 1266, 1267, 1268, 1269, 1270, 1271, 1283,
    1284, 1285, 1301, 1302, 1303, 1304, 1220, 1221, 1222, 1238, 1239, 1240, 1241, 1242, 1305, 1143,
    1144, 1145, 1146, 1080, 1081, 429, 430, 431, 1174, 1175, 1286, 1287, 1288, 1207, 1208, 1272,
    1273, 1274, 1289, 866, 867, 868, 869, 355, 1160, 1826, 1338, 602, 603, 1341, 604, 605, 606,
    607, 82, 83, 1339, 1176, 1177, 1178, 1189, 1190, 1209, 1210, 1249, 1250, 1251, 1290, 1191,
    1192, 1193, 1194, 850, 851, 852, 853, 849, 84, 85, 86, 1337, 1317, 152, 1778, 1779, 1794, 1810,
    1827, 1846, 1861, 1874, 1890, 1905, 2066, 2082, 649, 1703, 1935, 81, 1403, 1404, 1405, 167,
    1345, 1649, 1650, 1651, 1652, 1653, 1665, 1681, 1682, 1700, 1780, 1781, 1795, 1811, 1812, 1828,
    1829, 1847, 1848, 1862, 1863, 1864, 1875, 1876, 1891, 1892, 1906, 1925, 273, 1402, 1698, 1924,
    87, 1633, 1634, 2321, 88, 89, 432, 1635, 1636, 2322, 119, 191, 359, 425, 601, 665, 865, 1523,
    1524, 1539, 1540, 356, 241, 242, 243, 249, 250, 251, 258, 259, 260, 261, 265, 266, 267, 870,
    871, 872, 271, 2522, 1129, 1342, 2241, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1167,
    1225,
];
