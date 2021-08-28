using NPCConsoleTesting.DB_Connection;
using NPCConsoleTesting.Models;
using System;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatantRetriever
    {
        public ICombatant GetCombatantByName(string connectionString, string charName)
        {
            //string charName = GetNameFromUserInput();

            string query = $"SELECT * FROM npcs WHERE name = '{charName}'";

            //TODO: Add logic to handle unusable input / name not in db / multiple results returned
            var queryResult = DBConnection.QueryDB(connectionString, query);

            //Format results as ICombatant
            CharacterModel model = queryResult.ElementAt(0);
            //TODO: Extract from char info: values for initMod, AC, thac0 and attack dice
            ICombatant combatant = new Fighter(model.Name, model.currentHP, 0, 5, 15, 1, 4, 1);

            return combatant;
            //return new Fighter("testChar1", 10, 0, 10, 1, 1, 4, 1);
        }

        public string GetNameFromUserInput()
        {
            Console.WriteLine($"Enter the character's name.");
            string name = Console.ReadLine();

            return name;
        }
    }
}