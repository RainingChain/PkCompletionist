using System;

namespace PKHeX.Core;

public sealed class UnityTower5 : SaveBlock<SAV5>
{
    private const int CountryCount = 232;

    private static ReadOnlySpan<byte> LegalCountries => new byte[]
    {
        001, 002, 003, 006, 008, 009, 012, 013, 015, 016, 017, 018, 020, 021, 022, 023,
        025, 027, 028, 029, 031, 033, 034, 035, 036, 040, 042, 043, 045, 047, 048, 049,
        051, 053, 054, 058, 060, 061, 062, 063, 064, 071, 072, 073, 074, 076, 079, 080,
        081, 082, 083, 084, 085, 087, 088, 090, 091, 092, 093, 094, 095, 096, 098, 099,
        101, 102, 103, 105, 106, 109, 111, 115, 117, 118, 121, 125, 128, 130, 132, 134,
        138, 139, 141, 145, 147, 148, 149, 150, 151, 155, 156, 157, 160, 161, 163, 164,
        166, 167, 170, 173, 174, 181, 185, 186, 188, 189, 190, 191, 194, 195, 196, 198,
        199, 200, 201, 203, 205, 206, 210, 211, 215, 217, 218, 219, 220, 221, 222, 224,
        226, 227,
    };

    public static byte GetSubregionCount(byte country) => country switch
    {
        009 => 24,
        012 => 8,
        028 => 27,
        036 => 13,
        043 => 33,
        072 => 6,
        073 => 22,
        079 => 16,
        095 => 35,
        102 => 20,
        105 => 50,
        155 => 22,
        166 => 16,
        174 => 8,
        195 => 17,
        200 => 22,
        218 => 12,
        220 => 51,
        _ => 0,
    };

    public enum Point
    {
        None = 0, // never communicated with
        Blue = 1, // first communicated with today
        Yellow = 2, // already communicated with
        Red = 3, // own registered location
    }

    public UnityTower5(SAV5BW SAV, int offset) : base(SAV) => Offset = offset;
    public UnityTower5(SAV5B2W2 SAV, int offset) : base(SAV) => Offset = offset;

    private const int UnityTowerOffset = 0x320;
    private const int GeonetGlobalFlagOffset = 0x344;
    private const int UnityTowerFlagOffset = 0x345;
    private const int GeonetOffset = 0x348;

    public bool GlobalFlag { get => Data[Offset + GeonetGlobalFlagOffset] != 0; set => Data[Offset + GeonetGlobalFlagOffset] = (byte)(value ? 1 : 0); }
    public bool UnityTowerFlag { get => Data[Offset + UnityTowerFlagOffset] != 0; set => Data[Offset + UnityTowerFlagOffset] = (byte)(value ? 1 : 0); }

    public void SetCountrySubregion(byte country, byte subregion, Point point)
    {
        int index = Offset + GeonetOffset + ((country - 1) * 16) + (subregion / 4);
        int shift = 2 * (subregion % 4);
        Data[index] = (byte)((Data[index] & ~(0b11 << shift)) | ((int)point << shift));
    }

    public void SetUnityTowerFloor(byte country, bool unlocked)
    {
        int index = Offset + UnityTowerOffset + (country / 8);
        int shift = country % 8;
        Data[index] = (byte)((Data[index] & ~(0b1 << shift)) | (unlocked ? 0b1 : 0b0) << shift);
    }

    private void SetAllSubregions(byte country, Point type, bool floor)
    {
        SetUnityTowerFloor(country, floor);

        var subregionCount = GetSubregionCount(country);
        if (subregionCount == 0)
        {
            SetCountrySubregion(country, 0, type);
            return;
        }

        for (byte subregion = 1; subregion <= subregionCount; subregion++)
            SetCountrySubregion(country, subregion, type);
    }

    public void SetAll()
    {
        for (byte country = 1; country <= CountryCount; country++)
            SetAllSubregions(country, Point.Yellow, true);

        if (SAV.Country > 0)
            SetCountrySubregion((byte)SAV.Country, (byte)SAV.Region, Point.Red);

        GlobalFlag = true;
        UnityTowerFlag = true;
    }

    public void SetAllLegal()
    {
        foreach (var country in LegalCountries)
            SetAllSubregions(country, Point.Yellow, true);

        if (SAV.Country > 0)
            SetCountrySubregion((byte)SAV.Country, (byte)SAV.Region, Point.Red);

        GlobalFlag = true;
        UnityTowerFlag = true;
    }

    public void ClearAll()
    {
        for (byte country = 1; country <= CountryCount; country++)
            SetAllSubregions(country, Point.None, false);

        if (SAV.Country > 0)
            SetCountrySubregion((byte)SAV.Country, (byte)SAV.Region, Point.Red);

        GlobalFlag = (SAV.Country > 0 && SAV.Country != 103);
        UnityTowerFlag = false;
    }
}
