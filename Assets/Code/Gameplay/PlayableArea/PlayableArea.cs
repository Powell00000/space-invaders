using UnityEngine;

namespace Game.Gameplay
{
    //As name suggests, helper class for getting Playable area bounds, sizes, spawn points
    public class PlayableArea : MonoBehaviour
    {
        //can be set from inspector, Camera keeps transforms in view
        [SerializeField] Transform left = null;
        [SerializeField] Transform right = null;
        [SerializeField] Transform top = null;
        [SerializeField] Transform bottom = null;

        Bounds gameBounds;

        public Bounds GameBounds => gameBounds;

        public Vector3 PlayerSpawnPosition => bottom.position + Vector3.up * 3;
        public Vector3 TopGridSpawnPosition => top.position - Vector3.up * 3;

        public float Width => Mathf.Abs(left.position.x) + Mathf.Abs(right.position.x);
        public float PlayableHeight => Vector3.Distance(PlayerSpawnPosition + Vector3.up * 5, TopGridSpawnPosition - Vector3.up * 2);

        void CalculateBounds()
        {
            gameBounds = new Bounds(transform.position, Vector3.zero);
            gameBounds.Encapsulate(left.transform.position);
            gameBounds.Encapsulate(right.transform.position);
            gameBounds.Encapsulate(top.transform.position);
            gameBounds.Encapsulate(bottom.transform.position);
        }

#if UNITY_EDITOR

        bool TransformsValid()
        {
            if (left == null || right == null || top == null || bottom == null)
                return false;
            else
                return true;
        }

        private void OnValidate()
        {
            if (!TransformsValid())
                return;
            CalculateBounds();
        }

        private void OnDrawGizmos()
        {
            if (!TransformsValid())
                return;
            CalculateBounds();
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(gameBounds.center, gameBounds.size);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(PlayerSpawnPosition, 1);

        }
#endif
    }
}