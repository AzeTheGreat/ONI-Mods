using AzeLib.Extensions;
using HarmonyLib;
using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace ClarifiedMaxDecor
{
    [HarmonyPatch(typeof(SelectToolHoverTextCard), nameof(SelectToolHoverTextCard.UpdateHoverElements))]
    class RemoveCapFromCard
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var targetMethod = AccessTools.Method(typeof(GameUtil), nameof(GameUtil.GetFormattedDecor));
            var codes = instructions.ToList();

            var targetIndex = codes.FindLastIndex(codes.FindIndex(i => i.OperandIs(targetMethod)), i => i.OpCodeIs(OpCodes.Ldc_I4_1));
            codes[targetIndex] = new CodeInstruction(OpCodes.Ldc_I4_0);

            return codes.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(DecorDisplayer), nameof(DecorDisplayer.GetTooltip))]
    class AddCapToVitalsTooltip
    {
        static void Postfix(ref string __result, AmountInstance instance)
        {
            if(instance.gameObject.GetSMI<DecorMonitor.Instance>() is DecorMonitor.Instance smi)
            {
                var format = smi.GetYesterdaysAverageDecor() > DecorMonitor.MAXIMUM_DECOR_VALUE ? UI.OVERLAYS.DECOR.MAXIMUM_DECOR : UI.OVERLAYS.DECOR.VALUE;
                __result = string.Format(STRINGS.DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.DESC_FORMAT, __result, string.Format(format, "", DecorMonitor.MAXIMUM_DECOR_VALUE));
            }
        }
    }
}
