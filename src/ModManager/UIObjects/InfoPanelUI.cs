using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    class InfoPanelUI : UISource
    {
        public const float titleMaxTextLength = 400f;

        protected GameObject titleGO;
        protected GameObject bodyGO;

        protected override IUIComponent GetUIComponent()
        {
            var title = new PLabel()
            {
                FlexSize = Vector2.right,
                Text = " ",
                TextAlignment = TextAnchor.MiddleCenter
            }
            .AddOnRealize(ConstrainTextLength)
            .AddOnRealize(go => titleGO = go)
            .LockLayout();

            var body = new PTextArea()
            {
                FlexSize = Vector2.one,
                Text = " "
            }
            .AddOnRealize(go => bodyGO = go);

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
                PUIElements.SetText(Instance.titleGO, mod.Mod.title);
                PUIElements.SetText(Instance.bodyGO, mod.Mod.description);
            }
        }
    }
}
