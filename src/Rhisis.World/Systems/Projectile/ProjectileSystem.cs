using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Projectile
{
    [Injectable]
    public class ProjectileSystem : IProjectileSystem
    {
        /// <inheritdoc />
        public int CreateProjectile(int projectileId, Game.Structures.Projectile projectile)
        {
            ILivingEntity livingEntity = projectile.Owner;

            livingEntity.Battle.Projectiles.Add(projectileId, projectile);

            return projectileId;
        }

        /// <inheritdoc />
        public void RemoveProjectile(ILivingEntity livingEntity, int projectileId)
        {
            livingEntity.Battle.Projectiles.Remove(projectileId);
        }

        /// <inheritdoc />
        public TProjectile GetProjectile<TProjectile>(ILivingEntity livingEntity, int projectileId) 
            where TProjectile : Game.Structures.Projectile 
            => livingEntity.Battle.Projectiles.TryGetValue(projectileId, out Game.Structures.Projectile projectile) ? (TProjectile)projectile : default;
    }
}
