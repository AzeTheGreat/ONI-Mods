using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    public class VirtualPanelChildManager : KMonoBehaviour
    {
        protected KScrollRect scrollRect; // Parent component
        [MyCmpGet] protected BoxLayoutGroup boxLayoutGroup;

        protected List<UISource> children = new();
        protected GameObject spacer;

        // Represents the height from the top of the panel to the bottom of each element.
        protected List<float> cachedHeights = new();
        // Caches the height of each UISource type so that initialization doesn't need to build all children.
        protected Dictionary<Type, float> uiSourceHeights = new();

        public override void OnSpawn()
        {
            base.OnSpawn();
            scrollRect = GetComponentInParent<KScrollRect>();
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

            // Ensure the layout group is never locked.
            // A locked layout won't properly arrange children during scrolling.
            gameObject.SetLayoutLockState(false);

            RefreshChildren();
        }

        public void OnScrollValueChanged(Vector2 pos) => RefreshChildren();

        public IEnumerable<UISource> GetChildren() => children;

        public void UpdateChildren(IEnumerable<UISource> children, bool scrollToTop)
        {
            DestroyChildren(this.children);
            this.children = children.ToList();
            CacheRowHeights();
            gameObject.SetMinUISize(GetUISize());

            if (scrollToTop && scrollRect)
                scrollRect.verticalNormalizedPosition = 1;

            RefreshChildren();
        }

        private Vector2 GetUISize() => new(-1f, cachedHeights.LastOrDefault() + boxLayoutGroup.Params.Margin.bottom);

        private void RefreshChildren()
        {
            var (ymin, ymax) = scrollRect ? GetViewportVals(scrollRect) : (0f, 600f);
            var (firstActiveIndex, lastActiveIndex) = GetChildIndexRange(ymin, ymax);

            // Destroy active children outside the active index range.
            DestroyChildren(children.Where((x, i) => i < firstActiveIndex || i > lastActiveIndex));

            // Configure the spacer if there are hidden rows.
            if (firstActiveIndex >= 1)
                SetSpacerHeight(cachedHeights[firstActiveIndex - 1] - boxLayoutGroup.Params.Margin.top);
            else
                spacer?.SetActive(false);

            // Build visible + buffer children not already active.
            foreach (var child in children.Where((x, i) => i >= firstActiveIndex && i <= lastActiveIndex && x.GO == null))
                BuildChild(child);

            // Sort active children in hierarchy.
            spacer?.transform.SetAsLastSibling();
            foreach (var child in children)
                child.GO?.transform.SetAsLastSibling();
        }

        private (float min, float max) GetViewportVals(ScrollRect scrollRect)
        {
            var ymin = scrollRect.content.localPosition.y;
            var ymax = ymin + scrollRect.viewport.rect.height;
            return (ymin, ymax);
        }

        private (int first, int last) GetChildIndexRange(float yMin, float yMax)
        {
            // Keep a buffer of one extra active child above and below.
            var firstActiveIndex = cachedHeights.FindIndex(x => x > yMin) - 1;
            var endIndex = cachedHeights.FindIndex(x => x > yMax);
            var lastActiveIndex = endIndex == -1 ? cachedHeights.Count - 1 : endIndex + 1;
            return (firstActiveIndex, lastActiveIndex);
        }

        private void BuildChild(UISource child)
        {
            if (child is null)
                return;

            var go = child.CreateUIComponent().Build().SetParent(gameObject);
            PUIElements.SetAnchors(go, PUIAnchoring.Stretch, PUIAnchoring.Stretch);
        }

        private void CacheRowHeights()
        {
            cachedHeights.Clear();
            foreach (var child in children)
            {
                var height = GetCachedObjectHeight(child);

                if (child == children.First())
                    height += boxLayoutGroup.Params.Margin.top;
                else
                    height += boxLayoutGroup.Params.Spacing;

                cachedHeights.Add(cachedHeights.LastOrDefault() + height);
            }

            float GetCachedObjectHeight(UISource child)
            {
                var childType = child.GetType();
                if (!uiSourceHeights.TryGetValue(childType, out var height))
                    uiSourceHeights[childType] = height = GetObjectHeight(child);
                return height;
            }

            // Build a temporary object for the type to get a row height.
            float GetObjectHeight(UISource child)
            {
                BuildChild(child);
                LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.rectTransform());

                var rowHeight = 0f;
                foreach (var le in child.GO.GetComponents<ILayoutElement>())
                    rowHeight = Mathf.Max(rowHeight, le.minHeight, le.preferredHeight);

                child.DestroyGO();
                return rowHeight;
            }
        }

        private void SetSpacerHeight(float height)
        {
            spacer ??= BuildSpacer();
            spacer.SetActive(true);
            spacer.SetMinUISize(new Vector2(0f, height));
        }

        private GameObject BuildSpacer()
        {
            return new PPanel() { BackColor = Color.clear }
            .Build()
            .SetParent(gameObject);
        }

        private void DestroyChildren(IEnumerable<UISource> toClear)
        {
            foreach (var child in toClear.ToList())
                child.DestroyGO();
        }
    }
}
