using UnityEngine;

namespace Game.Gameplay
{
    public class EnemySpawner : Zenject.IInitializable, System.IDisposable
    {
        //TODO: too many dependencies?
        [Zenject.Inject] WaveManager waveManager = null;
        [Zenject.Inject] EnemyPool enemyPool = null;
        [Zenject.Inject] MiniBossPool miniBossPool = null;
        [Zenject.Inject] PlayableArea playableArea = null;
        [Zenject.Inject] EnemyDeathFxPool enemyDeathFxPool = null;

        public System.Action<Unit> OnEnemySpawned;

        Vector3 spawnOffset;
        Vector3 targetPosOnArc;

        public void Initialize()
        {
            //wave manager tells us when to spawn new enemies
            waveManager.OnWaveTriggered += SpawnEnemies;
            spawnOffset = Vector3.up * playableArea.Width / 1.5f;
        }

        public void Dispose()
        {
            waveManager.OnWaveTriggered -= SpawnEnemies;
        }

        void SpawnMiniBoss()
        {
            //random position on half sphere outside screen view
            targetPosOnArc = Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.forward) * spawnOffset;

            var miniBoss = miniBossPool.Spawn(new MiniBoss.SpawnContext(targetPosOnArc, enemyDeathFxPool));
            if (OnEnemySpawned != null)
                OnEnemySpawned(miniBoss);
        }

        void SpawnEnemies(WaveManager.Wave currentWave)
        {
            //spaghetti code 101

            //we take grids setup from Grid parent
            foreach (var gridArea in currentWave.SpawnedGridParent.Grids)
            {
                //random position on half sphere outside screen view
                targetPosOnArc = Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.forward) * spawnOffset;

                //for every cell in GridArea in GridParent
                for (int i = 0; i < gridArea.CellsCount; i++)
                {
                    bool hasFreeCell = gridArea.TryGetFreeCell(out var cell);

                    if (hasFreeCell)
                    {
                        var spawnContext = new Enemy.SpawnContext(targetPosOnArc, cell, enemyDeathFxPool);
                        var spawnedEnemy = enemyPool.Spawn(spawnContext);

                        cell.OccupyCell(spawnedEnemy);

                        if (OnEnemySpawned != null)
                            OnEnemySpawned(spawnedEnemy);
                    }
                }
            }
            for (int i = 0; i < currentWave.WaveSettings.MiniBossCount; i++)
            {
                SpawnMiniBoss();
            }
        }
    }
}