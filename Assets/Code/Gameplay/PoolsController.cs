namespace Game.Gameplay
{
    //Managing clearing scene from poolable objects OnRestart
    public class PoolsController : Zenject.IInitializable, System.IDisposable
    {
        [Zenject.Inject] ProjectilesPool projectilesPool = null;
        [Zenject.Inject] EnemyPool enemyPool = null;
        [Zenject.Inject] MiniBossPool miniBossPool = null;
        [Zenject.Inject] GameplayController gameplayCtrl = null;


        public void Initialize()
        {
            gameplayCtrl.OnLevelRestarting += OnRestart;
        }

        public void Dispose()
        {
            gameplayCtrl.OnLevelRestarting -= OnRestart;
        }

        void OnRestart()
        {
            projectilesPool.DespawnAll();
            enemyPool.DespawnAll();
            miniBossPool.DespawnAll();
        }
    }
}