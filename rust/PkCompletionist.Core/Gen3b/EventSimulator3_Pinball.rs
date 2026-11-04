pub fn event(name: &str, data: &mut [u8]) -> Option<String> {
    // Enum.TryParse also accepts a numeric value.
    if name.trim() != "TransmitDexWithEReaderMons" && name.trim() != "0" { return Some("Invalid event name.".into()); }
    for id in 202..=205 { let value = &mut data[4 + id - 1]; *value = (*value).max(2); }
    None
}
