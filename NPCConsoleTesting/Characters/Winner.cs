namespace NPCConsoleTesting.Characters
{
    public class Winner
    {
        public string Name { get; set; }
        public int Wins { get; set; }

        public Winner(string name, int wins = 0)
        {
            Name = name;
            Wins = wins;
        }
    }
}
