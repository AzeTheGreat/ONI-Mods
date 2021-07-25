using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    public class ModEntryUI : UISource
    {
        public const float entryTextMaxLength = 300f;

        public ModUIExtract Mod { get; set; }
        public ADragMe.IDragListener DragListener { get; set; }

        protected bool isBeingDragged;

        public void SetDragState(bool isBeingDragged)
        {
            this.isBeingDragged = isBeingDragged;
            GO.GetComponent<Image>().color = GetPanelColor();
        }

        protected override IUIComponent GetUIComponent()
        {
            var clearColorStyle = ScriptableObject.CreateInstance<ColorStyleSetting>();
            clearColorStyle.activeColor = Color.clear;

            var title = new PButton()
            {
                FlexSize = Vector2.right,
                Text = Mod.Title.text,
                TextAlignment = TextAnchor.MiddleLeft,
                Color = clearColorStyle
            }
            .AddOnRealize(ConstrainTextLength)
            .AddOnRealize(AddDrag)
            // Locking the layout here ensures that nothing can override the widths just before.
            // Without this, the UI will try to flex to fit the largest text string.
            .LockLayout();

            return new PPanel()
            {
                FlexSize = Vector2.right,
                Direction = PanelDirection.Horizontal,
                Margin = new(1, 1, 1, 1),
                BackColor = GetPanelColor()
            }
            .AddChild(title);
        }

        protected Color GetPanelColor() => isBeingDragged ?  PUITuning.Colors.ComponentDarkStyle.disabledActiveColor : PUITuning.Colors.DialogDarkBackground;

        private void ConstrainTextLength(GameObject go)
        {
            var locText = go.GetComponentInChildren<LocText>();
            locText.overflowMode = TextOverflowModes.Truncate;
            locText.alignment = TextAlignmentOptions.Left;

            var le = locText.gameObject.AddOrGet<LayoutElement>();
            // Set preferred width so that the LocText knows where to truncate.
            // Set min width so that even if no strings are long, UI is sized correctly.
            le.preferredWidth = le.minWidth = entryTextMaxLength;
        }

        private void AddDrag(GameObject go)
        {
            var dm = go.AddComponent<ADragMe>();
            dm.Listener = DragListener;
        }
    }
}
