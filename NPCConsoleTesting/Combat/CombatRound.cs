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

            //clear GotHitThisRound status for all combatants
            foreach (var cbt in combatants)
            {
                cbt.GotHitThisRound = false;
            }
                
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
                if ((combatants[priorityIndex].CurrentHP <= 0 && !opportunityForSimulAttack) || combatants[targetIndex].CurrentHP <= 0 || combatants[priorityIndex].Statuses.Contains("Held") || combatants[priorityIndex].Statuses.Contains("Asleep"))
                {
                    priorityIndex++;
                    break;
                }

                //check for spells--if none, do an attack
                if (combatants[priorityIndex].Spells == null || combatants[priorityIndex].Spells.Count < 1)
                {
                    //priority combatant does an attack against target
                    //int attackResult = combatMethods.Attack(combatants[priorityIndex].Thac0, combatants[targetIndex].AC,
                    //    combatants[priorityIndex].NumberOfAttackDice, combatants[priorityIndex].TypeOfAttackDie, combatants[priorityIndex].DmgModifier);

                    int attackResult = combatMethods.DoMeleeAttack(combatants[priorityIndex].CharacterClass, combatants[priorityIndex].Thac0, combatants[targetIndex].Strength,
                        combatants[priorityIndex].Armor, combatants[priorityIndex].Dexterity, combatants[priorityIndex].Weapon, combatants[priorityIndex].Ex_Strength, 0, 0, 0);

                    if (attackResult > 0)
                    {
                        logResults.Add($"{combatants[priorityIndex].Name} struck {combatants[targetIndex].Name} for {attackResult} damage.");

                        //adjust target hp and GotHitThisRound status
                        combatants[targetIndex].CurrentHP -= attackResult;
                        combatants[targetIndex].GotHitThisRound = true;

                        if (combatants[targetIndex].CurrentHP <= 0)
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
                    if (!combatants[priorityIndex].GotHitThisRound)
                    {
                        //do the spell effect
                        SpellResults spellResults = SpellMethods.DoASpell(combatants[priorityIndex].Spells[0], combatants[priorityIndex].Level);

                        //update combatants with spell results
                        if (spellResults.AffectType == "damage")
                        {
                            if (spellResults.Damage < 0)   //a negative result indicates cure light wounds
                            {
                                combatants[priorityIndex].CurrentHP -= spellResults.Damage;
                            }
                            else
                            {
                                combatants[targetIndex].CurrentHP -= spellResults.Damage;
                                combatants[targetIndex].GotHitThisRound = true;
                                logResults.Add($"{combatants[targetIndex].Name} got hit with a {combatants[priorityIndex].Spells[0]} effect for {spellResults.Damage} damage.");
                                if (combatants[targetIndex].CurrentHP < 1)
                                {
                                    logResults.Add($"{combatants[targetIndex].Name} fell.");
                                }
                            }
                        }

                        if (spellResults.AffectType == "status")
                        {
                            combatants[targetIndex].Statuses.Add(spellResults.Status);
                            logResults.Add($"{combatants[targetIndex].Name} is {spellResults.Status}.");
                        }
                    }

                    //remove spell from list
                    combatants[priorityIndex].Spells.RemoveAt(0);
                }

                priorityIndex++;
            }

            return logResults;
        }
    }
}
