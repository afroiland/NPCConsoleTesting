using NPCConsoleTesting.Characters;
using System.Collections.Generic;

namespace NPCConsoleTesting.Combat
{
    public interface IMultipleCombats
    {
        void DisplayWinRates(List<Winner> winners);
        List<Winner> DoMultipleCombats(List<Combatant> combatants, int numberOfCombats, bool isTeamBattle);
        int GetNumberOfTimesToRun();
        void PredictWinner(List<Combatant> combatants, bool isTeamBattle);
    }
}