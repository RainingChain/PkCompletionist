//! Selected PK3 fields and byte-preserving encryption. No move interpretation.
use crate::{
    binary::{u16_le, u32_le, InvalidData},
    poke_crypto,
};

#[derive(Clone, Debug)]
pub struct Pk3 {
    pub data: [u8; 100],
}

impl Pk3 {
    pub fn read(input: &[u8]) -> Result<Self, InvalidData> {
        if input.len() < 80 {
            return Err(InvalidData("Truncated PK3."));
        }
        let mut data = [0; 100];
        let length = input.len().min(100);
        data[..length].copy_from_slice(&input[..length]);
        if u16_le(&data, 0x1c)? != poke_crypto::checksum3(&data) {
            poke_crypto::decrypt3(&mut data);
        }
        Ok(Self { data })
    }
    pub fn pid(&self) -> u32 {
        u32_le(&self.data, 0).unwrap()
    }
    pub fn trainer_id(&self) -> u16 {
        u16_le(&self.data, 4).unwrap()
    }
    pub fn species(&self) -> u16 {
        crate::species_converter::national3(u16_le(&self.data, 0x20).unwrap())
    }
    pub fn held_item(&self) -> u16 {
        u16_le(&self.data, 0x22).unwrap()
    }
    pub fn ivs(&self) -> u32 {
        u32_le(&self.data, 0x48).unwrap() & 0x3fffffff
    }
    pub fn is_egg(&self) -> bool {
        self.data[0x4b] & 0x40 != 0
    }
    pub fn is_shiny(&self) -> bool {
        (self.trainer_id()
            ^ u16_le(&self.data, 6).unwrap()
            ^ self.pid() as u16
            ^ (self.pid() >> 16) as u16)
            < 8
    }
    pub fn form(&self) -> u8 {
        if self.species() != 201 {
            return 0;
        }
        let pid = self.pid();
        ((((pid & 0x3000000) >> 18) | ((pid & 0x30000) >> 12) | ((pid & 0x300) >> 6) | (pid & 3))
            % 28) as u8
    }
    pub fn ribbons(&self) -> u32 {
        u32_le(&self.data, 0x4c).unwrap()
    }
    pub fn write(&self, party: bool) -> Vec<u8> {
        let mut data = self.data;
        let checksum = poke_crypto::checksum3(&data);
        data[0x1c..0x1e].copy_from_slice(&checksum.to_le_bytes());
        poke_crypto::encrypt3(&mut data);
        data[..if party { 100 } else { 80 }].to_vec()
    }
}
