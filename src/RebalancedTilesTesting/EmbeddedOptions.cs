using AzeLib.Extensions;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RebalancedTilesTesting
{
    public class EmbeddedOptions
    {
        private readonly RectOffset margins = new RectOffset(5, 5, 5, 5);

        // Components cached in OnRealize.
        private GameObject editOptionsPanel;
        private VirtualPanelChildManager virtualPanelChildManager;

        public KScreen GetDialog()
        {
            var dialog = new PDialog("ModOptions")
            {
                Title = "Test",
                SortKey = 150f,
                DialogBackColor = PUITuning.Colors.OptionsBackground,
                //DialogClosed = new PUIDelegates.OnDialogClosed(this.OnOptionsSelected),
                RoundToNearestEven = true
            };

            dialog.Body.AddChild(new PGridPanel()
            .AddRow(new GridRowSpec())
            .AddRow(new GridRowSpec(600f))
            .AddColumn(new GridColumnSpec(300f))
            .AddColumn(new GridColumnSpec(300f))
            .AddChild(GetSearchHeader(), new GridComponentSpec(0, 0))
            .AddChild(GetSearchBody(), new GridComponentSpec(1, 0))
            .AddChild(GetEditHeader(), new GridComponentSpec(0, 1))
            .AddChild(GetEditBody(), new GridComponentSpec(1, 1)))
            .Margin = null;

            return dialog.Build().GetComponent<KScreen>();
        }

        private IUIComponent GetSearchHeader()
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
                Margin = margins,
                FlexSize = Vector2.right
            }
            .AddChild(searchField)
            .AddChild(filterCheckbox);

            void UpdateSearchResults(string text)
            {
                var newResults = Options.Opts.UIConfigOptions
                    .Where(x => x.Value.GetName().Contains(text))
                    .Cast<object>().ToList();
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
                    Text = kvp.Value.GetName(),
                    DynamicSize = false,
                    OnClick = (GameObject src) => Edit(kvp.Value)
                }
            }
            .AddOnRealize((GameObject realized) => virtualPanelChildManager = realized.GetComponent<VirtualPanelChildManager>());

            return GetScrollPaneLayout(scrollPanel);

            void Edit(UIConfigOptions uiConfigOptions)
            {
                foreach (var option in uiConfigOptions.GetOptions())
                    option.GetUIComponent().SetParent(editOptionsPanel);
            }
        }

        private IUIComponent GetEditHeader()
        {
            return new PLabel()
            {
                Text = "Currently Selected: "
            };
        }

        private IUIComponent GetEditBody()
        {
            return GetScrollPaneLayout(new PPanel()
            {
                BackColor = Color.clear,
                Spacing = 5,
                Margin = margins
            }
            .AddOnRealize((GameObject realized) => editOptionsPanel = realized));
        }

        private IUIComponent GetScrollPaneLayout(IUIComponent child)
        {
            return new PScrollPane()
            {
                Child = child,
                ScrollHorizontal = false,
                ScrollVertical = true,
                BackColor = Color.green,
                FlexSize = Vector2.one,
                TrackSize = 8f
            };
        }
    }
}