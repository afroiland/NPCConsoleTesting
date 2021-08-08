using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class Combat
    {
        private static readonly bool doReadLines = false;
        //private static readonly bool doReadLines = true;

        public static RoundResults CombatRound(List<Character> combatants)
        {
            List<Character> charResults = new();
            List<String> logResults = new();
            
            combatants = CombatMethods.DetermineTargets(combatants);
            combatants = CombatMethods.DetermineInit(combatants);

            //start tracking segments
            int segment = 0;
            int priorityIndex = 0;
            int targetIndex = 0;

            while (priorityIndex <= combatants.Count - 1)
            {
                while (segment < combatants[priorityIndex].init)
                {
                    segment++;
                    //if priority char has 0 or fewer hp, advance priority index and stop advancing segments
                    if (combatants[priorityIndex].hp <= 0)
                    {
                        priorityIndex++;
                        break;
                    }
                }

                //TODO: Fix this
                //Janky, but this check allows an attack from a char at <1 hp in the case of simultaneous init
                if (priorityIndex >= combatants.Count)
                {
                    break;
                }

                Console.WriteLine($"It is segment {segment}, {combatants[priorityIndex].name} is about to attack {combatants[priorityIndex].target}");
                if (doReadLines) { Console.ReadLine(); }

                //set targetIndex based on priority char's target
                targetIndex = combatants.FindIndex(x => x.name == combatants[priorityIndex].target);
                
                //priority char does an attack against target
                int attackResult = CombatMethods.Attack(combatants[priorityIndex].thac0, combatants[targetIndex].ac, combatants[priorityIndex].numberOfDice, combatants[priorityIndex].typeOfDie, combatants[priorityIndex].modifier);
                Console.WriteLine($"attackResult: {attackResult}");
                if (doReadLines) { Console.ReadLine(); }

                //update log
                if (attackResult > 0)
                {
                    logResults.Add($"{combatants[priorityIndex].name} struck {combatants[targetIndex].name} for {attackResult} damage.");

                    //TODO: Surely this isn't the best spot for this, no?
                    //adjust target hp
                    combatants[targetIndex].hp -= attackResult;
                    if (combatants[targetIndex].hp <= 0)
                    {
                        logResults.Add($"{combatants[targetIndex].name} has fallen.");
                    }

                    Console.WriteLine($"{combatants[priorityIndex].name} struck {combatants[targetIndex].name} for {attackResult} damage.");
                    Console.WriteLine($"{combatants[targetIndex].name} is at {combatants[targetIndex].hp}hp.");
                }
                else
                {
                    logResults.Add($"{combatants[priorityIndex].name} misses {combatants[targetIndex].name}.");
                }

                priorityIndex++;
            }

            //add orderedbyinit to charResults
            foreach (Character ch in combatants)
            {
                charResults.Add(ch);
            }

            return new RoundResults(charResults, logResults);
        }
    }
}
