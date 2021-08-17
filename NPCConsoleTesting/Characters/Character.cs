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
        public string title;
        public int level;
        public string characterClass;
        public string race;
        public int age;
        public string gender;
        public int currentHP;
        public List<int> hpByLevel;
        public int ac_adj;
        public int att_adj;
        public string status;
        public int str;
        public int ex_str;   //unsure if it's worth to make a child class for this one property
        public int intel;
        public int dex;
        public int con;
        public int wis;
        public int cha;
        public int gold;
        public string armor;
        public string weapon;
        public string items;
        public int probity;
        public string affiliation;
        public string notes;

        //spellbook/memorized in a separate class? Or child classes for MU, cleric, etc.

        //The following not needed? Determined during combat?
        public int thac0;
        public int numberOfDice;
        public int typeOfDie;
        public int dmgModifier;

        public Character()
        {

        }
    }
}
