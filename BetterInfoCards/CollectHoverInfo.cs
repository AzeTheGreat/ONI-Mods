using Harmony;
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
        public List<KSelectable> kSelectables = new List<KSelectable>();

        [HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
        private class Test
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                MethodInfo targetMethod = AccessTools.Method(typeof(UnityEngine.Component), "GetComponent").MakeGenericMethod(typeof(PrimaryElement));

                bool isFirst = true;
                foreach (CodeInstruction i in instructions)
                {
                    if(isFirst && i.opcode == OpCodes.Callvirt && i.operand == targetMethod)
                    {
                        isFirst = false;

                        yield return new CodeInstruction(OpCodes.Ldloc_0);

                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(SelectToolHoverTextCard), "overlayValidHoverObjects"));
                        yield return new CodeInstruction(OpCodes.Ldloc_S, 71);
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<KSelectable>), "get_Item"));

                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Test), "ExportData"));
                    }
                    yield return i;
                }
            }

            private static void ExportData(int gridPos, KSelectable kSelectable)
            {
                Instance.gridPositions.SetOrAdd(Instance.infoCard, gridPos);
                Instance.kSelectables.SetOrAdd(Instance.infoCard, kSelectable);
            }

            static void Prefix()
            {
                Instance.infoCard = 0;
                Instance.gridPositions.Clear();
                Instance.kSelectables.Clear();
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndShadowBar))]
        private class TrackInfoCards_Patch
        {
            static void Postfix()
            {
                Instance.infoCard++;
                Instance.gridPositions.ExpandToIndex(Instance.infoCard);
                Instance.kSelectables.ExpandToIndex(Instance.infoCard);
            }
        }
    }
}
