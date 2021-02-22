using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    //here we manage the wave progression, so keeping track of spawned and destroyed enemies
    public class WaveProgressController : IInitializable, IDisposable
    {
        [Inject] EnemySpawner enemySpawner = null;
        [Inject] InputController inputCtrl = null;
        [Inject] GameplayController gameplayCtrl = null;

        public Action OnWaveFinished;

        List<Unit> aliveEnemies;

        //TODO: should it be public?
        public List<Unit> AliveEnemies => aliveEnemies;

        int aliveEnemiesCount => aliveEnemies.Count;

        void IInitializable.Initialize()
        {
            aliveEnemies = new List<Unit>();
            enemySpawner.OnEnemySpawned += OnEnemySpawned;
            //for progressing faster
            inputCtrl.KillAllEnemies += DespawnAliveEnemies;
            //for restarting -> cleanup
            gameplayCtrl.OnLevelRestarting += ClearList;
        }

        //TODO: memory management
        void DespawnAliveEnemies()
        {
            //we cannot operate on aliveEnemies, because there are already spawning new enemies and collection changes
            List<Unit> tmp = new List<Unit>(aliveEnemies);
            for (int i = 0; i < tmp.Count; i++)
            {
                tmp[i].ForceDeath();
            }
            //we don't need to clear aliveEnemies, because ForceDeath() triggers OnEnemyDeath()
            tmp = null;
        }

        void ClearList()
        {
            aliveEnemies.Clear();
        }

        void OnEnemyDeath(Unit deadUnit)
        {
            var enemy = deadUnit;
            aliveEnemies.Remove(enemy);

            if (aliveEnemiesCount == 0)
            {
                if (OnWaveFinished != null)
                    OnWaveFinished();
            }
        }

        void OnEnemySpawned(Unit spawnedUnit)
        {
            var enemy = spawnedUnit;
            aliveEnemies.Add(enemy);

            spawnedUnit.OnDeath += OnEnemyDeath;
        }

        void IDisposable.Dispose()
        {
            if (enemySpawner != null)
                enemySpawner.OnEnemySpawned -= OnEnemySpawned;
            if (inputCtrl != null)
                inputCtrl.KillAllEnemies -= DespawnAliveEnemies;
            if (gameplayCtrl != null)
                gameplayCtrl.OnLevelRestarting -= ClearList;

        }
    }
}