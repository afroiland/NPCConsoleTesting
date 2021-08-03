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

            Console.WriteLine("Enter AC for character1");
            int npc1AC = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter thac0 for character1");
            int npc1Thac0 = int.Parse(Console.ReadLine());

            Console.WriteLine($"character1 HP: {npc1HP}, character1 initMod: {npc1InitMod}, " +
                $"character1 AC: {npc1AC}, character1 thac0: {npc1Thac0}");

            //user enters stats for NPC2

            //character objects are hydrated with above data
            Character npc1 = new Character(npc1HP, npc1InitMod, npc1AC, npc1Thac0);

            
            //Console.WriteLine($"{npc1.calcDmg()}");

            //do a fight

            //print results/combat log

            Console.ReadLine();
        }

        public int calcDmg(int numberOfDice, int typeOfDie, int modifier)
        {
            Random _random = new Random();
            int result = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                result += _random.Next(1, typeOfDie);
            }

            return result + modifier;

        }
    }
}
