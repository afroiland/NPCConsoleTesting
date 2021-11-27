using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatantBuilder
    {
        int MaxCombatantsForMultipleCombats { get; set; }
        int MaxCombatantsForSingleCombat { get; set; }
        int MaxLevel { get; set; }
        int MinLevel { get; set; }

        Combatant BuildCombatantRandomly();
        Combatant BuildCombatantViaConsole(int charNumber);
        List<Combatant> BuildListOfCombatants(string connectionString, int numberBattling);
        int DetermineNumberBattling(bool doingMultipleCombats);
        int CalcFullHP(List<int> HPByLevel, int con, string charClass);
    }
}