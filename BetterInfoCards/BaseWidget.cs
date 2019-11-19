using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterInfoCards
{
    public abstract class WidgetsBase
    {
        public enum EntryType
        {
            shadowBar,
            iconWidget,
            textWidget,
            selectBorder
        }

        // TODO: Make protected
        public List<Entry> shadowBars = new List<Entry>();
        public List<Entry> iconWidgets = new List<Entry>();
        public List<Entry> textWidgets = new List<Entry>();
        public List<Entry> selectBorders = new List<Entry>();

        protected List<Entry> GetEntries(EntryType type)
        {
            switch (type)
            {
                case EntryType.shadowBar:
                    return shadowBars;
                case EntryType.iconWidget:
                    return iconWidgets;
                case EntryType.textWidget:
                    return textWidgets;
                case EntryType.selectBorder:
                    return selectBorders;
                default:
                    throw new Exception("Invalid EntryType");
            }
        }
    }
}
