use crate::{binary::u16_le, gen5::Save5, item_storage5, item_storage5b2w2};
pub fn items(save: &Save5) -> Vec<u16> {
    let mut items = Vec::new();
    for (offset, count) in [
        (0, item_storage5::GENERAL.len()),
        (0x4d8, item_storage5b2w2::KEY.len()),
        (0x624, item_storage5::MACHINE.len()),
        (0x7d8, item_storage5::MEDICINE.len()),
        (0x898, item_storage5::BERRY.len()),
    ] {
        for index in 0..count {
            let item = u16_le(&save.data, 0x18400 + offset + index * 4).unwrap();
            if item != 0 {
                items.push(item);
            }
        }
    }
    items
}
