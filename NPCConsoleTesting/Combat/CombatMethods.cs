using NPCConsoleTesting.Combat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatMethods : ICombatMethods
    {
        static Random _random = new();

        public int DoMeleeAttack(string charClass, int thac0, int str, string armor, int dex, string weapon, int ex_str = 0, int magicalBonus = 0, int otherHitBonus = 0, int otherDmgBonus = 0)
        {
            int result = 0;
            int attackRoll = _random.Next(1, 21);
            int ac = CalcAC(armor, dex);

            int targetNumber = thac0 - ac - magicalBonus - otherHitBonus;
            if (charClass != "Monk")
            {
                targetNumber -= CalcStrBonusToHit(str, ex_str);
            }
            //An attack roll of 20 always suceeds and a roll of 1 always fails
            if (attackRoll == 20 || (targetNumber <= attackRoll && attackRoll != 1))
            {
                result = CalcWeaponDmg(weapon, str, ex_str, magicalBonus, otherDmgBonus);
            }

            return result;
        }

        public static int CalcStrBonusToHit(int str, int ex_str)
        {
            int result = 0;

            if (str >= 17)
            {
                if (0 <= ex_str && ex_str <= 50)
                {
                    result++;
                }
                else if (51 <= ex_str && ex_str <= 99)
                {
                    result += 2;
                }
                else if (ex_str == 100)
                {
                    result += 3;
                }
            }

            return result;
        }

        public static int CalcStrBonusToDmg(int str, int ex_str)
        {
            int result = 0;

            if (15 < str && str < 18)
            {
                result++;
            }

            if (str == 18)
            {
                if (ex_str == 0)
                {
                    result += 2;
                }
                else if (1 <= ex_str && ex_str <= 75)
                {
                    result += 3;
                }
                else if (76 <= ex_str && ex_str <= 90)
                {
                    result += 4;
                }
                else if (91 <= ex_str && ex_str <= 99)
                {
                    result += 5;
                }
                else if (ex_str == 100)
                {
                    result += 6;
                }
            }

            return result;
        }

        public int CalcWeaponDmg(string weapon, int str, int ex_str, int magicalBonus, int otherDmgBonus = 0)
        {
            WeaponInfo weaponInfo = GetWeaponInfo(weapon);
            int result = 0;

            for (int i = 0; i < weaponInfo.NumberOfAttackDice; i++)
            {
                result += _random.Next(1, weaponInfo.TypeOfAttackDie + 1);
            }

            return result + weaponInfo.DmgModifier + CalcStrBonusToDmg(str, ex_str) + magicalBonus + otherDmgBonus;
        }

        public static WeaponInfo GetWeaponInfo(string weapon)
        {
            List<int> results = weapon switch
            {
                "Darts" => new List<int> { 1, 3, 0 },
                "Dagger" => new List<int> { 1, 4, 0 },
                "Hammer" => new List<int> { 1, 4, 1 },
                "Club" or "Flail" or "Mace" or "Shortsword" or "Spear" or "Staff" => new List<int> { 1, 6, 0 },
                "Axe" or "Longsword" => new List<int> { 1, 8, 0 },
                "Halberd" or "Two-Handed Sword" => new List<int> { 1, 10, 0 },
                _ => new List<int> { 1, 3, 0 }
            };

            return new WeaponInfo(results[0], results[1], results[2]);
        }

        public static int CalcAC(string armor, int dex)
        {
            int result = armor switch
            {
                "None" => 10,
                "Shield Only" => 9,
                "Leather" => 8,
                "Leather + Shield" or "Studded Leather" => 7,
                "Studded Leather + Shield" or "Scale Mail" => 6,
                "Scale Mail + Shield" or "Chain Mail" => 5,
                "Chain Mail + Shield" or "Banded Mail" => 4,
                "Banded Mail + Shield" or "Plate Mail" => 3,
                "Plate Mail + Shield" => 2,
                _ => 10
            };

            //AC bonus for dex above 14
            for (int i = 14; i < 18; i++)
            {
                if (dex > i) { result--; }
            }

            return result;
        }

        public List<Combatant> DetermineInit(List<Combatant> chars)
        {
            //set inits
            foreach (Combatant ch in chars)
            {
                ch.Init = _random.Next(1, 11);
                if (ch.Spells != null && ch.Spells.Count > 0)
                {
                    ch.Init += GetCastingTime(ch.Spells[0]);
                }
                else
                {
                    ch.Init += ch.InitMod + GetSpeedFactor(ch.Weapon);
                }
            }

            //order chars with hp > 0 by init
            chars = chars.Where(x => x.CurrentHP > 0).OrderBy(x => x.Init).ToList();

            return chars;
        }

        private static int GetCastingTime(string spellName)
        {
            int result = spellName switch
            {
                "Burning Hands" or "Magic Missile" or "Shocking Grasp" or "Sleep" => 1,
                "Ray of Enfeeblement" or "Web" => 2,
                "Fireball" or "Haste" or "Lightning Bolt" or "Slow" => 3,
                "Cure Light Wounds" or "Hold Person" => 5,
                "Strength" => 10,
                _ => 10
            };

            return result;
        }

        private static int GetSpeedFactor(string weapon)
        {
            int result = weapon switch
            {
                "Dagger" or "Darts" => 2,
                "Shortsword" => 3,
                "Club" or "Hammer" or "Staff" => 4,
                "Longsword" => 5,
                "Flail" or "Mace" => 6,
                "Axe" or "Spear" => 7,
                "Halberd" => 9,
                "Two-Handed Sword" => 10,
                _ => 0
            };

            return result;
        }

        public List<Combatant> DetermineTargets(List<Combatant> chars)
        {
            //set targets if needed
            foreach (Combatant ch in chars)
            {
                if (ch.Target == "" || chars.Where(x => x.Name == ch.Target).Select(x => x.CurrentHP).ToList()[0] <= 0)
                {
                    List<string> potentialTargets = chars.Where(x => ch.Name != x.Name && x.CurrentHP > 0).Select(x => x.Name).ToList();
                    ch.Target = potentialTargets[_random.Next(0, potentialTargets.Count)];
                }
            }
            //show targets
            //for (int i = 0; i < chars.Count; i++)
            //{
            //    if (chars[i].hp > 0)
            //    {
            //        Console.WriteLine($"{chars[i].name} target: {chars[i].target}");
            //    }
            //}
            //if (doReadLines) { Console.ReadLine(); }

            return chars;
        }
    }
}
