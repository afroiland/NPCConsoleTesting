using System.Collections.Generic;

namespace NPCConsoleTesting
{
    public interface ICombatant
    {
        string Name { get; set; }
        string CharacterClass { get; set; }
        int Level { get; set; }
        int CurrentHP { get; set; }
        int InitMod { get; set; }
        List<string> Statuses { get; set; }
        string Armor { get; set; }
        string Weapon { get; set; }
        bool HasShield { get; set; }
        int Init { get; set; }
        string Target { get; set; }
        bool GotHitThisRound { get; set; }
    }
}
