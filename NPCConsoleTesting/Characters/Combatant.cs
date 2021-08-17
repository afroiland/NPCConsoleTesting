
namespace NPCConsoleTesting
{
    public class Combatant :ICombatant
    {
        public string name { get; set; }
        public int hp { get; set; }
        public int initMod { get; set; }
        public int ac { get; set; }
        public int thac0 { get; set; }
        public int numberOfAttackDice { get; set; }
        public int typeOfAttackDie { get; set; }
        public int dmgModifier { get; set; }
        public int init { get; set; }
        public string target { get; set; }

        public Combatant(string charName, int charHp, int charInitMod, int charAc, int charThac0, int charNumOfAttackDice, int charTypeOfAttackDie, int charDmgModifier)
        {
            name = charName;
            hp = charHp;
            initMod = charInitMod;
            ac = charAc;
            thac0 = charThac0;
            numberOfAttackDice = charNumOfAttackDice;
            typeOfAttackDie = charTypeOfAttackDie;
            dmgModifier = charDmgModifier;
            init = 0;
            target = "";
        }
    }
}
