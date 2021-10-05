using System.Collections.Generic;

namespace NPCConsoleTesting.Characters
{
    public class BaseCharacter
    {
        public string Name { get; set; }
        public string CharacterClass { get; set; }
        public int Level { get; set; }
        public List<int> HP_By_Level { get; set; }
        public int CurrentHP { get; set; }
        public int InitMod { get; set; }
        public List<Status> Statuses { get; set; }
        public int Strength { get; set; }
        public int Ex_Strength { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Charisma { get; set; }
    }
}
