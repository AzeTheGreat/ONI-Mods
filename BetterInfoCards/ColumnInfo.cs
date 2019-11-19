using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class ColumnInfo
    {
        public float offsetX = 0f;
        public float offsetY = 0f;
        public List<InfoCard> infoCards = new List<InfoCard>();
        public float maxXInCol = 0f;
        public float YMin { get { return infoCards.Last().YMin; } }
    }
}
