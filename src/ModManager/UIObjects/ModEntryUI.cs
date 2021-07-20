using PeterHan.PLib.UI;
using UnityEngine;

namespace ModManager
{
    public class ModEntryUI : IUISource
    {
        protected readonly ModUIExtract mod;

        public ModEntryUI(ModUIExtract mod)
        {
            this.mod = mod;
        }

        public IUIComponent GetUIComponent()
        {
            var title = new PLabel()
            {
                FlexSize = Vector2.right,
                Text = mod.Title.text,
                TextAlignment = TextAnchor.MiddleLeft
            };

            var version = new PLabel()
            {
                Text = mod.Version.text,
                TextAlignment = TextAnchor.MiddleRight
            };

            return new PPanel()
            {
                FlexSize = Vector2.one,
                Direction = PanelDirection.Horizontal,
                Margin = new(1, 1, 1, 1)
            }
            .AddChild(title)
            .AddChild(version);
        }
    }
}
