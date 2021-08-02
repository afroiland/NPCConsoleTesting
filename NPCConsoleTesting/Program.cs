using System;

namespace NPCConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter HP for character1");
            int npc1HP = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character1");
            int npc1InitMod = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character1");
            int npc1AC = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter initMod for character1");
            int npc1Thac0 = int.Parse(Console.ReadLine());

            //Console.WriteLine($"character1 HP: {npc1HP}");
            //Console.WriteLine($"character1 initMod: {npc1InitMod}");

            Character npc1 = new Character(npc1HP, npc1InitMod, npc1AC, npc1Thac0);


            //user enters stats for NPC2

            //character objects are hydrated with above data
            Character npc1 = new Character();
            //Console.WriteLine($"{npc1.calcDmg()}");

            //do a fight

            //print results/combat log

            Console.ReadLine();
        }
    }
}
