using NPCConsoleTesting.Combat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatMethods : ICombatMethods
    {
        static Random _random = new();

        //TODO: Lots of refactoring to be done here

        public int DoAMeleeAttack(IAttacker attacker, IDefender defender)
        {
            int result = 0;
            int attackRoll = _random.Next(1, 21);
            int armorClass = defender.CharacterClass != "Monk" ? CalcNonMonkAC(defender.Armor, defender.HasShield, defender.Dexterity, defender.OtherACBonus) :
                CalcMonkAC(defender.Level, defender.OtherACBonus);

            int targetNumber = CalcThac0(attacker.CharacterClass, attacker.Level) - armorClass - attacker.MagicalBonus - attacker.OtherHitBonus;
            if (attacker.CharacterClass != "Monk")
            {
                targetNumber -= CalcStrBonusToHit(attacker.Strength, attacker.Ex_Strength);
            }
            //An attack roll of 20 always suceeds and a roll of 1 always fails
            if (attackRoll == 20 || (targetNumber <= attackRoll && attackRoll != 1))
            {
                result = CalcMeleeDmg(attacker.CharacterClass, attacker.Weapon, attacker.Strength, attacker.Ex_Strength, attacker.MagicalBonus, attacker.OtherDmgBonus);
            }

            return result;
        }

        public static int CalcThac0(string charClass, int level)
        {
            List<int> MUThac0s = new() { 20, 20, 20, 20, 20, 19, 19, 19, 19, 19, 16, 16, 16, 16, 16, 13, 13, 13, 13, 13, 11 };
            List<int> ThiefThac0s = new() { 20, 20, 20, 20, 19, 19, 19, 19, 16, 16, 16, 16, 14, 14, 14, 14, 12, 12, 12, 12, 10 };

            int result = charClass switch
            {
                "Fighter" or "Paladin" or "Ranger" or "Monster" => 21 - level,
                "Magic-User" or "Illusionist" => MUThac0s[level - 1],
                "Cleric" or "Monk" or "Druid" => 20 - (int)(Math.Floor((level - 1) / 3d) * 2),
                "Thief" or "Assassin" => ThiefThac0s[level - 1],
                _ => 20
            };
            return result;
        }

        public static int CalcNonMonkAC(string armor, bool hasShield, int dex, int otherBonus)
        {
            int result = armor switch
            {
                "None" => 10,
                "Leather" => 8,
                "Studded Leather" => 7,
                "Scale" => 6,
                "Chain" => 5,
                "Banded" => 4,
                "Plate" => 3,
                _ => 10
            };

            //lower AC by 1 if combatant has shield
            result -= hasShield ? 1 : 0;

            //AC bonus for dex above 14
            if (dex > 14)
            {
                for (int i = 14; i < 18; i++)
                {
                    if (dex > i) { result--; }
                }
            }

            return result - otherBonus;
        }

        public static int CalcMonkAC(int level, int otherBonus)
        {
            int result = level switch
            {
                1 => 10,
                2 => 9,
                3 => 8,
                4 or 5 => 7,
                6 => 6,
                7 => 5,
                8 => 4,
                9 or 10 => 3,
                11 => 2,
                12 => 1,
                13 => 0,
                14 or 15 => -1,
                16 => -2,
                17 => -3,
                _ => 10
            };

            return result - otherBonus;
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

        public static int CalcConBonusToHP(int con, string charClass)
        {
            int result;

            if (charClass == "Fighter" || charClass == "Ranger" || charClass == "Paladin")
            {
                result = con switch
                {
                    <15 => 0,
                    15 => 1,
                    16 => 2,
                    17 => 3,
                    >17 => 4
                };
            }
            else
            {
                result = con switch
                {
                    < 15 => 0,
                    15 => 1,
                    >15 => 2
                };
            }

            return result;
        }

        public int CalcMeleeDmg(string attackerClass, string weapon, int str, int ex_str, int magicalBonus, int otherDmgBonus)
        {
            WeaponInfo weaponInfo = GetWeaponInfo(weapon);
            int result = 0;

            for (int i = 0; i < weaponInfo.NumberOfAttackDice; i++)
            {
                result += _random.Next(1, weaponInfo.TypeOfAttackDie + 1);
            }

            result += weaponInfo.DmgModifier + magicalBonus + otherDmgBonus;
            if (attackerClass != "Monk")
            {
                result += CalcStrBonusToDmg(str, ex_str);
            }
            
            return result;
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

        public CombatantUpdateResults ApplyMeleeResultToCombatant(Combatant attacker, Combatant target, int attackResult, int segment)
        {
            List<string> entries = new();
            bool opportunityForSimulAttack = false;

            if (attackResult > 0)
            {
                entries.Add($"{attacker.Name} struck {target.Name} for {attackResult} damage.");

                //adjust target hp and GotHitThisRound status
                target.CurrentHP -= attackResult;
                target.GotHitThisRound = true;

                if (target.CurrentHP <= 0)
                {
                    entries.Add($"{target.Name} fell.");

                    if (target.Init == segment)
                    {
                        opportunityForSimulAttack = true;
                    }
                }
                //a sleeping character who gets hit (and survives) wakes up
                else if (target.Statuses.Contains("Asleep"))
                {
                    target.Statuses.RemoveAll(r => r == "Asleep");
                }
            }

            return new CombatantUpdateResults(entries, opportunityForSimulAttack);
        }

        public CombatantUpdateResults ApplySpellResultToCombatant(Combatant caster, Combatant target, string spellName, SpellResults spellResults, int segment)
        {
            List<string> entries = new();
            bool opportunityForSimulAttack = false;

            //unless they have been hit this round, a combatant with a spell will cast it
            if (!caster.GotHitThisRound)
            {
                if (spellResults.AffectType == "damage")
                {
                    if (spellResults.Damage < 0)   //a negative result indicates cure light wounds, which gets applied to caster
                    {
                        caster.CurrentHP -= spellResults.Damage;
                        entries.Add($"{caster.Name} healed themself for {-(spellResults.Damage)} hit points.");
                    }
                    else
                    {

                        target.CurrentHP -= spellResults.Damage;
                        target.GotHitThisRound = true;
                        entries.Add($"{caster.Name} hit {target.Name} with a {spellName} effect for {spellResults.Damage} damage.");
                        if (target.CurrentHP < 1)
                        {
                            entries.Add($"{target.Name} fell.");

                            if (target.Init == segment)
                            {
                                opportunityForSimulAttack = true;
                            }
                        }
                    }
                }

                if (spellResults.AffectType == "status")
                {
                    target.Statuses.Add(spellResults.Status);
                    entries.Add($"{caster.Name} cast {spellName} on {target.Name}. {target.Name} is {spellResults.Status}.");
                }
            }
            else
            {
                entries.Add($"{caster.Name}'s casting of {spellName} was interrupted.");
            }

            return new CombatantUpdateResults(entries, opportunityForSimulAttack);
        }
    }
}
