using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterInfoCards
{
    class CollectHoverInfo
    {
        public static CollectHoverInfo Instance { get; set; }

        //private int infoCard;
        public List<KSelectable> selectables = new List<KSelectable>();
        public List<List<TextInfo>> activeStatuses = new List<List<TextInfo>>();

        private TextInfo intermediateTextInfo = null;
        private KSelectable intermediateSelectable = null;
        private List<TextInfo> intermediateStatuses = new List<TextInfo>();

        [HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
        private class Patch
        {
            private static void ExportSelectable(KSelectable selectable) => Instance.intermediateSelectable = selectable;
            private static void ExportTitle(GameObject go) => Instance.intermediateTextInfo = new TextInfo() { name = StatusDataManager.title, data = go };
            private static void ExportGerms(GameObject go) => Instance.intermediateTextInfo = new TextInfo() { name = StatusDataManager.germs, data = go };
            private static void ExportStatus(StatusItemGroup.Entry entry) => Instance.intermediateTextInfo = new TextInfo() { name = entry.item.Name, data = entry.data };
            private static void ExportTemp(GameObject go) => Instance.intermediateTextInfo = new TextInfo() { name = StatusDataManager.temp, data = go };

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                MethodInfo targetMethod = AccessTools.Method(typeof(UnityEngine.Component), "GetComponent").MakeGenericMethod(typeof(PrimaryElement));
                MethodInfo targetMethod2 = AccessTools.Method(typeof(HoverTextDrawer), "DrawText", new Type[] { typeof(string), typeof(TextStyleSetting) });
                MethodInfo targetMethod3 = AccessTools.Method(typeof(HoverTextDrawer), "EndShadowBar");

                bool isFirst = true;
                bool afterTarget = false;
                int hitsAfter = 0;
                foreach (CodeInstruction i in instructions)
                {
                    // For each selectable shown in current hover
                    if (isFirst && i.opcode == OpCodes.Callvirt && i.operand == targetMethod)
                    {
                        isFirst = false;
                        afterTarget = true;

                        yield return new CodeInstruction(OpCodes.Ldloc_S, 72);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch), nameof(Patch.ExportSelectable)));
                    }

                    else if (afterTarget && i.opcode == OpCodes.Callvirt && i.operand == targetMethod2)
                    {
                        hitsAfter++;
                        
                        // Title
                        if (hitsAfter == 1)
                            foreach (var ci in GetCIsToExport(nameof(Patch.ExportTitle))) { yield return ci; }
                           
                        // Germs
                        else if (hitsAfter == 2)
                            foreach (var ci in GetCIsToExport(nameof(Patch.ExportGerms))) { yield return ci; }

                        // Status items
                        else if (hitsAfter == 3)
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 84);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch), nameof(Patch.ExportStatus)));
                        }

                        // Status items 2
                        else if (hitsAfter == 4)
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 89);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch), nameof(Patch.ExportStatus)));
                        }

                        // Temps
                        else if (hitsAfter == 5)
                            foreach (var ci in GetCIsToExport(nameof(Patch.ExportTemp))) { yield return ci; }
                    }

                    else if (i.opcode == OpCodes.Call && i.operand == AccessTools.Method(typeof(WorldInspector), nameof(WorldInspector.MassStringsReadOnly)))
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_1);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch), nameof(Patch.Helper)));
                    }

                    yield return i;
                }
            }

            private static void Helper(List<KSelectable> selectables)
            {
                Instance.intermediateSelectable = selectables.LastOrDefault();
            }

            private static IEnumerable<CodeInstruction> GetCIsToExport(string method)
            {
                yield return new CodeInstruction(OpCodes.Ldloc_S, 72);
                yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(UnityEngine.Component), "get_gameObject"));
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch), method));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginDrawing))]
        private class HoverDrawerStart_Patch
        {
            static void Postfix()
            {
                Instance.selectables.Clear();
                Instance.activeStatuses.Clear();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginShadowBar))]
        private class BeginShadowBar_Patch
        {
            static void Postfix()
            {
                Instance.intermediateStatuses.Clear();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        private class EndShadowBar_Patch
        {
            // Occurs after each shadow bar
            // Essentially after each item
            static void Postfix()
            {
                Instance.selectables.Add(Instance.intermediateSelectable);
                Instance.intermediateSelectable = null;
                Instance.activeStatuses.Add(new List<TextInfo>(Instance.intermediateStatuses));
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new Type[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
        private class TrackTexts_Patch
        {
            // Occurs after each text is drawn
            static void Postfix()
            {
                Instance.intermediateStatuses.Add(Instance.intermediateTextInfo);
                Instance.intermediateTextInfo = null;
            }
        }
    }
}
