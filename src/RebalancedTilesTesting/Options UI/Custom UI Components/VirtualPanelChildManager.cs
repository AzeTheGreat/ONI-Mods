using PeterHan.PLib;
using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RebalancedTilesTesting.CustomUIComponents
{
    public class VirtualPanelChildManager : KMonoBehaviour
    {
        protected KScrollRect scrollRect; // Parent component
        [MyCmpGet] protected BoxLayoutGroup boxLayoutGroup;

        protected List<IUISource> children;
        protected Dictionary<int, GameObject> activeChildren = new();

        protected GameObject spacer;
        protected List<float> cachedHeights = new();
        protected Dictionary<Type, float> rowHeights = new();
        protected int lastFirstActiveIndex = 0;
        
        public override void OnSpawn()
        {
            Debug.Log("OnSpawn");
            base.OnSpawn();
            scrollRect = GetComponentInParent<KScrollRect>();
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

            CacheRowHeights();
            gameObject.SetMinUISize(GetUISize());
            RefreshChildren();
        }

        public void SetChildren(IEnumerable<IUISource> children) => this.children = children.ToList();

        public void OnScrollValueChanged(Vector2 pos) => RefreshChildren();

        public void UpdateChildren(IEnumerable<IUISource> children)
        {
            Debug.Log(1);
            DestroyChildren(activeChildren);
            this.children = children.ToList();
            Debug.Log(2);
            CacheRowHeights();
            gameObject.SetMinUISize(GetUISize());
            Debug.Log(5);
            scrollRect.verticalNormalizedPosition = 1;
            Debug.Log(6);
            lastFirstActiveIndex = int.MaxValue;
            Debug.Log(3);
            RefreshChildren();
        }

        private Vector2 GetUISize() => new Vector2(0f, cachedHeights.LastOrDefault());

        private void RefreshChildren()
        {
            var ymin = scrollRect.content.localPosition.y;
            var ymax = ymin + scrollRect.viewport.rect.height;

            // Keep a buffer of one extra active child above and below.
            var firstActiveIndex = cachedHeights.FindIndex(x => x > ymin) - 2;
            var endIndex = cachedHeights.FindIndex(x => x > ymax);
            var lastActiveIndex = (endIndex == -1 ? cachedHeights.Count - 1 : endIndex) + 1;

            // Early out if the active indices are the same.
            if (firstActiveIndex == lastFirstActiveIndex)
                return;
            lastFirstActiveIndex = firstActiveIndex;

            // Destroy active children outside the active index range.
            DestroyChildren(activeChildren
                .Where(x => x.Key < firstActiveIndex || x.Key > lastActiveIndex));

            // Configure the spacer if there are hidden rows.
            if (firstActiveIndex >= 1)
                SetSpacerHeight(cachedHeights[firstActiveIndex - 1]);
            else
                spacer?.SetActive(false);

            // Build visible + buffer children not already active.
            var activeIndices = activeChildren.Select(x => x.Key);
            for (int i = firstActiveIndex; i <= lastActiveIndex; i++)
                if (i >= 0 && i < children.Count() && !activeIndices.Contains(i))
                    BuildChild(i);

            // Sort active children in hierarchy.
            spacer?.transform.SetAsLastSibling();
            foreach (var kvp in activeChildren.OrderBy(x => x.Key))
                kvp.Value.transform.SetAsLastSibling();
        }

        private GameObject BuildChild(int i) => BuildChild(children.ElementAtOrDefault(i), i);
        private GameObject BuildChild(IUISource child) => BuildChild(child, children.IndexOf(child));
        private GameObject BuildChild(IUISource child, int i)
        {
            if (child is null)
                return null;

            var go = child.GetUIComponent().Build().SetParent(gameObject);

            activeChildren.Add(i, go);
            PUIElements.SetAnchors(go, PUIAnchoring.Stretch, PUIAnchoring.Stretch);
            return go;
        }

        private void CacheRowHeights()
        {
            cachedHeights.Clear();

            foreach (var child in children)
            {
                var childType = child.GetType();
                if (!rowHeights.TryGetValue(childType, out var height))
                    rowHeights[childType] = height = GetRowHeight(child);

                cachedHeights.Add(cachedHeights.LastOrDefault() + height);
            }

            float GetRowHeight(IUISource child)
            {
                float rowHeight = 0f;
                if (BuildChild(child)?.GetComponentsInChildren<ILayoutElement>() is ILayoutElement[] layoutElements)
                {
                    // Row height is the largest preferred or min height of all layout elements, plus layout spacing.
                    foreach (var le in layoutElements)
                        rowHeight = Mathf.Max(rowHeight, le.minHeight, le.preferredHeight);
                    rowHeight += boxLayoutGroup.Params.Spacing;
                }
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

        private void DestroyChildren(IEnumerable<KeyValuePair<int, GameObject>> toClear)
        {
            foreach (var kvp in toClear.ToList())
            {
                activeChildren.Remove(kvp.Key);
                Destroy(kvp.Value);
            }
        }
    }
}
