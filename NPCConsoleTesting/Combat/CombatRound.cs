using NPCConsoleTesting.Combat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatRound
    {
        public static List<string> DoACombatRound(List<Combatant> combatants)
        {
            ICombatMethods combatMethods = new CombatMethods();
            List<String> logResults = new();

            combatMethods.IncrementStatuses(combatants, logResults);
            combatMethods.DetermineActions(combatants);
            combatMethods.DetermineTargets(combatants);
            combatMethods.DetermineInits(combatants);

            //clear GotHitThisRound status for all combatants
            combatants.ForEach(x => x.GotHitThisRound = false);
                
            int segment = 0;
            int priorityIndex = 0;
            int targetIndex = 0;
            bool opportunityForSimulAttack = false;

            while (priorityIndex <= combatants.Count - 1)
            {
                while (segment < combatants[priorityIndex].Init)
                {
                    segment++;
                    opportunityForSimulAttack = false;
                }

                //set targetIndex based on priority combatant's target
                targetIndex = combatants.FindIndex(x => x.Name == combatants[priorityIndex].Target);

                //no attacks by or against dead combatants, unless there is a simultaneous attack
                //TODO: if target is at <0 hp, allow priority char to switch to a new target (if not using spell)?
                if ((combatants[priorityIndex].CurrentHP <= 0 && !opportunityForSimulAttack) || combatants[targetIndex].CurrentHP <= 0 ||
                    combatants[priorityIndex].Statuses.Any(x => x.Name == "Held" || x.Name == "Asleep"))
                {
                    priorityIndex++;
                    break;
                }

                if (combatants[priorityIndex].ActionForThisRound == "Melee Attack")
                {
                    //priority combatant does a melee attack against target
                    ActionResults attackResult = combatMethods.DoAMeleeAttack(combatants[priorityIndex], combatants[targetIndex]);

                    //update target combatant
                    CombatantUpdateResults updateResults = combatMethods.ApplyActionResultToCombatant(combatants[priorityIndex], combatants[targetIndex], attackResult, segment);

                    //update log
                    logResults.AddRange(updateResults.LogEntries);

                    opportunityForSimulAttack = updateResults.OpportunityForSimulAttack;
                }
                else
                {
                    //priority combatant casts a spell
                    ActionResults spellResults = SpellMethods.DoASpell(combatants[priorityIndex].ActionForThisRound, combatants[priorityIndex].Level);

                    //update combatants with spell results
                    CombatantUpdateResults updateResults = combatMethods.ApplyActionResultToCombatant(combatants[priorityIndex], combatants[targetIndex], spellResults, segment);

                    //update log
                    logResults.AddRange(updateResults.LogEntries);

                    opportunityForSimulAttack = updateResults.OpportunityForSimulAttack;

                    //remove spell from list
                    int index = combatants[priorityIndex].Spells.IndexOf(combatants[priorityIndex].ActionForThisRound);
                    combatants[priorityIndex].Spells.RemoveAt(index);
                }

                priorityIndex++;
            }

            return logResults;
        }
    }
}
