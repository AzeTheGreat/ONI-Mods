using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModManager
{
    public class ModsPanelUI : UISource, ADragMe.IDragListener
    {
        public Func<IEnumerable<ModUIExtract>> GetBaseChildren { get; set; }

        protected VirtualScrollPanel scrollContents;

        protected override IUIComponent GetUIComponent()
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

        public IEnumerable<UISource> GetUISources(IEnumerable<ModUIExtract> children) => children.Select(
            x => new ModEntryUI()
            {
                Mod = x,
                DragListener = this
            });

        public void UpdateSearchResults(string text)
        {
            var newChildren = GetBaseChildren().Where(x => CultureInfo.InvariantCulture.CompareInfo.IndexOf(x.Title.text, text, CompareOptions.IgnoreCase) >= 0);
            scrollContents.UpdateChildren(GetUISources(newChildren));
        }

        private ModEntryUI modToMove;

        public void OnBeginDrag(PointerEventData eventData)
        {
            modToMove = GetModAtPos(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            return;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var modAtTarget = GetModAtPos(eventData.position);

            var newChildren = scrollContents.Children
                .Cast<ModEntryUI>()
                .Where(x => x.Mod.Mod != modToMove.Mod.Mod)
                .ToList();

            var idx = modAtTarget != null ? newChildren.FindIndex(x => x.Mod.Mod == modAtTarget.Mod.Mod) : newChildren.Count - 1;
            newChildren.Insert(idx, modToMove);

            scrollContents.UpdateChildren(newChildren);
        }

        private ModEntryUI GetModAtPos(Vector2 pos)
        {
            GameObject lastChild = null;
            foreach (var child in scrollContents.GetBuiltChildren())
            {
                if (child.rectTransform().position.y < pos.y)
                    return scrollContents.GetUISourceForGO(lastChild) as ModEntryUI;
                lastChild = child;
            }
            return null;
        }
    }
}
