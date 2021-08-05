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

        public static List<String> Fight(List<Character> combatants)
        {
            //var combatLog = new List<String>();
            var combatLog = new List<String> { "test1", "test2", "test3" };

            //whole thing in an if/while stmt from here?

            //determine init
            foreach (Character ch in combatants)
            {
                ch.init = _random.Next(1, 10) + ch.initMod;
            }

            //show inits
            //for (int i = 0; i < combatants.Count; i++)
            //{
            //    Console.WriteLine($"char {i+1} init: {combatants[i].init}");
            //}
            //Console.ReadLine();

            //bool simultaneous = char1Init == char2Init ? true : false;

            //order chars by init
            List<Character> sortedByInit = combatants.OrderBy(x => x.init).ToList();
            Console.WriteLine(sortedByInit[0].name + " init: " + sortedByInit[0].init);
            Console.WriteLine(sortedByInit[1].name + " init: " + sortedByInit[1].init);
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

            return combatLog;
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
