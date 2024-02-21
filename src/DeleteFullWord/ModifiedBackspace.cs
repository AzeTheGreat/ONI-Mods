using AzeLib.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;

namespace DeleteFullWord;

[HarmonyPatch]
public class ModifiedBackspace
{
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(TMP_InputField), nameof(TMP_InputField.Backspace))]
    public static void CtrlBackspace(TMP_InputField instance, int charsToRemove)
    {
        // The Backspace method removes 1 char from the string.
        // This modifies it to remove the number of chars passed as a parameter.
        IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            // The text field manipulation occurs within 2 try blocks in the method.
            // Find codes that call String.Remove; this is the only unique target present in both blocks.
            var remove_method = AccessTools.Method(typeof(String), nameof(String.Remove), [typeof(int), typeof(int)]);
            var removeCalls = codes.Where(i => i.Calls(remove_method));

            // The default method always removes 1 char; to change this, this value needs to be modified in 3 places:
            // 1. The stringPositionInternal offset.
            // 2. The text.Remove count argument.
            // 3. The stringPositionInternal decrementer.
            // Not all Ldc_I4_1 opcodes can be replaced: some are used as bools in control flow.
            var codesToModify = removeCalls.SelectMany(c =>
            {
                var prev = c.FindPrior(codes, PushesOne);
                return new List<CodeInstruction>() {
                    prev,
                    prev.FindPrior(codes, PushesOne),
                    c.FindNext(codes, PushesOne) };

                bool PushesOne(CodeInstruction c) => c.OpCodeIs(OpCodes.Ldc_I4_1);
            });

            // Get the chars to remove from the passed method argument.
            return codes.Manipulator(
                i => codesToModify.Contains(i),
                i => i.opcode = OpCodes.Ldarg_1);
        }
        _ = Transpiler(null);
    }
}
