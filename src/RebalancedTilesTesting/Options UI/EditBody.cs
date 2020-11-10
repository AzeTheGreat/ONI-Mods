using AzeLib.Extensions;
using PeterHan.PLib;
using PeterHan.PLib.UI;
using UnityEngine;

namespace RebalancedTilesTesting.OptionsUI
{
    public class EditBody : ConnectedUIComponent<OptionsScreen>
    {
        private GameObject panel;
        
        public EditBody(OptionsScreen options) : base(options) => options.editBody = this;

        public override IUIComponent GetUIComponent()
        {
            return OptionsScreen.GetScrollPaneLayout(new PPanel()
            {
                BackColor = Color.clear,
                Spacing = 5,
                Margin = new RectOffset(5, 5, 5, 5)
            }
            .AddOnRealize((GameObject realized) => panel = realized));
        }

        public void SetChildren(UIConfigOptions uiConfigOptions)
        {
            foreach (var option in uiConfigOptions.GetOptions())
                option.GetUIComponent().SetParent(link.editBody.panel);
        }
    }
}
