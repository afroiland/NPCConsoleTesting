using NPCConsoleTesting.Combat;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatMethods
    {
        ActionResults DoAMeleeAttack(IAttacker attacker, IDefender defender);
        int CalcMeleeDmg(string attackerClass, string weapon, int str, int ex_str, int magicalBonus, int otherDmgBonus = 0);
        void IncrementStatuses(List<Combatant> chars, List<string> log);
        void DetermineInit(List<Combatant> chars);
        void DetermineTargets(List<Combatant> chars);
        CombatantUpdateResults ApplyActionResultToCombatant(Combatant attacker, Combatant defender, ActionResults results, int segment);
        CombatantUpdateResults ApplyMeleeResultToCombatant(Combatant attacker, Combatant defender, ActionResults attackResult, int segment);
        CombatantUpdateResults ApplySpellResultToCombatant(Combatant caster, Combatant target, ActionResults spellResults, int segment);
    }
}
