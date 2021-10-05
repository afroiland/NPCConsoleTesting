using NPCConsoleTesting.Characters;

namespace NPCConsoleTesting
{
    public class SpellResults
    {
        public string AffectType { get; set; }
        public Status Status { get; set; }
        public int Damage { get; set; }

        public SpellResults(string affectType, Status status, int dmg)
        {
            AffectType = affectType;
            Status = status;
            Damage = dmg;
        }
    }
}
