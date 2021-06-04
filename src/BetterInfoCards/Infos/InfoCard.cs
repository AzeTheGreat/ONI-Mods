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
        
        private List<Action<List<InfoCard>>> infos = new();
        private (int drawIndex, TextInfo ti) titleDrawer;

        // Assume that order is maintained here. Technically it isn't guaranteed...
        // But it works as used and I don't want to eat the performance hit of a SortedDictionary.
        public Dictionary<string, TextInfo> textInfos = new();

        public string GetTitleKey() => titleDrawer.ti?.Text.RemoveCountSuffix() ?? string.Empty;

        public void Draw(List<InfoCard> cards, int visCardIndex)
        {
            if (visCardIndex > 0)
            {
                // Getting the style like this is not ideal since it could potentially be different from the title's.
                // It does not appear to be an issue under current game conditions though.
                infos[titleDrawer.drawIndex] += _ =>
                {
                    InterceptHoverDrawer.drawerInstance.DrawText(
                    " #" + (++visCardIndex),
                    SelectTool.Instance.hoverTextConfiguration.Styles_Title.Standard);
                };
            }    

            foreach (var info in infos)
                info(cards);
        }

        public void AddDraw(Action<List<InfoCard>> drawAction) => infos.Add(drawAction);

        public void AddDraw(Action<List<InfoCard>> drawAction, TextInfo ti)
        {
            if (!textInfos.Any())
                titleDrawer = (infos.Count, ti);

            textInfos.Add(ti.ID, ti);
            AddDraw(drawAction);
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
            var newSB = InterceptHoverDrawer.drawerInstance.shadowBars.Draw(shadowBar.rect.anchoredPosition + new Vector2(shadowBar.rect.sizeDelta.x, 0f));
            newSB.rect.sizeDelta = new Vector2(width - shadowBar.rect.sizeDelta.x, shadowBar.rect.sizeDelta.y);

            if (selectBorder.rect != null)
                selectBorder.rect.sizeDelta = new Vector2(width + 2f, selectBorder.rect.sizeDelta.y);
        }
    }
}
