using System.Collections.Generic;

namespace NPCConsoleTesting.Characters
{
    public class BaseCharacter
    {
        public string Name { get; set; }
        public List<int> HPByLevel { get; set; }
        public int InitMod { get; set; }
        public int Level { get; set; }
        public List<string> Statuses { get; set; }
    }
}
