pub fn event(name: &str, data: &mut [u8]) -> Option<String> {
    let normalized = crate::event_simulator_x::event_name(
        name,
        &[
            "PikachuCelebration",
            "AddHearts",
            "AddJewels",
            "AddCoins",
            "ResetLimitedShop",
        ],
    );
    let name = normalized.as_ref();
    match name.trim() {
        "PikachuCelebration" | "0" => {
            for id in [888, 889, 890, 879, 880, 881, 882, 883, 884, 885, 886, 887] {
                for offset in [0xe6, 0x546, 0x5e6] {
                    data[offset + (id + 5) / 8] |= 1 << ((id + 5) % 8);
                }
            }
        }
        "AddHearts" | "1" => data[0x2d4a..0x2d4c].copy_from_slice(&[0x90, 0x31]),
        "AddJewels" | "2" => {
            data[0x6a] = (data[0x6a] & 0x0f) | 0x60;
            data[0x6b] = (data[0x6b] & 0xf0) | 9;
        }
        "AddCoins" | "3" => {
            data[0x68] = 0xf8;
            data[0x69] = 0x34;
            data[0x6a] = (data[0x6a] & 0xf0) | 0x0c;
        }
        "ResetLimitedShop" | "4" => data[0x2ec7..0x2ed1].fill(0),
        _ => {
            return Some(format!(
                "Internal error : Invalid event name \"{}\".",
                name.trim()
                    .parse::<i32>()
                    .map(|id| id.to_string())
                    .unwrap_or_default()
            ))
        }
    }
    None
}
