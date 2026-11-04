//! SAV3 sector storage used by Emerald. Logical buffers preserve unrelated bytes.
use crate::{
    binary::{u16_le, u32_le, InvalidData},
    pk3::Pk3,
};

pub struct Save3 {
    pub data: Vec<u8>,
    pub small: Vec<u8>,
    pub large: Vec<u8>,
    pub storage: Vec<u8>,
    active: usize,
    footer: Vec<u8>,
}

pub fn sector_zero(data: &[u8], slot: usize) -> Option<usize> {
    let mut mask = 0_u16;
    let mut zero = 0;
    for offset in (slot * 0xe000..(slot + 1) * 0xe000).step_by(0x1000) {
        let id = u16_le(data, offset + 0xff4).ok()?;
        if id >= 14 {
            return None;
        }
        mask |= 1 << id;
        if id == 0 {
            zero = offset;
        }
    }
    (mask == 0x3fff).then_some(zero)
}

impl Save3 {
    pub fn read(input: &[u8]) -> Result<Self, InvalidData> {
        let size = if input.len() >= 0x20000 {
            0x20000
        } else {
            0x10000
        };
        if input.len() < size {
            return Err(InvalidData("Truncated Generation 3 save."));
        }
        let first = sector_zero(input, 0);
        let second = if size == 0x20000 {
            sector_zero(input, 1)
        } else {
            None
        };
        let active = match (first, second) {
            (None, None) => return Err(InvalidData("Missing Generation 3 save sectors.")),
            (None, Some(_)) => 1,
            (Some(_), None) => 0,
            (Some(a), Some(b)) => {
                let a = u32_le(input, a + 0xffc)?;
                let b = u32_le(input, b + 0xffc)?;
                usize::from(if a == u32::MAX && b != u32::MAX - 1 {
                    true
                } else if b == u32::MAX && a != u32::MAX - 1 {
                    false
                } else {
                    b > a
                })
            }
        };
        let mut save = Self {
            data: input[..size].to_vec(),
            small: vec![0; 0xf80],
            large: vec![0; 0x3e00],
            storage: vec![0; 0x8b80],
            active,
            footer: input[size..].to_vec(),
        };
        for offset in (active * 0xe000..(active + 1) * 0xe000).step_by(0x1000) {
            let id = u16_le(input, offset + 0xff4)? as usize;
            save.chunk_mut(id)
                .copy_from_slice(&input[offset..offset + 0xf80]);
        }
        Ok(save)
    }

    fn chunk_mut(&mut self, id: usize) -> &mut [u8] {
        match id {
            0 => &mut self.small,
            1..=4 => &mut self.large[(id - 1) * 0xf80..id * 0xf80],
            _ => &mut self.storage[(id - 5) * 0xf80..(id - 4) * 0xf80],
        }
    }
    fn chunk(&self, id: usize) -> &[u8] {
        match id {
            0 => &self.small,
            1..=4 => &self.large[(id - 1) * 0xf80..id * 0xf80],
            _ => &self.storage[(id - 5) * 0xf80..(id - 4) * 0xf80],
        }
    }
    pub fn event(&self, id: usize) -> bool {
        self.large[0x1270 + id / 8] & (1 << (id % 8)) != 0
    }
    pub fn set_event(&mut self, id: usize, value: bool) {
        let byte = &mut self.large[0x1270 + id / 8];
        let mask = 1 << (id % 8);
        if value {
            *byte |= mask;
        } else {
            *byte &= !mask;
        }
    }
    pub fn work(&self, id: usize) -> u16 {
        u16_le(&self.large, 0x139c + id * 2).unwrap()
    }
    pub fn record(&self, id: usize) -> u32 {
        u32_le(&self.large, 0x159c + id * 4).unwrap() ^ u32_le(&self.small, 0xac).unwrap()
    }
    pub fn caught(&self, id: u16) -> bool {
        self.small[0x28 + (id as usize - 1) / 8] & (1 << ((id - 1) % 8)) != 0
    }
    pub fn seen(&self, id: u16) -> bool {
        self.small[0x5c + (id as usize - 1) / 8] & (1 << ((id - 1) % 8)) != 0
    }
    pub fn boxes(&self) -> Result<Vec<Pk3>, InvalidData> {
        (0..420)
            .map(|index| Pk3::read(&self.storage[4 + index * 80..4 + (index + 1) * 80]))
            .collect()
    }
    pub fn party(&self) -> Result<Vec<Pk3>, InvalidData> {
        (0..usize::from(self.large[0x234]).min(6))
            .map(|index| Pk3::read(&self.large[0x238 + index * 100..0x238 + (index + 1) * 100]))
            .collect()
    }
    pub fn pokemon(&self) -> Result<Vec<Pk3>, InvalidData> {
        let mut all = self.boxes()?;
        all.extend(self.party()?);
        all.retain(|pk| pk.species() != 0);
        Ok(all)
    }
    pub fn set_box(&mut self, index: usize, pk: &Pk3) {
        self.set_dex(pk);
        self.storage[4 + index * 80..4 + (index + 1) * 80].copy_from_slice(&pk.write(false));
    }
    pub fn set_party(&mut self, index: usize, pk: &Pk3) {
        self.set_dex(pk);
        let mut pk = pk.clone();
        if !(121..=132).contains(&pk.held_item()) {
            pk.data[0x55] = 0xff;
        }
        self.large[0x238 + index * 100..0x238 + (index + 1) * 100].copy_from_slice(&pk.write(true));
    }
    fn set_dex(&mut self, pk: &Pk3) {
        let id = pk.species();
        if id == 0 || id > 386 || pk.is_egg() {
            return;
        }
        if !self.seen(id) {
            let offset = match id {
                201 => Some(0x1c),
                327 => Some(0x20),
                _ => None,
            };
            if let Some(offset) = offset {
                self.small[offset..offset + 4].copy_from_slice(&pk.pid().to_le_bytes());
            }
        }
        let index = (id as usize - 1) / 8;
        let bit = 1 << ((id - 1) % 8);
        for offset in [0x28, 0x5c] {
            self.small[offset + index] |= bit;
        }
        for offset in [0x988, 0x3b24] {
            self.large[offset + index] |= bit;
        }
    }

    pub fn write(mut self) -> Vec<u8> {
        for offset in (self.active * 0xe000..(self.active + 1) * 0xe000).step_by(0x1000) {
            let id = u16_le(&self.data, offset + 0xff4).unwrap() as usize;
            let bytes = self.chunk(id).to_vec();
            self.data[offset..offset + 0xf80].copy_from_slice(&bytes);
            let checksum = checksum32(&bytes);
            self.data[offset + 0xff6..offset + 0xff8].copy_from_slice(&checksum.to_le_bytes());
        }
        if self.data.len() == 0x20000 {
            for offset in (0x1c000..0x20000).step_by(0x1000) {
                if self.data[offset..offset + 0x1000]
                    .iter()
                    .all(|byte| *byte == 0 || *byte == 0xff)
                {
                    continue;
                }
                let checksum = checksum32(&self.data[offset..offset + 0xf80]);
                self.data[offset + 0xff4..offset + 0xff6].copy_from_slice(&checksum.to_le_bytes());
            }
        }
        self.data.extend(self.footer);
        self.data
    }
}

fn checksum32(bytes: &[u8]) -> u16 {
    let sum = bytes.chunks_exact(4).fold(0_u32, |sum, word| {
        sum.wrapping_add(u32::from_le_bytes(word.try_into().unwrap()))
    });
    sum.wrapping_add(sum >> 16) as u16
}
