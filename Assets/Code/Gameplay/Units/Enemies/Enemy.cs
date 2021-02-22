using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class Enemy : Unit, IPoolable<Enemy.SpawnContext, EnemyPool>
    {
        //TODO: helper?
        [Inject] ProjectilesPool projectilePool = null;

        //TODO: STRONG TYPING
        private EnemyStats enemyStats => (EnemyStats)stats;

        EnemyPool enemyPool;

        private SpawnContext context;

        public System.Action<Enemy> OnPooled;

        public void OnDespawned()
        {
            if (OnPooled != null)
                OnPooled(this);
            ClearEvents();
        }

        protected override void ClearEvents()
        {
            base.ClearEvents();
            OnPooled = null;
        }

        protected override void Shoot()
        {
            projectilePool.Spawn(new Projectile.SpawnContext(
                transform.position,
                this,
                stats.BaseProjectileSpeed,
                -Vector3.up,
                stats.Color
            ));
        }

        public void OnSpawned(SpawnContext spawnContext, EnemyPool enemyPool)
        {
            Initialize();

            faction = EFaction.Enemy;

            context = spawnContext;
            transform.position = spawnContext.Position;
            this.enemyPool = enemyPool;
        }

        protected override void Death()
        {
            base.Death();
            context.DeathFxPool.Spawn(new EnemyDeathFx.SpawnContext()
            {
                Position = transform.position,
                ColorHD = enemyStats.Color
            });
            enemyPool.Despawn(this);
        }

        protected override void MovementFunction()
        {
            Vector3 targetPos = transform.position;
            if (context.Cell != null)
            {
                if (context.Cell.WorldTransform != null)
                {
                    targetPos = context.Cell.WorldTransform.position;
                }
            }

            Vector3 calculatedPosition;
            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                calculatedPosition = Vector3.MoveTowards(transform.position, targetPos, enemyStats.FlySpeed * Time.deltaTime);
            }
            else
            {
                calculatedPosition = Vector3.MoveTowards(transform.position, targetPos, enemyStats.MoveSpeed * Time.deltaTime);
            }
            transform.position = calculatedPosition;
        }

        public struct SpawnContext
        {
            GridArea.Cell cell;
            Vector3 position;
            EnemyDeathFxPool deathFxPool;

            public Vector3 Position => position;
            public GridArea.Cell Cell => cell;
            public EnemyDeathFxPool DeathFxPool => deathFxPool;

            public SpawnContext(Vector3 position, GridArea.Cell cell, EnemyDeathFxPool deathFxPool)
            {
                this.position = position;
                this.cell = cell;
                this.deathFxPool = deathFxPool;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            //TODO: this is NOT strong typing; create custom inspector or so
            if (stats != null)
            {
                if (enemyStats == null)
                {
                    stats = null;
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
        }

#endif
    }
}