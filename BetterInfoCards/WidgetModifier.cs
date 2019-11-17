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

        private Vector3 cachedMousePos = Vector3.positiveInfinity;
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

        public void ModifyWidgets()
        {
            if (DrawnWidgets.Instance.IsLayoutChanged())
                infoCards = DrawnWidgets.Instance.FormInfoCards();

            if (HasMouseMovedEnough())
                FormGridInfo();

            if (DrawnWidgets.Instance.IsSelectedChanged())
                FormSelectBorder();

            AlterInfoCards();
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

        private void FormGridInfo()
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

        private void FormSelectBorder()
        {
            float number = DrawnWidgets.Instance.selectPos;
            if (number != float.MaxValue)
            {
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
                        card.Resize(info.maxXInCol);
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
