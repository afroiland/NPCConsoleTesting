using NPCConsoleTesting;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    public class CombatTests
    {
        //Arrange
        List<Combatant> testList = new()
        {
            new Combatant("testChar1", 10, 0, 10, 1, 1, 4, 1),
            new Combatant("testChar2", 10, 0, 10, 1, 1, 4, 1),
            new Combatant("testChar3", 10, 0, 10, 1, 1, 4, 1)
        };

        [Test]
        public void Attack_succeeds_and_fails_as_expected()
        {
            //Arrange
            int thac0 = 11;
            int poorAC = 10;
            int goodAC = -10;
            int numOfDice = 1;
            int typeOfDie = 6;
            int modifier = 2;
            List<int> resultsListPoorAC = new();
            List<int> resultsListGoodAC = new();

            //Act
            for (int i = 0; i < 50; i++)
            {
                resultsListPoorAC.Add(CombatMethods.Attack(thac0, poorAC, numOfDice, typeOfDie, modifier));
                resultsListGoodAC.Add(CombatMethods.Attack(thac0, goodAC, numOfDice, typeOfDie, modifier));
            }

            //Assert
            Assert.That(resultsListPoorAC, Is.All.GreaterThan(0));
            Assert.That(resultsListGoodAC, Is.All.LessThan(1));
        }

        [Test]
        public void CalcDmg_falls_within_range()
        {
            //Arrange
            int numOfDice = 1;
            int typeOfDie = 6;
            int modifier = 2;
            List<int> resultsList = new();

            //Act
            for (int i = 0; i < 50; i++)
            {
                resultsList.Add(CombatMethods.CalcDmg(numOfDice, typeOfDie, modifier));
            }

            //Assert
            Assert.That(resultsList, Is.All.GreaterThan(2) & Is.All.LessThan(9) & Has.Member(3) & Has.Member(8));
        }

        [Test]
        public void Inits_get_set_for_all_chars()
        {
            //Act
            var result = CombatMethods.DetermineInit(testList);

            //Assert
            Assert.That(result, Is.Ordered.By("init"));
        }

        [Test]
        public void Targets_get_set_for_all_chars()
        {
            //Act
            var result = CombatMethods.DetermineTargets(testList);

            //Assert
            foreach (Combatant ch in result)
            {
                Assert.AreNotEqual(ch.target, "");
            }

            //TODO: The following seems cleaner once "target" has been made into a property
            //Assert.That(List.Map(result).Property("target"), Is.Not.EqualTo(""));
        }
    }
}
