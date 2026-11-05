pub struct Zukan5<'a>(pub &'a [u8]);
pub fn form_index(species: u16) -> Option<(usize, usize)> {
    Some(match species {
        201 => (0, 28),
        386 => (28, 4),
        492 => (32, 2),
        487 => (34, 2),
        479 => (36, 6),
        422 => (42, 2),
        423 => (44, 2),
        412 => (46, 3),
        413 => (49, 3),
        351 => (52, 4),
        421 => (56, 2),
        585 => (58, 4),
        586 => (62, 4),
        648 => (66, 2),
        555 => (68, 2),
        550 => (70, 2),
        646 => (72, 3),
        647 => (75, 2),
        642 => (77, 2),
        641 => (79, 2),
        645 => (81, 2),
        _ => return None,
    })
}
impl Zukan5<'_> {
    fn flag(&self, offset: usize, bit: usize) -> bool {
        self.0[offset + bit / 8] & (1 << (bit % 8)) != 0
    }
    pub fn caught(&self, species: u16) -> bool {
        (1..=649).contains(&species) && self.flag(8, usize::from(species - 1))
    }
    pub fn seen_region(&self, species: u16, gender: u8, shiny: bool) -> bool {
        (1..=649).contains(&species)
            && self.flag(
                0x5c + (usize::from(shiny) * 2 + usize::from(gender & 1)) * 0x54,
                usize::from(species - 1),
            )
    }
    pub fn seen(&self, species: u16) -> bool {
        (0..4).any(|region| self.seen_region(species, region & 1, region >= 2))
    }
    pub fn form_seen(&self, index: i32, shiny: bool) -> bool {
        (0..88).contains(&index) && self.flag(0x2fc + usize::from(shiny) * 11, index as usize)
    }
    pub fn language(&self, species: u16, language: usize) -> bool {
        (1..=493).contains(&species) && self.flag(0x328, usize::from(species - 1) * 7 + language)
    }
}
