using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterInfoCards
{
    class GridInfo
    {
        private const float shadowBarSpacingX = 5f;

        List<ColumnInfo> columnInfos = new List<ColumnInfo>();

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

        public GridInfo(List<InfoCard> infoCards)
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

            columnInfos.Clear();
            var colInfo = new ColumnInfo();

            foreach (var card in infoCards)
            {
                if (card.YMin + colInfo.offsetY < MinY)
                {
                    offsetX += colInfo.maxXInCol + shadowBarSpacingX;

                    columnInfos.Add(colInfo);
                    colInfo = new ColumnInfo
                    {
                        offsetX = offsetX,
                        offsetY = infoCards[0].YMax - card.YMax
                    };
                }

                if (card.Width > colInfo.maxXInCol)
                    colInfo.maxXInCol = card.Width;

                colInfo.infoCards.Add(card);
            }
            columnInfos.Add(colInfo);
        }

        public void MoveAndResizeInfoCards()
        {
            for (int i = columnInfos.Count - 1; i >= 0; i--)
            {
                float colToRightYMin = float.MaxValue;

                if (i != columnInfos.Count - 1)
                    colToRightYMin = columnInfos[i + 1].YMin;

                columnInfos[i].MoveAndResize(colToRightYMin);
            }
        }
    }
}
