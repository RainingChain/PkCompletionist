use pkhex_core::gen2::CrystalSave;

pub(crate) fn add_pokemon5(
    save: &mut pkhex_core::gen5::Save5,
    resource: &[u8],
) -> Result<(), &'static str> {
    let pk = pkhex_core::pk5::Pk5::read(resource).map_err(|_| "Invalid Pokemon data.")?;
    let boxes = save.boxes().map_err(|_| "Invalid Pokemon data.")?;
    let slot = boxes
        .iter()
        .position(|pk| pk.species() == 0)
        .ok_or("Error: All pokemon boxes are full. Free a box slot to fix the issue.")?;
    if save
        .pokemon()
        .map_err(|_| "Invalid Pokemon data.")?
        .iter()
        .any(|other| {
            pk.species() == other.species()
                && pk.form() == other.form()
                && pk.trainer_id() == other.trainer_id()
                && pk.original_trainer() == other.original_trainer()
                && pk.ivs() == other.ivs()
        })
    {
        return Err("Error: You already have that pokemon.");
    }
    save.set_box(slot, &pk);
    Ok(())
}

pub(crate) fn pokemon_count(
    format: crate::command::Format,
    data: &[u8],
) -> Result<usize, pkhex_core::binary::InvalidData> {
    use crate::command::Format;
    Ok(match format {
        Format::Black2 => pkhex_core::gen5::Save5::read(data)?.pokemon()?.len(),
        Format::Emerald => pkhex_core::gen3::Save3::read(data)?.pokemon()?.len(),
        Format::Crystal => pkhex_core::gen2::CrystalSave::read(data)?.pokemon().len(),
        Format::Yellow => pkhex_core::gen1::Save1::read(data)?
            .pokemon()
            .filter(|pk| pk.species() != 0)
            .count(),
        _ => 0,
    })
}

pub(crate) fn has_pokemon_with_trainer(save: &CrystalSave, species: u16) -> bool {
    save.pokemon()
        .iter()
        .any(|pk| pk.species == species && pk.trainer_id == save.trainer_id())
}

pub(crate) fn add_item(save: &mut CrystalSave, id: u16) -> Result<(), &'static str> {
    if pkhex_core::player_bag2::items(&save.data).contains(&id)
        || save.pokemon().iter().any(|pk| pk.held_item == id)
    {
        return Err("Error: You already have that item.");
    }
    pkhex_core::player_bag2::give_item(&mut save.data, id)
}

pub(crate) fn add_item3(save: &mut pkhex_core::gen3::Save3, id: u16) -> Result<(), &'static str> {
    if pkhex_core::player_bag3e::items(save).contains(&id)
        || save
            .pokemon()
            .map_err(|_| "Invalid Pokemon data.")?
            .iter()
            .any(|pk| pk.held_item() == id)
    {
        return Err("Error: You already have that item.");
    }
    pkhex_core::player_bag3e::give_item(save, id)
}

pub(crate) fn add_pokemon3(
    save: &mut pkhex_core::gen3::Save3,
    resource: &[u8],
) -> Result<(), &'static str> {
    let pk = pkhex_core::pk3::Pk3::read(resource).map_err(|_| "Invalid Pokemon data.")?;
    let boxes = save.boxes().map_err(|_| "Invalid Pokemon data.")?;
    let slot = boxes
        .iter()
        .position(|pk| pk.species() == 0)
        .ok_or("Error: All pokemon boxes are full. Free a box slot to fix the issue.")?;
    let name = |bytes: &[u8]| {
        bytes
            .iter()
            .copied()
            .take_while(|byte| *byte != 0xff)
            .collect::<Vec<_>>()
    };
    if save
        .pokemon()
        .map_err(|_| "Invalid Pokemon data.")?
        .iter()
        .any(|other| {
            pk.species() == other.species()
                && pk.form() == other.form()
                && pk.trainer_id() == other.trainer_id()
                && pk.ivs() == other.ivs()
                && name(&pk.data[0x14..0x1b]) == name(&other.data[0x14..0x1b])
        })
    {
        return Err("Error: You already have that pokemon.");
    }
    save.set_box(slot, &pk);
    Ok(())
}
