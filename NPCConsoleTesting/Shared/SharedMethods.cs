using System;

namespace NPCConsoleTesting.Shared
{
    public class SharedMethods
    {
        static Random _random = new();

        public static int CalcMultipleDice(RangeViaDice rangeViaDice)
        {
            int result = 0;

            for (int i = 0; i < rangeViaDice.NumberOfDice; i++)
            {
                result += _random.Next(1, rangeViaDice.TypeOfDie + 1);
            }

            return result + rangeViaDice.Modifier;
        }
    }
}
