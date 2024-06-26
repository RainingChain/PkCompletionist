﻿using System.Collections.Generic;

namespace PkCompletionist.Core;
internal class Cnst2
{
    public static Dictionary<string, int> EventNameToAddress = new Dictionary<string, int>
    {
        {"EVENT_GOT_PROTEIN_FROM_HUEY", 0x0265},
        {"EVENT_GOT_HP_UP_FROM_JOEY", 0x0266},
        {"EVENT_GOT_CARBOS_FROM_VANCE",0x0267},
        {"EVENT_GOT_IRON_FROM_PARRY", 0x0268},
        {"EVENT_GOT_CALCIUM_FROM_ERIN",0x0269},

        {"EVENT_BEAT_SWIMMERF_ELAINE", 0x03E8},
        {"EVENT_BEAT_SWIMMERF_PAULA", 0x03E9},
        {"EVENT_BEAT_SWIMMERF_KAYLEE", 0x03EA},
        {"EVENT_BEAT_SWIMMERF_SUSIE", 0x03EB},
        {"EVENT_BEAT_SWIMMERF_DENISE", 0x03EC},
        {"EVENT_BEAT_SWIMMERF_KARA", 0x03ED},
        {"EVENT_BEAT_SWIMMERF_WENDY", 0x03EE},
        {"EVENT_BEAT_SWIMMERF_LISA", 0x03EF},
        {"EVENT_BEAT_SWIMMERF_JILL", 0x03F0},
        {"EVENT_BEAT_SWIMMERF_MARY", 0x03F1},
        {"EVENT_BEAT_SWIMMERF_KATIE", 0x03F2},
        {"EVENT_BEAT_SWIMMERF_DAWN", 0x03F3},
        {"EVENT_BEAT_SWIMMERF_TARA", 0x03F4},
        {"EVENT_BEAT_SWIMMERF_NICOLE", 0x03F5},
        {"EVENT_BEAT_SWIMMERF_LORI", 0x03F6},
        {"EVENT_BEAT_SWIMMERF_JODY", 0x03F7},
        {"EVENT_BEAT_SWIMMERF_NIKKI", 0x03F8},
        {"EVENT_BEAT_SWIMMERF_DIANA", 0x03F9},
        {"EVENT_BEAT_SWIMMERF_BRIANA", 0x03FA},
        {"EVENT_BEAT_BIRD_KEEPER_ROD", 0x03FB},
        {"EVENT_BEAT_BIRD_KEEPER_ABE", 0x03FC},
        {"EVENT_BEAT_BIRD_KEEPER_BRYAN", 0x03FD},
        {"EVENT_BEAT_BIRD_KEEPER_THEO", 0x03FE},
        {"EVENT_BEAT_BIRD_KEEPER_TOBY", 0x03FF},
        {"EVENT_BEAT_BIRD_KEEPER_DENIS", 0x0400},
        {"EVENT_BEAT_BIRD_KEEPER_VANCE", 0x0401},
        {"EVENT_BEAT_BIRD_KEEPER_HANK", 0x0402},
        {"EVENT_BEAT_BIRD_KEEPER_ROY", 0x0403},
        {"EVENT_BEAT_BIRD_KEEPER_BORIS", 0x0404},
        {"EVENT_BEAT_BIRD_KEEPER_BOB", 0x0405},
        {"EVENT_BEAT_BIRD_KEEPER_JOSE", 0x0406},
        {"EVENT_BEAT_BIRD_KEEPER_PETER", 0x0407},
        {"EVENT_BEAT_BIRD_KEEPER_JOSE2", 0x0408},
        {"EVENT_BEAT_BIRD_KEEPER_PERRY", 0x0409},
        {"EVENT_BEAT_BIRD_KEEPER_BRET", 0x040A},
        {"EVENT_BEAT_BIRD_KEEPER_JOSE3", 0x040B},
        {"EVENT_BEAT_BIRD_KEEPER_VANCE2", 0x040C},
        {"EVENT_BEAT_BIRD_KEEPER_VANCE3", 0x040D},
        {"EVENT_BEAT_BOARDER_RONALD", 0x040E},
        {"EVENT_BEAT_BOARDER_BRAD", 0x040F},
        {"EVENT_BEAT_BOARDER_DOUGLAS", 0x0410},
        {"EVENT_BEAT_SAGE_CHOW", 0x0411},
        {"EVENT_BEAT_SAGE_NICO", 0x0412},
        {"EVENT_BEAT_SAGE_JIN", 0x0413},
        {"EVENT_BEAT_SAGE_TROY", 0x0414},
        {"EVENT_BEAT_SAGE_JEFFREY", 0x0415},
        {"EVENT_BEAT_SAGE_PING", 0x0416},
        {"EVENT_BEAT_SAGE_EDMOND", 0x0417},
        {"EVENT_BEAT_SAGE_NEAL", 0x0418},
        {"EVENT_BEAT_SAGE_LI", 0x0419},
        {"EVENT_BEAT_CAMPER_ROLAND", 0x041A},
        {"EVENT_BEAT_CAMPER_TODD", 0x041B},
        {"EVENT_BEAT_CAMPER_IVAN", 0x041C},
        {"EVENT_BEAT_CAMPER_ELLIOT", 0x041D},
        {"EVENT_BEAT_CAMPER_BARRY", 0x041E},
        {"EVENT_BEAT_CAMPER_LLOYD", 0x041F},
        {"EVENT_BEAT_CAMPER_DEAN", 0x0420},
        {"EVENT_BEAT_CAMPER_SID", 0x0421},
        {"EVENT_BEAT_CAMPER_HERVEY", 0x0422},
        {"EVENT_BEAT_CAMPER_DALE", 0x0423},
        {"EVENT_BEAT_CAMPER_TED", 0x0424},
        {"EVENT_BEAT_CAMPER_TODD2", 0x0425},
        {"EVENT_BEAT_CAMPER_TODD3", 0x0426},
        {"EVENT_BEAT_CAMPER_THOMAS", 0x0427},
        {"EVENT_BEAT_CAMPER_LEROY", 0x0428},
        {"EVENT_BEAT_CAMPER_DAVID", 0x0429},
        {"EVENT_BEAT_CAMPER_JOHN", 0x042A},
        {"EVENT_BEAT_CAMPER_JERRY", 0x042B},
        {"EVENT_BEAT_CAMPER_SPENCER", 0x042C},
        {"EVENT_BEAT_BURGLAR_DUNCAN", 0x042D},
        {"EVENT_BEAT_BURGLAR_EDDIE", 0x042E},
        {"EVENT_BEAT_BURGLAR_COREY", 0x042F},
        {"EVENT_BEAT_BIKER_BENNY", 0x0431},
        {"EVENT_BEAT_BIKER_KAZU", 0x0432},
        {"EVENT_BEAT_BIKER_DWAYNE", 0x0433},
        {"EVENT_BEAT_BIKER_HARRIS", 0x0434},
        {"EVENT_BEAT_BIKER_ZEKE", 0x0435},
        {"EVENT_BEAT_BIKER_CHARLES", 0x0436},
        {"EVENT_BEAT_BIKER_RILEY", 0x0437},
        {"EVENT_BEAT_BIKER_JOEL", 0x0438},
        {"EVENT_BEAT_BIKER_GLENN", 0x0439},
        {"EVENT_BEAT_PSYCHIC_NATHAN", 0x043A},
        {"EVENT_BEAT_PSYCHIC_FRANKLIN", 0x043B},
        {"EVENT_BEAT_PSYCHIC_HERMAN", 0x043C},
        {"EVENT_BEAT_PSYCHIC_FIDEL", 0x043D},
        {"EVENT_BEAT_PSYCHIC_GREG", 0x043E},
        {"EVENT_BEAT_PSYCHIC_NORMAN", 0x043F},
        {"EVENT_BEAT_PSYCHIC_MARK", 0x0440},
        {"EVENT_BEAT_PSYCHIC_PHIL", 0x0441},
        {"EVENT_BEAT_PSYCHIC_RICHARD", 0x0442},
        {"EVENT_BEAT_PSYCHIC_GILBERT", 0x0443},
        {"EVENT_BEAT_PSYCHIC_JARED", 0x0444},
        {"EVENT_BEAT_PSYCHIC_RODNEY", 0x0445},
        {"EVENT_BEAT_FIREBREATHER_OTIS", 0x0446},
        {"EVENT_BEAT_FIREBREATHER_DICK", 0x0447},
        {"EVENT_BEAT_FIREBREATHER_NED", 0x0448},
        {"EVENT_BEAT_FIREBREATHER_BURT", 0x0449},
        {"EVENT_BEAT_FIREBREATHER_BILL", 0x044A},
        {"EVENT_BEAT_FIREBREATHER_WALT", 0x044B},
        {"EVENT_BEAT_FIREBREATHER_RAY", 0x044C},
        {"EVENT_BEAT_FIREBREATHER_LYLE", 0x044D},
        {"EVENT_BEAT_FISHER_JUSTIN", 0x044E},
        {"EVENT_BEAT_FISHER_RALPH", 0x044F},
        {"EVENT_BEAT_FISHER_ARNOLD", 0x0450},
        {"EVENT_BEAT_FISHER_KYLE", 0x0451},
        {"EVENT_BEAT_FISHER_HENRY", 0x0452},
        {"EVENT_BEAT_FISHER_MARVIN", 0x0453},
        {"EVENT_BEAT_FISHER_TULLY", 0x0454},
        {"EVENT_BEAT_FISHER_ANDRE", 0x0455},
        {"EVENT_BEAT_FISHER_RAYMOND", 0x0456},
        {"EVENT_BEAT_FISHER_WILTON", 0x0457},
        {"EVENT_BEAT_FISHER_EDGAR", 0x0458},
        {"EVENT_BEAT_FISHER_JONAH", 0x0459},
        {"EVENT_BEAT_FISHER_MARTIN", 0x045A},
        {"EVENT_BEAT_FISHER_STEPHEN", 0x045B},
        {"EVENT_BEAT_FISHER_BARNEY", 0x045C},
        {"EVENT_BEAT_FISHER_RALPH2", 0x045D},
        {"EVENT_BEAT_FISHER_RALPH3", 0x045E},
        {"EVENT_BEAT_FISHER_TULLY2", 0x045F},
        {"EVENT_BEAT_FISHER_TULLY3", 0x0460},
        {"EVENT_BEAT_FISHER_WILTON2", 0x0461},
        {"EVENT_BEAT_FISHER_SCOTT", 0x0462},
        {"EVENT_BEAT_FISHER_WILTON3", 0x0463},
        {"EVENT_BEAT_TWINS_AMY_AND_MAY", 0x0464},
        {"EVENT_BEAT_TWINS_ANN_AND_ANNE", 0x0465},
        {"EVENT_BEAT_TWINS_ANN_AND_ANNE2", 0x0466},
        {"EVENT_BEAT_TWINS_AMY_AND_MAY2", 0x0467},
        {"EVENT_BEAT_TWINS_JO_AND_ZOE", 0x0468},
        {"EVENT_BEAT_TWINS_JO_AND_ZOE2", 0x0469},
        {"EVENT_BEAT_TWINS_MEG_AND_PEG", 0x046A},
        {"EVENT_BEAT_TWINS_MEG_AND_PEG2", 0x046B},
        {"EVENT_BEAT_SCHOOLBOY_JACK", 0x046C},
        {"EVENT_BEAT_SCHOOLBOY_KIP", 0x046D},
        {"EVENT_BEAT_SCHOOLBOY_ALAN", 0x046E},
        {"EVENT_BEAT_SCHOOLBOY_JOHNNY", 0x046F},
        {"EVENT_BEAT_SCHOOLBOY_DANNY", 0x0470},
        {"EVENT_BEAT_SCHOOLBOY_TOMMY", 0x0471},
        {"EVENT_BEAT_SCHOOLBOY_DUDLEY", 0x0472},
        {"EVENT_BEAT_SCHOOLBOY_JOE", 0x0473},
        {"EVENT_BEAT_SCHOOLBOY_BILLY", 0x0474},
        {"EVENT_BEAT_SCHOOLBOY_CHAD", 0x0475},
        {"EVENT_BEAT_SCHOOLBOY_NATE", 0x0476},
        {"EVENT_BEAT_SCHOOLBOY_RICKY", 0x0477},
        {"EVENT_BEAT_SCHOOLBOY_JACK2", 0x0478},
        {"EVENT_BEAT_SCHOOLBOY_JACK3", 0x0479},
        {"EVENT_BEAT_SCHOOLBOY_ALAN2", 0x047A},
        {"EVENT_BEAT_SCHOOLBOY_ALAN3", 0x047B},
        {"EVENT_BEAT_SCHOOLBOY_CHAD2", 0x047C},
        {"EVENT_BEAT_SCHOOLBOY_CHAD3", 0x047D},
        {"EVENT_BEAT_PICNICKER_LIZ", 0x047E},
        {"EVENT_BEAT_PICNICKER_GINA", 0x047F},
        {"EVENT_BEAT_PICNICKER_BROOKE", 0x0480},
        {"EVENT_BEAT_PICNICKER_KIM", 0x0481},
        {"EVENT_BEAT_PICNICKER_CINDY", 0x0482},
        {"EVENT_BEAT_PICNICKER_HOPE", 0x0483},
        {"EVENT_BEAT_PICNICKER_SHARON", 0x0484},
        {"EVENT_BEAT_PICNICKER_DEBRA", 0x0485},
        {"EVENT_BEAT_PICNICKER_GINA2", 0x0486},
        {"EVENT_BEAT_PICNICKER_ERIN", 0x0487},
        {"EVENT_BEAT_PICNICKER_LIZ2", 0x0488},
        {"EVENT_BEAT_PICNICKER_LIZ3", 0x0489},
        {"EVENT_BEAT_PICNICKER_HEIDI", 0x048A},
        {"EVENT_BEAT_PICNICKER_EDNA", 0x048B},
        {"EVENT_BEAT_PICNICKER_GINA3", 0x048C},
        {"EVENT_BEAT_PICNICKER_TIFFANY2", 0x048D},
        {"EVENT_BEAT_PICNICKER_TIFFANY3", 0x048E},
        {"EVENT_BEAT_PICNICKER_ERIN2", 0x048F},
        {"EVENT_BEAT_PICNICKER_TANYA", 0x0490},
        {"EVENT_BEAT_PICNICKER_TIFFANY", 0x0491},
        {"EVENT_BEAT_PICNICKER_ERIN3", 0x0492},
        {"EVENT_BEAT_GUITARIST_CLYDE", 0x0493},
        {"EVENT_BEAT_GUITARIST_VINCENT", 0x0494},
        {"EVENT_BEAT_JUGGLER_IRWIN", 0x0495},
        {"EVENT_BEAT_JUGGLER_FRITZ", 0x0496},
        {"EVENT_BEAT_JUGGLER_HORTON", 0x0497},
        {"EVENT_BEAT_JUGGLER_IRWIN2", 0x0498},
        {"EVENT_BEAT_JUGGLER_IRWIN3", 0x0499},
        {"EVENT_BEAT_GENTLEMAN_PRESTON", 0x049A},
        {"EVENT_BEAT_GENTLEMAN_EDWARD", 0x049B},
        {"EVENT_BEAT_GENTLEMAN_GREGORY", 0x049C},
        {"EVENT_BEAT_GENTLEMAN_VIRGIL", 0x049D},
        {"EVENT_BEAT_GENTLEMAN_ALFRED", 0x049E},
        {"EVENT_BEAT_SCIENTIST_ROSS", 0x049F},
        {"EVENT_BEAT_SCIENTIST_MITCH", 0x04A0},
        {"EVENT_BEAT_SCIENTIST_JED", 0x04A1},
        {"EVENT_BEAT_SCIENTIST_MARC", 0x04A2},
        {"EVENT_BEAT_SCIENTIST_RICH", 0x04A3},
        {"EVENT_BEAT_BLACKBELT_KENJI2", 0x04A4},
        {"EVENT_BEAT_BLACKBELT_YOSHI", 0x04A5},
        {"EVENT_BEAT_BLACKBELT_KENJI3", 0x04A6},
        {"EVENT_BEAT_BLACKBELT_LAO", 0x04A7},
        {"EVENT_BEAT_BLACKBELT_NOB", 0x04A8},
        {"EVENT_BEAT_BLACKBELT_KIYO", 0x04A9},
        {"EVENT_BEAT_BLACKBELT_LUNG", 0x04AA},
        {"EVENT_BEAT_BLACKBELT_KENJI", 0x04AB},
        {"EVENT_BEAT_BLACKBELT_WAI", 0x04AC},
        {"EVENT_BEAT_BEAUTY_VICTORIA", 0x04AD},
        {"EVENT_BEAT_BEAUTY_SAMANTHA", 0x04AE},
        {"EVENT_BEAT_BEAUTY_JULIE", 0x04AF},
        {"EVENT_BEAT_BEAUTY_JACLYN", 0x04B0},
        {"EVENT_BEAT_BEAUTY_BRENDA", 0x04B1},
        {"EVENT_BEAT_BEAUTY_CASSIE", 0x04B2},
        {"EVENT_BEAT_BEAUTY_CAROLINE", 0x04B3},
        {"EVENT_BEAT_BEAUTY_CARLENE", 0x04B4},
        {"EVENT_BEAT_BEAUTY_JESSICA", 0x04B5},
        {"EVENT_BEAT_BEAUTY_RACHAEL", 0x04B6},
        {"EVENT_BEAT_BEAUTY_ANGELICA", 0x04B7},
        {"EVENT_BEAT_BEAUTY_KENDRA", 0x04B8},
        {"EVENT_BEAT_BEAUTY_VERONICA", 0x04B9},
        {"EVENT_BEAT_BEAUTY_JULIA", 0x04BA},
        {"EVENT_BEAT_BEAUTY_THERESA", 0x04BB},
        {"EVENT_BEAT_BEAUTY_VALERIE", 0x04BC},
        {"EVENT_BEAT_FALKNER", 0x04BD},
        {"EVENT_BEAT_BUGSY", 0x04BE},
        {"EVENT_BEAT_WHITNEY", 0x04BF},
        {"EVENT_BEAT_MORTY", 0x04C0},
        {"EVENT_BEAT_JASMINE", 0x04C1},
        {"EVENT_BEAT_CHUCK", 0x04C2},
        {"EVENT_BEAT_PRYCE", 0x04C3},
        {"EVENT_BEAT_CLAIR", 0x04C4},
        {"EVENT_BEAT_BROCK", 0x04C5},
        {"EVENT_BEAT_MISTY", 0x04C6},
        {"EVENT_BEAT_LTSURGE", 0x04C7},
        {"EVENT_BEAT_ERIKA", 0x04C8},
        {"EVENT_BEAT_JANINE", 0x04C9},
        {"EVENT_BEAT_SABRINA", 0x04CA},
        {"EVENT_BEAT_BLAINE", 0x04CB},
        {"EVENT_BEAT_BLUE", 0x04CC},
        {"EVENT_BEAT_POKEFANM_WILLIAM", 0x04CD},
        {"EVENT_BEAT_POKEFANM_DEREK", 0x04CE},
        {"EVENT_BEAT_POKEFANM_ROBERT", 0x04CF},
        {"EVENT_BEAT_POKEFANM_JOSHUA", 0x04D0},
        {"EVENT_BEAT_POKEFANM_CARTER", 0x04D1},
        {"EVENT_BEAT_POKEFANM_TREVOR", 0x04D2},
        {"EVENT_BEAT_POKEFANM_BRANDON", 0x04D3},
        {"EVENT_BEAT_POKEFANM_JEREMY", 0x04D4},
        {"EVENT_BEAT_POKEFANM_COLIN", 0x04D5},
        {"EVENT_BEAT_POKEFANM_DEREK2", 0x04D6},
        {"EVENT_BEAT_POKEFANM_DEREK3", 0x04D7},
        {"EVENT_BEAT_POKEFANM_ALEX", 0x04D8},
        {"EVENT_BEAT_POKEFANF_BEVERLY", 0x04D9},
        {"EVENT_BEAT_POKEFANF_RUTH", 0x04DA},
        {"EVENT_BEAT_POKEFANF_BEVERLY2", 0x04DB},
        {"EVENT_BEAT_POKEFANF_BEVERLY3", 0x04DC},
        {"EVENT_BEAT_POKEFANF_GEORGIA", 0x04DD},
        {"EVENT_BEAT_KIMONO_GIRL_NAOKO", 0x04DF},
        {"EVENT_BEAT_KIMONO_GIRL_SAYO", 0x04E0},
        {"EVENT_BEAT_KIMONO_GIRL_ZUKI", 0x04E1},
        {"EVENT_BEAT_KIMONO_GIRL_KUNI", 0x04E2},
        {"EVENT_BEAT_KIMONO_GIRL_MIKI", 0x04E3},
        {"EVENT_BEAT_POKEMANIAC_LARRY", 0x04E4},
        {"EVENT_BEAT_POKEMANIAC_ANDREW", 0x04E5},
        {"EVENT_BEAT_POKEMANIAC_CALVIN", 0x04E6},
        {"EVENT_BEAT_POKEMANIAC_SHANE", 0x04E7},
        {"EVENT_BEAT_POKEMANIAC_BEN", 0x04E8},
        {"EVENT_BEAT_POKEMANIAC_BRENT", 0x04E9},
        {"EVENT_BEAT_POKEMANIAC_RON", 0x04EA},
        {"EVENT_BEAT_POKEMANIAC_ETHAN", 0x04EB},
        {"EVENT_BEAT_POKEMANIAC_BRENT2", 0x04EC},
        {"EVENT_BEAT_POKEMANIAC_BRENT3", 0x04ED},
        {"EVENT_BEAT_POKEMANIAC_ISSAC", 0x04EE},
        {"EVENT_BEAT_POKEMANIAC_DONALD", 0x04EF},
        {"EVENT_BEAT_POKEMANIAC_ZACH", 0x04F0},
        {"EVENT_BEAT_ROCKET_GRUNTM_1", 0x04F1},
        {"EVENT_BEAT_ROCKET_GRUNTM_2", 0x04F2},
        {"EVENT_BEAT_ROCKET_GRUNTM_3", 0x04F3},
        {"EVENT_BEAT_ROCKET_GRUNTM_4", 0x04F4},
        {"EVENT_BEAT_ROCKET_GRUNTM_5", 0x04F5},
        {"EVENT_BEAT_ROCKET_GRUNTM_6", 0x04F6},
        {"EVENT_BEAT_ROCKET_GRUNTM_7", 0x04F7},
        {"EVENT_BEAT_ROCKET_GRUNTM_8", 0x04F8},
        {"EVENT_BEAT_ROCKET_GRUNTM_9", 0x04F9},
        {"EVENT_BEAT_ROCKET_GRUNTM_10", 0x04FA},
        {"EVENT_BEAT_ROCKET_GRUNTM_11", 0x04FB},
        {"EVENT_BEAT_ROCKET_GRUNTM_12", 0x04FC},
        {"EVENT_BEAT_ROCKET_GRUNTM_13", 0x04FD},
        {"EVENT_BEAT_ROCKET_GRUNTM_14", 0x04FE},
        {"EVENT_BEAT_ROCKET_GRUNTM_15", 0x04FF},
        {"EVENT_BEAT_ROCKET_GRUNTM_16", 0x0500},
        {"EVENT_BEAT_ROCKET_GRUNTM_17", 0x0501},
        {"EVENT_BEAT_ROCKET_GRUNTM_18", 0x0502},
        {"EVENT_BEAT_ROCKET_GRUNTM_19", 0x0503},
        {"EVENT_BEAT_ROCKET_GRUNTM_20", 0x0504},
        {"EVENT_BEAT_ROCKET_GRUNTM_21", 0x0505},
        {"EVENT_BEAT_ROCKET_GRUNTM_22", 0x0506},
        {"EVENT_BEAT_ROCKET_GRUNTM_23", 0x0507},
        {"EVENT_BEAT_ROCKET_GRUNTM_24", 0x0508},
        {"EVENT_BEAT_ROCKET_GRUNTM_25", 0x0509},
        {"EVENT_BEAT_ROCKET_GRUNTM_26", 0x050A},
        {"EVENT_BEAT_ROCKET_GRUNTM_27", 0x050B},
        {"EVENT_BEAT_ROCKET_GRUNTM_28", 0x050C},
        {"EVENT_BEAT_ROCKET_GRUNTM_29", 0x050D},
        {"EVENT_BEAT_ROCKET_GRUNTM_30", 0x050E},
        {"EVENT_BEAT_ROCKET_GRUNTM_31", 0x050F},
        {"EVENT_BEAT_ROCKET_GRUNTF_1", 0x0510},
        {"EVENT_BEAT_ROCKET_GRUNTF_2", 0x0511},
        {"EVENT_BEAT_ROCKET_GRUNTF_3", 0x0512},
        {"EVENT_BEAT_ROCKET_GRUNTF_4", 0x0513},
        {"EVENT_BEAT_ROCKET_GRUNTF_5", 0x0514},
        {"EVENT_BEAT_LASS_CARRIE", 0x0515},
        {"EVENT_BEAT_LASS_BRIDGET", 0x0516},
        {"EVENT_BEAT_LASS_ALICE", 0x0517},
        {"EVENT_BEAT_LASS_KRISE", 0x0518},
        {"EVENT_BEAT_LASS_CONNIE", 0x0519},
        {"EVENT_BEAT_LASS_LINDA", 0x051A},
        {"EVENT_BEAT_LASS_LAURA", 0x051B},
        {"EVENT_BEAT_LASS_SHANNON", 0x051C},
        {"EVENT_BEAT_LASS_MICHELLE", 0x051D},
        {"EVENT_BEAT_LASS_DANA", 0x051E},
        {"EVENT_BEAT_LASS_ELLEN", 0x051F},
        {"EVENT_BEAT_LASS_CONNIE2", 0x0520},
        {"EVENT_BEAT_LASS_CONNIE3", 0x0521},
        {"EVENT_BEAT_LASS_DANA2", 0x0522},
        {"EVENT_BEAT_LASS_DANA3", 0x0523},
        {"EVENT_BEAT_HIKER_ANTHONY2", 0x0524},
        {"EVENT_BEAT_HIKER_RUSSELL", 0x0525},
        {"EVENT_BEAT_HIKER_PHILLIP", 0x0526},
        {"EVENT_BEAT_HIKER_LEONARD", 0x0527},
        {"EVENT_BEAT_HIKER_ANTHONY", 0x0528},
        {"EVENT_BEAT_HIKER_BENJAMIN", 0x0529},
        {"EVENT_BEAT_HIKER_ERIK", 0x052A},
        {"EVENT_BEAT_HIKER_MICHAEL", 0x052B},
        {"EVENT_BEAT_HIKER_PARRY", 0x052C},
        {"EVENT_BEAT_HIKER_TIMOTHY", 0x052D},
        {"EVENT_BEAT_HIKER_BAILEY", 0x052E},
        {"EVENT_BEAT_HIKER_ANTHONY3", 0x052F},
        {"EVENT_BEAT_HIKER_TIM", 0x0530},
        {"EVENT_BEAT_HIKER_NOLAND", 0x0531},
        {"EVENT_BEAT_HIKER_SIDNEY", 0x0532},
        {"EVENT_BEAT_HIKER_KENNY", 0x0533},
        {"EVENT_BEAT_HIKER_JIM", 0x0534},
        {"EVENT_BEAT_HIKER_DANIEL", 0x0535},
        {"EVENT_BEAT_HIKER_PARRY2", 0x0536},
        {"EVENT_BEAT_HIKER_PARRY3", 0x0537},
        {"EVENT_BEAT_BUG_CATCHER_DON", 0x0538},
        {"EVENT_BEAT_BUG_CATCHER_ROB", 0x0539},
        {"EVENT_BEAT_BUG_CATCHER_ED", 0x053A},
        {"EVENT_BEAT_BUG_CATCHER_WADE", 0x053B},
        {"EVENT_BEAT_BUG_CATCHER_BENNY", 0x053C},
        {"EVENT_BEAT_BUG_CATCHER_AL", 0x053D},
        {"EVENT_BEAT_BUG_CATCHER_JOSH", 0x053E},
        {"EVENT_BEAT_BUG_CATCHER_ARNIE", 0x053F},
        {"EVENT_BEAT_BUG_CATCHER_KEN", 0x0540},
        {"EVENT_BEAT_BUG_CATCHER_WADE2", 0x0541},
        {"EVENT_BEAT_BUG_CATCHER_WADE3", 0x0542},
        {"EVENT_BEAT_BUG_CATCHER_DOUG", 0x0543},
        {"EVENT_BEAT_BUG_CATCHER_ARNIE2", 0x0544},
        {"EVENT_BEAT_BUG_CATCHER_ARNIE3", 0x0545},
        {"EVENT_BEAT_OFFICER_KEITH", 0x0546},
        {"EVENT_BEAT_OFFICER_DIRK", 0x0547},
        {"EVENT_BEAT_COOLTRAINERM_NICK", 0x0548},
        {"EVENT_BEAT_COOLTRAINERM_AARON", 0x0549},
        {"EVENT_BEAT_COOLTRAINERM_PAUL", 0x054A},
        {"EVENT_BEAT_COOLTRAINERM_CODY", 0x054B},
        {"EVENT_BEAT_COOLTRAINERM_MIKE", 0x054C},
        {"EVENT_BEAT_COOLTRAINERM_GAVEN2", 0x054D},
        {"EVENT_BEAT_COOLTRAINERM_GAVEN3", 0x054E},
        {"EVENT_BEAT_COOLTRAINERM_RYAN", 0x054F},
        {"EVENT_BEAT_COOLTRAINERM_JAKE", 0x0550},
        {"EVENT_BEAT_COOLTRAINERM_GAVEN", 0x0551},
        {"EVENT_BEAT_COOLTRAINERM_BLAKE", 0x0552},
        {"EVENT_BEAT_COOLTRAINERM_BRIAN", 0x0553},
        {"EVENT_BEAT_COOLTRAINERM_ERICK", 0x0554},
        {"EVENT_BEAT_COOLTRAINERM_ANDY", 0x0555},
        {"EVENT_BEAT_COOLTRAINERM_TYLER", 0x0556},
        {"EVENT_BEAT_COOLTRAINERM_SEAN", 0x0557},
        {"EVENT_BEAT_COOLTRAINERM_KEVIN", 0x0558},
        {"EVENT_BEAT_COOLTRAINERM_STEVE", 0x0559},
        {"EVENT_BEAT_COOLTRAINERM_ALLEN", 0x055A},
        {"EVENT_BEAT_COOLTRAINERF_GWEN", 0x055B},
        {"EVENT_BEAT_COOLTRAINERF_LOIS", 0x055C},
        {"EVENT_BEAT_COOLTRAINERF_FRAN", 0x055D},
        {"EVENT_BEAT_COOLTRAINERF_LOLA", 0x055E},
        {"EVENT_BEAT_COOLTRAINERF_KATE", 0x055F},
        {"EVENT_BEAT_COOLTRAINERF_IRENE", 0x0560},
        {"EVENT_BEAT_COOLTRAINERF_KELLY", 0x0561},
        {"EVENT_BEAT_COOLTRAINERF_JOYCE", 0x0562},
        {"EVENT_BEAT_COOLTRAINERF_BETH", 0x0563},
        {"EVENT_BEAT_COOLTRAINERF_REENA", 0x0564},
        {"EVENT_BEAT_COOLTRAINERF_MEGAN", 0x0565},
        {"EVENT_BEAT_COOLTRAINERF_BETH2", 0x0566},
        {"EVENT_BEAT_COOLTRAINERF_CAROL", 0x0567},
        {"EVENT_BEAT_COOLTRAINERF_QUINN", 0x0568},
        {"EVENT_BEAT_COOLTRAINERF_EMMA", 0x0569},
        {"EVENT_BEAT_COOLTRAINERF_CYBIL", 0x056A},
        {"EVENT_BEAT_COOLTRAINERF_JENN", 0x056B},
        {"EVENT_BEAT_COOLTRAINERF_BETH3", 0x056C},
        {"EVENT_BEAT_COOLTRAINERF_REENA2", 0x056D},
        {"EVENT_BEAT_COOLTRAINERF_REENA3", 0x056E},
        {"EVENT_BEAT_ROCKET_EXECUTIVEF_1", 0x056F},
        {"EVENT_BEAT_ROCKET_EXECUTIVEF_2", 0x0570},
        {"EVENT_BEAT_ROCKET_EXECUTIVEM_1", 0x0571},
        {"EVENT_BEAT_ROCKET_EXECUTIVEM_2", 0x0572},
        {"EVENT_BEAT_ROCKET_EXECUTIVEM_3", 0x0573},
        {"EVENT_BEAT_ROCKET_EXECUTIVEM_4", 0x0574},
        {"EVENT_BEAT_SAILOR_EUGENE", 0x0575},
        {"EVENT_BEAT_SAILOR_HUEY", 0x0576},
        {"EVENT_BEAT_SAILOR_TERRELL", 0x0577},
        {"EVENT_BEAT_SAILOR_KENT", 0x0578},
        {"EVENT_BEAT_SAILOR_ERNEST", 0x0579},
        {"EVENT_BEAT_SAILOR_JEFF", 0x057A},
        {"EVENT_BEAT_SAILOR_GARRETT", 0x057B},
        {"EVENT_BEAT_SAILOR_KENNETH", 0x057C},
        {"EVENT_BEAT_SAILOR_STANLY", 0x057D},
        {"EVENT_BEAT_SAILOR_HARRY", 0x057E},
        {"EVENT_BEAT_SAILOR_HUEY2", 0x057F},
        {"EVENT_BEAT_SAILOR_HUEY3", 0x0580},
        {"EVENT_BEAT_SUPER_NERD_STAN", 0x0581},
        {"EVENT_BEAT_SUPER_NERD_ERIC", 0x0582},
        {"EVENT_BEAT_SUPER_NERD_GREGG", 0x0583},
        {"EVENT_BEAT_SUPER_NERD_JAY", 0x0584},
        {"EVENT_BEAT_SUPER_NERD_SAM", 0x0586},
        {"EVENT_BEAT_SUPER_NERD_TOM", 0x0587},
        {"EVENT_BEAT_SUPER_NERD_PAT", 0x0588},
        {"EVENT_BEAT_SUPER_NERD_SHAWN", 0x0589},
        {"EVENT_BEAT_SUPER_NERD_TERU", 0x058A},
        {"EVENT_BEAT_MEDIUM_MARTHA", 0x058B},
        {"EVENT_BEAT_MEDIUM_GRACE", 0x058C},
        {"EVENT_BEAT_MEDIUM_BETHANY", 0x058D},
        {"EVENT_BEAT_MEDIUM_MARGRET", 0x058E},
        {"EVENT_BEAT_MEDIUM_ETHEL", 0x058F},
        {"EVENT_BEAT_MEDIUM_REBECCA", 0x0590},
        {"EVENT_BEAT_MEDIUM_DORIS", 0x0591},
        {"EVENT_BEAT_SKIER_ROXANNE", 0x0592},
        {"EVENT_BEAT_SKIER_CLARISSA", 0x0593},
        {"EVENT_BEAT_SWIMMERM_HAROLD", 0x0594},
        {"EVENT_BEAT_SWIMMERM_SIMON", 0x0595},
        {"EVENT_BEAT_SWIMMERM_RANDALL", 0x0596},
        {"EVENT_BEAT_SWIMMERM_CHARLIE", 0x0597},
        {"EVENT_BEAT_SWIMMERM_GEORGE", 0x0598},
        {"EVENT_BEAT_SWIMMERM_BERKE", 0x0599},
        {"EVENT_BEAT_SWIMMERM_KIRK", 0x059A},
        {"EVENT_BEAT_SWIMMERM_MATHEW", 0x059B},
        {"EVENT_BEAT_SWIMMERM_HAL", 0x059C},
        {"EVENT_BEAT_SWIMMERM_PATON", 0x059D},
        {"EVENT_BEAT_SWIMMERM_DARYL", 0x059E},
        {"EVENT_BEAT_SWIMMERM_WALTER", 0x059F},
        {"EVENT_BEAT_SWIMMERM_TONY", 0x05A0},
        {"EVENT_BEAT_SWIMMERM_JEROME", 0x05A1},
        {"EVENT_BEAT_SWIMMERM_TUCKER", 0x05A2},
        {"EVENT_BEAT_SWIMMERM_RICK", 0x05A3},
        {"EVENT_BEAT_SWIMMERM_CAMERON", 0x05A4},
        {"EVENT_BEAT_SWIMMERM_SETH", 0x05A5},
        {"EVENT_BEAT_SWIMMERM_JAMES", 0x05A6},
        {"EVENT_BEAT_SWIMMERM_LEWIS", 0x05A7},
        {"EVENT_BEAT_SWIMMERM_PARKER", 0x05A8},
        {"EVENT_BEAT_YOUNGSTER_JOEY", 0x05A9},
        {"EVENT_BEAT_YOUNGSTER_MIKEY", 0x05AA},
        {"EVENT_BEAT_YOUNGSTER_ALBERT", 0x05AB},
        {"EVENT_BEAT_YOUNGSTER_GORDON", 0x05AC},
        {"EVENT_BEAT_YOUNGSTER_SAMUEL", 0x05AD},
        {"EVENT_BEAT_YOUNGSTER_IAN", 0x05AE},
        {"EVENT_BEAT_YOUNGSTER_JOEY2", 0x05AF},
        {"EVENT_BEAT_YOUNGSTER_JOEY3", 0x05B0},
        {"EVENT_BEAT_YOUNGSTER_WARREN", 0x05B1},
        {"EVENT_BEAT_YOUNGSTER_JIMMY", 0x05B2},
        {"EVENT_BEAT_YOUNGSTER_OWEN", 0x05B3},
        {"EVENT_BEAT_YOUNGSTER_JASON", 0x05B4},
        {"EVENT_BEAT_TEACHER_COLETTE", 0x05B5},
        {"EVENT_BEAT_TEACHER_HILLARY", 0x05B6},
        {"EVENT_BEAT_TEACHER_SHIRLEY", 0x05B7},
        {"EVENT_BEAT_ELITE_4_WILL", 0x05B8},
        {"EVENT_BEAT_ELITE_4_KOGA", 0x05B9},
        {"EVENT_BEAT_ELITE_4_BRUNO", 0x05BA},
        {"EVENT_BEAT_ELITE_4_KAREN", 0x05BB},
        {"EVENT_BEAT_CHAMPION_LANCE", 0x05BC},
        {"EVENT_BEAT_COOLTRAINERM_DARIN", 0x05BD},
        {"EVENT_BEAT_COOLTRAINERF_CARA", 0x05BE},
        {"EVENT_BEAT_TWINS_LEA_AND_PIA", 0x05BF},
        {"EVENT_BEAT_BUG_CATCHER_WAYNE", 0x05C0},
        {"EVENT_BEAT_BEAUTY_OLIVIA", 0x05C1},
        {"EVENT_BEAT_POKEFANF_JAIME", 0x05C2},
        {"EVENT_BEAT_CAMPER_QUENTIN", 0x05C3},
        {"EVENT_BEAT_POKEMANIAC_MILLER", 0x05C4},
        {"EVENT_BEAT_SUPER_NERD_HUGH", 0x05C5},
        {"EVENT_BEAT_SUPER_NERD_MARKUS", 0x05C6},
        {"EVENT_BEAT_POKEFANM_REX", 0x05C7},
        {"EVENT_BEAT_POKEFANM_ALLAN", 0x05C8},
        {"EVENT_BEAT_SAGE_GAKU", 0x05C9},
        {"EVENT_BEAT_SAGE_MASA", 0x05CA},
        {"EVENT_BEAT_SAGE_KOJI", 0x05CB}
    };

    public static Dictionary<string, int> EventWorkNameToAddress = new Dictionary<string, int>
    {
        {"wJackFightCount", 0x0080},
        {"wHueyFightCount", 0x0082},
        { "wGavenFightCount", 0x0083},
        { "wBethFightCount", 0x0084},
        { "wJoseFightCount", 0x0085},
        { "wReenaFightCount", 0x0086},
        { "wJoeyFightCount", 0x0087},
        { "wWadeFightCount", 0x0088},
        { "wRalphFightCount", 0x0089},
        { "wLizFightCount", 0x008A},
        { "wAnthonyFightCount", 0x008B},
        { "wToddFightCount", 0x008C},
        { "wGinaFightCount", 0x008D},
        { "wArnieFightCount", 0x008F},
        { "wAlanFightCount", 0x0090},
        { "wDanaFightCount", 0x0091},
        { "wChadFightCount", 0x0092},
        { "wTullyFightCount", 0x0094},
        { "wBrentFightCount", 0x0095},
        { "wTiffanyFightCount", 0x0096},
        { "wVanceFightCount", 0x0097},
        { "wWiltonFightCount", 0x0098},
        { "wParryFightCount", 0x009A},
        { "wErinFightCount", 0x009B},
    };


    public static Dictionary<string, int> TrainerToFlag = new Dictionary<string, int> {
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

    public static HashSet<string> UntrackableTrainers = new HashSet<string> { "ALAN5", "ANTHONY5", "ARNIE5", "BETH3", "BRENT4", "CAL2", "CAL3", "CAROL", "CASSIE", "CHAD5", "CLYDE", "COLIN", "COREY", "DANA5", "DEBRA", "EDWARD", "ETHAN", "FRITZ", "GARRETT", "GAVEN2", "GEORGIA", "GINA5", "JACK5", "JEFF", "JEREMY", "JONAH", "JOSE3", "KEN", "KENNETH", "LIZ5", "LYLE", "MEGANDPEG1", "MEGANDPEG2", "NATE", "NOLAND", "RALPH5", "RED1", "REENA3", "RICKY", "RODNEY", "SEAN", "SHAWN", "SHIRLEY", "TIFFANY4", "TODD5", "TULLY4", "WADE5", "WAI", "WILTON3" };

}