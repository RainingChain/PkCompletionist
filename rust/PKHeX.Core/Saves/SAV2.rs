//! International Crystal save recognition and serialization subset.
//! Derived from PKHeX SAV2, SAV2Offsets, PokeList2, SaveHandlerFooterRTC and
//! SaveFileMetadata at the revision recorded in /PkHeX.Core.version.txt.
//! Legality and move interpretation are intentionally outside this crate.
use crate::binary::InvalidData;
use crate::poke_list2::normalize_list;

const BOX_LENGTH: usize = 1102;
const PARTY_LENGTH: usize = 428;

pub struct CrystalSave {
    pub data: Vec<u8>,
    footer: Vec<u8>,
    boxes: Vec<Vec<u8>>,
    party: Vec<u8>,
    daycare: Vec<(usize, Vec<u8>)>,
}

fn has_list(data: &[u8], offset: usize) -> bool {
    let count = data[offset] as usize;
    count <= 20 && data[offset + 1 + count] == 0xff
}

impl CrystalSave {
    pub fn trainer_id(&self) -> u16 {
        u16::from_be_bytes([self.data[0x2009], self.data[0x200a]])
    }
    pub fn set_event(&mut self, index: usize, value: bool) {
        let mask = 1 << (index % 8);
        let byte = &mut self.data[0x2600 + index / 8];
        if value {
            *byte |= mask;
        } else {
            *byte &= !mask;
        }
    }

    pub fn add_pokemon(&mut self, resource: &[u8]) -> Result<(), &'static str> {
        let pk = crate::pk2::Pk2::read(&resource[3..51]);
        let empty = self.boxes.iter().enumerate().find_map(|(box_index, list)| {
            (0..20)
                .find(|slot| list[22 + slot * 32] == 0)
                .map(|slot| (box_index, slot))
        });
        let (box_index, slot) =
            empty.ok_or("Error: All pokemon boxes are full. Free a box slot to fix the issue.")?;
        let equal_body = |body: &[u8]| {
            let other = crate::pk2::Pk2::read(body);
            pk.species == other.species && pk.trainer_id == other.trainer_id && pk.dvs == other.dvs
        };
        let name = |bytes: &[u8]| {
            bytes
                .iter()
                .copied()
                .take_while(|byte| *byte != 0x50)
                .collect::<Vec<_>>()
        };
        let ot = name(&resource[51..62]);
        for list in &self.boxes {
            for index in 0..20 {
                if equal_body(&list[22 + index * 32..22 + (index + 1) * 32])
                    && name(&list[662 + index * 11..662 + (index + 1) * 11]) == ot
                {
                    return Err("Error: You already have that pokemon.");
                }
            }
        }
        for index in 0..6 {
            if equal_body(&self.party[8 + index * 48..8 + (index + 1) * 48])
                && name(&self.party[296 + index * 11..296 + (index + 1) * 11]) == ot
            {
                return Err("Error: You already have that pokemon.");
            }
        }
        if equal_body(&self.data[0x2af3 + 22..0x2af3 + 54])
            && name(&self.data[0x2af3 + 11..0x2af3 + 22]) == ot
        {
            return Err("Error: You already have that pokemon.");
        }
        let list = &mut self.boxes[box_index];
        list[slot + 1] = pk.species as u8;
        list[22 + slot * 32..22 + (slot + 1) * 32].copy_from_slice(&resource[3..35]);
        list[662 + slot * 11..662 + (slot + 1) * 11].copy_from_slice(&resource[51..62]);
        list[882 + slot * 11..882 + (slot + 1) * 11].copy_from_slice(&resource[62..73]);
        list[0] = list[1..21]
            .iter()
            .filter(|mark| **mark != 0 && **mark != 0xff)
            .count() as u8;
        if (1..=251).contains(&pk.species) && resource[1] != 0xfd {
            for offset in [0x2a27, 0x2a47] {
                self.data[offset + (pk.species as usize - 1) / 8] |= 1 << ((pk.species - 1) % 8);
            }
        }
        Ok(())
    }

    pub fn pokemon(&self) -> Vec<crate::pk2::Pk2> {
        let mut pokemon = Vec::new();
        for list in &self.boxes {
            for index in 0..20 {
                pokemon.push(crate::pk2::Pk2::read(
                    &list[22 + index * 32..22 + (index + 1) * 32],
                ));
            }
        }
        for index in 0..6 {
            pokemon.push(crate::pk2::Pk2::read(
                &self.party[8 + index * 48..8 + (index + 1) * 48],
            ));
        }
        // GetAllPKM includes the daycare egg, but not either daycare parent.
        pokemon.push(crate::pk2::Pk2::read(&self.data[0x2af3 + 22..0x2af3 + 54]));
        pokemon.retain(|pk| pk.species != 0);
        pokemon
    }

    pub fn event(&self, index: usize) -> bool {
        self.data[0x2600 + index / 8] & (1 << (index % 8)) != 0
    }
    pub fn work(&self, index: usize) -> u8 {
        self.data[0x2500 + index]
    }

    pub fn is_valid(input: &[u8]) -> bool {
        let footer = input.len() & 0x3f;
        let length = input.len() - footer;
        let has_footer = footer == 7 || (0xc..=0x30).contains(&footer) && footer % 2 == 0;
        // Preserve SaveUtil's GS-before-Crystal recognition precedence.
        length == 0x8000
            && (footer == 0 || has_footer)
            && !(has_list(input, 0x288a) && has_list(input, 0x2d6c))
            && has_list(input, 0x2865)
            && has_list(input, 0x2d10)
    }

    pub fn read(input: &[u8]) -> Result<Self, InvalidData> {
        if !Self::is_valid(input) {
            return Err(InvalidData("Not an international Crystal save."));
        }
        let data = input[..0x8000].to_vec();
        let boxes = (0..14)
            .map(|index| {
                let start = box_offset(index);
                normalize_list(&data[start..start + BOX_LENGTH], 20, false)
            })
            .collect();
        let party = normalize_list(&data[0x2865..0x2865 + PARTY_LENGTH], 6, true);
        let daycare = [0x2a84, 0x2abd, 0x2af3]
            .into_iter()
            .map(|offset| (offset, data[offset..offset + 54].to_vec()))
            .collect();
        Ok(Self {
            data,
            footer: input[0x8000..].to_vec(),
            boxes,
            party,
            daycare,
        })
    }

    pub fn write(mut self) -> Vec<u8> {
        for (index, bytes) in self.boxes.iter().enumerate() {
            let start = box_offset(index);
            let initialized = bytes.iter().any(|byte| *byte != 0);
            self.data[start..start + BOX_LENGTH].copy_from_slice(bytes);
            if initialized && index == self.data[0x2700] as usize {
                self.data[0x2d10..0x2d10 + BOX_LENGTH].copy_from_slice(bytes);
            }
        }
        self.data[0x2865..0x2865 + PARTY_LENGTH].copy_from_slice(&self.party);
        for (offset, bytes) in self.daycare {
            self.data[offset..offset + 54].copy_from_slice(&bytes);
        }
        let checksum = self.data[0x2009..=0x2b82]
            .iter()
            .fold(0_u16, |sum, byte| sum.wrapping_add(u16::from(*byte)));
        for offset in [0x2d0d, 0x1f0d] {
            self.data[offset..offset + 2].copy_from_slice(&checksum.to_le_bytes());
        }
        self.data.copy_within(0x2009..0x2b83, 0x1209);
        self.data.extend(self.footer);
        self.data
    }
}

fn box_offset(index: usize) -> usize {
    if index < 7 {
        0x4000 + index * 0x450
    } else {
        0x6000 + (index - 7) * 0x450
    }
}
