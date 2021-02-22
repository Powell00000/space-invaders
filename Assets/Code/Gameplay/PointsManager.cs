using System;

namespace Game.Gameplay
{
    //keeping track of points
    public class PointsManager : Zenject.IInitializable, IDisposable
    {
        [Zenject.Inject] EnemySpawner enemySpawner = null;
        [Zenject.Inject] GameplayController gameplayCtrl = null;

        int currentPoints;
        public Action<int> OnPointsChanged;

        public int CurrentPoints => currentPoints;

        public void Initialize()
        {
            enemySpawner.OnEnemySpawned += EnemySpawned;
            gameplayCtrl.OnLevelStarting += ClearPoints;
            ClearPoints();
        }

        void EnemySpawned(Unit unit)
        {
            unit.OnDeath += OnUnitDeath;
        }

        void OnUnitDeath(Unit deadUnit)
        {
            AddPoints(deadUnit.Stats.PointsForDestroying);
        }

        void ClearPoints()
        {
            SetPoints(0);
        }

        void SetPoints(int newValue)
        {
            currentPoints = newValue;
            RaisePointsChangedEvent();
        }

        public void AddPoints(int amount)
        {
            SetPoints(currentPoints + amount);
        }

        void RaisePointsChangedEvent()
        {
            if (OnPointsChanged != null)
                OnPointsChanged(currentPoints);
        }

        void IDisposable.Dispose()
        {
            if (enemySpawner != null)
                enemySpawner.OnEnemySpawned -= EnemySpawned;

            if (gameplayCtrl != null)
                gameplayCtrl.OnLevelStarting -= ClearPoints;
        }
    }
}