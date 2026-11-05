//! Paths mirror PkCompletionist.Core/*.cs. Runtime state belongs to each command.
#[path = "Gen3c/BitBlock.rs"]
pub mod bit_block;
#[path = "Gen5/EventSimulator5.rs"]
mod black2_events;
#[path = "Gen5/CompletionValidator5.rs"]
mod black2_validator;
#[path = "Gen2/Cnst2.rs"]
pub mod cnst2;
#[path = "Command.rs"]
mod command;
#[path = "CompletionValidator.rs"]
mod completion_validator;
#[path = "CompletionValidatorX.rs"]
mod completion_validator_x;
#[path = "Gen2/EventSimulator2.rs"]
mod crystal_events;
#[path = "Gen2/CompletionValidator2.rs"]
mod crystal_validator;
#[path = "Gen3/EventSimulator3.rs"]
mod emerald_events;
#[path = "Gen3/CompletionValidator3.rs"]
mod emerald_validator;
#[path = "Gen4/Enum4.rs"]
pub mod enum4;
#[path = "EventSimulator.rs"]
mod event_simulator;
#[path = "EventSimulatorX.rs"]
mod event_simulator_x;
#[path = "FlagAnalyzer.rs"]
mod flag_analyzer;
#[path = "Meta.rs"]
mod meta;
#[path = "Gen2/MysteryGiftSimulator2.rs"]
mod mystery_gift2;
#[path = "Gen1b/CompletionValidator1_Pinball.rs"]
mod pinball1_validator;
#[path = "Gen3b/EventSimulator3_Pinball.rs"]
mod pinball_events;
#[path = "Gen3b/SAV3_Pinball.rs"]
mod pinball_save;
#[path = "Gen3b/CompletionValidator3_Pinball.rs"]
mod pinball_validator;
#[path = "PkSorter.rs"]
mod pk_sorter;
#[path = "Gen4c/SAV4_Ranger.rs"]
mod ranger_save;
#[path = "Gen4c/CompletionValidator_Ranger.rs"]
mod ranger_validator;
#[path = "Gen3c/SAV3_PmdRescueTeam.rs"]
mod rescue_save;
#[path = "Gen3c/CompletionValidator3_PmdRescueTeam.rs"]
mod rescue_validator;
#[path = "SavDescriptor.rs"]
mod sav_descriptor;
#[path = "SavUtils.rs"]
mod sav_utils;
#[path = "Gen7b/EventSimulator7_Shuffle.rs"]
mod shuffle_events;
#[path = "Gen7b/SAV7_Shuffle.rs"]
mod shuffle_save;
#[path = "Gen7b/CompletionValidator7_Shuffle.rs"]
mod shuffle_validator;
#[path = "Gen4b/SAV4_PmdSky.rs"]
mod sky_save;
#[path = "Gen4b/CompletionValidator4_PmdSky.rs"]
mod sky_validator;
#[path = "Types.rs"]
mod types;
#[path = "Gen1/EventSimulator1.rs"]
mod yellow_events;
#[path = "Gen1/CompletionValidator1.rs"]
mod yellow_validator;

pub use command::{fix_checksum, CommandResult};
pub use completion_validator::validate;
use completion_validator_x::{category, ordered_category};
pub use event_simulator::event;
pub use flag_analyzer::analyze as analyze_flags;
pub use meta::version;
pub use pk_sorter::execute as sort_pokemon;
pub use sav_descriptor::describe;
pub use types::Objective;
