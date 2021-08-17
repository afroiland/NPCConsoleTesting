using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public class RoundResults
    {
        public List<Combatant> combatants { get; set; }
        public List<string> roundLog { get; set; }

        public RoundResults(List<Combatant> cmbts, List<string> log)
        {
            combatants = cmbts;
            roundLog = log;
        }
    }
}
