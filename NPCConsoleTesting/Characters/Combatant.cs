using NPCConsoleTesting.Characters;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public class Combatant : BaseCharacter, ICombatant
    {
        private int init;

        public string Armor { get; set; }
        public string Weapon { get; set; }
        public bool HasShield { get; set; }
        public int Init
        {
            get { return init; }
            set { if (value < 1) init = 1; else init = value; }
        }
        public string Target { get; set; }
        public bool GotHitThisRound { get; set; }
        public List<string> Spells { get; set; }

        public Combatant(string charName, string charClass, int charLevel, int charStrength, int charDexterity, int charHp, int charInitMod,
            int charEx_Strength = 0, string charArmor = "None", string charWeapon = "None", bool charHasShield = false, List<string> charSpells = null)
        {
            Name = charName;
            CharacterClass = charClass;
            Level = charLevel;
            Strength = charStrength;
            Ex_Strength = charEx_Strength;
            Dexterity = charDexterity;
            CurrentHP = charHp;
            InitMod = charInitMod;
            Armor = charArmor;
            Weapon = charWeapon;
            HasShield = charHasShield;
            Spells = charSpells;
            Init = 0;
            Target = "";
            GotHitThisRound = false;
            Statuses = new List<string>();
        }
    }
}
