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

            //user enters stats for NPC2
            //Console.WriteLine("Enter HP for character2");
            //int npc2HP = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter initMod for character2");
            //int npc2InitMod = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter AC for character2");
            //int npc2AC = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter thac0 for character2");
            //int npc2Thac0 = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter numberOfDice for character2");
            //int npc2NumberOfDice = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter typeOfDie for character2");
            //int npc2TypeOfDie = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter modifier for character2");
            //int npc2Modifier = int.Parse(Console.ReadLine());

            //character objects are hydrated with above data
            Character npc = new(npcHP, npcInitMod, npcAC, npcThac0, npcNumberOfDice, npcTypeOfDie, npcModifier);
            //Character npc2 = new(npc2HP, npc2InitMod, npc2AC, npc2Thac0, npc2NumberOfDice, npc2TypeOfDie, npc2Modifier);

            return npc;
        }
    }
}
