using UnityEngine;

namespace Game.Gameplay
{
    public class PoolsInstaller : Zenject.MonoInstaller
    {
        [SerializeField] Projectile projectilePrefab = null;
        [SerializeField] ProjectileHitFx projectileHitPrefab = null;
        [SerializeField] Enemy enemyPrefab = null;
        [SerializeField] MiniBoss miniBossPrefab = null;
        [SerializeField] EnemyDeathFx enemyDeathFxPrefab = null;

        static readonly int projectilesPoolSize = 200;
        static readonly int projectilesHitPoolSize = projectilesPoolSize;

        static readonly int enemyPoolSize = 40;
        static readonly int enemyDeathFxPoolSize = enemyPoolSize;

        static readonly int minibossPoolSize = 4;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<Projectile, ProjectilesPool>()
               .WithInitialSize(projectilesPoolSize)
               .FromComponentInNewPrefab(projectilePrefab)
               .UnderTransformGroup("Projectiles");

            Container.BindMemoryPool<ProjectileHitFx, ProjectileHitFxPool>()
               .WithInitialSize(projectilesHitPoolSize)
               .FromComponentInNewPrefab(projectileHitPrefab)
               .UnderTransformGroup("ProjectilesHit");

            Container.BindMemoryPool<Enemy, EnemyPool>()
                .WithInitialSize(enemyPoolSize)
                .FromComponentInNewPrefab(enemyPrefab)
                .UnderTransformGroup("Enemies");

            Container.BindMemoryPool<MiniBoss, MiniBossPool>()
                .WithInitialSize(minibossPoolSize)
                .FromComponentInNewPrefab(miniBossPrefab)
                .UnderTransformGroup("MiniBoss");

            Container.BindMemoryPool<EnemyDeathFx, EnemyDeathFxPool>()
                .WithInitialSize(enemyDeathFxPoolSize)
                .FromComponentInNewPrefab(enemyDeathFxPrefab)
                .UnderTransformGroup("EnemyDeathFx");
        }
    }
}