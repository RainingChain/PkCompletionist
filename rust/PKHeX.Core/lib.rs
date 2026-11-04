//! Only the PKHeX subset used by PkCompletionist; paths mirror the C# source.
pub mod binary;
#[path = "Saves/SAV2.rs"]
pub mod gen2;
#[path = "PKM/Shared/PokeList2.rs"]
mod poke_list2;
#[path = "PKM/PK2.rs"]
pub mod pk2;
#[path = "Items/Bags/PlayerBag2.rs"]
pub mod player_bag2;
#[path = "Items/ItemStorage2.rs"]
pub mod item_storage2;
