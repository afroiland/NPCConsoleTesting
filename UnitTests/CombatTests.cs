using NPCConsoleTesting;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    class CombatTests
    {
        //Arrange
        ICombatMethods combatMethods = new CombatMethods();
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
            int dmgModifier = 2;
            List<int> resultsListPoorAC = new();
            List<int> resultsListGoodAC = new();

            //Act
            for (int i = 0; i < 50; i++)
            {
                resultsListPoorAC.Add(combatMethods.Attack(thac0, poorAC, numOfDice, typeOfDie, dmgModifier));
                resultsListGoodAC.Add(combatMethods.Attack(thac0, goodAC, numOfDice, typeOfDie, dmgModifier));
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultsListPoorAC, Is.All.GreaterThan(0));
                Assert.That(resultsListGoodAC, Is.All.LessThan(1));
            });
        }

        [Test]
        public void CalcDmg_falls_within_range()
        {
            //Arrange
            int numOfDice = 1;
            int typeOfDie = 6;
            int dmgModifier = 2;
            List<int> resultsList = new();

            //Act
            for (int i = 0; i < 50; i++)
            {
                resultsList.Add(combatMethods.CalcDmg(numOfDice, typeOfDie, dmgModifier));
            }

            //Assert
            Assert.That(resultsList, Is.All.GreaterThan(2) & Is.All.LessThan(9) & Has.Member(3) & Has.Member(8));
        }

        [Test]
        public void CombatRound_returns_RoundResults()
        {
            //Act
            var result = Combat.CombatRound(testList);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.combatants, Is.Not.Null);
                Assert.That(result.roundLog, Is.Not.Null);
            });
        }

        [Test]
        public void Inits_get_set_for_all_chars()
        {
            //Act
            var result = combatMethods.DetermineInit(testList);

            //Assert
            Assert.That(result, Is.Ordered.By("init"));
        }

        [Test]
        public void Targets_get_set_for_all_chars()
        {
            //Act
            var result = combatMethods.DetermineTargets(testList);

            //Assert
            CollectionAssert.DoesNotContain(result.Select(x => x.target), "");
        }
    }
}
