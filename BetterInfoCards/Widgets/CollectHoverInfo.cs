using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BetterInfoCards
{
    class CollectHoverInfo
    {
        public static CollectHoverInfo Instance { get; set; }

        private int infoCard;
        public List<int> gridPositions = new List<int>();
        public List<List<TextInfo>> activeStatuses = new List<List<TextInfo>>();

        private List<TextInfo> intermediateList = new List<TextInfo>();

        [HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
        private class Test
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

                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportInitial"));
                        yield return i;
                    }

                    else if (afterTarget && i.opcode == OpCodes.Callvirt && i.operand == targetMethod2)
                    {
                        hitsAfter++;

                        // Title
                        if (hitsAfter == 1)
                        {
                            yield return i;
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 72);
                            yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(UnityEngine.Component), "get_gameObject"));
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportTitle"));
                        }
                            
                        // Germs
                        else if (hitsAfter == 2)
                        {
                            yield return i;
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 72);
                            yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(UnityEngine.Component), "get_gameObject"));
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportGerms"));
                            
                        }

                        // Status items
                        else if (hitsAfter == 3)
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 84);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportStatus"));
                            yield return i;
                        }

                        // Status items 2
                        else if (hitsAfter == 4)
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 89);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportStatus"));
                            yield return i;
                        }

                        // Temps
                        else if (hitsAfter == 5)
                        {
                            yield return i;
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 72);
                            yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(UnityEngine.Component), "get_gameObject"));
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportTemp"));
                        }     
                    }

                    // After each selectable
                    else if(afterTarget && i.opcode == OpCodes.Callvirt && i.operand == targetMethod3)
                    {
                        afterTarget = false;

                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportFinal"));
                        yield return i;
                    }

                    else
                    {
                        yield return i;
                    }
                }
            }

            private static void ExportInitial(int gridPos)
            {
                Instance.intermediateList.Clear();
                Instance.gridPositions.SetOrAdd(Instance.infoCard, gridPos);
            }

            private static void ExportTitle(GameObject go) => Instance.intermediateList.Add(new TextInfo() { name = StatusDataManager.title, data = go });

            private static void ExportGerms(GameObject go) => Instance.intermediateList.Add(new TextInfo() { name = StatusDataManager.germs, data = go });

            private static void ExportBlank() => Instance.intermediateList.Add(new TextInfo());

            private static void ExportStatus(StatusItemGroup.Entry entry) => Instance.intermediateList.Add(new TextInfo() { name = entry.item.Name, data = entry.data });

            private static void ExportTemp(GameObject go) => Instance.intermediateList.Add(new TextInfo() { name = StatusDataManager.temp, data = go });

            // Is the new list necessary?
            private static void ExportFinal() => Instance.activeStatuses.SetOrAdd(Instance.infoCard, new List<TextInfo>(Instance.intermediateList));

            static void Prefix()
            {
                Instance.infoCard = 0;
                Instance.gridPositions.Clear();
                Instance.activeStatuses.Clear();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        private class TrackInfoCards_Patch
        {
            static void Postfix()
            {
                Instance.infoCard++;
                Instance.gridPositions.ExpandToIndex(Instance.infoCard);
                Instance.activeStatuses.ExpandToIndex(Instance.infoCard);
            }
        }
    }

    public class TextInfo
    {
        public string name = string.Empty;
        public object data = null;
    }
}
