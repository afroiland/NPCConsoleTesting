using NUnit.Framework;
using NPCConsoleTesting;
using System.Collections.Generic;
using NPCConsoleTesting.Spells;

namespace UnitTests
{
    class SpellTests
    {
        //Arrange
        const int TIMES_TO_LOOP_FOR_RANDOM_TESTS = 50;

        Combatant testClericFullHP = new("testClericFullHP", "cleric", 10, "human", 12, 12, 12, new List<int>() { 8, 4, 4 }, 16);
        Combatant testClericDamaged = new("testClericDamaged", "cleric", 10, "human", 12, 12, 12, new List<int>() { 8, 4, 4 }, 10);

        string damageSpellName = "fireball";
        string statusSpellName = "sleep";
        int casterLevel = 5;

        [Test]
        public void DoASpell_returns_SpellResults_for_damage_spell()
        {
            //Arrange
            //TODO: clean up
            CombatantRetriever combatantRetriever = new();
            CombatantBuilder combatantBuilder = new(combatantRetriever);
            SpellMethods spellMethods = new(combatantBuilder);

            //Act
            var results = spellMethods.DoASpell(damageSpellName, casterLevel);

            //Assert
            Assert.That(results, Is.InstanceOf<ActionResults>());
        }

        [Test]
        public void DoASpell_returns_SpellResults_for_status_spell()
        {
            //Arrange
            //TODO: clean up
            CombatantRetriever combatantRetriever = new();
            CombatantBuilder combatantBuilder = new(combatantRetriever);
            SpellMethods spellMethods = new(combatantBuilder);

            //Act
            var results = spellMethods.DoASpell(statusSpellName, casterLevel);

            //Assert
            Assert.That(results, Is.InstanceOf<ActionResults>());
        }

        [Test]
        public void GetSpellDamage_returns_value_within_range()
        {
            //Arrange
            List<int> resultsList = new();

            //Act
            for (int i = 0; i < TIMES_TO_LOOP_FOR_RANDOM_TESTS; i++)
            {
                resultsList.Add(SpellMethods.GetSpellDamage(damageSpellName, casterLevel));
            }

            //Assert
            Assert.That(resultsList, Is.All.GreaterThan(4) & Is.All.LessThan(31));
        }

        [Test]
        public void GetStatusName_returns_correct_value()
        {
            //Act
            var results = SpellMethods.GetStatusName(statusSpellName);

            //Assert
            Assert.That(results, Is.EqualTo("asleep"));
        }

        [Test]
        public void SelectFromCombatantsSpells_returns_spell_as_expected()
        {
            //Arrange
            testClericFullHP.Spells = new List<string>() { "cure light wounds", "hold person" };
            testClericDamaged.Spells = new List<string>() { "cure light wounds", "hold person" };
            //TODO: clean up
            CombatantRetriever combatantRetriever = new();
            CombatantBuilder combatantBuilder = new(combatantRetriever);
            SpellMethods spellMethods = new(combatantBuilder);

            //Act
            string resultFullHP = spellMethods.SelectFromCombatantsSpells(testClericFullHP);
            string resultDamaged = spellMethods.SelectFromCombatantsSpells(testClericDamaged);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultFullHP, Is.EqualTo("hold person"));
                Assert.That(resultDamaged, Is.EqualTo("cure light wounds"));
            });
        }

    }
}
