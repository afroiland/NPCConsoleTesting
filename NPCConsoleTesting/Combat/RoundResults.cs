using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public class RoundResults
    {
        public List<ICombatant> combatants { get; set; }
        public List<string> roundLog { get; set; }

        public RoundResults(List<ICombatant> cmbts, List<string> log)
        {
            combatants = cmbts;
            roundLog = log;
        }
    }
}
