//! Generation 3 subset of PokeCrypto.cs.
const ORDER: [[usize; 4]; 24] = [
    [0, 1, 2, 3],
    [0, 1, 3, 2],
    [0, 2, 1, 3],
    [0, 3, 1, 2],
    [0, 2, 3, 1],
    [0, 3, 2, 1],
    [1, 0, 2, 3],
    [1, 0, 3, 2],
    [2, 0, 1, 3],
    [3, 0, 1, 2],
    [2, 0, 3, 1],
    [3, 0, 2, 1],
    [1, 2, 0, 3],
    [1, 3, 0, 2],
    [2, 1, 0, 3],
    [3, 1, 0, 2],
    [2, 3, 0, 1],
    [3, 2, 0, 1],
    [1, 2, 3, 0],
    [1, 3, 2, 0],
    [2, 1, 3, 0],
    [3, 1, 2, 0],
    [2, 3, 1, 0],
    [3, 2, 1, 0],
];

pub fn checksum3(data: &[u8]) -> u16 {
    data[32..80].chunks_exact(2).fold(0_u16, |sum, word| {
        sum.wrapping_add(u16::from_le_bytes([word[0], word[1]]))
    })
}

fn crypt(data: &mut [u8]) {
    let pid = u32::from_le_bytes(data[..4].try_into().unwrap());
    let trainer = u32::from_le_bytes(data[4..8].try_into().unwrap());
    for word in data[32..80].chunks_exact_mut(4) {
        let value = u32::from_le_bytes(word.try_into().unwrap()) ^ pid ^ trainer;
        word.copy_from_slice(&value.to_le_bytes());
    }
}

pub fn decrypt3(data: &mut [u8]) {
    crypt(data);
    let order = ORDER[(u32::from_le_bytes(data[..4].try_into().unwrap()) % 24) as usize];
    let source = data[32..80].to_vec();
    for (index, slot) in order.iter().enumerate() {
        data[32 + index * 12..44 + index * 12].copy_from_slice(&source[slot * 12..slot * 12 + 12]);
    }
}

pub fn encrypt3(data: &mut [u8]) {
    let order = ORDER[(u32::from_le_bytes(data[..4].try_into().unwrap()) % 24) as usize];
    let source = data[32..80].to_vec();
    for (index, slot) in order.iter().enumerate() {
        data[32 + slot * 12..44 + slot * 12].copy_from_slice(&source[index * 12..index * 12 + 12]);
    }
    crypt(data);
}
