using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    //TODO: Should derrive from Enemy, but there was no time for rework (No Time For Caution plays in background https://www.youtube.com/watch?v=m3zvVGJrTP8)
    public class MiniBoss : Unit, IPoolable<MiniBoss.SpawnContext, MiniBossPool>
    {
        //TODO: rework
        [Inject] PlayableArea playableArea = null;
        [Inject] ProjectilesPool projectilePool = null;

        //Maybe some sensors for finding player?
        [Inject] PlayerController playerCtrl = null;

        MiniBossPool pool;
        SpawnContext context;

        //TODO: STRONG TYPING!
        private EnemyStats enemyStats => (EnemyStats)stats;

        Vector3 moveTargetPosition;

        public void OnDespawned()
        {
            ClearEvents();
        }

        public void OnSpawned(SpawnContext spawnContext, MiniBossPool pool)
        {
            Initialize();

            this.pool = pool;
            faction = EFaction.Enemy;
            context = spawnContext;
            transform.position = context.Position;

            RandomizeTargetPosition();
        }

        protected override void Shoot()
        {
            Vector3 dirToPlayer = playerCtrl.PlayerPosition - transform.position;
            dirToPlayer.Normalize();
            projectilePool.Spawn(new Projectile.SpawnContext(
                   transform.position,
                   this,
                   stats.BaseProjectileSpeed,
                   dirToPlayer,
                   stats.Color
               ));
        }

        protected override void Death()
        {
            base.Death();
            context.DeathFxPool.Spawn(new EnemyDeathFx.SpawnContext()
            {
                Position = transform.position,
                ColorHD = enemyStats.Color
            });
            pool.Despawn(this);
        }

        protected override void Update()
        {
            base.Update();
            ShootIfCan();
        }

        protected override void MovementFunction()
        {
            Vector3 calculatedPosition = transform.position;
            if (Vector3.Distance(transform.position, moveTargetPosition) > 0.1f)
            {
                calculatedPosition = Vector3.MoveTowards(transform.position, moveTargetPosition, enemyStats.MoveSpeed * Time.deltaTime);
            }
            else
            {
                RandomizeTargetPosition();
            }
            transform.position = calculatedPosition;
        }

        //our miniboss moves outside Grid, quasi-randomly
        void RandomizeTargetPosition()
        {
            Vector3 randomInSphere = Random.insideUnitSphere.WithZ(0);
            randomInSphere *= playableArea.PlayableHeight;
            moveTargetPosition = randomInSphere;
        }

        public struct SpawnContext
        {
            Vector3 position;
            EnemyDeathFxPool deathFxPool;

            public Vector3 Position => position;

            //why? to inject once and not on every pool resize
            public EnemyDeathFxPool DeathFxPool => deathFxPool;

            public SpawnContext(Vector3 position, EnemyDeathFxPool deathFxPool)
            {
                this.position = position;
                this.deathFxPool = deathFxPool;
            }
        }
    }
}