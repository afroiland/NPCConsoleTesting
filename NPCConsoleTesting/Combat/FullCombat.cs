using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting.Combat
{
    public class FullCombat : IFullCombat
    {
        private const int DefaultMaxCombatantsToDisplay = 12;

        ICombatRound _combatRound;

        private int _MaxNumberOfCombatantsToDisplay;

        public int MaxNumberOfCombatantsToDisplay { get => _MaxNumberOfCombatantsToDisplay; set => _MaxNumberOfCombatantsToDisplay = value; }

        public FullCombat(ICombatRound combatRound, int maxNumberOfCombatantsToDisplay = DefaultMaxCombatantsToDisplay)
        {
            _combatRound = combatRound;
            MaxNumberOfCombatantsToDisplay = maxNumberOfCombatantsToDisplay;
        }

        public List<string> DoAFullCombat(List<Combatant> combatants, bool isTeamBattle)
        {
            List<string> wholeFightLog = new() { " " };
            int roundNumber = 0;

            bool theBattleIsOver = false;

            while (!theBattleIsOver)
            {
                List<string> logResults = _combatRound.DoACombatRound(combatants, isTeamBattle);

                //remove fallen combatants from list
                combatants.RemoveAll(x => x.CurrentHP < 1);

                roundNumber++;
                wholeFightLog.Add($"------Round {roundNumber}------");

                //add roundLog to wholeFightLog
                wholeFightLog.AddRange(logResults);

                //combatants fight until only one combatant/team remains. (in rare cases, zero combatants will remain)
                theBattleIsOver = isTeamBattle ? DetermineIfTeamBattleIsOver(combatants, wholeFightLog) : DetermineIfFFAIsOver(combatants, wholeFightLog);
            }

            return wholeFightLog;
        }

        private static bool DetermineIfTeamBattleIsOver(List<Combatant> combatants, List<string> wholeFightLog)
        {
            bool battleIsOver = false;

            if (combatants.Count < 1)
            {
                wholeFightLog.Add("The last two combatants simultaneously kill each other. A winner fails to emerge.");
                battleIsOver = true;
            }
            else
            {
                string affiliation = combatants.First().Affiliation;
                if (combatants.All(c => c.Affiliation == affiliation))
                {
                    wholeFightLog.Add($"Those fighting for {affiliation} win.");
                    battleIsOver = true;
                }
            }

            return battleIsOver;
        }

        private static bool DetermineIfFFAIsOver(List<Combatant> combatants, List<string> wholeFightLog)
        {
            bool battleIsOver = false;

            if (combatants.Count < 1)
            {
                wholeFightLog.Add("The last two combatants simultaneously kill each other. A winner fails to emerge.");
                battleIsOver = true;
            }

            if (combatants.Count == 1)
            {
                wholeFightLog.Add($"{combatants[0].Name} wins.");
                battleIsOver = true;
            }

            return battleIsOver;
        }

        public bool DetermineIfTeamBattle()
        {
            Console.WriteLine("1 = Simulate a free-for-all battle, 2 = Simulate a team battle");
            int isTeamBattle = 0;
            while (isTeamBattle != 1 && isTeamBattle != 2)
            {
                isTeamBattle = CombatantBuilder.GetPositiveIntFromUser();

                if (isTeamBattle != 1 && isTeamBattle != 2)
                {
                    Console.WriteLine("1 or 2, those are your options.");
                }
            }

            return isTeamBattle == 2;
        }

        public bool DetermineIfSingleBattle()
        {
            Console.WriteLine("1 = Simulate a single combat instance, 2 = Run a simulation multiple times");
            int isSingleBattle = 0;
            while (isSingleBattle != 1 && isSingleBattle != 2)
            {
                isSingleBattle = CombatantBuilder.GetPositiveIntFromUser();

                if (isSingleBattle != 1 && isSingleBattle != 2)
                {
                    Console.WriteLine("1 or 2, those are your options.");
                }
            }

            return isSingleBattle == 1;
        }

        public void DisplayPreCombatInformation(List<Combatant> combatants, bool isTeamBattle)
        {
            if (combatants.Count <= _MaxNumberOfCombatantsToDisplay)
            {
                Console.WriteLine();
                Console.WriteLine("Here are the combatants:");

                List<Combatant> sortedByAffiliation = combatants.OrderBy(x => x.Affiliation).ToList();

                foreach (Combatant c in sortedByAffiliation)
                {
                    string teamInfo = isTeamBattle ? $", fighting for {c.Affiliation}" : "";
                    Console.WriteLine($"{c.Name}, level {c.Level} {c.Race} {c.CharacterClass}, {c.CurrentHP} HP{teamInfo}");
                }
            }
        }

        public void DisplayCountdown()
        {
            Console.WriteLine();
            Console.WriteLine("Combatants are ready. Press any key to begin...");
            Console.ReadKey(true);
            Console.WriteLine("3");
            System.Threading.Thread.Sleep(700);
            Console.WriteLine("2");
            System.Threading.Thread.Sleep(700);
            Console.WriteLine("1");
            System.Threading.Thread.Sleep(700);
            Console.WriteLine("The fighting commences...");
            System.Threading.Thread.Sleep(1000);
        }

        public void DisplayPostCombatInformation(List<string> combatLog)
        {
            combatLog.ForEach(i => Console.WriteLine(i));
        }
    }
}
