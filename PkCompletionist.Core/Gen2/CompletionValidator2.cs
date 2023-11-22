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