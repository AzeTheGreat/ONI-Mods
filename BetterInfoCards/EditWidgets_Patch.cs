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
            //// No point in arranging.
            //if (infoCards.Count < 4)
            //    return;

            //// Determine column numbers
            //float totalLength = infoCards.Sum(x => x.shadowBar.rect.rect.height);
            //float averageWidth = infoCards.Average(x => x.shadowBar.rect.rect.width);

            //int columns = Mathf.CeilToInt(Mathf.Sqrt(targetAspectRatio * totalLength / averageWidth));
            //float lengthPerColumn = totalLength / columns;

            //// Build "grid"
            //float currentColLength = 0f;
            //float currentOffsetX = 0f;
            //float currentOffsetY = 0f;
            //int colStartIndex = 0;
            //for (int i = 0; i < infoCards.Count; i++)
            //{
            //    infoCards[i].Translate(currentOffsetX, currentOffsetY);
            //    currentColLength += infoCards[i].shadowBar.rect.rect.height;

            //    if (currentColLength > lengthPerColumn && i < infoCards.Count - 1)
            //    {
            //        currentColLength = 0f;
            //        currentOffsetX += infoCards.GetRange(colStartIndex, i-colStartIndex+1).Max(x => x.shadowBar.rect.rect.width) + shadowBarSpacingX;
            //        currentOffsetY = infoCards[0].shadowBar.rect.anchoredPosition.y - infoCards[i+1].shadowBar.rect.anchoredPosition.y;
            //        colStartIndex = i;
            //    }
            //}

            float minY = -infoCards[0].shadowBar.rect.gameObject.GetComponentInParent<Canvas>().pixelRect.height / infoCards[0].shadowBar.rect.gameObject.GetComponentInParent<Canvas>().scaleFactor;

            float offsetX = 0f;

            var gridInfo = new List<ColumnInfo>() { };
            var colInfo = new ColumnInfo
            {
                offsetX = 0f,
                offsetY = 0f,
                infoCards = new List<InfoCard>()
            };

            foreach (var infoCard in infoCards)
            {
                if (infoCard.YMin + colInfo.offsetY < minY)
                {
                    offsetX += colInfo.maxXInCol + shadowBarSpacingX;

                    colInfo.isLeftCol = true;

                    gridInfo.Add(colInfo);
                    colInfo = new ColumnInfo
                    {
                        offsetX = offsetX,
                        offsetY = infoCards[0].YMax - infoCard.YMax,
                        infoCards = new List<InfoCard>()
                    };
                }

                if (infoCard.Width > colInfo.maxXInCol)
                    colInfo.maxXInCol = infoCard.Width;

                colInfo.infoCards.Add(infoCard);
            }

            gridInfo.Add(colInfo);

            foreach (var info in gridInfo)
            {
                foreach (var card in info.infoCards)
                {
                    card.Translate(info.offsetX, info.offsetY);

                    if (info.isLeftCol)
                        card.shadowBar.rect.sizeDelta = new Vector2(info.maxXInCol, card.shadowBar.rect.sizeDelta.y);
                }
            }
        }

        private class ColumnInfo
        {
            public float offsetX;
            public float offsetY;
            public List<InfoCard> infoCards;
            public float maxXInCol = 0f;
            public bool isLeftCol = false;
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

            public float Width { get { return shadowBar.rect.rect.width; } }
            public float YMax { get { return shadowBar.rect.anchoredPosition.y; } }
            public float YMin { get { return YMax - shadowBar.rect.rect.height; } }

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
