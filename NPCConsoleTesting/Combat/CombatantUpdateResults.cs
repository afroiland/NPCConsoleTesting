using System.Collections.Generic;

namespace NPCConsoleTesting.Combat
{
    public class CombatantUpdateResults
    {
        public List<string> LogEntries { get; set; }
        public bool OpportunityForSimulAttack { get; set; }

        public CombatantUpdateResults(List<string> logEntries, bool opportunityForSimulAttack)
        {
            LogEntries = logEntries;
            OpportunityForSimulAttack = opportunityForSimulAttack;
        }
    }
}
