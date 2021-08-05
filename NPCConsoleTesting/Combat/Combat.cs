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

        public static RoundResults CombatRound(List<Character> combatants)
        {
            var emptyCharList = new List<Character>();
            var sampleRoundLog = new List<String> { "test1", "test2", "test3" };
            var results = new RoundResults(emptyCharList, sampleRoundLog);

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
            Console.ReadLine();

            //set inits
            foreach (Character ch in combatants)
            {
                ch.init = _random.Next(1, 10) + ch.initMod;
            }
            //show inits
            for (int i = 0; i < combatants.Count; i++)
            {
                Console.WriteLine($"{combatants[i].name} init: {combatants[i].init}");
            }
            Console.ReadLine();

            //order chars by init
            List<Character> sortedByInit = combatants.OrderBy(x => x.init).ToList();
            //show init order
            for (int i = 0; i < sortedByInit.Count; i++)
            {
                Console.WriteLine($"{sortedByInit[i].name} init: {sortedByInit[i].init}");
            }
            Console.ReadLine();

            //start tracking segments
            int segment = 0;
            int priorityIndex = 0;
            while (segment < sortedByInit[priorityIndex].init)
            {
                segment++;
            }

            Console.WriteLine($"It is segment {segment}, {sortedByInit[priorityIndex].name} attacks {sortedByInit[priorityIndex].target}");
            Console.ReadLine();

            //first char does an attack

            //int attackResult = Attack(attackerName.thac0, defenderName.ac, attackerName.numberOfDice, attackerName.typeOfDie, attackerName.modifier);


            //adjust defender hp
            //check hp of defender
            //if hp <= 0
            //      if simultaneous = false, fight over
            //next char get a turn      

            //next char attack

            //check hp

            //repeat if necessary
            //reset init?

            return results;
        }

        public int Attack(int thac0, int ac, int numberOfDice, int typeOfDie, int modifier)
        {
            int result = new();

            //calc if hit
            result = thac0 > ac + _random.Next(1, 20) ? 0 : CalcDmg(numberOfDice, typeOfDie, modifier);

            //determine dmg
            return result;
        }

        public int CalcDmg(int numberOfDice, int typeOfDie, int modifier)
        {
            //Random _random = new();
            int result = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                result += _random.Next(1, typeOfDie);
            }

            return result + modifier;
        }
    }
}
