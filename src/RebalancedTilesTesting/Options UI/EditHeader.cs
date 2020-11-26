using AzeLib.Extensions;
using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RebalancedTilesTesting.OptionsUI
{
    public class EditHeader : ConnectedUIComponent<OptionsScreen>
    {
        private LocText locText;

        public EditHeader(OptionsScreen options) : base(options) => options.editHeader = this;

        public override IUIComponent GetUIComponent()
        {
            var resetButton = new PButton()
            {
                Text = "Reset",
                OnClick = (GameObject src) => link.editBody.ResetToDefault(src)
            };

            var title = new PLabel()
            {
                Text = " ",  // Must be something or the object isn't created.
                TextStyle = PUITuning.Fonts.UIDarkStyle.DeriveStyle(25, null, FontStyles.Bold),
                TextAlignment = TextAnchor.MiddleLeft,
                FlexSize = Vector2.one,
                DynamicSize = true
            }
            .AddOnRealize(OnRealized);

            return new PPanel()
            {
                Direction = PanelDirection.Horizontal,
                Margin = new(5, 5, 5, 5),
                BackColor = new Color32(245, 245, 245, 255),
                FlexSize = Vector2.one,
                Alignment = TextAnchor.LowerRight
            }
            .AddChild(title)
            .AddChild(resetButton);
        }

        public void SetTitle(BuildingDef def) => locText.text = def.GetRawName();

        private void OnRealized(GameObject realized)
        {
            locText = realized.GetComponentInChildren<LocText>();
            locText.overflowMode = TextOverflowModes.Truncate;
            locText.alignment = TextAlignmentOptions.Left; // Not sure why this has to be set here?

            var le = locText.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = le.minWidth = 300f;
            le.layoutPriority = 5;
            LayoutRebuilder.ForceRebuildLayoutImmediate(le.rectTransform());
        }
    }
}
