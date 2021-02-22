using System;
using UnityEngine;

namespace Game.Gameplay
{
    public abstract class Unit : MonoBehaviour, IUnit
    {
        //TODO: create system for updating units during gameplay state
        [Zenject.Inject] GameplayController gameplayCtrl = null;

        [SerializeField] protected BoxCollider2D boxCollider2d = null;
        //TODO: strong typing
        [SerializeField] protected UnitStats stats = null;
        [SerializeField] protected MeshRenderer meshRenderer = null;

        protected float currentHealth;
        protected EFaction faction;

        //TODO: Change System.Action into custom events implementation for automatic refs cleanup
        public Action OnDamageReceived;
        public Action<Unit> OnDeath;

        public BoxCollider2D BoxCollider2D => boxCollider2d;
        public UnitStats Stats => stats;
        public float CurrentHealth => currentHealth;

        EFaction IUnit.Faction => faction;

        float shootTimer;
        float shootMaxTime;
        protected bool CanShoot => shootTimer >= shootMaxTime;

        protected virtual void Death()
        {
            if (OnDeath != null)
                OnDeath(this);
        }

        private void Start()
        {
            //just to be sure
            Initialize();
        }

        protected virtual void Initialize()
        {
            currentHealth = stats.Health;

            meshRenderer.SetEmission(stats.Color);
            shootMaxTime = stats.ShootTime;
            shootTimer = 0;
        }

        protected virtual void ClearEvents()
        {
            OnDamageReceived = null;
            OnDeath = null;
        }

        void IUnit.ReceiveDamage(float amount)
        {
            if (OnDamageReceived != null)
                OnDamageReceived();

            CalculateHealthLeft(amount);
            if (currentHealth <= 0)
            {
                Death();
            }
        }

        //for custom implementation (GODMODE)
        protected virtual void CalculateHealthLeft(float damageAmount)
        {
            currentHealth -= damageAmount;
        }

        protected abstract void MovementFunction();

        //TODO: create system for updating units during gameplay state
        protected virtual void Update()
        {
            if (gameplayCtrl.CurrentGameplayState != EGameplayState.Playing)
                return;
            CalculateShootTime();
            MovementFunction();
        }

        void CalculateShootTime()
        {
            if (shootTimer < shootMaxTime)
                shootTimer += Time.deltaTime;
        }

        public void ForceDeath()
        {
            Death();
        }

        public void ShootIfCan()
        {
            if (!CanShoot)
                return;
            Shoot();
            shootTimer = 0;
        }

        protected abstract void Shoot();
    }
}