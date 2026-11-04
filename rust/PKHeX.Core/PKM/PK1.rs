//! Raw Generation 1 entity storage. Move bytes are preserved without interpretation.
#[derive(Clone)]
pub struct Pk1 {
    pub marker: u8,
    pub body: [u8; 44],
    pub trainer: [u8; 11],
    pub nickname: [u8; 11],
}

impl Default for Pk1 {
    fn default() -> Self {
        Self {
            marker: 0,
            body: [0; 44],
            trainer: [0; 11],
            nickname: [0; 11],
        }
    }
}

impl Pk1 {
    pub fn species(&self) -> u8 {
        crate::species_converter::national1(self.body[0])
    }
    pub fn trainer_id(&self) -> u16 {
        u16::from_be_bytes([self.body[12], self.body[13]])
    }
    pub fn dvs(&self) -> u16 {
        u16::from_be_bytes([self.body[27], self.body[28]])
    }
    pub fn from_resource(bytes: &[u8]) -> Self {
        let mut pk = Self {
            marker: bytes[1],
            ..Self::default()
        };
        pk.body.copy_from_slice(&bytes[3..47]);
        pk.trainer.copy_from_slice(&bytes[47..58]);
        pk.nickname.copy_from_slice(&bytes[58..69]);
        pk
    }
}
