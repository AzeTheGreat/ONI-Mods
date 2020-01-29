using AzeLib.Extensions;
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
                var titleTarget = AccessTools.Method(typeof(GameUtil), nameof(GameUtil.GetUnitFormattedName), new Type[] { typeof(GameObject), typeof(bool) });
                var germTarget = AccessTools.Method(typeof(string), nameof(string.Format), new Type[] { typeof(string), typeof(object), typeof(object) });
                var tempTarget = AccessTools.Method(typeof(GameUtil), nameof(GameUtil.GetFormattedTemperature));
                var statusTarget = AccessTools.Method(typeof(StatusItemGroup.Entry), nameof(StatusItemGroup.Entry.GetName));

                var targetGetCompPrimaryElement = AccessTools.Method(typeof(UnityEngine.Component), "GetComponent").MakeGenericMethod(typeof(PrimaryElement));
                var targetDrawText = AccessTools.Method(typeof(HoverTextDrawer), "DrawText", new Type[] { typeof(string), typeof(TextStyleSetting) });
                var targetEndShadowBar = AccessTools.Method(typeof(HoverTextDrawer), "EndShadowBar");
                var targetElement = AccessTools.Method(typeof(WorldInspector), nameof(WorldInspector.MassStringsReadOnly));

                LocalBuilder titleLocal = null;
                LocalBuilder germLocal = null;

                bool isFirst = true;
                bool afterTarget = false;

                foreach (CodeInstruction i in instructions)
                {
                    yield return i;

                    if (isFirst && i.opcode == OpCodes.Callvirt && i.operand == targetGetCompPrimaryElement)
                    {
                        isFirst = false;
                        afterTarget = true;

                        var lastLocalSelectable = instructions.FindPrior(i, x => x.IsLocalOfType(typeof(KSelectable))).operand;
                        yield return new CodeInstruction(OpCodes.Ldloc_S, lastLocalSelectable);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportSelectable)));
                    }

                    else if (afterTarget)
                    {
                        if (i.operand == titleTarget)
                            titleLocal = instructions.FindNext(i, x => x.opcode == OpCodes.Stloc_S).operand as LocalBuilder;

                        else if (i.operand == germTarget)
                            germLocal = instructions.FindNext(i, x => x.opcode == OpCodes.Stloc_S).operand as LocalBuilder;


                        else if (i.opcode == OpCodes.Callvirt && i.operand == targetDrawText)
                        {
                            var lastStringPush = instructions.FindPrior(i, x => DoesPushString(x));

                            // Title
                            if (lastStringPush.operand == titleLocal)
                            {
                                yield return new CodeInstruction(OpCodes.Ldstr, StatusDataManager.title);
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.Export)));
                            }

                            // Germs
                            else if (lastStringPush.operand == germLocal)
                            {
                                yield return new CodeInstruction(OpCodes.Ldstr, StatusDataManager.germs);
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.Export)));
                            }

                            // Status items
                            else if (lastStringPush.operand == statusTarget)
                            {
                                var lastLocalEntry = instructions.FindPrior(i, x => x.IsLocalOfType(typeof(StatusItemGroup.Entry))).operand;
                                yield return new CodeInstruction(OpCodes.Ldloc_S, lastLocalEntry);
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportStatus)));
                            }

                            // Temps
                            else if (lastStringPush.operand == tempTarget)
                            {
                                yield return new CodeInstruction(OpCodes.Ldstr, StatusDataManager.temp);
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.Export)));
                            }
                        }

                        else if (i.opcode == OpCodes.Call && i.operand == targetElement)
                        {
                            yield return new CodeInstruction(OpCodes.Ldarg_1);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.Helper)));
                        }
                    }
                }
            }

            private static bool DoesPushString(CodeInstruction i)
            {
                var push = i.opcode.StackBehaviourPush;
                var op = i.operand;
                if ((push == StackBehaviour.Varpush || 
                    push == StackBehaviour.Push1) 
                    &&
                    ((op as MethodInfo)?.ReturnType == typeof(string) || 
                    (op as LocalBuilder)?.LocalType == typeof(string)))
                    return true;
                return false;
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
            static MethodBase TargetMethod() => AccessTools.FirstInner(typeof(HoverTextDrawer), x => x.IsGenericType).MakeGenericType(typeof(MonoBehaviour)).GetMethod("Draw");

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
