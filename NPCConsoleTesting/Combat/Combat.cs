using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class Combat
    {
        public List<String> Fight(Character char1, Character char2)
        {
            var combatLog = new List<String> { "test1", "test2", "test3" };

            DoARound(char1, char2);

            //repeat if necessary

            return combatLog;
        }

        public void DoARound(Character char1, Character char2)
        {
            //determine init

            //first char attack

            //check hp

            //second char attack

            //check hp
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
