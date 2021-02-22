using UnityEngine;

namespace Game.Gameplay
{
    //just input
    public class InputController : MonoBehaviour
    {
        [Zenject.Inject] GameplayController gameplayCtrl = null;
        float horizontal = 0;
        bool fire;

        //debug
        bool restart;
        bool destroyAll;

        public float Horizontal => horizontal;
        public bool Fire => fire;

        public System.Action Restart;
        public System.Action KillAllEnemies;
        public System.Action GodModePressed;

        void Update()
        {
            restart = false;
            destroyAll = false;
            horizontal = 0;
            fire = false;

            restart = Input.GetKeyDown(KeyCode.F1);

            if (restart)
            {
                if (Restart != null)
                    Restart();
            }

            //cluttered code
            if (gameplayCtrl.CurrentGameplayState == EGameplayState.Playing)
            {
                #region DEBUGS
                destroyAll = Input.GetKeyDown(KeyCode.F2);

                if (Input.GetKeyDown(KeyCode.F3))
                {
                    if (GodModePressed != null)
                        GodModePressed();
                }
                #endregion

                horizontal = Input.GetAxisRaw("Horizontal");
                fire = Input.GetButtonDown("Fire1");

                if (destroyAll)
                {
                    if (KillAllEnemies != null)
                        KillAllEnemies();
                }
            }

        }
    }
}