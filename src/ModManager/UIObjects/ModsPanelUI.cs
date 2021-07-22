using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace ModManager
{
    public class ModsPanelUI : IUISource
    {
        public Func<IEnumerable<ModUIExtract>> GetBaseChildren { get; set; }

        protected VirtualScrollPanel scrollContents;

        public IUIComponent GetUIComponent()
        {
            //scrollContents = new VirtualScrollPanel()
            var scrollContents = new PPanel
            {
                Alignment = TextAnchor.UpperLeft,
                FlexSize = Vector2.right,
                Margin = new(5, 13, 5, 5),
                //Children = GetUISources(GetBaseChildren()),
            };

            foreach (var src in GetUISources(GetBaseChildren()))
            {
                scrollContents.AddChild(src.GetUIComponent());
            }

            return new PScrollPane()
            {
                Child = scrollContents,
                ScrollHorizontal = false,
                ScrollVertical = true,
                FlexSize = Vector2.one,
                TrackSize = 8f
            };
        }

        public IEnumerable<IUISource> GetUISources(IEnumerable<ModUIExtract> children) => children.Select(x => new ModEntryUI(x));

        public void UpdateSearchResults(string text)
        {
            var newChildren = GetBaseChildren().Where(x => CultureInfo.InvariantCulture.CompareInfo.IndexOf(x.Title.text, text, CompareOptions.IgnoreCase) >= 0);
            //scrollContents.UpdateChildren(GetUISources(newChildren));
        }
    }
}
