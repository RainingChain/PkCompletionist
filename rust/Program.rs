use pkcompletionist_core::CommandResult;
use wasm_bindgen::prelude::*;

#[wasm_bindgen]
extern "C" {
    #[wasm_bindgen(js_namespace = Math, js_name = random)]
    fn random_number() -> f64;
}

#[wasm_bindgen]
#[derive(Default)]
pub struct Command {
    result: CommandResult,
}

#[wasm_bindgen]
impl Command {
    #[wasm_bindgen(constructor)]
    pub fn new() -> Self {
        Self::default()
    }

    pub fn validate(&mut self, bytes: &[u8], hint: &str, objective: i32) -> bool {
        self.result = pkcompletionist_core::validate(bytes, hint, objective);
        self.result.success
    }

    pub fn event(&mut self, name: &str, bytes: &[u8], hint: &str, second: Option<Vec<u8>>) -> bool {
        self.result =
            pkcompletionist_core::event(name, bytes, hint, second.as_deref(), &mut || {
                (random_number() * 256.0) as u8
            });
        self.result.success
    }

    pub fn result_json(&self) -> String {
        self.result.json().to_string()
    }
    pub fn describe(&mut self, bytes: &[u8], hint: &str) -> bool {
        self.result = pkcompletionist_core::describe(bytes, hint);
        self.result.success
    }
    pub fn sort_pokemon(&mut self, bytes: &[u8], hint: &str) -> bool {
        self.result = pkcompletionist_core::sort_pokemon(bytes, hint);
        self.result.success
    }
    pub fn version(&self) -> String {
        pkcompletionist_core::version().to_string()
    }
    pub fn analyze_flags(&mut self, before: &[u8], after: &[u8], ranges: &str, hint: &str) -> bool {
        self.result = pkcompletionist_core::analyze_flags(before, after, ranges, hint);
        self.result.success
    }
    pub fn save_a(&self) -> Vec<u8> {
        self.result.save_a.clone()
    }
    pub fn save_b(&self) -> Vec<u8> {
        self.result.save_b.clone()
    }
}
