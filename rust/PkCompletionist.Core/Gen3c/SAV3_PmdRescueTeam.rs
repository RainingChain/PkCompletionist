use pkhex_core::binary::{sum32, InvalidData};
pub fn is_valid(data: &[u8]) -> bool {
    data.len() >= 0x57d4 && data.get(0x404..0x410) == Some(b"POKE_DUNGEON")
}

pub fn write(data: &mut [u8]) -> Result<(), InvalidData> {
    let checksum = sum32(data, 4, 0x57d0)?;
    data[..4].copy_from_slice(&checksum.to_le_bytes());
    Ok(())
}
