using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Game.Structures
{
    public class MagicSkillProjectile : Projectile
    {
        /// <inheritdoc />
        public override AttackFlags Type => AttackFlags.AF_MAGICSKILL;

        /// <summary>
        /// Gets the projectile skill.
        /// </summary>
        public Skill Skill { get; }

        /// <summary>
        /// Creates a new <see cref="MagicSkillProjectile"/> instance.
        /// </summary>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        /// <param name="skill">Projectile skill.</param>
        /// <param name="onArrived">Action to execute when the magic attack projectile arrives to its target.</param>
        public MagicSkillProjectile(ILivingEntity owner, ILivingEntity target, Skill skill, Action onArrived) 
            : base(owner, target, onArrived)
        {
            Skill = skill;
        }
    }
}