using NPCConsoleTesting.Characters;
using System;
using System.Collections.Generic;

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
                List<string> combatLog = FullCombat.DoAFullCombat(combatants);

                //get winner's name from last item in combatLog and increment win counter for that character
                string lastEntry = combatLog[combatLog.Count - 1];
            }

            //format data (convert wins to percentages)
            //display results
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
