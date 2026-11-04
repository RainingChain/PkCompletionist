//! BitBlock port. Original: evandixon/SkyEditor.SaveEditor (MIT), as credited in C#.
use pkhex_core::binary::InvalidData;

#[derive(Debug, Default)]
pub struct BitBlock {
    pub bits: Vec<bool>,
    pub position: usize,
}

impl Clone for BitBlock {
    fn clone(&self) -> Self {
        Self {
            bits: self.bits.clone(),
            position: 0,
        }
    }
}

impl BitBlock {
    pub fn new(length: usize) -> Self {
        Self {
            bits: vec![false; length],
            position: 0,
        }
    }
    pub fn from_bits(bits: impl IntoIterator<Item = bool>) -> Self {
        Self {
            bits: bits.into_iter().collect(),
            position: 0,
        }
    }
    pub fn from_bytes(bytes: &[u8]) -> Self {
        Self::from_bits(
            bytes
                .iter()
                .flat_map(|byte| (0..8).map(move |bit| byte & (1 << bit) != 0)),
        )
    }
    pub fn count(&self) -> usize {
        self.bits.len()
    }

    pub fn get_uint(
        &self,
        byte_index: usize,
        bit_index: usize,
        width: usize,
    ) -> Result<u32, InvalidData> {
        let offset = byte_index
            .checked_mul(8)
            .and_then(|offset| offset.checked_add(bit_index))
            .ok_or(InvalidData("Offset overflow."))?;
        let end = offset
            .checked_add(width)
            .ok_or(InvalidData("Offset overflow."))?;
        let source = self
            .bits
            .get(offset..end)
            .ok_or(InvalidData("Invalid bit range."))?;
        Ok(source.iter().enumerate().fold(0, |value, (bit, set)| {
            value | (u32::from(*set) << (bit % 32))
        }))
    }
    pub fn get_int(
        &self,
        byte_index: usize,
        bit_index: usize,
        width: usize,
    ) -> Result<i32, InvalidData> {
        Ok(self.get_uint(byte_index, bit_index, width)? as i32)
    }
    pub fn get_ushort(
        &self,
        byte_index: usize,
        bit_index: usize,
        width: usize,
    ) -> Result<u16, InvalidData> {
        Ok(self.get_uint(byte_index, bit_index, width)? as u16)
    }
    pub fn get_short(
        &self,
        byte_index: usize,
        bit_index: usize,
        width: usize,
    ) -> Result<i16, InvalidData> {
        Ok(self.get_uint(byte_index, bit_index, width)? as i16)
    }

    pub fn set_uint(
        &mut self,
        byte_index: usize,
        bit_index: usize,
        width: usize,
        value: u32,
    ) -> Result<(), InvalidData> {
        self.set_value(byte_index, bit_index, width.clamp(1, 32), value)
    }
    pub fn set_int(
        &mut self,
        byte_index: usize,
        bit_index: usize,
        width: usize,
        value: i32,
    ) -> Result<(), InvalidData> {
        self.set_uint(byte_index, bit_index, width, value as u32)
    }
    pub fn set_short(
        &mut self,
        byte_index: usize,
        bit_index: usize,
        width: usize,
        value: i16,
    ) -> Result<(), InvalidData> {
        self.set_value(
            byte_index,
            bit_index,
            width.clamp(1, 16),
            value as u16 as u32,
        )
    }
    // C# SetUShort writes all sixteen bits, regardless of the requested bitLength.
    pub fn set_ushort(
        &mut self,
        byte_index: usize,
        bit_index: usize,
        _width: usize,
        value: u16,
    ) -> Result<(), InvalidData> {
        self.set_value(byte_index, bit_index, 16, u32::from(value))
    }

    fn set_value(
        &mut self,
        byte_index: usize,
        bit_index: usize,
        width: usize,
        value: u32,
    ) -> Result<(), InvalidData> {
        let offset = byte_index
            .checked_mul(8)
            .and_then(|offset| offset.checked_add(bit_index))
            .ok_or(InvalidData("Offset overflow."))?;
        let end = offset
            .checked_add(width)
            .ok_or(InvalidData("Offset overflow."))?;
        for (bit, target) in self
            .bits
            .get_mut(offset..end)
            .ok_or(InvalidData("Invalid bit range."))?
            .iter_mut()
            .enumerate()
        {
            *target = value & (1 << bit) != 0;
        }
        Ok(())
    }

    pub fn get_next_uint(&mut self, width: usize) -> Result<u32, InvalidData> {
        let value = self.get_uint(0, self.position, width)?;
        self.position += width;
        Ok(value)
    }
    pub fn get_next_int(&mut self, width: usize) -> Result<i32, InvalidData> {
        Ok(self.get_next_uint(width)? as i32)
    }
    pub fn get_next_short(&mut self, width: usize) -> Result<i16, InvalidData> {
        Ok(self.get_next_uint(width)? as i16)
    }
    pub fn get_next_ushort(&mut self, width: usize) -> Result<u16, InvalidData> {
        Ok(self.get_next_uint(width)? as u16)
    }
    pub fn set_next_uint(&mut self, width: usize, value: u32) -> Result<(), InvalidData> {
        self.set_uint(0, self.position, width, value)?;
        self.position += width;
        Ok(())
    }
    pub fn set_next_int(&mut self, width: usize, value: i32) -> Result<(), InvalidData> {
        self.set_next_uint(width, value as u32)
    }
    pub fn set_next_short(&mut self, width: usize, value: i16) -> Result<(), InvalidData> {
        self.set_short(0, self.position, width, value)?;
        self.position += width;
        Ok(())
    }
    pub fn set_next_ushort(&mut self, width: usize, value: u16) -> Result<(), InvalidData> {
        self.set_ushort(0, self.position, width, value)?;
        self.position += width;
        Ok(())
    }

    pub fn get_range(&self, index: usize, length: usize) -> Result<Self, InvalidData> {
        let end = index
            .checked_add(length)
            .ok_or(InvalidData("Offset overflow."))?;
        Ok(Self::from_bits(
            self.bits
                .get(index..end)
                .ok_or(InvalidData("Invalid bit range."))?
                .iter()
                .copied(),
        ))
    }
    pub fn set_range(
        &mut self,
        index: usize,
        length: usize,
        value: &Self,
    ) -> Result<(), InvalidData> {
        let end = index
            .checked_add(length)
            .ok_or(InvalidData("Offset overflow."))?;
        self.bits
            .get_mut(index..end)
            .ok_or(InvalidData("Invalid bit range."))?
            .copy_from_slice(
                value
                    .bits
                    .get(..length)
                    .ok_or(InvalidData("Invalid source bit range."))?,
            );
        Ok(())
    }
    pub fn get_next_range(&mut self, length: usize) -> Result<Self, InvalidData> {
        let value = self.get_range(self.position, length)?;
        self.position += length;
        Ok(value)
    }
    pub fn set_next_range(&mut self, length: usize, value: &Self) -> Result<(), InvalidData> {
        self.set_range(self.position, length, value)?;
        self.position += length;
        Ok(())
    }
    pub fn to_bytes(&self) -> Vec<u8> {
        self.bits
            .chunks_exact(8)
            .map(|chunk| {
                chunk
                    .iter()
                    .enumerate()
                    .fold(0, |byte, (bit, set)| byte | (u8::from(*set) << bit))
            })
            .collect()
    }
    pub fn append_byte(&mut self, value: u8) {
        self.bits.extend((0..8).map(|bit| value & (1 << bit) != 0));
    }
    pub fn get_string(
        &self,
        index: usize,
        length: usize,
        decode: impl FnOnce(&[u8]) -> String,
    ) -> Result<String, InvalidData> {
        Ok(decode(
            &self
                .get_range(
                    index,
                    length
                        .checked_mul(8)
                        .ok_or(InvalidData("Length overflow."))?,
                )?
                .to_bytes(),
        ))
    }
    pub fn set_string(
        &mut self,
        index: usize,
        length: usize,
        value: &str,
        encode: impl FnOnce(&str) -> Vec<u8>,
    ) -> Result<(), InvalidData> {
        let bytes = encode(value);
        let chars = value.encode_utf16().count();
        for offset in 0..length {
            self.set_uint(
                0,
                index + offset * 8,
                8,
                if offset < chars {
                    u32::from(
                        *bytes
                            .get(offset)
                            .ok_or(InvalidData("Encoded string is too short."))?,
                    )
                } else {
                    0
                },
            )?;
        }
        Ok(())
    }
}

/// Allocation-free equivalent for reading a field directly from a save buffer.
pub fn bits(data: &[u8], offset: usize, width: usize) -> Result<u32, InvalidData> {
    let end = offset
        .checked_add(width)
        .ok_or(InvalidData("Offset overflow."))?;
    if width > 32 || end > data.len().saturating_mul(8) {
        return Err(InvalidData("Invalid bit range."));
    }
    Ok((0..width).fold(0, |value, bit| {
        value | (u32::from((data[(offset + bit) / 8] >> ((offset + bit) % 8)) & 1) << bit)
    }))
}
