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
                Instance.cachedWidgets.UpdateCache(___entries, (CachedWidgets.EntryType)callNumber, ___drawnWidgets);

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

    public class CachedWidgets
    {
        public enum EntryType
        {
            shadowBar,
            iconWidget,
            textWidget,
            selectBorder
        }

        private DrawnWidgets drawnWidgets = new DrawnWidgets();

        private List<Entry> shadowBars = new List<Entry>();
        private List<Entry> iconWidgets = new List<Entry>();
        private List<Entry> textWidgets = new List<Entry>();
        private List<Entry> selectBorders = new List<Entry>();

        public void UpdateCache(List<Entry> entries, EntryType type, int numWidgetsDrawn)
        {
            DrawnWidgets.Instance = drawnWidgets;
            List<Entry> cachedEntries;

            switch (type)
            {
                case EntryType.shadowBar:
                    cachedEntries = shadowBars;
                    break;
                case EntryType.iconWidget:
                    cachedEntries = iconWidgets;
                    break;
                case EntryType.textWidget:
                    cachedEntries = textWidgets;
                    break;
                case EntryType.selectBorder:
                    cachedEntries = selectBorders;
                    break;
                default:
                    throw new Exception("Invalid EntryType");
            }

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
