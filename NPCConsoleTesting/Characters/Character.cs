using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class Character
    {
        public string name;
        public int hp;
        public int initMod;
        public int ac;
        public int thac0;
        public int numberOfDice;
        public int typeOfDie;
        public int modifier;
        public int init;
        public string target;

        public Character(string charName, int charHp, int charInitMod, int charAc, int charThac0, int charNumOfDice, int charTypeOfDie, int charModifier)
        {
            name = charName;
            hp = charHp;
            initMod = charInitMod;
            ac = charAc;
            thac0 = charThac0;
            numberOfDice = charNumOfDice;
            typeOfDie = charTypeOfDie;
            modifier = charModifier;
            init = 0;
            target = "";
        }
    }
}
