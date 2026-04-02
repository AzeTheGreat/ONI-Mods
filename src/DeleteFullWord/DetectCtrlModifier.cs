using AzeLib.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;

namespace DeleteFullWord;

[HarmonyPatch(typeof(TMP_InputField), nameof(TMP_InputField.KeyPressed))]
public class DetectCtrlModifier
{
    private static readonly char[] WordSeparators = [' ', '.', ',', '\t', '\r', '\n'];
    private static readonly Func<TMP_InputField, int> FindPreviousWordBegin;

    static DetectCtrlModifier()
    {
        // TMP renamed this private helper from FindtPrevWordBegin to FindPrevWordBegin in newer builds.
        var method =
            AccessTools.Method(typeof(TMP_InputField), "FindPrevWordBegin") ??
            AccessTools.Method(typeof(TMP_InputField), "FindtPrevWordBegin");

        if (method != null)
        {
            FindPreviousWordBegin = AccessTools.MethodDelegate<Func<TMP_InputField, int>>(method, null, false, Type.EmptyTypes);
            return;
        }

        // Match the legacy helper behavior if neither private method name is available, rather than crashing.
        FindPreviousWordBegin = instance =>
        {
            var text = instance.text ?? string.Empty;
            var searchPosition = Math.Min(instance.stringSelectPositionInternal, text.Length);
            if (searchPosition - 2 < 0)
                return 0;

            var separatorIndex = text.LastIndexOfAny(WordSeparators, searchPosition - 2);
            return separatorIndex == -1 ? 0 : separatorIndex + 1;
        };
    }

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
            var charsToRemove = isShiftPressed ? instance.stringPositionInternal : instance.stringPositionInternal - FindPreviousWordBegin(instance);

            if (charsToRemove > 0)
                ModifiedBackspace.MultiBackspace(instance, charsToRemove);
        }
        else
            instance.Backspace();
    }
}
