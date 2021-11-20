using NPCConsoleTesting.Combat;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatMethods
    {
        ActionResults DoAMeleeAttack(IAttacker attacker, IDefender defender);
        int CalcMonkMeleeDmg(int level, string weapon, int magicalDmgBonus, int otherDmgBonus = 0);
        int CalcNonMonkMeleeDmg(string weapon, int str, int ex_str, int magicalDmgBonus, int otherDmgBonus = 0);
        void IncrementStatuses(List<Combatant> combatants, List<string> log);
        void DetermineActions(List<Combatant> combatants);
        void DetermineInits(List<Combatant> combatants);
        void DetermineTargets(List<Combatant> combatants, bool isTeamBattle);
        void DetermineTargetForOneCombatant(List<Combatant> combatants, Combatant priorityC, bool isTeamBattle);
        CombatantUpdateResults ApplyActionResultToCombatant(Combatant attacker, Combatant defender, ActionResults results, int segment);
    }
}
