use pkcompletionist_core::{event, validate};
use serde_json::Value;
use sha2::{Digest, Sha256};
use std::{fs, path::PathBuf};

fn tests_directory() -> PathBuf { PathBuf::from(env!("CARGO_MANIFEST_DIR")).join("../tests/PkCompletionist.Tests") }
fn read_json(path: impl AsRef<std::path::Path>) -> Value { serde_json::from_slice(&fs::read(path).unwrap()).unwrap() }
fn hash(bytes: &[u8]) -> String { format!("{:X}", Sha256::digest(bytes)) }

#[test]
fn migrated_completion_matches_csharp_fixtures() {
    let root = tests_directory();
    for id in ["pinball-ruby-sapphire", "shuffle", "ranger", "red-rescue-team", "explorers-of-sky", "crystal"] {
        for (objective, number) in [("normal", 0), ("living", 1)] {
            let fixture = read_json(root.join(format!("Fixtures/Completion/{id}.{objective}.json")));
            let input = fs::read(root.join("Saves").join(fixture["SaveFile"].as_str().unwrap())).unwrap();
            assert_eq!(hash(&input), fixture["SaveSha256"], "{id}: input changed");
            let result = validate(&input, fixture["VersionHint"].as_str().unwrap(), number);
            for field in ["Success", "ObtainedStatus", "CompletionHints", "Messages"] {
                assert_eq!(result.json()[field], fixture[field], "{id}/{objective}/{field}");
            }
        }
    }
}

#[test]
fn migrated_events_match_csharp_fixtures() {
    let root = tests_directory();
    for entry in fs::read_dir(root.join("Fixtures/Events")).unwrap() {
        let path = entry.unwrap().path();
        let name = path.file_name().unwrap().to_str().unwrap();
        if !["pinball-ruby-sapphire.", "shuffle.", "ranger.", "red-rescue-team.", "explorers-of-sky."].iter().any(|prefix| name.starts_with(prefix)) { continue; }
        let fixture = read_json(path.join("result.json"));
        let input = fs::read(root.join("Saves").join(fixture["SaveFile"].as_str().unwrap())).unwrap();
        assert_eq!(hash(&input), fixture["SaveSha256"], "{name}: input changed");
        let result = event(fixture["Event"].as_str().unwrap(), &input, fixture["VersionHint"].as_str().unwrap(), None);
        for field in ["Success", "Error", "Messages"] { assert_eq!(result.json()[field], fixture[field], "{name}/{field}"); }
        assert_eq!(hash(&result.save_a), fixture["OutputSaveASha256"], "{name}: output bytes");
        assert!(result.save_b.is_empty(), "{name}: unexpected second save");
    }
}

#[test]
fn invalid_and_truncated_inputs_do_not_panic() {
    let root = tests_directory();
    for filename in ["Pokemon Pinball - Ruby & Sapphire.sav", "Pokemon Shuffle.bin", "Pokemon Ranger.dsv"] {
        let bytes = fs::read(root.join("Saves").join(filename)).unwrap();
        for length in [0,1,10,0x100,0x400] {
            let input = &bytes[..length];
            assert!(!validate(input, "", 0).success, "{filename}/{length}");
            assert!(!event("AddCoins", input, "", None).success, "{filename}/{length}");
        }
    }
}

#[test]
fn pinball_event_preserves_caught_entries_and_updates_backup() {
    let mut input = fs::read(tests_directory().join("Saves/Pokemon Pinball - Ruby & Sapphire.sav")).unwrap();
    input[205..209].copy_from_slice(&[0,1,3,4]);
    let result = event("TransmitDexWithEReaderMons", &input, "", None);
    assert!(result.success);
    assert_eq!(&result.save_a[205..209], &[2,2,3,4]);
    assert_eq!(&result.save_a[4..0x278], &result.save_a[0x2a4..0x518]);
    assert_eq!(&input[205..209], &[0,1,3,4]);
    assert_eq!(result.save_a, pkcompletionist_core::fix_checksum(&result.save_a, "").unwrap());
}

#[test]
fn shuffle_requires_all_three_caught_bits() {
    let mut input = fs::read(tests_directory().join("Saves/Pokemon Shuffle.bin")).unwrap();
    // Clear one copy of Bulbasaur's caught flag; the other copies cannot make it owned.
    input[0xe6] &= !(1 << 6);
    let result = validate(&input, "", 0);
    assert!(result.success);
    assert!(!result.obtained_status[1].split(',').any(|id| id == "1"));
    assert!(result.obtained_status[2].split(',').any(|id| id == "1"));
}

#[test]
fn crystal_flag_analysis_matches_csharp_fixture() {
    let root = tests_directory();
    let expected = read_json(root.join("Fixtures/crystal.flag-analyzer.json"));
    let before = fs::read(root.join("Saves").join(expected["SaveFile"].as_str().unwrap())).unwrap();
    let after = fs::read(root.join("Saves").join(expected["SecondSaveFile"].as_str().unwrap())).unwrap();
    assert_eq!(hash(&before), expected["SaveSha256"]);
    assert_eq!(hash(&after), expected["SecondSaveSha256"]);
    let result = pkcompletionist_core::analyze_flags(&before, &after, expected["ChangeRanges"].as_str().unwrap(), "");
    for field in ["Success", "Error", "Messages"] { assert_eq!(result.json()[field], expected[field], "{field}"); }
    assert_eq!(hash(&result.save_a), expected["OutputSaveASha256"]);
    assert_eq!(hash(&result.save_b), expected["OutputSaveBSha256"]);
}
