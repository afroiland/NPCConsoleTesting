using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatMethods : ICombatMethods
    {
        static Random _random = new();

        public int Attack(int thac0, int ac, int numberOfAttackDice, int typeOfAttackDie, int dmgModifier)
        {
            int result = 0;
            int attackRoll = _random.Next(1, 21);

            //An attack roll of 20 always suceeds and a roll of 1 always fails
            if (attackRoll == 20 || (thac0 - ac <= attackRoll && attackRoll != 1))
            {
                result = CalcDmg(numberOfAttackDice, typeOfAttackDie, dmgModifier);
            }

            return result;
            //return thac0 > ac + _random.Next(1, 21) ? 0 : CalcDmg(numberOfAttackDice, typeOfAttackDie, dmgModifier);
        }

        public int CalcDmg(int numberOfAttackDice, int typeOfAttackDie, int dmgModifier)
        {
            int result = 0;

            for (int i = 0; i < numberOfAttackDice; i++)
            {
                result += _random.Next(1, typeOfAttackDie + 1);
            }

            return result + dmgModifier;
        }

        public List<Combatant> DetermineInit(List<Combatant> chars)
        {
            //set inits
            foreach (Combatant ch in chars)
            {
                ch.Init = _random.Next(1, 11) + ch.InitMod;
                //ch.init = 5;
            }

            //order chars with hp > 0 by init
            chars = chars.Where(x => x.HP > 0).OrderBy(x => x.Init).ToList();

            return chars;
        }

        public List<Combatant> DetermineTargets(List<Combatant> chars)
        {
            //set targets if needed
            foreach (Combatant ch in chars)
            {
                if (ch.Target == "" || chars.Where(x => x.Name == ch.Target).Select(x => x.HP).ToList()[0] <= 0)
                {
                    List<string> potentialTargets = chars.Where(x => ch.Name != x.Name && x.HP > 0).Select(x => x.Name).ToList();
                    ch.Target = potentialTargets[_random.Next(0, potentialTargets.Count)];
                }
            }
            //show targets
            //for (int i = 0; i < chars.Count; i++)
            //{
            //    if (chars[i].hp > 0)
            //    {
            //        Console.WriteLine($"{chars[i].name} target: {chars[i].target}");
            //    }
            //}
            //if (doReadLines) { Console.ReadLine(); }

            return chars;
        }
    }
}
