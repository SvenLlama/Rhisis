using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Battle
{
    /// <summary>
    /// Provides a mechanism to fight other entities like monsters (PVE) or players (PVP).
    /// </summary>
    public interface IBattleSystem
    {
        /// <summary>
        /// Inflicts damages to a defender target based on the given attack arbiter.
        /// </summary>
        /// <param name="attacker">Attacker entity.</param>
        /// <param name="defender">Defender target entity.</param>
        /// <param name="attackDescriptor">Attack descriptor.</param>
        /// <returns>The attack result.</returns>
        AttackResult DamageTarget(ILivingEntity attacker, ILivingEntity defender, AttackDescriptor attackDescriptor);

        /// <summary>
        /// Process a melee attack on an ennemy.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="attackType">Attack type.</param>
        /// <param name="attackSpeed">Attack speed.</param>
        void MeleeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, float attackSpeed);

        /// <summary>
        /// Process a magic attack on an ennemy.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="attackType">Attack type.</param>
        /// <param name="magicAttackPower">Magic attack power.</param>
        /// <param name="projectileId">Magic projectile id.</param>
        void MagicAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, int magicAttackPower, int projectileId);

        /// <summary>
        /// Process a range attack on an ennemy.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="attackType">Attack type.</param>
        /// <param name="power">Range attack power.</param>
        /// <param name="projectileId">Projectile id.</param>
        void RangeAttack(ILivingEntity attacker, ILivingEntity defender, ObjectMessageType attackType, int power, int projectileId);

        /// <summary>
        /// Process a skill attack on a defender target. 
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="skill">Skill to execute.</param>
        /// <param name="castingTime">Skill casting time.</param>
        /// <returns>True if the skill has been executed; false otherwise.</returns>
        bool MeleeSkillAttack(ILivingEntity attacker, ILivingEntity defender, Skill skill, int castingTime);

        /// <summary>
        /// Process a magic skill attack on a defender target.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="skill">Skill to execute.</param>
        /// <param name="castingTime">Skill casting time.</param>
        /// <returns>True if the skill has been executed; false otherwise.</returns>
        bool MagicSkillAttack(ILivingEntity attacker, ILivingEntity defender, Skill skill, int castingTime);

        /// <summary>
        /// Process a magic skill projectile on a defender target.
        /// </summary>
        /// <param name="attacker">Attacker.</param>
        /// <param name="defender">Defender.</param>
        /// <param name="skill">Skill to execute.</param>
        /// <param name="castingTime">Skill casting time.</param>
        /// <returns>True if the skill has been executed; false otherwise.</returns>
        bool MagicSkillShotAttack(ILivingEntity attacker, ILivingEntity defender, Skill skill, int castingTime);
    }
}
