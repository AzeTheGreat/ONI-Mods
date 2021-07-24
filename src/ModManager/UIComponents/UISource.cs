using PeterHan.PLib.UI;
using UnityEngine;

namespace ModManager
{
    public abstract class UISource
    {
        public GameObject GO { get; private set; }

        public IUIComponent CreateUIComponent() => GetUIComponent().AddOnRealize(go => GO = go);

        public void DestroyGO()
        {
            if(GO != null)
            {
                Object.Destroy(GO);
                // Explicitly setting to null helps avoid any issues with lingering managed objects.
                GO = null;
            }
        }

        protected abstract IUIComponent GetUIComponent();
    }
}
