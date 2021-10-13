namespace NPCConsoleTesting.Characters
{
    public class Status
    {
        public string Name { get; set; }
        public int Duration { get; set; }

        public Status(string name, int duration)
        {
            Name = name;
            Duration = duration;
        }
    }
}
