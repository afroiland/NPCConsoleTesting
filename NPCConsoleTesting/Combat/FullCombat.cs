using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting.Combat
{
    public class FullCombat
    {
        public static void DoAFullCombat(List<Combatant> combatants)
        {
            //combatants fight until only one* remains.  (*in rare cases, zero)
            List<string> wholeFightLog = new() { " ", "Here's what happened:" };
            bool downToOne = false;
            int roundNumber = 0;

            while (!downToOne)
            {
                List<string> logResults = CombatRound.DoACombatRound(combatants);

                roundNumber++;
                wholeFightLog.Add($"------Round {roundNumber}------");

                //TODO: ensure there is not a shorter way to do this. No luck briefly with Join, Concat
                //add roundLog to wholeFightLog
                foreach (string logEntry in logResults)
                {
                    wholeFightLog.Add(logEntry);
                }

                //TODO: clean this up, likely using LINQ
                //check if we're down to one
                int numberOfSurvivors = 0;
                foreach (Combatant ch in combatants)
                {
                    if (ch.HP > 0)
                    {
                        numberOfSurvivors++;
                    }
                }
                if (numberOfSurvivors == 1)
                {
                    //the fight has ended
                    downToOne = true;

                    List<string> winner = combatants.Where(x => x.HP > 0).Select(x => x.Name).ToList();
                    wholeFightLog.Add($"{winner[0]} won.");

                    wholeFightLog.ForEach(i => Console.WriteLine(i));
                }

                //lol
                if (numberOfSurvivors < 1)
                {
                    Console.WriteLine("lol");
                    break;
                }
            }
        }
    }
}
