use crate::{
    command::{load, write, Format},
    CommandResult,
};

pub fn describe(bytes: &[u8], hint: &str) -> CommandResult {
    let (format, data) = match load(bytes, hint) {
        Ok(save) => save,
        Err(error) => {
            let mut result = CommandResult::failure(error.to_string());
            result.descriptor = vec![String::new(); 3];
            return result;
        }
    };
    let (version, language, trainer, hours, minutes, seconds) = match format {
        Format::Emerald => {
            let save = match pkhex_core::gen3::Save3::read(&data) {
                Ok(save) => save,
                Err(error) => return CommandResult::failure(error.to_string()),
            };
            (
                "E",
                "0",
                pkhex_core::string_converter3::decode(&save.small[..7], save.small[6..8] == [0, 0]),
                pkhex_core::binary::u16_le(&save.small, 0xe).unwrap(),
                save.small[0x10],
                save.small[0x11],
            )
        }
        Format::Yellow => (
            if if data[0x29c3] != 0 {
                data[0x29c3] == 0x54
            } else {
                data[0x271c] != 0
            } {
                "YW"
            } else {
                "RB"
            },
            "0",
            pkhex_core::string_converter1::decode(&data[0x2598..0x259f]),
            u16::from(data[0x2ced]),
            data[0x2cef],
            data[0x2cf0],
        ),
        Format::Crystal => (
            "C",
            "0",
            pkhex_core::string_converter2::decode(&data[0x200b..0x2012]),
            u16::from_be_bytes([data[0x2052], data[0x2053]]),
            data[0x2054],
            data[0x2055],
        ),
        _ => (
            "Unknown",
            "-1",
            match format {
                Format::PinballRs => "PRS",
                Format::Pinball => "PINBALL",
                Format::Shuffle => "SHUFFLE",
                Format::Ranger => "RANGER",
                Format::Rescue => "PmdRescueTeam",
                Format::Sky => "PmdSky",
                _ => unreachable!(),
            }
            .to_string(),
            0,
            0,
            0,
        ),
    };
    let mut time = format!("{hours}ː{minutes:02}ː{seconds:02}");
    if format == Format::Yellow && data[0x2cee] != 0 {
        time.push_str(&format!(
            " {:04X}",
            pkhex_core::checksums::crc16_ccitt(&data)
        ));
    }
    let descriptor = vec![
        version.to_string(),
        language.to_string(),
        format!("{trainer} ({version}) - {time}"),
    ];
    match write(format, data) {
        Ok(save_a) => CommandResult {
            success: true,
            descriptor,
            save_a,
            ..CommandResult::default()
        },
        Err(error) => CommandResult::failure(error.to_string()),
    }
}
