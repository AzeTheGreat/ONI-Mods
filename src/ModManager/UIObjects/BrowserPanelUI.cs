using PeterHan.PLib.UI;
using System.Linq;
using UnityEngine;

namespace ModManager
{
    public class BrowserPanelUI : UISource
    {
        // Used to constrain scroll panes.
        public const float scrollPaneHeight = 600f;

        protected override IUIComponent GetUIComponent()
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
                GetBaseChildren = () => AModsScreen.Instance.ModUIExtractions.Where(x => x.Mod.IsEnabled())
            };

            var inactiveModsPanel = new ModsPanelUI()
            {
                GetBaseChildren = () => AModsScreen.Instance.ModUIExtractions.Where(x => !x.Mod.IsEnabled())
            };

            return new PGridPanel()
            {
                FlexSize = Vector2.one
            }
            .AddRow(new()).AddRow(new()).AddRow(new()).AddRow(new(scrollPaneHeight))
            .AddColumn(new()).AddColumn(new())
            .AddChild(new PresetsUI().CreateUIComponent(), new(0, 0) { ColumnSpan = 2 })
            .AddChild(new SearchUI().CreateUIComponent(), new(1, 0) { ColumnSpan = 2 })
            .AddChild(inactiveTitle, new(2, 0))
            .AddChild(activeTitle, new(2, 1))
            .AddChild(inactiveModsPanel.CreateUIComponent(), new(3, 0))
            .AddChild(activeModsPanel.CreateUIComponent(), new(3, 1));
        }
    }
}
