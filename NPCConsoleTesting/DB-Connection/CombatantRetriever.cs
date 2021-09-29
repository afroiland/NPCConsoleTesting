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

            string name = queryResult[0].name;
            string charClass = queryResult[0].characterClass;
            int level = queryResult[0].level;
            int str = queryResult[0].str;
            int ex_str = queryResult[0].ex_str;
            int dex = queryResult[0].dex;
            int con = queryResult[0].con;
            List<int> HP_By_Level = queryResult[0].hp_by_lvl.Split(',').Select(int.Parse).ToList();
            int HP = queryResult[0].currentHP;
            //int initMod = 0;
            string armor = queryResult[0].armor;
            string weapon = queryResult[0].weapon;
            bool hasShield = queryResult[0].hasShield;
            List<string> spells = SelectOnlyCombatSpells(queryResult[0].memorized);

            return new Combatant(name, charClass, level, str, dex, con, HP_By_Level, HP, ex_str, charArmor: armor, charWeapon: weapon,
                charHasShield: hasShield, charSpells: spells);
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
