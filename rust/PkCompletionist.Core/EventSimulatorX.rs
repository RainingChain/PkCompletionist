use std::borrow::Cow;

/// Enum.TryParse-compatible names/numbers, including comma-separated name combinations.
pub(crate) fn parse_event(name: &str, variants: &[&str]) -> Option<i32> {
    let name = name.trim();
    if let Ok(value) = name.parse::<i32>() {
        return Some(value);
    }
    let mut value = 0;
    for part in name.split(',') {
        value |= variants
            .iter()
            .position(|variant| *variant == part.trim())? as i32;
    }
    Some(value)
}

pub(crate) fn event_name<'a>(name: &'a str, variants: &[&'static str]) -> Cow<'a, str> {
    match parse_event(name, variants) {
        Some(value) => match usize::try_from(value)
            .ok()
            .and_then(|index| variants.get(index))
        {
            Some(name) => Cow::Borrowed(name),
            None => Cow::Owned(value.to_string()),
        },
        None => Cow::Borrowed(name.trim()),
    }
}

pub(crate) fn unsupported() -> Option<String> {
    Some("Unsupported generation.".to_string())
}
