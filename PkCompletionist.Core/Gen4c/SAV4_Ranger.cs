using PkCompletionist.Core;
using System;
using System.IO;
namespace PkCompletionist.Core;

/*
 * facts:
 * - once decrypted, caught pokemon is allocated at 896D
 * - the order is the national dex (bul, ivy, venu, charmander), even if in ranger, charmander is R-140
 * 
 
 TODO:
    rewrite rangercrypt in c#
    assocaited R-XXX with national dex (internal id)
 
 */

class SAV4_Ranger : SAV_Dummy
{
    public SAV4_Ranger(byte[] data) : base(data)
    {
        Decrypt();
    }
    public override string OT => "RANGER";

    static public SAV4_Ranger? NewIfValid(byte[] data, string Hint)
    {
        if (data[0] == 0x50 &&
            data[1] == 0x4B &&
            data[2] == 0x52 &&
            data[3] == 0x2D &&
            data[4] == 0x30 &&
            data[5] == 0x35 &&
            data[6] == 0x39 &&
            data[7] == 0x00 &&
            data[8] == 0x61 &&
            data[9] == 0x30 &&
            data[10] == 0x30)
          return new SAV4_Ranger(data);

      return null;
    }
    public void Decrypt()
    {
        var bytes = this.Data;

        var size = bytes.Length / sizeof(int);
        var ints = new UInt32[size];
        for (var index = 0; index < size; index++)
            ints[index] = BitConverter.ToUInt32(bytes, index * sizeof(int));

        var saveinfo = new ArraySegment<uint>(ints, 0xC / 4, 0x1F4 / 4);
        if (saveinfo[1] >> 16 != 0)
            RangerCrypto(saveinfo, 0x7D);
        else
        {
            throw new Exception("else");
            //NO_PROD
            /*
            var saveinfo2 = new ArraySegment<uint>(ints, 0xC, 0x1F4);
            saveinfo[1] = CRC16((char*)tmp+8, 0x1EC);

            RangerCrypto(tmp, 0x7D);
            fseek(sav, 0xC, SEEK_SET);
            fwrite(tmp, 1, 0x1F4, sav);
            free(tmp);*/
        }

        var saveinfo2 = new ArraySegment<uint>(ints, 0x20C / 4, 0x1F4 / 4);

        if (saveinfo2[1] >> 16 == 0) // if decrypted fix checksum before encrypting
        {
            //error, but continue anyway
            //throw new Exception("else2");
        }

        RangerCrypto(saveinfo2, 0x7D);

        int c;
        for (c = 0; c < 10; c++)
        {
            uint chunknumber = saveinfo[0xC + (c * 0xC)] >> 16 & 0xFF;
            if (chunknumber == 0)
                continue;
            uint chunkoffset = saveinfo[0xD + (c * 0xC)];
            uint datasize = saveinfo[0xE + (c * 0xC)];
            uint chunksize = saveinfo[0xF + (c * 0xC)] / chunknumber;

            if (chunkoffset != 0 && chunksize <= 0x40000)
            {
                int i;
                for (i = 0; (i < chunknumber) && (i < 3); i++)
                {
                    var start = chunkoffset + (chunksize * i);
                    var chunk = new ArraySegment<uint>(ints, (int)start / 4, (int)chunksize / 4);

                    if (chunk[1] >> 16 == 0 && (chunk[1] & 0xFFFF) != 0) // if decrypted fix checksum before encrypting
                    {
                        //error, but continue anyway
                        //throw new Exception("else3");
                    }

                    RangerCrypto(chunk, (int)((datasize + 0x18) / 4));
                }
            }
        }

        for (var index = 0; index < size; index++)
        {
            this.Data[index * 4 + 0] = (byte)((ints[index] & 0x000000FF) >> 0);
            this.Data[index * 4 + 1] = (byte)((ints[index] & 0x0000FF00) >> 8);
            this.Data[index * 4 + 2] = (byte)((ints[index] & 0x00FF0000) >> 16);
            this.Data[index * 4 + 3] = (byte)((ints[index] & 0xFF000000) >> 24);
        }
    }
    public static void RangerCrypto(ArraySegment<uint> data, int size)
    {
        int i; // r5
        uint rand; // ST04_4
        uint h_rand; // r0
        uint s_rand = data[0]; // the initial seed is located before the encrypted data

        for (i = 1; i < size; i++)
        {
            rand = 1566083941 * s_rand;
            h_rand = rand;
            s_rand = 1566083941 * rand;
            data[i] ^= (h_rand & 0xFFFF0000) | (s_rand >> 16);
        }
    }
    static uint[] crc16_table = {
        0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
        0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
        0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
        0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
        0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,
        0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
        0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
        0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
        0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
        0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
        0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
        0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
        0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
        0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
        0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
        0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
        0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
        0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
        0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
        0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
        0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
        0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
        0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
        0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
        0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
        0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
        0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
        0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
        0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
        0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
        0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
        0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040
    };

    public static uint CRC16(uint[] data, int size)
    {
        uint chk = 0;
        for (int i = 0; i < size; i++)
        {
            chk = (crc16_table[(data[i] ^ chk) & 0xFF] ^ chk >> 8);
        }
        return chk;
    }

}