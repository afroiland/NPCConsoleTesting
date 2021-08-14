using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Build
    {
        public static Combatant BuildCombatant()
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

            //Combatant npc = new(name, HP, initMod, AC, thac0, numberOfDice, typeOfDie, modifier);
            return new(name, HP, initMod, AC, thac0, numberOfDice, typeOfDie, modifier);
        }
    }
}
