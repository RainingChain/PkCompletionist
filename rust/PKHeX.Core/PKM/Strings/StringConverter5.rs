//! Read-only string conversion used by Generation 5 saves and Pokémon.
pub fn decode(data: &[u8]) -> String {
    let units: Vec<_> = data
        .chunks_exact(2)
        .map(|word| u16::from_le_bytes([word[0], word[1]]))
        .take_while(|value| *value != 0 && *value != 0xffff)
        .map(|value| match value {
            0x246d => 0x2642,
            0x246e => 0x2640,
            _ => value,
        })
        .collect();
    String::from_utf16_lossy(&units)
}
