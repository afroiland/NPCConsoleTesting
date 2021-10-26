using NPCConsoleTesting.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class SpellMethods
    {
        static Random _random = new();

        public static List<string> DamageSpells = new() {
            "Burning Hands",
            "Cure Light Wounds",
            "Fireball",
            "Lightning Bolt",
            "Magic Missile",
            "Shocking Grasp"
        };

        public List<string> StatusSpells = new()
        {
            "Haste",
            "Hold Person",
            "Slow",
            "Strength",
            "Ray of Enfeeblement",
            "Sleep",
            "Web"
        };

        public static ActionResults DoASpell(string spellName, int casterLevel, int bonus = 0)
        {
            string effectType = DamageSpells.Contains(spellName) ? "Damage" : "Status";
            Status status = new("", 0);
            int dmg = 0;

            //TODO: add checks for input not in either list
            if (effectType == "Damage")
            {
                dmg = GetSpellDamage(spellName, casterLevel) + bonus;
            }
            else
            {
                status.Name = GetStatusName(spellName);
                status.Duration = GetStatusDuration(spellName, casterLevel);
            }

            string savingThrowType = GetSavingThrowType(spellName);

            return new ActionResults(dmg, spellName, effectType, savingThrowType, status);
        }

        public static int GetSpellDamage(string spellName, int casterLevel)
        {
            int result = spellName switch
            {
                //TODO: calc damage by level where needed
                "Burning Hands" => casterLevel,
                "Cure Light Wounds" => -(_random.Next(1, 9)),
                "Fireball" or "Lightning Bolt" => 18,                
                "Magic Missile" => 5,
                "Shocking Grasp" => _random.Next(1, 9) + casterLevel,
                _ => 0
            };

            return result;
        }

        public static string GetStatusName(string spellName)
        {
            string result = spellName switch
            {
                "Haste" => "Hasted",
                "Hold Person" or "Web" => "Held",
                "Slow" => "Slowed",
                "Strength" => "Strong",
                "Ray of Enfeeblement" => "Weakened",
                "Sleep" => "Asleep",
                _ => ""
            };

            return result;
        }

        public static int GetStatusDuration(string spellName, int casterLevel)
        {
            int result = spellName switch
            {
                "Haste" => 3 + casterLevel,
                "Hold Person" => 4 + casterLevel,
                "Slow" => 3 + casterLevel,
                "Strength" => 60 * casterLevel,
                "Ray of Enfeeblement" => casterLevel,
                "Sleep" => 5 * casterLevel,
                _ => 0
            };

            return result;
        }

        public static string GetSavingThrowType(string spellName)
        {
            return spellName switch
            {
                //"Burning Hands" => "",
                //"Cure Light Wounds",
                //"Magic Missile",
                //"Shocking Grasp",
                //"Haste",
                //"Slow",
                //"Strength",
                //"Sleep",
                "Fireball" or "Lightning Bolt" => "Half",
                "Hold Person" or "Ray of Enfeeblement" or "Web" => "Negate",
                _ => "None"
            };
        }

        public static string SelectFromCombatantsSpells(Combatant combatant)
        {
            if (combatant.Spells == null || combatant.Spells.Count < 1)
            {
                return "";
            }

            List<string> cureSpells = combatant.Spells.Where(x => x.Contains("Cure")).ToList();
            List<string> nonCureSpells = combatant.Spells.Where(x => !x.Contains("Cure")).ToList();

            //a combatant at full HP with only cure spells will not cast a spell
            if (combatant.CurrentHP >= CombatantBuilder.CalcMaxHP(combatant.HP_By_Level, combatant.Constitution, combatant.CharacterClass) &&
                nonCureSpells.Count < 1)
            {
                return "";
            }

            //unless the combatant is at full health, cure spells are prioritized
            if (combatant.CurrentHP < CombatantBuilder.CalcMaxHP(combatant.HP_By_Level, combatant.Constitution, combatant.CharacterClass) &&
                cureSpells.Count > 0)
            {
                //select random cure spell
                return cureSpells[_random.Next(0, cureSpells.Count)];
            }

            //select random nonCure spell
            return nonCureSpells[_random.Next(0, nonCureSpells.Count)];
        }
    }
}
