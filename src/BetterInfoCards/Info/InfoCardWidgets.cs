using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCardWidgets
    {
        private static readonly Dictionary<Type, Func<object, RectTransform>> rectAccessors = new();
        private static readonly object rectAccessorLock = new();

        public List<RectTransform> widgets = new();
        public RectTransform shadowBar;
        public RectTransform selectBorder;
        public Vector2 offset = new();

        public float YMax => shadowBar != null ? shadowBar.anchoredPosition.y : 0f;
        public float YMin => YMax - Height;
        public float Width => shadowBar != null ? shadowBar.rect.width : 0f;
        public float Height => shadowBar != null ? shadowBar.rect.height : 0f;

        public void AddWidget(object entry, GameObject prefab)
        {
            if (entry == null)
                return;

            var rect = ExtractRect(entry);
            if (rect == null)
                return;

            var skin = HoverTextScreen.Instance.drawer.skin;

            if (prefab == skin.shadowBarWidget.gameObject)
                shadowBar = rect;
            else if (prefab == skin.selectBorderWidget.gameObject)
                selectBorder = rect;
            else
                widgets.Add(rect);
        }

        public void Translate(float x)
        {
            var shift = new Vector2(x, offset.y);

            if (shadowBar != null)
                shadowBar.anchoredPosition += shift;

            if (selectBorder != null)
                selectBorder.anchoredPosition += shift;

            foreach (var widget in widgets)
                if (widget != null)
                    widget.anchoredPosition += shift;
        }

        public void SetWidth(float width)
        {
            if (shadowBar == null)
                return;

            // Modifying existing SBs triggers rebuilds somewhere and has a major impact on performance.
            // Genius idea from Peter to just add new ones to fill the gap.
            Vector2 newShadowBarPosition = shadowBar.anchoredPosition + new Vector2(shadowBar.sizeDelta.x, 0f);
            dynamic drawer = InterceptHoverDrawer.drawerInstance;
            var newShadowBar = ExtractRect(drawer.shadowBars.Draw(newShadowBarPosition));
            if (newShadowBar != null)
                newShadowBar.sizeDelta = new Vector2(width - shadowBar.sizeDelta.x, shadowBar.sizeDelta.y);

            if (selectBorder != null)
                selectBorder.sizeDelta = new Vector2(width + 2f, selectBorder.sizeDelta.y);
        }

        private static RectTransform ExtractRect(object entry)
        {
            var type = entry.GetType();

            if (!rectAccessors.TryGetValue(type, out var accessor))
            {
                lock (rectAccessorLock)
                {
                    if (!rectAccessors.TryGetValue(type, out accessor))
                    {
                        accessor = CreateAccessor(type);
                        rectAccessors[type] = accessor;
                    }
                }
            }

            return accessor(entry);
        }

        private static Func<object, RectTransform> CreateAccessor(Type type)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var rectField = type.GetField("rect", flags);
            if (rectField != null && typeof(RectTransform).IsAssignableFrom(rectField.FieldType))
                return entry => (RectTransform)rectField.GetValue(entry);

            var rectProperty = type.GetProperty("rect", flags);
            if (rectProperty != null && typeof(RectTransform).IsAssignableFrom(rectProperty.PropertyType))
                return entry => (RectTransform)rectProperty.GetValue(entry);

            return _ => null;
        }
    }
}
