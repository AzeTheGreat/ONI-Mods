using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    public class ModifyWidgets
    {
        public static ModifyWidgets Instance { get; set; }

        private CachedWidgets cachedWidgets = new CachedWidgets();
        private int callNumber = 0;

        [HarmonyPatch]
        private class GetWidgets_Patch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.FirstInner(typeof(HoverTextDrawer), x => x.IsGenericType).MakeGenericType(typeof(object)).GetMethod("EndDrawing");
            }

            static void Postfix(List<Entry> ___entries, int ___drawnWidgets)
            {
                ref int callNumber = ref Instance.callNumber;
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
                Instance.cachedWidgets.Update();
            }
        }
    }

    public struct Entry
    {
        public MonoBehaviour widget;
        public RectTransform rect;
    }
}
