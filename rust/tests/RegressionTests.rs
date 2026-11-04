use pkcompletionist_core::{validate, CommandResult};
use serde_json::Value;
use sha2::{Digest, Sha256};
use std::{fs, path::PathBuf};

fn tests_directory() -> PathBuf {
    PathBuf::from(env!("CARGO_MANIFEST_DIR")).join("../tests/PkCompletionist.Tests")
}
fn read_json(path: impl AsRef<std::path::Path>) -> Value {
    serde_json::from_slice(&fs::read(path).unwrap()).unwrap()
}
fn hash(bytes: &[u8]) -> String {
    format!("{:X}", Sha256::digest(bytes))
}
fn event(name: &str, data: &[u8], hint: &str, second: Option<&[u8]>) -> CommandResult {
    pkcompletionist_core::event(name, data, hint, second, &mut || 0)
}

#[test]
fn migrated_completion_matches_csharp_fixtures() {
    let root = tests_directory();
    for id in [
        "pinball-ruby-sapphire",
        "shuffle",
        "ranger",
        "red-rescue-team",
        "explorers-of-sky",
        "crystal",
        "yellow",
        "emerald",
    ] {
        for (objective, number) in [("normal", 0), ("living", 1)] {
            let fixture =
                read_json(root.join(format!("Fixtures/Completion/{id}.{objective}.json")));
            let input = fs::read(
                root.join("Saves")
                    .join(fixture["SaveFile"].as_str().unwrap()),
            )
            .unwrap();
            assert_eq!(hash(&input), fixture["SaveSha256"], "{id}: input changed");
            let result = validate(&input, fixture["VersionHint"].as_str().unwrap(), number);
            for field in ["Success", "ObtainedStatus", "CompletionHints", "Messages"] {
                assert_eq!(
                    result.json()[field],
                    fixture[field],
                    "{id}/{objective}/{field}"
                );
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
        if ![
            "pinball-ruby-sapphire.",
            "shuffle.",
            "ranger.",
            "red-rescue-team.",
            "explorers-of-sky.",
            "crystal.",
            "yellow.",
            "emerald.",
        ]
        .iter()
        .any(|prefix| name.starts_with(prefix))
        {
            continue;
        }
        let fixture = read_json(path.join("result.json"));
        let input = fs::read(
            root.join("Saves")
                .join(fixture["SaveFile"].as_str().unwrap()),
        )
        .unwrap();
        assert_eq!(hash(&input), fixture["SaveSha256"], "{name}: input changed");
        let result = event(
            fixture["Event"].as_str().unwrap(),
            &input,
            fixture["VersionHint"].as_str().unwrap(),
            None,
        );
        for field in ["Success", "Error", "Messages"] {
            assert_eq!(result.json()[field], fixture[field], "{name}/{field}");
        }
        assert_eq!(
            hash(&result.save_a),
            fixture["OutputSaveASha256"],
            "{name}: output bytes"
        );
        assert!(result.save_b.is_empty(), "{name}: unexpected second save");
    }
}

#[test]
fn invalid_and_truncated_inputs_do_not_panic() {
    let root = tests_directory();
    for filename in [
        "Pokemon Pinball - Ruby & Sapphire.sav",
        "Pokemon Shuffle.bin",
        "Pokemon Ranger.dsv",
    ] {
        let bytes = fs::read(root.join("Saves").join(filename)).unwrap();
        for length in [0, 1, 10, 0x100, 0x400] {
            let input = &bytes[..length];
            assert!(!validate(input, "", 0).success, "{filename}/{length}");
            assert!(
                !event("AddCoins", input, "", None).success,
                "{filename}/{length}"
            );
        }
    }
}

#[test]
fn pinball_event_preserves_caught_entries_and_updates_backup() {
    let mut input =
        fs::read(tests_directory().join("Saves/Pokemon Pinball - Ruby & Sapphire.sav")).unwrap();
    input[205..209].copy_from_slice(&[0, 1, 3, 4]);
    let result = event("TransmitDexWithEReaderMons", &input, "", None);
    assert!(result.success);
    assert_eq!(&result.save_a[205..209], &[2, 2, 3, 4]);
    assert_eq!(&result.save_a[4..0x278], &result.save_a[0x2a4..0x518]);
    assert_eq!(&input[205..209], &[0, 1, 3, 4]);
    assert_eq!(
        result.save_a,
        pkcompletionist_core::fix_checksum(&result.save_a, "").unwrap()
    );
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
    let before = fs::read(
        root.join("Saves")
            .join(expected["SaveFile"].as_str().unwrap()),
    )
    .unwrap();
    let after = fs::read(
        root.join("Saves")
            .join(expected["SecondSaveFile"].as_str().unwrap()),
    )
    .unwrap();
    assert_eq!(hash(&before), expected["SaveSha256"]);
    assert_eq!(hash(&after), expected["SecondSaveSha256"]);
    let result = pkcompletionist_core::analyze_flags(
        &before,
        &after,
        expected["ChangeRanges"].as_str().unwrap(),
        "",
    );
    for field in ["Success", "Error", "Messages"] {
        assert_eq!(result.json()[field], expected[field], "{field}");
    }
    assert_eq!(hash(&result.save_a), expected["OutputSaveASha256"]);
    assert_eq!(hash(&result.save_b), expected["OutputSaveBSha256"]);
}

#[test]
fn crystal_mystery_gift_updates_both_players_and_preserves_rtc_footers() {
    let root = tests_directory();
    let mut input = fs::read(root.join("Saves/Pokemon Crystal.sav")).unwrap();
    input[0xbe3] = 0; // Mystery Gift unlocked.
    let mut calls = 0;
    let result = pkcompletionist_core::event("MysteryGift", &input, "", Some(&input), &mut || {
        calls += 1;
        255 // Always choose an item rather than a decoration.
    });
    assert!(result.success, "{:?}", result.messages);
    assert_eq!(calls, 10);
    assert_eq!(result.messages.len(), 2);
    assert!(result.messages[0].starts_with("Player 1 received: "));
    assert!(result.messages[1].starts_with("Player 2 received: "));
    for bytes in [&result.save_a, &result.save_b] {
        let save = pkhex_core::gen2::CrystalSave::read(bytes).unwrap();
        assert!(!save.event(1809));
        assert_ne!(save.data[0xbe4], 0);
        assert_eq!(&bytes[0x8000..], &input[0x8000..]);
        assert_eq!(bytes, &save.write());
    }
}

#[test]
fn crystal_mystery_gift_stops_on_pending_item_and_keeps_prior_changes() {
    let input = fs::read(tests_directory().join("Saves/Pokemon Crystal.sav")).unwrap();
    let mut save = pkhex_core::gen2::CrystalSave::read(&input).unwrap();
    save.data[0xbe3] = 0;
    save.set_event(1809, true);
    let input = save.write();
    let result =
        pkcompletionist_core::event("MysteryGiftx1000", &input, "", Some(&input), &mut || 255);
    assert!(!result.success);
    assert_eq!(
        result.error.as_deref(),
        Some("Player 1 has an item waiting at PK Center.")
    );
    assert_eq!(result.messages.len(), 1); // only player 1 decorations would be printed
    for bytes in [&result.save_a, &result.save_b] {
        assert_ne!(
            pkhex_core::gen2::CrystalSave::read(bytes).unwrap().data[0xbe4],
            0
        );
    }
}

#[test]
fn bit_block_preserves_unaligned_fields_signed_values_and_partial_byte_behavior() {
    use pkcompletionist_core::bit_block::BitBlock;
    let mut block = BitBlock::from_bytes(&[0xaa, 0x55, 0, 0, 0, 0]);
    block.set_uint(0, 7, 11, 0x731).unwrap();
    assert_eq!(block.get_uint(0, 7, 11).unwrap(), 0x731);
    assert_eq!(block.get_uint(0, 0, 7).unwrap(), 0x2a);
    block.set_int(2, 0, 32, -1).unwrap();
    assert_eq!(block.get_int(2, 0, 32).unwrap(), -1);
    block.position = 7;
    assert_eq!(block.get_next_uint(11).unwrap(), 0x731);
    assert_eq!(block.position, 18);
    assert_eq!(block.clone().position, 0);
    assert_eq!(
        block.get_range(0, 9).unwrap().to_bytes(),
        vec![block.to_bytes()[0]]
    );
    assert!(block.get_uint(6, 0, 1).is_err());
}

#[test]
fn migrated_descriptors_match_csharp_fixtures() {
    let root = tests_directory();
    for id in [
        "yellow",
        "emerald",
        "crystal",
        "pinball-ruby-sapphire",
        "shuffle",
        "ranger",
        "red-rescue-team",
        "explorers-of-sky",
    ] {
        let fixture = read_json(root.join(format!("Fixtures/Descriptors/{id}.json")));
        let input = fs::read(
            root.join("Saves")
                .join(fixture["SaveFile"].as_str().unwrap()),
        )
        .unwrap();
        let result =
            pkcompletionist_core::describe(&input, fixture["VersionHint"].as_str().unwrap());
        assert!(result.success, "{id}: {:?}", result.messages);
        assert_eq!(result.json()["Descriptor"], fixture["Descriptor"], "{id}");
        assert_eq!(result.json()["Messages"], fixture["Messages"], "{id}");
    }
}

#[test]
fn emerald_sorted_save_matches_csharp_order_and_complete_output_hash() {
    let root = tests_directory();
    let input = fs::read(root.join("Saves/Pokemon Emerald.sav")).unwrap();
    let result = pkcompletionist_core::sort_pokemon(&input, "");
    assert!(result.success, "{:?}", result.messages);
    let reloaded = pkhex_core::gen3::Save3::read(&result.save_a).unwrap();
    let order = reloaded
        .boxes()
        .unwrap()
        .iter()
        .map(|pk| pk.species().to_string())
        .collect::<Vec<_>>()
        .join(",");
    assert_eq!(
        order,
        fs::read_to_string(root.join("Fixtures/emerald.sorted-boxes.txt")).unwrap()
    );
    assert_eq!(
        hash(&result.save_a),
        fs::read_to_string(root.join("Fixtures/emerald.sorted-save.sha256.txt")).unwrap()
    );
}

#[test]
fn pk3_round_trips_every_block_permutation_without_changing_record_content() {
    let resource = fs::read(
        PathBuf::from(env!("CARGO_MANIFEST_DIR"))
            .join("PkCompletionist.Core/Gen3/Resources/Jirachi.pk3"),
    )
    .unwrap();
    let pk = pkhex_core::pk3::Pk3::read(&resource).unwrap();
    for permutation in 0..24 {
        let mut pk = pk.clone();
        pk.data[..4].copy_from_slice(&(0x12345678_u32 / 24 * 24 + permutation).to_le_bytes());
        let encoded = pk.write(false);
        let decoded = pkhex_core::pk3::Pk3::read(&encoded).unwrap();
        assert_eq!(
            &decoded.data[..0x1c],
            &pk.data[..0x1c],
            "Header, permutation {permutation}"
        );
        assert_eq!(
            &decoded.data[0x1e..80],
            &pk.data[0x1e..80],
            "Record, permutation {permutation}"
        );
        assert_eq!(
            decoded.write(false),
            encoded,
            "Idempotence, permutation {permutation}"
        );
    }
}

#[test]
fn flag_analyzer_uses_difference_indices_and_exclusive_range_end() {
    let input = fs::read(tests_directory().join("Saves/Pokemon Shuffle.bin")).unwrap();
    let mut after = input.clone();
    after[0x40] ^= 1;
    after[0x41] ^= 2;
    let result = pkcompletionist_core::analyze_flags(&input, &after, "1,2", "");
    assert!(result.success);
    assert_eq!(result.messages.len(), 2);
    assert_eq!(result.messages[0], "Change count + 1: 3");
    assert_eq!(result.save_a[0x40], input[0x40]);
    assert_eq!(result.save_a[0x41], after[0x41]);
    assert_eq!(result.save_b, after);
    assert!(!pkcompletionist_core::analyze_flags(&input, &after, "-1,2", "").success);
}
