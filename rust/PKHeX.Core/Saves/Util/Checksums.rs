//! CRC16_CCITT port from the bundled PKHeX Checksums.cs.
pub fn crc16_ccitt(data: &[u8]) -> u16 {
    let (mut top, mut bottom) = (0xff_u8, 0xff_u8);
    for byte in data {
        let mut x = byte ^ top;
        x ^= x >> 4;
        top = bottom ^ (x >> 3) ^ (x << 4);
        bottom = x ^ (x << 5);
    }
    (u16::from(top) << 8) | u16::from(bottom)
}
