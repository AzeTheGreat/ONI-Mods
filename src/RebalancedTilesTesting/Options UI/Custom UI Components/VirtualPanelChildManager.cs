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

        protected List<object> children;
        protected Func<object, IUIComponent> childFactory;
        protected Dictionary<int, GameObject> activeChildren = new();

        protected GameObject spacer;
        protected float rowHeight;
        protected int lastFirstActiveIndex = 0;
        
        public override void OnSpawn()
        {
            base.OnSpawn();
            scrollRect = GetComponentInParent<KScrollRect>();
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

            // Row height is the largest preferred or min height of all layout elements, plus layout spacing.
            var layoutElements = BuildChild(0).GetComponentsInChildren<ILayoutElement>();
            foreach (var le in layoutElements)
                rowHeight = Mathf.Max(rowHeight, le.minHeight, le.preferredHeight);
            rowHeight += boxLayoutGroup.Params.Spacing;

            gameObject.SetMinUISize(new Vector2(0f, children.Count() * rowHeight));
            RefreshChildren();
        }

        public void SetChildren(IEnumerable<object> children, Func<object, IUIComponent> childFactory)
        {
            this.children = children.ToList();
            this.childFactory = childFactory;
        }

        public void OnScrollValueChanged(Vector2 pos) => RefreshChildren();

        public void UpdateChildren(List<object> children)
        {
            DestroyChildren(activeChildren);
            this.children = children;

            gameObject.SetMinUISize(new Vector2(0f, children.Count() * rowHeight));
            scrollRect.verticalNormalizedPosition = 1;
            lastFirstActiveIndex = int.MaxValue;

            RefreshChildren();
        }

        private void RefreshChildren()
        {
            var ymin = scrollRect.content.localPosition.y;
            var rowsDisplayed = Math.Ceiling(scrollRect.viewport.rect.height / rowHeight);

            // Keep a buffer of one extra active child above and below.
            var firstActiveIndex = (int)Math.Floor(ymin / rowHeight) - 1;
            var lastActiveIndex = firstActiveIndex + rowsDisplayed + 2;

            // Early out if the active indices are the same.
            if (firstActiveIndex == lastFirstActiveIndex)
                return;
            lastFirstActiveIndex = firstActiveIndex;

            // Destroy active children outside the active index range.
            DestroyChildren(activeChildren
                .Where(x => x.Key < firstActiveIndex || x.Key > lastActiveIndex));

            // Configure the spacer if there are hidden rows.
            if (firstActiveIndex >= 1)
                SetSpacerHeight(firstActiveIndex * rowHeight);
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

        private GameObject BuildChild(int i)
        {
            if (!(children.ElementAtOrDefault(i) is var child))
                return null;

            var go = childFactory(child)
                .Build()
                .SetParent(gameObject);

            activeChildren.Add(i, go);
            PUIElements.SetAnchors(go, PUIAnchoring.Stretch, PUIAnchoring.Stretch);
            return go;
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
