using AzeLib.Extensions;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace BetterInfoCards.Tweaks
{
    [HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
    class PreventDuplicateTempText
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var targetMethod = AccessTools.Method(typeof(UnityEngine.Component), "GetComponent").MakeGenericMethod(typeof(Constructable));

            var ins = instructions.ToList();

            int targetI = ins.FindIndex(x => x.OperandIs(targetMethod)) + 2;
            ins.InsertRange(targetI, insert());

            IEnumerable<CodeInstruction> insert()
            {
                var lastLocalSelectable = instructions.FindPrior(ins[targetI], x => x.IsLocalOfType(typeof(KSelectable))).operand;
                yield return new CodeInstruction(OpCodes.Ldloc_S, lastLocalSelectable);
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PreventDuplicateTempText), nameof(PreventDuplicateTempText.PreventDuplcate)));
            }

            return ins;
        }

        private static bool PreventDuplcate(bool isConstructable, KSelectable selectable) => isConstructable || selectable.GetComponent<ElementChunk>();
    }
}
