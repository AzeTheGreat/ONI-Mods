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
                InitialChildren = GetUISources(GetBaseChildren()),
                Spacing = 4
            };

            return new PScrollPane()
            {
                Child = scrollContents,
                ScrollHorizontal = false,
                ScrollVertical = true,
                FlexSize = Vector2.one,
                TrackSize = 8f
            }
            .AddOnRealize(AddSearchListener);

            void AddSearchListener(GameObject go)
            {
                var target = go.AddComponent<SearchFieldChangedTarget>();
                target.Instance = this;
            }
        }

        public IEnumerable<UISource> GetUISources(IEnumerable<ModUIExtract> children) => children.Select(
            x => new ModEntryUI()
            {
                Mod = x,
                DragListener = this
            });

        private ModEntryUI modToMove;

        public void OnBeginDrag(PointerEventData eventData)
        {
            modToMove = GetModAtPos(eventData.position);
            modToMove.SetDragState(true);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            var modAtTarget = GetModBelowPos(eventData.position);

            var newChildren = scrollContents.GetChildren()
                .Cast<ModEntryUI>()
                .Where(x => x.Mod.Mod != modToMove.Mod.Mod)
                .ToList();

            var idx = modAtTarget != null ? newChildren.FindIndex(x => x.Mod.Mod == modAtTarget.Mod.Mod) : newChildren.Count - 1;
            newChildren.Insert(idx, modToMove);

            modToMove.SetDragState(false);
            scrollContents.UpdateChildren(newChildren, false);
        }

        private ModEntryUI GetModAtPos(Vector2 pos)
        {
            return scrollContents.GetChildren()
                .Where(x => x.GO != null)
                .FirstOrDefault(x => RectTransformUtility.RectangleContainsScreenPoint(x.GO.rectTransform(), pos)) as ModEntryUI;
        }

        private ModEntryUI GetModBelowPos(Vector2 pos)
        {
            return scrollContents.GetChildren()
                .Where(x => x.GO != null)
                .FirstOrDefault(x => x.GO.rectTransform().position.y < pos.y) as ModEntryUI;
        }

        private class SearchFieldChangedTarget : MonoBehaviour, SearchUI.ITextChanged
        {
            public ModsPanelUI Instance { get; set; }

            public void OnSeachFieldChanged(string text)
            {
                var newChildren = Instance.GetBaseChildren()
                    .Where(x => CultureInfo.InvariantCulture.CompareInfo.IndexOf(x.Title.text, text, CompareOptions.IgnoreCase) >= 0);
                Instance.scrollContents.UpdateChildren(Instance.GetUISources(newChildren), true);
            }
        }
    }
}
