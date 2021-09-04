namespace NPCConsoleTesting.Combat
{
    public class WeaponInfo
    {
        public int NumberOfAttackDice { get; set; }
        public int TypeOfAttackDie { get; set; }
        public int DmgModifier { get; set; }

        public WeaponInfo(int weaponNumOfAttackDice, int weaponTypeOfAttackDie, int weaponDmgModifier)
        {
            NumberOfAttackDice = weaponNumOfAttackDice;
            TypeOfAttackDie = weaponTypeOfAttackDie;
            DmgModifier = weaponDmgModifier;
        }
    }
}
