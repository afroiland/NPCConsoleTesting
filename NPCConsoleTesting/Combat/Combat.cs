using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class Combat
    {
        static Random _random = new();
        private static readonly bool doReadLines = false;
        //private static readonly bool doReadLines = true;

        public static RoundResults CombatRound(List<Character> combatants)
        {
            List<Character> charResults = new();
            List<String> logResults = new();
            
            combatants = DetermineTargets(combatants);
            combatants = DetermineInit(combatants);

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
                int attackResult = Attack(combatants[priorityIndex].thac0, combatants[targetIndex].ac, combatants[priorityIndex].numberOfDice, combatants[priorityIndex].typeOfDie, combatants[priorityIndex].modifier);
                Console.WriteLine($"attackResult: {attackResult}");
                if (doReadLines) { Console.ReadLine(); }

                //update log
                if (attackResult > 0)
                {
                    logResults.Add($"{combatants[priorityIndex].name} struck {combatants[targetIndex].name} for {attackResult} damage.");

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

                //advance priorityIndex
                priorityIndex++;
            }

            //add orderedbyinit to charResults
            foreach (Character ch in combatants)
            {
                charResults.Add(ch);
            }

            return new RoundResults(charResults, logResults);
        }

        public static int Attack(int thac0, int ac, int numberOfDice, int typeOfDie, int modifier)
        {
            return thac0 > ac + _random.Next(1, 20) ? 0 : CalcDmg(numberOfDice, typeOfDie, modifier);
        }

        public static int CalcDmg(int numberOfDice, int typeOfDie, int modifier)
        {
            int result = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                result += _random.Next(1, typeOfDie);
            }

            return result + modifier;
        }

        public static List<Character> DetermineInit(List<Character> chars)
        {
            //set inits
            foreach (Character ch in chars)
            {
                ch.init = _random.Next(1, 10) + ch.initMod;
                //ch.init = 5;
            }
            //show inits
            //for (int i = 0; i < chars.Count; i++)
            //{
            //    Console.WriteLine($"{chars[i].name} init: {chars[i].init}");
            //}
            //if (doReadLines) { Console.ReadLine(); }

            //order chars by init
            chars = chars.OrderBy(x => x.init).ToList();
            //show init order
            for (int i = 0; i < chars.Count; i++)
            {
                Console.WriteLine($"{chars[i].name} init: {chars[i].init}");
            }
            if (doReadLines) { Console.ReadLine(); }

            return chars;
        }

        public static List<Character> DetermineTargets(List<Character> chars)
        {
            //set targets if needed
            foreach (Character ch in chars)
            {
                if (ch.target == "")
                {
                    List<string> others = chars.Where(x => ch.name != x.name).Select(x => x.name).ToList();
                    ch.target = others[_random.Next(0, others.Count - 1)];
                }
            }
            //show targets
            for (int i = 0; i < chars.Count; i++)
            {
                Console.WriteLine($"{chars[i].name} target: {chars[i].target}");
            }
            if (doReadLines) { Console.ReadLine(); }

            return chars;
        }
    }
}
