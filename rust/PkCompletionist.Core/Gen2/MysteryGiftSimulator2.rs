use pkhex_core::gen2::CrystalSave;

pub(crate) fn execute(
    first: &mut CrystalSave,
    second: &mut CrystalSave,
    count: i32,
    only_first_decorations: bool,
    random: &mut dyn FnMut() -> u8,
    messages: &mut Vec<String>,
) -> Option<&'static str> {
    for _ in 0..count {
        if first.data[0xbe3] != 0 {
            return Some("Player 1 has not unlocked Mystery Gift.");
        }
        if second.data[0xbe3] != 0 {
            return Some("Player 2 has not unlocked Mystery Gift.");
        }
        if count <= 1 {
            for save in [&mut *first, &mut *second] {
                save.set_event(1809, true);
                save.data[0xbe4] = 0;
            }
        }
        if !first.event(1809) {
            return Some("Player 1 has an item waiting at PK Center.");
        }
        if !second.event(1809) {
            return Some("Player 2 has an item waiting at PK Center.");
        }
        receive(
            first,
            second.trainer_id(),
            1,
            only_first_decorations,
            random,
            messages,
        );
        receive(
            second,
            first.trainer_id(),
            2,
            only_first_decorations,
            random,
            messages,
        );
    }
    None
}

fn receive(
    save: &mut CrystalSave,
    trainer: u16,
    player: usize,
    quiet: bool,
    random: &mut dyn FnMut() -> u8,
    messages: &mut Vec<String>,
) {
    let decoration = random_index(trainer, true, random);
    let flag = DECORATIONS[decoration];
    if random() % 2 == 0 && !save.event(flag) {
        save.set_event(flag, true);
        if player == 1 || !quiet {
            messages.push(format!(
                "Player {player} received: {} (Idx {decoration})",
                DECORATION_NAMES[decoration]
            ));
        }
    } else {
        let item = random_index(trainer, false, random);
        save.set_event(1809, false);
        save.data[0xbe4] = ITEMS[item];
        if !quiet {
            messages.push(format!(
                "Player {player} received: {} (Idx {item})",
                ITEM_NAMES[item]
            ));
        }
    }
}

fn random_index(trainer: u16, decoration: bool, random: &mut dyn FnMut() -> u8) -> usize {
    if random() >= 26 {
        let group = (random() % 8) as usize;
        let bit = if decoration {
            [15, 8, 9, 10, 11, 12, 13, 14][group]
        } else {
            [7, 0, 1, 2, 3, 4, 5, 6][group]
        };
        return group * 2 + usize::from(trainer & (1 << bit) != 0);
    }
    if random() >= 51 {
        let group = (random() % 4) as usize;
        let bit = if decoration {
            [7, 0, 1, 2][group]
        } else {
            [15, 8, 9, 10][group]
        };
        return 16 + group * 2 + usize::from(trainer & (1 << bit) != 0);
    }
    if random() >= 51 {
        return 24
            + if decoration {
                ((trainer & 0b1110000) >> 4) as usize
            } else {
                ((trainer & 0b111000000000000) >> 12) as usize
            };
    }
    32 + usize::from(trainer & (1 << if decoration { 7 } else { 15 }) != 0)
}
// Literal gift tables ported from MysteryGiftSimulator2.cs.
const ITEMS: &[u8] = &[
    173, 78, 84, 80, 79, 74, 41, 51, 49, 83, 44, 53, 33, 185, 186, 188, 109, 174, 39, 4, 42, 43,
    65, 63, 24, 22, 34, 23, 64, 21, 40, 140, 26, 62,
];
const DECORATIONS: &[usize] = &[
    698, 702, 703, 704, 705, 706, 707, 708, 709, 710, 689, 690, 692, 711, 713, 714, 684, 685, 691,
    693, 699, 701, 677, 678, 680, 681, 682, 683, 686, 694, 720, 688, 721, 696,
];
const ITEM_NAMES: &[&str] = &[
    "Berry",
    "PRZCureBerry",
    "Mint Berry",
    "Ice Berry",
    "Burnt Berry",
    "PSNCureBerry",
    "Guard Spec",
    "X Defend",
    "X Attack",
    "Bitter Berry",
    "Dire Hit",
    "X Special",
    "X Accuracy",
    "Eon Mail",
    "Morph Mail",
    "Music Mail",
    "MiracleBerry",
    "Gold Berry",
    "Revive",
    "Great Ball",
    "Super Repel",
    "Max Repel",
    "Elixer",
    "Ether",
    "Water Stone",
    "Fire Stone",
    "Leaf Stone",
    "Thunderstone",
    "Max Ether",
    "Max Elixer",
    "Max Revive",
    "Scope Lens",
    "HP Up",
    "PP Up",
];
const DECORATION_NAMES: &[&str] = &[
    "Jigglypuff Doll",
    "Poliwag Doll",
    "Diglett Doll",
    "Staryu Doll",
    "Magikarp Doll",
    "Oddish Doll",
    "Gengar Doll",
    "Shellder Doll",
    "Grimer Doll",
    "Voltorb Doll",
    "Clefairy Poster",
    "Jigglypuff Poster",
    "Super NES",
    "Weedle Doll",
    "Geodude Doll",
    "Machop Doll",
    "Magnaplant",
    "Tropicplant",
    "NES",
    "Nintendo64",
    "Bulbasaur Doll",
    "Squirtle Doll",
    "Pink Bed",
    "Polkadot Bed",
    "Red Carpet",
    "Blue Carpet",
    "Yellow Carpet",
    "Green Carpet",
    "Jumboplant",
    "Virtual Boy",
    "Big Onix Doll",
    "Pikachu Poster",
    "Big Lapras Doll",
    "Surf Pikachu Doll",
];
