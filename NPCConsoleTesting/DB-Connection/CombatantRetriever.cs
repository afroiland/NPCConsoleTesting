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

            //TODO: Extract from char info: values for initMod and attack dice
            string name = queryResult[0].name;
            string charClass = queryResult[0].characterClass;
            int level = queryResult[0].level;
            int str = queryResult[0].str;
            int ex_str = queryResult[0].ex_str;
            int dex = queryResult[0].dex;
            int HP = queryResult[0].currentHP;
            int initMod = 0;
            int AC = CombatantBuilder.CalcAC(queryResult[0].armor, queryResult[0].dex);
            int thac0 = CombatantBuilder.CalcThac0(queryResult[0].characterClass, queryResult[0].level);
            int numberOfAttackDice = 1;
            int typeOfAttackDie = 4;
            int dmgModifier = 1;
            string weapon = queryResult[0].weapon;
            List<string> spells = SelectOnlyCombatSpells(queryResult[0].memorized);

            return new Combatant(name, charClass, level, str, dex, HP, initMod, AC, thac0, numberOfAttackDice, typeOfAttackDie, dmgModifier, ex_str, weapon, spells);
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
