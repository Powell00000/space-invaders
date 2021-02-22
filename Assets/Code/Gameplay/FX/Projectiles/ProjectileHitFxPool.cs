using System.Collections.Generic;

namespace Game.Gameplay
{
    public class ProjectileHitFxPool : ParticlesPoolBase<ProjectileHitFx.SpawnContext, ProjectileHitFx>
    {
        List<ProjectileHitFx> spawnedItems = new List<ProjectileHitFx>();

        protected override void Reinitialize(ProjectileHitFx.SpawnContext context, ProjectileHitFx item)
        {
            base.Reinitialize(context, item);
            item.OnSpawned(context, this);
        }

        protected override void OnSpawned(ProjectileHitFx item)
        {
            base.OnSpawned(item);
            spawnedItems.Add(item);
        }

        protected override void OnDespawned(ProjectileHitFx item)
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