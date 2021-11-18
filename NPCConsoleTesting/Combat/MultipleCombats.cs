using NPCConsoleTesting.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting.Combat
{
    public class MultipleCombats
    {
        private const int MaxNumberOfTimesToRun = 1000;

        public static void DoMultipleCombats(List<Combatant> combatants, int numberOfCombats)
        {
            List<Winner> winners = new();
            foreach (Combatant c in combatants)
            {
                winners.Add(new Winner(c.Name, 0));
            }

            for (int i = 0; i < numberOfCombats; i++)
            {
                //deep clone list of combatants
                List<Combatant> tempList = new List<Combatant>();
                foreach (Combatant c in combatants)
                {
                    tempList.Add(c.DeepClone());
                }

                List<string> combatLog = FullCombat.DoAFullCombat(tempList);

                //get winner's name from last item in combatLog and increment win counter for that character
                string lastEntry = combatLog[combatLog.Count - 1];

                if (lastEntry != "The last two combatants simultaneously killed each other. A winner failed to emerge.")
                {
                    string name = lastEntry.Replace(" won.", "");
                    winners[winners.FindIndex(x => x.Name == name)].Wins++;
                }
            }

            List<Winner> orderedDescending = winners.OrderByDescending(x => x.Wins).ToList();
            foreach (Winner w in orderedDescending)
            {
                string plural = w.Wins == 1 ? "" : "s";
                int winPercentage = CalcWinPercentage(w.Wins, numberOfCombats);
                Console.WriteLine($"{w.Name} won {w.Wins} time{plural}, a winrate of {winPercentage}%.");
            }
        }

        private static int CalcWinPercentage(int wins, int numberOfCombats)
        {
            return (int)((double)wins / numberOfCombats * 100);
        }

        public static int GetNumberOfTimesToRun()
        {
            Console.WriteLine("How many times shall the simulation be run?");
            int numberOfTimesToRun = 0;
            while (numberOfTimesToRun < 1)
            {
                numberOfTimesToRun = CombatantBuilder.GetPositiveIntFromUser();

                if (numberOfTimesToRun > MaxNumberOfTimesToRun)
                {
                    Console.WriteLine($"Current settings won't allow for that many. Maximum number of times is {MaxNumberOfTimesToRun} at this time.");
                    numberOfTimesToRun = 0;
                }
            }

            return numberOfTimesToRun;
        }
    }
}
