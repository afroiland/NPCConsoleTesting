using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatMethods
    {
        int Attack(int thac0, int ac, int numberOfAttackDice, int typeOfAttackDie, int dmgModifier);
        int CalcDmg(int numberOfAttackDice, int typeOfAttackDie, int dmgModifier);
        List<ICombatant> DetermineInit(List<ICombatant> chars);
        List<ICombatant> DetermineTargets(List<ICombatant> chars);
    }
}
