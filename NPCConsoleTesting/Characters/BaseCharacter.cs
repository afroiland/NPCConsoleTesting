using System.Collections.Generic;

namespace NPCConsoleTesting.Characters
{
    public class BaseCharacter
    {
        public string Name { get; set; }
        public string CharacterClass { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int? Ex_Strength { get; set; }
        public int Dexterity { get; set; }
        public List<string> HP_By_Level { get; set; }
        public int CurrentHP { get; set; }
        public int InitMod { get; set; }
        public List<string> Statuses { get; set; }
    }
}
