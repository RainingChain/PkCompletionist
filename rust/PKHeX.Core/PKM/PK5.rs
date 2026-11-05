use crate::{
    binary::{u16_le, u32_le, InvalidData},
    poke_crypto::decrypt45,
};

/// Selected PK5 fields. Move bytes remain opaque in the original record.
pub struct Pk5 {
    pub data: Vec<u8>,
}
impl Pk5 {
    pub fn read(input: &[u8]) -> Result<Self, InvalidData> {
        if !matches!(input.len(), 136 | 220) {
            return Err(InvalidData("Invalid PK5 record length."));
        }
        if u32_le(input, 0x64).unwrap() != 0 {
            Self::read_encrypted(input)
        } else {
            Ok(Self {
                data: input.to_vec(),
            })
        }
    }
    pub fn read_encrypted(input: &[u8]) -> Result<Self, InvalidData> {
        if !matches!(input.len(), 136 | 220) {
            return Err(InvalidData("Invalid PK5 record length."));
        }
        let mut data = input.to_vec();
        decrypt45(&mut data);
        Ok(Self { data })
    }
    pub fn species(&self) -> u16 {
        u16_le(&self.data, 8).unwrap()
    }
    pub fn held_item(&self) -> u16 {
        u16_le(&self.data, 10).unwrap()
    }
    pub fn trainer_id(&self) -> u16 {
        u16_le(&self.data, 12).unwrap()
    }
    pub fn form(&self) -> u8 {
        self.data[0x40] >> 3
    }
    pub fn original_trainer(&self) -> String {
        crate::string_converter5::decode(&self.data[0x68..0x78])
    }
    pub fn shiny(&self) -> bool {
        let pid = u32_le(&self.data, 0).unwrap();
        (self.trainer_id() ^ u16_le(&self.data, 14).unwrap() ^ pid as u16 ^ (pid >> 16) as u16) < 8
    }
    pub fn ribbon(&self, offset: usize, bit: u8) -> bool {
        self.data[offset] & (1 << bit) != 0
    }
    pub fn write(&self, party: bool) -> Vec<u8> {
        let mut data = self.data.clone();
        data.resize(if party { 220 } else { 136 }, 0);
        let checksum = data[8..136].chunks_exact(2).fold(0_u16, |sum, word| {
            sum.wrapping_add(u16::from_le_bytes([word[0], word[1]]))
        });
        data[6..8].copy_from_slice(&checksum.to_le_bytes());
        crate::poke_crypto::encrypt45(&mut data);
        data
    }
    pub fn ivs(&self) -> u32 {
        u32_le(&self.data, 0x38).unwrap() & 0x3fff_ffff
    }
}
