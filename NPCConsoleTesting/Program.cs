using System;

namespace NPCConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Build builder = new();
            builder.BuildCharacter();

            //do a fight
            Combat combat = new();
            combat.Fight();

            //print results/combat log

            Console.ReadLine();
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
