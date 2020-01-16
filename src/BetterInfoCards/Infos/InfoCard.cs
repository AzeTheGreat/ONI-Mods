﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCard
    {
        public KSelectable selectable;

        private Entry shadowBar;
        private List<Entry> iconWidgets = new List<Entry>();
        private List<Entry> textWidgets = new List<Entry>();
        public Entry selectBorder;

        private List<TextInfo> statusDatas = new List<TextInfo>();

        public Dictionary<string, object> textValues = new Dictionary<string, object>();

        public float Width { get { return shadowBar.rect.rect.width; } }
        public float Height { get { return shadowBar.rect.rect.height; } }
        public float YMax { get { return shadowBar.rect.anchoredPosition.y; } }
        public float YMin { get { return YMax - shadowBar.rect.rect.height; } }

        public void AddData(List<TextInfo> statusDatas, KSelectable selectable)
        {
            this.statusDatas = statusDatas;
            this.selectable = selectable;
        }

        public void AddWidget(Entry entry, GameObject prefab)
        {
            var skin = HoverTextScreen.Instance.drawer.skin;

            if (prefab == skin.shadowBarWidget.gameObject)
                shadowBar = entry;
            else if (prefab == skin.iconWidget.gameObject)
                iconWidgets.Add(entry);
            else if (prefab == skin.textWidget.gameObject)
                textWidgets.Add(entry);
            else if (prefab == skin.selectBorderWidget.gameObject)
                selectBorder = entry;
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

        public string GetTitleKey()
        {
            return ((LocText)textWidgets[0].widget).text.RemoveCountSuffix();
        }

        public string GetTextKey()
        {
            var texts = new List<string>();
            for (int i = 0; i < textWidgets.Count; i++)
            {
                TextInfo status = statusDatas[i];
                if (status != null && StatusDataManager.statusConverter.ContainsKey(status.name))
                    texts.Add(status.name);
                else
                    texts.Add(((LocText)textWidgets[i].widget).text);
            }

            texts.Sort();
            return string.Join(null, texts.ToArray());
        }

        public void FormTextValues()
        {
            textValues.Clear();
            foreach (TextInfo status in statusDatas)
            {
                if(status != null)
                {
                    string name = status.name;
                    if (StatusDataManager.statusConverter.TryGetValue(name, out var statusData))
                        textValues[status.name] = statusData.GetTextValue(status.data);
                }
            }
        }

        public List<string> GetTextOverrides(List<InfoCard> cards)
        {
            var overrides = new List<string>();

            for (int i = 0; i < textWidgets.Count; i++)
            {
                string original = ((LocText)textWidgets[i].widget).text;
                string name = string.Empty;

                if(statusDatas[i] != null)
                    name = statusDatas[i].name;

                if (StatusDataManager.statusConverter.TryGetValue(name, out var statusData))
                    overrides.Add(statusData.GetTextOverride(original, cards.Select(x => x.textValues[name]).ToList()));
                else
                    overrides.Add(original);
            }

            return overrides;
        }

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

            float largestWdith = 0f;
            float indentWidth = 0f;

            for (int i = 0; i < textWidgets.Count; i++)
            {
                var widget = textWidgets[i].widget as LocText;

                float width = 0f;
                if (i > 0)
                    indentWidth = widget.rectTransform.anchoredPosition.x - ((LocText)textWidgets[0].widget).rectTransform.anchoredPosition.x;
                width = widget.renderedWidth + indentWidth;

                if (width > largestWdith)
                    largestWdith = width;
            }

            return largestWdith + HoverTextScreen.Instance.drawer.skin.shadowBarBorder.x * 2f - Width;
        }
    }
}