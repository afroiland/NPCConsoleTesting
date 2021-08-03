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

            //user enters stats for NPC2
            Console.WriteLine("Enter HP for character2");
            int npc2HP = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character2");
            int npc2InitMod = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter AC for character2");
            int npc2AC = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter thac0 for character2");
            int npc2Thac0 = int.Parse(Console.ReadLine());

            //character objects are hydrated with above data
            Character npc1 = new Character(npc1HP, npc1InitMod, npc1AC, npc1Thac0);
            Character npc2 = new Character(npc2HP, npc2InitMod, npc2AC, npc2Thac0);

            Console.WriteLine($"character1 HP: {npc1.hp}, character1 initMod: {npc1.initMod}, " +
                $"character1 AC: {npc1.ac}, character1 thac0: {npc1.thac0}");

            Console.WriteLine($"character2 HP: {npc2.hp}, character2 initMod: {npc2.initMod}, " +
                $"character2 AC: {npc2.ac}, character2 thac0: {npc2.thac0}");
        }
    }
}
