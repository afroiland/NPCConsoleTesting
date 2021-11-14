using NPCConsoleTesting;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    class BuildTests
    {
        const int TIMES_TO_LOOP_FOR_RANDOM_TESTS = 100;

        [Test]
        public void BuildCombatantRandomly_returns_combatant_within_ranges()
        {

            //Arrange
            CombatantBuilder cBuilder = new();
            List<Combatant> resultsList = new();
            List<string> resultsListClass = new();
            List<string> resultsListRace = new();
            List<string> resultsListArmor = new();
            List<string> resultsListWeapon = new();

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                resultsList.Add(cBuilder.BuildCombatantRandomly());
                resultsListClass.Add(resultsList[i].CharacterClass);
                resultsListRace.Add(resultsList[i].Race);
                resultsListArmor.Add(resultsList[i].Armor);
                resultsListWeapon.Add(resultsList[i].Weapon);
            }

            //Assert
            Assert.Multiple(() =>
            {
                foreach (Combatant cmbt in resultsList)
                {
                    Assert.That(cmbt.Name, Is.Not.Null);
                    Assert.That(cmbt.CharacterClass, Is.Not.Null);
                    Assert.That(cmbt.Race, Is.Not.Null);
                    Assert.That(cmbt.Level, Is.Not.Null & Is.GreaterThan(0) & Is.LessThan(6));
                    Assert.That(cmbt.Strength, Is.Not.Null);
                    Assert.That(cmbt.Ex_Strength, Is.Not.Null);
                    Assert.That(cmbt.Dexterity, Is.Not.Null);
                    Assert.That(cmbt.Constitution, Is.Not.Null);
                    Assert.That(cmbt.HP_By_Level, Is.Not.Null);
                    //Assert.That(cmbt.CurrentHP, Is.GreaterThan(3) & Is.LessThan(69));
                    //Assert.That(cmbt.InitMod, Is.GreaterThan(-1) & Is.LessThan(6));
                    Assert.That(cmbt.Armor, Is.Not.Null);
                    Assert.That(cmbt.Weapon, Is.Not.Null);
                    Assert.That(cmbt.HasShield, Is.Not.Null);
                }

                CollectionAssert.Contains(resultsListClass, "fighter");
                CollectionAssert.Contains(resultsListClass, "magic-user");
                CollectionAssert.Contains(resultsListClass, "monk");
                CollectionAssert.Contains(resultsListClass, "assassin");

                CollectionAssert.Contains(resultsListRace, "human");
                CollectionAssert.Contains(resultsListRace, "elf");
                CollectionAssert.Contains(resultsListRace, "dwarf");
                CollectionAssert.Contains(resultsListRace, "halfling");

                CollectionAssert.Contains(resultsListArmor, "leather");
                CollectionAssert.Contains(resultsListArmor, "scale");
                CollectionAssert.Contains(resultsListArmor, "chain");
                CollectionAssert.Contains(resultsListArmor, "plate");

                //TODO: add assertions once SelectRandomWeapon() gets built out
                CollectionAssert.Contains(resultsListWeapon, "dagger");
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

        [Test]
        public void GenerateHPByLevelByCharClass_returns_values_within_range()
        {
            //Act
            List<int> fighterLvl1 = CombatantBuilder.GenerateHPByLevelByCharClass("fighter", 1);
            List<int> rangerLvl1 = CombatantBuilder.GenerateHPByLevelByCharClass("ranger", 1);
            List<int> rangerLvl7 = CombatantBuilder.GenerateHPByLevelByCharClass("ranger", 7);
            List<int> thiefLvl3 = CombatantBuilder.GenerateHPByLevelByCharClass("thief", 3);
            List<int> monkLvl5 = CombatantBuilder.GenerateHPByLevelByCharClass("monk", 5);

            //for testing the range of each values, the first int is excluded for monks and rangers
            List<int> rangerLvl7Copy = new(rangerLvl7);
            rangerLvl7Copy.RemoveAt(0);
            List<int> monkLvl5Copy = new(monkLvl5);
            monkLvl5Copy.RemoveAt(0);

            //Assert
            Assert.That(fighterLvl1[0], Is.EqualTo(10));
            Assert.That(rangerLvl1[0], Is.EqualTo(16));
            Assert.That(rangerLvl7.Sum(), Is.GreaterThan(21) & Is.LessThan(65));
            Assert.That(rangerLvl7Copy, Is.All.GreaterThan(0) & Is.All.LessThan(9));
            Assert.That(thiefLvl3.Sum(), Is.GreaterThan(7) & Is.LessThan(19));
            Assert.That(thiefLvl3, Is.All.GreaterThan(0) & Is.All.LessThan(7));
            Assert.That(monkLvl5.Sum(), Is.GreaterThan(11) & Is.LessThan(25));
            Assert.That(monkLvl5Copy, Is.All.GreaterThan(0) & Is.All.LessThan(5));
        }

        //GenerateAttributes (including racial mods)

        //GenerateSpellList    (for five classes; maybe break this out into 5 separate tests)
    }
}
