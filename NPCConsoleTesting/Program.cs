using System;

namespace NPCConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Build builder = new();
            Character npc1 = builder.BuildCharacter();
            Character npc2 = builder.BuildCharacter();

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

            //do a fight
            Combat combat = new();
            combat.Fight(npc1, npc2);

            //print results/combat log

            Console.ReadLine();
        }
    }
}
