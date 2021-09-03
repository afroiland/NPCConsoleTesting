using System;
using System.Collections.Generic;

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
            int level = _random.Next(_MinLevel, _MaxLevel + 1);
            int HP = _random.Next(_MinHP, _MaxHP + 1);
            int initMod = _random.Next(_MinInitMod, _MaxInitMod + 1);
            int AC = _random.Next(_MinAC, _MaxAC + 1);
            int thac0 = CalcThac0(charClass, level);
            int numberOfAttackDice = _random.Next(_MinNumberOfAttackDice, _MaxNumberOfAttackDice + 1);
            int typeOfAttackDie = _random.Next(_MinTypeOfAttackDie, _MaxTypeOfAttackDie + 1);
            int dmgModifier = _random.Next(_MinDmgModifier, _MaxDmgModifier + 1);
            List<string> spells = GenerateSpellList(charClass, level);

            return new Combatant(name, charClass, level, HP, initMod, AC, thac0, numberOfAttackDice, typeOfAttackDie, dmgModifier, spells);
        }

        public static Combatant BuildCombatantViaConsole()
        {
            Console.WriteLine("Enter name for character");
            string name = Console.ReadLine();

            Console.WriteLine("Enter class for character");
            string charClass = Console.ReadLine();

            Console.WriteLine("Enter level for character");
            int level = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter HP for character");
            int HP = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character");
            int initMod = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter AC for character");
            int AC = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter thac0 for character");
            int thac0 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter numberOfAttackDice for character");
            int numberOfAttackDice = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter typeOfAttackDie for character");
            int typeOfAttackDie = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter dmgModifier for character");
            int dmgModifier = int.Parse(Console.ReadLine());

            //TODO: spells?

            return new Combatant(name, charClass, level, HP, initMod, AC, thac0, numberOfAttackDice, typeOfAttackDie, dmgModifier);
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

            //CombatantBuilder cBuilder = new();
            List<Combatant> combatants = new();

            while (combatants.Count < numberBattling)
            {
                if (charOrigin == 2)
                {
                    try
                    {
                        combatants.Add(BuildCombatantViaConsole());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("That didn't work. Try again.");
                    }

                }
                else if (charOrigin == 3)
                {
                    string name = CombatantRetriever.GetNameFromUserInput();
                    try
                    {
                        combatants.Add(CombatantRetriever.GetCombatantByName(connectionString, name));
                    }
                    catch (Exception ex)
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
            return charClasses[_random.Next(1, charClasses.Count)];
        }

        public static int CalcThac0(string charClass, int level)
        {
            List<int> MUThac0s = new() { 20, 20, 20, 20, 20, 19, 19, 19, 19, 19, 16, 16, 16, 16, 16, 13, 13, 13, 13, 13, 11 };
            List<int> ThiefThac0s = new() { 20, 20, 20, 20, 19, 19, 19, 19, 16, 16, 16, 16, 14, 14, 14, 14, 12, 12, 12, 12, 10 };

            int result = charClass switch
            {
                "Fighter" or "Paladin" or "Ranger" or "Monster" => 21 - level,
                "Magic-User" or "Illusionist" => MUThac0s[level - 1],
                "Cleric" or "Monk" or "Druid" => 20 - (int)(Math.Floor((level - 1) / 3d) * 2),
                "Thief" or "Assassin" => ThiefThac0s[level - 1],
                _ => 20
            };
            return result;
        }

        public static int CalcAC(string armor, int dex)
        {
            return 10;
        }

        private List<string> GenerateSpellList(string charClass, int level)
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

        private List<string> GenerateMUSpellList(int level)
        {
            return new List<string> { "Magic Missile", "Sleep" };
        }

        private List<string> GenerateClericSpellList(int level)
        {
            return new List<string> { "Hold Person", "Cure Light Wounds" };
        }

        private List<string> GeneratePaladinSpellList(int level)
        {
            return new List<string> { };
        }

        private List<string> GenerateDruidSpellList(int level)
        {
            return new List<string> { "Cure Light Wounds" };
        }

        private List<string> GenerateRangerSpellList(int level)
        {
            return new List<string> { };
        }
    }
}
