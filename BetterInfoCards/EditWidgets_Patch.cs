using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
    public class EditWidgets_Patch
    {
        private const float targetAspectRatio = 1.5f;
        private const float shadowBarSpacingX = 5f;

        private static List<InfoCard> infoCards;

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
            foreach (var shadowBar in DrawnWidgets.shadowBars)
            {
                infoCards.Add(new InfoCard(shadowBar, ref iconIndex, ref textIndex));
            }

            // If something is selected, add the border to the correct info card.
            if (DrawnWidgets.selectBorders.Count > 0)
            {
                var number = DrawnWidgets.selectBorders[0].rect.anchoredPosition.y;
                var closestInfoCard = infoCards.Aggregate((x, y) => Math.Abs(x.shadowBar.rect.anchoredPosition.y - number) < Math.Abs(y.shadowBar.rect.anchoredPosition.y - number) ? x : y);
                closestInfoCard.selectBorder = DrawnWidgets.selectBorders[0];
            }
        }

        private static void ArrangeInfoCards()
        {
            float minY = -infoCards[0].shadowBar.rect.gameObject.GetComponentInParent<Canvas>().pixelRect.height / infoCards[0].shadowBar.rect.gameObject.GetComponentInParent<Canvas>().scaleFactor;

            float offsetX = 0f;
            float offsetY = 0f;
            float maxXInCol = 0f;
            int card = 0;

            foreach (var infoCard in infoCards)
            {
                if (infoCard.shadowBar.rect.anchoredPosition.y - infoCard.shadowBar.rect.rect.height + offsetY < minY)
                {
                    offsetX += maxXInCol + shadowBarSpacingX;
                    offsetY = infoCards[0].shadowBar.rect.anchoredPosition.y - infoCard.shadowBar.rect.anchoredPosition.y;
                    maxXInCol = 0f;
                }

                if (infoCard.shadowBar.rect.rect.width > maxXInCol)
                    maxXInCol = infoCard.shadowBar.rect.rect.width;

                infoCard.Translate(offsetX, offsetY);
                card++;
            }
        }

        private static bool RectWithin(Entry main, Entry sub)
        {
            return sub.rect.anchoredPosition.y > main.rect.offsetMin.y && sub.rect.anchoredPosition.y < main.rect.offsetMax.y;
        }

        // Collect the game's widgets into a single class grouped by item to make manipulation easier.
        private class InfoCard
        {
            public Entry shadowBar;
            public List<Entry> iconWidgets;
            public List<Entry> textWidgets;
            public Entry selectBorder;

            public InfoCard(Entry shadowBar, ref int iconIndex, ref int textIndex)
            {
                this.shadowBar = shadowBar;
                iconWidgets = GetEntries(ref iconIndex, DrawnWidgets.iconWidgets);
                textWidgets = GetEntries(ref textIndex, DrawnWidgets.textWidgets);
            }

            public void Translate(float x, float y)
            {
                shadowBar.rect.anchoredPosition = new Vector2(shadowBar.rect.anchoredPosition.x + x, shadowBar.rect.anchoredPosition.y + y);

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
        }
    }
}
