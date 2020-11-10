using AzeLib.Extensions;
using PeterHan.PLib.UI;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RebalancedTilesTesting.OptionsUI
{
    public class SearchHeader : ConnectedUIComponent<OptionsScreen>
    {
        public SearchHeader(OptionsScreen options) : base(options) => options.searchHeader = this;

        public override IUIComponent GetUIComponent()
        {
            var searchField = new PTextField()
            {
                Text = "Search...",
                FlexSize = Vector2.right,
                TextAlignment = TextAlignmentOptions.Left
            }
            .AddOnRealize((GameObject realized) =>
                realized.GetComponent<TMP_InputField>().onValueChanged.AddListener(UpdateSearchResults));

            var filterCheckbox = new PCheckBox()
            {
                OnChecked = (GameObject realized, int state) => {
                    PCheckBox.SetCheckState(realized, state == 0 ? 1 : 0);
                }
            };

            return new PPanel()
            {
                BackColor = Color.green,
                Direction = PanelDirection.Horizontal,
                Spacing = 10,
                Margin = new RectOffset(5, 5, 5, 5),
                FlexSize = Vector2.right
            }
            .AddChild(searchField)
            .AddChild(filterCheckbox);
        }

        private void UpdateSearchResults(string text)
        {
            var newResults = Options.Opts.UIConfigOptions
                .Where(x => x.Value.GetName().Contains(text))
                .Cast<object>().ToList();
            link.searchBody.SetChildren(newResults);
        }
    }
}
