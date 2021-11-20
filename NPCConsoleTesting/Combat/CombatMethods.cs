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

            //a held or sleeping combatant can be hit automatically for max damage
            if (defender.Statuses.Any(s => s.Name == "asleep") || defender.Statuses.Any(s => s.Name == "held"))
            {
                RangeViaDice range = GetRangeViaDice(attacker.Weapon);
                int damage = range.NumberOfDice * range.TypeOfDie + range.Modifier;
                result.Damage = damage + CalcStrBonusToDmg(attacker.Strength, attacker.Ex_Strength) + attacker.MagicalBonus + attacker.OtherDmgBonus;

                return result;
            }

            int attackRoll = _random.Next(1, 21);

            //calculate defender's armor class
            int armorClass = defender.CharacterClass != "monk" ? CalcNonMonkAC(defender.Armor, defender.HasShield, defender.Dexterity, defender.OtherACBonus) :
                CalcMonkAC(defender.Level, defender.OtherACBonus);

            //calculate number needed for successful attack roll
            int targetNumber = CalcThac0(attacker.CharacterClass, attacker.Level) - armorClass - attacker.MagicalBonus - attacker.OtherHitBonus
                - CalcWeaponVsArmorAdjustment(attacker.Weapon, defender.Armor, defender.HasShield);
            if (attacker.CharacterClass != "monk")
            {
                targetNumber -= CalcStrBonusToHit(attacker.Strength, attacker.Ex_Strength);
            }

            //an attack roll of 20 always succeeds and a roll of 1 always fails
            if (attackRoll == 20 || (attackRoll >= targetNumber && attackRoll != 1))
            {
                result.Damage = attacker.CharacterClass == "monk" ?
                    CalcMonkMeleeDmg(attacker.Level, attacker.Weapon, attacker.MagicalBonus, attacker.OtherDmgBonus) :
                    CalcNonMonkMeleeDmg(attacker.Weapon, attacker.Strength, attacker.Ex_Strength, attacker.MagicalBonus, attacker.OtherDmgBonus);
            }

            return result;
        }

        public static int CalcThac0(string charClass, int level)
        {
            List<int> muThac0s = new() { 20, 20, 20, 20, 20, 19, 19, 19, 19, 19, 16, 16, 16, 16, 16, 13, 13, 13, 13, 13, 11 };
            List<int> thiefThac0s = new() { 20, 20, 20, 20, 19, 19, 19, 19, 16, 16, 16, 16, 14, 14, 14, 14, 12, 12, 12, 12, 10 };

            return charClass switch
            {
                "fighter" or "paladin" or "ranger" or "monster" => 21 - level,
                "magic-user" or "illusionist" => muThac0s[level - 1],
                "cleric" or "monk" or "druid" => 20 - (int)(Math.Floor((level - 1) / 3d) * 2),
                "thief" or "assassin" => thiefThac0s[level - 1],
                _ => 20
            };
        }

        public static int CalcNonMonkAC(string armor, bool hasShield, int dex, int otherBonus)
        {
            int result = armor switch
            {
                "none" => 10,
                "leather" => 8,
                "studded leather" => 7,
                "scale" => 6,
                "chain" => 5,
                "banded" => 4,
                "plate" => 3,
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

        public int CalcMonkMeleeDmg(int level, string weapon, int magicalDmgBonus, int otherDmgBonus)
        {
            int result;

            if (weapon == "none")
            {
                result = CalcMonkOpenHandDmg(level);
            }
            else
            {
                RangeViaDice rangeViaDice = GetRangeViaDice(weapon);
                result = CalcMultipleDice(rangeViaDice);

                //monk weapon damage bonus
                result += level / 2;
            }

            return result += magicalDmgBonus + otherDmgBonus;
        }

        private static int CalcMonkOpenHandDmg(int level)
        {
            RangeViaDice results = level switch
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

            return CalcMultipleDice(results);
        }

        public int CalcNonMonkMeleeDmg(string weapon, int str, int ex_str, int magicalDmgBonus, int otherDmgBonus)
        {
            RangeViaDice rangeViaDice = GetRangeViaDice(weapon);

            return CalcMultipleDice(rangeViaDice) + CalcStrBonusToDmg(str, ex_str) + magicalDmgBonus + otherDmgBonus;
        }

        public static int CalcMultipleDice(RangeViaDice rangeViaDice)
        {
            int result = 0;

            for (int i = 0; i < rangeViaDice.NumberOfDice; i++)
            {
                result += _random.Next(1, rangeViaDice.TypeOfDie + 1);
            }

            return result + rangeViaDice.Modifier;
        }

        private static int CalcWeaponVsArmorAdjustment(string weapon, string armor, bool hasShield)
        {
            return CalcNonMonkAC(armor, hasShield, 0, 0) switch
            {
                10 =>
                    weapon switch
                    {
                        "club" or "darts" or "staff" => 1,
                        "axe" or "longsword" or "shortsword" => 2,
                        "dagger" => 3,
                        _ => 0
                    },
                9 =>
                    weapon switch
                    {
                        "axe" or "dagger" or "flail" or "halberd" or "longsword" or "staff" or "two-handed sword" => 1,
                        _ => 0
                    },
                8 =>
                    weapon switch
                    {
                        "axe" or "dagger" or "darts" or "flail" or "halberd" or "shortsword" or "staff" => 1,
                        "two-handed sword" => 3,
                        _ => 0
                    },
                7 =>
                    weapon switch
                    {
                        "club" => -1,
                        "flail" => 1,
                        "halberd" => 2,
                        "two-handed sword" => 3,
                        _ => 0
                    },
                6 =>
                    weapon switch
                    {
                        "club" or "darts" => -1,
                        "halberd" => 2,
                        "two-handed sword" => 3,
                        _ => 0
                    },
                5 =>
                    weapon switch
                    {
                        "club" or "dagger" or "darts" => -2,
                        "axe" or "spear" or "staff" => -1,
                        "hammer" => 1,
                        "halberd" or "two-handed sword" => 2,
                        _ => 0
                    },
                4 =>
                    weapon switch
                    {

                        "club" or "darts" or "staff" => -3,
                        "dagger" => -2,
                        "axe" or "shortsword" or "spear" => -1,
                        "halberd" => 1,
                        "two-handed sword" => 2,
                        _ => 0
                    },
                3 =>
                    weapon switch
                    {
                        "staff" => -5,
                        "club" or "darts" => -4,
                        "dagger" => -3,
                        "axe" or "shortsword" => -2,
                        "longsword" or "spear" => -1,
                        "halberd" or "hammer" or "mace" => 1,
                        "two-handed sword" => 2,
                        _ => 0
                    },
                2 =>
                    weapon switch
                    {
                        "staff" => -7,
                        "club" or "darts" => -5,
                        "axe" or "dagger" or "shortsword" => -3,
                        "longsword" or "spear" => -2,
                        "halberd" or "mace" => 1,
                        "two-handed sword" => 2,
                        _ => 0
                    },
                _ => 0
            };
        }

        private static RangeViaDice GetRangeViaDice(string weapon)
        {
            RangeViaDice results = weapon switch
            {
                "darts" => new(1, 3, 0),
                "dagger" => new(1, 4, 0),
                "flail" or "hammer" => new(1, 4, 1),
                "club" or "mace" or "shortsword" or "spear" or "staff" => new(1, 6, 0),
                "axe" or "longsword" => new(1, 8, 0 ),
                "halberd" or "two-handed sword" => new(1, 10, 0),
                _ => new(1, 3, 0 )
            };

            return results;
        }

        private static int GetCastingTime(string spellName)
        {
            return spellName switch
            {
                "burning hands" or "magic missile" or "shocking grasp" or "sleep" => 1,
                "ray of enfeeblement" or "web" => 2,
                "fireball" or "haste" or "lightning bolt" or "slow" => 3,
                "cure light wounds" or "hold person" => 5,
                "strength" => 10,
                _ => 10
            };
        }

        private static int GetSpeedFactor(string weapon)
        {
            return weapon switch
            {
                "dagger" or "darts" => 2,
                "shortsword" => 3,
                "club" or "hammer" or "staff" => 4,
                "longsword" => 5,
                "flail" or "mace" => 6,
                "axe" or "spear" => 7,
                "halberd" => 9,
                "two-handed sword" => 10,
                _ => 0
            };
        }

        public void IncrementStatuses(List<Combatant> combatants, List<string> log)
        {
            foreach (Combatant x in combatants)
            {
                //TODO: possible check here for haste and slow effets canceling each other out

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
                ch.ActionForThisRound = spellName == "" ? "melee attack" : spellName;
            }
        }

        public void DetermineTargets(List<Combatant> combatants, bool isTeamBattle)
        {
            foreach (Combatant ch in combatants)
            {
                if (ch.Target == "" || !combatants.Any(x => x.Name == ch.Target))
                {
                    DetermineTargetForOneCombatant(combatants, ch, isTeamBattle);
                }
            }
        }

        public void DetermineTargetForOneCombatant(List<Combatant> combatants, Combatant priorityC, bool isTeamBattle)
        {
            List<string> potentialTargets = new();
            if (isTeamBattle)
            {
                potentialTargets = combatants.Where(x => priorityC.Name != x.Name && x.CurrentHP > 0 && x.Affiliation != priorityC.Affiliation)
                .Select(x => x.Name).ToList();
            }
            else
            {
                potentialTargets = combatants.Where(x => priorityC.Name != x.Name && x.CurrentHP > 0).Select(x => x.Name).ToList();
            }

            priorityC.Target = potentialTargets[_random.Next(0, potentialTargets.Count)];
        }

        public void DetermineInits(List<Combatant> combatants)
        {
            foreach (Combatant ch in combatants)
            {
                if(ch.ActionForThisRound != "melee attack")
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

            //handle spell actions
            if (results.SpellName != null)
            {
                //if a combatant whose round action is a spell takes damage before their turn, they take no action this round 
                if (targeter.GotHitThisRound)
                {
                    entries.Add($"{targeter.Name}'s casting of {results.SpellName} was interrupted.");
                    return new CombatantUpdateResults(entries, opportunityForSimulAttack);
                }

                //TODO: fix / refactor this
                if (results.SpellEffectType == "status")
                {
                    if (results.SpellSavingThrowType == "negate")
                    {
                        if (DoASavingThrow(target) == "failure")
                        {
                            target.Statuses.Add(results.Status);
                            entries.Add($"{targeter.Name} cast {results.SpellName} on {target.Name}. {target.Name} is {results.Status.Name} for {results.Status.Duration} rounds.");
                        }
                        else if (DoASavingThrow(target) == "success")
                        {
                            entries.Add($"{targeter.Name} attempted to cast {results.SpellName} on {target.Name}, but {target.Name}'s saving throw was successful.");
                        }
                    }
                    else
                    {
                        target.Statuses.Add(results.Status);
                        entries.Add($"{targeter.Name} cast {results.SpellName} on {target.Name}. {target.Name} is {results.Status.Name} for {results.Status.Duration} rounds.");
                    }
                }

                if (results.Damage < 0)   //a negative result indicates a healing spell, which gets applied to caster
                {
                    targeter.CurrentHP -= results.Damage;
                    entries.Add($"{targeter.Name} healed themselves for {-(results.Damage)} hit points.");
                }
            }

            //apply damage from spell or melee attack
            if (results.Damage > 0)
            {
                if (results.SpellSavingThrowType == "half")
                {
                    if (DoASavingThrow(target) == "success")
                    {
                        entries.Add($"{target.Name}'s saving throw was successful; damage reduced by half.");
                        results.Damage /= 2;
                    }
                }

                opportunityForSimulAttack = ApplyDamageToCombatant(targeter, target, results.SpellName, results.Damage, entries, segment, opportunityForSimulAttack);
            }

            return new CombatantUpdateResults(entries, opportunityForSimulAttack);
        }

        private string DoASavingThrow(Combatant target)
        {
            int targetNumber = GetSavingThrowTargetNumber(target);

            return _random.Next(1, 21) < targetNumber ? "failure" : "success";
        }

        private int GetSavingThrowTargetNumber(Combatant target)
        {
            //TODO: refactor into nested switches
            int result;

            if (target.CharacterClass == "magic-user" || target.CharacterClass == "illusionist")
            {
                result = target.Level switch
                {
                    < 6 => 12,
                    6 or 7 or 8 or 9 or 10 => 10,
                    11 or 12 or 13 or 14 or 15 => 8,
                    16 or 17 or 18 or 19 or 20 => 6,
                    > 20 => 4
                };
            }
            else if (target.CharacterClass == "cleric" || target.CharacterClass == "druid")
            {
                result = target.Level switch
                {
                    < 4 => 15,
                    4 or 5 or 6 => 14,
                    7 or 8 or 9 => 12,
                    10 or 11 or 12 => 11,
                    13 or 14 or 15 => 10,
                    16 or 17 or 18 => 9,
                    > 18 => 7
                };
            }
            else if (target.CharacterClass == "thief" || target.CharacterClass == "assassin" || target.CharacterClass == "monk")
            {
                result = target.Level switch
                {
                    < 5 => 15,
                    5 or 6 or 7 or 8 => 13,
                    9 or 10 or 11 or 12 => 11,
                    13 or 14 or 15 or 16 => 9,
                    17 or 18 or 19 or 20 => 7,
                    > 20 => 5
                };
            }
            else
            {
                result = target.Level switch
                {
                    < 1 => 19,
                    1 or 2 => 17,
                    3 or 4 => 16,
                    5 or 6 => 14,
                    7 or 8 => 13,
                    9 or 10 => 11,
                    11 or 12 => 10,
                    13 or 14 => 8,
                    15 or 16 => 7,
                    > 16 => 6
                };
            }

            return result;
        }

        public bool ApplyDamageToCombatant(Combatant targeter, Combatant target, string spellName, int damage, List<string> entries, int segment, bool opportunityForSimulAttack)
        {
            //adjust target hp and GotHitThisRound status
            target.CurrentHP -= damage;
            target.GotHitThisRound = true;

            if (spellName != null)
            {
                entries.Add($"{targeter.Name}'s {spellName} spell did {damage} damage to {target.Name}.");
            }
            else
            {
                if (target.Statuses.Any(s => s.Name == "asleep") || target.Statuses.Any(s => s.Name == "held"))
                {
                    entries.Add($"{target.Name}, helpless, is subject to a deliberate strike from {targeter.Name}...");
                }
                entries.Add($"{targeter.Name} struck {target.Name} for {damage} damage.");
            }

            if (target.CurrentHP < 1)
            {
                entries.Add($"{target.Name} fell.");

                //a combatant that falls during the segment where they were about to take their action still gets to take their action
                if (target.Init == segment)
                {
                    opportunityForSimulAttack = true;
                }
            }

            //a sleeping character who takes damage (and survives) wakes up
            if (target.Statuses.FindIndex(x => x.Name == "asleep") >= 0)
            {
                entries.Add($"{target.Name} is no longer asleep.");
                target.Statuses.RemoveAll(r => r.Name == "asleep");
            }

            return opportunityForSimulAttack;
        }
    }
}
