using NPCConsoleTesting.Characters;
using NPCConsoleTesting.Combat;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public class Combatant : BaseCharacter, IAttacker, IDefender
    {
        private int init;

        public string Armor { get; set; }
        public string Weapon { get; set; }
        public bool HasShield { get; set; }
        public int MagicalBonus { get; set; }
        public int OtherHitBonus { get; set; }
        public int OtherDmgBonus { get; set; }
        public int Init
        {
            get { return init; }
            set { if (value < 1) init = 1; else init = value; }
        }
        public string Target { get; set; }
        public bool GotHitThisRound { get; set; }
        public List<string> Spells { get; set; }

        public Combatant(string charName, string charClass, int charLevel, int charStrength, int charDexterity, int charCon, List<int> charHP_By_Level,
            int charCurrentHP, int charInitMod = 0, int magicalBonus = 0, int otherHitBonus = 0, int otherDmgBonus = 0, int charEx_Strength = 0,
            string charArmor = "None", string charWeapon = "None", bool charHasShield = false, List<string> charSpells = null)
        {
            Name = charName;
            CharacterClass = charClass;
            Level = charLevel;
            Strength = charStrength;
            Ex_Strength = charEx_Strength;
            Dexterity = charDexterity;
            Constitution = charCon;
            HP_By_Level = charHP_By_Level;
            CurrentHP = charCurrentHP;
            InitMod = charInitMod;
            Armor = charArmor;
            Weapon = charWeapon;
            HasShield = charHasShield;
            MagicalBonus = magicalBonus;
            OtherHitBonus = otherHitBonus;
            OtherDmgBonus = otherDmgBonus;
            Spells = charSpells;
            Init = 0;
            Target = "";
            GotHitThisRound = false;
            Statuses = new List<string>();
        }
    }
}
