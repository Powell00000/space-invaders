using UnityEngine;

namespace Game.Gameplay
{
    //object for setting enemies locations (Grid) as well as bosses

    [CreateAssetMenu(menuName = "WaveHolder")]
    public class WaveHolder : ScriptableObject
    {
        [SerializeField] GridParent gridParent = null;
        [SerializeField] bool autoPositionRows = true;
        [SerializeField, Range(0, 4)] int miniBossCount = 0;

        //GridParent contains GridAreas for enemies to occupy
        public GridParent GridParent => gridParent;
        public bool AutoPositionRows => autoPositionRows;
        public int MiniBossCount => miniBossCount;
    }
}