using Game.Gameplay;
using UnityEngine;

namespace Game.UI
{
    //Controlls visibility of views
    public class UIController : MonoBehaviour, Zenject.IInitializable, System.IDisposable
    {
        [Zenject.Inject] GameplayController gameplayCtrl = null;

        //TODO: REWORK to base class GUIView
        [SerializeField] GameObject warmup = null;
        [SerializeField] GameObject gameplay = null;
        [SerializeField] GameObject gameOver = null;

        public void Initialize()
        {
            gameplayCtrl.OnStateChanged += GameStateChanged;
        }

        void Start()
        {
            GameStateChanged(gameplayCtrl.CurrentGameplayState);
        }

        void GameStateChanged(EGameplayState state)
        {
            warmup.SetActive(gameplayCtrl.CurrentGameplayState == EGameplayState.Warmup);
            gameplay.SetActive(gameplayCtrl.CurrentGameplayState == EGameplayState.Playing);
            gameOver.SetActive(gameplayCtrl.CurrentGameplayState == EGameplayState.GameOver);
        }

        public void Dispose()
        {
            if (gameplayCtrl != null)
                gameplayCtrl.OnStateChanged -= GameStateChanged;
        }
    }
}