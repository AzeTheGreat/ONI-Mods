using AzeLib.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterInfoCards
{
    public class ExportSelectToolData
    {
        private static KSelectable curSelectable;
        private static (string id, object data) curTextInfo = (string.Empty, null);

        public static KSelectable ConsumeSelectable()
        {
            var sel = curSelectable;
            curSelectable = null;
            return sel;
        }

        public static (string id, object data) ConsumeTextInfo()
        {
            var ti = curTextInfo;
            curTextInfo = (string.Empty, null);
            return ti;
        }

        public class GetSelectInfo_Patch
        {
            public static IEnumerable<CodeInstruction> ChildTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var titleTarget = AccessTools.Method(typeof(GameUtil), nameof(GameUtil.GetUnitFormattedName), new Type[] { typeof(GameObject), typeof(bool) });
                var germTarget = AccessTools.Method(typeof(string), nameof(string.Format), new Type[] { typeof(string), typeof(object), typeof(object) });
                var tempTarget = AccessTools.Method(typeof(GameUtil), nameof(GameUtil.GetFormattedTemperature));
                var statusTarget = AccessTools.Method(typeof(StatusItemGroup.Entry), nameof(StatusItemGroup.Entry.GetName));

                var targetGetCompPrimaryElement = AccessTools.Method(typeof(Component), "GetComponent").MakeGenericMethod(typeof(PrimaryElement));
                var targetDrawText = AccessTools.Method(typeof(HoverTextDrawer), "DrawText", new Type[] { typeof(string), typeof(TextStyleSetting) });
                var targetEndShadowBar = AccessTools.Method(typeof(HoverTextDrawer), "EndShadowBar");
                var targetElement = AccessTools.Method("HoverTextHelper:MassStringsReadOnly") ??
                    AccessTools.Method("WorldInspector:MassStringsReadOnly");   // Fallback for vanilla assembly

                LocalBuilder titleLocal = null;
                LocalBuilder germLocal = null;

                bool isFirst = true;
                bool afterTarget = false;

                foreach (CodeInstruction i in instructions)
                {
                    if (isFirst && i.Is(OpCodes.Callvirt, targetGetCompPrimaryElement))
                    {
                        isFirst = false;
                        afterTarget = true;

                        var lastLocalSelectable = instructions.FindPrior(i, x => x.IsLocalOfType(typeof(KSelectable))).operand;
                        yield return new CodeInstruction(OpCodes.Ldloc_S, lastLocalSelectable);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportSelectable)));
                    }

                    else if (afterTarget)
                    {
                        if (i.OperandIs(titleTarget))
                            titleLocal = instructions.FindNext(i, x => x.OpCodeIs(OpCodes.Stloc_S)).operand as LocalBuilder;

                        else if (i.OperandIs(germTarget))
                            germLocal = instructions.FindNext(i, x => x.OpCodeIs(OpCodes.Stloc_S)).operand as LocalBuilder;

                        else if (i.Is(OpCodes.Callvirt, targetDrawText))
                        {
                            var lastStringPush = instructions.FindPrior(i, x => DoesPushString(x));

                            // Title
                            if (lastStringPush.OperandIs(titleLocal))
                            {
                                yield return new CodeInstruction(OpCodes.Ldstr, ConverterManager.title);
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportGO)));
                            }

                            // Germs
                            else if (lastStringPush.OperandIs(germLocal))
                            {
                                yield return new CodeInstruction(OpCodes.Ldstr, ConverterManager.germs);
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportGO)));
                            }

                            // Status items
                            else if (lastStringPush.OperandIs(statusTarget))
                            {
                                var lastLocalEntry = instructions.FindPrior(i, x => x.IsLocalOfType(typeof(StatusItemGroup.Entry))).operand;
                                yield return new CodeInstruction(OpCodes.Ldloc_S, lastLocalEntry);
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportStatus)));
                            }

                            // Temps
                            else if (lastStringPush.OperandIs(tempTarget))
                            {
                                yield return new CodeInstruction(OpCodes.Ldstr, ConverterManager.temp);
                                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportGO)));
                            }
                        }

                        else if (i.Is(OpCodes.Call, targetElement))
                        {
                            yield return new CodeInstruction(OpCodes.Ldarg_1);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetSelectInfo_Patch), nameof(GetSelectInfo_Patch.ExportSelectableFromList)));
                        }
                    }

                    yield return i;
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

            private static void ExportSelectableFromList(List<KSelectable> selectables) => ExportSelectable(selectables.LastOrDefault());
            private static void ExportSelectable(KSelectable selectable) => curSelectable = selectable;
            private static void Export(string name, object data) => curTextInfo = (name, data);
            private static void ExportGO(string name)
            {
                if (curSelectable == null)
                {
                    Debug.LogWarning($"[BetterInfoCards] Skipping export for '{name}' because there is no active selectable.");
                    return;
                }

                Export(name, curSelectable.gameObject);
            }
            private static void ExportStatus(StatusItemGroup.Entry entry) => Export(entry.item.Id, entry.data);
        }
    }
}
