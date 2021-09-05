namespace NPCConsoleTesting
{
    public class SpellResults
    {
        public string AffectType { get; set; }
        public string Status { get; set; }
        public int Damage { get; set; }

        public SpellResults(string affectType, string status, int dmg)
        {
            AffectType = affectType;
            Status = status;
            Damage = dmg;
        }
    }
}
