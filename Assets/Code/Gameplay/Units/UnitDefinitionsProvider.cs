using UnityEngine;

namespace Game.Gameplay
{
    public class UnitDefinitionsProvider : MonoBehaviour
    {
        [SerializeField] UnitDefinition player = null;
        [SerializeField] UnitDefinition enemy = null;

        public UnitDefinition Player => player;
        public UnitDefinition Enemy => enemy;
    }
}