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

        private readonly RectOffset margins = new RectOffset(5, 5, 5, 5);

        private const string SEARCH_HEADER = "Search Header";
        private const string SEARCH_BODY = "Search Body";
        private const string EDIT_HEADER = "Edit Header";
        private const string EDIT_BODY = "Edit Body";

        private GameObject searchHeaderGO;
        private GameObject searchBodyGO;
        private GameObject editHeaderGO;
        private GameObject editBodyGO;

        public EmbeddedOptions() : base(string.Empty, string.Empty, string.Empty) { }

        public override GameObject GetUIComponent()
        {
            var panel = new PPanel("Main")
            {
                BackColor = Color.blue,
                DynamicSize = true,
                Direction = PanelDirection.Vertical,
                Alignment = TextAnchor.MiddleCenter,
                Spacing = 10,
                Margin = margins
            };

            var builtUI = panel
                .AddChild(GetSearchHeader())
                .AddChild(GetSearchBody())
                .AddChild(GetEditHeader())
                .AddChild(GetEditBody())
                .Build();

            foreach (Transform child in builtUI.transform)
            {
                if (child.name == SEARCH_HEADER)
                    searchHeaderGO = child.gameObject;
                if (child.name == SEARCH_BODY)
                    searchBodyGO = child.gameObject;
                if (child.name == EDIT_HEADER)
                    editHeaderGO = child.gameObject;
                if (child.name == EDIT_BODY)
                    editBodyGO = child.gameObject;
            }

            EnableSearchUI();
            return builtUI;
        }

        private void EnableSearchUI()
        {
            searchHeaderGO.SetActive(true);
            searchBodyGO.SetActive(true);
            editHeaderGO.SetActive(false);
            editBodyGO.SetActive(false);
        }

        private void EnableEditUI()
        {
            searchHeaderGO.SetActive(false);
            searchBodyGO.SetActive(false);
            editHeaderGO.SetActive(true);
            editBodyGO.SetActive(true);
        }

        private IUIComponent GetSearchHeader()
        {
            var searchField = new PTextField()
            {
                Text = "Search...",
                FlexSize = Vector2.right,
                TextAlignment = TMPro.TextAlignmentOptions.Left
            };

            var clearSearchButton = new PButton()
            {
                Text = "Clear"
            };

            return new PPanel(SEARCH_HEADER)
            {
                BackColor = Color.green,
                Direction = PanelDirection.Horizontal,
                Spacing = 10,
                Margin = margins,
                FlexSize = Vector2.right
            }
            .AddChild(searchField)
            .AddChild(clearSearchButton);
        }

        private IUIComponent GetSearchBody()
        {
            var searchableDefs = new List<IUIComponent>();
            foreach (var kvp in Options.Opts.UIConfigOptions)
                searchableDefs.Add(new PButton()
                {
                    Text = kvp.Key,
                    DynamicSize = false,
                    OnClick = (GameObject src) => EnableEditUI()
                });

            return GetScrollPaneLayout(SEARCH_BODY, new VirtualScrollPanel()
            {
                Alignment = TextAnchor.UpperLeft,
                BackColor = Color.magenta,
                Spacing = 5,
                Margin = margins,
                Children = searchableDefs
            });
        }

        private IUIComponent GetEditHeader()
        {
            return new PButton(EDIT_HEADER)
            {
                Text = "Currently Selected: ",
                FlexSize = Vector2.right,
                Margin = margins,
                OnClick = (GameObject src) => EnableSearchUI()
            };
        }

        private IUIComponent GetEditBody()
        {
            return GetScrollPaneLayout(EDIT_BODY, new PPanel()
                {
                    BackColor = Color.clear,
                    Spacing = 5,
                    Margin = margins
                });
        }

        private IUIComponent GetScrollPaneLayout(string name, IUIComponent child)
        {
            var panel = new PScrollPane()
            {
                Child = child,
                ScrollHorizontal = false,
                ScrollVertical = true,
                BackColor = Color.green,
                FlexSize = Vector2.one,
                TrackSize = 8f
            };

            return new PGridPanel(name)
            {
                FlexSize = Vector2.one,
                DynamicSize = true
            }
            .AddColumn(new GridColumnSpec(350f, float.MaxValue)) // Must set a min width somewhere, might as well be here.
            .AddRow(new GridRowSpec(300f, 0f)) // For some reason this doesn't flex down and must be hardcoded.
            .AddChild(panel, new GridComponentSpec(0, 0));
        }
    }
}