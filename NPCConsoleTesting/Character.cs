using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Character
    {
        private int hp;
        private int initMod;
        private int ac;
        private int thac0;
        
        public Character(int charHp, int charInitMod, int charAc, int charThac0)
        {
            hp = charHp;
            initMod = charInitMod;
            ac = charAc;
            thac0 = charThac0;
        }
    }
}
