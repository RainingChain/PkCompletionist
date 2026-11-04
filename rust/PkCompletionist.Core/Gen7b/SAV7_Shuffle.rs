pub fn is_valid(data: &[u8]) -> bool { data.len() == 74807 && data.starts_with(&[9, 0, 0, 0, 0x40]) }

pub(crate) fn bit(data: &[u8], offset: usize, index: usize) -> bool { data[offset + index / 8] & (1 << (index % 8)) != 0 }
pub(crate) fn has_mon(data: &[u8], id: usize) -> bool { [0xe6, 0x546, 0x5e6].iter().all(|offset| bit(data, *offset, id + 5)) }
pub(crate) fn mega_stone(data: &[u8], id: usize, stone: usize) -> bool { (data[0x406 + (id + 2) / 4] >> ((5 + id * 2) % 8)) & (1 << stone) != 0 }

