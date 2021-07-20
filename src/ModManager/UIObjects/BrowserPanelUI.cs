using PeterHan.PLib.UI;
using System.Linq;
using UnityEngine;

namespace ModManager
{
    public class BrowserPanelUI : IUISource
    {
        // Used to constrain scroll panes.
        public const float scrollPaneHeight = 600f;
        // Used to force a min width.
        public const float scrollPaneWidth = 300f;

        public IUIComponent GetUIComponent()
        {
            var inactiveTitle = new PLabel()
            {
                Text = "Inactive Mods"
            };

            var activeTitle = new PLabel()
            {
                Text = "Active Mods"
            };

            var activeModsPanel = new ModsPanelUI()
            {
                GetBaseChildren = () => OverrideModsScreen.ModUIExtractions.Where(x => x.Mod.IsActive())
            };

            var inactiveModsPanel = new ModsPanelUI()
            {
                GetBaseChildren = () => OverrideModsScreen.ModUIExtractions.Where(x => !x.Mod.IsActive())
            };

            var search = new SearchUI()
            {
                OnTextChanged = text =>
                {
                    activeModsPanel.UpdateSearchResults(text);
                    inactiveModsPanel.UpdateSearchResults(text);
                }
            }
            .GetUIComponent();

            return new PGridPanel()
            {
                FlexSize = Vector2.one
            }
            .AddRow(new()).AddRow(new()).AddRow(new()).AddRow(new(scrollPaneHeight))
            .AddColumn(new(scrollPaneWidth)).AddColumn(new(scrollPaneWidth))
            .AddChild(new PresetsUI().GetUIComponent(), new(0, 0) { ColumnSpan = 2 })
            .AddChild(search, new(1, 0) { ColumnSpan = 2 })
            .AddChild(inactiveTitle, new(2, 0))
            .AddChild(activeTitle, new(2, 1))
            .AddChild(inactiveModsPanel.GetUIComponent(), new(3, 0))
            .AddChild(activeModsPanel.GetUIComponent(), new(3, 1));
        }
    }
}
