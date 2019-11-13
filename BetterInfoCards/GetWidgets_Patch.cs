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

        static void Postfix(ref List<Entry> ___entries, int ___drawnWidgets, object __instance)
        {
            var drawnEntries = new List<Entry>();
            for (int i = 0; i < ___drawnWidgets; i++)
            {
                drawnEntries.Add(___entries[i]);
            }

            switch (callNumber)
            {
                case 0:
                    DrawnWidgets.shadowBars = drawnEntries;
                    callNumber++;
                    break;
                case 1:
                    DrawnWidgets.iconWidgets = drawnEntries;
                    callNumber++;
                    break;
                case 2:
                    DrawnWidgets.textWidgets = drawnEntries;
                    callNumber++;
                    break;
                case 3:
                    DrawnWidgets.selectBorders = drawnEntries;
                    callNumber = 0;
                    break;
                default:
                    throw new Exception("GetWidgets is out of sync with the game.");
            }
        }
    }

    public struct Entry
    {
        public MonoBehaviour widget;
        public RectTransform rect;
    }

    public static class DrawnWidgets
    {
        public static List<Entry> shadowBars;
        public static List<Entry> iconWidgets;
        public static List<Entry> textWidgets;
        public static List<Entry> selectBorders;
    }
}
