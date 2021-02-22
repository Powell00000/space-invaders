using UnityEngine;

namespace Game.Gameplay
{
    public class ProjectileHitFx : PoolableParticlesBase<ProjectileHitFx.SpawnContext, ProjectileHitFxPool>
    {
        ProjectileHitFxPool pool;

        public override void OnSpawned(SpawnContext context, ProjectileHitFxPool pool)
        {
            base.OnSpawned(context, pool);
            this.pool = pool;

            transform.position = context.Position;

            particlesRenderer.SetEmission(context.ColorHD);

            PlayParticles();
        }

        protected override void Destroy()
        {
            base.Destroy();
            pool.Despawn(this);
        }

        public struct SpawnContext
        {
            public Vector3 Position;
            public Color32 ColorHD;
        }
    }
}