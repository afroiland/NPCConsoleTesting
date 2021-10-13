namespace NPCConsoleTesting.Combat
{
    public interface IAttacker
    {
        string CharacterClass { get; set; }
        int Level { get; set; }
        int Strength { get; set; }
        int Ex_Strength { get; set; }
        string Weapon { get; set; }
        int MagicalBonus { get; set; }
        int OtherHitBonus { get; set; }
        int OtherDmgBonus { get; set; }
    }
}
