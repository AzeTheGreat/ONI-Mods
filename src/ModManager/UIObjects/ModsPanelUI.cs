using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModManager
{
    public class ModsPanelUI : IUISource
    {
        public Func<IEnumerable<ModUIExtract>> GetBaseChildren { get; set; }

        protected VirtualScrollPanel scrollContents;
        protected ADragMe.IDragListener dragListener = new ModsPanelDragListener();

        public IUIComponent GetUIComponent()
        {
            scrollContents = new VirtualScrollPanel()
            {
                Alignment = TextAnchor.UpperLeft,
                FlexSize = Vector2.right,
                Margin = new(5, 13, 5, 5),
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

        public IEnumerable<IUISource> GetUISources(IEnumerable<ModUIExtract> children) => children.Select(
            x => new ModEntryUI(x)
            {
                DragListener = dragListener
            });

        public void UpdateSearchResults(string text)
        {
            var newChildren = GetBaseChildren().Where(x => CultureInfo.InvariantCulture.CompareInfo.IndexOf(x.Title.text, text, CompareOptions.IgnoreCase) >= 0);
            scrollContents.UpdateChildren(GetUISources(newChildren));
        }

        private class ModsPanelDragListener : ADragMe.IDragListener
        {
            public void OnBeginDrag(PointerEventData eventData)
            {
                return;
            }

            public void OnDrag(PointerEventData eventData)
            {
                return;
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                return;
            }
        }
    }
}
