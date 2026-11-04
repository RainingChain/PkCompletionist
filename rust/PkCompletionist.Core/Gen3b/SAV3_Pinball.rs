// Ported from PkCompletionist.Core/Gen3b/{SAV3_Pinball,EventSimulator3_Pinball}.cs.
use pkhex_core::binary::{u16_le, InvalidData};

pub fn is_valid(data: &[u8]) -> bool {
    // C# accepts 0x400 bytes, but serialization needs the complete backup region.
    data.len() >= 0x518 && data.get(0x268..0x272) == Some(b"POKEPINAGB")
}

pub fn set_checksums(data: &mut [u8]) -> Result<(), InvalidData> {
    if data.len() < 0x518 {
        return Err(InvalidData("Truncated Pinball Ruby & Sapphire save."));
    }
    data[0x272..0x274].fill(0);
    let mut checksum = (4..0x278).step_by(2).try_fold(0_u32, |sum, offset| {
        Ok(sum + u32::from(u16_le(data, offset)?))
    })?;
    checksum = (checksum & 0xffff) + (checksum >> 16);
    checksum = !((checksum >> 16) + checksum);
    data[0x272..0x274].copy_from_slice(&(checksum as u16).to_le_bytes());
    data.copy_within(4..0x278, 0x2a4);
    Ok(())
}
