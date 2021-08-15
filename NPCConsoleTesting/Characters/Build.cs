using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public static class Build
    {
        static Random _random = new();

        public static Combatant BuildCombatantRandomly()
        {
            string name = GenerateRandomName();
            //string name = "random" + _random.Next(10000, 100000).ToString();
            int HP = _random.Next(1, 11);
            int initMod = _random.Next(1, 6);
            int AC = _random.Next(-10, 11);
            int thac0 = _random.Next(1, 21);
            int numberOfDice = _random.Next(1, 3);
            int typeOfDie = _random.Next(1, 7);
            int modifier = _random.Next(1, 3);

            return new(name, HP, initMod, AC, thac0, numberOfDice, typeOfDie, modifier);
        }

        public static Combatant BuildCombatantViaConsole()
        {
            Console.WriteLine("Enter name for character");
            string name = Console.ReadLine();

            Console.WriteLine("Enter HP for character");
            int HP = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character");
            int initMod = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter AC for character");
            int AC = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter thac0 for character");
            int thac0 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter numberOfDice for character");
            int numberOfDice = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter typeOfDie for character");
            int typeOfDie = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter modifier for character");
            int modifier = int.Parse(Console.ReadLine());

            return new(name, HP, initMod, AC, thac0, numberOfDice, typeOfDie, modifier);
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

            int patternLength = _random.Next(3, 7);
            //the pattern will be a list of ints between 0 and 3 that each correspond to a LetterGroup (consonants, etc.)

            //initialize the pattern with a random int between 0 and 3
            List<int> pattern = new() {_random.Next(0, 4)};
            string name = new("");

            //add random ints to the pattern according to patternLength
            while (pattern.Count < patternLength)
            {
                //avoid assigning multiple vowels or non-vowels in a row
                if (pattern[pattern.Count - 1] != (int)LetterGroups.vowels)
                {
                    pattern.Add((int)LetterGroups.vowels);
                }
                else
                {
                    //0, 1 and 2 all correspond to non-vowels
                    pattern.Add(_random.Next(0, 3));
                }
                //ensure pattern does not end with a startingBlend
                if (pattern[pattern.Count - 1] == (int)LetterGroups.startingBlends)
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
                        name += consonants[_random.Next(1, consonants.Length - 1)];
                        break;
                    case (int)LetterGroups.startingBlends:
                        name += startingBlends[_random.Next(1, startingBlends.Length - 1)];
                        break;
                    case (int)LetterGroups.endingBlends:
                        name += endingBlends[_random.Next(1, endingBlends.Length - 1)];
                        break;
                    case (int)LetterGroups.vowels:
                        //use single vowels slightly more often (60% of the time) than double vowels
                        if (_random.Next(1, 11) > 4)
                        {
                            name += vowels[_random.Next(1, vowels.Length - 1)];
                        }
                        else
                        {
                            name += doubleVowels[_random.Next(1, doubleVowels.Length - 1)];
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
    }
}
