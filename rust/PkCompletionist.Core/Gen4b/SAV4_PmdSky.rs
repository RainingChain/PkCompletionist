#![allow(non_snake_case)]
use crate::bit_block::bits;
use pkhex_core::binary::{sum32, InvalidData};
pub fn is_valid(data: &[u8]) -> bool {
    data.len() >= 0x1e800 && data.get(4..17) == Some(b"POKE_DUN_SORA")
}

pub fn write(data: &mut [u8]) -> Result<(), InvalidData> {
    let source = data.to_vec();
    write_from(&source, data)
}

/// SAV4_PmdSky.Bits retains the original input independently from SetData edits.
pub fn write_from(source: &[u8], data: &mut [u8]) -> Result<(), InvalidData> {
    for (offset, start, end) in [
        (0, 4, 0xb65a),
        (0xc800, 0xc804, 0x17e5a),
        (0x19000, 0x19004, 0x1e7ff),
    ] {
        let checksum = sum32(source, start, end)?;
        data[offset..offset + 4].copy_from_slice(&checksum.to_le_bytes());
    }
    Ok(())
}

pub fn GetStoredPokemon(data: &[u8], baseOffset: usize) -> Result<Vec<u32>, InvalidData> {
    let mut owned = Vec::new();
    for index in 0..720 {
        let id = bits(data, baseOffset * 8 + 0x464 * 8 + index * 362 + 8, 11)?;
        owned.push(if id >= 600 { id - 600 } else { id });
    }
    Ok(owned)
}

pub fn GetOwnedItems(data: &[u8], baseOffset: usize) -> Result<Vec<u32>, InvalidData> {
    let mut items = Vec::new();
    for index in 0..1000 {
        let id = bits(data, baseOffset * 8 + 0x8e0c * 8 + 6 + index * 11, 11)?;
        if id == 0 {
            break;
        }
        items.push(id);
    }
    for index in 0..50 {
        let offset = baseOffset * 8 + 0x8ba2 * 8 + index * 33;
        if bits(data, offset, 1)? == 0 {
            break;
        }
        let id = bits(data, offset + 19, 11)?;
        if id > 0 {
            items.push(id);
        }
    }
    Ok(items)
}
