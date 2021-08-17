
namespace NPCConsoleTesting
{
    public interface ICombatant
    {
        string Name { get; set; }
        int HP { get; set; }
        int InitMod { get; set; }
        int AC { get; set; }
        int Thac0 { get; set; }
        int NumberOfAttackDice { get; set; }
        int TypeOfAttackDie { get; set; }
        int DmgModifier { get; set; }
        int Init { get; set; }
        string Target { get; set; }
    }
}
