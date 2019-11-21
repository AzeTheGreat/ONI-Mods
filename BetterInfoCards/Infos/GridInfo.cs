using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterInfoCards
{
    class GridInfo
    {
        private const float shadowBarSpacing = 5f;

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

        public GridInfo(List<DisplayCard> displayCards, float topY)
        {
            if (displayCards.Count == 0)
                return;

            Vector2 offset = new Vector2(0f, topY);

            columnInfos.Clear();
            var colInfo = new ColumnInfo();
    
            foreach (DisplayCard card in displayCards)
            {
                if (offset.y - card.Height < MinY)
                {
                    offset.x += colInfo.maxXInCol + shadowBarSpacing;

                    columnInfos.Add(colInfo);
                    colInfo = new ColumnInfo { offsetX = offset.x };
                    offset.y = topY;
                }

                card.offset.y = offset.y - card.YMax;
                offset.y -= card.Height + shadowBarSpacing;

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
