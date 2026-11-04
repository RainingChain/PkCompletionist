//! Emerald-specific recognition, using the same small-block discriminator as SaveUtil.
use crate::{binary::u32_le, gen3::sector_zero};

pub fn is_valid(input: &[u8]) -> bool {
    let footer = input.len() & 0x3f;
    let size = input.len() - footer;
    if ![0x10000, 0x20000].contains(&size)
        || !(footer == 0 || footer == 7 || (0xc..=0x30).contains(&footer) && footer % 2 == 0)
    {
        return false;
    }
    let zero = sector_zero(input, 0).or_else(|| {
        if size == 0x20000 {
            sector_zero(input, 1)
        } else {
            None
        }
    });
    zero.is_some_and(|offset| {
        !matches!(u32_le(input, offset + 0xac), Ok(0 | 1))
            && input[offset + 0x890..offset + 0xf2c]
                .iter()
                .any(|byte| *byte != 0)
    })
}
