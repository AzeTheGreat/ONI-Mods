using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Entry = HoverTextDrawer.Pool<UnityEngine.MonoBehaviour>.Entry;

namespace BetterInfoCards
{
    public class InfoCard
    {
        public KSelectable selectable;

        public Dictionary<string, TextInfo> textInfos = new();
        public Entry selectBorder;
        private Entry shadowBar;
        private List<(Entry entry, string key, object data)> tiDatas = new();
        private List<Entry> iconWidgets = new();

        private float widthOverride;
        public float Width => shadowBar.rect.rect.width + widthOverride;
        public float Height => shadowBar.rect.rect.height;
        public float YMax => shadowBar.rect.anchoredPosition.y;
        public float YMin => YMax - shadowBar.rect.rect.height;

        // The widget's text is not set until after Pool.Draw.
        // So TIs must be added to a dict after so Text can be used as a Key if necessary.
        public void Finalize(KSelectable selectable)
        {
            this.selectable = selectable;

            foreach (var (entry, key, data) in tiDatas)
            {
                var ti = TextInfo.Create(entry, key, data);

                // Klei keeps causing duplicate texts, so they must be checked for.
                // This means duplicates won't be accounted for when grouping, but that should be fine.
                if (!textInfos.TryGetValue(ti.Key, out _))
                    textInfos[ti.Key] = ti;
            }
        }

        public void AddWidget(Entry entry, GameObject prefab, string key, object data)
        {
            var skin = HoverTextScreen.Instance.drawer.skin;

            if (prefab == skin.textWidget.gameObject)
                tiDatas.Add((entry, key, data));
            else if (prefab == skin.iconWidget.gameObject)
                iconWidgets.Add(entry);
            else if (prefab == skin.shadowBarWidget.gameObject)
                shadowBar = entry;
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

            foreach (var data in tiDatas)
                data.entry.rect.anchoredPosition += shift;
        }

        public void Resize(float width)
        {
            shadowBar.rect.sizeDelta = new Vector2(width, shadowBar.rect.sizeDelta.y);

            if (selectBorder.rect != null)
                selectBorder.rect.sizeDelta = new Vector2(width + 2f, selectBorder.rect.sizeDelta.y);
        }

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

            var potentialWidthOverride = newWidthDelta + HoverTextScreen.Instance.drawer.skin.shadowBarBorder.x * 2f;
            if (potentialWidthOverride > Width)
                widthOverride = potentialWidthOverride - Width;
        }
    }
}
