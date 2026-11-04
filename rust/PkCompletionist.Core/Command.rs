use crate::{
    pinball_save as pinball, ranger_save as ranger, rescue_save as rescue, shuffle_save as shuffle,
    sky_save as sky,
};
use pkhex_core::binary::InvalidData;
use serde_json::{json, Value};
#[derive(Debug, Default, Clone, PartialEq, Eq)]
pub struct CommandResult {
    pub success: bool,
    pub error: Option<String>,
    pub messages: Vec<String>,
    pub obtained_status: Vec<String>,
    pub completion_hints: Vec<String>,
    pub descriptor: Vec<String>,
    pub save_a: Vec<u8>,
    pub save_b: Vec<u8>,
}

impl CommandResult {
    pub(crate) fn failure(error: impl Into<String>) -> Self {
        let error = error.into();
        Self {
            error: Some(error.clone()),
            messages: vec![error],
            ..Self::default()
        }
    }

    /// Stable wire format consumed by CLI tooling and the browser compatibility adapter.
    pub fn json(&self) -> Value {
        json!({"Success": self.success, "Error": self.error, "Messages": self.messages,
            "ObtainedStatus": self.obtained_status, "CompletionHints": self.completion_hints, "Descriptor": self.descriptor})
    }
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub(crate) enum Format {
    PinballRs,
    Pinball,
    Shuffle,
    Ranger,
    Rescue,
    Sky,
    Crystal,
    Yellow,
    Emerald,
}

fn detect(data: &[u8], hint: &str) -> Option<Format> {
    // Preserve Command.GetVariantSAV's precedence.
    if shuffle::is_valid(data) {
        Some(Format::Shuffle)
    } else if ranger::is_valid(data) {
        Some(Format::Ranger)
    } else if pinball::is_valid(data) {
        Some(Format::PinballRs)
    } else if hint == "Pinball" && data.len() >= 0x10c + 151 {
        Some(Format::Pinball)
    } else if rescue::is_valid(data) {
        Some(Format::Rescue)
    } else if sky::is_valid(data) {
        Some(Format::Sky)
    } else if pkhex_core::gen1::Save1::is_valid(data) {
        Some(Format::Yellow)
    } else if pkhex_core::gen2::CrystalSave::is_valid(data) {
        Some(Format::Crystal)
    } else if pkhex_core::emerald::is_valid(data) {
        Some(Format::Emerald)
    } else {
        None
    }
}

pub(crate) fn load(data: &[u8], hint: &str) -> Result<(Format, Vec<u8>), InvalidData> {
    let format = detect(data, hint).ok_or(InvalidData(
        "Save format has not been migrated or is invalid.",
    ))?;
    let bytes = if format == Format::Ranger {
        ranger::decrypt(data)?
    } else {
        data.to_vec()
    };
    Ok((format, bytes))
}

pub(crate) fn write(format: Format, mut data: Vec<u8>) -> Result<Vec<u8>, InvalidData> {
    if format == Format::Emerald {
        return Ok(pkhex_core::gen3::Save3::read(&data)?.write());
    }
    if format == Format::PinballRs {
        pinball::set_checksums(&mut data)?;
    }
    if format == Format::Rescue {
        rescue::write(&mut data)?;
    }
    if format == Format::Sky {
        sky::write(&mut data)?;
    }
    if format == Format::Crystal {
        return Ok(pkhex_core::gen2::CrystalSave::read(&data)?.write());
    }
    if format == Format::Yellow {
        return Ok(pkhex_core::gen1::Save1::read(&data)?.write());
    }
    Ok(data)
}

pub fn fix_checksum(data: &[u8], hint: &str) -> Result<Vec<u8>, InvalidData> {
    if pkhex_core::gen2::CrystalSave::is_valid(data) {
        return Ok(pkhex_core::gen2::CrystalSave::read(data)?.write());
    }
    let (format, bytes) = load(data, hint)?;
    write(format, bytes)
}
