using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCConsoleTesting
{
    class Character
    {
        int hp;
        int initMod;
        int ac;
        int thac0;
        public int calcDmg()
        {
            Random _random = new Random();
            return _random.Next(1, 6);
        }
                
    }
}
