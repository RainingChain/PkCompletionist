using System;

namespace PKHeX.Core;

/// <summary>
/// <see cref="PersonalInfo"/> table (array).
/// </summary>
/// <remarks>
/// Serves as the main object that is accessed for stat data in a particular generation/game format.
/// </remarks>
public static class PersonalTable
{
    /// <summary>
    /// Personal Table used in <see cref="EntityContext.Gen9a"/>.
    /// </summary>
    public static readonly PersonalTable9ZA ZA = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="EntityContext.Gen9"/>.
    /// </summary>
    public static readonly PersonalTable9SV SV = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="EntityContext.Gen8a"/>.
    /// </summary>
    public static readonly PersonalTable8LA LA = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="EntityContext.Gen8b"/>.
    /// </summary>
    public static readonly PersonalTable8BDSP BDSP = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="EntityContext.Gen8"/>.
    /// </summary>
    public static readonly PersonalTable8SWSH SWSH = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="EntityContext.Gen7b"/>.
    /// </summary>
    public static readonly PersonalTable7GG GG = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.USUM"/>.
    /// </summary>
    public static readonly PersonalTable7 USUM = new(Memory<byte>.Empty, Legal.MaxSpeciesID_7_USUM);

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.SM"/>.
    /// </summary>
    public static readonly PersonalTable7 SM = new(Memory<byte>.Empty, Legal.MaxSpeciesID_7);

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.ORAS"/>.
    /// </summary>
    public static readonly PersonalTable6AO AO = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.XY"/>.
    /// </summary>
    public static readonly PersonalTable6XY XY = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.B2W2"/>.
    /// </summary>
    public static readonly PersonalTable5B2W2 B2W2 = new(GetTable("b2w2"));

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.BW"/>.
    /// </summary>
    public static readonly PersonalTable5BW BW = new(Memory<byte>.Empty);

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.HGSS"/>.
    /// </summary>
    public static readonly PersonalTable4 HGSS = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.Pt"/>.
    /// </summary>
    public static readonly PersonalTable4 Pt = new(GetTable("pt"));

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.DP"/>.
    /// </summary>
    public static readonly PersonalTable4 DP = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.LG"/>.
    /// </summary>
    public static readonly PersonalTable3 LG = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.FR"/>.
    /// </summary>
    public static readonly PersonalTable3 FR = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.E"/>.
    /// </summary>
    public static readonly PersonalTable3 E = new(GetTable("e"));

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.RS"/>.
    /// </summary>
    public static readonly PersonalTable3 RS = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.C"/>.
    /// </summary>
    public static readonly PersonalTable2 C = new(GetTable("c"));

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.GS"/>.
    /// </summary>
    public static readonly PersonalTable2 GS = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.RB"/>.
    /// </summary>
    public static readonly PersonalTable1 RB = new(Memory<byte>.Empty); // RC

    /// <summary>
    /// Personal Table used in <see cref="GameVersion.YW"/>.
    /// </summary>
    public static readonly PersonalTable1 Y = new(GetTable("y"));

    private static Memory<byte> GetTable(string game) => Util.GetBinaryResource($"personal_{game}");

    static PersonalTable() // Finish Setup
    {
        //PopulateGen3Tutors(); // RC
        //PopulateGen4Tutors(); // RC
    }

    private static void PopulateGen3Tutors()
    {
        // Enable Gen3 data with Emerald's data, FR/LG is a subset of Emerald's compatibility.
        var machine = BinLinkerAccessor.Get(Util.GetBinaryResource("hmtm_g3.pkl"), "g3"u8);
        var tutors = BinLinkerAccessor.Get(Util.GetBinaryResource("tutors_g3.pkl"), "g3"u8);
        E.LoadTables(machine, tutors);
        FR.CopyTables(E);
        LG.CopyTables(E);
        RS.CopyTables(E);
    }

    private static void PopulateGen4Tutors()
    {
        var tutors = BinLinkerAccessor.Get(Util.GetBinaryResource("tutors_g4.pkl"), "g4"u8);
        HGSS.LoadTables(tutors);
    }
}
