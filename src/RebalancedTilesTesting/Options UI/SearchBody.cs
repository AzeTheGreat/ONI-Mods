using AzeLib.Extensions;
using PeterHan.PLib.UI;
using RebalancedTilesTesting.CustomUIComponents;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RebalancedTilesTesting.OptionsUI
{
    public class SearchBody : ConnectedUIComponent<OptionsScreen>
    {
        private VirtualScrollPanel scrollPanel;

        public SearchBody(OptionsScreen options) : base(options) => options.searchBody = this;

        public override IUIComponent GetUIComponent()
        {
            scrollPanel = new VirtualScrollPanel()
            {
                Alignment = TextAnchor.UpperLeft,
                FlexSize = Vector2.right,
                Spacing = 0,
                Margin = new RectOffset(5, 13, 5, 5),
                Children = Assets.BuildingDefs.Select(x => new ButtonSource(link, x)),
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

        public void SetChildren(List<BuildingDef> children) => scrollPanel.UpdateChildren(children.Select(x => new ButtonSource(link, x)));

        private class ButtonSource : ConnectedUIComponent<OptionsScreen>
        {
            private readonly BuildingDef def;
            public ButtonSource(OptionsScreen options, BuildingDef def) : base(options) => this.def = def;

            public override IUIComponent GetUIComponent() => new PButton()
            {
                Text = def.GetRawName(),
                TextAlignment = TextAnchor.MiddleLeft,
                FlexSize = Vector2.right,
                DynamicSize = true,
                OnClick = (GameObject _) => Edit()
            }
            .SetKleiBlueStyle()
            .AddOnRealize(ConstrainSize);

            private void ConstrainSize(GameObject button)
            {
                var locText = button.GetComponentInChildren<LocText>();
                locText.overflowMode = TextOverflowModes.Truncate;
                locText.alignment = TextAlignmentOptions.Left; // Not sure why this has to be set here?

                var le = locText.gameObject.AddComponent<LayoutElement>();
                le.preferredWidth = le.minWidth = 165f;
                le.layoutPriority = 5;
                LayoutRebuilder.ForceRebuildLayoutImmediate(le.rectTransform());
            }

            private void Edit()
            {
                link.editBody.SetChildren(def);
                link.editHeader.SetTitle(def);
            }
        }
    }
}
