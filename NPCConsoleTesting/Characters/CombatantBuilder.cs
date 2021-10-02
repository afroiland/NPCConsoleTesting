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
        private int _MinThac0;
        private int _MaxThac0;
        private int _MinNumberOfAttackDice;
        private int _MaxNumberOfAttackDice;
        private int _MinTypeOfAttackDie;
        private int _MaxTypeOfAttackDie;
        private int _MinDmgModifier;
        private int _MaxDmgModifier;
        private int _MinLevel;
        private int _MaxLevel;

        public int MinHP { get => _MinHP; set => _MinHP = value; }
        public int MaxHP { get => _MaxHP; set => _MaxHP = value; }
        public int MinInitMod { get => _MinInitMod; set => _MinInitMod = value; }
        public int MaxInitMod { get => _MaxInitMod; set => _MaxInitMod = value; }
        public int MinAC { get => _MinAC; set => _MinAC = value; }
        public int MaxAC { get => _MaxAC; set => _MaxAC = value; }
        public int MinThac0 { get => _MinThac0; set => _MinThac0 = value; }
        public int MaxThac0 { get => _MaxThac0; set => _MaxThac0 = value; }
        public int MinNumberOfAttackDice { get => _MinNumberOfAttackDice; set => _MinNumberOfAttackDice = value; }
        public int MaxNumberOfAttackDice { get => _MaxNumberOfAttackDice; set => _MaxNumberOfAttackDice = value; }
        public int MinTypeOfAttackDie { get => _MinTypeOfAttackDie; set => _MinTypeOfAttackDie = value; }
        public int MaxTypeOfAttackDie { get => _MaxTypeOfAttackDie; set => _MaxTypeOfAttackDie = value; }
        public int MinDmgModifier { get => _MinDmgModifier; set => _MinDmgModifier = value; }
        public int MaxDmgModifier { get => _MaxDmgModifier; set => _MaxDmgModifier = value; }
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
            MinThac0 = 1;
            MaxThac0 = 20;
            MinNumberOfAttackDice = 1;
            MaxNumberOfAttackDice = 2;
            MinTypeOfAttackDie = 1;
            MaxTypeOfAttackDie = 6;
            MinDmgModifier = 0;
            MaxDmgModifier = 2;
            MinLevel = 1;
            MaxLevel = 5;
        }

        const int MIN_NAME_PATTERN_LENGTH = 3;
        const int MAX_NAME_PATTERN_LENGTH = 7;

        static Random _random = new();

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
            //set currentHP to maxHP (sum of HPByLevel values + con bonus)
            int currentHP = HPByLevel.Sum() + CombatMethods.CalcConBonusToHP(con, charClass);
            //int initMod = 0;
            string armor = SelectRandomArmor(charClass);
            string weapon = SelectRandomWeapon(charClass);
            bool hasShield = DetermineShieldPresence(charClass, weapon);
            List<string> spells = GenerateSpellList(charClass, level);

            return new Combatant(name, charClass, level, str, dex, con, HPByLevel, currentHP, ex_str, charArmor: armor,
                charWeapon: weapon, charHasShield: hasShield, charSpells: spells);
        }

        public static Combatant BuildCombatantViaConsole(int charNumber)
        {
            string name = GetNameFromUserInput(charNumber);

            Console.WriteLine($"Enter class for character {charNumber}");
            string charClass = Console.ReadLine();

            Console.WriteLine($"Enter level for character {charNumber}");
            int level = int.Parse(Console.ReadLine());

            Console.WriteLine($"Enter HP for character {charNumber}");
            int HP = int.Parse(Console.ReadLine());

            Console.WriteLine($"Enter initMod for character {charNumber}");
            int initMod = int.Parse(Console.ReadLine());

            Console.WriteLine($"Enter weapon for character {charNumber}");
            string weapon = Console.ReadLine();

            //TODO: spells?

            //TODO: figure out what we do for HP_By_Level here
            return new Combatant(name, charClass, level, 12, 12, 12, new List<int>() { 1 }, HP, initMod, charWeapon:weapon);
        }

        public List<Combatant> BuildListOfCombatants(string connectionString)
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
                        Console.WriteLine("Must be at least two");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("We're looking for an integer");
                }
            }
            
            //Set the source for all characters
            Console.WriteLine("1 = Random, 2 = Custom, 3 = Get from db");
            int charOrigin = 0;
            bool intEntered = false;
            while (!intEntered)
            {
                try
                {
                    charOrigin = int.Parse(Console.ReadLine());
                    intEntered = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("We're looking for an integer");
                }
            }

            List<Combatant> combatants = new();

            while (combatants.Count < numberBattling)
            {
                if (charOrigin == 2)
                {
                    try
                    {
                        combatants.Add(BuildCombatantViaConsole(combatants.Count +1));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }

                }
                else if (charOrigin == 3)
                {
                    string name = GetNameFromUserInput(combatants.Count + 1);
                    try
                    {
                        combatants.Add(CombatantRetriever.GetCombatantByName(connectionString, name));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }
                }
                else
                {
                    combatants.Add(BuildCombatantRandomly());
                }
            }

            return combatants;
        }

        public static string GetNameFromUserInput(int charNumber)
        {
            //Console.WriteLine($"Enter the character's name.");
            Console.WriteLine($"Enter name for character {charNumber}");
            return Console.ReadLine();
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

            //Capitalize first letter and return
            return char.ToUpper(name[0]) + name[1..];
        }

        public enum LetterGroups
        {
            consonants, startingBlends, endingBlends, vowels
        }

        private static string SelectRandomClass()
        {
            List<string> charClasses = new() {"Fighter", "Paladin", "Ranger", "Magic-User", "Cleric", "Monk", "Druid", "Thief", "Assassin"};
            return charClasses[_random.Next(0, charClasses.Count)];
        }

        private static string SelectRandomRace()
        {
            List<string> races = new() { "Human", "Elf", "Dwarf", "Halfling" };
            return races[_random.Next(0, races.Count)];
        }

        public static Attributes GenerateAttributes(string charClass, string race)
        {
            Attributes mins = GetAttributeMins(charClass);
            Attributes attributes = new();

            do
            {
                attributes.Strength = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7) + GetRacialAttributeModifier("Strength", race);
            } while (attributes.Strength < mins.Strength);
            
            do
            {
                attributes.Intelligence = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7);
            } while (attributes.Intelligence < mins.Intelligence);

            do
            {
                attributes.Constitution = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7) + GetRacialAttributeModifier("Constitution", race);
            } while (attributes.Constitution < mins.Constitution);

            do
            {
                attributes.Wisdom = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7);
            } while (attributes.Wisdom < mins.Wisdom);

            do
            {
                attributes.Dexterity = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7) + GetRacialAttributeModifier("Dexterity", race);
            } while (attributes.Dexterity < mins.Dexterity);

            do
            {
                attributes.Charisma = _random.Next(1, 7) + _random.Next(1, 7) + _random.Next(1, 7) + GetRacialAttributeModifier("Charisma", race);
            } while (attributes.Charisma < mins.Charisma);

            //Fighters with 18 strength get Ex_Strength
            attributes.Ex_Strength = charClass == "Fighter" && attributes.Strength == 18 ? _random.Next(1, 101) : 0;

            return attributes;
        }

        private static int GetRacialAttributeModifier(string attribute, string race)
        {
            switch (attribute)
            {
                case "Strength":
                    if(race == "Halfling") { return -1; }
                    break;
                case "Constitution":
                    if (race == "Dwarf") { return 1; }
                    if (race == "Elf") { return -1; }
                    break;
                case "Dexterity":
                    if (race == "Halfling" || race == "Elf") { return 1; }
                    break;
                case "Charisma":
                    if (race == "Dwarf") { return -1; }
                    break;
                default: break;
            }

            return 0;
        }

        private static Attributes GetAttributeMins(string charClass)
        {
            Attributes result = charClass switch
            {
                "Fighter" => new Attributes() { Strength = 9},
                "Paladin" => new Attributes() { Strength = 12, Intelligence = 9, Wisdom = 13, Constitution = 9, Charisma = 17 },
                "Ranger" => new Attributes() { Strength = 13, Wisdom = 14, Constitution = 14 },
                "Magic-User" => new Attributes() { Intelligence = 9 },
                "Cleric" => new Attributes() { Wisdom = 9 },
                "Druid" => new Attributes() { Wisdom = 12, Charisma = 15 },
                "Thief" => new Attributes() { Dexterity = 9 },
                "Assassin" => new Attributes() { Strength = 12, Intelligence = 11, Dexterity = 12 },
                "Monk" => new Attributes() { Strength = 15, Wisdom = 15, Dexterity = 15, Constitution = 11 },                
                _ => new Attributes() { }
            };

            return result;
        }

        public static List<int> GenerateHPByLevelByCharClass(string charClass, int level)
        {
            int dieType = charClass switch
            {
                "Fighter" or "Paladin" => 10,
                "Cleric" or "Druid" or "Ranger" => 8,
                "Thief" or "Assassin" => 6,
                "Magic-User" or "Illusionist" or "Monk" => 4,
                _ => 3
            };

            //first-level HD is maximum of range
            List<int> result = new() { dieType };

            //rangers and monks get two HD at first level
            if (charClass == "Ranger" || charClass == "Monk")
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

        private static string SelectRandomArmor(string charClass)
        {
            List<string> armorList = new() { "Leather", "Studded Leather", "Scale Mail", "Chain Mail", "Banded Mail", "Plate Mail" };

            string result = charClass switch
            {
                "Fighter" or "Cleric" or "Paladin" or "Ranger" => armorList[_random.Next(0, armorList.Count)],
                "Druid" or "Assassin" or "Thief" => "Leather",
                _ => "None"
            };

            return result;
        }

        private static string SelectRandomWeapon(string charClass)
        {
            List<string> weaponList = new() { };

            return "Dagger";
        }

        private static bool DetermineShieldPresence(string charClass, string weapon)
        {
            List<string> classesThatCannotUseShield = new() { "Magic-User", "Illusionist" };
            List<string> weaponsThatRequireTwoHands = new() { "Spear", "Halberd", "Staff", "Two-Handed Sword" };

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
                "Magic-User" => GenerateMUSpellList(level),
                "Cleric" => GenerateClericSpellList(level),
                "Paladin" => GeneratePaladinSpellList(level),
                "Druid" => GenerateDruidSpellList(level),
                "Ranger" => GenerateRangerSpellList(level),
                _ => new List<String>() { }
            };

            return result;
        }

        private static List<string> GenerateMUSpellList(int level)
        {
            return new List<string> { "Magic Missile", "Sleep" };
        }

        private static List<string> GenerateClericSpellList(int level)
        {
            return new List<string> { "Hold Person", "Cure Light Wounds" };
        }

        private static List<string> GeneratePaladinSpellList(int level)
        {
            return new List<string> { };
        }

        private static List<string> GenerateDruidSpellList(int level)
        {
            return new List<string> { "Cure Light Wounds" };
        }

        private static List<string> GenerateRangerSpellList(int level)
        {
            return new List<string> { };
        }
    }
}
