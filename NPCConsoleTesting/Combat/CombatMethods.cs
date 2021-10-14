using NPCConsoleTesting.Characters;
using NPCConsoleTesting.Combat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCConsoleTesting
{
    public class CombatMethods : ICombatMethods
    {
        static Random _random = new();

        public ActionResults DoAMeleeAttack(IAttacker attacker, IDefender defender)
        {
            ActionResults result = new(0);
            int attackRoll = _random.Next(1, 21);

            //calculate defender's armor class
            int armorClass = defender.CharacterClass != "Monk" ? CalcNonMonkAC(defender.Armor, defender.HasShield, defender.Dexterity, defender.OtherACBonus) :
                CalcMonkAC(defender.Level, defender.OtherACBonus);

            //calculate number needed for successful attack roll
            int targetNumber = CalcThac0(attacker.CharacterClass, attacker.Level) - armorClass - attacker.MagicalBonus - attacker.OtherHitBonus;
            if (attacker.CharacterClass != "Monk")
            {
                targetNumber -= CalcStrBonusToHit(attacker.Strength, attacker.Ex_Strength);
            }

            //an attack roll of 20 always succeeds and a roll of 1 always fails
            if (attackRoll == 20 || (attackRoll >= targetNumber && attackRoll != 1))
            {
                result.Damage = attacker.CharacterClass == "Monk" ?
                    CalcMonkMeleeDmg(attacker.Level, attacker.Weapon, attacker.MagicalBonus, attacker.OtherDmgBonus) :
                    CalcNonMonkMeleeDmg(attacker.Weapon, attacker.Strength, attacker.Ex_Strength, attacker.MagicalBonus, attacker.OtherDmgBonus);
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
                result = ex_str switch
                {
                    < 51 => 1,
                    < 100 => 2,
                    100 => 3,
                    _ => 0
                };
            }

            return result;
        }

        public static int CalcStrBonusToDmg(int str, int ex_str)
        {
            int result = 0;

            if (15 < str && str < 18)
            {
                result = 1;
            }

            if (str == 18)
            {
                int tempResult = ex_str switch
                {
                    0 => 2,
                    <76 => 3,
                    <91 => 4,
                    <100 => 5,
                    100 => 6,
                    _ => 2
                };

                result = tempResult;
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

        public int CalcMonkMeleeDmg(int level, string weapon, int magicalDmgBonus, int otherDmgBonus)
        {
            int result;

            if (weapon == "None")
            {
                result = CalcMonkOpenHandDmg(level);
            }
            else
            {
                WeaponInfo weaponInfo = GetWeaponInfo(weapon);
                result = CalcWeaponDmg(weaponInfo);

                //add monk weapon damage bonus
                result += level / 2;
            }

            result += magicalDmgBonus + otherDmgBonus;

            return result;
        }

        private static int CalcMonkOpenHandDmg(int level)
        {
            WeaponInfo results = level switch
            {
                1 => new(1, 3, 0),
                2 => new(1, 4, 0),
                3 or 4 => new(1, 6, 0),
                5 => new(1, 6, 1),
                6 => new(2, 4, 0),
                7 => new(2, 4, 1),
                8 => new(2, 6, 0),
                9 => new(3, 4, 0),
                10 => new(2, 6, 1),
                11 => new(3, 4, 1),
                12 => new(4, 4, 0),
                13 => new(4, 4, 1),
                14 => new(5, 4, 0),
                15 => new(6, 4, 0),
                16 => new(5, 6, 0),
                17 => new(8, 4, 0),
                _ => new(1, 3, 0)
            };

            return CalcWeaponDmg(results);
        }

        public int CalcNonMonkMeleeDmg(string weapon, int str, int ex_str, int magicalDmgBonus, int otherDmgBonus)
        {
            WeaponInfo weaponInfo = GetWeaponInfo(weapon);

            return CalcWeaponDmg(weaponInfo) + CalcStrBonusToDmg(str, ex_str) + magicalDmgBonus + otherDmgBonus;
        }

        private static int CalcWeaponDmg(WeaponInfo weaponInfo)
        {
            int result = 0;

            for (int i = 0; i < weaponInfo.NumberOfAttackDice; i++)
            {
                result += _random.Next(1, weaponInfo.TypeOfAttackDie + 1);
            }

            return result + weaponInfo.DmgModifier;
        }

        private static WeaponInfo GetWeaponInfo(string weapon)
        {
            WeaponInfo results = weapon switch
            {
                "Darts" => new(1, 3, 0),
                "Dagger" => new(1, 4, 0),
                "Hammer" => new(1, 4, 1),
                "Club" or "Flail" or "Mace" or "Shortsword" or "Spear" or "Staff" => new(1, 6, 0),
                "Axe" or "Longsword" => new(1, 8, 0 ),
                "Halberd" or "Two-Handed Sword" => new(1, 10, 0),
                _ => new(1, 3, 0 )
            };

            return results;
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

        public void IncrementStatuses(List<Combatant> combatants, List<string> log)
        {
            foreach (Combatant x in combatants)
            {
                foreach (Status y in x.Statuses)
                {
                    y.Duration--;
                    if (y.Duration < 1)
                    {
                        log.Add($"{x.Name} is no longer {y.Name}.");
                    }
                }

                //when the duration of a status reaches zero, that status is removed
                x.Statuses.RemoveAll(z => z.Duration < 1);
            }
        }

        public void DetermineActions(List<Combatant> combatants)
        {
            foreach (Combatant ch in combatants)
            {
                string spellName = SpellMethods.SelectFromCombatantsSpells(ch);
                ch.ActionForThisRound = spellName == "" ? "Melee Attack" : spellName;
            }
        }

        public void DetermineTargets(List<Combatant> combatants)
        {
            //set targets if needed
            foreach (Combatant ch in combatants)
            {
                if (ch.Target == "" || combatants.Where(x => x.Name == ch.Target).Count() == 0)
                {
                    DetermineTargetForOneCombatant(combatants, ch);
                }
            }
        }

        public void DetermineTargetForOneCombatant(List<Combatant> combatants, Combatant priorityC)
        {
            List<string> potentialTargets = combatants.Where(x => priorityC.Name != x.Name && x.CurrentHP > 0).Select(x => x.Name).ToList();
            priorityC.Target = potentialTargets[_random.Next(0, potentialTargets.Count)];
        }

        public void DetermineInits(List<Combatant> combatants)
        {
            //set inits
            foreach (Combatant ch in combatants)
            {
                if(ch.ActionForThisRound != "Melee Attack")
                {
                    //ch.Init = _random.Next(1, 11) + GetCastingTime(ch.Spells[0]) + ch.InitMod;
                    ch.Init = GetCastingTime(ch.Spells[0]) + ch.InitMod;
                }
                else
                {
                    ch.Init = _random.Next(1, 11) + GetSpeedFactor(ch.Weapon) + ch.InitMod;
                }
            }

            //order combatants by init
            combatants.Sort((p, q) => p.Init.CompareTo(q.Init));
        }

        public CombatantUpdateResults ApplyActionResultToCombatant(Combatant targeter, Combatant target, ActionResults results, int segment)
        {
            List<string> entries = new();
            bool opportunityForSimulAttack = false;

            if (results.SpellName != null)
            {
                if (targeter.GotHitThisRound)
                {
                    entries.Add($"{targeter.Name}'s casting of {results.SpellName} was interrupted.");
                    return new CombatantUpdateResults(entries, opportunityForSimulAttack);
                }

                if (results.SpellAffectType == "Status")
                {
                    target.Statuses.Add(results.Status);
                    entries.Add($"{targeter.Name} cast {results.SpellName} on {target.Name}. {target.Name} is {results.Status.Name} for {results.Status.Duration} rounds.");
                }

                if (results.Damage < 0)   //a negative result indicates a healing spell, which gets applied to caster
                {
                    targeter.CurrentHP -= results.Damage;
                    entries.Add($"{targeter.Name} healed themselves for {-(results.Damage)} hit points.");
                }
            }

            if (results.Damage > 0)
            {
                opportunityForSimulAttack = ApplyDamageToCombatant(targeter, target, results.Damage, entries, segment, opportunityForSimulAttack);
            }

            return new CombatantUpdateResults(entries, opportunityForSimulAttack);
        }

        public bool ApplyDamageToCombatant(Combatant targeter, Combatant target, int damage, List<string> entries, int segment, bool opportunityForSimulAttack)
        {
            //adjust target hp and GotHitThisRound status
            target.CurrentHP -= damage;
            target.GotHitThisRound = true;
            entries.Add($"{targeter.Name} struck {target.Name} for {damage} damage.");

            if (target.CurrentHP < 1)
            {
                entries.Add($"{target.Name} fell.");

                if (target.Init == segment)
                {
                    opportunityForSimulAttack = true;
                }
            }

            //a sleeping character who takes damage (and survives) wakes up
            if (target.Statuses.FindIndex(x => x.Name == "Asleep") >= 0)
            {
                entries.Add($"{target.Name} is no longer asleep.");
                target.Statuses.RemoveAll(r => r.Name == "Asleep");
            }

            return opportunityForSimulAttack;
        }
    }
}
