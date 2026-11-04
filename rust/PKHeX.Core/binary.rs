//! Checked binary primitives shared by save formats.
use std::fmt;

#[derive(Debug, Clone, PartialEq, Eq)]
pub struct InvalidData(pub &'static str);

impl fmt::Display for InvalidData {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result { f.write_str(self.0) }
}
impl std::error::Error for InvalidData {}

pub fn u16_le(data: &[u8], offset: usize) -> Result<u16, InvalidData> {
    let end = offset.checked_add(2).ok_or(InvalidData("Offset overflow."))?;
    let bytes = data.get(offset..end).ok_or(InvalidData("Truncated save."))?;
    Ok(u16::from_le_bytes([bytes[0], bytes[1]]))
}

pub fn u32_le(data: &[u8], offset: usize) -> Result<u32, InvalidData> {
    let end = offset.checked_add(4).ok_or(InvalidData("Offset overflow."))?;
    let bytes = data.get(offset..end).ok_or(InvalidData("Truncated save."))?;
    Ok(u32::from_le_bytes([bytes[0], bytes[1], bytes[2], bytes[3]]))
}

/// BitBlock-compatible little-endian bit numbering, including unaligned fields.
pub fn bits(data: &[u8], offset: usize, width: usize) -> Result<u32, InvalidData> {
    let end = offset.checked_add(width).ok_or(InvalidData("Offset overflow."))?;
    if width > 32 || end > data.len().saturating_mul(8) { return Err(InvalidData("Invalid bit range.")); }
    Ok((0..width).fold(0, |value, bit| value | (u32::from((data[(offset + bit) / 8] >> ((offset + bit) % 8)) & 1) << bit)))
}

/// Sum little-endian 32-bit words through the inclusive final word offset.
pub fn sum32(data: &[u8], start: usize, end: usize) -> Result<u32, InvalidData> {
    (start..=end).step_by(4).try_fold(0_u32, |sum, offset| Ok(sum.wrapping_add(u32_le(data, offset)?)))
}
