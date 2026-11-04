use crate::{
    binary::{u16_le, u32_le},
    gen3::Save3,
    item_storage3e,
};

const POUCHES: &[(usize, usize, u16)] = &[
    (0xc8, 30, 99),
    (0x140, 30, 1),
    (0x1b8, 16, 99),
    (0x1f8, 64, 99),
    (0x2f8, 46, 999),
    (0, 50, 999),
];

pub fn items(save: &Save3) -> Vec<u16> {
    let mut items = Vec::new();
    for (offset, count, _) in POUCHES {
        for index in 0..*count {
            items.push(u16_le(&save.large, 0x498 + offset + index * 4).unwrap());
        }
    }
    items
}

pub fn give_item(save: &mut Save3, id: u16) -> Result<(), &'static str> {
    let candidates = [
        item_storage3e::GENERAL,
        item_storage3e::KEY,
        item_storage3e::BALLS,
        item_storage3e::MACHINE,
        item_storage3e::BERRY,
    ];
    let pouch = candidates
        .iter()
        .position(|items| items.contains(&id))
        .ok_or("Error: No valid pouch found for that item.")?;
    let (offset, capacity, max) = POUCHES[pouch];
    let existing =
        (0..capacity).find(|index| u16_le(&save.large, 0x498 + offset + index * 4).unwrap() == id);
    let slot = existing
        .or_else(|| {
            (0..capacity)
                .find(|index| u16_le(&save.large, 0x498 + offset + index * 4).unwrap() == 0)
        })
        .ok_or("Error: Your item pouch is full.")?;
    let start = 0x498 + offset + slot * 4;
    let key = u32_le(&save.small, 0xac).unwrap() as u16;
    let old = if existing.is_some() {
        u16_le(&save.large, start + 2).unwrap() ^ key
    } else {
        0
    };
    let quantity = old.saturating_add(1).min(max) ^ key;
    save.large[start..start + 2].copy_from_slice(&id.to_le_bytes());
    save.large[start + 2..start + 4].copy_from_slice(&quantity.to_le_bytes());
    Ok(())
}
