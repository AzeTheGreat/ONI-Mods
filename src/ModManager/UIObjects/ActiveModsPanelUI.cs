using PeterHan.PLib.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    public partial class ActiveModsPanelUI : IUISource
    {
        public IUIComponent GetUIComponent()
        {
            var scrollContents = new VirtualScrollPanel()
            {
                Alignment = TextAnchor.UpperLeft,
                FlexSize = Vector2.right,
                Children = Global.Instance.modManager.mods.Where(x => x.IsActive()).Select(x => new ModEntryUI(x)),
            };

            return new PScrollPane()
            {
                Child = scrollContents,
                ScrollHorizontal = false,
                ScrollVertical = true,
                FlexSize = Vector2.one,
                TrackSize = 8f
            };
        }
    }
}
