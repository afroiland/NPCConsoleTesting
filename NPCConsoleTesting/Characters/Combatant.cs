using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class Combatant
    {
        public string name { get; set; }
        public int hp { get; set; }
        public int initMod { get; set; }
        public int ac { get; set; }
        public int thac0 { get; set; }
        public int numberOfDice { get; set; }
        public int typeOfDie { get; set; }
        public int dmgModifier { get; set; }
        public int init { get; set; }
        public string target { get; set; }

        public Combatant(string charName, int charHp, int charInitMod, int charAc, int charThac0, int charNumOfDice, int charTypeOfDie, int charDmgModifier)
        {
            name = charName;
            hp = charHp;
            initMod = charInitMod;
            ac = charAc;
            thac0 = charThac0;
            numberOfDice = charNumOfDice;
            typeOfDie = charTypeOfDie;
            dmgModifier = charDmgModifier;
            init = 0;
            target = "";
        }
    }
}
