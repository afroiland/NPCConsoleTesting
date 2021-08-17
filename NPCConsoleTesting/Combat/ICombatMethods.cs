using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public interface ICombatMethods
    {
        int Attack(int thac0, int ac, int numberOfDice, int typeOfDie, int dmgModifier);
        int CalcDmg(int numberOfDice, int typeOfDie, int dmgModifier);
        List<Combatant> DetermineInit(List<Combatant> chars);
        List<Combatant> DetermineTargets(List<Combatant> chars);
    }
}
