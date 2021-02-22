using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayController gameplayController = null;
        [SerializeField] private InputController inputController = null;
        [SerializeField] private UnitDefinitionsProvider unitDefinitionsProvider = null;
        [SerializeField] PlayableArea playableArea = null;
        [SerializeField] WaveManager waveManager = null;


        public override void InstallBindings()
        {
            Container.Bind<PlayerController>().AsSingle();

            Container.Bind<InputController>().FromInstance(inputController);
            Container.Bind<PlayableArea>().FromInstance(playableArea);
            Container.Bind<UnitDefinitionsProvider>().FromInstance(unitDefinitionsProvider);

            Container.BindInterfacesAndSelfTo<GlobalShootingAI>().AsSingle();
            Container.BindInterfacesAndSelfTo<PointsManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<PoolsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<WaveProgressController>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<WaveManager>().FromInstance(waveManager);
            Container.BindInterfacesAndSelfTo<GameplayController>().FromInstance(gameplayController);
        }
    }
}