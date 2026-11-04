use crate::{category, CommandResult};

pub fn validate(data: &[u8], result: &mut CommandResult) {
    category(result, "pokemon", (1..=205).map(|id| (id.to_string(), data[4 + id - 1] == 4)));
}