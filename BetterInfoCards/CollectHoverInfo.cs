using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BetterInfoCards
{
    class CollectHoverInfo
    {
        public static CollectHoverInfo Instance { get; set; }

        private int infoCard;
        public List<int> gridPositions = new List<int>();
        public List<List<StatusItemGroup.Entry>> activeStatuses = new List<List<StatusItemGroup.Entry>>();

        private List<StatusItemGroup.Entry> intermediateList = new List<StatusItemGroup.Entry>();

        [HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
        private class Test
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                MethodInfo targetMethod = AccessTools.Method(typeof(UnityEngine.Component), "GetComponent").MakeGenericMethod(typeof(PrimaryElement));
                MethodInfo targetMethod2 = AccessTools.Method(typeof(HoverTextDrawer), "DrawText", new Type[] { typeof(string), typeof(TextStyleSetting) });
                Debug.Log(targetMethod2);
                MethodInfo targetMethod3 = AccessTools.Method(typeof(HoverTextDrawer), "EndShadowBar");
                Debug.Log(targetMethod3);

                bool isFirst = true;
                bool afterTarget = false;
                int hitsAfter = 0;
                foreach (CodeInstruction i in instructions)
                {
                    // For every selectable shown in current hover
                    if (isFirst && i.opcode == OpCodes.Callvirt && i.operand == targetMethod)
                    {
                        isFirst = false;
                        afterTarget = true;

                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportInitial"));
                    }

                    else if (afterTarget && i.opcode == OpCodes.Callvirt && i.operand == targetMethod2)
                    {
                        hitsAfter++;
                        Debug.Log("hits: " + hitsAfter);

                        // Title
                        if (hitsAfter == 1)
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportBlank"));

                        // Germs
                        else if (hitsAfter == 2)
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportBlank"));

                        // Status items
                        else if (hitsAfter == 3)
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 84);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportStatus"));
                        }

                        else if (hitsAfter == 4)
                        {
                            yield return new CodeInstruction(OpCodes.Ldloc_S, 89);
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportStatus"));
                        }

                        // Temps
                        else if (hitsAfter == 5)
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportBlank"));
                    }

                    else if(afterTarget && i.opcode == OpCodes.Callvirt && i.operand == targetMethod3)
                    {
                        afterTarget = false;

                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportFinal"));
                    }

                    yield return i;
                }
            }

            private static void ExportInitial(int gridPos)
            {
                Instance.intermediateList.Clear();
                Instance.gridPositions.SetOrAdd(Instance.infoCard, gridPos);
            }

            private static void ExportBlank() => Instance.intermediateList.Add(new StatusItemGroup.Entry());

            private static void ExportStatus(StatusItemGroup.Entry entry) => Instance.intermediateList.Add(entry);

            private static void ExportFinal() => Instance.activeStatuses.SetOrAdd(Instance.infoCard, new List<StatusItemGroup.Entry>(Instance.intermediateList));

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
}
