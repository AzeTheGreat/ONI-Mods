using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModManager
{
    public class ModsPanelUI : UISource
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
            .AddOnRealize(AddListeners);

            void AddListeners(GameObject go)
            {
                var search = go.AddComponent<SearchTextChangeListener>();
                var entryDrag = go.AddComponent<ModEntryDragListener>();
                search.Instance = entryDrag.Instance = this;
            }
        }

        public IEnumerable<UISource> GetUISources(IEnumerable<ModUIExtract> children) => children.Select(
            x => new ModEntryUI()
            {
                Mod = x
            });

        private class SearchTextChangeListener : MonoBehaviour, SearchUI.ITextChangeHandler
        {
            public ModsPanelUI Instance { get; set; }

            public void OnTextChange(string text)
            {
                var newChildren = Instance.GetBaseChildren()
                    .Where(x => CultureInfo.InvariantCulture.CompareInfo.IndexOf(x.Title.text, text, CompareOptions.IgnoreCase) >= 0);
                Instance.scrollContents.UpdateChildren(Instance.GetUISources(newChildren), true);
            }
        }

        private class ModEntryDragListener : MonoBehaviour, ADragMe.IDragHandler
        {
            public ModsPanelUI Instance { get; set; }

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

                var newChildren = Instance.scrollContents.GetChildren()
                    .Cast<ModEntryUI>()
                    .Where(x => x.Mod.Mod != modToMove.Mod.Mod)
                    .ToList();

                var idx = modAtTarget != null ? newChildren.FindIndex(x => x.Mod.Mod == modAtTarget.Mod.Mod) : newChildren.Count - 1;
                newChildren.Insert(idx, modToMove);

                modToMove.SetDragState(false);
                Instance.scrollContents.UpdateChildren(newChildren, false);
            }

            private ModEntryUI GetModAtPos(Vector2 pos)
            {
                return Instance.scrollContents.GetChildren()
                    .Where(x => x.GO != null)
                    .FirstOrDefault(x => RectTransformUtility.RectangleContainsScreenPoint(x.GO.rectTransform(), pos)) as ModEntryUI;
            }

            private ModEntryUI GetModBelowPos(Vector2 pos)
            {
                return Instance.scrollContents.GetChildren()
                    .Where(x => x.GO != null)
                    .FirstOrDefault(x => x.GO.rectTransform().position.y < pos.y) as ModEntryUI;
            }
        }
    }
}
