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
        private const float targetAspectRatio = 1.5f;

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
                var infoCard = new InfoCard(shadowBar)
                {
                    iconWidgets = GetEntries(ref iconIndex, GetWidgets_Patch.iconWidgets, shadowBar),
                    textWidgets = GetEntries(ref textIndex, GetWidgets_Patch.textWidgets, shadowBar)
                };

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

            // Determine column numbers
            float totalLength = infoCards.Sum(x => x.ShadowBar.rect.rect.height);
            float averageWidth = infoCards.Average(x => x.ShadowBar.rect.rect.width);

            int columns = Mathf.CeilToInt(Mathf.Sqrt(targetAspectRatio * totalLength / averageWidth));
            float lengthPerColumn = totalLength / columns;

            // Build "grid"
            float currentColLength = 0f;
            float currentOffsetX = 0f;
            float currentOffsetY = 0f;
            int colStartIndex = 0;
            for (int i = 0; i < infoCards.Count; i++)
            {
                infoCards[i].Translate(currentOffsetX, currentOffsetY);
                currentColLength += infoCards[i].ShadowBar.rect.rect.height;

                if (currentColLength > lengthPerColumn && i < infoCards.Count - 1)
                {
                    currentColLength = 0f;
                    currentOffsetX += infoCards.GetRange(colStartIndex, i-colStartIndex+1).Max(x => x.ShadowBar.rect.rect.width) + 5f;
                    currentOffsetY = infoCards[0].ShadowBar.rect.anchoredPosition.y - infoCards[i+1].ShadowBar.rect.anchoredPosition.y;
                    colStartIndex = i;
                }
            }
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
