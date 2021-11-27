using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatRound
    {
        List<string> DoACombatRound(List<Combatant> combatants, bool isTeamBattle);
    }
}