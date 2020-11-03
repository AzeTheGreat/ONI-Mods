using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System.Collections.Generic;
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

            var test2 = new PLabel()
            {
                Text = "Other Building",
                TextAlignment = TextAnchor.LowerLeft,
                BackColor = Color.clear
            };

            var searchResultsPanel = new VirtualScrollPanel()
            {
                Alignment = TextAnchor.UpperLeft,
                BackColor = Color.magenta,
                Spacing = 5,
                Margin = margins,
                Children = new List<IUIComponent>() { test, test2, test, test2, test, test2, test, test2, test, test2, test, test2,
                test, test, test, test, test, test, test, test, test, test, test, test,
                test, test, test, test, test, test, test, test, test, test, test, test,
                test, test, test, test, test, test, test, test, test, test, test, test,
                test, test, test, test, test, test, test, test, test, test, test, test,
                test, test, test, test, test, test, test, test, test2, test, test2, test2,}
            };

            var searchResultsPane = new PScrollPane()
            {
                Child = searchResultsPanel,
                ScrollHorizontal = false,
                ScrollVertical = true,
                BackColor = Color.green,
                FlexSize = Vector2.one,
                TrackSize = 8f
            };

            var searchResults = new PGridPanel()
            {
                FlexSize = Vector2.right,
                DynamicSize = true
            }
            .AddColumn(new GridColumnSpec(0f, float.MaxValue))
            .AddRow(new GridRowSpec(100f, 0f))
            .AddChild(searchResultsPane, new GridComponentSpec(0, 0));

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

            var modificationsPanel = new PScrollPane()
            {
                Child = modificationPanel,
                BackColor = Color.green,
                FlexSize = Vector2.right
            };

            var modifications = new PGridPanel()
            {
                FlexSize = Vector2.right,
                DynamicSize = true
            }
            .AddColumn(new GridColumnSpec(0f, float.MaxValue))
            .AddRow(new GridRowSpec(100f, 0f))
            .AddChild(modificationsPanel, new GridComponentSpec(0, 0));

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