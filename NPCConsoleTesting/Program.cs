using System;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CHARACTER 1");
            Character npc1 = Build.BuildCharacter();
            Console.WriteLine("CHARACTER 2");
            Character npc2 = Build.BuildCharacter();

            Console.WriteLine($"character1 HP: {npc1.hp}");
            Console.WriteLine($"character1 initMod: {npc1.initMod}");
            Console.WriteLine($"character1 AC: {npc1.ac}");
            Console.WriteLine($"character1 thac0: {npc1.thac0}");
            Console.WriteLine($"character1 dmg: {npc1.numberOfDice}d{npc1.typeOfDie}+{npc1.modifier}");
            Console.WriteLine();

            Console.WriteLine($"character2 HP: {npc2.hp}");
            Console.WriteLine($"character2 initMod: {npc2.initMod}");
            Console.WriteLine($"character2 AC: {npc2.ac}");
            Console.WriteLine($"character2 thac0: {npc2.thac0}");
            Console.WriteLine($"character2 dmg: {npc2.numberOfDice}d{npc2.typeOfDie}+{npc2.modifier}");
            Console.WriteLine();

            List<Character> combatants = new();
            combatants.Add(npc1);
            combatants.Add(npc2);

            //do a fight
            var log = new List<String>(Combat.Fight(combatants));

            log.ForEach(i => Console.WriteLine(i));
            Console.ReadLine();
        }
    }
}
