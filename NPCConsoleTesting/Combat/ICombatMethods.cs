using NPCConsoleTesting.Combat;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatMethods
    {
        ActionResults DoAMeleeAttack(IAttacker attacker, IDefender defender);
        int CalcMonkMeleeDmg(int level, string weapon, int magicalDmgBonus, int otherDmgBonus = 0);
        int CalcNonMonkMeleeDmg(string weapon, int str, int ex_str, int magicalDmgBonus, int otherDmgBonus = 0);
        void IncrementStatuses(List<Combatant> chars, List<string> log);
        void DetermineActions(List<Combatant> chars);
        void DetermineInits(List<Combatant> chars);
        void DetermineTargets(List<Combatant> chars);
        CombatantUpdateResults ApplyActionResultToCombatant(Combatant attacker, Combatant defender, ActionResults results, int segment);
    }
}
