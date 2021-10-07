using NUnit.Framework;
using NPCConsoleTesting;
using System.Collections.Generic;

namespace UnitTests
{
    class SpellTests
    {
        //Arrange
        const int TIMES_TO_LOOP_FOR_RANDOM_TESTS = 50;

        string damageSpellName = "Fireball";
        string statusSpellName = "Sleep";
        int casterLevel = 5;

        [Test]
        public void DoASpell_returns_SpellResults_for_damage_spell()
        {
            //Act
            var results = SpellMethods.DoASpell(damageSpellName, casterLevel);

            //Assert
            Assert.That(results, Is.InstanceOf<ActionResults>());
        }

        [Test]
        public void DoASpell_returns_SpellResults_for_status_spell()
        {
            //Act
            var results = SpellMethods.DoASpell(statusSpellName, casterLevel);

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
            Assert.That(results, Is.EqualTo("Asleep"));
        }

    }
}
