using PeterHan.PLib;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System.Reflection;
using UnityEngine;

namespace RebalancedTilesTesting
{
    public class EmbeddedOptions : OptionsEntry
    {
        private object ignore;
        public override object Value { get => ignore; set => ignore = value; }

        public EmbeddedOptions() : base(string.Empty, string.Empty, string.Empty) { }

        public override GameObject GetUIComponent()
        {
            // CONSTANTS
            var margins = new RectOffset(5, 5, 5, 5);

            // SEARCH FIELD
            var searchField = new PTextField()
            {
                Text = "Search...",
                MinWidth = 300,
                TextAlignment = TMPro.TextAlignmentOptions.Left
            };

            var clearSearchButton = new PButton()
            {
                Text = "Clear"
            };
            
            var searchPanel = new PPanel()
            {
                BackColor = Color.green,
                Direction = PanelDirection.Horizontal,
                Spacing = 10,
                Margin = margins
            }
            .AddChild(searchField)
            .AddChild(clearSearchButton);

            // SEARCH RESULTS
            var test = new PLabel()
            {
                Text = "Building",
                TextAlignment = TextAnchor.LowerLeft,
                BackColor = Color.clear
            };

            var searchResultsPanel = new PPanel()
            {
                Alignment = TextAnchor.MiddleLeft,
                BackColor = Color.clear,
                Spacing = 5,
                Margin = margins
            }
            .AddChild(test).AddChild(test).AddChild(test).AddChild(test).AddChild(test).AddChild(test)
            .AddChild(test).AddChild(test).AddChild(test).AddChild(test).AddChild(test).AddChild(test)
            .AddChild(test).AddChild(test).AddChild(test).AddChild(test).AddChild(test).AddChild(test)
            .AddChild(test).AddChild(test).AddChild(test).AddChild(test).AddChild(test).AddChild(test);

            var searchResults = new PScrollPane()
            {
                Child = searchResultsPanel,
                ScrollHorizontal = false,
                ScrollVertical = true,
                BackColor = Color.green,
                FlexSize = Vector2.zero,
                TrackSize = 8f
            };

            // SELECTED
            var selectedLabel = new PLabel()
            {
                Text = "Currently Selected: ",
                BackColor = Color.green,
                FlexSize = Vector2.right,
                Margin = margins
            };

            // MODIFY SELECTED
            var modificationPanel = new PPanel()
            {
                BackColor = Color.clear,
                Spacing = 5,
                Margin = margins
            };

            var modifications = new PScrollPane()
            {
                Child = modificationPanel,
                BackColor = Color.green,
                FlexSize = Vector2.right
            };

            // MAIN PANEL
            var mainPanel = new PPanel()
            {
                BackColor = Color.blue,
                DynamicSize = true,
                Direction = PanelDirection.Vertical,
                Alignment = TextAnchor.MiddleCenter,
                Spacing = 10,
                Margin = margins
            };

            return mainPanel
                .AddChild(searchPanel)
                .AddChild(searchResults)
                .AddChild(selectedLabel)
                .AddChild(modifications)
                .Build();
        }
    }
}