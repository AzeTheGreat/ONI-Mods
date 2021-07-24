using PeterHan.PLib.UI;
using UnityEngine;

namespace ModManager
{
    public abstract class UISource
    {
        public GameObject GO { get; private set; }

        public IUIComponent CreateUIComponent() => GetUIComponent().AddOnRealize(go => GO = go);

        protected abstract IUIComponent GetUIComponent();
    }
}
