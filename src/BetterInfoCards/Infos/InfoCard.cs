using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCard
    {
        public KSelectable selectable;

        private Entry shadowBar;
        private List<Entry> iconWidgets = new List<Entry>();
        public List<TextInfo> textInfos = new List<TextInfo>();
        public Entry selectBorder;

        public float Width => shadowBar.rect.rect.width;
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
                textInfos.Add(TextInfo.Create(entry, name, data));
            else if (prefab == skin.selectBorderWidget.gameObject)
                selectBorder = entry;
        }

        public void Translate(float x, float y)
        {
            shadowBar.rect.anchoredPosition = new Vector2(shadowBar.rect.anchoredPosition.x + x, shadowBar.rect.anchoredPosition.y + y);

            if (selectBorder.rect != null)
                selectBorder.rect.anchoredPosition = new Vector2(selectBorder.rect.anchoredPosition.x + x, selectBorder.rect.anchoredPosition.y + y);

            foreach (var icon in iconWidgets)
                icon.rect.anchoredPosition = new Vector2(icon.rect.anchoredPosition.x + x, icon.rect.anchoredPosition.y + y);

            foreach (var text in textInfos)
                text.TextEntry.rect.anchoredPosition = new Vector2(text.TextEntry.rect.anchoredPosition.x + x, text.TextEntry.rect.anchoredPosition.y + y);
        }

        public void Resize(float width)
        {
            shadowBar.rect.sizeDelta = new Vector2(width, shadowBar.rect.sizeDelta.y);

            if (selectBorder.rect != null)
                selectBorder.rect.sizeDelta = new Vector2(width + 2f, selectBorder.rect.sizeDelta.y);
        }

        public string GetTitleKey()
        {
            return ((LocText)textInfos[0].TextEntry.widget).text.RemoveCountSuffix();
        }

        public string GetTextKey()
        {
            var texts = new List<string>();
            for (int i = 1; i < textInfos.Count; i++)
            {
                texts.Add(textInfos[i].Key);
            }

            texts.Sort();
            return string.Join(null, texts.ToArray());
        }

        public void FormTextValues()
        {
            foreach (TextInfo textInfo in textInfos)
            {
                textInfo.FormTextResult();
            }
        }

        public List<string> GetTextOverrides(List<InfoCard> cards)
        {
            var overrides = new List<string>();

            for (int i = 0; i < textInfos.Count; i++)
            {
                var textInfo = textInfos[i];
                overrides.Add(textInfo.GetTextOverride(cards.Select(x => GetResult(cards, x, i)).ToList()));
            }

            return overrides;

            object GetResult(List<InfoCard> cards, InfoCard card, int i)
            {
                try
                {
                    return textInfos[i].Result;
                }
                catch (System.Exception ex)
                {
                    Debug.Log("Crash in Better Info Cards!");
                    Debug.Log("PLEASE send this complete output log to the mod author (Aze) via Steam, Discord, or Github!");
                    Debug.Log(ex);
                    Debug.Log("------------------------------------------------------------");

                    foreach (var srcCard in cards)
                    {
                        Debug.Log("Card: " + srcCard);
                        foreach (var item in srcCard.textInfos)
                            Debug.Log(item.Result);
                    }

                    Debug.Log("Card Crash: " + card);
                    foreach (var item in card.textInfos)
                        Debug.Log(item.Result);

                    throw;
                }
            }
        }

        public void Rename(List<string> overrides, bool forceUpdate = false)
        { 
            for (int i = 0; i < textInfos.Count; i++)
            {
                var widget = textInfos[i].TextEntry.widget as LocText;
                widget.text = overrides[i];

                if(forceUpdate)
                    widget.KForceUpdateDirty(); 
            }
        }

        public float GetWidthDelta(List<string> overrides)
        {
            Rename(overrides, true);

            float largestWdith = 0f;
            float indentWidth = 0f;

            for (int i = 0; i < textInfos.Count; i++)
            {
                var widget = textInfos[i].TextEntry.widget as LocText;

                float width = 0f;
                if (i > 0)
                    indentWidth = widget.rectTransform.anchoredPosition.x - ((LocText)textInfos[0].TextEntry.widget).rectTransform.anchoredPosition.x;
                width = widget.renderedWidth + indentWidth;

                if (width > largestWdith)
                    largestWdith = width;
            }

            return largestWdith + HoverTextScreen.Instance.drawer.skin.shadowBarBorder.x * 2f - Width;
        }
    }
}
