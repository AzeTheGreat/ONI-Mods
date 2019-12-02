using System;
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

        private List<StatusItemGroup.Entry> statusItemEntries = new List<StatusItemGroup.Entry>();

        public int quantity = 0;
        public Dictionary<string, ValueType> textValues = new Dictionary<string, ValueType>();

        private string cachedTitle = string.Empty;

        public string Title
        {
            get
            {
                if(cachedTitle == string.Empty)
                {
                    cachedTitle = ((LocText)textWidgets[0].widget).text;

                    var charStack = new Stack<char>();
                    int i;
                    for (i = cachedTitle.Length - 1; i >= 0; i--)
                    {
                        if (!char.IsDigit(cachedTitle[i]))
                            break;

                        charStack.Push(cachedTitle[i]);
                    }
                    
                    if (int.TryParse(new string(charStack.ToArray()), out quantity))
                        cachedTitle = cachedTitle.Remove(i - 1, cachedTitle.Length - i + 1);
                    else
                        quantity = 1;
                }
                return cachedTitle;
            }
            set { ((LocText)textWidgets[0].widget).text = value; }
        }

        public InfoCard(Entry shadowBar, List<Entry> icons, List<Entry> texts, List<StatusItemGroup.Entry> entries, int gridPos, ref int iconIndex, ref int textIndex)
        {
            this.shadowBar = shadowBar;
            iconWidgets = GetEntries(ref iconIndex, icons);
            textWidgets = GetEntries(ref textIndex, texts);
            statusItemEntries = entries;
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

        public string GetTextKey()
        {
            var texts = new List<string>();
            foreach (var item in statusItemEntries)
            {
                texts.Add(item.item?.Name ?? string.Empty);
            }
            texts.Sort();

            return string.Join(null, texts.ToArray()); ;
        }

        public void FormTextValues()
        {
            Debug.Log(statusItemEntries.Count);
            textValues.Clear();
            foreach (var item in statusItemEntries)
            {
                string name = item.item?.Name ?? string.Empty;
                if (StatusDataManager.statusConverter.TryGetValue(name, out StatusDataManager.StatusData statusData))
                    textValues[name] = statusData.getStatusValue(item.data);
            }
        }
    }
}
