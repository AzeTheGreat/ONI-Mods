using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    class InfoCard
    {
        public Entry shadowBar;
        public List<Entry> iconWidgets;
        public List<Entry> textWidgets;
        public Entry selectBorder;

        public float Width { get { return shadowBar.rect.rect.width; } }
        public float YMax { get { return shadowBar.rect.anchoredPosition.y; } }
        public float YMin { get { return YMax - shadowBar.rect.rect.height; } }

        public InfoCard(Entry shadowBar, ref int iconIndex, ref int textIndex)
        {
            this.shadowBar = shadowBar;
            iconWidgets = GetEntries(ref iconIndex, DrawnWidgets.Instance.iconWidgets);
            textWidgets = GetEntries(ref textIndex, DrawnWidgets.Instance.textWidgets);
        }

        public void Translate(float x, float y)
        {
            shadowBar.rect.anchoredPosition = new Vector2(shadowBar.rect.anchoredPosition.x + x, shadowBar.rect.anchoredPosition.y + y);

            if (selectBorder.rect != null)
                selectBorder.rect.anchoredPosition = new Vector2(selectBorder.rect.anchoredPosition.x + x, selectBorder.rect.anchoredPosition.y + y);

            foreach (var icon in iconWidgets)
            {
                icon.rect.anchoredPosition = new Vector2(icon.rect.anchoredPosition.x + x, icon.rect.anchoredPosition.y + y);
            }
            foreach (var text in textWidgets)
            {
                text.rect.anchoredPosition = new Vector2(text.rect.anchoredPosition.x + x, text.rect.anchoredPosition.y + y);
            }
        }

        private List<Entry> GetEntries(ref int index, List<Entry> widgets)
        {
            var entries = new List<Entry>();

            for (int i = index; i < widgets.Count; i++)
            {
                var widget = widgets[i];

                if (RectWithin(shadowBar, widget))
                    entries.Add(widget);
                else
                {
                    index = i;
                    break;
                }
            }
            return entries;
        }

        private bool RectWithin(Entry main, Entry sub)
        {
            return sub.rect.anchoredPosition.y > main.rect.offsetMin.y && sub.rect.anchoredPosition.y < main.rect.offsetMax.y;
        }
    }
}
