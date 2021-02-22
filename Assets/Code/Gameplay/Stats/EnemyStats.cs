using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Units/Enemy")]
public class EnemyStats : UnitStats
{
    [SerializeField] float flySpeed = 5;
    [SerializeField] float movementSpeed = 5;

    public float FlySpeed => flySpeed;
    public float MoveSpeed => movementSpeed;

}
