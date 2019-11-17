using Harmony;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (IsLayoutChanged())
                FormInfoCards();

            if (HasMouseMovedEnough())
                ArrangeInfoCards();

            if (IsSelectedChanged())
                FormSelectBorder();

            AlterInfoCards();
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            //for (int i = 0; i < 10000; i++)
            //{
            //    AlterInfoCards();
            //}
            //stopwatch.Stop();
            //Debug.Log("Alter Info Cards: " + stopwatch.ElapsedMilliseconds);
        }

        private static float[] cachedShadowWidths = new float[0];
        private static float[] cachedShadowHeights = new float[0];

        private const float equalsThreshold = 0.001f;

        // This could theoretically fail if objects with the same exact shadow bar width and height were swapped in the same frame...
        private static bool IsLayoutChanged()
        {
            if (DrawnWidgets.shadowBars.Count != cachedShadowWidths.Length)
                return true;

            for (int i = 0; i < DrawnWidgets.shadowBars.Count; i++)
            {
                Rect rect = DrawnWidgets.shadowBars[i].rect.rect;

                if (!NearEquals(rect.height, cachedShadowHeights[i], equalsThreshold) || !NearEquals(rect.width, cachedShadowWidths[i], equalsThreshold))
                {
                    return true;
                }
                    
            }
            return false;
        }

        private static Vector3 cachedSelectBorder;

        private static bool IsSelectedChanged()
        {
            Vector3 currentSelectBorder;
            if (DrawnWidgets.selectBorders.Count > 0)
                currentSelectBorder = DrawnWidgets.selectBorders[0].rect.anchoredPosition;
            else
                currentSelectBorder = Vector3.positiveInfinity;

            if (Vector3.Distance(currentSelectBorder, cachedSelectBorder) > 0.0001f)
            {
                cachedSelectBorder = currentSelectBorder;
                return true;
            }
            return false;
        }

        private static Vector3 cachedMousePos = Vector3.positiveInfinity;

        private static bool HasMouseMovedEnough()
        {
            Vector3 cursorPos = Input.mousePosition;

            if (cursorPos != cachedMousePos)
            {
                cachedMousePos = cursorPos;
                return true;
            }

            return false;
        }

        private static bool NearEquals(float f1, float f2, float diff)
        {
            if (Math.Abs(f1 - f2) < diff)
                return true;
            else
                return false;
        }

        private static void FormInfoCards()
        {
            infoCards = new List<InfoCard>();

            int iconIndex = 0;
            int textIndex = 0;

            cachedShadowWidths = new float[DrawnWidgets.shadowBars.Count];
            cachedShadowHeights = new float[DrawnWidgets.shadowBars.Count];

            // For each shadow bar, create an info card and add the relevant icons and texts.
            for (int i = 0; i < DrawnWidgets.shadowBars.Count; i++)
            {
                Entry shadowBar = DrawnWidgets.shadowBars[i];
                infoCards.Add(new InfoCard(shadowBar, ref iconIndex, ref textIndex));

                cachedShadowWidths[i] = shadowBar.rect.rect.width;
                cachedShadowHeights[i] = shadowBar.rect.rect.height; 
            }
        }

        private static void FormSelectBorder()
        {
            if (DrawnWidgets.selectBorders.Count > 0)
            {
                Vector2 currentSelectBorderPos = DrawnWidgets.selectBorders[0].rect.anchoredPosition;
                float number = DrawnWidgets.selectBorders[0].rect.anchoredPosition.y;
                InfoCard closestInfoCard = infoCards.Aggregate((x, y) => Math.Abs(x.YMax - number) < Math.Abs(y.YMax - number) ? x : y);

                if(cachedClosestInfoCard != null)
                    cachedClosestInfoCard.selectBorder = new Entry();

                cachedClosestInfoCard = closestInfoCard;

            }
        }

        private static List<ColumnInfo> gridInfo = new List<ColumnInfo>();

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

            float minY = -CameraController.Instance.uiCamera.pixelRect.height;

            float offsetX = 0f;

            gridInfo.Clear();
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
        }

        private static InfoCard cachedClosestInfoCard;

        private static void AlterInfoCards()
        {
            if (DrawnWidgets.selectBorders.Count > 0)
                cachedClosestInfoCard.selectBorder = DrawnWidgets.selectBorders[0];

            for (int i = gridInfo.Count - 1; i >= 0; i--)
            {
                ColumnInfo info = gridInfo[i];

                foreach (var card in info.infoCards)
                {
                    card.Translate(info.offsetX, info.offsetY);

                    if (i != gridInfo.Count - 1 && gridInfo[i + 1].YMin < card.YMax - 10f)
                    {
                        Vector2 newSizeDelta = new Vector2(info.maxXInCol, card.shadowBar.rect.sizeDelta.y);
                        card.shadowBar.rect.sizeDelta = newSizeDelta;

                        if (card.selectBorder.rect != null)
                            card.selectBorder.rect.sizeDelta = newSizeDelta;
                    }
                }
            }
        }

        private class ColumnInfo
        {
            public float offsetX;
            public float offsetY;
            public List<InfoCard> infoCards;
            public float maxXInCol = 0f;
            public float YMin { get { return infoCards.Last().YMin; } }
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
