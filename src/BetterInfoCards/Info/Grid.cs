using System.Collections.Generic;
using UnityEngine;

namespace BetterInfoCards
{
    public class Grid
    {
        private const float shadowBarSpacing = 4f;

        List<Column> columns = new();

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

        public Grid(List<InfoCardWidgets> cards, float topY)
        {
            if (cards.Count == 0)
                return;

            var offset = new Vector2(0f, topY);

            columns.Clear();
            var col = new Column();

            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];

                // If the first one can't fit, put it down anyways otherwise they all get shifted over by the shadow bar spacing.
                if (offset.y - card.Height < MinY && i > 0)
                {
                    offset.x += col.maxXInCol + shadowBarSpacing;

                    columns.Add(col);
                    col = new() { offsetX = offset.x };
                    offset.y = topY;
                }

                card.offset.y = offset.y - card.YMax;
                offset.y -= card.Height + shadowBarSpacing;

                if (card.Width > col.maxXInCol)
                    col.maxXInCol = card.Width;

                col.cards.Add(card);
            }

            columns.Add(col);
        }

        public void MoveAndResizeInfoCards()
        {
            for (int i = columns.Count - 1; i >= 0; i--)
            {
                float colToRightYMin = float.MaxValue;

                if (i != columns.Count - 1)
                    colToRightYMin = columns[i + 1].YMin;

                columns[i].MoveAndResize(colToRightYMin);
            }
        }
    }
}
