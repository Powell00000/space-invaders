using System.Collections.Generic;
using Zenject;

namespace Game.Gameplay
{
    public class MiniBossPool : MonoMemoryPool<MiniBoss.SpawnContext, MiniBoss>
    {
        List<MiniBoss> spawnedItems = new List<MiniBoss>();

        protected override void Reinitialize(MiniBoss.SpawnContext p1, MiniBoss item)
        {
            item.OnSpawned(p1, this);
            base.Reinitialize(p1, item);
        }

        protected override void OnSpawned(MiniBoss item)
        {
            base.OnSpawned(item);
            spawnedItems.Add(item);
        }

        protected override void OnDespawned(MiniBoss item)
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