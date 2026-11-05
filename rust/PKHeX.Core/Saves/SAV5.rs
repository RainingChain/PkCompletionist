use crate::{binary::InvalidData, pk5::Pk5};

pub struct Save5 {
    pub data: Vec<u8>,
}
impl Save5 {
    pub fn box_offset(index: usize) -> usize {
        0x400 + index / 30 * 0x1000 + index % 30 * 136
    }
    pub fn boxes(&self) -> Result<Vec<Pk5>, InvalidData> {
        (0..720)
            .map(|index| {
                let offset = Self::box_offset(index);
                Pk5::read_encrypted(&self.data[offset..offset + 136])
            })
            .collect()
    }
    pub fn set_box(&mut self, index: usize, pk: &Pk5) {
        let offset = Self::box_offset(index);
        self.data[offset..offset + 136].copy_from_slice(&pk.write(false));
        self.set_dex(pk);
    }
    pub fn set_party(&mut self, index: usize, pk: &Pk5) {
        let offset = 0x18e08 + index * 220;
        self.data[offset..offset + 220].copy_from_slice(&pk.write(true));
        self.set_dex(pk);
    }
    fn set_dex(&mut self, pk: &Pk5) {
        let species = pk.species();
        if !(1..=649).contains(&species) || pk.data[0x3b] & 0x40 != 0 {
            return;
        }
        let gender = (pk.data[0x40] >> 1) & 1;
        let shiny = pk.shiny();
        let region = usize::from(gender) + usize::from(shiny) * 2;
        let bit = usize::from(species - 1);
        let dex = &mut self.data[0x21400..0x218dc];
        let set = |data: &mut [u8], offset: usize, bit: usize| {
            data[offset + bit / 8] |= 1 << (bit % 8);
        };
        if species == 327 && !(0..4).any(|r| dex[0x5c + r * 0x54 + bit / 8] & (1 << (bit % 8)) != 0)
        {
            dex[0x4d8..0x4dc].copy_from_slice(&pk.data[..4]);
        }
        set(dex, 8, bit);
        set(dex, 0x5c + region * 0x54, bit);
        if !(0..4).any(|r| dex[0x1ac + r * 0x54 + bit / 8] & (1 << (bit % 8)) != 0) {
            set(dex, 0x1ac + region * 0x54, bit);
        }
        let language = i32::from(pk.data[0x17]) - 1;
        let language = if language > 5 { language - 1 } else { language };
        if species <= 493 && (0..=5).contains(&language) {
            set(dex, 0x328, bit * 7 + language as usize);
        }
        if let Some((index, count)) = crate::zukan5::form_index(species) {
            // Preserve Zukan5.SetDex's base-form seen flag behavior.
            set(dex, 0x2fc + usize::from(shiny) * 11, index);
            let displayed = (index..index + count)
                .any(|i| (0..2).any(|r| dex[0x312 + r * 11 + i / 8] & (1 << (i % 8)) != 0));
            if !displayed && usize::from(pk.form()) < count {
                set(
                    dex,
                    0x312 + usize::from(shiny) * 11,
                    index + usize::from(pk.form()),
                );
            }
        }
    }
    pub fn pokemon(&self) -> Result<Vec<Pk5>, InvalidData> {
        let mut locations = Vec::new();
        for box_index in 0..24 {
            for slot in 0..30 {
                locations.push((0x400 + box_index * 0x1000 + slot * 136, 136));
            }
        }
        for slot in 0..usize::from(self.data[0x18e04]).min(6) {
            locations.push((0x18e08 + slot * 220, 220));
        }
        let normal_count = locations.len();
        locations.extend([(0x20400, 220), (0x1fa04, 220), (0x1d308, 220)]);
        for slot in 0..6 {
            locations.push((0x20900 + slot * 136, 136));
        }
        let mut pokemon = Vec::new();
        for (index, (offset, size)) in locations.into_iter().enumerate() {
            let input = &self.data[offset..offset + size];
            let pk = if index < normal_count {
                Pk5::read_encrypted(input)?
            } else {
                Pk5::read(input)?
            };
            if pk.species() != 0 {
                pokemon.push(pk);
            }
        }
        Ok(pokemon)
    }
}
