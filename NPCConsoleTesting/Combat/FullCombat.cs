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
                    wholeFightLog.ForEach(i => Console.WriteLine(i));
                }

                //lol
                if (combatants.Count < 1)
                {
                    Console.WriteLine("lol");
                    break;
                }
            }
        }
    }
}
