//! International R/B/Y save subset from SAV1 and SAV1Offsets.
use crate::{
    binary::InvalidData,
    pk1::Pk1,
    poke_list1::{pack, unpack},
};

const BOX_LENGTH: usize = 1122;
const PARTY_LENGTH: usize = 404;

pub struct Save1 {
    pub data: Vec<u8>,
    boxes: Vec<Vec<Pk1>>,
    party: Vec<Pk1>,
    daycare: Vec<u8>,
}

impl Save1 {
    pub fn is_valid(input: &[u8]) -> bool {
        if input.len() != 0x8000 {
            return false;
        }
        [0x2f2c, 0x30c0].into_iter().all(|offset| {
            let count = input[offset] as usize;
            count <= 20 && input[offset + count + 1] == 0xff
        })
    }

    pub fn read(input: &[u8]) -> Result<Self, InvalidData> {
        if !Self::is_valid(input) {
            return Err(InvalidData("Not an international Generation 1 save."));
        }
        let mut boxes = vec![vec![Pk1::default(); 20]; 12];
        let current = (input[0x284c] & 0x7f) as usize;
        if input[0x284c] & 0x80 != 0 {
            for (index, records) in boxes.iter_mut().enumerate() {
                if index != current {
                    *records = unpack(
                        &input[box_offset(index)..box_offset(index) + BOX_LENGTH],
                        20,
                        false,
                    );
                }
            }
        }
        if current < 12 {
            boxes[current] = unpack(&input[0x30c0..0x30c0 + BOX_LENGTH], 20, false);
        }
        Ok(Self {
            data: input.to_vec(),
            boxes,
            party: unpack(&input[0x2f2c..0x2f2c + PARTY_LENGTH], 6, true),
            daycare: input[0x2cf5..0x2d2c].to_vec(),
        })
    }

    pub fn pokemon(&self) -> impl Iterator<Item = &Pk1> {
        self.boxes
            .iter()
            .flatten()
            .chain(self.party.iter())
            .filter(|pk| pk.species() != 0)
    }
    pub fn flag(&self, offset: usize, index: usize) -> bool {
        self.data[offset + index / 8] & (1 << (index % 8)) != 0
    }
    pub fn event(&self, index: usize) -> bool {
        self.flag(0x29f3, index)
    }

    pub fn add_pokemon(&mut self, resource: &[u8]) -> Result<(), &'static str> {
        let mut pk = Pk1::from_resource(resource);
        let empty = self.boxes.iter().enumerate().find_map(|(index, list)| {
            list.iter()
                .position(|pk| pk.species() == 0)
                .map(|slot| (index, slot))
        });
        let (box_index, slot) =
            empty.ok_or("Error: All pokemon boxes are full. Free a box slot to fix the issue.")?;
        let name = |bytes: &[u8]| {
            bytes
                .iter()
                .copied()
                .take_while(|byte| *byte != 0x50)
                .collect::<Vec<_>>()
        };
        if self.pokemon().any(|other| {
            pk.species() == other.species()
                && pk.trainer_id() == other.trainer_id()
                && pk.dvs() == other.dvs()
                && name(&pk.trainer) == name(&other.trainer)
        }) {
            return Err("Error: You already have that pokemon.");
        }
        let personal = include_bytes!("../Resources/byte/personal/personal_y");
        let growth = personal[pk.species() as usize * 0x1c + 0x13];
        let exp =
            (u32::from(pk.body[14]) << 16) | (u32::from(pk.body[15]) << 8) | u32::from(pk.body[16]);
        pk.body[3] = crate::experience::level(exp, growth);
        for offset in [0x25a3, 0x25b6] {
            self.data[offset + (pk.species() as usize - 1) / 8] |= 1 << ((pk.species() - 1) % 8);
        }
        self.boxes[box_index][slot] = pk;
        Ok(())
    }

    pub fn write(mut self) -> Vec<u8> {
        let mut current = (self.data[0x284c] & 0x7f) as usize;
        let mut initialized = self.data[0x284c] & 0x80 != 0;
        if !initialized {
            initialized = self.boxes.iter().enumerate().any(|(index, records)| {
                index != current && records.iter().any(|pk| pk.species() != 0)
            });
            if initialized {
                self.data[0x284c] |= 0x80;
            } else {
                current = 0;
                self.data[0x284c] &= 0x80;
            }
        }
        for (index, records) in self.boxes.iter().enumerate() {
            let offset = box_offset(index);
            let written = pack(
                records,
                &mut self.data[offset..offset + BOX_LENGTH],
                false,
                initialized,
            );
            if index == current {
                if written {
                    self.data.copy_within(offset..offset + BOX_LENGTH, 0x30c0);
                } else {
                    pack(
                        records,
                        &mut self.data[0x30c0..0x30c0 + BOX_LENGTH],
                        false,
                        initialized,
                    );
                }
            }
        }
        pack(
            &self.party,
            &mut self.data[0x2f2c..0x2f2c + PARTY_LENGTH],
            true,
            true,
        );
        self.data[0x2cf5..0x2d2c].copy_from_slice(&self.daycare);
        self.data[0x3523] = !self.data[0x2598..0x3523]
            .iter()
            .fold(0_u8, |sum, byte| sum.wrapping_add(*byte));
        self.data
    }
}

fn box_offset(index: usize) -> usize {
    if index < 6 {
        0x4000 + index * BOX_LENGTH
    } else {
        0x6000 + (index - 6) * BOX_LENGTH
    }
}
