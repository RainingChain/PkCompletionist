use crate::pk1::Pk1;

pub fn unpack(input: &[u8], capacity: usize, party: bool) -> Vec<Pk1> {
    let body_length = if party { 44 } else { 33 };
    let body_start = capacity + 2;
    let trainer_start = body_start + capacity * body_length;
    let nickname_start = trainer_start + capacity * 11;
    let mut records = vec![Pk1::default(); capacity];
    for (index, record) in records
        .iter_mut()
        .enumerate()
        .take(capacity.min(input[0] as usize))
    {
        record.marker = input[index + 1];
        record.body[..body_length].copy_from_slice(
            &input[body_start + index * body_length..body_start + (index + 1) * body_length],
        );
        record
            .trainer
            .copy_from_slice(&input[trainer_start + index * 11..trainer_start + (index + 1) * 11]);
        record.nickname.copy_from_slice(
            &input[nickname_start + index * 11..nickname_start + (index + 1) * 11],
        );
    }
    records
}

pub fn pack(records: &[Pk1], output: &mut [u8], party: bool, initialized: bool) -> bool {
    let present = |pk: &&Pk1| pk.marker != 0 && pk.marker != 0xff;
    let count = records.iter().filter(present).count();
    if count == 0 && (!initialized || output.iter().all(|byte| *byte == 0)) {
        return false;
    }
    let capacity = records.len();
    let body_length = if party { 44 } else { 33 };
    let body_start = capacity + 2;
    let trainer_start = body_start + capacity * body_length;
    let nickname_start = trainer_start + capacity * 11;
    output[0] = count as u8;
    output[capacity + 1] = 0xff;
    for (index, record) in records
        .iter()
        .filter(present)
        .chain(records.iter().filter(|pk| !present(pk)))
        .enumerate()
    {
        output[index + 1] = if record.marker == 0 {
            0xff
        } else {
            record.marker
        };
        output[body_start + index * body_length..body_start + (index + 1) * body_length]
            .copy_from_slice(&record.body[..body_length]);
        output[trainer_start + index * 11..trainer_start + (index + 1) * 11]
            .copy_from_slice(&record.trainer);
        output[nickname_start + index * 11..nickname_start + (index + 1) * 11]
            .copy_from_slice(&record.nickname);
    }
    true
}
