use pkhex_core::binary::{sum32, InvalidData};
pub fn is_valid(data: &[u8]) -> bool { data.len() >= 0x1e800 && data.get(4..17) == Some(b"POKE_DUN_SORA") }

pub fn write(data: &mut [u8]) -> Result<(), InvalidData> {
    for (offset, start, end) in [(0,4,0xb65a), (0xc800,0xc804,0x17e5a), (0x19000,0x19004,0x1e7ff)] {
        let checksum = sum32(data, start, end)?;
        data[offset..offset + 4].copy_from_slice(&checksum.to_le_bytes());
    }
    Ok(())
}

