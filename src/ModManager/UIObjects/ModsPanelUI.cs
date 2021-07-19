using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace ModManager
{
    public partial class ModsPanelUI : IUISource
    {
        public Func<IEnumerable<KMod.Mod>> GetBaseChildren { get; set; }

        protected VirtualScrollPanel scrollContents;

        public IUIComponent GetUIComponent()
        {
            scrollContents = new VirtualScrollPanel()
            {
                Alignment = TextAnchor.UpperLeft,
                FlexSize = Vector2.right,
                Children = GetUISources(GetBaseChildren()),
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

        public IEnumerable<IUISource> GetUISources(IEnumerable<KMod.Mod> children) => children.Select(x => new ModEntryUI(x));

        public void UpdateSearchResults(string text)
        {
            var newChildren = GetBaseChildren().Where(x => CultureInfo.InvariantCulture.CompareInfo.IndexOf(x.title, text, CompareOptions.IgnoreCase) >= 0);
            scrollContents.UpdateChildren(GetUISources(newChildren));
        }
    }
}
