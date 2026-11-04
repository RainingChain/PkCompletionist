//! Inventory subset of PlayerBag2 and InventoryPouchGB used by Crystal completion.
use crate::item_storage2::MACHINE;

pub fn items(data: &[u8]) -> Vec<u16> {
    let mut items = Vec::new();
    for (index, id) in MACHINE.iter().enumerate() { if data[0x23e7 + index] != 0 { items.push(*id); } }
    for (offset, capacity, keys) in [(0x2420,20,false), (0x244a,26,true), (0x2465,12,false), (0x247f,50,false)] {
        let count = data[offset] as usize;
        if count > capacity { continue; }
        for index in 0..count { items.push(u16::from(data[offset + 1 + index * if keys { 1 } else { 2 }])); }
    }
    items
}
