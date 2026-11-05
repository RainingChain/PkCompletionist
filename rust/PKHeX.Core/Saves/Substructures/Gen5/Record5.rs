use crate::{
    binary::{u16_le, u32_le},
    poke_crypto::crypt_array,
};
pub struct Record5(Vec<u8>);
impl Record5 {
    pub fn read(data: &[u8]) -> Self {
        let mut records = data[4..data.len() - 4].to_vec();
        crypt_array(&mut records, u32_le(data, data.len() - 4).unwrap());
        Self(records)
    }
    pub fn record32(&self, index: usize) -> u32 {
        u32_le(&self.0, index * 4).unwrap()
    }
    pub fn record16(&self, index: usize) -> u16 {
        u16_le(&self.0, 68 * 4 + index * 2).unwrap()
    }
}
