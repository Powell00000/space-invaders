using System.Collections.Generic;

namespace Game.Gameplay
{
    public class EnemyDeathFxPool : ParticlesPoolBase<EnemyDeathFx.SpawnContext, EnemyDeathFx>
    {
        List<EnemyDeathFx> spawnedItems = new List<EnemyDeathFx>();

        protected override void Reinitialize(EnemyDeathFx.SpawnContext context, EnemyDeathFx item)
        {
            base.Reinitialize(context, item);
            item.OnSpawned(context, this);
        }

        protected override void OnSpawned(EnemyDeathFx item)
        {
            base.OnSpawned(item);
            spawnedItems.Add(item);
        }

        protected override void OnDespawned(EnemyDeathFx item)
        {
            base.OnDespawned(item);
            spawnedItems.Remove(item);
        }

        public void DespawnAll()
        {
            foreach (var spawnedItem in spawnedItems)
            {
                this.Despawn(spawnedItem);
            }
            spawnedItems.Clear();
        }
    }
}