using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    public class ModEntryUI : IUISource
    {
        public const float entryTextMaxLength = 300f;

        protected readonly ModUIExtract mod;

        public ModEntryUI(ModUIExtract mod)
        {
            this.mod = mod;
        }

        public IUIComponent GetUIComponent()
        {
            var title = new PButton()
            {
                FlexSize = Vector2.right,
                Text = mod.Title.text,
                TextAlignment = TextAnchor.MiddleLeft
            }
            .AddOnRealize(go =>
            {
                var locText = go.GetComponentInChildren<LocText>();
                locText.overflowMode = TextOverflowModes.Truncate;
                locText.alignment = TextAlignmentOptions.Left;

                var le = locText.gameObject.AddOrGet<LayoutElement>();
                // Set preferred width so that the LocText knows where to truncate.
                // Set min width so that even if no strings are long, UI is sized correctly.
                le.preferredWidth = le.minWidth = entryTextMaxLength;
            })
            // Locking the layout here ensures that nothing can override the widths just before.
            // Without this, the UI will try to flex to fit the largest text string.
            .LockLayout();

            return new PPanel()
            {
                FlexSize = Vector2.one,
                Direction = PanelDirection.Horizontal,
                Margin = new(1, 1, 1, 1)
            }
            .AddChild(title);
        }
    }
}
