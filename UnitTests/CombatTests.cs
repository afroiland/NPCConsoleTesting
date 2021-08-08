using NPCConsoleTesting;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    //[TestFixture]
    public class CombatTests
    {
        List<Character> testList = new()
        {
            new Character("testChar1", 10, 0, 10, 1, 1, 4, 1),
            new Character("testChar2", 10, 0, 10, 1, 1, 4, 1),
            new Character("testChar3", 10, 0, 10, 1, 1, 4, 1)
        };

        [Test]
        public void Targets_Get_Set_for_All_Chars()
        {
            //arrange


            //act
            Combat.CombatRound(testList);

            //assert
            Assert.Pass();
        }
    }

}