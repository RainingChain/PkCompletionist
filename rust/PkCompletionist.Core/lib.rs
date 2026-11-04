//! Paths mirror PkCompletionist.Core/*.cs. Runtime state belongs to each command.
#[path = "Command.rs"]
mod command;
#[path = "CompletionValidator.rs"]
mod completion_validator;
#[path = "CompletionValidatorX.rs"]
mod completion_validator_x;
#[path = "EventSimulator.rs"]
mod event_simulator;
#[path = "FlagAnalyzer.rs"]
mod flag_analyzer;
#[path = "Gen1b/CompletionValidator1_Pinball.rs"]
mod pinball1_validator;
#[path = "Gen2/CompletionValidator2.rs"]
mod crystal_validator;
#[path = "Gen2/Cnst2.rs"]
mod cnst2;
#[path = "Gen3b/SAV3_Pinball.rs"]
mod pinball_save;
#[path = "Gen3b/CompletionValidator3_Pinball.rs"]
mod pinball_validator;
#[path = "Gen3b/EventSimulator3_Pinball.rs"]
mod pinball_events;
#[path = "Gen3c/SAV3_PmdRescueTeam.rs"]
mod rescue_save;
#[path = "Gen3c/CompletionValidator3_PmdRescueTeam.rs"]
mod rescue_validator;
#[path = "Gen4b/SAV4_PmdSky.rs"]
mod sky_save;
#[path = "Gen4b/CompletionValidator4_PmdSky.rs"]
mod sky_validator;
#[path = "Gen4c/SAV4_Ranger.rs"]
mod ranger_save;
#[path = "Gen4c/CompletionValidator_Ranger.rs"]
mod ranger_validator;
#[path = "Gen7b/SAV7_Shuffle.rs"]
mod shuffle_save;
#[path = "Gen7b/CompletionValidator7_Shuffle.rs"]
mod shuffle_validator;
#[path = "Gen7b/EventSimulator7_Shuffle.rs"]
mod shuffle_events;

pub use command::{fix_checksum, CommandResult};
pub use completion_validator::validate;
pub use event_simulator::event;
pub use flag_analyzer::analyze as analyze_flags;
use completion_validator_x::{category, ordered_category};
