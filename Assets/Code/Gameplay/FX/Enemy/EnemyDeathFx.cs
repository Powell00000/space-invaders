using UnityEngine;

namespace Game.Gameplay
{
    public class EnemyDeathFx : PoolableParticlesBase<EnemyDeathFx.SpawnContext, EnemyDeathFxPool>
    {
        EnemyDeathFxPool pool;

        public override void OnSpawned(SpawnContext context, EnemyDeathFxPool pool)
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