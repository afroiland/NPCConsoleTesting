using System;
using System.Collections.Generic;

namespace NPCConsoleTesting.Combat
{
    public class FullCombat
    {
        const int MaxNumberOfCombatantsToDisplay = 6;

        public static List<string> DoAFullCombat(List<Combatant> combatants)
        {
            //combatants fight until only one* remains.  (*in rare cases, zero)
            List<string> wholeFightLog = new() { " ", "Here's what happened:" };
            int roundNumber = 0;

            while(combatants.Count > 1)
            {
                List<string> logResults = CombatRound.DoACombatRound(combatants);

                //Remove fallen combatants from list
                combatants.RemoveAll(x => x.CurrentHP < 1);

                roundNumber++;
                wholeFightLog.Add($"------Round {roundNumber}------");

                //add roundLog to wholeFightLog
                wholeFightLog.AddRange(logResults);

                //check if only one combatant remains
                if (combatants.Count == 1)
                {
                    //the fight has ended
                    wholeFightLog.Add($"{combatants[0].Name} won.");
                    //wholeFightLog.ForEach(i => Console.WriteLine(i));
                }

                //lol
                if (combatants.Count < 1)
                {
                    Console.WriteLine("lol");
                    break;
                }
            }

            return wholeFightLog;
        }

        public static void DisplayPreCombatInformation(List<Combatant> combatants)
        {
            if (combatants.Count <= MaxNumberOfCombatantsToDisplay)
            {
                Console.WriteLine();
                Console.WriteLine("Here are the combatants:");
                foreach (Combatant c in combatants)
                {
                    Console.WriteLine($"{c.Name}, level {c.Level} {c.Race} {c.CharacterClass}, {c.CurrentHP} HP");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Combatants are ready. Press any key to begin...");
            Console.ReadKey(true);
            Console.WriteLine("3");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("2");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("1");
            System.Threading.Thread.Sleep(1500);
        }

        public static void DisplayPostCombatInformation(List<string> combatLog)
        {

        }
    }
}
