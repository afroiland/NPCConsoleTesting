using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatMethods
    {
        int DoAMeleeAttack(string attackerClass, string defenderClass, int attackerLevel, int defenderLevel, int str, string armor,
            bool hasShield, int dex, string weapon, int ex_str = 0, int magicalBonus = 0, int otherHitBonus = 0, int otherDmgBonus = 0);
        int CalcMeleeDmg(string attackerClass, string weapon, int str, int ex_str, int magicalBonus, int otherDmgBonus = 0);
        List<Combatant> DetermineInit(List<Combatant> chars);
        List<Combatant> DetermineTargets(List<Combatant> chars);
    }
}
