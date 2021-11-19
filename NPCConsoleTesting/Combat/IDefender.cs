using NPCConsoleTesting.Characters;
using System.Collections.Generic;

namespace NPCConsoleTesting.Combat
{
    public interface IDefender
    {
        string CharacterClass { get; set; }
        int Level { get; set; }
        int Dexterity { get; set; }
        string Armor { get; set; }
        bool HasShield { get; set; }
        int OtherACBonus { get; set; }
        List<Status> Statuses { get; set; }
    }
}
