namespace NPCConsoleTesting
{
    public class Combatant : ICombatant
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int InitMod { get; set; }
        public int AC { get; set; }
        public int Thac0 { get; set; }
        public int NumberOfAttackDice { get; set; }
        public int TypeOfAttackDie { get; set; }
        public int DmgModifier { get; set; }
        public int Init { get; set; }
        public string Target { get; set; }

        public Combatant(string charName, int charHp, int charInitMod, int charAc, int charThac0, int charNumOfAttackDice, int charTypeOfAttackDie, int charDmgModifier)
        {
            Name = charName;
            HP = charHp;
            InitMod = charInitMod;
            AC = charAc;
            Thac0 = charThac0;
            NumberOfAttackDice = charNumOfAttackDice;
            TypeOfAttackDie = charTypeOfAttackDie;
            DmgModifier = charDmgModifier;
            Init = 0;
            Target = "";
        }
    }
}
