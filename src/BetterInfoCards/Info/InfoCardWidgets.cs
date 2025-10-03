using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace BetterInfoCards
{
    public class InfoCardWidgets
    {
        private static readonly Dictionary<Type, Func<object, RectTransform>> rectAccessors = new();
        private static readonly object rectAccessorLock = new();
        private const BindingFlags memberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static readonly FieldInfo shadowBarsField = typeof(HoverTextDrawer).GetField("shadowBars", memberBindingFlags);
        private static readonly MethodInfo shadowBarsDrawMethod = shadowBarsField?.FieldType.GetMethod(
            "Draw",
            memberBindingFlags,
            binder: null,
            types: new[] { typeof(Vector2) },
            modifiers: null);

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
            var drawer = InterceptHoverDrawer.drawerInstance;
            if (drawer == null)
                return;

            if (shadowBarsField == null)
            {
                Debug.LogWarning("[BetterInfoCards] Unable to locate HoverTextDrawer.shadowBars field.");
                return;
            }

            if (shadowBarsDrawMethod == null)
            {
                Debug.LogWarning("[BetterInfoCards] Unable to locate Draw(Vector2) on HoverTextDrawer.shadowBars.");
                return;
            }

            var shadowBars = shadowBarsField.GetValue(drawer);
            if (shadowBars == null)
            {
                Debug.LogWarning("[BetterInfoCards] HoverTextDrawer.shadowBars instance is null.");
                return;
            }

            var newShadowBar = ExtractRect(shadowBarsDrawMethod.Invoke(shadowBars, new object[] { newShadowBarPosition }));
            if (newShadowBar != null)
            {
                newShadowBar.sizeDelta = new Vector2(width - shadowBar.sizeDelta.x, shadowBar.sizeDelta.y);
                var graphic = newShadowBar.GetComponent<Graphic>();
                if (graphic != null)
                    CardTweaker.ApplyShadowBarColor(graphic);
            }

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
            var rectField = type.GetField("rect", memberBindingFlags);
            if (rectField != null && typeof(RectTransform).IsAssignableFrom(rectField.FieldType))
                return entry => (RectTransform)rectField.GetValue(entry);

            var rectProperty = type.GetProperty("rect", memberBindingFlags);
            if (rectProperty != null && typeof(RectTransform).IsAssignableFrom(rectProperty.PropertyType))
                return entry => (RectTransform)rectProperty.GetValue(entry);

            return _ => null;
        }
    }
}
