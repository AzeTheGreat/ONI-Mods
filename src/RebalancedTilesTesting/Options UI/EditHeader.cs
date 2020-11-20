using AzeLib.Extensions;
using PeterHan.PLib.UI;
using TMPro;
using UnityEngine;

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
                OnClick = link.editBody.ResetToDefault()
            };

            var title = new PLabel()
            {
                Text = " ",  // Must be something or the object isn't created.
                TextStyle = PUITuning.Fonts.UIDarkStyle.DeriveStyle(25, null, FontStyles.Bold),
                DynamicSize = true
            }
            .AddOnRealize((GameObject realized) => locText = realized.GetComponentInChildren<LocText>());

            return new PGridPanel()
            {
                BackColor = new Color32(245, 245, 245, 255),
                Margin = new(5, 5, 5, 5)
            }
            .AddColumn(new(0f, 0.5f))
            .AddColumn(new(0f, 0.5f))
            .AddRow(new(0f, 1f))
            .AddChild(title, new(0, 0) { Alignment = TextAnchor.MiddleLeft })
            .AddChild(resetButton, new(0, 1) { Alignment = TextAnchor.LowerRight });
        }

        public void SetTitle(BuildingDef def) => locText.text = def.GetRawName();
    }
}
