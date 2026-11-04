/// PokeList2.Unpack + MergeSingles, preserving raw entity bytes and strings.
/// Unused slots are zeroed because the unpacked backing buffer starts empty.
pub(crate) fn normalize_list(input: &[u8], capacity: usize, party: bool) -> Vec<u8> {
    if input.iter().all(|byte| *byte == 0) {
        return input.to_vec();
    }
    let body_length = if party { 48 } else { 32 };
    let body_offset = capacity + 2;
    let trainer_offset = body_offset + capacity * body_length;
    let nickname_offset = trainer_offset + capacity * 11;
    let count = capacity.min(input[0] as usize);
    let mut slots = vec![(0xff_u8, vec![0; body_length], [0; 11], [0; 11]); capacity];
    for (index, slot) in slots.iter_mut().take(count).enumerate() {
        slot.0 = match input[index + 1] {
            0 => 0xff,
            marker => marker,
        };
        slot.1.copy_from_slice(
            &input[body_offset + index * body_length..body_offset + (index + 1) * body_length],
        );
        slot.2.copy_from_slice(
            &input[trainer_offset + index * 11..trainer_offset + (index + 1) * 11],
        );
        slot.3.copy_from_slice(
            &input[nickname_offset + index * 11..nickname_offset + (index + 1) * 11],
        );
    }
    slots.sort_by_key(|slot| slot.0 == 0xff);
    let mut output = vec![0; input.len()];
    output[0] = slots.iter().filter(|slot| slot.0 != 0xff).count() as u8;
    output[capacity + 1] = 0xff;
    for (index, slot) in slots.iter().enumerate() {
        output[index + 1] = slot.0;
        output[body_offset + index * body_length..body_offset + (index + 1) * body_length]
            .copy_from_slice(&slot.1);
        output[trainer_offset + index * 11..trainer_offset + (index + 1) * 11]
            .copy_from_slice(&slot.2);
        output[nickname_offset + index * 11..nickname_offset + (index + 1) * 11]
            .copy_from_slice(&slot.3);
    }
    output
}
