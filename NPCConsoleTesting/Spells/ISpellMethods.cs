namespace NPCConsoleTesting.Spells
{
    public interface ISpellMethods
    {
        ActionResults DoASpell(string spellName, int casterLevel, int bonus = 0);
        string SelectFromCombatantsSpells(Combatant combatant);
    }
}