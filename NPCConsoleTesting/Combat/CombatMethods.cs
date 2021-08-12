﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class CombatMethods
    {
        static Random _random = new();
        private static readonly bool doReadLines = false;
        //private static readonly bool doReadLines = true;

        public static int Attack(int thac0, int ac, int numberOfDice, int typeOfDie, int modifier)
        {
            return thac0 > ac + _random.Next(1, 21) ? 0 : CalcDmg(numberOfDice, typeOfDie, modifier);
        }

        public static int CalcDmg(int numberOfDice, int typeOfDie, int modifier)
        {
            int result = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                result += _random.Next(1, typeOfDie + 1);
            }

            return result + modifier;
        }

        public static List<Combatant> DetermineInit(List<Combatant> chars)
        {
            //set inits
            foreach (Combatant ch in chars)
            {
                ch.init = _random.Next(1, 11) + ch.initMod;
                //ch.init = 5;
            }

            //order chars by init
            //chars = chars.OrderBy(x => x.init).ToList();
            chars = chars.Where(x => x.hp > 0).OrderBy(x => x.init).ToList();

            //show init order
            for (int i = 0; i < chars.Count; i++)
            {
                Console.WriteLine($"{chars[i].name} init: {chars[i].init}");
            }
            if (doReadLines) { Console.ReadLine(); }

            return chars;
        }

        public static List<Combatant> DetermineTargets(List<Combatant> chars)
        {
            //set targets if needed
            foreach (Combatant ch in chars)
            {
                if (ch.target == "" || chars.Where(x => x.name == ch.target).Select(x => x.hp).ToList()[0] <= 0)
                {
                    List<string> potentialTargets = chars.Where(x => ch.name != x.name && x.hp > 0).Select(x => x.name).ToList();
                    ch.target = potentialTargets[_random.Next(0, potentialTargets.Count)];
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