using AzeLib.Extensions;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RebalancedTilesTesting
{
    public class EmbeddedOptions : OptionsEntry
    {
        private object ignore;
        public override object Value { get => ignore; set => ignore = value; }

        private readonly RectOffset margins = new RectOffset(5, 5, 5, 5);

        // Components cached in OnRealize.
        private GameObject searchHeaderGO;
        private GameObject searchBodyGO;
        private GameObject editHeaderGO;
        private GameObject editBodyGO;
        private VirtualPanelChildManager virtualPanelChildManager;
        private TMP_InputField textField;

        public EmbeddedOptions() : base(string.Empty, string.Empty, string.Empty) { }

        public override GameObject GetUIComponent()
        {
            var panel = new PPanel()
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
                OnTextChanged = UpdateSearchResults,
                FlexSize = Vector2.right,
                TextAlignment = TMPro.TextAlignmentOptions.Left
            }
            .AddOnRealize((GameObject realized) => textField = realized.GetComponent<TMP_InputField>());

            var clearSearchButton = new PCheckBox()
            {
                OnChecked = (GameObject realized, int state) => PCheckBox.SetCheckState(realized, state == 0 ? 1 : 0)
            };

            return new PPanel()
            {
                BackColor = Color.green,
                Direction = PanelDirection.Horizontal,
                Spacing = 10,
                Margin = margins,
                FlexSize = Vector2.right
            }
            .AddChild(searchField)
            .AddChild(clearSearchButton)
            .AddOnRealize((GameObject realized) => searchHeaderGO = realized);

            void UpdateSearchResults(GameObject realized, string text)
            {
                var newResults = Options.Opts.UIConfigOptions
                    .Where(x => x.Key.Contains(text))
                    .Cast<object>().ToList();

                Debug.Log("START: " + text);
                foreach (var item in newResults)
                {
                    Debug.Log(item);
                }

                virtualPanelChildManager.UpdateChildren(newResults);
            }
        }

        private IUIComponent GetSearchBody()
        {
            var scrollPanel = new VirtualScrollPanel<KeyValuePair<string, UIConfigOptions>>()
            {
                Alignment = TextAnchor.UpperLeft,
                BackColor = Color.magenta,
                Spacing = 5,
                Margin = margins,
                Children = Options.Opts.UIConfigOptions,
                ChildFactory = (KeyValuePair<string, UIConfigOptions> kvp) => new PButton()
                {
                    Text = kvp.Key,
                    DynamicSize = false,
                    OnClick = (GameObject src) => EnableEditUI()
                }
            }
            .AddOnRealize((GameObject realized) => virtualPanelChildManager = realized.GetComponent<VirtualPanelChildManager>());

            return GetScrollPaneLayout(scrollPanel)
                .AddOnRealize((GameObject realized) => searchBodyGO = realized);
        }

        private IUIComponent GetEditHeader()
        {
            return new PButton()
            {
                Text = "Currently Selected: ",
                FlexSize = Vector2.right,
                Margin = margins,
                OnClick = (GameObject src) => EnableSearchUI()
            }
            .AddOnRealize((GameObject realized) => editHeaderGO = realized);
        }

        private IUIComponent GetEditBody()
        {
            return GetScrollPaneLayout(new PPanel()
            {
                BackColor = Color.clear,
                Spacing = 5,
                Margin = margins
            })
            .AddOnRealize((GameObject realized) => editBodyGO = realized);
        }

        private IUIComponent GetScrollPaneLayout(IUIComponent child)
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

            return new PGridPanel()
            {
                FlexSize = Vector2.one,
                DynamicSize = true
            }
            .AddColumn(new GridColumnSpec(300f, float.MaxValue)) // Must set a min width somewhere, might as well be here.
            .AddRow(new GridRowSpec(270f, 0f)) // Must constrain to prevent the scroll pane from expanding to fit content, while still flexing.
            .AddChild(panel, new GridComponentSpec(0, 0));
        }
    }
}