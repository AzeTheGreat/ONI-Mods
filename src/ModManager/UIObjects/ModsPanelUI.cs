using PeterHan.PLib.UI;
using UnityEngine;

namespace ModManager
{
    public class ModsPanelUI : IUISource
    {
        // Used to constrain scroll panes since nothing else seems to work.
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

            return new PGridPanel()
            {
                FlexSize = Vector2.one
            }
            .AddRow(new()).AddRow(new()).AddRow(new()).AddRow(new(scrollPaneHeight))
            .AddColumn(new(scrollPaneWidth)).AddColumn(new(scrollPaneWidth))
            .AddChild(new PresetsUI().GetUIComponent(), new(0, 0) { ColumnSpan = 2 })
            .AddChild(new SearchUI().GetUIComponent(), new(1, 0) { ColumnSpan = 2 })
            .AddChild(inactiveTitle, new(2, 0))
            .AddChild(activeTitle, new(2, 1))
            .AddChild(new InactiveModsPanelUI().GetUIComponent(), new(3, 0))
            .AddChild(new ActiveModsPanelUI().GetUIComponent(), new(3, 1));
        }
    }

    public class SearchUI : IUISource
    {
        public IUIComponent GetUIComponent()
        {
            var tf = new PTextField()
            {
                Text = "Search for mods",
                FlexSize = Vector2.one,
            };

            return new PPanel()
            {
                Margin = new(2, 2, 2, 2),
                FlexSize = Vector2.one
            }
            .AddChild(tf);
        }
    }
}
