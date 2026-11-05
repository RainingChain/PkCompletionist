use crate::{
    binary::{u16_le, InvalidData},
    blocks5::BLOCKS,
    checksums::crc16_ccitt,
    gen5::Save5,
};

pub fn is_valid(data: &[u8]) -> bool {
    if data.len() != 0x80000 {
        return false;
    }
    // SaveUtil checks BW first, then B2W2.
    let valid_footer = |start: usize, length: usize| {
        u16_le(data, start + length + 14).unwrap() == crc16_ccitt(&data[start..start + length])
    };
    !valid_footer(0x23f00, 0x8c) && valid_footer(0x25f00, 0x94)
}
impl Save5 {
    pub fn read(input: &[u8]) -> Result<Self, InvalidData> {
        if !is_valid(input) {
            return Err(InvalidData("Invalid Black 2 / White 2 save."));
        }
        Ok(Self {
            data: input.to_vec(),
        })
    }
    pub fn write(&self) -> Vec<u8> {
        let mut data = self.data.clone();
        for &(offset, length, checksum, mirror) in BLOCKS {
            let crc = crc16_ccitt(&data[offset..offset + length]).to_le_bytes();
            data[checksum..checksum + 2].copy_from_slice(&crc);
            data[mirror..mirror + 2].copy_from_slice(&crc);
        }
        data
    }
    pub fn dex(&self) -> crate::zukan5::Zukan5<'_> {
        crate::zukan5::Zukan5(&self.data[0x21400..0x218dc])
    }
    pub fn event(&self, id: usize) -> bool {
        self.data[0x1ff00 + 0x35e + id / 8] & (1 << (id % 8)) != 0
    }
    pub fn records(&self) -> crate::record5::Record5 {
        crate::record5::Record5::read(&self.data[0x1da00..0x1dbe0])
    }
    pub fn pwt(&self, id: usize) -> u16 {
        u16_le(&self.data, 0x2375c + id * 2).unwrap()
    }
}
