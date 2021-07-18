using PeterHan.PLib.UI;
using UnityEngine;

namespace ModManager
{
    public class ModEntryUI : IUISource
    {
        protected readonly KMod.Mod mod;

        public ModEntryUI(KMod.Mod mod)
        {
            this.mod = mod;
        }

        public IUIComponent GetUIComponent()
        {
            return new PLabel()
            {
                FlexSize = Vector2.right,
                Text = mod.title
            };
        }
    }
}
