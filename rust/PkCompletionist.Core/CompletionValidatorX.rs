use crate::CommandResult;

/// C# Dictionary insertion order, including overwrites and removals.
#[derive(Default)]
pub(crate) struct Checklist { entries: Vec<(String, bool)> }

impl Checklist {
    pub fn set(&mut self, key: impl ToString, value: bool) {
        let key = key.to_string();
        if let Some(entry) = self.entries.iter_mut().find(|(existing, _)| existing == &key) { entry.1 = value; }
        else { self.entries.push((key, value)); }
    }
    pub fn get(&self, key: &str) -> bool { self.entries.iter().any(|(existing, value)| existing == key && *value) }
    pub fn remove(&mut self, key: &str) { self.entries.retain(|(existing, _)| existing != key); }
    pub fn emit(self, result: &mut CommandResult, name: &str) { category(result, name, self.entries); }
}
pub(crate) fn category(result: &mut CommandResult, name: &str, entries: impl IntoIterator<Item = (String, bool)>) {
    let mut obtained = Vec::new();
    let mut missing = Vec::new();
    for (key, owned) in entries { if owned { obtained.push(key); } else { missing.push(key); } }
    result.obtained_status.extend([name.to_string(), obtained.join(","), missing.join(",")]);
}

/// Dictionary assignment replaces a value without moving its insertion position.
pub(crate) fn ordered_category(result: &mut CommandResult, name: &str, entries: &[(&str, bool)]) {
    let mut values: Vec<(String, bool)> = Vec::new();
    for (key, owned) in entries {
        if let Some(entry) = values.iter_mut().find(|(existing, _)| existing == key) { entry.1 = *owned; }
        else { values.push((key.to_string(), *owned)); }
    }
    category(result, name, values);
}
