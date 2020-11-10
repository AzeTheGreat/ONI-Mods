using AzeLib.Extensions;
using PeterHan.PLib;
using PeterHan.PLib.UI;
using RebalancedTilesTesting.VirtualScroll;
using System.Collections.Generic;
using UnityEngine;

namespace RebalancedTilesTesting.OptionsUI
{
    public class SearchBody : ConnectedUIComponent<OptionsScreen>
    {
        private VirtualPanelChildManager childManager;

        public SearchBody(OptionsScreen options) : base(options) => options.searchBody = this;

        public override IUIComponent GetUIComponent()
        {
            var scrollPanel = new VirtualScrollPanel<KeyValuePair<string, UIConfigOptions>>()
            {
                Alignment = TextAnchor.UpperLeft,
                BackColor = Color.magenta,
                Spacing = 5,
                Margin = new RectOffset(5, 5, 5, 5),
                Children = Options.Opts.UIConfigOptions,
                ChildFactory = CreateChild
            }
            .AddOnRealize((GameObject realized) => childManager = realized.GetComponent<VirtualPanelChildManager>());

            return OptionsScreen.GetScrollPaneLayout(scrollPanel);
        }

        public void SetChildren(List<object> children) => childManager.UpdateChildren(children);

        private IUIComponent CreateChild(KeyValuePair<string, UIConfigOptions> kvp) => new PButton()
        {
            Text = kvp.Value.GetName(),
            DynamicSize = false,
            OnClick = (GameObject src) => Edit(kvp.Value)
        };

        private void Edit(UIConfigOptions uiConfigOptions)
        {
            link.editBody.SetChildren(uiConfigOptions);
            link.editHeader.SetTitle(uiConfigOptions);
        }
    }
}
