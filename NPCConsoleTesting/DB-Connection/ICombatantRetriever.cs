namespace NPCConsoleTesting
{
    public interface ICombatantRetriever
    {
        Combatant GetCombatantByName(string connectionString, string charName);
    }
}