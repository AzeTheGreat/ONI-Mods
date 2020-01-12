using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterInfoCards
{
    public class CollectHoverInfo
    {
        public static CollectHoverInfo Instance { get; set; }

        private List<InfoCard> infoCards = new List<InfoCard>();
        private DisplayCards displayCardManager = new DisplayCards();

        private TextInfo intermediateTextInfo = null;
        private KSelectable intermediateSelectable = null;
        private List<TextInfo> intermediateStatuses = new List<TextInfo>();

        [HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
        private class GetSelectInfo_Patch
        {
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
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportSelectable)));
                    }

                    else if (afterTarget && i.opcode == OpCodes.Callvirt && i.operand == targetMethod2)
                    {
                        hitsAfter++;

                        // Title
                        if (hitsAfter == 1)
                        {
                            yield return new CodeInstruction(OpCodes.Ldstr, StatusDataManager.title);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.Export)));
                        }

                        // Germs
                        else if (hitsAfter == 2)
                        {
                            yield return new CodeInstruction(OpCodes.Ldstr, StatusDataManager.germs);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.Export)));
                        }

                        // Status items
                        else if (hitsAfter == 3)
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 84);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportStatus)));
                        }

                        // Status items 2
                        else if (hitsAfter == 4)
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 89);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportStatus)));
                        }

                        // Temps
                        else if (hitsAfter == 5)
                        {
                            yield return new CodeInstruction(OpCodes.Ldstr, StatusDataManager.temp);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.Export)));
                        }
                    }

                    else if (i.opcode == OpCodes.Call && i.operand == AccessTools.Method(typeof(WorldInspector), nameof(WorldInspector.MassStringsReadOnly)))
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_1);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.Helper)));
                    }

                    yield return i;
                }
            }

            private static void Helper(List<KSelectable> selectables) => Instance.intermediateSelectable = selectables.LastOrDefault();
            private static void ExportSelectable(KSelectable selectable) => Instance.intermediateSelectable = selectable;
            private static void Export(string name) => Instance.intermediateTextInfo = new TextInfo() { name = name, data = Instance.intermediateSelectable.gameObject };
            private static void ExportStatus(StatusItemGroup.Entry entry) => Instance.intermediateTextInfo = new TextInfo() { name = entry.item.Name, data = entry.data };
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new Type[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
        private class TrackTexts_Patch
        {
            static void Postfix()
            {
                Instance.intermediateStatuses.Add(Instance.intermediateTextInfo);
                Instance.intermediateTextInfo = null;
            }
        }

        [HarmonyPatch]
        private class GetWidget_Patch
        {
            // HoverTextDrawer.Pool.Draw
            static MethodBase TargetMethod() => AccessTools.FirstInner(typeof(HoverTextDrawer), x => x.IsGenericType).MakeGenericType(typeof(object)).GetMethod("Draw");

            static void Postfix(Entry __result, GameObject ___prefab)
            {
                InfoCard card = Instance.infoCards.Last();
                card.AddWidget(__result, ___prefab);
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.BeginShadowBar))]
        private class BeginShadowBar_Patch
        {
            static void Postfix()
            {
                Instance.infoCards.Add(new InfoCard());
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        private class EndShadowBar_Patch
        {
            static void Postfix()
            {
                InfoCard card = Instance.infoCards.Last();
                card.AddData(new List<TextInfo>(Instance.intermediateStatuses), Instance.intermediateSelectable);
                Instance.intermediateSelectable = null;
                Instance.intermediateStatuses.Clear();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
        private class EditWidgets_Patch
        {
            static void Postfix()
            {
                var displayCards = Instance.displayCardManager.UpdateData(Instance.infoCards);
                ModifyHits.Instance.Update(displayCards);
                displayCards.ForEach(x => x.Rename());

                if (displayCards.Count == 0)
                    return;
                var gridInfo = new GridInfo(displayCards, Instance.infoCards[0].YMax);
                gridInfo.MoveAndResizeInfoCards();

                Instance.infoCards.Clear();
            }
        }
    }
}
