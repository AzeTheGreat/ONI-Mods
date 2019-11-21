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

        public GridInfo(List<DisplayCard> displayCards)
        {
            float offsetX = 0f;

            columnInfos.Clear();
            var colInfo = new ColumnInfo();

            foreach (DisplayCard card in displayCards)
            {
                if (card.YMin + colInfo.offsetY < MinY)
                {
                    offsetX += colInfo.maxXInCol + shadowBarSpacingX;

                    columnInfos.Add(colInfo);
                    colInfo = new ColumnInfo
                    {
                        offsetX = offsetX,
                        offsetY = displayCards[0].YMax - card.YMax
                    };
                }

                if (card.Width > colInfo.maxXInCol)
                    colInfo.maxXInCol = card.Width;

                colInfo.displayCards.Add(card);
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
