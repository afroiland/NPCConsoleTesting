using NPCConsoleTesting.DB_Connection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatantRetriever
    {
        public static Combatant GetCombatantByName(string connectionString, string charName)
        {
            string query = $"SELECT * FROM npcs WHERE name = '{charName}'";

            var queryResult = DBConnection.QueryDB(connectionString, query);

            //TODO: Extract from char info: values for initMod, AC, thac0 and attack dice
            Combatant combatant = new(queryResult[0].name, queryResult[0].currentHP, 0, 5, 15, 1, 4, 1, queryResult[0].level, SelectOnlyCombatSpells(queryResult[0].memorized));

            return combatant;
            //return new Combatant("testChar1", 10, 0, 10, 1, 1, 4, 1);
        }

        public static string GetNameFromUserInput()
        {
            Console.WriteLine($"Enter the character's name.");
            return Console.ReadLine();
        }

        public static List<string> SelectOnlyCombatSpells(string allSpells)
        {
            List<string> combatSpells = new() {"Burning Hands", "Cure Light Wounds", "Fireball", "Flame Arrow", "Haste", "Hold Person",
                "Lightning Bolt", "Magic Missile", "Shocking Grasp", "Slow", "Strength", "Ray of Enfeeblement", "Sleep", "Web"};

            List<string> listOfAllSpells = allSpells.Split(", ").ToList();

            return listOfAllSpells.Where(x => combatSpells.Contains(x)).ToList();
        }
    }
}
