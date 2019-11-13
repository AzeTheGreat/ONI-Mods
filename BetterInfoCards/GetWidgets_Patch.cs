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
        public static int callNumber = 0;

        public static List<Entry> shadowBars;
        public static List<Entry> iconWidgets;
        public static List<Entry> textWidgets;
        public static List<Entry> selectBorders;

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
                    shadowBars = drawnEntries;
                    break;
                case 1:
                    iconWidgets = drawnEntries;
                    break;
                case 2:
                    textWidgets = drawnEntries;
                    break;
                case 3:
                    selectBorders = drawnEntries;
                    break;
                default:
                    throw new Exception("GetWidgets is out of sync with the game.");
            }

            callNumber++;
        }
    }

    public struct Entry
    {
        public MonoBehaviour widget;
        public RectTransform rect;
    }
}
