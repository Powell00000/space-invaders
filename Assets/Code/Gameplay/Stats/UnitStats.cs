using UnityEngine;

//base class for unit stats
public abstract class UnitStats : ScriptableObject
{
    [SerializeField] private float health = 1;
    [SerializeField] int pointsForDestroying = 10;
    [SerializeField] Color color = Color.black;

    [SerializeField] float baseProjectileSpeed = 10;
    [SerializeField] float shootTime = 1;

    public float Health => health;
    public int PointsForDestroying => pointsForDestroying;
    public Color Color => color;

    public float BaseProjectileSpeed => baseProjectileSpeed;
    public float ShootTime => shootTime;

}
