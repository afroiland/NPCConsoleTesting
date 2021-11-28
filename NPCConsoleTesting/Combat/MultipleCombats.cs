using NPCConsoleTesting.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting.Combat
{
    public class MultipleCombats : IMultipleCombats
    {
        private const int MaxNumberOfTimesToRun = 1000;
        private const int MaxNumberOfCombatantsToRunPrediction = 50;

        IFullCombat _fullCombat;

        public MultipleCombats(IFullCombat fullCombat)
        {
            _fullCombat = fullCombat;
        }

        public List<Winner> DoMultipleCombats(List<Combatant> combatants, int numberOfCombats, bool isTeamBattle)
        {
            List<Winner> winners = new();
            foreach (Combatant c in combatants)
            {
                if (isTeamBattle)
                {
                    //TODO: there's probably a better way to do this
                    if (winners.Count == 0)
                    {
                        winners.Add(new Winner(c.Affiliation, 0));
                    }
                    else
                    {
                        if (!winners.Any(w => w.Name == c.Affiliation))
                        {
                            winners.Add(new Winner(c.Affiliation, 0));
                        }
                    }
                }
                else
                {
                    winners.Add(new Winner(c.Name, 0));
                }
            }

            for (int i = 0; i < numberOfCombats; i++)
            {
                //deep clone list of combatants
                List<Combatant> tempList = new List<Combatant>();
                foreach (Combatant c in combatants)
                {
                    tempList.Add(c.DeepClone());
                }

                List<string> combatLog = _fullCombat.DoAFullCombat(tempList, isTeamBattle);

                string lastEntry = combatLog[^1];
                if (lastEntry != "The last two combatants simultaneously kill each other. A winner fails to emerge.")
                {
                    //get winner's name from last item in combatLog and increment win counter for that character / team
                    string winner = isTeamBattle ? lastEntry.Replace("Those fighting for ", "").Replace(" win.", "") : lastEntry.Replace(" wins.", "");

                    winners[winners.FindIndex(x => x.Name == winner)].Wins++;
                }
            }

            List<Winner> orderedDescending = winners.OrderByDescending(x => x.Wins).ToList();
            List<Winner> winnersWithWinPercentages = GetWinPercentages(orderedDescending, numberOfCombats);
            return winnersWithWinPercentages;
        }

        private static List<Winner> GetWinPercentages(List<Winner> winners, int numberOfCombats)
        {
            foreach (Winner w in winners)
            {
                w.WinPercentage = CalcWinPercentage(w.Wins, numberOfCombats);
            }

            return winners;
        }

        public int GetNumberOfTimesToRun()
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

        public void PredictWinner(List<Combatant> combatants, bool isTeamBattle)
        {
            if (combatants.Count <= MaxNumberOfCombatantsToRunPrediction)
            {
                List<Winner> winners = DoMultipleCombats(combatants, 1000, isTeamBattle);

                string confidence = winners[0].WinPercentage switch
                {
                    > 95 => "supremely",
                    > 85 => "very",
                    > 75 => "fairly",
                    > 65 => "somewhat",
                    _ => "not very"
                };

                Console.WriteLine();
                Console.WriteLine($"{winners[0].Name} is predicted to win ({confidence} confident).");
            }
        }

        public void DisplayWinRates(List<Winner> winners)
        {
            foreach (Winner w in winners)
            {
                string plural = w.Wins == 1 ? "" : "s";
                Console.WriteLine($"{w.Name} won {w.Wins} time{plural}, a winrate of {w.WinPercentage}%.");
            }
        }

        private static int CalcWinPercentage(int wins, int numberOfCombats)
        {
            return (int)((double)wins / numberOfCombats * 100);
        }
    }
}
