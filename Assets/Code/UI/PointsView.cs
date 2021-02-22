using Game.Gameplay;
using UnityEngine;

namespace Game.UI
{
    //base class for visualizing points, should be more abstract
    //show points on end screen
    public class PointsView : MonoBehaviour
    {
        [Zenject.Inject] protected PointsManager pointsMgr;
        [SerializeField] protected TMPro.TextMeshProUGUI pointsLabel;

        protected virtual void OnEnable()
        {
            SetLabelValue();
        }

        protected virtual void OnDisable()
        {

        }

        protected void SetLabelValue()
        {
            pointsLabel.text = pointsMgr.CurrentPoints.ToString();
        }
    }
}