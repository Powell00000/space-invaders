using System.Collections.Generic;
using Zenject;

namespace Game.Gameplay
{
    public class ProjectilesPool : MonoMemoryPool<Projectile.SpawnContext, Projectile>
    {
        List<Projectile> spawnedItems = new List<Projectile>();

        protected override void Reinitialize(Projectile.SpawnContext p1, Projectile item)
        {
            item.OnSpawned(p1, this);
            base.Reinitialize(p1, item);
        }

        protected override void OnSpawned(Projectile item)
        {
            spawnedItems.Add(item);
            base.OnSpawned(item);
        }

        protected override void OnDespawned(Projectile item)
        {
            base.OnDespawned(item);
            item.OnDespawned();
            spawnedItems.Remove(item);
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