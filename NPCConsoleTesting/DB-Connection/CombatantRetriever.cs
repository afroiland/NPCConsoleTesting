using NPCConsoleTesting.DB_Connection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatantRetriever : ICombatantRetriever
    {
        public Combatant GetCombatantByName(string connectionString, string charName)
        {
            string query = $"SELECT * FROM npcs WHERE name = '{charName}'";

            var queryResult = DBConnection.QueryDB(connectionString, query);

            //TODO: add .ToLower() to race, armor, etc.
            string name = queryResult[0].Name;
            string charClass = queryResult[0].CharacterClass;
            int level = queryResult[0].Level;
            string race = queryResult[0].Race;
            int str = queryResult[0].Str;
            int ex_str = queryResult[0].Ex_str;
            int dex = queryResult[0].Dex;
            int con = queryResult[0].Con;
            List<int> HP_By_Level = queryResult[0].HP_by_lvl.Split(',').Select(int.Parse).ToList();
            int HP = queryResult[0].CurrentHP;
            //int initMod = 0;
            string armor = queryResult[0].Armor;
            string weapon = queryResult[0].Weapon;
            bool hasShield = queryResult[0].HasShield;
            List<string> spells = SelectOnlyCombatSpells(queryResult[0].Memorized);

            return new Combatant(name, charClass, level, race, str, dex, con, HP_By_Level, HP, ex_str, charArmor: armor, charWeapon: weapon,
                charHasShield: hasShield, charSpells: spells);
        }

        public static List<string> SelectOnlyCombatSpells(string allSpells)
        {
            List<string> combatSpells = new() { "burning hands", "cure light wounds", "fireball", "flame arrow", "haste", "hold person",
                "lightning bolt", "magic missile", "shocking grasp", "slow", "strength", "ray of enfeeblement", "sleep", "web" };

            List<string> listOfAllSpells = allSpells.Split(", ").ToList();

            //TODO: if at some point everything in the db is standardized to use lowercase, the next line can be removed
            List<string> allSpellsToLower = listOfAllSpells.Select(x => x.ToLower()).ToList();

            return allSpellsToLower.Where(x => combatSpells.Contains(x)).ToList();
        }
    }
}
