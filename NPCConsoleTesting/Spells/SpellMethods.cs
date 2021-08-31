using System;
using System.Collections.Generic;

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
            int result = 0;

            switch(spellName)
            {
                case "Burning Hands":
                    result = casterLevel;
                    break;
                case "Cure Light Wounds":
                    result = -(_random.Next(1, 9));
                    break;
                case "Fireball":
                    for (int i = 0; i < casterLevel; i++)
                    {
                        result += _random.Next(1, 7);
                    }
                    break;
                case "Lightning Bolt":
                    for (int i = 0; i < casterLevel; i++)
                    {
                        result += _random.Next(1, 7);
                    }
                    break;
                case "Magic Missile":
                    for (int i = 0; i < Math.Floor(casterLevel/3d)+1; i++)
                    {
                        result += _random.Next(1, 5) + 1;
                    }
                    break;
                case "Shocking Grasp":
                    result = _random.Next(1, 9) + casterLevel;
                    break;
            }

            return result;
        }

        public static string GetSpellStatusEffect(string spellName)
        {
            string result = "";

            switch(spellName)
            {
                case "Haste":
                    result = "Hasted";
                    break;
                case "Hold Person":
                case "Web":
                    result = "Held";
                    break;
                case "Slow":
                    result = "Slowed";
                    break;
                //case "Strength":
                //    result = "";
                //    break;
                //case "Ray of Enfeeblement":
                //    result = "";
                //    break;
                case "Sleep":
                    result = "Asleep";
                    break;
                //case "Web":
                //    result = "Held";
                //    break;
            }

            return result;
        }
    }
}
