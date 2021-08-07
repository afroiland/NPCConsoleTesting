using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Combat
    {
        static Random _random = new();
        //private static readonly bool doReadLines = false;
        private static readonly bool doReadLines = true;

        public static RoundResults CombatRound(List<Character> combatants)
        {
            //var emptyCharList = new List<Character>();
            //var sampleRoundLog = new List<String> { "test1", "test2", "test3" };
            //var results = new RoundResults(emptyCharList, sampleRoundLog);

            List<Character> charResults = new();
            List<String> logResults = new();

            //set targets if needed
            foreach (Character ch in combatants)
            {
                if (ch.target == "")
                {
                    List<string> others = combatants.Where(x => ch.name != x.name).Select(x => x.name).ToList();
                    ch.target = others[_random.Next(0, others.Count)];
                }
            }
            //show targets
            for (int i = 0; i < combatants.Count; i++)
            {
                Console.WriteLine($"{combatants[i].name} target: {combatants[i].target}");
            }
            if (doReadLines) { Console.ReadLine(); }

            //set inits
            foreach (Character ch in combatants)
            {
                ch.init = _random.Next(1, 10) + ch.initMod;
                //ch.init = 5;
            }
            //show inits
            //for (int i = 0; i < combatants.Count; i++)
            //{
            //    Console.WriteLine($"{combatants[i].name} init: {combatants[i].init}");
            //}
            //Console.ReadLine();

            //order chars by init
            List<Character> sortedByInit = combatants.OrderBy(x => x.init).ToList();
            //show init order
            for (int i = 0; i < sortedByInit.Count; i++)
            {
                Console.WriteLine($"{sortedByInit[i].name} init: {sortedByInit[i].init}");
            }
            if (doReadLines) { Console.ReadLine(); }

            //start tracking segments
            int segment = 0;
            int priorityIndex = 0;
            int targetIndex = 0;

            while (priorityIndex <= sortedByInit.Count - 1)
            {
                while (segment < sortedByInit[priorityIndex].init)
                {
                    segment++;
                    //if priority char has 0 or fewer hp, advance priority index and stop advancing segments
                    if (sortedByInit[priorityIndex].hp <= 0)
                    {
                        priorityIndex++;
                        break;
                    }
                }

                //TODO: Fix this
                //Janky, but this check allows an attack from a char at <1 hp in the case of simultaneous init
                if (priorityIndex >= sortedByInit.Count)
                {
                    break;
                }

                Console.WriteLine($"It is segment {segment}, {sortedByInit[priorityIndex].name} is about to attack {sortedByInit[priorityIndex].target}");
                if (doReadLines) { Console.ReadLine(); }

                //set targetIndex based on priority char's target
                targetIndex = sortedByInit.FindIndex(x => x.name == sortedByInit[priorityIndex].target);
                //Console.WriteLine($"priorityIndex: {priorityIndex}");
                //Console.WriteLine($"targetIndex: {targetIndex}");
                //if (doReadLines) { Console.ReadLine(); }

                //priority char does an attack against target
                int attackResult = Attack(sortedByInit[priorityIndex].thac0, sortedByInit[targetIndex].ac, sortedByInit[priorityIndex].numberOfDice, sortedByInit[priorityIndex].typeOfDie, sortedByInit[priorityIndex].modifier);
                Console.WriteLine($"attackResult: {attackResult}");
                if (doReadLines) { Console.ReadLine(); }

                //update log
                if (attackResult > 0)
                {
                    logResults.Add($"{sortedByInit[priorityIndex].name} struck {sortedByInit[targetIndex].name} for {attackResult} damage.");

                    //adjust target hp
                    sortedByInit[targetIndex].hp -= attackResult;
                    if (sortedByInit[targetIndex].hp <= 0)
                    {
                        logResults.Add($"{sortedByInit[targetIndex].name} has fallen.");
                    }

                    Console.WriteLine($"{sortedByInit[priorityIndex].name} struck {sortedByInit[targetIndex].name} for {attackResult} damage.");
                    Console.WriteLine($"{sortedByInit[targetIndex].name} is at {sortedByInit[targetIndex].hp}hp.");
                }
                else
                {
                    logResults.Add($"{sortedByInit[priorityIndex].name} misses {sortedByInit[targetIndex].name}.");
                }

                

                //advance priorityIndex
                priorityIndex++;
            }

            //add orderedbyinit to charResults
            foreach (Character ch in sortedByInit)
            {
                charResults.Add(ch);
            }

            return new RoundResults(charResults, logResults);
            //return results;
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
    }
}
