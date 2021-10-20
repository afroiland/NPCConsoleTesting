using NPCConsoleTesting.Characters;

namespace NPCConsoleTesting
{
    public class ActionResults
    {
        public int Damage { get; set; }
        public string SpellName { get; set; }
        public string SpellEffectType { get; set; }
        public string SpellSavingThrow { get; set; }
        public Status Status { get; set; }

        public ActionResults(int dmg, string spellName = null, string effectType = null, string savingThrow = null, Status status = null)
        {
            Damage = dmg;
            SpellName = spellName;
            SpellEffectType = effectType;
            SpellSavingThrow = savingThrow;
            Status = status;
        }
    }
}
