using NPCConsoleTesting;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    class BuildTests
    {
        const int TIMES_TO_LOOP_FOR_RANDOM_TESTS = 50;

        [Test]
        public void BuildCombatantRandomly_returns_combatant_within_ranges()
        {
            //Arrange
            CombatantBuilder cBuilder = new();
            List<ICombatant> resultsList = new();

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                resultsList.Add(cBuilder.BuildCombatantRandomly());
            }

            //Assert
            Assert.Multiple(() =>
            {
                foreach (ICombatant cmbt in resultsList)
                {
                    Assert.That(cmbt.Name, Is.Not.Null);
                    Assert.That(cmbt.HP, Is.GreaterThan(0) & Is.LessThan(11));
                    Assert.That(cmbt.InitMod, Is.GreaterThan(0) & Is.LessThan(6));
                    Assert.That(cmbt.AC, Is.GreaterThan(-11) & Is.LessThan(11));
                    Assert.That(cmbt.Thac0, Is.GreaterThan(0) & Is.LessThan(21));
                    Assert.That(cmbt.NumberOfAttackDice, Is.GreaterThan(0) & Is.LessThan(3));
                    Assert.That(cmbt.TypeOfAttackDie, Is.GreaterThan(0) & Is.LessThan(7));
                    Assert.That(cmbt.DmgModifier, Is.GreaterThan(-1) & Is.LessThan(3));
                }
            });
        }

        [Test]
        public void GenerateRandomName_returns_name_within_ranges()
        {
            //Arrange
            List<string> resultsList = new();

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                resultsList.Add(CombatantBuilder.GenerateRandomName());
            }

            //Assert
            Assert.That(resultsList, Is.All.Length.GreaterThan(2) & Is.All.Length.LessThan(16));
        }
    }
}
