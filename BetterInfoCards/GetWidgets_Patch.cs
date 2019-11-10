using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    [HarmonyPatch]
    public class GetWidgets_Patch
    {
        public static int callNumber;

        public static List<Entry> shadowBars;
        public static List<Entry> iconWidgets;
        public static List<Entry> textWidgets;
        public static List<Entry> selectBorders;

        static MethodBase TargetMethod()
        {
            return AccessTools.FirstInner(typeof(HoverTextDrawer), x => x.IsGenericType).MakeGenericType(typeof(object)).GetMethod("EndDrawing");
        }

        static void Postfix(ref List<Entry> ___entries, int ___drawnWidgets, object __instance)
        {
            var drawnEntries = new List<Entry>();
            for (int i = 0; i < ___drawnWidgets; i++)
            {
                drawnEntries.Add(___entries[i]);
            }

            switch (callNumber)
            {
                case 0:
                    shadowBars = drawnEntries;
                    break;
                case 1:
                    iconWidgets = drawnEntries;
                    break;
                case 2:
                    textWidgets = drawnEntries;
                    break;
                case 3:
                    selectBorders = drawnEntries;
                    break;
                default:
                    throw new Exception("GetWidgets is out of sync with the game.");
            }

            callNumber++;
        }
    }

    public struct Entry
    {
        public MonoBehaviour widget;
        public RectTransform rect;
    }

    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
    public class EditWidgets_Patch
    {
        private const float maxColumnHeight = 1200f;

        private static List<InfoCard> infoCards;

        static void Prefix()
        {
            GetWidgets_Patch.callNumber = 0;
        }

        static void Postfix()
        {
            FormInfoCards();
            ArrangeInfoCards();
        }

        private static void FormInfoCards()
        {
            infoCards = new List<InfoCard>();

            int iconIndex = 0;
            int textIndex = 0;

            // For each shadow bar, create an info card and add the relevant icons and texts.
            foreach (var shadowBar in GetWidgets_Patch.shadowBars)
            {
                var infoCard = new InfoCard(shadowBar);

                infoCard.iconWidgets = GetEntries(ref iconIndex, GetWidgets_Patch.iconWidgets, shadowBar);
                infoCard.textWidgets = GetEntries(ref textIndex, GetWidgets_Patch.textWidgets, shadowBar);

                infoCards.Add(infoCard);
            }

            // If something is selected, add the border to the correct info card.
            if (GetWidgets_Patch.selectBorders.Count > 0)
            {
                var number = GetWidgets_Patch.selectBorders[0].rect.anchoredPosition.y;
                var closestInfoCard = infoCards.Aggregate((x, y) => Math.Abs(x.ShadowBar.rect.anchoredPosition.y - number) < Math.Abs(y.ShadowBar.rect.anchoredPosition.y - number) ? x : y);
                closestInfoCard.selectBorder = GetWidgets_Patch.selectBorders[0];
            }
        }

        private static void ArrangeInfoCards()
        {
            // No point in arranging.
            if (infoCards.Count < 4)
                return;

            // Get total length
            float length = 0f;
            foreach (var infoCard in infoCards)
            {
                length += infoCard.ShadowBar.rect.rect.height;
            }

            int minColumns = Mathf.CeilToInt(length / maxColumnHeight);

            // Get the max width shadow bar
            float maxWidth = 0f;
            foreach (var infoCard in infoCards)
            {
                if (infoCard.ShadowBar.rect.rect.width > maxWidth)
                    maxWidth = infoCard.ShadowBar.rect.rect.width;
            }

            int startColumns = Mathf.CeilToInt(length / maxWidth);
        }

        private static List<Entry> GetEntries(ref int index, List<Entry> widgets, Entry shadowBar)
        {
            var entries = new List<Entry>();

            for (int i = index; i < widgets.Count; i++)
            {
                var widget = widgets[i];

                if (shadowBar.Contains(widget))
                    entries.Add(widget);
                else
                {
                    index = i;
                    break;
                }
            }
            return entries;
        }

        private class InfoCard
        {
            public Entry ShadowBar;
            public List<Entry> iconWidgets;
            public List<Entry> textWidgets;
            public Entry selectBorder;

            public InfoCard(Entry shadowBar)
            {
                ShadowBar = shadowBar;
                iconWidgets = new List<Entry>();
                textWidgets = new List<Entry>();
            }

            public void Translate(float x, float y)
            {
                ShadowBar.rect.anchoredPosition = new Vector2(ShadowBar.rect.anchoredPosition.x + x, ShadowBar.rect.anchoredPosition.y + y);
                if(selectBorder.rect != null)
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
        }
    }

    public static class Extensions
    {
        public static bool Contains(this Entry main, Entry sub)
        {
            return sub.rect.anchoredPosition.y > (main.rect.offsetMin.y - HoverTextScreen.Instance.drawer.skin.selectBorder.x)
                    && sub.rect.anchoredPosition.y < main.rect.offsetMax.y;
        }
    }
}
