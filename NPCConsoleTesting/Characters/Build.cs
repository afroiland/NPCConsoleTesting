using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Build
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
            //TODO: This method could use a lot of cleanup. Enum, comments, etc.
            string[] consonants = {"b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "qu", "r", "s", "t", "v", "w", "x", "z"};
            string[] startingBlends = {"bl", "br", "cl", "cr", "dr", "fl", "fr", "gl", "gr", "pl", "pr", "sl",
                "sn", "sw", "tr", "tw", "wh", "wr", "scr", "shr", "sph", "spl", "spr", "squ", "str", "thr"};
            string[] endingBlends = { "ch", "sc", "sh", "sk", "sm", "sp", "st", "th", "sch"};
            string[] vowels = {"a", "e", "i", "o", "u", "y"};
            string[] doubleVowels = {"aa", "ae", "ai", "ao", "au", "ea", "ee", "ei", "eo", "eu", "ia", "ie",
                "io", "iu", "oa", "oe", "oi", "oo", "ou", "ua", "ue", "ui", "uo"};

            int patternLength = _random.Next(3, 7);
            List<int> pattern = new() {_random.Next(1, 5)};
            string name = new("");

            //determine pattern
            while (pattern.Count < patternLength)
            {
                if (pattern[pattern.Count - 1] != 4)
                {
                    pattern.Add(4);
                }
                else
                {
                    pattern.Add(_random.Next(1, 4));
                }
                if (pattern[pattern.Count - 1] == 2)
                {
                    pattern.RemoveAt(pattern.Count - 1);
                }
            }

            //add letter strings according to pattern
            for (int i = 0; i < pattern.Count; i++)
            {
                switch (pattern[i])
                {
                    case 1:
                        name += consonants[_random.Next(1, consonants.Length - 1)];
                        break;
                    case 2:
                        name += startingBlends[_random.Next(1, startingBlends.Length - 1)];
                        break;
                    case 3:
                        name += endingBlends[_random.Next(1, endingBlends.Length - 1)];
                        break;
                    case 4:
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
    }
}
