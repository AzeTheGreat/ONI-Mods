﻿using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BetterDeselect.Deselect
{
    [HarmonyPatch(typeof(PlanScreen), "OnActiveToolChanged")]
    public class DeselectBuildMenu_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Options.ReadSettings();

            MethodInfo targetMethodInfo = AccessTools.Method(typeof(PlanScreen), "CloseCategoryPanel");

            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Call && i.operand == targetMethodInfo && Options.options.ImplementBuildMenu)
                {
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Nop);
                }
                else
                    yield return i;
            }
        }
    }
}