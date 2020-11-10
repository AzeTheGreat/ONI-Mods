using AzeLib.Extensions;
using PeterHan.PLib.UI;
using UnityEngine;

namespace RebalancedTilesTesting.OptionsUI
{
    public class EditHeader : ConnectedUIComponent<OptionsScreen>
    {
        private LocText locText;

        public EditHeader(OptionsScreen options) : base(options) => options.editHeader = this;

        public override IUIComponent GetUIComponent()
        {
            return new PLabel()
            {
                Text = "Currently Selected: "
            }
            .AddOnRealize((GameObject realized) => locText = realized.GetComponentInChildren<LocText>());
        }

        public void SetTitle(UIConfigOptions uiConfigOptions) => locText.text = uiConfigOptions.GetName();
    }
}
