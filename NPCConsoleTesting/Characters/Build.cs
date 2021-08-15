using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Build
    {
        public static Combatant BuildCombatantRandomly()
        {
            Random _random = new();

            string name = "random" + _random.Next(10000, 100000).ToString();
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
    }
}
