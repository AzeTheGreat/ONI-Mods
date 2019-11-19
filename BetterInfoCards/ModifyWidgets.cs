using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    public class ModifyWidgets
    {
        public static ModifyWidgets Instance { get; set; }

        private CachedWidgets cachedWidgets = new CachedWidgets();

        [HarmonyPatch]
        private class GetWidgets_Patch
        {
            private static int callNumber = 0;

            public static void Initialize()
            {
                callNumber = 0;
            }

            static MethodBase TargetMethod()
            {
                return AccessTools.FirstInner(typeof(HoverTextDrawer), x => x.IsGenericType).MakeGenericType(typeof(object)).GetMethod("EndDrawing");
            }

            static void Postfix(ref List<Entry> ___entries, int ___drawnWidgets)
            {
                Instance.cachedWidgets.UpdateCache(___entries, (WidgetsBase.EntryType)callNumber, ___drawnWidgets);

                callNumber++;
                if (callNumber > 3)
                    callNumber = 0;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
        private class EditWidgets_Patch
        {
            static void Postfix()
            {
                WidgetModifier.Instance.ModifyWidgets();
            }
        }
    }

    public class CachedWidgets : WidgetsBase
    {
        private DrawnWidgets drawnWidgets = new DrawnWidgets();

        public void UpdateCache(List<Entry> entries, EntryType type, int numWidgetsDrawn)
        {
            DrawnWidgets.Instance = drawnWidgets;

            List<Entry> cachedEntries = GetEntries(type);

            if (entries.Count > cachedEntries.Count)
            {
                // Why the hell can't I convert after getting the range!?  BS
                cachedEntries.AddRange(entries.ConvertAll(new Converter<Entry, Entry>(ConvertEntryToEntry)).GetRange(cachedEntries.Count, entries.Count - cachedEntries.Count));
            }

            drawnWidgets.Update(cachedEntries, type, numWidgetsDrawn);
        }

        private Entry ConvertEntryToEntry(Entry source)
        {
            return source;
        }
    }

    public struct Entry
    {
        public MonoBehaviour widget;
        public RectTransform rect;
    }
}
