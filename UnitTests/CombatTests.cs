using NPCConsoleTesting;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    //[TestFixture]
    public class CombatTests
    {
        //Arrange
        List<Character> testList = new()
        {
            new Character("testChar1", 10, 0, 10, 1, 1, 4, 1),
            new Character("testChar2", 10, 0, 10, 1, 1, 4, 1),
            new Character("testChar3", 10, 0, 10, 1, 1, 4, 1)
        };

        [Test]
        public void Targets_Get_Set_for_All_Chars()
        {
            //Act
            var result = Combat.CombatRound(testList);

            //Assert
            foreach  (Character ch in result.characters)
            {
                Assert.AreNotEqual(ch.target, "");
            }
        }

        [Test]
        public void Inits_Get_Set_for_All_Chars()
        {
            //Act
            var result = Combat.DetermineInit(testList);

            //Assert
            Assert.That(result, Is.Ordered.By("init"));
        }
    }
}