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
                FlexSize = Vector2.right,
                Spacing = 0,
                Margin = new RectOffset(5, 5, 5, 5),
                Children = Assets.BuildingDefs,
                ChildFactory = CreateChild
            };

            return new PScrollPane()
            {
                BackColor = new Color32(50, 50, 50, 255),
                Child = scrollPanel,
                ScrollHorizontal = false,
                ScrollVertical = true,
                FlexSize = Vector2.one,
                TrackSize = 8f
            };
        }

        public void SetChildren(List<BuildingDef> children) => scrollPanel.UpdateChildren(children);

        private IUIComponent CreateChild(BuildingDef def) => new PButton()
        {
            Text = def.GetRawName(),
            TextAlignment = TextAnchor.MiddleLeft,
            FlexSize = Vector2.right,
            DynamicSize = false,
            OnClick = (GameObject src) => Edit(def)
        }
        .SetKleiBlueStyle();

        private void Edit(BuildingDef def)
        {
            link.editBody.SetChildren(def);
            link.editHeader.SetTitle(def);
        }
    }
}
