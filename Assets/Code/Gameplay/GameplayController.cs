using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    //base class for setting gameplay states and maintaining basic flow
    //gives lots of events for other systems
    public class GameplayController : MonoBehaviour, IInitializable, IDisposable
    {
        [Inject] PlayerController playerCtrl = null;
        [Inject] WaveManager waveManager = null;
        [Inject] InputController inputCtrl = null;

        GameplayStateMachine gameplayState;
        public Action OnLevelStarting;
        public Action OnLevelRestarting;
        public Action<EGameplayState> OnStateChanged;

        public EGameplayState CurrentGameplayState => gameplayState.CurrentState;

        //wait for a moment for game start
        MEC.CoroutineHandle warmupCoroutineHandle;

        void IInitializable.Initialize()
        {
            gameplayState = new GameplayStateMachine();
            gameplayState.OnStateEnter += StateChanged;

            playerCtrl.OnPlayerDied += GameOver;
            waveManager.OnAllWavesFinished += GameOver;
            inputCtrl.Restart += Restart;

            Warmup();
        }

        private void Warmup()
        {
            gameplayState.ChangeState(EGameplayState.Warmup);

            playerCtrl.MaintainPlayerSpawn();

            warmupCoroutineHandle = MEC.Timing.CallDelayed(2f, StartPlaying);
        }

        private void StartPlaying()
        {
            gameplayState.ChangeState(EGameplayState.Playing);

            if (OnLevelStarting != null)
                OnLevelStarting();
        }

        public void Restart()
        {
            if (CurrentGameplayState == EGameplayState.Warmup)
                return;

            if (OnLevelRestarting != null)
                OnLevelRestarting();

            Warmup();
        }

        //both winning and losing
        private void GameOver()
        {
            gameplayState.ChangeState(EGameplayState.GameOver);
        }

        void StateChanged(EGameplayState state)
        {
            if (OnStateChanged != null)
                OnStateChanged(state);
        }

        public void Dispose()
        {
            if (playerCtrl != null)
                playerCtrl.OnPlayerDied -= GameOver;
            if (waveManager != null)
                waveManager.OnAllWavesFinished += GameOver;
            if (inputCtrl != null)
                inputCtrl.Restart += Restart;
        }
    }
}