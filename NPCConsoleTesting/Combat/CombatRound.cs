using System;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public class CombatRound
    {
        public static List<string> DoACombatRound(List<Combatant> combatants)
        {
            ICombatMethods combatMethods = new CombatMethods();
            List<String> logResults = new();
            
            combatants = combatMethods.DetermineTargets(combatants);
            combatants = combatMethods.DetermineInit(combatants);
                
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
                //TODO: If target is at <0 hp, allow priority char to switch to a new target (if not using spell)?
                if ((combatants[priorityIndex].HP <= 0 && !opportunityForSimulAttack) || combatants[targetIndex].HP <= 0)
                {
                    priorityIndex++;
                    break;
                }

                //check for spells--if none, do an attack
                if (combatants[priorityIndex].Spells == null || combatants[priorityIndex].Spells.Count < 1)
                {
                    //priority combatant does an attack against target
                    int attackResult = combatMethods.Attack(combatants[priorityIndex].Thac0, combatants[targetIndex].AC,
                        combatants[priorityIndex].NumberOfAttackDice, combatants[priorityIndex].TypeOfAttackDie, combatants[priorityIndex].DmgModifier);

                    if (attackResult > 0)
                    {
                        logResults.Add($"{combatants[priorityIndex].Name} struck {combatants[targetIndex].Name} for {attackResult} damage.");

                        //adjust target hp
                        combatants[targetIndex].HP -= attackResult;

                        if (combatants[targetIndex].HP <= 0)
                        {
                            logResults.Add($"{combatants[targetIndex].Name} fell.");

                            if (combatants[targetIndex].Init == segment)
                            {
                                opportunityForSimulAttack = true;
                            }
                        }
                    }
                }
                else
                {
                    //do the spell affect
                    SpellResults spellResults = SpellMethods.DoASpell(combatants[priorityIndex].Spells[0], combatants[priorityIndex].Level);

                    //update combatants with spell results
                    //if spellResults.AffectType == damage, UpdateTargetWithSpellDmg()

                    //if spellResults.AffectType == status, UpdateTargetWithStatusEffect()

                    //remove that spell from list
                    combatants[priorityIndex].Spells.RemoveAt(0);
                }

                priorityIndex++;
            }

            return logResults;
        }
    }
}
