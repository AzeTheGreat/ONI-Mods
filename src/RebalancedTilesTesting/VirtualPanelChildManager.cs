using PeterHan.PLib;
using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RebalancedTilesTesting
{
    class VirtualPanelChildManager : KMonoBehaviour
    {
        protected KScrollRect scrollRect; // Parent component
        [MyCmpGet] protected BoxLayoutGroup boxLayoutGroup;

        protected List<IUIComponent> children;
        protected List<GameObject> activeChildren = new List<GameObject>();
        protected GameObject spacer;
        protected float rowHeight;
        protected int lastFirstActiveIndex = 0;
        
        public override void OnSpawn()
        {
            base.OnSpawn();

            scrollRect = GetComponentInParent<KScrollRect>();
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

            rowHeight = BuildChild(children.First()).GetComponent<ILayoutElement>().minHeight + boxLayoutGroup.Params.Spacing;
            gameObject.SetMinUISize(new Vector2(0f, children.Count() * rowHeight));
            UpdateChildren();
        }

        public void OnScrollValueChanged(Vector2 pos) => UpdateChildren();

        // Insanely naive - lots of room for optimization of performance if it becomes an issue.
        private void UpdateChildren()
        {
            var ymin = scrollRect.content.localPosition.y;
            var rowsDisplayed = Math.Ceiling(scrollRect.viewport.rect.height / rowHeight);

            // Keep a buffer of one extra active child above and below.
            var firstActiveIndex = (int)Math.Floor(ymin / rowHeight) - 1;
            var lastActiveIndex = firstActiveIndex + rowsDisplayed + 2;

            // Don't fully rebuild if the active indices are the same
            if (firstActiveIndex == lastFirstActiveIndex)
                return;
            lastFirstActiveIndex = firstActiveIndex;

            activeChildren.ForEach(x => Destroy(x));
            activeChildren.Clear();

            // Configure the spacer if there are hidden rows.
            if (firstActiveIndex >= 1)
                SetSpacerHeight(firstActiveIndex * rowHeight);
            else
                spacer?.SetActive(false);

            // Build visible + buffer children.
            for (int i = firstActiveIndex; i <= lastActiveIndex; i++)
            {
                if (i >= 0 && i < children.Count())
                    BuildChild(children[i]);
            }
        }

        private GameObject BuildChild(IUIComponent child)
        {
            var go = child.Build();
            go.SetParent(gameObject);
            activeChildren.Add(go);
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
            var spacer = new PPanel() { BackColor = Color.clear };
            return spacer.Build().SetParent(gameObject);
        }

        public void SetChildren(IEnumerable<IUIComponent> children) => this.children = children.ToList();
    }
}
