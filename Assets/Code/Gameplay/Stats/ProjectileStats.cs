using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Projectile")]
public class ProjectileStats : ScriptableObject
{
    [SerializeField] float damage = 1;
    [SerializeField] int pointsForDestroying = 5;

    public float Damage => damage;

    public int PointsForDestroying => pointsForDestroying;
}
