using System.Collections.Generic;
using System.Linq;

namespace BetterInfoCards
{
    public class Column
    {
        private const float isOverlappedThreshold = 10f;

        public float offsetX = 0f;
        public List<InfoCardWidgets> cards = new();
        public float maxXInCol = 0f;
        public float YMin => cards.Last().YMin;

        public void MoveAndResize(float colToRightYMin)
        {
            foreach (var widget in cards)
            {
                widget.Translate(offsetX);

                if (colToRightYMin < widget.YMax - isOverlappedThreshold)
                    widget.SetWidth(maxXInCol);
            }
        }
    }
}
