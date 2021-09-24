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

        public static SpellResults DoASpell(string spellName, int casterLevel, int bonus = 0)
        {
            string affectType = DamageSpells.Contains(spellName) ? "damage" : "status";
            string status = "";
            int dmg = 0;

            //TODO: somewhere in here we need to check if a saving throw is called for

            //TODO: add checks for input not in either list
            if (affectType == "damage")
            {
                dmg = GetSpellDamage(spellName, casterLevel) + bonus;
            }
            else
            {
                status = GetSpellStatusEffect(spellName);
            }

            return new SpellResults(affectType, status, dmg);
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

        public static string GetSpellStatusEffect(string spellName)
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

        public static string SelectFromCombatantsSpells(Combatant combatant)
        {
            if (combatant.Spells == null || combatant.Spells.Count < 1)
            {
                return "";
            }

            List<string> cureSpells = combatant.Spells.Where(x => x.Contains("Cure")).ToList();
            List<string> nonCureSpells = combatant.Spells.Where(x => !x.Contains("Cure")).ToList();

            //a combatant at full HP with only cure spells will not cast a spell
            //if (combatant.CurrentHP >= combatant.HP_By_Level.Sum() && nonCureSpells.Count < 1)
            if (combatant.CurrentHP >= (combatant.HP_By_Level.Sum() + CombatMethods.CalcConBonusToHP(combatant.Constitution, combatant.CharacterClass)) && nonCureSpells.Count < 1)
            {
                return "";
            }

            //unless the combatant is at full health, cure spells are prioritized
            if (combatant.CurrentHP < combatant.HP_By_Level.Sum() && cureSpells.Count > 0)
            {
                //select random cure spell
                return cureSpells[_random.Next(0, cureSpells.Count)];
            }

            //select random nonCure spell
            return nonCureSpells[_random.Next(0, nonCureSpells.Count)];
        }
    }
}
