using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("CHARACTER 1");
            Character npc1 = Build.BuildCharacter();
            Console.WriteLine("CHARACTER 2");
            Character npc2 = Build.BuildCharacter();
            Console.WriteLine("CHARACTER 3");
            Character npc3 = Build.BuildCharacter();

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

            Console.WriteLine($"character3 HP: {npc3.hp}");
            Console.WriteLine($"character3 initMod: {npc3.initMod}");
            Console.WriteLine($"character3 AC: {npc3.ac}");
            Console.WriteLine($"character3 thac0: {npc3.thac0}");
            Console.WriteLine($"character3 dmg: {npc3.numberOfDice}d{npc3.typeOfDie}+{npc3.modifier}");
            Console.WriteLine();

            List<Character> combatants = new();
            combatants.Add(npc1);
            combatants.Add(npc2);
            combatants.Add(npc3);

            //do a round
            //RoundResults roundResults = Combat.CombatRound(combatants);
            //roundResults.roundLog.ForEach(i => Console.WriteLine(i));
            //Console.ReadLine();

            //do a whole fight
            List<string> wholeFightLog = new() { "Here's what happened:"};
            bool downToOne = false;
            
            while (!downToOne)
            {
                RoundResults roundResults = Combat.CombatRound(combatants);

                //TODO: ensure there is not a shorter way to do this. No luck briefly with Join, Concat
                //add roundLog to wholeFightLog
                foreach (string log in roundResults.roundLog)
                {
                    wholeFightLog.Add(log);
                }

                //TODO: clean this up, likely using LINQ
                //check if we're down to one
                int numberOfSurvivors = 0;
                foreach (Character ch in roundResults.characters)
                {
                    if (ch.hp > 0)
                    {
                        numberOfSurvivors++;
                    }
                }
                if (numberOfSurvivors == 1)
                {
                    //the fight has ended
                    downToOne = true;

                    List<string> winner = combatants.Where(x => x.hp != 0).Select(x => x.name).ToList();
                    wholeFightLog.Add($"{winner[0]} wins.");

                    wholeFightLog.ForEach(i => Console.WriteLine(i));
                    Console.ReadLine();
                }

                //lol
                if (numberOfSurvivors < 1)
                {
                    Console.WriteLine("lol");
                    break;
                }

                //update combatants list with returned
                combatants = roundResults.characters;
            }
        }
    }
}
