using NPCConsoleTesting.Characters;

namespace NPCConsoleTesting
{
    public class ActionResults
    {
        public int Damage { get; set; }
        public string SpellName { get; set; }
        public string SpellAffectType { get; set; }
        public Status Status { get; set; }

        public ActionResults(int dmg, string spellName = null, string affectType = null, Status status = null)
        {
            Damage = dmg;
            SpellName = spellName;
            SpellAffectType = affectType;
            Status = status;
        }
    }
}
