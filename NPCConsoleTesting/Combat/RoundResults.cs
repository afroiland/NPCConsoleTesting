using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class RoundResults
    {
        public List<Combatant> combatants;
        public List<string> roundLog;

        public RoundResults(List<Combatant> cmbts, List<string> log)
        {
            combatants = cmbts;
            roundLog = log;
        }
    }
}
