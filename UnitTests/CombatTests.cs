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
        Combatant testChar = new("testChar", "fighter", 10, "human", 12, 12, 12, new List<int>() { 1 }, 10);
        Combatant testCharPoorAC = new("testCharPoorAC", "fighter", 1, "human", 12, 12, 12, new List<int>() { 1 }, 10, otherACBonus: -5);
        Combatant testCharGoodAC = new("testCharGoodAC", "fighter", 1, "human", 12, 12, 12, new List<int>() { 1 }, 10, otherACBonus: 25);
        
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
                if (CombatMethods.DoAMeleeAttack(testChar, testCharPoorAC).Damage == 0) { missesAgainstPoorAC++; }
                if (CombatMethods.DoAMeleeAttack(testChar, testCharGoodAC).Damage != 0) { hitsAgainstGoodAC++; }
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
            int fighterLvl1Thac0 = CombatMethods.CalcThac0("fighter", 1);
            int paladinLvl10Thac0 = CombatMethods.CalcThac0("paladin", 10);
            int muLvl6Thac0 = CombatMethods.CalcThac0("magic-user", 6);
            int illusionistLvl11Thac0 = CombatMethods.CalcThac0("illusionist", 11);
            int clericLvl4Thac0 = CombatMethods.CalcThac0("cleric", 4);
            int druidLvl10Thac0 = CombatMethods.CalcThac0("druid", 10);
            int thiefLvl2Thac0 = CombatMethods.CalcThac0("thief", 2);
            int assassinLvl10Thac0 = CombatMethods.CalcThac0("assassin", 10);

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

        [Test]
        public void CalcNonMonkAC_returns_correct_value()
        {
            //Act
            int chainWithShield = CombatMethods.CalcNonMonkAC("chain", true, 12, 0);
            int chainNoShield = CombatMethods.CalcNonMonkAC("chain", false, 12, 0);
            int leatherDex16 = CombatMethods.CalcNonMonkAC("leather", false, 16, 0);
            int noArmorDex18 = CombatMethods.CalcNonMonkAC("none", false, 18, 0);
            int plateShieldAndBonus = CombatMethods.CalcNonMonkAC("plate", true, 12, 5);

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

        [Test]
        public void CalcStrBonusToHit_returns_correct_value()
        {
            //Act
            int str10 = CombatMethods.CalcStrBonusToHit(10, 0);
            int str18 = CombatMethods.CalcStrBonusToHit(18, 0);
            int str18_51 = CombatMethods.CalcStrBonusToHit(18, 51);
            int str18_00 = CombatMethods.CalcStrBonusToHit(18, 100);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(str10, Is.EqualTo(0));
                Assert.That(str18, Is.EqualTo(1));
                Assert.That(str18_51, Is.EqualTo(2));
                Assert.That(str18_00, Is.EqualTo(3));
            });
        }

        [Test]
        public void CalcStrBonusToDmg_returns_correct_value()
        {
            //Act
            int str10 = CombatMethods.CalcStrBonusToDmg(10, 0);
            int str18 = CombatMethods.CalcStrBonusToDmg(18, 0);
            int str18_51 = CombatMethods.CalcStrBonusToDmg(18, 51);
            int str18_00 = CombatMethods.CalcStrBonusToDmg(18, 100);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(str10, Is.EqualTo(0));
                Assert.That(str18, Is.EqualTo(2));
                Assert.That(str18_51, Is.EqualTo(3));
                Assert.That(str18_00, Is.EqualTo(6));
            });
        }

        [Test]
        public void CalcConBonusToHP_returns_correct_value()
        {
            //Act
            int fighter14Con = CombatantBuilder.CalcConBonusToHP(14, "fighter");
            int ranger18Con = CombatantBuilder.CalcConBonusToHP(18, "ranger");
            int paladin17Con = CombatantBuilder.CalcConBonusToHP(17, "paladin");
            int cleric15Con = CombatantBuilder.CalcConBonusToHP(15, "cleric");
            int monk17Con = CombatantBuilder.CalcConBonusToHP(17, "monk");
            int thief5Con = CombatantBuilder.CalcConBonusToHP(5, "thief");

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

        [Test]
        public void CalcNonMonkMeleeDmg_falls_within_range()
        {
            //Arrange
            CombatMethods combatMethods = new();
            List<int> longSwordResultsList = new();
            List<int> dartsResultsList = new();
            List<int> hammerResultsList = new();

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                longSwordResultsList.Add(CombatMethods.CalcNonMonkMeleeDmg("longsword", 17, 0, 0, 1));
                dartsResultsList.Add(CombatMethods.CalcNonMonkMeleeDmg("darts", 12, 0, 0, 0));
                hammerResultsList.Add(CombatMethods.CalcNonMonkMeleeDmg("hammer", 12, 0, 2, 0));
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(longSwordResultsList, Is.All.GreaterThan(2) & Is.All.LessThan(11) & Has.Member(3) & Has.Member(10));
                Assert.That(dartsResultsList, Is.All.GreaterThan(0) & Is.All.LessThan(4) & Has.Member(1) & Has.Member(3));
                Assert.That(hammerResultsList, Is.All.GreaterThan(3) & Is.All.LessThan(8) & Has.Member(4) & Has.Member(7));
            });
        }

        [Test]
        public void CalcMonkMeleeDmg_without_weapon_falls_within_range()
        {
            //Arrange
            CombatMethods combatMethods = new();
            int level = 7;
            string weapon = "none";
            int magicalBonus = 0;
            int otherDmgBonus = 0;
            List<int> resultsList = new();

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                resultsList.Add(CombatMethods.CalcMonkMeleeDmg(level, weapon, magicalBonus, otherDmgBonus));
            }

            //Assert
            Assert.That(resultsList, Is.All.GreaterThan(2) & Is.All.LessThan(10) & Has.Member(3) & Has.Member(9));
        }

        [Test]
        public void CalcMonkMeleeDmg_with_weapon_falls_within_range()
        {
            //Arrange
            CombatMethods combatMethods = new();
            int level = 7;
            string weapon = "dagger";
            int magicalBonus = 0;
            int otherDmgBonus = 0;
            List<int> resultsList = new();

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                resultsList.Add(CombatMethods.CalcMonkMeleeDmg(level, weapon, magicalBonus, otherDmgBonus));
            }

            //Assert
            Assert.That(resultsList, Is.All.GreaterThan(3) & Is.All.LessThan(8) & Has.Member(4) & Has.Member(7));
        }

        [Test]
        public void DoACombatRound_returns_logResults()
        {
            //Arrange
            List<Combatant> testList = new() {testChar, testCharGoodAC, testCharPoorAC };

            //Act
            List<string> logResults = CombatRound.DoACombatRound(testList, false);

            //Assert
            Assert.That(logResults, Is.Not.Null);
        }

        [Test]
        public void Targets_get_set_for_all_combatants()
        {
            //Arrange
            List<Combatant> testList = new() { testChar, testCharGoodAC, testCharPoorAC };

            //Act
            CombatMethods.DetermineTargets(testList, false);

            //Assert
            CollectionAssert.DoesNotContain(testList.Select(x => x.Target), "");
        }

        [Test]
        public void DetermineInits_sets_inits_correctly()
        {
            //Arrange
            List<Combatant> testList = new() { testChar, testCharGoodAC, testCharPoorAC };
            testChar.ActionForThisRound = "melee attack";
            testChar.Weapon = "two-handed sword";
            testChar.InitMod = -1;
            testCharGoodAC.Spells = new List<string>() { "web" };
            testCharPoorAC.Spells = new List<string>() { "hold person" };

            List<int> inits2hsword = new();
            List<int> initsWeb = new();
            List<int> initsHoldPerson = new();
            
            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                CombatMethods.DetermineInits(testList);
                inits2hsword.Add(testList.Where(x => x.Name == "testChar").Select(c => c.Init).ToList()[0]);
                initsWeb.Add(testList.Where(x => x.Name == "testCharGoodAC").Select(c => c.Init).ToList()[0]);
                initsHoldPerson.Add(testList.Where(x => x.Name == "testCharPoorAC").Select(c => c.Init).ToList()[0]);
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(testList, Is.Ordered.By("Init"));
                Assert.That(inits2hsword, Is.All.GreaterThan(9) & Is.All.LessThan(20) & Has.Member(10) & Has.Member(19));
                Assert.That(initsWeb, Is.All.EqualTo(2));
                Assert.That(initsHoldPerson, Is.All.EqualTo(5));
            });
        }

        [Test]
        public void DoAFullCombat_leaves_one_or_zero_remaining()
        {
            //Arrange
            List<Combatant> fullCombatTestList = new()
            {
                new Combatant("testChar1", "fighter", 1, "human", 12, 12, 12, new List<int>() { 1 }, 10, 0),
                new Combatant("testChar2", "fighter", 1, "human", 12, 12, 12, new List<int>() { 1 }, 10, 0),
                new Combatant("testChar3", "fighter", 1, "human", 12, 12, 12, new List<int>() { 1 }, 10, 0)
            };

            //Act
            FullCombat.DoAFullCombat(fullCombatTestList, false);

            //Assert
            Assert.LessOrEqual(fullCombatTestList.Count, 1);
        }

        [Test]
        public void Simultaneous_init_allows_attack_from_dead_combatant()
        {
            //Arrange
            List<Combatant> twoCombatantTestList = new()
            {
                new Combatant("testChar1", "fighter", 1, "human", 12, 12, 12, new List<int>() { 1 }, 1, 0, otherHitBonus: 20),
                new Combatant("testChar2", "fighter", 1, "human", 12, 12, 12, new List<int>() { 1 }, 1, 0, otherHitBonus: 20)
            };

            int init1 = 0;
            int init2 = 0;
            bool simultaneousInit = false;

            //Act
            while (!simultaneousInit)
            {

                CombatRound.DoACombatRound(twoCombatantTestList, false);
                
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
    }
}
