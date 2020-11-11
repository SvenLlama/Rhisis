using Rhisis.Core.Common;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Game.Features.AttackArbiters.Participants;
using System;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class AttackArbiterBase
    {
        public IMover AttackerOld { get; }
        
        public IMover DefenderOld { get; }
        
        public IBattleParticipant Attacker { get; }
        public IBattleParticipant Defender { get; }

        protected AttackArbiterBase(IMover attackerOld, IMover defenderOld, IBattleParticipant attacker, IBattleParticipant defender)
        {
            AttackerOld = attackerOld;
            DefenderOld = defenderOld;
            Attacker = attacker;
            Defender = defender;
        }

        /// <summary>
        /// Calculates the damages to inflict to the given defender.
        /// </summary>
        public virtual AttackResult CalculateDamages() => AttackResult.Miss();
  

        /// <summary>
        /// Gets the weapon extra damages based weapon type and entity attribute bonuses.
        /// </summary>
        /// <param name="entity">Current entity.</param>
        /// <param name="weapon">Current entity weapon.</param>
        /// <returns>Weapon extra damages</returns>
        public int GetWeaponExtraDamages(IMover entity, IItem weapon)
        {
            if (weapon is null)
            {
                return default;
            }

            int extraDamages = weapon.Data.WeaponType switch
            {
                WeaponType.MELEE_SWD => entity.Attributes.Get(DefineAttributes.SWD_DMG) + entity.Attributes.Get(DefineAttributes.TWOHANDMASTER_DMG),
                WeaponType.MELEE_AXE => entity.Attributes.Get(DefineAttributes.AXE_DMG) + entity.Attributes.Get(DefineAttributes.TWOHANDMASTER_DMG),
                WeaponType.KNUCKLE => entity.Attributes.Get(DefineAttributes.KNUCKLE_DMG) + entity.Attributes.Get(DefineAttributes.KNUCKLEMASTER_DMG),
                WeaponType.MELEE_YOYO => entity.Attributes.Get(DefineAttributes.YOY_DMG) + entity.Attributes.Get(DefineAttributes.YOYOMASTER_DMG),
                WeaponType.RANGE_BOW => entity.Attributes.Get(DefineAttributes.BOW_DMG) + entity.Attributes.Get(DefineAttributes.BOWMASTER_DMG),
                _ => default
            };

            if (entity is IPlayer player)
            {
                // TODO: check if player has dual weapons
                // TODO: if yes add "ONEHANDMASTER_DMG" to extra damages
            }

            return extraDamages;
        }

        /// <summary>
        /// Gets attacker attack multiplier.
        /// </summary>
        /// <returns></returns>
        public float GetAttackMultiplier()
        {
            var multiplier = 1.0f + AttackerOld.Attributes.Get(DefineAttributes.ATKPOWER_RATE) / 100f;

            if (AttackerOld is IPlayer)
            {
                // TODO: check SM mode SM_ATTACK_UP or SM_ATTACK_UP1 => multiplier *= 1.2f;

                var attribute = DefenderOld is IPlayer ? DefineAttributes.PVP_DMG : DefineAttributes.MONSTER_DMG;
                int damages = AttackerOld.Attributes.Get(attribute);

                if (damages > 0)
                {
                    multiplier += multiplier * damages / 100;
                }
            }

            return multiplier;
        }
    }
}
