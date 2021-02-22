using UnityEngine;

namespace Game.UI
{
    public class UIInstaller : Zenject.MonoInstaller
    {
        [SerializeField] UIController uiController = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIController>().FromInstance(uiController);
        }
    }
}