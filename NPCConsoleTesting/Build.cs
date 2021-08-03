using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Build
    {
        public void BuildCharacter()
        {
            Console.WriteLine("Enter HP for character1");
            int npc1HP = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character1");
            int npc1InitMod = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter AC for character1");
            int npc1AC = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter thac0 for character1");
            int npc1Thac0 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter numberOfDice for character1");
            int npc1NumberOfDice = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter typeOfDie for character1");
            int npc1TypeOfDie = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter modifier for character1");
            int npc1Modifier = int.Parse(Console.ReadLine());

            //user enters stats for NPC2
            Console.WriteLine("Enter HP for character2");
            int npc2HP = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character2");
            int npc2InitMod = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter AC for character2");
            int npc2AC = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter thac0 for character2");
            int npc2Thac0 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter numberOfDice for character2");
            int npc2NumberOfDice = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter typeOfDie for character2");
            int npc2TypeOfDie = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter modifier for character2");
            int npc2Modifier = int.Parse(Console.ReadLine());

            //character objects are hydrated with above data
            Character npc1 = new(npc1HP, npc1InitMod, npc1AC, npc1Thac0, npc1NumberOfDice, npc1TypeOfDie, npc1Modifier);
            Character npc2 = new(npc2HP, npc2InitMod, npc2AC, npc2Thac0, npc2NumberOfDice, npc2TypeOfDie, npc2Modifier);

            Console.WriteLine($"character1 HP: {npc1.hp}");
            Console.WriteLine($"character1 initMod: {npc1.initMod}");
            Console.WriteLine($"character1 AC: {npc1.ac}");
            Console.WriteLine($"character1 thac0: {npc1.thac0}");
            Console.WriteLine($"character1 dmg: {npc1.numberOfDice}d{npc1.typeOfDie}+{npc1.modifier}");

            Console.WriteLine($"character2 HP: {npc2.hp}");
            Console.WriteLine($"character2 initMod: {npc2.initMod}");
            Console.WriteLine($"character2 AC: {npc2.ac}");
            Console.WriteLine($"character2 thac0: {npc2.thac0}");
            Console.WriteLine($"character2 dmg: {npc2.numberOfDice}d{npc2.typeOfDie}+{npc2.modifier}");
        }
    }
}
