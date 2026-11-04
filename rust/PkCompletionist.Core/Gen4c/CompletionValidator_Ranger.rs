use crate::{category, CommandResult, ranger_save::MON_LIST};
pub fn validate(data: &[u8], result: &mut CommandResult) {
    let has = |name: &str| MON_LIST.iter().position(|mon| *mon == name).is_some_and(|index| data[0x896d + index] == 2);
    category(result, "pokemon", MON_LIST.iter().map(|name| (name.to_string(), has(name))));
    category(result, "mission", MISSIONS.iter().map(|(name, mon)| (name.to_string(), has(mon))));
    category(result, "misc", [("50000PtsatCaptureArena", "R-040"), ("5000PtsatGrasslandCaptureChallenge", "R-042"), ("5000PtsatMarineCaptureChallenge", "R-123")].into_iter().map(|(name, mon)| (name.to_string(), has(mon))));
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
