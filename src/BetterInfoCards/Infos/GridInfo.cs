using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    public class GridInfo
    {
        private const float shadowBarSpacing = 4f;

        List<ColumnInfo> columnInfos = new List<ColumnInfo>();

        // The HoverTextScreen is initialized before CameraController
        private float _minY = float.MaxValue;
        private float MinY
        {
            get
            {
                if (_minY == float.MaxValue)
                {
                    var canvas = HoverTextScreen.Instance.gameObject.GetComponentInParent<Canvas>();
                    _minY = -canvas.pixelRect.height / canvas.scaleFactor;
                }
                   
                return _minY;
            }
        }

        public GridInfo(List<DisplayCard> displayCards, float topY)
        {
            if (displayCards.Count == 0)
                return;

            var offset = new Vector2(0f, topY);

            columnInfos.Clear();
            var colInfo = new ColumnInfo();

            for (int i = 0; i < displayCards.Count; i++)
            {
                DisplayCard card = displayCards[i];

                // If the first one can't fit, put it down anyways otherwise they all get shifted over by the shadow bar spacing.
                if (offset.y - card.Height < MinY && i > 0)
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
