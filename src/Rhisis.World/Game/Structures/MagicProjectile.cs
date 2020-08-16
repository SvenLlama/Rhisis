using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Game.Structures
{
    public class MagicProjectile : Projectile
    {
        /// <inheritdoc />
        public override AttackFlags Type => AttackFlags.AF_MAGIC;

        /// <summary>
        /// Gets the projectile magic power.
        /// </summary>
        public int MagicPower { get; }

        /// <summary>
        /// Creates a new <see cref="MagicProjectile"/> instance created by a wand attack.
        /// </summary>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        /// <param name="magicPower">Magic attack power.</param>
        /// <param name="onArrived">Action to execute when the magic projectile arrives to its target.</param>
        public MagicProjectile(ILivingEntity owner, ILivingEntity target, int magicPower, Action onArrived) 
            : base(owner, target, onArrived)
        {
            MagicPower = magicPower;
        }
    }
}