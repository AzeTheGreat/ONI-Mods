using AzeLib.Extensions;
using PeterHan.PLib.UI;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RebalancedTilesTesting.OptionsUI
{
    public class SearchHeader : ConnectedUIComponent<OptionsScreen>
    {
        private GameObject checkBox;
        private TMP_InputField textField;

        public SearchHeader(OptionsScreen options) : base(options) => options.searchHeader = this;

        public override IUIComponent GetUIComponent()
        {
            var label = new PLabel()
            {
                Text = "Search Database:",
                TextAlignment = TextAnchor.MiddleLeft
            };

            var searchField = new PTextField()
            {
                Text = string.Empty,
                FlexSize = Vector2.right,
                TextAlignment = TextAlignmentOptions.Left
            }
            .AddOnRealize((GameObject realized) =>
            {
                textField = realized.GetComponent<TMP_InputField>();
                textField.onValueChanged.AddListener(UpdateSearchResults);
            });

            var filterCheckbox = new PCheckBox()
            {
                OnChecked = UpdateCheckedState
            }
            .AddOnRealize((GameObject realized) => checkBox = realized);

            var searchPanel = new PPanel()
            {
                Direction = PanelDirection.Horizontal,
                Spacing = 5,
                FlexSize = Vector2.right
            }
            .AddChild(searchField)
            .AddChild(filterCheckbox);

            return new PPanel()
            {
                BackColor = new Color32(50, 50, 50, 255),
                Direction = PanelDirection.Vertical,
                Spacing = 5,
                Margin = new RectOffset(5, 5, 5, 5),
                FlexSize = Vector2.right,
                Alignment = TextAnchor.MiddleLeft
            }
            .AddChild(label)
            .AddChild(searchPanel);
        }

        private void UpdateCheckedState(GameObject realized, int state)
        {
            PCheckBox.SetCheckState(realized, state == 0 ? 1 : 0);
            UpdateSearchResults(textField.text);
        }

        private void UpdateSearchResults(string text)
        {
            var newResults = Assets.BuildingDefs.Where(x => x.GetRawName().Contains(text)).ToList();
            if (PCheckBox.GetCheckState(checkBox) == 1)
                newResults = newResults.Where(x => Options.Opts.configOptions.DefIsModified(x)).ToList();

            link.searchBody.SetChildren(newResults);
        }
    }
}
