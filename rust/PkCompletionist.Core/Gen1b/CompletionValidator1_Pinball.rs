use crate::{category, CommandResult};

pub fn validate(data: &[u8], result: &mut CommandResult) {
    category(result, "pokemon", (1..=151).map(|id| (id.to_string(), data[0x10c + id - 1] == 3)));
}