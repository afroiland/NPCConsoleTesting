
namespace NPCConsoleTesting
{
    interface ICombatant
    {
        string name { get; set; }
        int hp { get; set; }
        int initMod { get; set; }
        int ac { get; set; }
        int thac0 { get; set; }
        int numberOfAttackDice { get; set; }
        int typeOfAttackDie { get; set; }
        int dmgModifier { get; set; }
        int init { get; set; }
        string target { get; set; }
    }
}
