#![allow(non_snake_case, non_camel_case_types)]
use crate::{category, CommandResult};
struct CompletionValidator3_Pinball<'a> {
    sav: &'a [u8],
}
pub fn validate(data: &[u8], result: &mut CommandResult) {
    CompletionValidator3_Pinball { sav: data }.GenerateAll(result);
}
impl CompletionValidator3_Pinball<'_> {
    fn GenerateAll(&self, result: &mut CommandResult) {
        self.Generate_pokemon(result);
    }
    fn Generate_pokemon(&self, result: &mut CommandResult) {
        let data = self.sav;

        category(
            result,
            "pokemon",
            (1..=205).map(|id| (id.to_string(), data[4 + id - 1] == 4)),
        );
    }
}
