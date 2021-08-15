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
            Combatant result = Build.BuildCombatantRandomly();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GenerateRandomName_returns_name_within_ranges()
        {
            string result = Build.GenerateRandomName();

            Assert.That(result, Is.Not.Null);
        }
    }
}
