using UnityEngine;

namespace Game.Gameplay
{
    public class WaveManager : MonoBehaviour, Zenject.IInitializable, System.IDisposable
    {
        //here is our progression of waves
        [SerializeField] WaveHolder[] waveStatsHolders = null;

        [Zenject.Inject] GameplayController gameplayCtrl = null;
        [Zenject.Inject] WaveProgressController waveProgress = null;
        [Zenject.Inject] PlayableArea playableArea = null;
        [Zenject.Inject] InputController inputCtrl = null;

        public System.Action<Wave> OnWaveTriggered;
        public System.Action OnAllWavesFinished;

        //for auto positioning
        const float gridsYOffset = 2;

        Wave currentWave;
        GridParent spawnedGridParent;

        int waveIndex = -1;

        public void Initialize()
        {
            gameplayCtrl.OnLevelStarting += OnLevelStarting;
            waveProgress.OnWaveFinished += SpawnNextWave;
        }

        void SpawnNextWave()
        {
            ClearSpawnedAreas();

            waveIndex++;

            if (waveIndex >= waveStatsHolders.Length)
            {
                //game over
                OnAllWavesFinished?.Invoke();
                return;
            }

            //get scriptable with grids definition
            WaveHolder waveStats = waveStatsHolders[waveIndex];

            //GridParent->GridArea[]
            var gridsCount = waveStats.GridParent.Grids.Length;

            //spawn grid parent with all grids inside
            spawnedGridParent = Instantiate(waveStats.GridParent) as GridParent;

            //now iterate through all grids
            for (int i = 0; i < gridsCount; i++)
            {
                GridArea area = spawnedGridParent.Grids[i];

                if (waveStats.AutoPositionRows)
                    area.transform.position = playableArea.TopGridSpawnPosition - Vector3.up * i * gridsYOffset;

                area.Initialize();
            }

            currentWave = new Wave(spawnedGridParent, waveStats);

            OnWaveTriggered?.Invoke(currentWave);
        }

        void ClearSpawnedAreas()
        {
            //enemies does not despawn with grids
            if (spawnedGridParent != null)
            {
                Destroy(spawnedGridParent.gameObject);
                spawnedGridParent = null;
            }
        }

        void OnLevelStarting()
        {
            waveIndex = -1;
            SpawnNextWave();
        }

        public void Dispose()
        {
            if (gameplayCtrl)
                gameplayCtrl.OnLevelStarting -= OnLevelStarting;
            if (waveProgress != null)
                waveProgress.OnWaveFinished -= SpawnNextWave;
            if (inputCtrl != null)
                inputCtrl.KillAllEnemies -= ClearSpawnedAreas;
        }

        [System.Serializable]
        public class Wave
        {
            GridParent gridParent;
            WaveHolder waveSettings;

            public GridParent SpawnedGridParent => gridParent;
            public WaveHolder WaveSettings => waveSettings;

            public Wave(GridParent spawnedGridParent, WaveHolder waveSettings)
            {
                this.gridParent = spawnedGridParent;
                this.waveSettings = waveSettings;
            }
        }
    }
}