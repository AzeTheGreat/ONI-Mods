using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    class InfoPanelUI : UISource
    {
        public const float titleMaxTextLength = 400f;

        public string TitleText { get; set; } = " ";
        public string BodyText { get; set; } = " ";

        protected override IUIComponent GetUIComponent()
        {
            var title = new PLabel()
            {
                FlexSize = Vector2.right,
                Text = TitleText,
                TextAlignment = TextAnchor.MiddleCenter
            }
            .AddOnRealize(ConstrainTextLength)
            .LockLayout();

            var body = new PTextArea()
            {
                FlexSize = Vector2.one,
                Text = BodyText
            };

            return new PPanel("InfoPanel")
            {
                FlexSize = Vector2.one,
                Direction = PanelDirection.Vertical
            }
            .AddChild(title)
            .AddChild(body)
            .AddOnRealize(AddModEntrySelectedTarget);

            void AddModEntrySelectedTarget(GameObject go)
            {
                var target = go.AddComponent<ModEntrySelectedTarget>();
                target.Instance = this;
            }
        }

        // TODO: This is largely duplicated from ModEntryUI - extract and refactor.
        private void ConstrainTextLength(GameObject go)
        {
            var locText = go.GetComponentInChildren<LocText>();
            locText.overflowMode = TextOverflowModes.Truncate;

            var le = locText.gameObject.AddOrGet<LayoutElement>();
            // Set preferred width so that the LocText knows where to truncate.
            // Set min width so that even if no strings are long, UI is sized correctly.
            le.preferredWidth = le.minWidth = titleMaxTextLength;
        }

        private class ModEntrySelectedTarget : MonoBehaviour, ModEntryUI.IOnClick
        {
            public InfoPanelUI Instance { get; set; }

            public void OnClick(ModUIExtract mod)
            {
                Instance.TitleText = mod.Mod.title;
                Instance.BodyText = mod.Mod.description;
                Instance.RebuildGO();
            }
        }
    }
}
