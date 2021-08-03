using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Combat
    {
        public void Fight(Character char1, Character char2)
        {
            Console.WriteLine("test");
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
