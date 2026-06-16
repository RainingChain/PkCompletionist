using PKHeX.Core;
using System.Collections.Generic;
using System;
using System.Buffers.Binary;

namespace PkCompletionist.Core;
partial class SAV_Dummy : SaveFile
{
    public SAV_Dummy(byte[] data) : base(data)
    {
    }

    public override ushort MaxSpeciesID => 0;
    public override string OT => "";
    public override GameVersion Version { get => GameVersion.Unknown; set { } }


    protected override void SetChecksums(){}

    public override IPersonalTable Personal => throw new NotImplementedException();

    protected internal override string ShortSummary => throw new NotImplementedException();

    public override string Extension => throw new NotImplementedException();

    public override bool ChecksumsValid => throw new NotImplementedException();

    public override string ChecksumInfo => throw new NotImplementedException();

    public override byte Generation => throw new NotImplementedException();

    public override EntityContext Context => throw new NotImplementedException();


    public override int MaxStringLengthNickname => throw new NotImplementedException();

    public override int MaxStringLengthTrainer => throw new NotImplementedException();

    public override ushort MaxMoveID => throw new NotImplementedException();


    public override int MaxAbilityID => throw new NotImplementedException();

    public override int MaxItemID => throw new NotImplementedException();

    public override int MaxBallID => throw new NotImplementedException();

    public override GameVersion MaxGameID => throw new NotImplementedException();

    public override int BoxCount => throw new NotImplementedException();

    public override Type PKMType => throw new NotImplementedException();

    public override PKM BlankPKM => throw new NotImplementedException();

    public override int SIZE_STORED => throw new NotImplementedException();

    public override int SIZE_PARTY => throw new NotImplementedException();

    public override int MaxEV => throw new NotImplementedException();

    public override ReadOnlySpan<ushort> HeldItems => throw new NotImplementedException();


    public override int GetPartyOffset(int slot)
    {
        throw new NotImplementedException();
    }


    protected override SaveFile CloneInternal()
    {
        throw new NotImplementedException();
    }

    public override string GetString(ReadOnlySpan<byte> data)
    {
        throw new NotImplementedException();
    }

    public override int LoadString(ReadOnlySpan<byte> data, Span<char> text)
    {
        throw new NotImplementedException();
    }

    public override int SetString(Span<byte> destBuffer, ReadOnlySpan<char> value, int maxLength, StringConverterOption option)
    {
        throw new NotImplementedException();
    }

    protected override PKM GetPKM(Memory<byte> data)
    {
        throw new NotImplementedException();
    }

    protected override void DecryptPKM(Span<byte> data)
    {
        throw new NotImplementedException();
    }

    public override int GetBoxOffset(int box)
    {
        throw new NotImplementedException();
    }

    public string GetBoxName(int box)
    {
        throw new NotImplementedException();
    }

    public void SetBoxName(int box, ReadOnlySpan<char> value)
    {
        throw new NotImplementedException();
    }
}
