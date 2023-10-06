using PKHeX.Core;
using System.Collections.Generic;

namespace PkCompletionist.Core;
internal class CompletionValidator1 : CompletionValidatorX
{
    public CompletionValidator1(Command command, SAV1 sav, bool living) : base(command, sav, living)
    {
        this.sav = sav;
    }

    new SAV1 sav;

    public override void GenerateAll()
    {
        base.GenerateAll();

        Generate_inGameGift();
        Generate_misc();
        Generate_inGameTrade();
    }

    public override void Generate_item()
    {
        base.Generate_item();

        var ow = owned["item"];

        var Add = (int type, int flag, int itemId) =>
        {
            var key = itemId.ToString();
            if (ow.ContainsKey(key) && ow[key])
                return;

            var got = false;
            if (type == 0)
                got = this.sav.GetFlag(0x2852 + (flag / 8), flag % 8); // MissableObjectFlags
            else if (type == 1)
                got = this.sav.GetFlag(0x299C + (flag / 8), flag % 8); // ObtainedHiddenItems 
            else if (type == 2)
                got = this.sav.GetEventFlag(flag);

            ow[key] = got;
        };

        Add(0, 0x07, 228); // TM28 - Dig
        Add(0, 0x1A, 10); // Moon Stone
        Add(0, 0x1B, 35); // HP Up
        Add(0, 0x1C, 204); // TM04 - Whirlwind
        Add(0, 0x1D, 230); // TM30 - Teleport
        Add(0, 0x1F, 216); // TM16 - Pay Day
        Add(0, 0x20, 37); // Iron
        Add(0, 0x21, 220); // TM20 - Rage
        Add(0, 0x26, 245); // TM45 - Thunder Wave
        Add(0, 0x27, 219); // TM19 - Seismic Toss
        Add(0, 0x32, 53); // Revive
        Add(0, 0x35, 40); // Rare Candy
        Add(0, 0x36, 83); // Max Elixer
        Add(0, 0x37, 54); // Max Revive
        Add(0, 0x38, 2); // Ultra Ball
        Add(0, 0x3A, 29); // Escape Rope
        Add(0, 0x3B, 82); // Elixer
        Add(0, 0x3C, 14); // Awakening
        Add(0, 0x3D, 35); // HP Up
        Add(0, 0x3E, 49); // Nugget
        Add(0, 0x3F, 40); // Rare Candy
        Add(0, 0x40, 46); // X Accuracy
        Add(0, 0x47, 40); // Rare Candy
        Add(0, 0x48, 29); // Escape Rope
        Add(0, 0x49, 38); // Carbos
        Add(0, 0x56, 38); // Carbos
        Add(0, 0x57, 35); // HP Up
        Add(0, 0x58, 40); // Rare Candy
        Add(0, 0x59, 225); // TM25 - Thunder
        Add(0, 0x5A, 233); // TM33 - Reflect
        Add(0, 0x5C, 217); // TM17 - Submission
        Add(0, 0x5D, 52); // Full Heal
        Add(0, 0x5E, 205); // TM05 - Mega Kick
        Add(0, 0x5F, 55); // Guard Spec.
        Add(0, 0x64, 11); // Antidote
        Add(0, 0x65, 20); // Potion
        Add(0, 0x66, 4); // Poké Ball
        Add(0, 0x67, 20); // Potion
        Add(0, 0x68, 10); // Moon Stone
        Add(0, 0x69, 40); // Rare Candy
        Add(0, 0x6A, 29); // Escape Rope
        Add(0, 0x6B, 20); // Potion
        Add(0, 0x6C, 212); // TM12 - Water Gun
        Add(0, 0x71, 35); // HP Up
        Add(0, 0x72, 201); // TM01 - Mega Punch
        Add(0, 0x74, 208); // TM08 - Body Slam
        Add(0, 0x75, 81); // Max Ether
        Add(0, 0x76, 40); // Rare Candy
        Add(0, 0x77, 80); // Ether
        Add(0, 0x78, 244); // TM44 - Rest
        Add(0, 0x79, 17); // Max Potion
        Add(0, 0x7A, 54); // Max Revive
        Add(0, 0x7B, 247); // TM47 - Explosion
        Add(0, 0x7D, 29); // Escape Rope
        Add(0, 0x7E, 18); // Hyper Potion
        Add(0, 0x7F, 10); // Moon Stone
        Add(0, 0x80, 49); // Nugget
        Add(0, 0x81, 207); // TM07 - Horn Drill
        Add(0, 0x82, 19); // Super Potion
        Add(0, 0x83, 210); // TM10 - Double-Edge
        Add(0, 0x84, 40); // Rare Candy
        Add(0, 0x88, 35); // HP Up
        Add(0, 0x89, 202); // TM02 - Razor Wind
        Add(0, 0x8A, 37); // Iron
        Add(0, 0x8B, 72); // Silph Scope
        Add(0, 0x8C, 74); // Lift Key
        Add(0, 0x94, 18); // Hyper Potion
        Add(0, 0x98, 52); // Full Heal
        Add(0, 0x99, 54); // Max Revive
        Add(0, 0x9A, 29); // Escape Rope
        Add(0, 0x9F, 209); // TM09 - Take Down
        Add(0, 0xA0, 36); // Protein
        Add(0, 0xA1, 48); // Card Key
        Add(0, 0xA5, 35); // HP Up
        Add(0, 0xA6, 46); // X Accuracy
        Add(0, 0xAC, 39); // Calcium
        Add(0, 0xAD, 203); // TM03 - Swords Dance
        Add(0, 0xB8, 226); // TM26 - Earthquake
        Add(0, 0xB9, 40); // Rare Candy
        Add(0, 0xBA, 38); // Carbos
        Add(0, 0xC0, 39); // Calcium
        Add(0, 0xC1, 17); // Max Potion
        Add(0, 0xC2, 37); // Iron
        Add(0, 0xC3, 40); // Rare Candy
        Add(0, 0xC4, 16); // Full Restore
        Add(0, 0xC5, 214); // TM14 - Blizzard
        Add(0, 0xC6, 222); // TM22 - Solar Beam
        Add(0, 0xC7, 43); // Secret Key
        Add(0, 0xC8, 16); // Full Restore
        Add(0, 0xC9, 17); // Max Potion
        Add(0, 0xCA, 38); // Carbos
        Add(0, 0xCB, 237); // TM37 - Egg Bomb
        Add(0, 0xCC, 36); // Protein
        Add(0, 0xCD, 240); // TM40 - Skull Bash
        Add(0, 0xCE, 17); // Max Potion
        Add(0, 0xCF, 232); // TM32 - Double Team
        Add(0, 0xD0, 54); // Max Revive
        Add(0, 0xD1, 64); // Gold Teeth
        Add(0, 0xD2, 49); // Nugget
        Add(0, 0xD3, 40); // Rare Candy
        Add(0, 0xD4, 2); // Ultra Ball
        Add(0, 0xD5, 54); // Max Revive
        Add(0, 0xD6, 16); // Full Restore
        Add(0, 0xD8, 2); // Ultra Ball
        Add(0, 0xD9, 2); // Ultra Ball
        Add(0, 0xDA, 54); // Max Revive
        Add(0, 0xDB, 83); // Max Elixer
        Add(0, 0xDC, 243); // TM43 - Sky Attack
        Add(0, 0xDD, 40); // Rare Candy
        Add(1, 0x00, 82); // Elixer
        Add(1, 0x01, 17); // Max Potion
        Add(1, 0x02, 54); // Max Revive
        Add(1, 0x03, 40); // Rare Candy
        Add(1, 0x04, 53); // Revive
        Add(1, 0x05, 79); // PP Up
        Add(1, 0x06, 79); // PP Up
        Add(1, 0x08, 49); // Nugget
        Add(1, 0x09, 83); // Max Elixer
        Add(1, 0x0A, 2); // Ultra Ball
        Add(1, 0x0B, 20); // Potion
        Add(1, 0x0C, 11); // Antidote
        Add(1, 0x0D, 10); // Moon Stone
        Add(1, 0x0E, 80); // Ether
        Add(1, 0x0F, 3); // Great Ball
        Add(1, 0x10, 18); // Hyper Potion
        Add(1, 0x11, 16); // Full Restore
        Add(1, 0x12, 68); // X Special
        Add(1, 0x13, 49); // Nugget
        Add(1, 0x14, 82); // Elixer
        Add(1, 0x15, 79); // PP Up
        Add(1, 0x16, 49); // Nugget
        Add(1, 0x17, 19); // Super Potion
        Add(1, 0x18, 19); // Super Potion
        Add(1, 0x19, 81); // Max Ether
        Add(1, 0x1A, 83); // Max Elixer
        Add(1, 0x1B, 79); // PP Up
        Add(1, 0x1C, 29); // Escape Rope
        Add(1, 0x1D, 18); // Hyper Potion
        Add(1, 0x1E, 79); // PP Up
        Add(1, 0x1F, 39); // Calcium
        Add(1, 0x20, 40); // Rare Candy
        Add(1, 0x21, 16); // Full Restore
        Add(1, 0x22, 79); // PP Up
        Add(1, 0x23, 54); // Max Revive
        Add(1, 0x24, 83); // Max Elixer
        Add(1, 0x25, 16); // Full Restore
        Add(1, 0x26, 2); // Ultra Ball
        Add(1, 0x27, 81); // Max Ether
        Add(1, 0x28, 2); // Ultra Ball
        Add(1, 0x29, 16); // Full Restore
        Add(1, 0x2A, 80); // Ether
        Add(1, 0x2B, 82); // Elixer
        Add(1, 0x2C, 3); // Great Ball
        Add(1, 0x2D, 80); // Ether
        Add(1, 0x2E, 49); // Nugget
        Add(1, 0x2F, 20); // Potion
        Add(1, 0x30, 40); // Rare Candy
        Add(1, 0x31, 79); // PP Up
        Add(1, 0x32, 82); // Elixer
        Add(1, 0x33, 81); // Max Ether
        Add(1, 0x34, 79); // PP Up
        Add(1, 0x36, 10); // Moon Stone
        Add(2, 0x018, 5); // Town Map
        Add(2, 0x024, 4); // Poké Ball
        Add(2, 0x025, 9); // Pokédex
        Add(2, 0x029, 242); // TM42 - Dream Eater
        Add(2, 0x039, 70); // Oak's Parcel
        Add(2, 0x050, 227); // TM27 - Fissure
        Add(2, 0x069, 31); // Old Amber
        Add(2, 0x076, 234); // TM34 - Bide
        Add(2, 0x0BE, 211); // TM11 - Bubble Beam
        Add(2, 0x0C0, 6); // Bicycle
        Add(2, 0x128, 73); // Poké Flute
        Add(2, 0x151, 45); // Bike Voucher
        Add(2, 0x166, 224); // TM24 - Thunderbolt
        Add(2, 0x180, 241); // TM41 - Softboiled
        Add(2, 0x18C, 213); // TM13 - Ice Beam
        Add(2, 0x18D, 248); // TM48 - Rock Slide
        Add(2, 0x18E, 249); // TM49 - Tri Attack
        Add(2, 0x18F, 218); // TM18 - Counter
        Add(2, 0x1A8, 221); // TM21 - Mega Drain
        Add(2, 0x238, 199); // HM04 - Strength
        Add(2, 0x258, 206); // TM06 - Toxic
        Add(2, 0x298, 238); // TM38 - Fire Blast
        Add(2, 0x2D7, 235); // TM35 - Metronome
        Add(2, 0x340, 231); // TM31 - Mimic
        Add(2, 0x360, 246); // TM46 - Psywave
        Add(2, 0x3B0, 229); // TM29 - Psychic
        Add(2, 0x3D8, 200); // HM05 - Flash
        Add(2, 0x3C0, 20); // Potion
        Add(2, 0x47F, 71); // Itemfinder
        Add(2, 0x480, 239); // TM39 - Swift
        Add(2, 0x4B0, 75); // Exp. All
        Add(2, 0x4CE, 197); // HM02 - Fly
        Add(2, 0x540, 49); // Nugget
        Add(2, 0x55C, 63); // S.S. Ticket
        Add(2, 0x578, 41); // Dome Fossil
        Add(2, 0x57F, 42); // Helix Fossil
        Add(2, 0x5E0, 196); // HM01 - Cut
        Add(2, 0x6FF, 236); // TM36 - Selfdestruct
        Add(2, 0x78D, 1); // Master Ball
        Add(2, 0x880, 198); // HM03 - Surf

        if ((sav.Badges & 0b11111111) == 0b11111111)
            ow["8"] = true; // Safari Ball

        // no warning for not owned item, because there's not enough space in Gen1 for all items
        if (!this.living)
        {
            // remove all false values
            var itemsToRemove = new List<string>();
            foreach (var entry in ow)
                if (entry.Value == false)
                    itemsToRemove.Add(entry.Key);
            foreach (var item in itemsToRemove)
                ow.Remove(item);


            // The only way to obtain those items is triggerring a flag. If there are no flags, the player never had the item.
            int[] itemOnlyWithFlags = new int[]{
                1 /*Master Ball*/, 5 /*Town Map*/, 6 /*Bicycle*/, 8 /*Safari Ball*/, 9 /*Pokédex*/, 10 /*Moon Stone*/, 31 /*Old Amber*/, 40 /*Rare Candy*/, 42 /*Helix Fossil*/, 43 /*Secret Key*/, 45 /*Bike Voucher*/, 48 /*Card Key*/, 49 /*Nugget*/, 54 /*Max Revive*/, 63 /*S.S. Ticket*/, 64 /*Gold Teeth*/, 69 /*Coin Case*/, 70 /*Oak's Parcel*/, 71 /*Itemfinder*/, 72 /*Silph Scope*/, 73 /*Poké Flute*/, 74 /*Lift Key*/, 75 /*Exp. All*/, 76 /*Old Rod*/, 77 /*Good Rod*/, 78 /*Super Rod*/, 79 /*PP Up*/, 80 /*Ether*/, 81 /*Max Ether*/, 82 /*Elixer*/, 83 /*Max Elixer*/, 196 /*HM01 - Cut*/, 197 /*HM02 - Fly*/, 198 /*HM03 - Surf*/, 199 /*HM04 - Strength*/, 200 /*HM05 - Flash*/, 203 /*TM03 - Swords Dance*/, 204 /*TM04 - Whirlwind*/, 206 /*TM06 - Toxic*/, 208 /*TM08 - Body Slam*/, 210 /*TM10 - Double-Edge*/, 211 /*TM11 - Bubble Beam*/, 212 /*TM12 - Water Gun*/, 213 /*TM13 - Ice Beam*/, 214 /*TM14 - Blizzard*/, 216 /*TM16 - Pay Day*/, 219 /*TM19 - Seismic Toss*/, 220 /*TM20 - Rage*/, 221 /*TM21 - Mega Drain*/, 222 /*TM22 - Solar Beam*/, 224 /*TM24 - Thunderbolt*/, 225 /*TM25 - Thunder*/, 226 /*TM26 - Earthquake*/, 227 /*TM27 - Fissure*/, 228 /*TM28 - Dig*/, 229 /*TM29 - Psychic*/, 230 /*TM30 - Teleport*/, 231 /*TM31 - Mimic*/, 234 /*TM34 - Bide*/, 235 /*TM35 - Metronome*/, 236 /*TM36 - Selfdestruct*/, 238 /*TM38 - Fire Blast*/, 239 /*TM39 - Swift*/, 240 /*TM40 - Skull Bash*/, 241 /*TM41 - Softboiled*/, 242 /*TM42 - Dream Eater*/, 243 /*TM43 - Sky Attack*/, 244 /*TM44 - Rest*/, 245 /*TM45 - Thunder Wave*/, 246 /*TM46 - Psywave*/, 247 /*TM47 - Explosion*/, 248 /*TM48 - Rock Slide*/, 249 /*TM49 - Tri Attack*/
            };

            foreach (var itemId in itemOnlyWithFlags)
            {
                var key = itemId.ToString();
                if (ow.ContainsKey(key) && ow[key])
                    continue;
                ow[key] = false;
            }
        }
        
    }

    public void Generate_misc()
    {
        //0x09  Become Pokémon League Champion and finish main story
        var ow = new Dictionary<string, bool>();
        owned["misc"] = ow;

        ow["PlayPikachusBeachMinigame"] = sav.PikaBeachScore > 0;
    }


    public void Generate_inGameGift()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameGift"] = ow;

        ow["Magikarp"] = sav.GetEventFlag(0x3FF);
        ow["Bulbasaur"] = sav.GetEventFlag(0x0A8);
        ow["Charmander"] = sav.GetEventFlag(0x54F);
        ow["Squirtle"] = sav.GetEventFlag(0x147);
        ow["Eevee"] = sav.EventSpawnFlags[0x45];
        ow["HitmonleeorHitmonchan"] = sav.GetEventFlag(0x356) || sav.GetEventFlag(0x357);
        ow["Pikachu"] = true;
        ow["Lapras"] = sav.GetFlag(0x29DA, 0);
    }

    public void Generate_inGameTrade()
    {
        var ow = new Dictionary<string, bool>();
        owned["inGameTrade"] = ow;

        ow["LickitungforDugtrio"] = sav.GetFlag(0x29E3, 0);
        ow["ClefairyforMrMime"] = sav.GetFlag(0x29E3, 1);
        ow["KangaskhanforMuk"] = sav.GetFlag(0x29E3, 3);
        ow["TangelaforParasect"] = sav.GetFlag(0x29E3, 5);
        ow["GolduckforRhydon"] = sav.GetFlag(0x29E3, 7);
        ow["GrowlitheforDewgong"] = sav.GetFlag(0x29E4, 0);
        ow["CuboneforMachamp"] = sav.GetFlag(0x29E4, 1);
    }
}