using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Combat
    {
        public static List<String> Fight(Character char1, Character char2)
        {
            Character tempChar1 = char1;
            Character tempChar2 = char2;
            //var combatLog = new List<String>();
            var combatLog = new List<String> { "test1", "test2", "test3" };

            //whole thing in an if stmt from here?

            //determine init


            //first char attack (attack could be a separate method, just returns a pos or neg int?)

            //check hp

            //second char attack

            //check hp

            //repeat if necessary
            //reset init?

            return combatLog;
        }

        public int CalcDmg(int numberOfDice, int typeOfDie, int modifier)
        {
            Random _random = new Random();
            int result = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                result += _random.Next(1, typeOfDie);
            }

            return result + modifier;
        }
    }
}
