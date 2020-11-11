using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Game.Features.AttackArbiters.Participants;
using System;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class MeleeAttackArbiter : AttackArbiterBase
    {
        private readonly AttackFlags _attackFlags;
        private readonly int _attackPower;

        /// <summary>
        /// Creates a new <see cref="MeleeAttackArbiter"/> instance.
        /// </summary>
        /// <param name="attackerOld">Attacker entity</param>
        /// <param name="defenderOld">Defender entity</param>
        /// <param name="attackFlags">Default attack flags.</param>
        /// <param name="attackPower">Attack power.</param>
        public MeleeAttackArbiter(IMover attackerOld, IMover defenderOld, IBattleParticipant attacker, IBattleParticipant defender, AttackFlags attackFlags = AttackFlags.AF_GENERIC, int attackPower = 0)
            : base(attackerOld, defenderOld, attacker, defender)
        {
            _attackFlags = attackFlags;
            _attackPower = attackPower;
        }

        /// <summary>
        /// Calculates the melee attack damages.
        /// </summary>
        /// <returns></returns>
        public override AttackResult CalculateDamages()
        {
            AttackFlags flags = GetAttackFlags();

            if (flags.HasFlag(AttackFlags.AF_MISS))
            {
                return AttackResult.Miss();
            }

            Range<int> attackRange = Attacker.CalculateMeleeAttackRange();

            if (IsCriticalAttack(AttackerOld, flags))
            {
                flags |= AttackFlags.AF_CRITICAL;
                attackRange = CalculateCriticalDamages(attackRange);

                if (IsKnockback(flags))
                {
                    flags |= AttackFlags.AF_FLYING;
                }
            }

            int damages = RandomHelper.Random(attackRange.Minimum, attackRange.Maximum);

            if (flags.HasFlag(AttackFlags.AF_RANGE))
            {
                damages = (int)(damages * GetChargeAttackMultiplier()); 
            }

            return AttackResult.Success(damages, flags);
        }

        /// <summary>
        /// Gets the <see cref="AttackFlags"/> of this melee attack.
        /// </summary>
        /// <returns></returns>
        private AttackFlags GetAttackFlags()
        {
            if (Attacker.HasOneKillMode())
            {
                return AttackFlags.AF_GENERIC;
            }

            var hitRating = Attacker.CalculateHitRating();
            var escapeRating = Defender.CalculateEscapeRating();
            var hitRatingNumeratorMultiplier = Attacker.GetHitRatingNumeratorMultiplier();
            var hitRatingDenominator = Attacker.GetHitRatingDenominatorMultiplier();
            var hitRatingLevelNumeratorMultiplier = Attacker.GetHitRatingLevelNumeratorMultiplier();
            var hitRatingLevelDenominatorMultiplier = Attacker.GetHitRatingLevelDenominatorMultiplier();

            int hitRate = (int)(hitRating * hitRatingNumeratorMultiplier / (hitRating + escapeRating) * hitRatingDenominator *
                      (AttackerOld.Level * hitRatingLevelNumeratorMultiplier / (AttackerOld.Level + DefenderOld.Level * hitRatingLevelDenominatorMultiplier)) * 100.0f);

            hitRate = Math.Clamp(hitRate, Attacker.GetMinHitRating(), Attacker.GetMaxHitRating());

            return RandomHelper.Random(0, 100) < hitRate ? _attackFlags : AttackFlags.AF_MISS;
        }

        /// <summary>
        /// Check if the attacker's melee attack is a critical hit.
        /// </summary>
        /// <param name="attacker">Attacker</param>
        /// <param name="currentAttackFlags">Attack flags</param>
        /// <returns></returns>
        private bool IsCriticalAttack(IMover attacker, AttackFlags currentAttackFlags)
        {
            if (currentAttackFlags.HasFlag(AttackFlags.AF_MELEESKILL) || currentAttackFlags.HasFlag(AttackFlags.AF_MAGICSKILL))
                return false;

            int baseDexterity = attacker switch
            {
                IPlayer p => p.Statistics.Dexterity,
                IMonster m => m.Statistics.Dexterity,
                _ => attacker.Data.Dexterity
            };
            var criticalJobFactor = attacker is IPlayer player ? player.Job.Critical : 1f;
            var criticalProbability = (int)((baseDexterity + attacker.Attributes.Get(DefineAttributes.DEX)) / 10 * criticalJobFactor);
            // TODO: add DST_CHR_CHANCECRITICAL to criticalProbability

            if (criticalProbability < 0)
            {
                criticalProbability = 0;
            }

            // TODO: check if player is in party and if it has the MVRF_CRITICAL flag

            return RandomHelper.Random(0, 100) < criticalProbability;
        }

        /// <summary>
        /// Calculate critical damages.
        /// </summary>
        /// <param name="actualAttackRange">Attack result</param>
        private Range<int> CalculateCriticalDamages(Range<int> actualAttackRange)
        {
            var criticalMin = 1.1f;
            var criticalMax = 1.4f;

            if (AttackerOld.Level > DefenderOld.Level)
            {
                if (DefenderOld is IMonster)
                {
                    criticalMin = 1.2f;
                    criticalMax = 2.0f;
                }
                else
                {
                    criticalMin = 1.4f;
                    criticalMax = 1.8f;
                }
            }

            float criticalBonus = 1; // TODO: 1 + (DST_CRITICAL_BONUS / 100)

            if (criticalBonus < 0.1f)
            {
                criticalBonus = 0.1f;
            }

            var attackMin = (int)(actualAttackRange.Minimum * criticalMin * criticalBonus);
            var attackMax = (int)(actualAttackRange.Maximum * criticalMax * criticalBonus);

            return new Range<int>(attackMin, attackMax);
        }

        /// <summary>
        /// Check if the current attack is a chance to knockback the defender.
        /// </summary>
        /// <param name="attackerAttackFlags">Attacker attack flags</param>
        /// <returns></returns>
        private bool IsKnockback(AttackFlags attackerAttackFlags)
        {
            var knockbackChance = RandomHelper.Random(0, 100) < 15;

            if (DefenderOld is IPlayer)
            {
                return false;
            }

            if (AttackerOld is IPlayer player)
            {
                IItem weapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? player.Inventory.Hand;

                if (weapon.Data.WeaponType == WeaponType.MELEE_YOYO || attackerAttackFlags.HasFlag(AttackFlags.AF_FORCE))
                {
                    return false;
                }
            }

            var canFly = false;

            // TODO: if is flying, return false
            if (DefenderOld.ObjectState.HasFlag(ObjectState.OBJSTA_DMG_FLY_ALL) && DefenderOld is IMonster monster)
            {
                canFly = monster.Data.Class != MoverClassType.RANK_SUPER &&
                    monster.Data.Class != MoverClassType.RANK_MATERIAL &&
                    monster.Data.Class != MoverClassType.RANK_MIDBOSS;
            }

            return canFly && knockbackChance;
        }

        /// <summary>
        /// Gets range attack charge multiplier.
        /// </summary>
        /// <returns></returns>
        private float GetChargeAttackMultiplier()
        {
            if (!_attackFlags.HasFlag(AttackFlags.AF_RANGE))
            {
                return 1f;
            }

            return _attackPower switch
            {
                0 => 1.0f,
                1 => 1.2f,
                2 => 1.5f,
                3 => 1.8f,
                4 => 2.2f,
                _ => 1.0f
            };
        }
    }
}
