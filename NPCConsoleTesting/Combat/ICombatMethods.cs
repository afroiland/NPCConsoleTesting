using NPCConsoleTesting.Combat;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatMethods
    {
        CombatantUpdateResults ApplyActionResultToCombatant(Combatant targeter, Combatant target, ActionResults results, int segment);
        void DetermineActions(List<Combatant> combatants);
        void DetermineInits(List<Combatant> combatants);
        void DetermineTargetForOneCombatant(List<Combatant> combatants, Combatant priorityC, bool isTeamBattle);
        void DetermineTargets(List<Combatant> combatants, bool isTeamBattle);
        ActionResults DoAMeleeAttack(IAttacker attacker, IDefender defender);
        void IncrementStatuses(List<Combatant> combatants, List<string> log);
    }
}