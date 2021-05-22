using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCard
    {
        public KSelectable selectable;

        public Dictionary<string, TextInfo> textInfos = new();
        public Entry selectBorder;
        private Entry shadowBar;
        private List<Entry> iconWidgets = new();

        private float widthOverride;
        public float Width => shadowBar.rect.rect.width + widthOverride;
        public float Height => shadowBar.rect.rect.height;
        public float YMax => shadowBar.rect.anchoredPosition.y;
        public float YMin => YMax - shadowBar.rect.rect.height;

        public void AddSelectable(KSelectable selectable)
        {
            this.selectable = selectable;
        }

        public void AddWidget(Entry entry, GameObject prefab, string name, object data)
        {
            var skin = HoverTextScreen.Instance.drawer.skin;

            if (prefab == skin.shadowBarWidget.gameObject)
                shadowBar = entry;
            else if (prefab == skin.iconWidget.gameObject)
                iconWidgets.Add(entry);
            else if (prefab == skin.textWidget.gameObject)
            {
                // TODO: Why are there duplicate keys being created...?
                var ti = TextInfo.Create(entry, name, data);
                if (!ti.Key.IsNullOrWhiteSpace() && !textInfos.ContainsKey(ti.Key))
                    textInfos.Add(ti.Key, ti);
            }  
            else if (prefab == skin.selectBorderWidget.gameObject)
                selectBorder = entry;
        }

        public void Translate(float x, float y)
        {
            var shift = new Vector2(x, y);
            shadowBar.rect.anchoredPosition += shift;

            if (selectBorder.rect != null)
                selectBorder.rect.anchoredPosition += shift;

            foreach (var icon in iconWidgets)
                icon.rect.anchoredPosition += shift;

            foreach (var text in textInfos.Values)
                text.TextEntry.rect.anchoredPosition += shift;
        }

        public void Resize(float width)
        {
            shadowBar.rect.sizeDelta = new Vector2(width, shadowBar.rect.sizeDelta.y);

            if (selectBorder.rect != null)
                selectBorder.rect.sizeDelta = new Vector2(width + 2f, selectBorder.rect.sizeDelta.y);
        }

        // TODO: Not use first
        public string GetTitleKey()
        {
            return textInfos.Values.FirstOrDefault()?.GetText().RemoveCountSuffix() ?? string.Empty;
        }

        public void Rename(List<InfoCard> cards, int visCardIndex)
        {
            var newWidthDelta = 0f;
            var titlePos = textInfos.Values.First().Widget.rectTransform.anchoredPosition.x;

            foreach (var textInfo in textInfos.Values)
            {
                var textOverride = textInfo.GetTextOverride(cards);

                if(textInfo.Key == ConverterManager.title && visCardIndex > 0)
                    textOverride += " #" + (visCardIndex + 1);

                var widget = textInfo.Widget;
                if(widget.text != textOverride)
                {
                    widget.text = textOverride;
                    var width = widget.preferredWidth + widget.rectTransform.anchoredPosition.x - titlePos;
                    newWidthDelta = Math.Max(width, newWidthDelta);
                }
            }

            widthOverride = newWidthDelta + HoverTextScreen.Instance.drawer.skin.shadowBarBorder.x * 2f - Width;
        }
    }
}
