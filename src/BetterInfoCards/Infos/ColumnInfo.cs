using System.Collections.Generic;
using System.Linq;

namespace BetterInfoCards
{
    public class ColumnInfo
    {
        private const float isOverlappedThreshold = 10f;

        public float offsetX = 0f;
        public List<ICWidgetData> displayCards = new();
        public float maxXInCol = 0f;
        public float YMin => displayCards.Last().YMin;

        public void MoveAndResize(float colToRightYMin)
        {
            foreach (var card in displayCards)
            {
                card.Translate(offsetX);

                if (colToRightYMin < card.YMax - isOverlappedThreshold)
                    card.SetWidth(maxXInCol);
            }
        }
    }
}
