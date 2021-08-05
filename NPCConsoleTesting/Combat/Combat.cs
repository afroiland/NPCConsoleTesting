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

        public static List<String> Fight(Character char1, Character char2)
        {
            Character tempChar1 = char1;
            Character tempChar2 = char2;
            //var combatLog = new List<String>();
            var combatLog = new List<String> { "test1", "test2", "test3" };

            //Random _random = new();

            //whole thing in an if/while stmt from here?

            //determine init
            var char1Init = _random.Next(1, 10) + char1.initMod;
            var char2Init = _random.Next(1, 10) + char2.initMod;
            //Console.WriteLine($"char1Init: {char1Init}");
            //Console.WriteLine($"char2Init: {char2Init}");
            //Console.ReadLine();

            //first char attack (attack could be a separate method, just returns a pos or neg int?)
            bool simultaneous = char1Init == char2Init ? true : false;

            //find char w/ lowest init
            


            //string attackerName = attacker.name;
            //string defenderName = defender.name;

            //that char does an attack
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
