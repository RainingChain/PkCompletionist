#![allow(non_snake_case, non_camel_case_types)]
use crate::{category, ranger_save::MON_LIST, CommandResult};
struct CompletionValidator4_Ranger<'a> {
    sav: &'a [u8],
}
pub fn validate(data: &[u8], result: &mut CommandResult) {
    CompletionValidator4_Ranger { sav: data }.GenerateAll(result);
}
impl CompletionValidator4_Ranger<'_> {
    fn HasMon(&self, name: &str) -> bool {
        MON_LIST
            .iter()
            .position(|mon| *mon == name)
            .is_some_and(|index| self.sav[0x896d + index] == 2)
    }
    fn GenerateAll(&self, result: &mut CommandResult) {
        self.Generate_pokemon(result);
        self.Generate_mission(result);
        self.Generate_misc(result);
    }
    fn Generate_pokemon(&self, result: &mut CommandResult) {
        category(
            result,
            "pokemon",
            MON_LIST
                .iter()
                .map(|name| (name.to_string(), self.HasMon(name))),
        );
    }
    fn Generate_mission(&self, result: &mut CommandResult) {
        category(
            result,
            "mission",
            MISSIONS
                .iter()
                .map(|(name, mon)| (name.to_string(), self.HasMon(mon))),
        );
    }
    fn Generate_misc(&self, result: &mut CommandResult) {
        category(
            result,
            "misc",
            [
                ("50000PtsatCaptureArena", "R-040"),
                ("5000PtsatGrasslandCaptureChallenge", "R-042"),
                ("5000PtsatMarineCaptureChallenge", "R-123"),
            ]
            .into_iter()
            .map(|(name, mon)| (name.to_string(), self.HasMon(mon))),
        );
    }
}
const MISSIONS: &[(&str, &str)] = &[
    ("RookieMissionDontFearFailure", "R-036"),
    ("Mission1EscorttheProfessor", "R-004"),
    ("Mission2FallCityCaseLog", "R-081"),
    ("Mission3GrimerOutbreak", "R-088"),
    ("Mission4WheresPolitoed", "R-055"),
    ("Mission5ClearTheRockfalls", "R-136"),
    ("Mission6TheWaywardWanderer", "R-046"),
    ("Mission7InvestigateTheFactory", "R-044"),
    ("Mission8AquamoleToTheNorth", "R-179"),
    ("Mission9HideoutInfiltration", "R-195"),
    ("Mission10FioreTemple", "R-204"),
    ("SpecialMissionSearchtheSafraSea", "R-208"),
    ("SpecialMissionSummerlandRescueDuo", "R-209"),
    ("SpecialMissionTheTemplesSinisterShadows", "R-210"),
    ("RangerNetMissionGainDeoxyssTrust", "R-211"),
    ("RangerNetMissionRescueCelebi", "R-212"),
    ("RangerNetMissionFindMewTheMirage", "R-213"),
];
