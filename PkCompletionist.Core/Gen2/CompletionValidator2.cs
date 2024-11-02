using PKHeX.Core;
using System.Collections.Generic;
using System.Linq;

namespace PkCompletionist.Core;
internal class CompletionValidator2 : CompletionValidatorX
{
    public CompletionValidator2(Command command, SAV2 sav, Objective objective) : base(command, sav, objective)
    {
        this.sav = sav;
        this.unobtainableItems = new List<int>() { 6, 25, 45, 50, 56, 90, 100, 120, 135, 136, 137, 141, 142, 145, 147, 148, 149, 153, 154, 155, 162, 171, 176, 179, 190, 195, 220, 250, 251, 252, 253, 254, 255 };

        var MailItemID = new List<int>{0x9E, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA, 0xBB, 0xBC, 0xBD};
        for (int i = 0; i < 16; i++)
        {
            var mail = new Mail2(sav, i);
            if (mail.IsEmpty == true)
                continue;
            if (MailItemID.Contains(mail.MailType))
                this.OwnedItems.Add(mail.MailType);
        }
    }

    new SAV2 sav;

    public override void GenerateAll()
    {
        base.GenerateAll();

        Generate_inGameGift();
        Generate_misc();
        Generate_itemInMap();
        Generate_itemGift();
        Generate_inGameTrade();
        Generate_decoration();
        Generate_phone();
        Generate_battle(); 
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

        if (this.objective == Objective.living)
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

        ow["GetShinyPokemon"] = sav.GetAllPKM().FirstOrDefault(pkm => pkm.IsShiny) != null;
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

        var pkms = sav.GetAllPKM();
        var GetInfectedbyPokerus = () =>
        {
            return pkms.FirstOrDefault(pkm =>
            {
                var pk2 = (PK2)pkm;
                if (pk2 == null)
                    return false;
                return pk2.PKRS_Infected || pk2.PKRS_Cured || pk2.PKRS_Days > 0 || pk2.PKRS_Strain > 0;
            }) != null;
        };
        ow["GetInfectedbyPokerus"] = GetInfectedbyPokerus();
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


    public void Generate_battle()
    {
        var ow = new Dictionary<string, bool>();
        owned["battle"] = ow;

        var pokemonBattles = new List<int> { 1760, 1761, 1762, 42, 791, 792, 1872, 1873, };
        foreach (var evtIdx in pokemonBattles)
            ow[evtIdx.ToString()] = sav.GetEventFlag(evtIdx);

        /*
            wVanceFightCount
            wWiltonFightCount
        */

        // Rematch Logic:
        // After first battle, sets a regular EventFlag
        // If rematch, increments EventWork by 1
        // The rematch can be against the same pokemons as the first battle
        // In most cases, impossible to know if last battle is done. only possible when last battle gives item

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
            for (var i = 1; i < list.Count - 1; i++)
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
        func2(new List<string>{"JOEY1", "JOEY2", "JOEY3", "JOEY4", "JOEY5"}, "EVENT_BEAT_YOUNGSTER_JOEY", "wJoeyFightCount", "EVENT_GOT_HP_UP_FROM_JOEY");
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

        foreach (var pair in Cnst2.TrainerToFlag)
        {
            var val = sav.GetEventFlag(pair.Value);
            if (!val && Cnst2.UntrackableTrainers.Contains(pair.Key))
                continue;
            ow[pair.Key] = val;
        }

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
    public void Generate_itemInMap()
    {
        var ow = new Dictionary<string, bool>();
        owned["itemInMap"] = ow;

        var list = new List<int> { 1654, 1976, 1978, 141, 1629, 1944, 250, 1655, 1983, 1928, 244, 1683, 1986, 1671, 149, 1925, 1682, 127, 1943, 1650, 1998, 1923, 154, 1704, 1667, 1632, 1668, 1605, 174, 1722, 157, 243, 1918, 1926, 1947, 1955, 1610, 1661, 1669, 1685, 1614, 1691, 1979, 136, 1653, 169, 128, 1636, 1659, 242, 1917, 177, 138, 132, 135, 1647, 1675, 1652, 1695, 159, 1702, 252, 238, 1611, 146, 1994, 181, 1701, 229, 239, 153, 1681, 1686, 126, 1619, 184, 1693, 1952, 1940, 1712, 165, 1626, 1643, 1672, 1622, 1703, 1927, 1991, 1946, 1954, 176, 1633, 180, 133, 171, 144, 1663, 1670, 1641, 1696, 1648, 150, 1992, 1674, 235, 1920, 249, 1984, 162, 1706, 247, 1688, 1618, 1689, 172, 179, 1995, 1717, 1657, 246, 237, 173, 1958, 1664, 183, 148, 1676, 142, 161, 1723, 158, 233, 1924, 125, 1990, 156, 145, 1981, 1662, 147, 1718, 1700, 228, 1687, 1616, 1692, 1953, 1705, 236, 1941, 1948, 1949, 1678, 1980, 170, 1959, 1642, 1720, 1929, 1684, 1617, 1603, 1996, 1673, 175, 253, 1930, 1921, 1613, 1988, 1607, 1624, 139, 1711, 1709, 164, 1710, 1609, 1628, 1694, 248, 234, 1645, 1982, 1931, 1690, 1945, 1604, 167, 1637, 1665, 182, 1708, 254, 151, 1615, 163, 1713, 1942, 1649, 178, 1660, 134, 143, 160, 1721, 1697, 245, 1922, 240, 1651, 1951, 1950, 166, 1634, 137, 168, 1640, 1716, 140, 1638, 1714, 1698, 1635, 1623, 1707, 1699, 1625, 1639, 1658, 1630, 1666, 1606, 1677, 1644, 1919, 1656, 255, 1715, 1993, 1620, 1719, 1621, 1997, 241, 1679, 1680, 152, 1612, 1985, 155, 1987, 1608, 231, 1627, 1977, 1631, 232, 1646, 230, 1724 };
        foreach (var evtIdx in list)
            ow[evtIdx.ToString()] = sav.GetEventFlag(evtIdx);

        if (ow["236"] == false) // Mt. Moon Square (Hidden) event every monday
            ow.Remove("236");
    }

    public void Generate_itemGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["itemGift"] = ow;

        var list = new List<int> { 10, 101, 103, 105, 107, 109, 11, 111, 112, 113, 114, 115, 116, 117, 12, 122, 124, 13, 1395, 14, 15, 16, 17, 18, 19, 20, 200, 206, 209, 21, 210, 212, 216, 218, 219, 220, 221, 222, 223, 224, 226, 227, 23, 24, 25, 256, 257, 258, 259, 260, 35, 36, 39, 53, 613, 614, 615, 616, 617, 62, 71, 72, 75, 77, 78, 8, 800, 801, 802, 803, 804, 82, 83, 86, 87, 88, 89, 9, 90, 91, 92, 93, 94, 95, 99 };
        var untrackable = new HashSet<int> { 101, 103, 105, 107, 109, 111, 99 };
        foreach (var evtIdx in list)
        {
            var val = sav.GetEventFlag(evtIdx);
            if (!val && untrackable.Contains(evtIdx))
                continue;
            ow[evtIdx.ToString()] = val;
        }

        ow["custom0"] = sav.GetEventFlag(0x1A) && sav.GetWork(0x15) != 5; // Potion
        ow["custom2"] = sav.GetEventFlag(0x1F) && sav.GetWork(0x15) != 6; // Pokeball
        //ow["custom3"] = sav.GetEventFlag(0x1F) && sav.GetWork(0x15) != 6; // Red Scale for Exp.Share

        //var SysFlags = new List<int> { 91 }; // Return/Frustration. Untrackable anyway


    }
}