using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCard
    {
        private Entry shadowBar;
        private List<Entry> iconWidgets;
        private List<Entry> textWidgets;
        public Entry selectBorder;

        public float Width { get { return shadowBar.rect.rect.width; } }
        public float Height { get { return shadowBar.rect.rect.height; } }
        public float YMax { get { return shadowBar.rect.anchoredPosition.y; } }
        public float YMin { get { return YMax - shadowBar.rect.rect.height; } }

        private List<TextInfo> statusDatas = new List<TextInfo>();

        public Dictionary<string, object> textValues = new Dictionary<string, object>();

        public InfoCard(Entry shadowBar, List<Entry> icons, List<Entry> texts, List<TextInfo> statusDatas, int gridPos, ref int iconIndex, ref int textIndex)
        {
            this.shadowBar = shadowBar;
            iconWidgets = GetEntries(ref iconIndex, icons);
            textWidgets = GetEntries(ref textIndex, texts);
            this.statusDatas = statusDatas;
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

        // TODO: Check, but pretty sure this is actually a width not an x?
        public void Resize(float newX)
        {
            shadowBar.rect.sizeDelta = new Vector2(newX, shadowBar.rect.sizeDelta.y);

            if (selectBorder.rect != null)
                selectBorder.rect.sizeDelta = new Vector2(newX + 2f, selectBorder.rect.sizeDelta.y);
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

        public string GetTitleKey()
        {
            string text = ((LocText)textWidgets[0].widget).text;

            var charStack = new Stack<char>();
            int i;
            for (i = text.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(text[i]))
                    break;

                charStack.Push(text[i]);
            }

            text = text.Remove(i - 1, text.Length - i + 1);

            return text;
        }

        public string GetTextKey()
        {
            var texts = new List<string>();
            foreach (TextInfo status in statusDatas)
            {
                texts.Add(status.name);
            }
            texts.Sort();

            return string.Join(null, texts.ToArray());
        }

        public void FormTextValues()
        {
            textValues.Clear();
            foreach (TextInfo status in statusDatas)
            {
                string name = status.name;
                if (StatusDataManager.statusConverter.TryGetValue(name, out var statusData))
                    textValues[name] = statusData.GetTextValue(status.data);
            }
        }

        public List<string> GetTextOverrides(List<InfoCard> cards)
        {
            var overrides = new List<string>();

            for (int i = 0; i < statusDatas.Count; i++)
            {
                TextInfo status = statusDatas[i];
                string name = status.name;
                string original = ((LocText)textWidgets[i].widget).text;

                if (StatusDataManager.statusConverter.TryGetValue(name, out var statusData))
                    overrides.Add(statusData.GetTextOverride(original, cards.Select(x => x.textValues[name]).ToList()));
                else
                    overrides.Add(original);
            }

            return overrides;
        }

        // TODO: cache width checking
        public void Rename(List<string> overrides, bool forceUpdate = false)
        { 
            for (int i = 0; i < textWidgets.Count; i++)
            {
                var widget = textWidgets[i].widget as LocText;
                widget.text = overrides[i];

                if(forceUpdate)
                    widget.KForceUpdateDirty(); 
            }
        }

        public float GetWidthDelta(List<string> overrides)
        {
            Rename(overrides, true);

            float largestWidth = 0f;
            for (int i = 0; i < textWidgets.Count; i++)
            {
                var widget = textWidgets[i].widget as LocText;

                if (widget.renderedWidth > largestWidth)
                    largestWidth = widget.renderedWidth;
            }

            return largestWidth + HoverTextScreen.Instance.drawer.skin.shadowBarBorder.x * 2f - Width;
        }
    }
}
