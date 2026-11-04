//! Inventory subset of PlayerBag2 and InventoryPouchGB used by Crystal completion.
use crate::item_storage2::{BALLS, GENERAL, KEY_CRYSTAL, MACHINE};

pub fn items(data: &[u8]) -> Vec<u16> {
    let mut items = Vec::new();
    for (index, id) in MACHINE.iter().enumerate() {
        if data[0x23e7 + index] != 0 {
            items.push(*id);
        }
    }
    for (offset, capacity, keys) in [
        (0x2420, 20, false),
        (0x244a, 26, true),
        (0x2465, 12, false),
        (0x247f, 50, false),
    ] {
        let count = data[offset] as usize;
        if count > capacity {
            continue;
        }
        for index in 0..count {
            items.push(u16::from(
                data[offset + 1 + index * if keys { 1 } else { 2 }],
            ));
        }
    }
    items
}

pub fn give_item(data: &mut [u8], id: u16) -> Result<(), &'static str> {
    // Select the first compatible pouch, in PlayerBag2 order. The PC is last.
    let target = if MACHINE.contains(&id) {
        0
    } else if GENERAL.contains(&id) {
        1
    } else if KEY_CRYSTAL.contains(&id) {
        2
    } else if BALLS.contains(&id) {
        3
    } else {
        return Err("Error: No valid pouch found for that item.");
    };
    let specs = [
        (0x23e7, 57, false),
        (0x2420, 20, false),
        (0x244a, 26, true),
        (0x2465, 12, false),
        (0x247f, 50, false),
    ];
    let mut pouches = Vec::new();
    for (pouch_index, (offset, capacity, keys)) in specs.iter().enumerate() {
        let mut entries = vec![(0_u16, 0_u8); *capacity];
        if pouch_index == 0 {
            let mut index = 0;
            for (slot, item) in MACHINE.iter().enumerate() {
                if data[offset + slot] > 0 {
                    entries[index] = (*item, data[offset + slot]);
                    index += 1;
                }
            }
        } else if data[*offset] as usize <= *capacity {
            for (slot, entry) in entries.iter_mut().take(data[*offset] as usize).enumerate() {
                *entry = (
                    u16::from(data[offset + 1 + slot * if *keys { 1 } else { 2 }]),
                    if *keys {
                        1
                    } else {
                        data[offset + 2 + slot * 2]
                    },
                );
            }
        }
        pouches.push(entries);
    }
    let entries = &mut pouches[target];
    let entry = entries.iter_mut().find(|(item, _)| *item == id);
    if let Some(entry) = entry {
        entry.1 = entry.1.saturating_add(1).min(99);
    } else if let Some(entry) = entries.iter_mut().find(|(item, _)| *item == 0) {
        *entry = (id, 1);
    } else {
        return Err("Error: Your item pouch is full.");
    }
    for (pouch_index, ((offset, capacity, keys), entries)) in
        specs.into_iter().zip(pouches).enumerate()
    {
        let mut entries: Vec<_> = entries
            .into_iter()
            .filter(|(_, count)| *count != 0)
            .collect();
        let count = entries.len();
        entries.resize(capacity, (0, 0));
        if pouch_index == 0 {
            for (item, count) in entries {
                if let Some(index) = MACHINE.iter().position(|known| *known == item) {
                    data[offset + index] = count;
                }
            }
        } else {
            data[offset] = count as u8;
            for (slot, (id, count)) in entries.into_iter().enumerate() {
                if keys {
                    data[offset + 1 + slot] = id as u8;
                } else {
                    data[offset + 1 + slot * 2] = id as u8;
                    data[offset + 2 + slot * 2] = count;
                }
            }
            data[offset + 1 + count * if keys { 1 } else { 2 }] = 0xff;
        }
    }
    Ok(())
}
