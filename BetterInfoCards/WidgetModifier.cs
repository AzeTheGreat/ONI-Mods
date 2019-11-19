using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    class WidgetModifier
    {
        public static WidgetModifier Instance { get; set; }

        private Vector3 cachedMousePos = Vector3.positiveInfinity;
        private InfoCard cachedClosestInfoCard = null;

        private GridInfo gridInfo = null;
        private List<InfoCard> infoCards = new List<InfoCard>();

        public void ModifyWidgets()
        {
            if (DrawnWidgets.Instance.IsLayoutChanged())
            {
                infoCards = DrawnWidgets.Instance.FormInfoCards();
            }

            if (HasMouseMovedEnough())
                gridInfo = new GridInfo(infoCards);                

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

            gridInfo.MoveAndResizeInfoCards();
        }
    }

    public class ColumnInfo
    {
        public float offsetX = 0f;
        public float offsetY = 0f;
        public List<InfoCard> infoCards = new List<InfoCard>();
        public float maxXInCol = 0f;
        public float YMin { get { return infoCards.Last().YMin; } }
    }
}
