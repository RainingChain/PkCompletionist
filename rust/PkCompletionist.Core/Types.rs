/// Numeric values used by the JavaScript API and completion fixtures.
#[repr(i32)]
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum Objective {
    Normal = 0,
    Living = 1,
    Other = 2,
}
