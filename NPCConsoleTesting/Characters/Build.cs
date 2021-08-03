using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Build
    {
        public Character BuildCharacter()
        {
            Console.WriteLine("Enter HP for character1");
            int npcHP = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character1");
            int npcInitMod = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter AC for character1");
            int npcAC = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter thac0 for character1");
            int npcThac0 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter numberOfDice for character1");
            int npcNumberOfDice = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter typeOfDie for character1");
            int npcTypeOfDie = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter modifier for character1");
            int npcModifier = int.Parse(Console.ReadLine());

            Character npc = new(npcHP, npcInitMod, npcAC, npcThac0, npcNumberOfDice, npcTypeOfDie, npcModifier);

            return npc;
        }
    }
}
