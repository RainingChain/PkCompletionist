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

        var flagDict = new Dictionary<string, int> {
         { "AARON", 0x549 },
         { "ABE", 0x3FC },
         { "AL", 0x53D },
         { "ALBERT", 0x5AB },
         { "ALEX", 0x4D8 },
         { "ALFRED", 0x49E },
         { "ALICE", 0x517 },
         { "ALLAN", 0x5C8 },
         { "ALLEN", 0x55A },
         { "AMYANDMAY1", 0x464 },
         { "AMYANDMAY2", 0x467 },
         { "ANDRE", 0x455 },
         { "ANDREW", 0x4E5 },
         { "ANNANDANNE1", 0x465 },
         { "ANNANDANNE2", 0x466 },
         { "ARNOLD", 0x450 },
         { "BAILEY", 0x52E },
         { "BARNEY", 0x45C },
         { "BARRY", 0x41E },
         { "BEN", 0x4E8 },
         { "BENJAMIN", 0x529 },
         { "BERKE", 0x599 },
         { "BEVERLY1", 0x4D9 },
         { "BILL", 0x44A },
         { "BILLY", 0x474 },
         { "BLAINE1", 0x4CB },
         { "BLAKE", 0x552 },
         { "BLUE1", 0x4CC },
         { "BOB", 0x405 },
         { "BORIS", 0x404 },
         { "BRAD", 0x40F },
         { "BRANDON", 0x4D3 },
         { "BRET", 0x40A },
         { "BRIAN", 0x553 },
         { "BRIANA", 0x3FA },
         { "BRIDGET", 0x516 },
         { "BROCK1", 0x4C5 },
         { "BROOKE", 0x480 },
         { "BRUNO1", 0x5BA },
         { "BRYAN", 0x3FD },
         { "BUG_CATCHER_BENNY", 0x53C },
         { "BUGSY1", 0x4BE },
         { "BURT", 0x449 },
         { "CALVIN", 0x4E6 },
         { "CAMERON", 0x5A4 },
         { "CARA", 0x5BE },
         { "CAROL", 0x567 },
         { "CARRIE", 0x515 },
         { "CARTER", 0x4D1 },
         { "CASSIE", 0x4B2 },
         { "CHARLES", 0x436 },
         { "CHARLIE", 0x597 },
         { "CHOW", 0x411 },
         { "CHUCK1", 0x4C2 },
         { "CINDY", 0x482 },
         { "CLAIR1", 0x4C4 },
         { "CLARISSA", 0x593 },
         { "CLYDE", 0x493 },
         { "CODY", 0x54B },
         { "COLETTE", 0x5B5 },
         { "COLIN", 0x4D5 },
         { "CONNIE1", 0x519 },
         { "COREY", 0x42F },
         { "CYBIL", 0x56A },
         { "DANIEL", 0x535 },
         { "DANNY", 0x470 },
         { "DARIN", 0x5BD },
         { "DAWN", 0x3F3 },
         { "DEAN", 0x420 },
         { "DEBRA", 0x485 },
         { "DENIS", 0x400 },
         { "DENISE", 0x3EC },
         { "DEREK1", 0x4CE },
         { "DIANA", 0x3F9 },
         { "DIRK", 0x547 },
         { "DON", 0x538 },
         { "DONALD", 0x4EF },
         { "DORIS", 0x591 },
         { "DOUG", 0x543 },
         { "DOUGLAS", 0x410 },
         { "DUDLEY", 0x472 },
         { "DUNCAN", 0x42D },
         { "DWAYNE", 0x433 },
         { "ED", 0x53A },
         { "EDDIE", 0x42E },
         { "EDGAR", 0x458 },
         { "EDMOND", 0x417 },
         { "EDNA", 0x48B },
         { "EDWARD", 0x49B },
         { "ELAINE", 0x3E8 },
         { "ELLEN", 0x51F },
         { "ELLIOT", 0x41D },
         { "EMMA", 0x569 },
         { "ERIC", 0x582 },
         { "ERIK", 0x52A },
         { "ERIKA1", 0x4C8 },
         { "ERNEST", 0x579 },
         { "ETHAN", 0x4EB },
         { "EUGENE", 0x575 },
         { "EUSINE", 0x333 },
         { "EXECUTIVEF_1", 0x56F },
         { "EXECUTIVEF_2", 0x570 },
         { "EXECUTIVEM_1", 0x571 },
         { "EXECUTIVEM_2", 0x572 },
         { "EXECUTIVEM_3", 0x573 },
         { "EXECUTIVEM_4", 0x574 },
         { "FALKNER1", 0x4BD },
         { "FIDEL", 0x43D },
         { "FRAN", 0x55D },
         { "FRANKLIN", 0x43B },
         { "FRITZ", 0x496 },
         { "GAKU", 0x5C9 },
         { "GARRETT", 0x57B },
         { "GEORGE", 0x598 },
         { "GEORGIA", 0x4DD },
         { "GILBERT", 0x443 },
         { "GLENN", 0x439 },
         { "GORDON", 0x5AC },
         { "GRACE", 0x58C },
         { "GREG", 0x43E },
         { "GREGORY", 0x49C },
         { "GRUNTF_1", 0x510 },
         { "GRUNTF_2", 0x511 },
         { "GRUNTF_3", 0x512 },
         { "GRUNTF_4", 0x513 },
         { "GRUNTF_5", 0x514 },
         { "GRUNTM_1", 0x4F1 },
         { "GRUNTM_10", 0x4FA },
         { "GRUNTM_11", 0x4FB },
         { "GRUNTM_13", 0x4FD },
         { "GRUNTM_14", 0x4FE },
         { "GRUNTM_15", 0x4FF },
         { "GRUNTM_16", 0x500 },
         { "GRUNTM_17", 0x501 },
         { "GRUNTM_18", 0x502 },
         { "GRUNTM_19", 0x503 },
         { "GRUNTM_2", 0x4F2 },
         { "GRUNTM_20", 0x504 },
         { "GRUNTM_21", 0x505 },
         { "GRUNTM_24", 0x508 },
         { "GRUNTM_25", 0x509 },
         { "GRUNTM_28", 0x50C },
         { "GRUNTM_29", 0x50D },
         { "GRUNTM_3", 0x4F3 },
         { "GRUNTM_4", 0x4F4 },
         { "GRUNTM_5", 0x4F5 },
         { "GRUNTM_6", 0x4F6 },
         { "GRUNTM_7", 0x4F7 },
         { "GRUNTM_8", 0x4F8 },
         { "GRUNTM_9", 0x4F9 },
         { "GWEN", 0x55B },
         { "HANK", 0x402 },
         { "HAROLD", 0x594 },
         { "HARRIS", 0x434 },
         { "HARRY", 0x57E },
         { "HEIDI", 0x48A },
         { "HENRY", 0x452 },
         { "HERMAN", 0x43C },
         { "HILLARY", 0x5B6 },
         { "HOPE", 0x483 },
         { "HORTON", 0x497 },
         { "HUGH", 0x5C5 },
         { "IAN", 0x5AE },
         { "IRENE", 0x560 },
         { "IRWIN1", 0x495 },
         { "ISSAC", 0x4EE },
         { "IVAN", 0x41C },
         { "JAIME", 0x5C2 },
         { "JAKE", 0x550 },
         { "JANINE1", 0x4C9 },
         { "JARED", 0x444 },
         { "JASMINE1", 0x4C1 },
         { "JASON", 0x5B4 },
         { "JED", 0x4A1 },
         { "JEFF", 0x57A },
         { "JEFFREY", 0x415 },
         { "JENN", 0x56B },
         { "JEREMY", 0x4D4 },
         { "JEROME", 0x5A1 },
         { "JERRY", 0x42B },
         { "JIM", 0x534 },
         { "JIMMY", 0x5B2 },
         { "JIN", 0x413 },
         { "JOANDZOE1", 0x468 },
         { "JOANDZOE2", 0x469 },
         { "JOE", 0x473 },
         { "JOEL", 0x438 },
         { "JOHNNY", 0x46F },
         { "JONAH", 0x459 },
         { "JOSH", 0x53E },
         { "JOSHUA", 0x4D0 },
         { "JOYCE", 0x562 },
         { "JULIA", 0x4BA },
         { "JUSTIN", 0x44E },
         { "KARA", 0x3ED },
         { "KAREN1", 0x5BB },
         { "KATE", 0x55F },
         { "KAYLEE", 0x3EA },
         { "KEITH", 0x546 },
         { "KELLY", 0x561 },
         { "KEN", 0x540 },
         { "KENJI3", 0x4AB },
         { "KENNETH", 0x57C },
         { "KENNY", 0x533 },
         { "KENT", 0x578 },
         { "KEVIN", 0x558 },
         { "KIM", 0x481 },
         { "KIPP", 0x46D },
         { "KIRK", 0x59A },
         { "KIYO", 0x4A9 },
         { "KOGA1", 0x5B9 },
         { "KOJI", 0x5CB },
         { "KRISE", 0x518 },
         { "KUNI", 0x4E2 },
         { "KYLE", 0x451 },
         { "LANCE", 0x5BC },
         { "LAO", 0x4A7 },
         { "LARRY", 0x4E4 },
         { "LAURA", 0x51B },
         { "LEAANDPIA1", 0x5BF },
         { "LEONARD", 0x527 },
         { "LI", 0x419 },
         { "LINDA", 0x51A },
         { "LLOYD", 0x41F },
         { "LOIS", 0x55C },
         { "LOLA", 0x55E },
         { "LORI", 0x3F6 },
         { "LT_SURGE1", 0x4C7 },
         { "LUNG", 0x4AA },
         { "LYLE", 0x44D },
         { "MARC", 0x4A2 },
         { "MARK", 0x440 },
         { "MARKUS", 0x5C6 },
         { "MARTHA", 0x58B },
         { "MARTIN", 0x45A },
         { "MARVIN", 0x453 },
         { "MASA", 0x5CA },
         { "MATHEW", 0x59B },
         { "MEGAN", 0x565 },
         { "MEGANDPEG1", 0x46A },
         { "MEGANDPEG2", 0x46B },
         { "MICHAEL", 0x52B },
         { "MICHELLE", 0x51D },
         { "MIKE", 0x54C },
         { "MIKEY", 0x5AA },
         { "MIKI", 0x4E3 },
         { "MILLER", 0x5C4 },
         { "MISTY1", 0x4C6 },
         { "MITCH", 0x4A0 },
         { "MORTY1", 0x4C0 },
         { "NAOKO", 0x4DF },
         { "NATE", 0x476 },
         { "NATHAN", 0x43A },
         { "NEAL", 0x418 },
         { "NICK", 0x548 },
         { "NICO", 0x412 },
         { "NICOLE", 0x3F5 },
         { "NIKKI", 0x3F8 },
         { "NOB", 0x4A8 },
         { "NOLAND", 0x531 },
         { "NORMAN", 0x43F },
         { "OLIVIA", 0x5C1 },
         { "OTIS", 0x446 },
         { "OWEN", 0x5B3 },
         { "PARKER", 0x5A8 },
         { "PAT", 0x588 },
         { "PAUL", 0x54A },
         { "PAULA", 0x3E9 },
         { "PERRY", 0x409 },
         { "PETER", 0x407 },
         { "PHIL", 0x441 },
         { "PHILLIP", 0x526 },
         { "PING", 0x416 },
         { "PRESTON", 0x49A },
         { "PRYCE1", 0x4C3 },
         { "QUENTIN", 0x5C3 },
         { "QUINN", 0x568 },
         { "RANDALL", 0x596 },
         { "RAY", 0x44C },
         { "RAYMOND", 0x456 },
         { "REBECCA", 0x590 },
         { "REX", 0x5C7 },
         { "RICH", 0x4A3 },
         { "RICHARD", 0x442 },
         { "RICKY", 0x477 },
         { "RILEY", 0x437 },
         { "ROB", 0x539 },
         { "ROBERT", 0x4CF },
         { "ROD", 0x3FB },
         { "RODNEY", 0x445 },
         { "ROLAND", 0x41A },
         { "RON", 0x4EA },
         { "RONALD", 0x40E },
         { "ROSS", 0x49F },
         { "ROXANNE", 0x592 },
         { "ROY", 0x403 },
         { "RUSSELL", 0x525 },
         { "RUTH", 0x4DA },
         { "RYAN", 0x54F },
         { "SABRINA1", 0x4CA },
         { "SAM", 0x586 },
         { "SAMANTHA", 0x4AE },
         { "SAMUEL", 0x5AD },
         { "SAYO", 0x4E0 },
         { "SCOTT", 0x462 },
         { "SEAN", 0x557 },
         { "SETH", 0x5A5 },
         { "SHANE", 0x4E7 },
         { "SHANNON", 0x51C },
         { "SHARON", 0x484 },
         { "SHAWN", 0x589 },
         { "SHIRLEY", 0x5B7 },
         { "SID", 0x421 },
         { "SIDNEY", 0x532 },
         { "SIMON", 0x595 },
         { "SPENCER", 0x42C },
         { "STAN", 0x581 },
         { "STANLY", 0x57D },
         { "STEPHEN", 0x45B },
         { "SUSIE", 0x3EB },
         { "TANYA", 0x490 },
         { "TED", 0x424 },
         { "TERRELL", 0x577 },
         { "TERU", 0x58A },
         { "THEO", 0x3FE },
         { "TIM", 0x530 },
         { "TIMOTHY", 0x52D },
         { "TOBY", 0x3FF },
         { "TOM", 0x587 },
         { "TOMMY", 0x471 },
         { "TREVOR", 0x4D2 },
         { "TROY", 0x414 },
         { "TUCKER", 0x5A2 },
         { "VALERIE", 0x4BC },
         { "VICTORIA", 0x4AD },
         { "VINCENT", 0x494 },
         { "WAI", 0x4AC },
         { "WALT", 0x44B },
         { "WARREN", 0x5B1 },
         { "WAYNE", 0x5C0 },
         { "WENDY", 0x3EE },
         { "WHITNEY1", 0x4BF },
         { "WILL1", 0x5B8 },
         { "WILLIAM", 0x4CD },
         { "YOSHI", 0x4A5 },
         { "ZACH", 0x4F0 },
         { "ZEKE", 0x435 },
         { "ZUKI", 0x4E1 },
        };


        /*
            wVanceFightCount
            wWiltonFightCount
        */

        // Rematch Logic:
        // After first battle, sets a regular EventFlag
        // If rematch, increments EventWork by 1
        // The rematch can be against the same pokemons as the first battle
        // In most cases, impossible to know if last battle is done

        var func = (List<string> list, string eventFlag, string eventWork) => {
            var eventFlagAddr = Cnst2.EventNameToAddress.GetValueOrDefault(eventFlag);
            if (eventFlagAddr == 0)
                throw new System.Exception("Invalid eventFlag " + eventFlag);

            var eventWorkAddr = Cnst2.EventWorkNameToAddress.GetValueOrDefault(eventWork);
            if (eventWorkAddr == 0)
                throw new System.Exception("Invalid eventFlag " + eventWork);

            ow[list[0]] = sav.GetEventFlag(eventFlagAddr);
            for (var i = 1; i < list.Count - 1; i++)
                ow[list[i]] = sav.GetWork(eventWorkAddr) >= i + 1;
            if (sav.GetWork(eventWorkAddr) < list.Count - 1)
                ow[list[list.Count - 1]] = false;
        };
          
        var func2 = (List<string> list, string eventFlag, string eventWork, string eventFlag2) => {
            var eventFlagAddr = Cnst2.EventNameToAddress.GetValueOrDefault(eventFlag);
            if (eventFlagAddr == 0)
                throw new System.Exception("Invalid eventFlag " + eventFlag);

            var eventFlagAddr2 = Cnst2.EventNameToAddress.GetValueOrDefault(eventFlag2);
            if (eventFlagAddr2 == 0)
                throw new System.Exception("Invalid eventFlag " + eventFlag2);

            var eventWorkAddr = Cnst2.EventWorkNameToAddress.GetValueOrDefault(eventWork);
            if (eventWorkAddr == 0)
                throw new System.Exception("Invalid eventFlag " + eventWork);


            ow[list[0]] = sav.GetEventFlag(eventFlagAddr);
            for (var i = 1; i < list.Count - 2; i++)
                ow[list[i]] = sav.GetWork(eventWorkAddr) >= i + 1;
            
            ow[list[list.Count - 1]] = sav.GetEventFlag(eventFlagAddr2);
        };

        func(new List<string>{"ALAN1", "ALAN2", "ALAN3", "ALAN4", "ALAN5"}, "EVENT_BEAT_SCHOOLBOY_ALAN", "wAlanFightCount");
        func(new List<string>{"ANTHONY2", "ANTHONY1", "ANTHONY3", "ANTHONY4", "ANTHONY5"}, "EVENT_BEAT_HIKER_ANTHONY", "wAnthonyFightCount");
        func(new List<string>{"ARNIE1", "ARNIE2", "ARNIE3", "ARNIE4", "ARNIE5"}, "EVENT_BEAT_BUG_CATCHER_ARNIE", "wArnieFightCount");
        func(new List<string>{"BETH1", "BETH2", "BETH3"}, "EVENT_BEAT_COOLTRAINERF_BETH", "wBethFightCount");        
        func(new List<string>{"BRENT1", "BRENT2", "BRENT3", "BRENT4"}, "EVENT_BEAT_POKEMANIAC_BRENT", "wBrentFightCount");        
        func(new List<string>{"CHAD1", "CHAD2", "CHAD3", "CHAD4", "CHAD5"}, "EVENT_BEAT_SCHOOLBOY_CHAD", "wChadFightCount");        
        func(new List<string>{"DANA1", "DANA2", "DANA3", "DANA4", "DANA5"}, "EVENT_BEAT_LASS_DANA", "wDanaFightCount");
        func(new List<string>{"GINA1", "GINA2", "GINA3", "GINA4", "GINA5"}, "EVENT_BEAT_PICNICKER_GINA", "wGinaFightCount");          
        func2(new List<string>{"HUEY1", "HUEY2", "HUEY3", "HUEY4"}, "EVENT_BEAT_SAILOR_HUEY", "wHueyFightCount","EVENT_GOT_PROTEIN_FROM_HUEY");
        func(new List<string>{"JACK1", "JACK2", "JACK3", "JACK4", "JACK5"}, "EVENT_BEAT_SCHOOLBOY_JACK", "wJackFightCount");
        func2(new List<string>{"JOEY1", "JOEY2", "JOEY3", "JOEY4", "JOEY5"}, "EVENT_BEAT_YOUNGSTER_JOEY", "wJoeyFightCount", "EVENT_JOEY_HP_UP");
        func(new List<string>{"LIZ1", "LIZ2", "LIZ3", "LIZ4", "LIZ5"}, "EVENT_BEAT_PICNICKER_LIZ", "wLizFightCount");
        func(new List<string>{"TIFFANY3", "TIFFANY1", "TIFFANY2", "TIFFANY4"}, "EVENT_BEAT_PICNICKER_TIFFANY", "wTiffanyFightCount");
        func(new List<string>{"TODD1", "TODD2", "TODD3", "TODD4", "TODD5"}, "EVENT_BEAT_CAMPER_TODD", "wToddFightCount");
        func(new List<string>{"TULLY3", "TULLY1", "TULLY2", "TULLY4"}, "EVENT_BEAT_FISHER_TULLY", "wTullyFightCount");
        func(new List<string>{"WADE1", "WADE2", "WADE3", "WADE4", "WADE5"}, "EVENT_BEAT_BUG_CATCHER_WADE", "wWadeFightCount");


        func2(new List<string>{"ERIN1", "ERIN2", "ERIN3"}, "EVENT_BEAT_PICNICKER_ERIN", "wErinFightCount", "EVENT_GOT_CALCIUM_FROM_ERIN");

        func(new List<string>{"GAVEN3", "GAVEN1", "GAVEN2"}, "EVENT_BEAT_COOLTRAINERM_GAVEN", "wGavenFightCount");
        func(new List<string>{"JOSE2", "JOSE1", "JOSE3"}, "EVENT_BEAT_BIRD_KEEPER_JOSE2", "wJoseFightCount");

        func2(new List<string>{"PARRY3", "PARRY1", "PARRY2"}, "EVENT_BEAT_HIKER_PARRY", "wParryFightCount","EVENT_GOT_IRON_FROM_PARRY");

        func(new List<string>{"REENA1", "REENA2", "REENA3"}, "EVENT_BEAT_COOLTRAINERF_REENA", "wReenaFightCount");
        func(new List<string>{"RALPH1", "RALPH2", "RALPH3", "RALPH4", "RALPH5"}, "EVENT_BEAT_FISHER_RALPH", "wRalphFightCount");

        func2(new List<string>{"VANCE1", "VANCE2", "VANCE3"}, "EVENT_BEAT_BIRD_KEEPER_VANCE", "wVanceFightCount","EVENT_GOT_CARBOS_FROM_VANCE");
        func(new List<string>{"WILTON1", "WILTON2", "WILTON3"}, "EVENT_BEAT_FISHER_WILTON", "wWiltonFightCount");
         
        foreach (var pair in flagDict)
            ow[pair.Key] = sav.GetEventFlag(pair.Value);

        if (sav.GetEventFlag(0x0044)) // BEAT_ELITE_FOUR
        {
            ow["GRUNTM_31"] = true;

            if (sav.GetEventFlag(0x1B)) // EVENT_GOT_CYNDAQUIL_FROM_ELM
            {
                ow["RIVAL1_1_TOTODILE"] = true;
                ow["RIVAL1_2_TOTODILE"] = true;
                ow["RIVAL1_3_TOTODILE"] = true;
                ow["RIVAL1_4_TOTODILE"] = true;
                ow["RIVAL1_5_TOTODILE"] = true;
                ow["RIVAL2_1_TOTODILE"] = true;
                ow["RIVAL2_2_TOTODILE"] = true;
            }

            if (sav.GetEventFlag(0x1C)) // EVENT_GOT_TOTODILE_FROM_ELM
            {
                ow["RIVAL1_1_CHIKORITA"] = true;
                ow["RIVAL1_2_CHIKORITA"] = true;
                ow["RIVAL1_3_CHIKORITA"] = true;
                ow["RIVAL1_4_CHIKORITA"] = true;
                ow["RIVAL1_5_CHIKORITA"] = true;
                ow["RIVAL2_1_CHIKORITA"] = true;
                ow["RIVAL2_2_CHIKORITA"] = true;
            }

            if (sav.GetEventFlag(0x1D)) // EVENT_GOT_CHIKORITA_FROM_ELM
            {
                ow["RIVAL1_1_CYNDAQUIL"] = true;
                ow["RIVAL1_2_CYNDAQUIL"] = true;
                ow["RIVAL1_3_CYNDAQUIL"] = true;
                ow["RIVAL1_4_CYNDAQUIL"] = true;
                ow["RIVAL1_5_CYNDAQUIL"] = true;
                ow["RIVAL2_1_CYNDAQUIL"] = true;
                ow["RIVAL2_2_CYNDAQUIL"] = true;
            }
        }
        


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