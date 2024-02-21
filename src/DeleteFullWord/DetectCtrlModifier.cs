using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;

namespace DeleteFullWord;

[HarmonyPatch(typeof(TMP_InputField), nameof(TMP_InputField.KeyPressed))]
public class DetectCtrlModifier
{
    // Wrap the Backspace call so that Ctrl can be checked for and handled.
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
    {
        return codes.Manipulator(
            i => i.Calls(AccessTools.Method(typeof(TMP_InputField), nameof(TMP_InputField.Backspace))),
            i => [
                new CodeInstruction(OpCodes.Ldarg_1),
                CodeInstruction.Call(typeof(DetectCtrlModifier), nameof(Wrapper))
            ]);
    }

    private static void Wrapper(TMP_InputField instance, Event evt)
    {
        var isControlPressed = (evt.modifiers & EventModifiers.Control) > EventModifiers.None;
        if (isControlPressed)
        {
            var isShiftPressed = (evt.modifiers & EventModifiers.Shift) > EventModifiers.None;
            var charsToRemove = isShiftPressed ? instance.stringPositionInternal : instance.stringPositionInternal - instance.FindtPrevWordBegin();

            if (charsToRemove > 0)
                ModifiedBackspace.MultiBackspace(instance, charsToRemove);
        }
        else
            instance.Backspace();
    }
}
