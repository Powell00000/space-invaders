using UnityEngine;

namespace Game.Gameplay
{
    public class Player : Unit
    {
        //TODO: rework
        [Zenject.Inject] ProjectilesPool projectilePool = null;
        [Zenject.Inject] PlayableArea playableArea = null;

        [Zenject.Inject] InputController inputCtrl = null;

        //TODO: STRONG TYPING!
        private PlayerStats playerStats => (PlayerStats)stats;

        //debug hax
        bool godMode = false;

        protected override void Initialize()
        {
            base.Initialize();

            inputCtrl.GodModePressed += SwitchGodMode;
        }

        void SwitchGodMode()
        {
            godMode = !godMode;

            if (godMode)
                meshRenderer.SetEmission(Color.green); //just for visibility
            else
                meshRenderer.SetEmission(stats.Color);
        }

        protected override void CalculateHealthLeft(float damageAmount)
        {
            if (!godMode)
                base.CalculateHealthLeft(damageAmount);
        }

        protected override void ClearEvents()
        {
            base.ClearEvents();
            if (inputCtrl != null)
                inputCtrl.GodModePressed -= SwitchGodMode;

        }
        private void OnDestroy()
        {
            ClearEvents();
        }

        protected override void Update()
        {
            base.Update();

            //there was supposed to be a feature with gun heat and cooldown,
            //to sometimes be able to shoot a series of projectiles, but...well, have fun
            if (inputCtrl.Fire)
                Shoot();

            ShootIfCan();
        }

        protected override void Shoot()
        {
            projectilePool.Spawn(
                new Projectile.SpawnContext(
                    transform.position,
                    this,
                    stats.BaseProjectileSpeed,
                    Vector3.up,
                    stats.Color
                ));
        }

        protected override void MovementFunction()
        {
            var horizontalInput = inputCtrl.Horizontal;

            var newPosition = transform.position + Vector3.right * horizontalInput * Time.deltaTime * playerStats.Speed;

            if (playableArea.GameBounds.Contains(newPosition))
                transform.position = newPosition;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            //TODO: strong typing
            if (stats != null)
            {
                if (playerStats == null)
                {
                    stats = null;
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
        }
#endif

    }
}