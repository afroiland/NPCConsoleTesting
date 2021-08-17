using NPCConsoleTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    class BuildTests
    {
        [Test]
        public void BuildCombatantRandomly_returns_combatant_within_ranges()
        {
            //Arrange
            Build build = new();
            List<Combatant> resultsList = new();

            //Act
            for (int i = 0; i < 20; i++)
            {
                resultsList.Add(build.BuildCombatantRandomly());
            }

            //Assert
            Assert.Multiple(() =>
            {
                foreach (Combatant cmbt in resultsList)
                {
                    Assert.That(cmbt.name, Is.Not.Null);
                    Assert.That(cmbt.hp, Is.GreaterThan(0) & Is.LessThan(11));
                    Assert.That(cmbt.initMod, Is.GreaterThan(0) & Is.LessThan(6));
                    Assert.That(cmbt.ac, Is.GreaterThan(-11) & Is.LessThan(11));
                    Assert.That(cmbt.thac0, Is.GreaterThan(0) & Is.LessThan(21));
                    Assert.That(cmbt.numberOfDice, Is.GreaterThan(0) & Is.LessThan(3));
                    Assert.That(cmbt.typeOfDie, Is.GreaterThan(0) & Is.LessThan(7));
                    Assert.That(cmbt.dmgModifier, Is.GreaterThan(-1) & Is.LessThan(3));
                }
            });
        }

        [Test]
        public void GenerateRandomName_returns_name_within_ranges()
        {
            //Arrange
            List<string> resultsList = new();

            //Act
            for (int i = 0; i < 50; i++)
            {
                resultsList.Add(Build.GenerateRandomName());
            }

            //Assert
            Assert.That(resultsList, Is.All.Length.GreaterThan(2) & Is.All.Length.LessThan(16));
        }
    }
}
