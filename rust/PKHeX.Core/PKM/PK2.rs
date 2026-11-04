//! Fields consumed by completion checks; uninterpreted record bytes remain in SAV2.
#[derive(Debug, Clone)]
pub struct Pk2 {
    pub species: u16,
    pub held_item: u16,
    pub trainer_id: u16,
    pub dvs: u16,
    pub pokerus: u8,
}

impl Pk2 {
    pub fn read(data: &[u8]) -> Self {
        Self { species: u16::from(data[0]), held_item: u16::from(data[1]), trainer_id: u16::from_be_bytes([data[6],data[7]]), dvs: u16::from_be_bytes([data[0x15],data[0x16]]), pokerus: data[0x1c] }
    }

    pub fn is_shiny(&self) -> bool { self.dvs & 0x2fff == 0x2aaa }

    pub fn form(&self) -> u8 {
        if self.species != 201 { return 0; }
        let atk = self.dvs >> 12;
        let def = (self.dvs >> 8) & 15;
        let spe = (self.dvs >> 4) & 15;
        let spc = self.dvs & 15;
        ((((atk & 6) << 5) | ((def & 6) << 3) | ((spe & 6) << 1) | ((spc & 6) >> 1)) / 10) as u8
    }
}
