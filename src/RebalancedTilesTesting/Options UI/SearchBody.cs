using AzeLib.Extensions;
using PeterHan.PLib.UI;
using RebalancedTilesTesting.CustomUIComponents;
using System.Collections.Generic;
using UnityEngine;

namespace RebalancedTilesTesting.OptionsUI
{
    public class SearchBody : ConnectedUIComponent<OptionsScreen>
    {
        private VirtualScrollPanel<BuildingDef> scrollPanel;

        public SearchBody(OptionsScreen options) : base(options) => options.searchBody = this;

        public override IUIComponent GetUIComponent()
        {
            scrollPanel = new()
            {
                Alignment = TextAnchor.UpperLeft,
                BackColor = Color.magenta,
                Spacing = 5,
                Margin = new RectOffset(5, 5, 5, 5),
                Children = Assets.BuildingDefs,
                ChildFactory = CreateChild
            };

            return OptionsScreen.GetScrollPaneLayout(scrollPanel);
        }

        public void SetChildren(List<BuildingDef> children) => scrollPanel.UpdateChildren(children);

        private IUIComponent CreateChild(BuildingDef def) => new PButton()
        {
            Text = def.GetRawName(),
            DynamicSize = false,
            OnClick = (GameObject src) => Edit(def)
        };

        private void Edit(BuildingDef def)
        {
            link.editBody.SetChildren(def);
            link.editHeader.SetTitle(def);
        }
    }
}
