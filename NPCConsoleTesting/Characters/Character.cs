using NPCConsoleTesting.Characters;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public class Character : BaseCharacter
    {
        //public string name;
        public string title;
        //public string characterClass;
        //public int level;
        public string race;
        public int age;
        public string gender;
        //public int currentHP;
        //public List<int> hpByLevel;
        public int ac_adj;
        public int att_adj;
        //public string status;
        //public int str;
        //public int ex_str;
        //public int intel;
        //public int wis;
        //public int dex;
        //public int con;
        //public int cha;
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
        public int numberOfAttackDice;
        public int typeOfAttackDie;
        public int dmgModifier;

        public Character()
        {

        }
    }
}
