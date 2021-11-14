using NPCConsoleTesting.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatantBuilder
    {
        private int _MinHP;
        private int _MaxHP;
        private int _MinInitMod;
        private int _MaxInitMod;
        private int _MinAC;
        private int _MaxAC;
        private int _MinLevel;
        private int _MaxLevel;

        public int MinHP { get => _MinHP; set => _MinHP = value; }
        public int MaxHP { get => _MaxHP; set => _MaxHP = value; }
        public int MinInitMod { get => _MinInitMod; set => _MinInitMod = value; }
        public int MaxInitMod { get => _MaxInitMod; set => _MaxInitMod = value; }
        public int MinAC { get => _MinAC; set => _MinAC = value; }
        public int MaxAC { get => _MaxAC; set => _MaxAC = value; }
        public int MinLevel { get => _MinLevel; set => _MinLevel = value; }
        public int MaxLevel { get => _MaxLevel; set => _MaxLevel = value; }

        public CombatantBuilder()
        {
            MinHP = 1;
            MaxHP = 10;
            MinInitMod = 1;
            MaxInitMod = 5;
            MinAC = 3;
            MaxAC = 10;
            MinLevel = 1;
            MaxLevel = 5;
        }

        const int MIN_NAME_PATTERN_LENGTH = 3;
        const int MAX_NAME_PATTERN_LENGTH = 7;

        static List<string> charClasses = new() { "fighter", "paladin", "ranger", "magic-user", "cleric", "monk", "druid", "thief", "assassin" };
        static List<string> races = new() { "human", "elf", "dwarf", "halfling" };

        static List<string> muWeaponList = new() { "dagger", "darts", "staff" };
        static List<string> clericWeaponList = new() { "club", "flail", "hammer", "mace", "staff" };
        static List<string> druidWeaponList = new() { "club", "dagger", "darts", "hammer", "spear", "staff" };
        static List<string> thiefWeaponList = new() { "club", "dagger", "darts", "longsword", "shortsword" };
        static List<string> monkWeaponList = new() { "club", "darts", "dagger", "staff", "none" };
        static List<string> fighterWeaponList = new() { "axe", "halberd", "longsword", "shortsword", "spear", "two-handed sword" };

        static Random _random = new();

        public List<Combatant> BuildListOfCombatants(string connectionString)
        {
            int numberBattling = DetermineNumberBattling();
            int retrievalMethod = DetermineRetrievalMethod();

            List<Combatant> combatants = new();
            while (combatants.Count < numberBattling)
            {
                combatants.Add(GetCombatant(retrievalMethod, combatants.Count, connectionString));
            }

            return combatants;
        }

        private int DetermineNumberBattling()
        {
            int numberBattling = 0;
            while (numberBattling < 2)
            {
                Console.WriteLine("How many are battling?");
                try
                {
                    numberBattling = int.Parse(Console.ReadLine());
                    if (numberBattling < 2)
                    {
                        Console.WriteLine("At least two are needed for a battle.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("We're looking for an integer...");
                }
            }

            Console.WriteLine("Hmm, a good number for a battle.");
            Console.WriteLine();

            return numberBattling;
        }

        private int DetermineRetrievalMethod()
        {
            Console.WriteLine("How shall the combatants be selected? 1 = Random, 2 = Custom, 3 = Get from db.");
            return GetIntFromUser();
        }

        private static int GetIntFromUser()
        {
            int integer = 0;
            while (integer == 0)
            {
                try
                {
                    integer = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("We're looking for an integer...");
                }
            }

            return integer;
        }

        private Combatant GetCombatant(int retrievalMethod, int numberOfCombatants, string connectionString)
        {
            List<Combatant> result = new();

            while (result.Count < 1)
            {
                if (retrievalMethod == 2)
                {
                    try
                    {
                        result.Add(BuildCombatantViaConsole(numberOfCombatants + 1));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Something went wrong; we'll have to start over for this character.");
                    }
                }
                else if (retrievalMethod == 3)
                {
                    string name = GetCustomNameFromUserInput(numberOfCombatants + 1);
                    try
                    {
                        result.Add(CombatantRetriever.GetCombatantByName(connectionString, name));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }
                }
                else
                {
                    CombatantBuilder cb = new();
                    result.Add(cb.BuildCombatantRandomly());
                }
            }

            return result[0];
        }

        public Combatant BuildCombatantRandomly()
        {
            string name = GenerateRandomName();
            string charClass = SelectRandomClass();
            string race = SelectRandomRace();
            int level = _random.Next(_MinLevel, _MaxLevel + 1);
            Attributes attributes = GenerateAttributes(charClass, race);
            int str = attributes.Strength;
            int ex_str = attributes.Ex_Strength;
            int dex = attributes.Dexterity;
            int con = attributes.Constitution;
            List<int> HPByLevel = GenerateHPByLevelByCharClass(charClass, level);
            int currentHP = CalcMaxHP(HPByLevel, con, charClass);
            //int initMod = 0;
            string armor = SelectRandomArmor(charClass);
            string weapon = SelectRandomWeapon(charClass, level);
            bool hasShield = DetermineShieldPresence(charClass, weapon);
            List<string> spells = GenerateSpellList(charClass, level);

            return new Combatant(name, charClass, level, race, str, dex, con, HPByLevel, currentHP, charEx_Strength: ex_str, charArmor: armor,
                charWeapon: weapon, charHasShield: hasShield, charSpells: spells);
        }

        public Combatant BuildCombatantViaConsole(int charNumber)
        {
            string name = GetName(charNumber);
            string charClass = GetCharClass(name);
            string race = GetCharRace(name, charClass);
            int level = GetLevel(name, _MinLevel, _MaxLevel);

            Attributes attributes = GenerateAttributes(charClass, race);
            int str = attributes.Strength;
            int ex_str = attributes.Ex_Strength;
            int dex = attributes.Dexterity;
            int con = attributes.Constitution;

            List<int> HPByLevel = GenerateHPByLevelByCharClass(charClass, level);
            int currentHP = CalcMaxHP(HPByLevel, con, charClass);

            //Console.WriteLine($"Enter HP for character {charNumber}");
            //int HP = int.Parse(Console.ReadLine());

            //Console.WriteLine($"Enter initMod for character {charNumber}");
            //int initMod = int.Parse(Console.ReadLine());

            string weapon = GetWeapon(name, charClass, level);

            //armor

            //shield

            //TODO: spells?

            return new Combatant(name, charClass, level, "human", 12, 12, 12, HPByLevel, currentHP, charWeapon: weapon);
        }

        public static string GetName(int charNumber)
        {
            Console.WriteLine($"Generate name for character {charNumber} randomly or enter a custom name? 1 = Random, 2 = Custom.");
            int nameCreationTechnique = GetIntFromUser();
            string name = nameCreationTechnique == 2 ? GetCustomNameFromUserInput(charNumber) : GenerateRandomName();

            Console.WriteLine($"{name}... A fine name.");
            Console.WriteLine();

            return name;
        }

        public static string GetCustomNameFromUserInput(int charNumber)
        {
            Console.WriteLine($"Enter name for character {charNumber}:");

            string name = "";
            while (name == "")
            {
                try
                {
                    name = Console.ReadLine();
                    if (name == "")
                    {
                        Console.WriteLine("At least one letter is needed.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("That didn't work. Try again.");
                }
            }

            return name;
        }

        public static string GetCharClass(string name)
        {
            Console.WriteLine($"Determine class for {name} randomly or enter manually? 1 = Random, 2 = Manually.");
            int classSelectionTechnique = GetIntFromUser();

            string charClass = "";
            while (!charClasses.Contains(charClass))
            {
                if (classSelectionTechnique == 2)
                {
                    try
                    {
                        Console.WriteLine($"Enter class for {name}:");
                        charClass = Console.ReadLine();

                        List<string> muNames = new() { "wizard", "mage", "magic user" };
                        //TODO: lowercase input before checking against list
                        if (muNames.Contains(charClass))
                        {
                            charClass = "magic-user";
                        }
                        else if (!charClasses.Contains(charClass))
                        {
                            string randomClass = SelectRandomClass();
                            //TODO: create AddArticle() method
                            string article2 = randomClass == "assassin" ? "an" : "a";
                            Console.WriteLine($"That class isn't recognized; try something else. Perhaps {article2} {randomClass}?");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }
                }
                else
                {
                    charClass = SelectRandomClass();
                }
            }

            string article = charClass == "assassin" ? "an" : "a";
            Console.WriteLine($"Very well, {name} shall be {article} {charClass}.");
            Console.WriteLine();

            return charClass;
        }

        public static string GetCharRace(string name, string charClass)
        {
            Console.WriteLine($"Determine race for {name} randomly or enter manually? 1 = Random, 2 = Manually.");
            int raceSelectionTechnique = GetIntFromUser();

            //TODO: add logic for race dependent on class
            string race = "";
            while(!races.Contains(race))
            {
                if (raceSelectionTechnique == 2)
                {
                    try
                    {
                        Console.WriteLine($"Enter race for {name}:");
                        race = Console.ReadLine();

                        if (!races.Contains(race))
                        {
                            string randomRace = SelectRandomRace();
                            //TODO: create DetermineArticle() method
                            string article2 = randomRace == "elf" ? "an" : "a";
                            Console.WriteLine($"That race isn't recognized; try something else. Perhaps {article2} {randomRace}?");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }
                }
                else
                {
                    //TODO: add logic for race dependent on class
                    race = SelectRandomRace();
                }
            }

            string article = race == "elf" ? "an" : "a";
            Console.WriteLine($"Very well, {name} shall be {article} {race}.");
            Console.WriteLine();

            return race;
        }

        public static int GetLevel(string name, int minLevel, int maxLevel)
        {
            Console.WriteLine($"Determine level for {name} randomly or enter manually? 1 = Random, 2 = Manually.");
            int levelSelectionTechnique = GetIntFromUser();

            int level = 0;
            while (level < minLevel || level > maxLevel)
            {
                if (levelSelectionTechnique == 2)
                {
                    try
                    {
                        Console.WriteLine($"Enter level for {name}:");
                        level = int.Parse(Console.ReadLine());

                        if (level < minLevel)
                        {
                            Console.WriteLine("Level must be a positive integer.");
                        }

                        if (level > maxLevel)
                        {
                            Console.WriteLine($"Current settings won't allow for a character of that level. Maximum level is {maxLevel} at this time.");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }
                }
                else
                {
                    level = _random.Next(minLevel, maxLevel + 1);
                }
            }

            Console.WriteLine($"Very well, {name}'s level shall be {level}.");
            Console.WriteLine();

            return level;
        }

        public static string GetWeapon(string name, string charClass, int level)
        {
            Console.WriteLine($"Determine {name}'s weapon randomly or enter manually? 1 = Random, 2 = Manually.");
            int weaponSelectionTechnique = GetIntFromUser();

            string weapon = "";
            while (!WeaponIsAppropriate(charClass, weapon))
            {
                if (weaponSelectionTechnique == 2)
                {
                    try
                    {
                        Console.WriteLine($"Enter weapon for {name}:");
                        weapon = Console.ReadLine();

                        if (!WeaponIsAppropriate(charClass, weapon))
                        {
                            //TODO: suggest an appropriate weapon based on class
                            Console.WriteLine($"That's not an appropriate weapon. Try something else.");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }
                }
                else
                {
                    weapon = SelectRandomWeapon(charClass, level);
                }
            }

            //TODO: fix grammar for "axe", "darts" and "none"
            Console.WriteLine($"Very well, {name} shall wield a {weapon}.");
            Console.WriteLine();

            return weapon;
        }

        private static bool WeaponIsAppropriate(string charClass, string weapon)
        {
            return charClass switch
            {
                "magic-user" or "illusionist" => muWeaponList.Contains(weapon),
                "cleric" => clericWeaponList.Contains(weapon),
                "druid" => druidWeaponList.Contains(weapon),
                "thief" => thiefWeaponList.Contains(weapon),
                "monk" => monkWeaponList.Contains(weapon),
                "fighter" or "paladin" or "ranger" or "assassin" => fighterWeaponList.Contains(weapon),
                _ => false
            }; 
        }

        public static string GenerateRandomName()
        {
            string[] consonants = {"b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "qu", "r", "s", "t", "v", "w", "x", "z"};
            string[] startingBlends = {"bl", "br", "cl", "cr", "dr", "fl", "fr", "gl", "gr", "pl", "pr", "sl",
                "sn", "sw", "tr", "tw", "wh", "wr", "scr", "shr", "sph", "spl", "spr", "squ", "str", "thr"};
            string[] endingBlends = { "ch", "sc", "sh", "sk", "sm", "sp", "st", "th", "sch"};
            string[] vowels = {"a", "e", "i", "o", "u", "y"};
            string[] doubleVowels = {"aa", "ae", "ai", "ao", "au", "ea", "ee", "ei", "eo", "eu", "ia", "ie",
                "io", "iu", "oa", "oe", "oi", "oo", "ou", "ua", "ue", "ui", "uo"};

            int patternLength = _random.Next(MIN_NAME_PATTERN_LENGTH, MAX_NAME_PATTERN_LENGTH);
            //the pattern will be a list of ints with values between 0 and 3, each int corresponding to a LetterGroup (consonants, etc.)

            //initialize the pattern with a random int between 0 and 3
            List<int> pattern = new() {_random.Next(0, 4)};
            string name = new("");

            //add random ints to the pattern until patternLength is met
            while (pattern.Count < patternLength)
            {
                //avoid assigning multiple vowels or non-vowels in a row
                if (pattern[^1] != (int)LetterGroups.vowels)
                {
                    pattern.Add((int)LetterGroups.vowels);
                }
                else
                {
                    //0, 1 and 2 all correspond to non-vowels
                    pattern.Add(_random.Next(0, 3));
                }
                //ensure pattern does not end with a startingBlend
                if (pattern[^1] == (int)LetterGroups.startingBlends)
                {
                    pattern.RemoveAt(pattern.Count - 1);
                }
            }

            //build name with letter strings according to pattern
            for (int i = 0; i < pattern.Count; i++)
            {
                switch (pattern[i])
                {
                    case (int)LetterGroups.consonants:
                        name += consonants[_random.Next(1, consonants.Length)];
                        break;
                    case (int)LetterGroups.startingBlends:
                        name += startingBlends[_random.Next(1, startingBlends.Length)];
                        break;
                    case (int)LetterGroups.endingBlends:
                        name += endingBlends[_random.Next(1, endingBlends.Length)];
                        break;
                    case (int)LetterGroups.vowels:
                        //use single vowels slightly more often (60% of the time) than double vowels
                        if (_random.Next(1, 11) > 4)
                        {
                            name += vowels[_random.Next(1, vowels.Length)];
                        }
                        else
                        {
                            name += doubleVowels[_random.Next(1, doubleVowels.Length)];
                        }
                        break;
                    default: break;
                }
            }

            return CapitalizeFirstLetterOfString(name);
        }

        public static string CapitalizeFirstLetterOfString(string text)
        {
            return char.ToUpper(text[0]) + text[1..];
        }

        public enum LetterGroups
        {
            consonants, startingBlends, endingBlends, vowels
        }

        private static string SelectRandomClass() => charClasses[_random.Next(0, charClasses.Count)];

        private static string SelectRandomRace()
        {
            //TODO: add logic for race dependent on class
            return races[_random.Next(0, races.Count)];
        }

        public static Attributes GenerateAttributes(string charClass, string race)
        {
            Attributes mins = GetAttributeMins(charClass);
            Attributes attributes = new()
            {
                Strength = AssignAttribute("Strength", race, mins.Strength),
                Intelligence = AssignAttribute("Intelligence", race, mins.Intelligence),
                Wisdom = AssignAttribute("Wisdom", race, mins.Wisdom),
                Dexterity = AssignAttribute("Dexterity", race, mins.Dexterity),
                Constitution = AssignAttribute("Constitution", race, mins.Constitution),
                Charisma = AssignAttribute("Charisma", race, mins.Charisma)
            };

            //fighters with 18 strength get Ex_Strength
            attributes.Ex_Strength = charClass == "fighter" && attributes.Strength == 18 ? _random.Next(1, 101) : 0;

            return attributes;
        }

        private static int AssignAttribute(string attribute, string race, int min)
        {
            int result;

            do
            {
                result = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7) + GetRacialAttributeModifier(attribute, race);
            } while (result < min);

            return result;
        }

        private static int GetRacialAttributeModifier(string attribute, string race)
        {
            switch (attribute)
            {
                case "Strength":
                    if(race == "halfling") { return -1; }
                    break;
                case "Constitution":
                    if (race == "dwarf") { return 1; }
                    if (race == "elf") { return -1; }
                    break;
                case "Dexterity":
                    if (race == "halfling" || race == "elf") { return 1; }
                    break;
                case "Charisma":
                    if (race == "dwarf") { return -1; }
                    break;
                default: break;
            }

            return 0;
        }

        private static Attributes GetAttributeMins(string charClass)
        {
            Attributes result = charClass switch
            {
                "fighter" => new Attributes() { Strength = 9},
                "paladin" => new Attributes() { Strength = 12, Intelligence = 9, Wisdom = 13, Constitution = 9, Charisma = 17 },
                "ranger" => new Attributes() { Strength = 13, Wisdom = 14, Constitution = 14 },
                "magic-user" => new Attributes() { Intelligence = 9 },
                "cleric" => new Attributes() { Wisdom = 9 },
                "druid" => new Attributes() { Wisdom = 12, Charisma = 15 },
                "thief" => new Attributes() { Dexterity = 9 },
                "assassin" => new Attributes() { Strength = 12, Intelligence = 11, Dexterity = 12 },
                "monk" => new Attributes() { Strength = 15, Wisdom = 15, Dexterity = 15, Constitution = 11 },
                _ => new Attributes() { }
            };

            return result;
        }

        public static List<int> GenerateHPByLevelByCharClass(string charClass, int level)
        {
            int dieType = charClass switch
            {
                "fighter" or "paladin" => 10,
                "cleric" or "druid" or "ranger" => 8,
                "thief" or "assassin" => 6,
                "magic-user" or "illusionist" or "monk" => 4,
                _ => 3
            };

            //first-level HD is maximum of range
            List<int> result = new() { dieType };

            //rangers and monks get two HD at first level
            if (charClass == "ranger" || charClass == "monk")
            {
                result[0] += dieType;
            }

            //HD values for levels beyond first assigned randomly
            for (int i = 1; i < level; i++)
            {
                result.Add(_random.Next(1, dieType + 1));
            }

            return result;
        }

        public static int CalcMaxHP(List<int> HPByLevel, int con, string charClass)
        {
            return HPByLevel.Sum() + CalcConBonusToHP(con, charClass);
        }

        public static int CalcConBonusToHP(int con, string charClass)
        {
            //TODO: refactor to use nested switches
            int result;

            if (charClass == "fighter" || charClass == "ranger" || charClass == "paladin")
            {
                result = con switch
                {
                    < 15 => 0,
                    15 => 1,
                    16 => 2,
                    17 => 3,
                    > 17 => 4
                };
            }
            else
            {
                result = con switch
                {
                    < 15 => 0,
                    15 => 1,
                    > 15 => 2
                };
            }

            return result;
        }

        private static string SelectRandomArmor(string charClass)
        {
            List<string> armorList = new() { "leather", "studded leather", "scale", "chain", "banded", "plate" };

            string result = charClass switch
            {
                "fighter" or "cleric" or "paladin" or "ranger" => armorList[_random.Next(0, armorList.Count)],
                "druid" or "assassin" or "thief" => "leather",
                _ => "none"
            };

            return result;
        }

        private static string SelectRandomWeapon(string charClass, int level)
        {
            //beyond level five, a monk has higher damage potential w/o a weapon
            if (charClass == "monk" && level > 5)
            {
                return "none";
            }

            return charClass switch
            {
                "magic-user" or "illusionist" => muWeaponList[_random.Next(0, muWeaponList.Count)],
                "cleric" => clericWeaponList[_random.Next(0, clericWeaponList.Count)],
                "druid" => druidWeaponList[_random.Next(0, druidWeaponList.Count)],
                "thief" => thiefWeaponList[_random.Next(0, thiefWeaponList.Count)],
                "monk" => monkWeaponList[_random.Next(0, monkWeaponList.Count)],
                "fighter" or "paladin" or "ranger" or "assassin" => fighterWeaponList[_random.Next(0, fighterWeaponList.Count)],
                _ => "none"
            };
        }

        private static bool DetermineShieldPresence(string charClass, string weapon)
        {
            List<string> classesThatCannotUseShield = new() { "magic-user", "illusionist" };
            List<string> weaponsThatRequireTwoHands = new() { "spear", "halberd", "staff", "two-handed sword" };

            bool result = true;
            if (classesThatCannotUseShield.Contains(charClass) || weaponsThatRequireTwoHands.Contains(weapon))
            {
                result = false;
            }

            return result;
        }

        private static List<string> GenerateSpellList(string charClass, int level)
        {
            List<String> result = charClass switch
            {
                "magic-user" => GenerateMUSpellList(level),
                "cleric" => GenerateClericSpellList(level),
                "paladin" => GeneratePaladinSpellList(level),
                "druid" => GenerateDruidSpellList(level),
                "ranger" => GenerateRangerSpellList(level),
                _ => new List<String>() { }
            };

            return result;
        }

        private static List<string> GenerateMUSpellList(int level)
        {
            return new List<string> { "magic missile", "sleep" };
        }

        private static List<string> GenerateClericSpellList(int level)
        {
            return new List<string> { "hold person", "cure light wounds" };
        }

        private static List<string> GeneratePaladinSpellList(int level)
        {
            return new List<string> { };
        }

        private static List<string> GenerateDruidSpellList(int level)
        {
            return new List<string> { "cure light wounds" };
        }

        private static List<string> GenerateRangerSpellList(int level)
        {
            return new List<string> { };
        }
    }
}
