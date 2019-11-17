using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    class WidgetModifier : KMonoBehaviour
    {
        private const float shadowBarSpacingX = 5f;

        public static WidgetModifier Instance { get; set; }

        private float[] cachedShadowWidths = new float[0];
        private float[] cachedShadowHeights = new float[0];
        private Vector3 cachedMousePos = Vector3.positiveInfinity;
        private Vector3 cachedSelectBorder = Vector3.positiveInfinity;
        private InfoCard cachedClosestInfoCard = null;

        private List<InfoCard> infoCards = new List<InfoCard>();
        private List<ColumnInfo> gridInfo = new List<ColumnInfo>();

        // The HoverTextScreen is initialized before CameraController
        private float _minY = float.MaxValue;
        private float MinY
        {
            get
            {
                if (_minY == float.MaxValue)
                    _minY = -CameraController.Instance?.uiCamera.pixelRect.height ?? float.MaxValue;
                return _minY;
            }
        }
            

        private const float equalsThreshold = 0.001f;

        public void ModifyWidgets()
        {
            if (IsLayoutChanged())
                FormInfoCards();

            if (HasMouseMovedEnough())
                ArrangeInfoCards();

            if (IsSelectedChanged())
                FormSelectBorder();

            AlterInfoCards();
        }

        // This could theoretically fail if objects with the same exact shadow bar width and height were swapped in the same frame...
        private bool IsLayoutChanged()
        {
            if (DrawnWidgets.Instance.shadowBars.Count != cachedShadowWidths.Length)
                return true;

            for (int i = 0; i < DrawnWidgets.Instance.shadowBars.Count; i++)
            {
                Rect rect = DrawnWidgets.Instance.shadowBars[i].rect.rect;

                if (!NearEquals(rect.height, cachedShadowHeights[i], equalsThreshold) || !NearEquals(rect.width, cachedShadowWidths[i], equalsThreshold))
                    return true;
            }
            return false;
        }

        private bool NearEquals(float f1, float f2, float diff)
        {
            if (Math.Abs(f1 - f2) < diff)
                return true;
            else
                return false;
        }

        private void FormInfoCards()
        {
            infoCards = new List<InfoCard>();

            int iconIndex = 0;
            int textIndex = 0;

            cachedShadowWidths = new float[DrawnWidgets.Instance.shadowBars.Count];
            cachedShadowHeights = new float[DrawnWidgets.Instance.shadowBars.Count];

            // For each shadow bar, create an info card and add the relevant icons and texts.
            for (int i = 0; i < DrawnWidgets.Instance.shadowBars.Count; i++)
            {
                Entry shadowBar = DrawnWidgets.Instance.shadowBars[i];
                infoCards.Add(new InfoCard(shadowBar, ref iconIndex, ref textIndex));

                cachedShadowWidths[i] = shadowBar.rect.rect.width;
                cachedShadowHeights[i] = shadowBar.rect.rect.height;
            }
        }

        private bool HasMouseMovedEnough()
        {
            Vector3 cursorPos = Input.mousePosition;

            if (cursorPos != cachedMousePos)
            {
                cachedMousePos = cursorPos;
                return true;
            }
            return false;
        }

        private void ArrangeInfoCards()
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
                if (infoCard.YMin + colInfo.offsetY < MinY)
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

        private bool IsSelectedChanged()
        {
            Vector3 currentSelectBorder;
            if (DrawnWidgets.Instance.selectBorders.Count > 0)
                currentSelectBorder = DrawnWidgets.Instance.selectBorders[0].rect.anchoredPosition;
            else
                currentSelectBorder = Vector3.positiveInfinity;

            if (Vector3.Distance(currentSelectBorder, cachedSelectBorder) > 0.0001f)
            {
                cachedSelectBorder = currentSelectBorder;
                return true;
            }
            return false;
        }

        private void FormSelectBorder()
        {
            if (DrawnWidgets.Instance.selectBorders.Count > 0)
            {
                Vector2 currentSelectBorderPos = DrawnWidgets.Instance.selectBorders[0].rect.anchoredPosition;
                float number = DrawnWidgets.Instance.selectBorders[0].rect.anchoredPosition.y;
                InfoCard closestInfoCard = infoCards.Aggregate((x, y) => Math.Abs(x.YMax - number) < Math.Abs(y.YMax - number) ? x : y);

                if (cachedClosestInfoCard != null)
                    cachedClosestInfoCard.selectBorder = new Entry();

                cachedClosestInfoCard = closestInfoCard;
            }
        }

        private void AlterInfoCards()
        {
            if (DrawnWidgets.Instance.selectBorders.Count > 0)
                cachedClosestInfoCard.selectBorder = DrawnWidgets.Instance.selectBorders[0];

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
    }
}
