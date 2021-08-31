namespace NPCConsoleTesting
{
    public class SpellResults
    {
        string AffectType { get; set; }
        string Status { get; set; }
        int Damage { get; set; }

        public SpellResults(string affectType, string status, int dmg)
        {
            AffectType = affectType;
            Status = status;
            Damage = dmg;
        }
    }
}
