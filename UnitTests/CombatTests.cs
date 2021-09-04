using NPCConsoleTesting;
using NPCConsoleTesting.Combat;
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
            new Combatant("testChar1", "Fighter", 1, 12, 12, 10, 0, 10, 1, 1, 4, 1),
            new Combatant("testChar2", "Fighter", 1, 12, 12, 10, 0, 10, 1, 1, 4, 1),
            new Combatant("testChar3", "Fighter", 1, 12, 12, 10, 0, 10, 1, 1, 4, 1)
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
            //for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            //{
            //    if (combatMethods.DoMeleeAttack(thac0, poorAC, numOfAttackDice, typeOfAttackDie, dmgModifier) == 0) { missesAgainstPoorAC++; }
            //    if (combatMethods.DoMeleeAttack(thac0, goodAC, numOfAttackDice, typeOfAttackDie, dmgModifier) != 0) { hitsAgainstGoodAC++; }
            //}

            ////Assert
            //Assert.Multiple(() =>
            //{
            //    Assert.That((float)missesAgainstPoorAC / (float)TIMES_TO_LOOP_FOR_RANDOM_TESTS, Is.LessThan(ACCURACY_RANGE_FOR_5_PERCENT_OCCURENCE));
            //    Assert.That((float)hitsAgainstGoodAC / (float)TIMES_TO_LOOP_FOR_RANDOM_TESTS, Is.LessThan(ACCURACY_RANGE_FOR_5_PERCENT_OCCURENCE));
            //});
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
            //for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            //{
            //    resultsList.Add(combatMethods.CalcDmg(numOfAttackDice, typeOfAttackDie, dmgModifier));
            //}

            ////Assert
            //Assert.That(resultsList, Is.All.GreaterThan(2) & Is.All.LessThan(9) & Has.Member(3) & Has.Member(8));
        }

        [Test]
        public void DoACombatRound_returns_logResults()
        {
            //Act
            List<string> logResults = CombatRound.DoACombatRound(testList);

            //Assert
            Assert.That(logResults, Is.Not.Null);
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
        public void DoAFullCombat_leaves_one_or_zero_remaining()
        {
            //Arrange
            List<Combatant> fullCombatTestList = new()
            {
                new Combatant("testChar1", "Fighter", 1, 12, 12, 10, 0, 10, 1, 1, 4, 1),
                new Combatant("testChar2", "Fighter", 1, 12, 12, 10, 0, 10, 1, 1, 4, 1),
                new Combatant("testChar3", "Fighter", 1, 12, 12, 10, 0, 10, 1, 1, 4, 1)
            };

            //Act
            FullCombat.DoAFullCombat(fullCombatTestList);

            //Assert
            Assert.Less(fullCombatTestList.Where(x => x.CurrentHP > 0).Count(), 2);
        }

        [Test]
        public void Simultaneous_init_allows_attack_from_dead_combatant()
        {
            //Arrange
            List<Combatant> twoCombatantTestList = new()
            {
                new Combatant("testChar1", "Fighter", 1, 12, 12, 1, 0, 10, 1, 1, 4, 1),
                new Combatant("testChar2", "Fighter", 1, 12, 12, 1, 0, 10, 1, 1, 4, 1)
            };

            int init1 = 0;
            int init2 = 0;
            bool simultaneousInit = false;

            //Act
            while (!simultaneousInit)
            {
                CombatRound.DoACombatRound(twoCombatantTestList);
                
                if (twoCombatantTestList[0].CurrentHP <= 0 && twoCombatantTestList[1].CurrentHP <= 0)
                {
                    init1 = twoCombatantTestList[0].Init;
                    init2 = twoCombatantTestList[1].Init;
                    simultaneousInit = true;
                }
                else
                {
                    twoCombatantTestList[0].CurrentHP = 1;
                    twoCombatantTestList[1].CurrentHP = 1;
                }
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(init1, init2);
                Assert.That(twoCombatantTestList.Select(x => x.CurrentHP), Is.All.LessThanOrEqualTo(0));
            });
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
