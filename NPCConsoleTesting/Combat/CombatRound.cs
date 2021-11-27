using NPCConsoleTesting.Combat;
using NPCConsoleTesting.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatRound : ICombatRound
    {
        ICombatMethods _combatMethods;
        ISpellMethods _spellMethods;

        public CombatRound(ICombatMethods combatMethods, ISpellMethods spellMethods)
        {
            _combatMethods = combatMethods;
            _spellMethods = spellMethods;
        }

        public List<string> DoACombatRound(List<Combatant> combatants, bool isTeamBattle)
        {
            List<String> logResults = new();

            _combatMethods.IncrementStatuses(combatants, logResults);
            _combatMethods.DetermineActions(combatants);
            _combatMethods.DetermineTargets(combatants, isTeamBattle);
            _combatMethods.DetermineInits(combatants);

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

                //if target has <= 0 hp and priority combatant's action is melee attack, priority combatant switches to a new target
                if (combatants[targetIndex].CurrentHP <= 0 && combatants[priorityIndex].ActionForThisRound == "melee attack" &&
                    combatants.Where(x => x.CurrentHP > 0).Count() > 1)
                {
                    _combatMethods.DetermineTargetForOneCombatant(combatants, combatants[priorityIndex], isTeamBattle);
                    targetIndex = combatants.FindIndex(x => x.Name == combatants[priorityIndex].Target);
                }

                //no attacks by or against dead combatants, unless there is a simultaneous attack
                if ((combatants[priorityIndex].CurrentHP <= 0 && !opportunityForSimulAttack) || combatants[targetIndex].CurrentHP <= 0 ||
                    combatants[priorityIndex].Statuses.Any(x => x.Name == "held" || x.Name == "asleep"))
                {
                    priorityIndex++;
                    break;
                }

                //priority combatant does an action, the results are applied to the target, and bool opportunityForSimulAttack is updated
                opportunityForSimulAttack = DoActionAndApplyResults(combatants[priorityIndex], combatants[targetIndex], segment, logResults);

                priorityIndex++;
            }

            return logResults;
        }

        private bool DoActionAndApplyResults(Combatant priorityCombatant, Combatant targetCombatant, int segment, List<String> logResults)
        {
            //priority combatant does an action
            ActionResults actionResults = priorityCombatant.ActionForThisRound == "melee attack" ?
                _combatMethods.DoAMeleeAttack(priorityCombatant, targetCombatant) :
                _spellMethods.DoASpell(priorityCombatant.ActionForThisRound, priorityCombatant.Level);

            //update combatants with action results
            CombatantUpdateResults updateResults = _combatMethods.ApplyActionResultToCombatant(priorityCombatant, targetCombatant, actionResults, segment);

            //update log
            logResults.AddRange(updateResults.LogEntries);

            //if a spell was cast, remove it from the combatant's spell list
            if (priorityCombatant.ActionForThisRound != "melee attack")
            {
                int index = priorityCombatant.Spells.IndexOf(priorityCombatant.ActionForThisRound);
                priorityCombatant.Spells.RemoveAt(index);
            }

            return updateResults.OpportunityForSimulAttack;
        }
    }
}
