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
            //TODO: create multiple combatants and check ranges
            Combatant result = Build.BuildCombatantRandomly();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GenerateRandomName_returns_name_within_ranges()
        {
            //TODO: create multiplt names and check ranges
            string result = Build.GenerateRandomName();

            Assert.That(result, Is.Not.Null);
        }
    }
}
