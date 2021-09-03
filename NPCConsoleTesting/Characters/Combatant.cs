using NPCConsoleTesting.Characters;
using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public class Combatant : BaseCharacter, ICombatant
    {
        //public string Name { get; set; }
        //public string CharacterClass { get; set; }
        //public int Level { get; set; }
        //public int CurrentHP { get; set; }
        //public int InitMod { get; set; }
        //public List<string> Statuses { get; set; }
        public int AC { get; set; }
        public int Thac0 { get; set; }
        public string Weapon { get; set; }
        public int NumberOfAttackDice { get; set; }
        public int TypeOfAttackDie { get; set; }
        public int DmgModifier { get; set; }
        public int Init { get; set; }
        public string Target { get; set; }
        public bool GotHitThisRound { get; set; }
        public List<string> Spells { get; set; }

        public Combatant(string charName, string charClass, int charLevel, int charStrength, int charDexterity, int charHp, int charInitMod,
            int charAc, int charThac0, int charNumOfAttackDice, int charTypeOfAttackDie, int charDmgModifier, int? charEx_Strength = null,
            string charWeapon = "None", List<string> charSpells = null)
        {
            Name = charName;
            CharacterClass = charClass;
            Level = charLevel;
            Strength = charStrength;
            Ex_Strength = charEx_Strength;
            Dexterity = charDexterity;
            CurrentHP = charHp;
            InitMod = charInitMod;
            AC = charAc;
            Thac0 = charThac0;
            Weapon = charWeapon;
            NumberOfAttackDice = charNumOfAttackDice;
            TypeOfAttackDie = charTypeOfAttackDie;
            DmgModifier = charDmgModifier;
            Spells = charSpells;
            Init = 0;
            Target = "";
            GotHitThisRound = false;
            Statuses = new List<string>();
        }
    }
}
