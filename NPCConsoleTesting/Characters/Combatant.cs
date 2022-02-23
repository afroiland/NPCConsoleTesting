using NPCConsoleTesting.Characters;
using NPCConsoleTesting.Combat;
using System.Collections.Generic;
using System.Text.Json;

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
        public int OtherACBonus { get; set; }
        public int InitMod { get; set; }
        public int Init
        {
            get { return init; }
            set { if (value < 1) init = 1; else init = value; }
        }
        public string Target { get; set; }
        public string ActionForThisRound { get; set; }
        public bool GotHitThisRound { get; set; }
        public List<string> Spells { get; set; }
        public string Affiliation { get; set; }

        public Combatant(string charName, string charClass, int charLevel, string charRace, int charStrength, int charDexterity, int charCon,
            List<int> charHP_By_Level, int charCurrentHP, int charInitMod = 0, int magicalBonus = 0, int otherHitBonus = 0, int otherDmgBonus = 0,
            int otherACBonus = 0, int initMod = 0, int charEx_Strength = 0, string charArmor = "none", string charWeapon = "none",
            bool charHasShield = false, List<string> charSpells = null, List<Status> charStatuses = null, string charAffiliation = "none")
        {
            Name = charName;
            CharacterClass = charClass;
            Level = charLevel;
            Race = charRace;
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
            OtherACBonus = otherACBonus;
            InitMod = initMod;
            Spells = charSpells;
            Affiliation = charAffiliation;
            Init = 0;
            Target = "";
            ActionForThisRound = "";
            GotHitThisRound = false;
            Statuses = charStatuses;
        }

        //json constructor
        public Combatant() { }

        public Combatant DeepClone()
        {
            return JsonSerializer.Deserialize<Combatant>(JsonSerializer.Serialize(this, this.GetType()));
        }
    }
}
