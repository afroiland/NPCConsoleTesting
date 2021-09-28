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
        Combatant testChar = new("testChar", "Fighter", 10, 12, 12, 12, new List<int>() { 1 }, 10);
        Combatant testCharPoorAC = new("testCharPoorAC", "Fighter", 1, 12, 12, 12, new List<int>() { 1 }, 10, otherACBonus: -5);
        Combatant testCharGoodAC = new("testCharGoodAC", "Fighter", 1, 12, 12, 12, new List<int>() { 1 }, 10, otherACBonus: 25);
        
        const int TIMES_TO_LOOP_FOR_RANDOM_TESTS = 100;
        const float ACCURACY_RANGE_FOR_5_PERCENT_OCCURENCE = .17F;

        [Test]
        public void DoAMeleeAttack_succeeds_and_fails_as_expected()
        {
            //Arrange
            int missesAgainstPoorAC = 0;
            int hitsAgainstGoodAC = 0;

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                if (combatMethods.DoAMeleeAttack(testChar, testCharPoorAC) == 0) { missesAgainstPoorAC++; }
                if (combatMethods.DoAMeleeAttack(testChar, testCharGoodAC) != 0) { hitsAgainstGoodAC++; }
            }

            ////Assert
            Assert.Multiple(() =>
            {
                Assert.That((float)missesAgainstPoorAC / (float)TIMES_TO_LOOP_FOR_RANDOM_TESTS, Is.LessThan(ACCURACY_RANGE_FOR_5_PERCENT_OCCURENCE));
                Assert.That((float)hitsAgainstGoodAC / (float)TIMES_TO_LOOP_FOR_RANDOM_TESTS, Is.LessThan(ACCURACY_RANGE_FOR_5_PERCENT_OCCURENCE));
            });
        }

        [Test]
        public void CalcThac0_returns_correct_values()
        {
            //Act
            int fighterLvl1Thac0 = CombatMethods.CalcThac0("Fighter", 1);
            int paladinLvl10Thac0 = CombatMethods.CalcThac0("Paladin", 10);
            int muLvl6Thac0 = CombatMethods.CalcThac0("Magic-User", 6);
            int illusionistLvl11Thac0 = CombatMethods.CalcThac0("Illusionist", 11);
            int clericLvl4Thac0 = CombatMethods.CalcThac0("Cleric", 4);
            int druidLvl10Thac0 = CombatMethods.CalcThac0("Druid", 10);
            int thiefLvl2Thac0 = CombatMethods.CalcThac0("Thief", 2);
            int assassinLvl10Thac0 = CombatMethods.CalcThac0("Assassin", 10);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(fighterLvl1Thac0, Is.EqualTo(20));
                Assert.That(paladinLvl10Thac0, Is.EqualTo(11));
                Assert.That(muLvl6Thac0, Is.EqualTo(19));
                Assert.That(illusionistLvl11Thac0, Is.EqualTo(16));
                Assert.That(clericLvl4Thac0, Is.EqualTo(18));
                Assert.That(druidLvl10Thac0, Is.EqualTo(14));
                Assert.That(thiefLvl2Thac0, Is.EqualTo(20));
                Assert.That(assassinLvl10Thac0, Is.EqualTo(16));
            });
        }

        //CalcNonMonkAC
        [Test]
        public void CalcNonMonkAC_returns_correct_value()
        {
            //Act
            int chainWithShield = CombatMethods.CalcNonMonkAC("Chain", true, 12, 0);
            int chainNoShield = CombatMethods.CalcNonMonkAC("Chain", false, 12, 0);
            int leatherDex16 = CombatMethods.CalcNonMonkAC("Leather", false, 16, 0);
            int noArmorDex18 = CombatMethods.CalcNonMonkAC("None", false, 18, 0);
            int plateShieldAndBonus = CombatMethods.CalcNonMonkAC("Plate", true, 12, 5);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(chainWithShield, Is.EqualTo(4));
                Assert.That(chainNoShield, Is.EqualTo(5));
                Assert.That(leatherDex16, Is.EqualTo(6));
                Assert.That(noArmorDex18, Is.EqualTo(6));
                Assert.That(plateShieldAndBonus, Is.EqualTo(-3));
            });
        }

        [Test]
        public void CalcMonkAC_returns_correct_value()
        {
            //Act
            int level1Result = CombatMethods.CalcMonkAC(1, 0);
            int level3Result = CombatMethods.CalcMonkAC(3, 0);
            int level7Result = CombatMethods.CalcMonkAC(7, 0);
            int level10Result = CombatMethods.CalcMonkAC(10, 0);
            int level17Result = CombatMethods.CalcMonkAC(17, 0);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(level1Result, Is.EqualTo(10));
                Assert.That(level3Result, Is.EqualTo(8));
                Assert.That(level7Result, Is.EqualTo(5));
                Assert.That(level10Result, Is.EqualTo(3));
                Assert.That(level17Result, Is.EqualTo(-3));
            });
        }

        //CalcStrBonusToHit

        //CalcStrBonusToDmg

        [Test]
        public void CalcConBonusToHP_returns_correct_value()
        {
            //Act
            int fighter14Con = CombatMethods.CalcConBonusToHP(14, "Fighter");
            int ranger18Con = CombatMethods.CalcConBonusToHP(18, "Ranger");
            int paladin17Con = CombatMethods.CalcConBonusToHP(17, "Paladin");
            int cleric15Con = CombatMethods.CalcConBonusToHP(15, "Cleric");
            int monk17Con = CombatMethods.CalcConBonusToHP(17, "Monk");
            int thief5Con = CombatMethods.CalcConBonusToHP(5, "Thief");

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(fighter14Con, Is.EqualTo(0));
                Assert.That(ranger18Con, Is.EqualTo(4));
                Assert.That(paladin17Con, Is.EqualTo(3));
                Assert.That(cleric15Con, Is.EqualTo(1));
                Assert.That(monk17Con, Is.EqualTo(2));
                Assert.That(thief5Con, Is.EqualTo(0));
            });
        }

        //GetWeaponInfo

        //GetCastingTime

        //GetSpeedFactor

        //CalcMeleeDmg_falls_within_range_for_non_monk()

        [Test]
        public void CalcMeleeDmg_falls_within_range_for_non_monk()
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
            //Arrange
            List<Combatant> testList = new() {testChar, testCharGoodAC, testCharPoorAC };

            //Act
            List<string> logResults = CombatRound.DoACombatRound(testList);

            //Assert
            Assert.That(logResults, Is.Not.Null);
        }

        [Test]
        public void Inits_get_set_for_all_chars()
        {
            //Arrange
            List<Combatant> testList = new() { testChar, testCharGoodAC, testCharPoorAC };

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
                new Combatant("testChar1", "Fighter", 1, 12, 12, 12, new List<int>() { 1 }, 10, 0),
                new Combatant("testChar2", "Fighter", 1, 12, 12, 12, new List<int>() { 1 }, 10, 0),
                new Combatant("testChar3", "Fighter", 1, 12, 12, 12, new List<int>() { 1 }, 10, 0)
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
                new Combatant("testChar1", "Fighter", 1, 12, 12, 12, new List<int>() { 1 }, 1, 0),
                new Combatant("testChar2", "Fighter", 1, 12, 12, 12, new List<int>() { 1 }, 1, 0)
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
            //Arrange
            List<Combatant> testList = new() { testChar, testCharGoodAC, testCharPoorAC };

            //Act
            var result = combatMethods.DetermineTargets(testList);

            //Assert
            CollectionAssert.DoesNotContain(result.Select(x => x.Target), "");
        }
    }
}
