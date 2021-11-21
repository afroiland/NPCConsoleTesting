namespace NPCConsoleTesting.Characters
{
    public class Winner
    {
        public string Name { get; set; }
        public int Wins { get; set; }
        public int WinPercentage { get; set; }

        public Winner(string name, int wins = 0, int winPercentage = 0)
        {
            Name = name;
            Wins = wins;
            WinPercentage = winPercentage;
        }
    }
}
