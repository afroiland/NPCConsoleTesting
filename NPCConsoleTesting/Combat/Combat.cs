using System;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public class Combat
    {
        private static readonly bool doReadLines = false;
        //private static readonly bool doReadLines = true;

        public static RoundResults CombatRound(List<ICombatant> combatants)
        {
            ICombatMethods combatMethods = new CombatMethods();
            List<ICombatant> charResults = new();
            List<String> logResults = new();
            
            combatants = combatMethods.DetermineTargets(combatants);
            combatants = combatMethods.DetermineInit(combatants);

            //foreach (ICombatant x in combatants)
            //{
            //    Console.WriteLine($"{x.Name} hp: {x.HP}");
            //}
                
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
                if ((combatants[priorityIndex].HP <= 0 && !opportunityForSimulAttack) || combatants[targetIndex].HP <= 0)
                {
                    priorityIndex++;
                    break;
                }

                //Console.WriteLine($"It is segment {segment}, {combatants[priorityIndex].name} is about to attack {combatants[priorityIndex].target}");
                //if (doReadLines) { Console.ReadLine(); }
                
                //priority combatant does an attack against target
                int attackResult = combatMethods.Attack(combatants[priorityIndex].Thac0, combatants[targetIndex].AC,
                    combatants[priorityIndex].NumberOfAttackDice, combatants[priorityIndex].TypeOfAttackDie, combatants[priorityIndex].DmgModifier);
                //Console.WriteLine($"attackResult: {attackResult}");
                //if (doReadLines) { Console.ReadLine(); }

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

                    //Console.WriteLine($"{combatants[priorityIndex].Name} struck {combatants[targetIndex].Name} for {attackResult} damage.");
                    //Console.WriteLine($"{combatants[targetIndex].Name} is at {combatants[targetIndex].HP}hp.");
                }
                //else
                //{
                //    logResults.Add($"{combatants[priorityIndex].Name} missed {combatants[targetIndex].Name}.");
                //}

                priorityIndex++;
            }

            //add combatants to charResults
            foreach (ICombatant ch in combatants)
            {
                charResults.Add(ch);
            }

            return new RoundResults(charResults, logResults);
        }
    }
}
