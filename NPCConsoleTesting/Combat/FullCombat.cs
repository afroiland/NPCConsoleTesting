using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting.Combat
{
    public class FullCombat
    {
        const int MaxNumberOfCombatantsToDisplay = 10;

        public static List<string> DoAFullCombat(List<Combatant> combatants, bool isTeamBattle)
        {
            //combatants fight until only one* remains.  (*in rare cases, zero)
            List<string> wholeFightLog = new() { " ", "Here's what happened:" };
            int roundNumber = 0;

            bool theBattleIsOver = false;

            //while(combatants.Count > 1)
            while (!theBattleIsOver)
                {
                List<string> logResults = CombatRound.DoACombatRound(combatants);

                //Remove fallen combatants from list
                combatants.RemoveAll(x => x.CurrentHP < 1);

                roundNumber++;
                wholeFightLog.Add($"------Round {roundNumber}------");

                //add roundLog to wholeFightLog
                wholeFightLog.AddRange(logResults);

                theBattleIsOver = isTeamBattle ? DetermineIfTeamBattleIsOver(combatants, wholeFightLog) : DetermineIfFFAIsOver(combatants, wholeFightLog);

                ////check if only one combatant remains
                //if (combatants.Count == 1)
                //{
                //    //the fight has ended
                //    wholeFightLog.Add($"{combatants[0].Name} won.");
                //    //wholeFightLog.ForEach(i => Console.WriteLine(i));
                //}

                //if (combatants.Count < 1)
                //{
                //    wholeFightLog.Add("The last two combatants simultaneously killed each other. A winner failed to emerge.");
                //    break;
                //}
            }

            return wholeFightLog;
        }

        private static bool DetermineIfTeamBattleIsOver(List<Combatant> combatants, List<string> wholeFightLog)
        {
            bool battleIsOver = false;

            if (combatants.Count < 1)
            {
                wholeFightLog.Add("The last two combatants simultaneously killed each other. A winner failed to emerge.");
                battleIsOver = true;
            }

            string affiliation = combatants.First().Affiliation;
            if (combatants.All(c => c.Affiliation == affiliation))
            {
                Console.WriteLine($"Those fighting for {affiliation} have won.");
                battleIsOver = true;
            }

            return battleIsOver;
        }

        private static bool DetermineIfFFAIsOver(List<Combatant> combatants, List<string> wholeFightLog)
        {
            bool battleIsOver = false;

            if (combatants.Count < 1)
            {
                wholeFightLog.Add("The last two combatants simultaneously killed each other. A winner failed to emerge.");
                battleIsOver = true;
            }

            if (combatants.Count == 1)
            {
                wholeFightLog.Add($"{combatants[0].Name} won.");
                battleIsOver = true;
            }

            return battleIsOver;
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
        }

        public static void DisplayCountdown()
        {
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
            combatLog.ForEach(i => Console.WriteLine(i));
        }
    }
}
