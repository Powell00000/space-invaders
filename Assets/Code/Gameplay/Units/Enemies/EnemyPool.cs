using System.Collections.Generic;
using Zenject;

namespace Game.Gameplay
{
    public class EnemyPool : MonoMemoryPool<Enemy.SpawnContext, Enemy>
    {
        List<Enemy> spawnedItems = new List<Enemy>();

        protected override void Reinitialize(Enemy.SpawnContext p1, Enemy item)
        {
            item.OnSpawned(p1, this);
            base.Reinitialize(p1, item);
        }

        protected override void OnSpawned(Enemy item)
        {
            base.OnSpawned(item);
            spawnedItems.Add(item);
        }

        protected override void OnDespawned(Enemy item)
        {
            base.OnDespawned(item);
            item.OnDespawned();
            spawnedItems.Remove(item);
            //ConditionalLogger.Log("enemy despawned");
        }

        public void DespawnAll()
        {
            for (int i = 0; i < spawnedItems.Count;)
            {
                this.Despawn(spawnedItems[i]);
            }
            spawnedItems.Clear();
        }
    }
}