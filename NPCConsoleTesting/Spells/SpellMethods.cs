using NPCConsoleTesting.Characters;
using NPCConsoleTesting.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting.Spells
{
    public class SpellMethods : ISpellMethods
    {
        private static List<string> damageSpells = new()
        {
            "burning hands",
            "cure light wounds",
            "fireball",
            "lightning bolt",
            "magic missile",
            "shocking grasp"
        };

        private static List<string> statusSpells = new()
        {
            "haste",
            "hold person",
            "slow",
            "strength",
            "ray of enfeeblement",
            "sleep",
            "web"
        };

        static Random _random = new();

        ICombatantBuilder _combatantBuilder;

        public SpellMethods(ICombatantBuilder combatantBuilder)
        {
            _combatantBuilder = combatantBuilder;
        }

        public ActionResults DoASpell(string spellName, int casterLevel, int bonus = 0)
        {
            //TODO: convert to switch to handle spells other than damage and status
            string effectType = damageSpells.Contains(spellName) ? "damage" : "status";
            Status status = new("", 0);
            int dmg = 0;

            if (effectType == "damage")
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
            return spellName switch
            {
                "burning hands" => casterLevel,
                "cure light wounds" => -(_random.Next(1, 9)),
                "fireball" or "lightning bolt" => SharedMethods.CalcMultipleDice(new RangeViaDice(casterLevel, 6, 0)),
                "magic missile" => CalcMagicMissileDamage(casterLevel),
                "shocking grasp" => _random.Next(1, 9) + casterLevel,
                _ => 0
            };
        }

        private static int CalcMagicMissileDamage(int level)
        {
            int damage = 0;
            int numberOfMissiles = (int)(Math.Floor(level / 2.1d) + 1);

            for (int i = 0; i < numberOfMissiles; i++)
            {
                damage += _random.Next(1, 5) + 1;
            }

            return damage;
        }

        public static string GetStatusName(string spellName)
        {
            return spellName switch
            {
                "haste" => "hasted",
                "hold person" or "web" => "held",
                "slow" => "slowed",
                "strength" => "strengthened",
                "ray of enfeeblement" => "weakened",
                "sleep" => "asleep",
                _ => ""
            };
        }

        public static int GetStatusDuration(string spellName, int casterLevel)
        {
            return spellName switch
            {
                "haste" => 3 + casterLevel,
                "hold person" => 4 + casterLevel,
                "slow" => 3 + casterLevel,
                "strength" => 60 * casterLevel,
                "ray of enfeeblement" => casterLevel,
                "sleep" => 5 * casterLevel,
                _ => 0
            };
        }

        public static string GetSavingThrowType(string spellName)
        {
            return spellName switch
            {
                "fireball" or "lightning bolt" => "half",
                "hold person" or "ray of enfeeblement" or "web" => "negate",
                _ => "none"
            };
        }

        public string SelectFromCombatantsSpells(Combatant combatant)
        {
            if (combatant.Spells == null || combatant.Spells.Count < 1)
            {
                return "";
            }

            List<string> cureSpells = combatant.Spells.Where(x => x.Contains("cure")).ToList();
            List<string> nonCureSpells = combatant.Spells.Where(x => !x.Contains("cure")).ToList();

            //a combatant at full HP with only cure spells will not cast a spell
            if (combatant.CurrentHP >= _combatantBuilder.CalcFullHP(combatant.HP_By_Level, combatant.Constitution, combatant.CharacterClass) &&
                nonCureSpells.Count < 1)
            {
                return "";
            }

            //unless the combatant is at full health, cure spells are prioritized
            if (combatant.CurrentHP < _combatantBuilder.CalcFullHP(combatant.HP_By_Level, combatant.Constitution, combatant.CharacterClass) &&
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
