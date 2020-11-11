using Rhisis.Core.Common;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Game.Features.AttackArbiters.Participants
{
    class PlayerBattleParticipant : IBattleParticipant
    {
        private IPlayer _player;

        public PlayerBattleParticipant(IPlayer player)
        {
            this._player = player;
        }

        public Range<int> CalculateMeleeAttackRange()
        {
            IItem weapon = _player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? _player.Inventory.Hand;
            // TODO: get dual weapon for blades
            int weaponAttack = GetWeaponAttackDamages(_player, weapon.Data.WeaponType);

            var attackRange = new Range<int>(weapon.Data.AbilityMin * 2 + weaponAttack, weapon.Data.AbilityMax * 2 + weaponAttack);

            return attackRange;
        }

        public int CalculateEscapeRating()
        {
            int playerDexterity = _player.Statistics.Dexterity + _player.Attributes.Get(DefineAttributes.DEX);

            return (int)(playerDexterity * 0.5f); // TODO: add DST_PARRY
        }

        public int CalculateHitRating()
        {
            return _player.Statistics.Dexterity + _player.Attributes.Get(DefineAttributes.DEX);
        }

        public bool HasOneKillMode()
        {
            return _player.Mode.HasFlag(ModeType.ONEKILL_MODE);
        }

        public int GetLevel()
        {
            return _player.Level;
        }

        public float GetHitRatingNumeratorMultiplier()
        {
            return 1.6f;
        }

        public float GetHitRatingDenominatorMultiplier()
        {
            return 1.5f;
        }

        public float GetHitRatingLevelNumeratorMultiplier()
        {
            return 1.2f;
        }

        public float GetHitRatingLevelDenominatorMultiplier()
        {
            return 1f;
        }

        public int GetMinHitRating()
        {
            return 20;
        }

        public int GetMaxHitRating()
        {
            return 96;
        }

 

        public float GetMagicAttackDamageMultiplier(int magicPower)
        {
            return magicPower switch
            {
                0 => 0.6f,
                1 => 0.8f,
                2 => 1.05f,
                3 => 1.1f,
                4 => 1.3f,
                _ => 1.0f
            };
        }

        public Range<int> CalculateMagicAttackRange()
        {
            IItem wandWeapon = _player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? _player.Inventory.Hand;

            Range<int> weaponAttackResult = GetWeaponAttackPower(_player, wandWeapon);
            int weaponAttackDamages = GetWeaponAttackDamages(_player, WeaponType.MAGIC_WAND);
            var attackRange = new Range<int>(weaponAttackResult.Minimum + weaponAttackDamages, weaponAttackResult.Maximum + weaponAttackDamages);
            return attackRange;
        }

        public int GetBonusMagicAttackDamage()
        {
            return Math.Max(0, _player.Attributes.Get(DefineAttributes.CHR_DMG));
        }

        /// <summary>
        /// Gets the weapon attack damages based on player's statistics.
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="weaponType">Weapon type</param>
        /// <returns></returns>
        private int GetWeaponAttackDamages(IPlayer player, WeaponType weaponType)
        {
            float attribute = 0f;
            float levelFactor = 0f;
            float jobFactor = 1f;

            switch (weaponType)
            {
                case WeaponType.MELEE_SWD:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 12;
                    levelFactor = player.Level * 1.1f;
                    jobFactor = player.Job.MeleeSword;
                    break;
                case WeaponType.MELEE_AXE:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 12;
                    levelFactor = player.Level * 1.2f;
                    jobFactor = player.Job.MeleeAxe;
                    break;
                case WeaponType.MELEE_STAFF:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 10;
                    levelFactor = player.Level * 1.1f;
                    jobFactor = player.Job.MeleeStaff;
                    break;
                case WeaponType.MELEE_STICK:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 10;
                    levelFactor = player.Level * 1.3f;
                    jobFactor = player.Job.MeleeStick;
                    break;
                case WeaponType.MELEE_KNUCKLE:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 10;
                    levelFactor = player.Level * 1.2f;
                    jobFactor = player.Job.MeleeKnuckle;
                    break;
                case WeaponType.MAGIC_WAND:
                    attribute = player.Statistics.Intelligence + player.Attributes.Get(DefineAttributes.INT) - 10;
                    levelFactor = player.Level * 1.2f;
                    jobFactor = player.Job.MagicWand;
                    break;
                case WeaponType.MELEE_YOYO:
                    attribute = player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR) - 10;
                    levelFactor = player.Level * 1.1f;
                    jobFactor = player.Job.MeleeYoyo;
                    break;
                case WeaponType.RANGE_BOW:
                    attribute = ((player.Statistics.Dexterity + player.Attributes.Get(DefineAttributes.DEX)) - 14) * 4f;
                    levelFactor = player.Level * 1.3f;
                    jobFactor = ((player.Statistics.Strength + player.Attributes.Get(DefineAttributes.STR)) * 0.2f) * 0.7f;
                    break;
            }

            return (int)(attribute * jobFactor + levelFactor);
        }

        /// <summary>
        /// Gets the weapon attack power.
        /// </summary>
        /// <param name="entity">Living entity using the weapon.</param>
        /// <param name="weapon">Weapon used by the living entity.</param>
        /// <returns><see cref="AttackResult"/> with AttackMin and AttackMax range.</returns>
        private Range<int> GetWeaponAttackPower(IMover entity, IItem weapon)
        {
            float multiplier = GetWeaponItemMultiplier(weapon);
            int power = weapon?.Refine > 0 ? (int)Math.Pow(weapon?.Refine ?? default, 1.5f) : default;

            return new Range<int>(
                (int)((entity.Attributes.Get(DefineAttributes.ABILITY_MIN) + weapon?.Data.AbilityMin) * multiplier) + power,
                (int)((entity.Attributes.Get(DefineAttributes.ABILITY_MAX) + weapon?.Data.AbilityMax) * multiplier) + power
            );
        }

        /// <summary>
        /// Gets the weapon item multiplier.
        /// </summary>
        /// <param name="weapon">Current used weapon.</param>
        /// <returns></returns>
        private float GetWeaponItemMultiplier(IItem weapon)
        {
            if (weapon is null)
            {
                return 1f;
            }

            // TODO: check if item has expired.
            float multiplier = 1.0f;
            int refine = weapon.Data.WeaponKind == WeaponKindType.Ultimate ? ItemConstants.WeaponArmonRefineMax : weapon.Refine;

            if (refine > 0)
            {
                // TODO: get item exp up
                int itemExpUp = 0 + 100;
                multiplier *= (itemExpUp / 100);
            }

            return multiplier;
        }
    }
}
