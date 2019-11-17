using System.Collections.Generic;

namespace BetterInfoCards
{
    public class DrawnWidgets
    {
        public static DrawnWidgets Instance { get; set; }

        public List<Entry> shadowBars = new List<Entry>();
        public List<Entry> iconWidgets = new List<Entry>();
        public List<Entry> textWidgets = new List<Entry>();
        public List<Entry> selectBorders = new List<Entry>();
    }
}
