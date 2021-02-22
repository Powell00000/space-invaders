using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Units/Player")]
public class PlayerStats : UnitStats
{
    [SerializeField] float speed = 10;

    public float Speed => speed;
}
