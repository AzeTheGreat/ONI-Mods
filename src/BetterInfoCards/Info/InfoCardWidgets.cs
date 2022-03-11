using System.Collections.Generic;
using UnityEngine;
using Entry = HoverTextDrawer.Pool<UnityEngine.MonoBehaviour>.Entry;

namespace BetterInfoCards
{
    public class InfoCardWidgets
    {
        public List<Entry> widgets = new();
        public Entry shadowBar;
        public Entry selectBorder;
        public Vector2 offset = new();

        public float YMax => shadowBar.rect.anchoredPosition.y;
        public float YMin => YMax - Height;
        public float Width => shadowBar.rect.rect.width;
        public float Height => shadowBar.rect.rect.height;

        public void AddWidget(Entry entry, GameObject prefab)
        {
            var skin = HoverTextScreen.Instance.drawer.skin;

            if (prefab == skin.shadowBarWidget.gameObject)
                shadowBar = entry;
            else if (prefab == skin.selectBorderWidget.gameObject)
                selectBorder = entry;
            else
                widgets.Add(entry);
        }

        public void Translate(float x)
        {
            var shift = new Vector2(x, offset.y);

            shadowBar.rect.anchoredPosition += shift;

            if (selectBorder.rect != null)
                selectBorder.rect.anchoredPosition += shift;

            foreach (var widget in widgets)
                widget.rect.anchoredPosition += shift;
        }

        public void SetWidth(float width)
        {
            // Modifying existing SBs triggers rebuilds somewhere and has a major impact on performance.
            // Genius idea from Peter to just add new ones to fill the gap.
            var newSB = InterceptHoverDrawer.drawerInstance.shadowBars.Draw(shadowBar.rect.anchoredPosition + new Vector2(shadowBar.rect.sizeDelta.x, 0f));
            newSB.rect.sizeDelta = new Vector2(width - shadowBar.rect.sizeDelta.x, shadowBar.rect.sizeDelta.y);

            if (selectBorder.rect != null)
                selectBorder.rect.sizeDelta = new Vector2(width + 2f, selectBorder.rect.sizeDelta.y);
        }
    }
}
