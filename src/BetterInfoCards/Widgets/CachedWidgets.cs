using System;
using System.Collections.Generic;

namespace BetterInfoCards
{
    public class CachedWidgets : WidgetsBase
    {
        private DrawnWidgets drawnWidgets = new DrawnWidgets();

        public void UpdateCache(List<Entry> entries, EntryType type, int numWidgetsDrawn)
        {
            List<Entry> cachedEntries = GetEntries(type);

            if (entries.Count > cachedEntries.Count)
            {
                // Why the hell can't I convert AFTER getting the range!?  BS
                cachedEntries.AddRange(entries.ConvertAll(new Converter<Entry, Entry>(ConvertEntryToEntry)).GetRange(cachedEntries.Count, entries.Count - cachedEntries.Count));
            }

            drawnWidgets.UpdateCache(cachedEntries, type, numWidgetsDrawn);
        }

        public void Update()
        {
            drawnWidgets.Update();
        }

        private Entry ConvertEntryToEntry(Entry source)
        {
            return source;
        }
    }
}
