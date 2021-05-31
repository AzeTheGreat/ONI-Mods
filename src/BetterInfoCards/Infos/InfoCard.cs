using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCard
    {
        public bool isSelected;
        public KSelectable selectable;
        public List<Action<List<InfoCard>>> infos = new();
        public Dictionary<string, TextInfo> textInfos = new();

        public string GetTitleKey()
        {
            return textInfos.Values.FirstOrDefault()?.Text.RemoveCountSuffix() ?? string.Empty;
        }

        public void Draw(List<InfoCard> cards)
        {
            foreach (var info in infos)
                info(cards);
        }
    }

    // TODO: Figure out clear distinction between Infos and Datas
    public class ICWidgetData
    {
        public List<Entry> entries = new();
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
                entries.Add(entry);
        }

        public void Translate(float x)
        {
            var shift = new Vector2(x, offset.y);

            shadowBar.rect.anchoredPosition += shift;

            if (selectBorder.rect != null)
                selectBorder.rect.anchoredPosition += shift;

            foreach (var entry in entries)
                entry.rect.anchoredPosition += shift;
        }

        public void SetWidth(float width)
        {
            // Modifying existing SBs triggers rebuilds somewhere and has a major impact on performance.
            // Genius idea from Peter to just add new ones to fill the gap.
            var newSB = InterceptDrawer.drawerInstance.shadowBars.Draw(shadowBar.rect.anchoredPosition + new Vector2(shadowBar.rect.sizeDelta.x, 0f));
            newSB.rect.sizeDelta = new Vector2(width - shadowBar.rect.sizeDelta.x, shadowBar.rect.sizeDelta.y);

            if (selectBorder.rect != null)
                selectBorder.rect.sizeDelta = new Vector2(width + 2f, selectBorder.rect.sizeDelta.y);
        }
    }
}
