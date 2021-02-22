namespace Game.UI
{
    //class for showing points during gameplay
    public class PointsViewInGame : PointsView
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            pointsMgr.OnPointsChanged += PointsChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            pointsMgr.OnPointsChanged -= PointsChanged;
        }

        void PointsChanged(int points)
        {
            SetLabelValue();
        }
    }
}