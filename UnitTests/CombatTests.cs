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
        List<ICombatant> testList = new()
        {
            new Fighter("testChar1", 10, 0, 10, 1, 1, 4, 1),
            new Fighter("testChar2", 10, 0, 10, 1, 1, 4, 1),
            new Fighter("testChar3", 10, 0, 10, 1, 1, 4, 1)
        };

        const int TIMES_TO_LOOP_FOR_RANDOM_TESTS = 100;
        const float ACCURACY_RANGE_FOR_5_PERCENT_OCCURENCE = .17F;

        [Test]
        public void Attack_succeeds_and_fails_as_expected()
        {
            //Arrange
            int thac0 = 11;
            int poorAC = 10;
            int goodAC = -10;
            int numOfAttackDice = 1;
            int typeOfAttackDie = 6;
            int dmgModifier = 2;
            int missesAgainstPoorAC = 0;
            int hitsAgainstGoodAC = 0;

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                if (combatMethods.Attack(thac0, poorAC, numOfAttackDice, typeOfAttackDie, dmgModifier) == 0) { missesAgainstPoorAC++; }
                if (combatMethods.Attack(thac0, goodAC, numOfAttackDice, typeOfAttackDie, dmgModifier) != 0) { hitsAgainstGoodAC++; }
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That((float)missesAgainstPoorAC / (float)TIMES_TO_LOOP_FOR_RANDOM_TESTS, Is.LessThan(ACCURACY_RANGE_FOR_5_PERCENT_OCCURENCE));
                Assert.That((float)hitsAgainstGoodAC / (float)TIMES_TO_LOOP_FOR_RANDOM_TESTS, Is.LessThan(ACCURACY_RANGE_FOR_5_PERCENT_OCCURENCE));
            });
        }

        [Test]
        public void CalcDmg_falls_within_range()
        {
            //Arrange
            int numOfAttackDice = 1;
            int typeOfAttackDie = 6;
            int dmgModifier = 2;
            List<int> resultsList = new();

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                resultsList.Add(combatMethods.CalcDmg(numOfAttackDice, typeOfAttackDie, dmgModifier));
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
            Assert.That(result, Is.Ordered.By("Init"));
        }

        [Test]
        public void Targets_get_set_for_all_chars()
        {
            //Act
            var result = combatMethods.DetermineTargets(testList);

            //Assert
            CollectionAssert.DoesNotContain(result.Select(x => x.Target), "");
        }
    }
}
