namespace NPCConsoleTesting.Combat
{
    public interface IDefender
    {
        string CharacterClass { get; set; }
        int Level { get; set; }
        int Dexterity { get; set; }
        string Armor { get; set; }
        bool HasShield { get; set; }
    }
}
