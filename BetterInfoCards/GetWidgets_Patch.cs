using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    [HarmonyPatch]
    public class GetWidgets_Patch
    {
        private static int callNumber = 0;

        static MethodBase TargetMethod()
        {
            return AccessTools.FirstInner(typeof(HoverTextDrawer), x => x.IsGenericType).MakeGenericType(typeof(object)).GetMethod("EndDrawing");
        }

        private static Entry ConvertEntryToEntry(Entry source)
        {
            return source;
        }

        static void Postfix(ref List<Entry> ___entries, int ___drawnWidgets, object __instance)
        {
            List<Entry> cachedEntries;
            List<Entry> drawnEntries;

            switch (callNumber)
            {
                case 0:
                    cachedEntries = CachedWidgets.shadowBars;
                    drawnEntries = DrawnWidgets.shadowBars;
                    callNumber++;
                    break;
                case 1:
                    cachedEntries = CachedWidgets.iconWidgets;
                    drawnEntries = DrawnWidgets.iconWidgets;
                    callNumber++;
                    break;
                case 2:
                    cachedEntries = CachedWidgets.textWidgets;
                    drawnEntries = DrawnWidgets.textWidgets;
                    callNumber++;
                    break;
                case 3:
                    cachedEntries = CachedWidgets.selectBorders;
                    drawnEntries = DrawnWidgets.selectBorders;
                    callNumber = 0;
                    break;
                default:
                    throw new Exception("GetWidgets is out of sync with the game.");
            }

            if (___entries.Count > cachedEntries.Count)
            {
                // Why the hell can't I convert after getting the range!?  BS
                cachedEntries.AddRange(___entries.ConvertAll(new Converter<Entry, Entry>(ConvertEntryToEntry)).GetRange(cachedEntries.Count, ___entries.Count - cachedEntries.Count));
            }

            if (___drawnWidgets > drawnEntries.Count)
                drawnEntries.AddRange(cachedEntries.GetRange(drawnEntries.Count, ___drawnWidgets - drawnEntries.Count));
            if(___drawnWidgets < drawnEntries.Count)
                drawnEntries.RemoveRange(___drawnWidgets, drawnEntries.Count - ___drawnWidgets);
        }

        private static class CachedWidgets
        {
            public static List<Entry> shadowBars = new List<Entry>();
            public static List<Entry> iconWidgets = new List<Entry>();
            public static List<Entry> textWidgets = new List<Entry>();
            public static List<Entry> selectBorders = new List<Entry>();
        }
    }

    public struct Entry
    {
        public MonoBehaviour widget;
        public RectTransform rect;
    }

    public static class DrawnWidgets
    {
        public static List<Entry> shadowBars = new List<Entry>();
        public static List<Entry> iconWidgets = new List<Entry>();
        public static List<Entry> textWidgets = new List<Entry>();
        public static List<Entry> selectBorders = new List<Entry>();
    }
}
