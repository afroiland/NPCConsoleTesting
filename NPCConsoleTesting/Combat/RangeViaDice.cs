namespace NPCConsoleTesting.Combat
{
    public class RangeViaDice
    {
        public int NumberOfDice { get; set; }
        public int TypeOfDie { get; set; }
        public int Modifier { get; set; }

        public RangeViaDice(int numberOfDice, int typeOfDie, int modifier)
        {
            NumberOfDice = numberOfDice;
            TypeOfDie = typeOfDie;
            Modifier = modifier;
        }
    }
}
