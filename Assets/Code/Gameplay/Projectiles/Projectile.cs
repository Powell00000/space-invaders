using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    //Should be a base for all types of projectiles, but right now there is only one
    public class Projectile : MonoBehaviour, IPoolable<Projectile.SpawnContext, ProjectilesPool>
    {
        [SerializeField] private ProjectileStats stats = null;
        [SerializeField] BoxCollider2D boxCollider2d = null;
        [SerializeField] MeshRenderer meshRenderer = null;

        //playable area should keep track of out of bounds objects?
        [Inject] PlayableArea playableArea = null;
        [Inject] GameplayController gameplayCtrl = null;
        [Inject] ProjectileHitFxPool hitFxPool = null;
        //this...should not be here
        [Inject] PointsManager pointsManager = null;

        //projectiles also can be friendly!
        private EFaction faction;
        private ProjectilesPool attachedPool;
        private SpawnContext context;

        ContactFilter2D contactFilter;

        //stash collision results for reusing
        Collider2D[] contactResults = new Collider2D[1];

        //yeah, base class for all projectiles
        protected EFaction Faction => faction;

        public void OnDespawned()
        {
            //hack for spawning outside of bounds, Zenject.Poolable flow has wrong order of enabling objects before setting ther positions
            transform.position = Vector3.zero;
        }

        public void OnSpawned(SpawnContext spawnContext, ProjectilesPool projectilesPool)
        {
            contactFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                useTriggers = true
            };

            faction = spawnContext.Instigator.Faction;

            meshRenderer.SetEmission(spawnContext.Color);

            //manage parameters regarding instigator faction
            switch (spawnContext.Instigator.Faction)
            {
                case EFaction.Player:
                    contactFilter.layerMask = Layers.Bit.Enemy | Layers.Bit.EnemyProjectile;
                    this.gameObject.layer = Layers.Int.PlayerProjectile;
                    break;
                case EFaction.Enemy:
                    contactFilter.layerMask = Layers.Bit.Player | Layers.Bit.PlayerProjectile;
                    this.gameObject.layer = Layers.Int.EnemyProjectile;
                    break;
                default:
                    break;
            }
            attachedPool = projectilesPool;
            context = spawnContext;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, context.Direction);
            transform.position = context.Position;
        }

        //TODO: move spawning of projectiles to helper class
        //or create a "spawnable pickable"
        void SpawnFourProjectiles()
        {
            Vector3 dir = Vector3.up;
            Quaternion rotation = Quaternion.Euler(0, 0, 45);

            dir = rotation * dir;
            rotation = Quaternion.Euler(0, 0, 90);
            for (int i = 0; i < 4; i++)
            {
                attachedPool.Spawn(new SpawnContext(
                    transform.position,
                    context.Instigator,
                    context.Speed,
                    dir,
                    context.Color
                    ));
                dir = rotation * dir;
            }
        }

        void Update()
        {
            //TODO: create custom update to be updated during gameplay
            if (gameplayCtrl.CurrentGameplayState == EGameplayState.Playing)
                transform.position += context.Direction * context.Speed * Time.deltaTime;
        }

        void FixedUpdate()
        {
            //can't even...
            if (!playableArea.GameBounds.Contains(transform.position))
            {
                attachedPool.Despawn(this);
            }

            //WARNING - lots of bugs when can collide with their own layer! (ignores moer than 1 collision)
            int hits = boxCollider2d.OverlapCollider(contactFilter, contactResults);

            if (hits > 0)
            {
                bool isUnit = contactResults[0].TryGetComponent<IUnit>(out var unit);
                if (isUnit)
                {
                    //no friendly fire
                    if (unit.Faction != faction)
                    {
                        unit.ReceiveDamage(stats.Damage);
                        Destroy();
                    }
                }
                else
                {
                    if (faction == EFaction.Enemy)
                        return;

                    //checking collision of projectiles from different layes/instigators/factions
                    bool isProjectile = contactResults[0].TryGetComponent<Projectile>(out var projectile);
                    if (isProjectile)
                    {
                        //TODO: move to other system!
                        //points for player
                        pointsManager.AddPoints(stats.PointsForDestroying);
                        SpawnFourProjectiles(); // REWORK!
                        projectile.Destroy();
                        this.Destroy();
                    }
                }
            }
        }

        void Destroy()
        {
            hitFxPool.Spawn(new ProjectileHitFx.SpawnContext()
            {
                Position = transform.position,
                ColorHD = context.Color
            });
            attachedPool.Despawn(this);
        }

        public struct SpawnContext
        {
            private Vector3 position;
            private IUnit instigator;
            private float speed;
            private Vector3 direction;
            private Color32 color;

            public Vector3 Position => position;
            public IUnit Instigator => instigator;
            public float Speed => speed;
            public Vector3 Direction => direction;
            public Color32 Color => color;

            public SpawnContext(Vector3 position, IUnit instigator, float speed, Vector3 direction, Color32 color)
            {
                this.position = position;
                this.instigator = instigator;
                this.speed = speed;
                this.direction = direction;
                this.color = color;
            }
        }
    }
}