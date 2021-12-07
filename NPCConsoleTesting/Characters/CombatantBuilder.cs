using NPCConsoleTesting.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatantBuilder : ICombatantBuilder
    {
        private const int DefaultMinLevel = 1;
        private const int DefaultMaxLevel = 7;
        private const int DefaultMaxCombatantsForSingleCombat = 1000;
        private const int DefaultMaxCombatantsForMultipleCombats = 10;
        private const int MinNamePatternLength = 3;
        private const int MaxNamePatternLength = 7;

        static List<string> charClasses = new() { "fighter", "paladin", "ranger", "magic-user", "cleric", "monk", "druid", "thief", "assassin" };
        static List<string> races = new() { "human", "elf", "dwarf", "halfling" };
        static List<string> muWeaponList = new() { "dagger", "darts", "staff" };
        static List<string> clericWeaponList = new() { "club", "flail", "hammer", "mace", "staff" };
        static List<string> druidWeaponList = new() { "club", "dagger", "darts", "hammer", "spear", "staff" };
        static List<string> thiefWeaponList = new() { "club", "dagger", "darts", "longsword", "shortsword" };
        static List<string> monkWeaponList = new() { "club", "darts", "dagger", "staff", "none" };
        static List<string> fighterWeaponList = new() { "axe", "halberd", "longsword", "shortsword", "spear",
            "two-handed sword", "dagger", "darts", "staff", "club", "flail", "hammer", "mace", "none" };
        static List<string> armorList = new() { "none", "leather", "studded leather", "scale", "chain", "banded", "plate" };
        static List<string> affiliationList = new() { "The Crown", "The Church", "House Tellerue", "Oriyama Clan" };

        static Random _random = new();

        ICombatantRetriever _combatantRetriever;

        private int _MinLevel;
        private int _MaxLevel;
        private int _MaxCombatantsForSingleCombat;
        private int _MaxCombatantsForMultipleCombats;

        public int MinLevel { get => _MinLevel; set => _MinLevel = value; }
        public int MaxLevel { get => _MaxLevel; set => _MaxLevel = value; }
        public int MaxCombatantsForSingleCombat { get => _MaxCombatantsForSingleCombat; set => _MaxCombatantsForSingleCombat = value; }
        public int MaxCombatantsForMultipleCombats { get => _MaxCombatantsForMultipleCombats; set => _MaxCombatantsForMultipleCombats = value; }

        public CombatantBuilder(ICombatantRetriever combatantRetriever, int minLevel = DefaultMinLevel, int maxLevel = DefaultMaxLevel,
            int maxCombatantsForSingleCombat = DefaultMaxCombatantsForSingleCombat,
            int maxCombatantsForMultipleCombats = DefaultMaxCombatantsForMultipleCombats)
        {
            _combatantRetriever = combatantRetriever;
            MinLevel = minLevel;
            MaxLevel = maxLevel;
            MaxCombatantsForSingleCombat = maxCombatantsForSingleCombat;
            MaxCombatantsForMultipleCombats = maxCombatantsForMultipleCombats;
        }

        public List<Combatant> BuildListOfCombatants(string connectionString, int numberBattling)
        {
            int retrievalMethod = DetermineRetrievalMethod();

            List<Combatant> combatants = new();
            while (combatants.Count < numberBattling)
            {
                combatants.Add(GetCombatant(retrievalMethod, combatants.Count, connectionString));
            }

            return combatants;
        }

        public int DetermineNumberBattling(bool doingMultipleCombats)
        {
            int maxNumberOfCombatants = doingMultipleCombats ? _MaxCombatantsForMultipleCombats : _MaxCombatantsForSingleCombat;
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

                    if (numberBattling > maxNumberOfCombatants)
                    {
                        Console.WriteLine($"Current settings won't allow for that many. Maximum number of combatants is {maxNumberOfCombatants} at this time.");
                        numberBattling = 0;
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

        private static int DetermineRetrievalMethod()
        {
            Console.WriteLine("How shall the combatants be selected? 1 = Random, 2 = Custom, 3 = Get from db.");
            return GetPositiveIntFromUser();
        }

        public static int GetPositiveIntFromUser()
        {
            //TODO: refactor / clean up. Should this method take an error message param?
            int integer = 0;
            while (integer < 1)
            {
                bool exceptionThrown = false;
                try
                {
                    integer = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("We're looking for a positive integer...");
                    exceptionThrown = true;
                }

                if (integer < 1 && !exceptionThrown)
                {
                    Console.WriteLine("We're looking for a positive integer...");
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
                        result.Add(_combatantRetriever.GetCombatantByName(connectionString, name));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }
                }
                else
                {
                    result.Add(BuildCombatantRandomly());
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
            List<int> HPByLevel = GetHPByLevel(charClass, level);
            int currentHP = CalcFullHP(HPByLevel, con, charClass);
            //int initMod = 0;
            string armor = SelectRandomArmor(charClass);
            string weapon = SelectRandomWeapon(charClass, level);
            bool hasShield = DetermineShieldPresence(charClass, weapon);
            List<string> spells = GenerateSpellList(charClass, level);
            string affiliation = SelectRandomAffiliation();

            return new Combatant(name, charClass, level, race, str, dex, con, HPByLevel, currentHP, charEx_Strength: ex_str, charArmor: armor,
                charWeapon: weapon, charHasShield: hasShield, charSpells: spells, charAffiliation: affiliation);
        }

        public Combatant BuildCombatantViaConsole(int charNumber)
        {
            string name = GetName(charNumber);
            string charClass = GetCharClass(name);
            string race = GetRace(name, charClass);
            int level = GetLevel(name, _MinLevel, _MaxLevel);
            Attributes attributes = GenerateAttributes(charClass, race);
            int str = attributes.Strength;
            int ex_str = attributes.Ex_Strength;
            int dex = attributes.Dexterity;
            int con = attributes.Constitution;
            //TODO: option to build HPByLevel to sum to a specific number?
            List<int> HPByLevel = GetHPByLevel(charClass, level);
            int currentHP = CalcFullHP(HPByLevel, con, charClass);
            //int initMod = GetPositiveIntFromUser();
            string weapon = GetWeapon(name, charClass, level);
            string armor = GetArmor(name, charClass);
            bool hasShield = DetermineShieldPresence(charClass, weapon);
            //TODO: option to determine spells from user input?
            List<string> spells = GenerateSpellList(charClass, level);
            string affiliation = GetAffiliation(name);

            return new Combatant(name, charClass, level, race, str, dex, con, HPByLevel, currentHP, charEx_Strength: ex_str, charArmor: armor,
                charWeapon: weapon, charHasShield: hasShield, charSpells: spells, charAffiliation: affiliation);
        }

        private List<int> GetHPByLevel(string charClass, int level)
        {
            List<int> result = new();
            bool givingMax = true;  //DetermineIfGivingMax();

            result = GenerateHPByLevelByCharClass(charClass, level, givingMax);

            return result;
        }

        public static string GetName(int charNumber)
        {
            Console.WriteLine($"Generate name for character {charNumber} randomly or enter a custom name? 1 = Random, 2 = Custom.");
            int nameCreationTechnique = GetPositiveIntFromUser();
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

        private static string GetCharInfoStringFromUser(string property, string name, string charClass = "", int minLevel = 1, int maxLevel = 1)
        {
            string inputString = "";

            try
            {
                Console.WriteLine($"Enter {property} for {name}:");
                inputString = Console.ReadLine().ToLower();

                inputString = property switch
                {
                    "class" => CheckInputForCharClass(inputString),
                    "race" => CheckInputForRace(inputString, charClass),
                    "level" => CheckInputForLevel(inputString, minLevel, maxLevel),
                    "weapon" => CheckInputForWeapon(inputString, charClass),
                    "armor" => CheckInputForArmor(inputString, charClass),
                    "affiliation" => inputString,
                    _ => ""
                };
            }
            catch (Exception)
            {
                Console.WriteLine("That didn't work. Try again.");
            }

            return inputString;
        }

        private static string CheckInputForCharClass(string charClass)
        {
            string charClassToBeChecked = charClass;

            List<string> muNames = new() { "wizard", "mage", "magic user" };
            charClassToBeChecked = muNames.Contains(charClassToBeChecked) ? "magic-user" : charClassToBeChecked;

            if (!charClasses.Contains(charClassToBeChecked))
            {
                string randomClass = SelectRandomClass();
                Console.WriteLine($"That class isn't recognized; try something else. Perhaps {DetermineIndefiniteArticle(randomClass)} {randomClass}?");
            }

            return charClassToBeChecked;
        }

        private static string CheckInputForRace(string race, string charClass)
        {
            //though currently unneeded, the following string can be used if we want to add logic (cf. CheckInputForCharClass())
            string raceToBeChecked = race;

            if (!races.Contains(raceToBeChecked))
            {
                string randomRace = SelectRandomRace();
                Console.WriteLine($"That race isn't recognized; try something else. Perhaps {DetermineIndefiniteArticle(randomRace)} {randomRace}?");
            }

            return raceToBeChecked;
        }

        private static string CheckInputForLevel(string level, int minLevel, int maxLevel)
        {
            //TODO: this seems suboptimal
            int levelToBeChecked = 0;

            try
            {
                levelToBeChecked = int.Parse(level);
            }
            catch (Exception)
            {
                Console.WriteLine("Level must be a positive integer.");
                return "0";
            }

            if (levelToBeChecked < minLevel)
            {
                Console.WriteLine($"Current settings won't allow for a character of that level. Minimum level is {minLevel} at this time.");
            }

            if (levelToBeChecked > maxLevel)
            {
                Console.WriteLine($"Current settings won't allow for a character of that level. Maximum level is {maxLevel} at this time.");
            }

            return levelToBeChecked.ToString();
        }

        private static string CheckInputForWeapon(string weapon, string charClass)
        {
            //though currently unneeded, the following string can be used if we want to add logic (cf. CheckInputForCharClass())
            string weaponToBeChecked = weapon;

            if (!WeaponIsAppropriate(charClass, weaponToBeChecked))
            {
                string suggestion = SelectRandomWeapon(charClass, 1);
                string article = suggestion == "darts" || suggestion == "none" ? "" : DetermineIndefiniteArticle(suggestion) + " ";
                Console.WriteLine($"That's not an appropriate weapon for {DetermineIndefiniteArticle(charClass)} {charClass}. Perhaps {article}{suggestion}?");
            }

            return weaponToBeChecked;
        }

        private static string CheckInputForArmor(string armor, string charClass)
        {
            string armorToBeChecked = armor;

            armorToBeChecked = armorToBeChecked.Contains("mail") ? armorToBeChecked.Replace("mail", "") : armorToBeChecked;
            armorToBeChecked = armorToBeChecked.Contains("armor") ? armorToBeChecked.Replace("armor", "") : armorToBeChecked;
            armorToBeChecked = armorToBeChecked.Contains(" ") ? armorToBeChecked.Replace(" ", "") : armorToBeChecked;

            if (!ArmorIsAppropriate(charClass, armorToBeChecked))
            {
                string suggestion = SelectRandomArmor(charClass);
                Console.WriteLine($"That's not appropriate armor for {DetermineIndefiniteArticle(charClass)} {charClass}. Perhaps {suggestion}?");
            }

            return armorToBeChecked;
        }

        public static string GetCharClass(string name)
        {
            Console.WriteLine($"Determine class for {name} randomly or enter manually? 1 = Random, 2 = Manually.");
            int classSelectionTechnique = GetPositiveIntFromUser();

            string charClass = "";
            while (!charClasses.Contains(charClass))
            {
                charClass = classSelectionTechnique == 2 ? GetCharInfoStringFromUser("class", name) : SelectRandomClass();
            }

            Console.WriteLine($"Very well, {name} shall be {DetermineIndefiniteArticle(charClass)} {charClass}.");
            Console.WriteLine();

            return charClass;
        }

        public static string GetRace(string name, string charClass)
        {
            Console.WriteLine($"Determine race for {name} randomly or enter manually? 1 = Random, 2 = Manually.");
            int raceSelectionTechnique = GetPositiveIntFromUser();

            string race = "";
            while (!races.Contains(race))
            {
                race = raceSelectionTechnique == 2 ? GetCharInfoStringFromUser("race", name, charClass) : SelectRandomRace();
            }

            Console.WriteLine($"Very well, {name} shall be {DetermineIndefiniteArticle(race)} {race}.");
            Console.WriteLine();

            return race;
        }

        public static int GetLevel(string name, int minLevel, int maxLevel)
        {
            Console.WriteLine($"Determine level for {name} randomly or enter manually? 1 = Random, 2 = Manually.");
            int levelSelectionTechnique = GetPositiveIntFromUser();

            int level = 0;
            while (level < minLevel || level > maxLevel)
            {
                level = levelSelectionTechnique == 2 ? int.Parse(GetCharInfoStringFromUser("level", name, minLevel: minLevel, maxLevel: maxLevel))
                    : _random.Next(minLevel, maxLevel + 1);
            }

            Console.WriteLine($"Very well, {name}'s level shall be {level}.");
            Console.WriteLine();

            return level;
        }

        public static string GetWeapon(string name, string charClass, int level)
        {
            Console.WriteLine($"Determine {name}'s weapon randomly or enter manually? 1 = Random, 2 = Manually.");
            int weaponSelectionTechnique = GetPositiveIntFromUser();

            string weapon = "";
            string article;
            while (!WeaponIsAppropriate(charClass, weapon))
            {
                weapon = weaponSelectionTechnique == 2 ? GetCharInfoStringFromUser("weapon", name, charClass) : SelectRandomWeapon(charClass, level);
            }

            article = weapon == "darts" ? "" : DetermineIndefiniteArticle(weapon) + " ";
            Console.WriteLine($"Very well, {name} shall wield {article}{weapon}.");
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

        public static string GetArmor(string name, string charClass)
        {
            Console.WriteLine($"Determine {name}'s armor randomly or enter manually? 1 = Random, 2 = Manually.");
            int armorSelectionTechnique = GetPositiveIntFromUser();

            string armor = "";
            while (!ArmorIsAppropriate(charClass, armor))
            {
                armor = armorSelectionTechnique == 2 ? GetCharInfoStringFromUser("armor", name, charClass) : SelectRandomArmor(charClass);
            }

            if (armor == "none")
            {
                Console.WriteLine($"Very well, {name} shall wear no armor.");
            }
            else
            {
                string armorType = armor.Contains("leather") ? "armor" : "mail";
                Console.WriteLine($"Very well, {name} shall wear {armor} {armorType}.");
            }
            Console.WriteLine();

            return armor;
        }

        private static bool ArmorIsAppropriate(string charClass, string armor)
        {
            return charClass switch
            {
                "magic-user" or "illusionist" or "monk" => armor == "none",
                "thief" or "druid" or "assassin" => armor == "none" || armor == "leather",
                "fighter" or "cleric" or "paladin" or "ranger" => armorList.Contains(armor),
                _ => false
            };
        }

        private static string GetAffiliation(string name)
        {
            Console.WriteLine($"Determine {name}'s affiliation randomly or enter manually? 1 = Random, 2 = Manually.");
            int affiliationSelectionTechnique = GetPositiveIntFromUser();

            string affiliation = "";
            while (affiliation == "")
            {
                affiliation = affiliationSelectionTechnique == 2 ? GetCharInfoStringFromUser("affiliation", name) : SelectRandomAffiliation();
            }

            Console.WriteLine($"Very well, {name} shall fight for {affiliation}.");
            Console.WriteLine();

            return affiliation;
        }

        public static string GenerateRandomName()
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "qu", "r", "s", "t", "v", "w", "x", "z" };
            string[] startingBlends = { "bl", "br", "cl", "cr", "dr", "fl", "fr", "gl", "gr", "pl", "pr", "sl",
                "sn", "sw", "tr", "tw", "wh", "wr", "scr", "shr", "sph", "spl", "spr", "squ", "str", "thr" };
            string[] endingBlends = { "ch", "sc", "sh", "sk", "sm", "sp", "st", "th", "sch" };
            char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };
            string[] doubleVowels = { "aa", "ae", "ai", "ao", "au", "ea", "ee", "ei", "eo", "eu", "ia", "ie",
                "io", "iu", "oa", "oe", "oi", "oo", "ou", "ua", "ue", "ui", "uo" };

            int patternLength = _random.Next(MinNamePatternLength, MaxNamePatternLength);
            //the pattern will be a list of ints with values between 0 and 3, each int corresponding to a LetterGroup (consonants, etc.)

            //initialize the pattern with a random int between 0 and 3
            List<int> pattern = new() { _random.Next(0, 4) };
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

        private static string DetermineIndefiniteArticle(string text)
        {
            char[] nonY_Vowels = { 'a', 'e', 'i', 'o', 'u', };
            return nonY_Vowels.Contains(text[0]) ? "an" : "a";
        }

        private static string SelectRandomClass() => charClasses[_random.Next(0, charClasses.Count)];

        private static string SelectRandomRace()
        {
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
                    if (race == "halfling") { return -1; }
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
                "fighter" => new Attributes() { Strength = 9 },
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

        public static List<int> GenerateHPByLevelByCharClass(string charClass, int level, bool givingMax)
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
                if (givingMax)
                {
                    result.Add(dieType);
                }
                else
                {
                    result.Add(_random.Next(1, dieType + 1));
                }
            }

            return result;
        }

        public int CalcFullHP(List<int> HPByLevel, int con, string charClass)
        {
            return HPByLevel.Sum() + CalcConBonusToHP(charClass, con);
        }

        public static int CalcConBonusToHP(string charClass, int con)
        {
            return charClass switch
            {
                "fighter" or "ranger" or "paladin" => con switch
                {
                    < 15 => 0,
                    15 => 1,
                    16 => 2,
                    17 => 3,
                    > 17 => 4
                },
                _ => con switch
                {
                    < 15 => 0,
                    15 => 1,
                    > 15 => 2
                }
            };
        }

        private static string SelectRandomArmor(string charClass)
        {
            return charClass switch
            {
                //the indices here are intentional to exclude certain options
                "fighter" or "cleric" or "paladin" or "ranger" => armorList[_random.Next(1, armorList.Count)],
                "druid" or "assassin" or "thief" => armorList[_random.Next(0, 2)],
                _ => "none"
            };
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
                "fighter" or "paladin" or "ranger" or "assassin" => fighterWeaponList[_random.Next(0, 6)],
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

        private static string SelectRandomAffiliation()
        {
            return affiliationList[_random.Next(0, affiliationList.Count)];
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
