using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    public class Character
    {
        public int hp;
        public int initMod;
        public int ac;
        public int thac0;
        public int numberOfDice;
        public int typeOfDie;
        public int modifier;

        public Character(int charHp, int charInitMod, int charAc, int charThac0, int charNumOfDice, int charTypeOfDie, int charModifier)
        {
            hp = charHp;
            initMod = charInitMod;
            ac = charAc;
            thac0 = charThac0;
            numberOfDice = charNumOfDice;
            typeOfDie = charTypeOfDie;
            modifier = charModifier;
        }
    }
}
