using System.Collections.Generic;

namespace NPCConsoleTesting.Combat
{
    public interface IFullCombat
    {
        int MaxNumberOfCombatantsToDisplay { get; set; }

        bool DetermineIfSingleBattle();
        bool DetermineIfTeamBattle();
        void DisplayCountdown();
        void DisplayPostCombatInformation(List<string> combatLog);
        void DisplayPreCombatInformation(List<Combatant> combatants, bool isTeamBattle);
        List<string> DoAFullCombat(List<Combatant> combatants, bool isTeamBattle);
    }
}