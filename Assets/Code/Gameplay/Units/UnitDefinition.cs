using UnityEngine;

namespace Game.Gameplay
{
    //it was supposed to be used as an entry point for spawning and setting stats, but "supposed to" is adequate
    [CreateAssetMenu(menuName = "Unit definition")]
    public class UnitDefinition : ScriptableObject
    {
        [SerializeField] Unit prefab = null;
        [SerializeField] UnitStats stats = null;

        public Unit Prefab => prefab;
        public UnitStats Stats => stats;
    }
}