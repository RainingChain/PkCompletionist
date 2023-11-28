using PKHeX.Core;
using System.Collections.Generic;
using System.Linq;

namespace PkCompletionist.Core;
internal class CompletionValidator2 : CompletionValidatorX
{
    public CompletionValidator2(Command command, SAV2 sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;
        this.unobtainableItems = new List<int>() {6,25,45,50,56,90,100,120,135,136,137,141,142,145,147,148,149,153,154,155,162,171,176,179,190,195,220,250,251,252,253,254,255 };
    }

    new SAV2 sav;

    public override void GenerateAll()
    {
        base.GenerateAll();

        Generate_inGameGift();
        Generate_misc();
        Generate_inGameTrade();
        Generate_decoration();
        Generate_phone();
        Generate_trainerBattle(); 
        Generate_pokegear();
        Generate_pokemonForm();
    }

    public override void Generate_item()
    {
        base.Generate_item();

        var ow = this.owned["item"];

        var hasAllBadges = (sav.Badges & 0b1111111111111111) == 0b1111111111111111;
        if (hasAllBadges)
        {
            ow["66"] = true; // Red Scale
            ow["67"] = true; // SecretPotion
            ow["128"] = true; // Machine Part
        }

        if (ow["134"]) // Pass
            ow["130"] = true; // Lost Item

        if (sav.GetEventFlag(0x001E))
            ow["69"] = true; // Mystery Egg

        if (HasPkmWithTID(123)) // Only way to get Scyther is with Park Ball
            ow["177"] = true; // Park Ball

        if (this.living)
            return;

        if (sav.GetEventFlag(718)) // Silver Trophy
            ow["167"] = true; // Normal Box

        if (sav.GetEventFlag(717)) // Gold Trophy
            ow["168"] = true; // Gorgeous Box

        if (sav.GetEventFlag(0x00DE))
            ow["172"] = true; // Up-Grade

        if (sav.GetEventFlag(0x0073))
            ow["82"] = true; // King's Rock

        if (sav.GetEventFlag(0x0071))
            ow["143"] = true; // Metal Coat

        if (sav.GetEventFlag(0x0683))
            ow["151"] = true; // Dragon Scale

        if (sav.GetEventFlag(0x0324))
            ow["23"] = true; // Thunderstone from Bill's grandfather

        if (sav.GetEventFlag(0x0321))
            ow["34"] = true; // Leaf Stone from Bill's grandfather

        if (HasPkmWithTID(251)) // Celebi
            ow["115"] = true; // GS Ball
    }

    public int GetUnownFormCount()
    {
        var count = 0;
        for (byte i = 0; i <= 25; i++)
            if (HasUnownForm(i))
                count++;
        return count;
    }

    public bool HasUnownForm(byte form)
    {
        return HasPkmForm(201, form);
    }

    public void Generate_pokemonForm()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokemonForm"] = ow;

        ow["UnownA"] = HasUnownForm(0);
        ow["UnownB"] = HasUnownForm(1);
        ow["UnownC"] = HasUnownForm(2);
        ow["UnownD"] = HasUnownForm(3);
        ow["UnownE"] = HasUnownForm(4);
        ow["UnownF"] = HasUnownForm(5);
        ow["UnownG"] = HasUnownForm(6);
        ow["UnownH"] = HasUnownForm(7);
        ow["UnownI"] = HasUnownForm(8);
        ow["UnownJ"] = HasUnownForm(9);
        ow["UnownK"] = HasUnownForm(10);
        ow["UnownL"] = HasUnownForm(11);
        ow["UnownM"] = HasUnownForm(12);
        ow["UnownN"] = HasUnownForm(13);
        ow["UnownO"] = HasUnownForm(14);
        ow["UnownP"] = HasUnownForm(15);
        ow["UnownQ"] = HasUnownForm(16);
        ow["UnownR"] = HasUnownForm(17);
        ow["UnownS"] = HasUnownForm(18);
        ow["UnownT"] = HasUnownForm(19);
        ow["UnownU"] = HasUnownForm(20);
        ow["UnownV"] = HasUnownForm(21);
        ow["UnownW"] = HasUnownForm(22);
        ow["UnownX"] = HasUnownForm(23);
        ow["UnownY"] = HasUnownForm(24);
        ow["UnownZ"] = HasUnownForm(25);
    }

    public void Generate_misc()
    {
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;

        var work = sav.GetAllEventWork();
        if (work[0x002C] == 0 || work[0x002D] == 0 || work[0x002E] == 0 || work[0x002F] == 0) // if not visited battle tower
            ow["BeatBattleTower"] = false;

        ow["UnlockMysteryGiftoption"] = sav.Data[0xBE3] == 0x00; // 0xB51 in japan
        ow["NewMagikarpSizeRecord"] = sav.Data[0x2B76] >= 0x04; // if magikarp is 5'0 feet, 0x2B76 becomes 5. default value is 3.
                                                                // for 5'0: hp=4, atk=0, def=13, spa=4, spd=4, spe=12
        ow["PokedexUnownMode"] = GetUnownFormCount() >= 3;

        if (!sav.GetEventFlag(0x0762) && sav.GetEventFlag(0x0044)) // if red not in mt silver and elite four defeated
            ow["DefeatRed"] = true;
    }


    public void Generate_inGameGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameGift"] = ow;

        ow["Starter"] = true;
        ow["Togepi"] = sav.GetEventFlag(0x0054);
        ow["OddEgg"] = sav.GetEventFlag(0x033E);
        ow["Spearow"] = sav.GetEventFlag(0x0050);
        ow["Eevee"] = sav.GetEventFlag(0x004F);
        ow["Dratini"] = sav.GetEventFlag(0x00BD);
        ow["Shuckle"] = sav.GetEventFlag(0x0045);
        ow["Tyrogue"] = sav.GetEventFlag(0x0061);
    }

    public void Generate_phone()
    {
        var ow = new Dictionary<string, bool>();
        owned["phone"] = ow;

        ow["Mom"] = true;
        ow["ProfessorElm"] = true;
        ow["Bill"] = sav.GetEventFlag(0x004F); //BAD
        ow["Buena"] = sav.GetEventFlag(0x033C) || sav.GetEventFlag(0x029E);
        ow["PokefanBeverly"] = sav.GetEventFlag(0x0261);
        ow["PokefanDerek"] = sav.GetEventFlag(0x028D);

        ow["SchoolKidJack"] = sav.GetEventFlag(0x025F);
        ow["SailorHuey"] = sav.GetEventFlag(0x0263);
        ow["AceTrainerGaven"] = sav.GetEventFlag(0x026B);
        ow["CooltrainerBeth"] = sav.GetEventFlag(0x026D);
        ow["BirdKeeperJose"] = sav.GetEventFlag(0x026F);
        ow["AceTrainerReena"] = sav.GetEventFlag(0x0271);
        ow["YoungsterJoey"] = sav.GetEventFlag(0x0273);
        ow["BugCatcherWade"] = sav.GetEventFlag(0x0275);
        ow["FishermanRalph"] = sav.GetEventFlag(0x0277);
        ow["PicnickerLiz"] = sav.GetEventFlag(0x0279);
        ow["HikerAnthony"] = sav.GetEventFlag(0x027B);
        ow["CamperTodd"] = sav.GetEventFlag(0x027D);
        ow["PicnickerGina"] = sav.GetEventFlag(0x027F);
        ow["JugglerIrwin"] = sav.GetEventFlag(0x0281);
        ow["BugCatcherArnie"] = sav.GetEventFlag(0x0283);
        ow["SchoolKidAlan"] = sav.GetEventFlag(0x0285);
        ow["LassDana"] = sav.GetEventFlag(0x0289);
        ow["SchoolKidChad"] = sav.GetEventFlag(0x028B);
        ow["FishermanTully"] = sav.GetEventFlag(0x028F);
        ow["PokeManiacBrent"] = sav.GetEventFlag(0x0291);
        ow["PicnickerTiffany"] = sav.GetEventFlag(0x0293);
        ow["BirdKeeperVance"] = sav.GetEventFlag(0x0295);
        ow["FishermanWilton"] = sav.GetEventFlag(0x0297);
        ow["BlackBeltKenji"] = sav.GetEventFlag(0x0299);
        ow["HikerParry"] = sav.GetEventFlag(0x029B);
        ow["PicnickerErin"] = sav.GetEventFlag(0x029D);
    }


    public void Generate_trainerBattle()
    {
        var ow = new Dictionary<string, bool>();
        owned["trainerBattle"] = ow;

        var flagDict = new Dictionary<string, int> { { "FALKNER1", 0x4BD }, { "WHITNEY1", 0x4BF }, { "BUGSY1", 0x4BE }, { "MORTY1", 0x4C0 }, { "PRYCE1", 0x4C3 }, { "JASMINE1", 0x4C1 }, { "CHUCK1", 0x4C2 }, { "CLAIR1", 0x4C4 }, { "WILL1", 0x5B8 }, { "BRUNO1", 0x5BA }, { "KAREN1", 0x5BB }, { "KOGA1", 0x5B9 }, { "LANCE", 0x5BC }, { "BROCK1", 0x4C5 }, { "MISTY1", 0x4C6 }, { "LT_SURGE1", 0x4C7 }, { "ROSS", 0x49F }, { "MITCH", 0x4A0 }, { "JED", 0x4A1 }, { "MARC", 0x4A2 }, { "RICH", 0x4A3 }, { "ERIKA1", 0x4C8 }, { "JOEY1", 0x5A9 }, { "MIKEY", 0x5AA }, { "ALBERT", 0x5AB }, { "GORDON", 0x5AC }, { "SAMUEL", 0x5AD }, { "IAN", 0x5AE }, { "JOEY2", 0x5AF }, { "JOEY3", 0x5B0 }, { "WARREN", 0x5B1 }, { "JIMMY", 0x5B2 }, { "OWEN", 0x5B3 }, { "JASON", 0x5B4 }, { "JACK1", 0x46C }, { "KIPP", 0x46D }, { "ALAN1", 0x46E }, { "JOHNNY", 0x46F }, { "DANNY", 0x470 }, { "TOMMY", 0x471 }, { "DUDLEY", 0x472 }, { "JOE", 0x473 }, { "BILLY", 0x474 }, { "CHAD1", 0x475 }, { "NATE", 0x476 }, { "RICKY", 0x477 }, { "JACK2", 0x478 }, { "JACK3", 0x479 }, { "ALAN2", 0x47A }, { "ALAN3", 0x47B }, { "CHAD2", 0x47C }, { "CHAD3", 0x47D }, { "ROD", 0x3FB }, { "ABE", 0x3FC }, { "BRYAN", 0x3FD }, { "THEO", 0x3FE }, { "TOBY", 0x3FF }, { "DENIS", 0x400 }, { "VANCE1", 0x401 }, { "HANK", 0x402 }, { "ROY", 0x403 }, { "BORIS", 0x404 }, { "BOB", 0x405 }, { "JOSE1", 0x406 }, { "PETER", 0x407 }, { "JOSE2", 0x408 }, { "PERRY", 0x409 }, { "BRET", 0x40A }, { "JOSE3", 0x40B }, { "VANCE2", 0x40C }, { "VANCE3", 0x40D }, { "CARRIE", 0x515 }, { "BRIDGET", 0x516 }, { "ALICE", 0x517 }, { "KRISE", 0x518 }, { "CONNIE1", 0x519 }, { "LINDA", 0x51A }, { "LAURA", 0x51B }, { "SHANNON", 0x51C }, { "MICHELLE", 0x51D }, { "DANA1", 0x51E }, { "ELLEN", 0x51F }, { "DANA2", 0x522 }, { "DANA3", 0x523 }, { "JANINE1", 0x4C9 }, { "NICK", 0x548 }, { "AARON", 0x549 }, { "PAUL", 0x54A }, { "CODY", 0x54B }, { "MIKE", 0x54C }, { "GAVEN1", 0x551 }, { "GAVEN2", 0x54D }, { "RYAN", 0x54F }, { "JAKE", 0x550 }, { "GAVEN3", 0x54E }, { "BLAKE", 0x552 }, { "BRIAN", 0x553 }, { "SEAN", 0x557 }, { "KEVIN", 0x558 }, { "ALLEN", 0x55A }, { "DARIN", 0x5BD }, { "GWEN", 0x55B }, { "LOIS", 0x55C }, { "FRAN", 0x55D }, { "LOLA", 0x55E }, { "KATE", 0x55F }, { "IRENE", 0x560 }, { "KELLY", 0x561 }, { "JOYCE", 0x562 }, { "BETH1", 0x563 }, { "REENA1", 0x564 }, { "MEGAN", 0x565 }, { "BETH2", 0x566 }, { "CAROL", 0x567 }, { "QUINN", 0x568 }, { "EMMA", 0x569 }, { "CYBIL", 0x56A }, { "JENN", 0x56B }, { "BETH3", 0x56C }, { "REENA2", 0x56D }, { "REENA3", 0x56E }, { "CARA", 0x5BE }, { "VICTORIA", 0x4AD }, { "SAMANTHA", 0x4AE }, { "CASSIE", 0x4B2 }, { "JULIA", 0x4BA }, { "VALERIE", 0x4BC }, { "OLIVIA", 0x5C1 }, { "LARRY", 0x4E4 }, { "ANDREW", 0x4E5 }, { "CALVIN", 0x4E6 }, { "SHANE", 0x4E7 }, { "BEN", 0x4E8 }, { "BRENT1", 0x4E9 }, { "RON", 0x4EA }, { "ETHAN", 0x4EB }, { "BRENT2", 0x4EC }, { "BRENT3", 0x4ED }, { "ISSAC", 0x4EE }, { "DONALD", 0x4EF }, { "ZACH", 0x4F0 }, { "MILLER", 0x5C4 }, { "GRUNTM_1", 0x4F1 }, { "GRUNTM_2", 0x4F2 }, { "GRUNTM_3", 0x4F3 }, { "GRUNTM_4", 0x4F4 }, { "GRUNTM_5", 0x4F5 }, { "GRUNTM_6", 0x4F6 }, { "GRUNTM_7", 0x4F7 }, { "GRUNTM_8", 0x4F8 }, { "GRUNTM_9", 0x4F9 }, { "GRUNTM_10", 0x4FA }, { "GRUNTM_11", 0x4FB }, { "GRUNTM_13", 0x4FD }, { "GRUNTM_14", 0x4FE }, { "GRUNTM_15", 0x4FF }, { "GRUNTM_16", 0x500 }, { "GRUNTM_17", 0x501 }, { "GRUNTM_18", 0x502 }, { "GRUNTM_19", 0x503 }, { "GRUNTM_20", 0x504 }, { "GRUNTM_21", 0x505 }, { "GRUNTM_24", 0x508 }, { "GRUNTM_25", 0x509 }, { "GRUNTM_28", 0x50C }, { "GRUNTM_29", 0x50D }, { "PRESTON", 0x49A }, { "EDWARD", 0x49B }, { "GREGORY", 0x49C }, { "ALFRED", 0x49E }, { "ROXANNE", 0x592 }, { "CLARISSA", 0x593 }, { "COLETTE", 0x5B5 }, { "HILLARY", 0x5B6 }, { "SHIRLEY", 0x5B7 }, { "SABRINA1", 0x4CA }, { "DON", 0x538 }, { "ROB", 0x539 }, { "ED", 0x53A }, { "WADE1", 0x53B }, { "BUG_CATCHER_BENNY", 0x53C }, { "AL", 0x53D }, { "JOSH", 0x53E }, { "ARNIE1", 0x53F }, { "KEN", 0x540 }, { "WADE2", 0x541 }, { "WADE3", 0x542 }, { "DOUG", 0x543 }, { "ARNIE2", 0x544 }, { "ARNIE3", 0x545 }, { "WAYNE", 0x5C0 }, { "JUSTIN", 0x44E }, { "RALPH1", 0x44F }, { "ARNOLD", 0x450 }, { "KYLE", 0x451 }, { "HENRY", 0x452 }, { "MARVIN", 0x453 }, { "TULLY1", 0x454 }, { "ANDRE", 0x455 }, { "RAYMOND", 0x456 }, { "WILTON1", 0x457 }, { "EDGAR", 0x458 }, { "JONAH", 0x459 }, { "MARTIN", 0x45A }, { "STEPHEN", 0x45B }, { "BARNEY", 0x45C }, { "RALPH2", 0x45D }, { "RALPH3", 0x45E }, { "TULLY2", 0x45F }, { "TULLY3", 0x460 }, { "WILTON2", 0x461 }, { "SCOTT", 0x462 }, { "WILTON3", 0x463 }, { "RALPH4", 0x45D }, { "RALPH5", 0x45E }, { "HAROLD", 0x594 }, { "SIMON", 0x595 }, { "RANDALL", 0x596 }, { "CHARLIE", 0x597 }, { "GEORGE", 0x598 }, { "BERKE", 0x599 }, { "KIRK", 0x59A }, { "MATHEW", 0x59B }, { "JEROME", 0x5A1 }, { "TUCKER", 0x5A2 }, { "CAMERON", 0x5A4 }, { "SETH", 0x5A5 }, { "PARKER", 0x5A8 }, { "ELAINE", 0x3E8 }, { "PAULA", 0x3E9 }, { "KAYLEE", 0x3EA }, { "SUSIE", 0x3EB }, { "DENISE", 0x3EC }, { "KARA", 0x3ED }, { "WENDY", 0x3EE }, { "DAWN", 0x3F3 }, { "NICOLE", 0x3F5 }, { "LORI", 0x3F6 }, { "NIKKI", 0x3F8 }, { "DIANA", 0x3F9 }, { "BRIANA", 0x3FA }, { "EUGENE", 0x575 }, { "HUEY1", 0x576 }, { "TERRELL", 0x577 }, { "KENT", 0x578 }, { "ERNEST", 0x579 }, { "JEFF", 0x57A }, { "GARRETT", 0x57B }, { "KENNETH", 0x57C }, { "STANLY", 0x57D }, { "HARRY", 0x57E }, { "HUEY2", 0x57F }, { "HUEY3", 0x580 }, { "STAN", 0x581 }, { "ERIC", 0x582 }, { "SAM", 0x586 }, { "TOM", 0x587 }, { "PAT", 0x588 }, { "SHAWN", 0x589 }, { "TERU", 0x58A }, { "HUGH", 0x5C5 }, { "MARKUS", 0x5C6 }, { "CLYDE", 0x493 }, { "VINCENT", 0x494 }, { "ANTHONY1", 0x528 }, { "RUSSELL", 0x525 }, { "PHILLIP", 0x526 }, { "LEONARD", 0x527 }, { "ANTHONY2", 0x524 }, { "BENJAMIN", 0x529 }, { "ERIK", 0x52A }, { "MICHAEL", 0x52B }, { "PARRY1", 0x52C }, { "TIMOTHY", 0x52D }, { "BAILEY", 0x52E }, { "ANTHONY3", 0x52F }, { "TIM", 0x530 }, { "NOLAND", 0x531 }, { "SIDNEY", 0x532 }, { "KENNY", 0x533 }, { "JIM", 0x534 }, { "DANIEL", 0x535 }, { "PARRY2", 0x536 }, { "PARRY3", 0x537 }, { "DWAYNE", 0x433 }, { "HARRIS", 0x434 }, { "ZEKE", 0x435 }, { "CHARLES", 0x436 }, { "RILEY", 0x437 }, { "JOEL", 0x438 }, { "GLENN", 0x439 }, { "BLAINE1", 0x4CB }, { "DUNCAN", 0x42D }, { "EDDIE", 0x42E }, { "COREY", 0x42F }, { "OTIS", 0x446 }, { "BURT", 0x449 }, { "BILL", 0x44A }, { "WALT", 0x44B }, { "RAY", 0x44C }, { "LYLE", 0x44D }, { "IRWIN1", 0x495 }, { "FRITZ", 0x496 }, { "HORTON", 0x497 }, { "YOSHI", 0x4A5 }, { "LAO", 0x4A7 }, { "NOB", 0x4A8 }, { "KIYO", 0x4A9 }, { "LUNG", 0x4AA }, { "KENJI3", 0x4AB }, { "WAI", 0x4AC }, { "EXECUTIVEM_1", 0x571 }, { "EXECUTIVEM_2", 0x572 }, { "EXECUTIVEM_3", 0x573 }, { "EXECUTIVEM_4", 0x574 }, { "NATHAN", 0x43A }, { "FRANKLIN", 0x43B }, { "HERMAN", 0x43C }, { "FIDEL", 0x43D }, { "GREG", 0x43E }, { "NORMAN", 0x43F }, { "MARK", 0x440 }, { "PHIL", 0x441 }, { "RICHARD", 0x442 }, { "GILBERT", 0x443 }, { "JARED", 0x444 }, { "RODNEY", 0x445 }, { "LIZ1", 0x47E }, { "GINA1", 0x47F }, { "BROOKE", 0x480 }, { "KIM", 0x481 }, { "CINDY", 0x482 }, { "HOPE", 0x483 }, { "SHARON", 0x484 }, { "DEBRA", 0x485 }, { "GINA2", 0x486 }, { "ERIN1", 0x487 }, { "LIZ2", 0x488 }, { "LIZ3", 0x489 }, { "HEIDI", 0x48A }, { "EDNA", 0x48B }, { "GINA3", 0x48C }, { "TIFFANY1", 0x491 }, { "TIFFANY2", 0x48D }, { "ERIN2", 0x48F }, { "TANYA", 0x490 }, { "TIFFANY3", 0x48E }, { "ERIN3", 0x492 }, { "ROLAND", 0x41A }, { "TODD1", 0x41B }, { "IVAN", 0x41C }, { "ELLIOT", 0x41D }, { "BARRY", 0x41E }, { "LLOYD", 0x41F }, { "DEAN", 0x420 }, { "SID", 0x421 }, { "TED", 0x424 }, { "TODD2", 0x425 }, { "TODD3", 0x426 }, { "JERRY", 0x42B }, { "SPENCER", 0x42C }, { "QUENTIN", 0x5C3 }, { "EXECUTIVEF_1", 0x56F }, { "EXECUTIVEF_2", 0x570 }, { "CHOW", 0x411 }, { "NICO", 0x412 }, { "JIN", 0x413 }, { "TROY", 0x414 }, { "JEFFREY", 0x415 }, { "PING", 0x416 }, { "EDMOND", 0x417 }, { "NEAL", 0x418 }, { "LI", 0x419 }, { "GAKU", 0x5C9 }, { "MASA", 0x5CA }, { "KOJI", 0x5CB }, { "MARTHA", 0x58B }, { "GRACE", 0x58C }, { "REBECCA", 0x590 }, { "DORIS", 0x591 }, { "RONALD", 0x40E }, { "BRAD", 0x40F }, { "DOUGLAS", 0x410 }, { "WILLIAM", 0x4CD }, { "DEREK1", 0x4CE }, { "ROBERT", 0x4CF }, { "JOSHUA", 0x4D0 }, { "CARTER", 0x4D1 }, { "TREVOR", 0x4D2 }, { "BRANDON", 0x4D3 }, { "JEREMY", 0x4D4 }, { "COLIN", 0x4D5 }, { "ALEX", 0x4D8 }, { "REX", 0x5C7 }, { "ALLAN", 0x5C8 }, { "NAOKO", 0x4DF }, { "SAYO", 0x4E0 }, { "ZUKI", 0x4E1 }, { "KUNI", 0x4E2 }, { "MIKI", 0x4E3 }, { "AMYANDMAY1", 0x464 }, { "ANNANDANNE1", 0x465 }, { "ANNANDANNE2", 0x466 }, { "AMYANDMAY2", 0x467 }, { "JOANDZOE1", 0x468 }, { "JOANDZOE2", 0x469 }, { "MEGANDPEG1", 0x46A }, { "MEGANDPEG2", 0x46B }, { "LEAANDPIA1", 0x5BF }, { "BEVERLY1", 0x4D9 }, { "RUTH", 0x4DA }, { "GEORGIA", 0x4DD }, { "JAIME", 0x5C2 }, { "BLUE1", 0x4CC }, { "KEITH", 0x546 }, { "DIRK", 0x547 }, { "GRUNTF_1", 0x510 }, { "GRUNTF_2", 0x511 }, { "GRUNTF_3", 0x512 }, { "GRUNTF_4", 0x513 }, { "GRUNTF_5", 0x514 }, { "EUSINE", 0x333 } };

        foreach (var pair in flagDict)
            ow[pair.Key] = sav.GetEventFlag(pair.Value);

        ow["ALAN4"] = sav.GetEventFlag();
        ow["ALAN5"] = sav.GetEventFlag();
        ow["ANTHONY4"] = sav.GetEventFlag();
        ow["ANTHONY5"] = sav.GetEventFlag();
        ow["ARNIE4"] = sav.GetEventFlag();
        ow["ARNIE5"] = sav.GetEventFlag();
        ow["BRENT4"] = sav.GetEventFlag();
        ow["CHAD4"] = sav.GetEventFlag();
        ow["CHAD5"] = sav.GetEventFlag();
        ow["DANA4"] = sav.GetEventFlag();
        ow["DANA5"] = sav.GetEventFlag();
        ow["GINA4"] = sav.GetEventFlag();
        ow["GINA5"] = sav.GetEventFlag();
        ow["GRUNTM_31"] = sav.GetEventFlag();
        ow["HUEY4"] = sav.GetEventFlag();
        ow["JACK4"] = sav.GetEventFlag();
        ow["JACK5"] = sav.GetEventFlag();
        ow["JOEY4"] = sav.GetEventFlag();
        ow["JOEY5"] = sav.GetEventFlag();
        ow["LIZ4"] = sav.GetEventFlag();
        ow["LIZ5"] = sav.GetEventFlag();
        ow["RIVAL1_1_CHIKORITA"] = sav.GetEventFlag();
        ow["RIVAL1_1_CYNDAQUIL"] = sav.GetEventFlag();
        ow["RIVAL1_1_TOTODILE"] = sav.GetEventFlag();
        ow["RIVAL1_2_CHIKORITA"] = sav.GetEventFlag();
        ow["RIVAL1_2_CYNDAQUIL"] = sav.GetEventFlag();
        ow["RIVAL1_2_TOTODILE"] = sav.GetEventFlag();
        ow["RIVAL1_3_CHIKORITA"] = sav.GetEventFlag();
        ow["RIVAL1_3_CYNDAQUIL"] = sav.GetEventFlag();
        ow["RIVAL1_3_TOTODILE"] = sav.GetEventFlag();
        ow["RIVAL1_4_CHIKORITA"] = sav.GetEventFlag();
        ow["RIVAL1_4_CYNDAQUIL"] = sav.GetEventFlag();
        ow["RIVAL1_4_TOTODILE"] = sav.GetEventFlag();
        ow["RIVAL1_5_CHIKORITA"] = sav.GetEventFlag();
        ow["RIVAL1_5_CYNDAQUIL"] = sav.GetEventFlag();
        ow["RIVAL1_5_TOTODILE"] = sav.GetEventFlag();
        ow["RIVAL2_1_CHIKORITA"] = sav.GetEventFlag();
        ow["RIVAL2_1_CYNDAQUIL"] = sav.GetEventFlag();
        ow["RIVAL2_1_TOTODILE"] = sav.GetEventFlag();
        ow["RIVAL2_2_CHIKORITA"] = sav.GetEventFlag();
        ow["RIVAL2_2_CYNDAQUIL"] = sav.GetEventFlag();
        ow["RIVAL2_2_TOTODILE"] = sav.GetEventFlag();
        ow["TIFFANY4"] = sav.GetEventFlag();
        ow["TODD4"] = sav.GetEventFlag();
        ow["TODD5"] = sav.GetEventFlag();
        ow["TULLY4"] = sav.GetEventFlag();
        ow["WADE4"] = sav.GetEventFlag();
        ow["WADE5"] = sav.GetEventFlag();



    }

    public void Generate_pokegear()
    {
        var ow = new Dictionary<string, bool>();
        owned["pokegear"] = ow;

        ow["TownMap"] = true;
        ow["Radio"] = true;
    }

    

    public void Generate_inGameTrade()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameTrade"] = ow;

        ow["AbraforMachop"] = sav.GetFlag(0x24EE, 0);
        ow["BellsproutforOnix"] = sav.GetFlag(0x24EE, 1);
        ow["KrabbyforVoltorb"] = sav.GetFlag(0x24EE, 2);
        ow["DragonairforDodrio"] = sav.GetFlag(0x24EE, 3);
        ow["HaunterforXatu"] = sav.GetFlag(0x24EE, 4);
        ow["ChanseyforAerodactyl"] = sav.GetFlag(0x24EE, 5);
        ow["DugtrioforMagneton"] = sav.GetFlag(0x24EE, 6);
    }

    public void Generate_decoration()
    {
        var ow = new Dictionary<string, bool>();
        owned["decoration"] = ow;

        ow["TownMap"] = true; // PlayerRoomsPoster

        ow["FeatheryBed"] = true; //BED1
        ow["PinkBed"] = sav.GetEventFlag(677); //BED2
        ow["PolkadotBed"] = sav.GetEventFlag(678);  //BED3
        ow["RedCarpet"] = sav.GetEventFlag(680); //CARPET1
        ow["BlueCarpet"] = sav.GetEventFlag(681); //CARPET2
        ow["YellowCarpet"] = sav.GetEventFlag(682); //CARPET3
        ow["GreenCarpet"] = sav.GetEventFlag(683); //CARPET4
        ow["Magnaplant"] = sav.GetEventFlag(684); //PLANT1
        ow["Tropicplant"] = sav.GetEventFlag(685); //PLANT2
        ow["Jumboplant"] = sav.GetEventFlag(686); //PLANT3
        ow["PikachuPoster"] = sav.GetEventFlag(688); //DECO_POSTER_1
        ow["ClefairyPoster"] = sav.GetEventFlag(689); //DECO_POSTER_2
        ow["JigglypuffPoster"] = sav.GetEventFlag(690); //DECO_POSTER_3
        ow["NES"] = sav.GetEventFlag(691);
        ow["SuperNES"] = sav.GetEventFlag(692);
        ow["Nintendo64"] = sav.GetEventFlag(693);
        ow["VirtualBoy"] = sav.GetEventFlag(694);
        ow["BulbasaurDoll"] = sav.GetEventFlag(699);
        ow["CharmanderDoll"] = sav.GetEventFlag(700);
        ow["ClefairyDoll"] = sav.GetEventFlag(697);
        ow["DiglettDoll"] = sav.GetEventFlag(703);
        ow["GengarDoll"] = sav.GetEventFlag(707);
        ow["GeodudeDoll"] = sav.GetEventFlag(713);
        ow["GrimerDoll"] = sav.GetEventFlag(709);
        ow["JigglypuffDoll"] = sav.GetEventFlag(698);
        ow["MachopDoll"] = sav.GetEventFlag(714);
        ow["MagikarpDoll"] = sav.GetEventFlag(705);
        ow["OddishDoll"] = sav.GetEventFlag(706);
        ow["PikachuDoll"] = sav.GetEventFlag(695);
        ow["PoliwagDoll"] = sav.GetEventFlag(702);
        ow["ShellderDoll"] = sav.GetEventFlag(708);
        ow["SquirtleDoll"] = sav.GetEventFlag(701);
        ow["StaryuDoll"] = sav.GetEventFlag(704);
        ow["SurfPikachuDoll"] = sav.GetEventFlag(696);
        ow["VoltorbDoll"] = sav.GetEventFlag(710);
        ow["WeedleDoll"] = sav.GetEventFlag(711);
        ow["BigLaprasDoll"] = sav.GetEventFlag(721);
        ow["BigOnixDoll"] = sav.GetEventFlag(720);
        ow["BigSnorlaxDoll"] = sav.GetEventFlag(719);

        ow["SilverTrophy"] = sav.GetEventFlag(718);
        ow["GoldTrophy"] = sav.GetEventFlag(717);

    }
}