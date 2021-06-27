using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BetterLogicOverlay
{
    [HarmonyPatch(typeof(OverlayModes.Logic), nameof(OverlayModes.Logic.UpdateUI))]
    class GateOutputColor_Patch
    {
        static bool Prepare() => Options.Opts.FixWireOverwrite;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.Manipulator(
                AccessTools.Method(typeof(LogicCircuitNetwork), nameof(LogicCircuitNetwork.IsBitActive), new[] { typeof(int) }),
                ReplaceWithWrapper);

            IEnumerable<CodeInstruction> ReplaceWithWrapper(CodeInstruction i)
            {
                // Locate the uiInfo local var
                var uiInfoLocal = instructions.FindPrior(
                    instructions.FindPrior(i, x => x.OperandIs(AccessTools.Field(typeof(OverlayModes.Logic.UIInfo), "bitDepth"))),
                    x => x.OpCodeIs(OpCodes.Ldloc_S)).operand;

                // Load cell parameter.
                yield return new CodeInstruction(OpCodes.Ldloc_S, uiInfoLocal);
                yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(OverlayModes.Logic.UIInfo), "cell"));
                
                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GateOutputColor_Patch), nameof(GateOutputColor_Patch.IsBitActiveWrapper)));
            }
        }

        static bool IsBitActiveWrapper(LogicCircuitNetwork networkForCell, int bit, int cell) => networkForCell.IsBitActive(bit) && IsLogicCellActive(cell, networkForCell);

        static bool IsLogicCellActive(int cell, LogicCircuitNetwork networkForCell)
        {
            // If there's only 1 sender on the network then it's impossible for another to be overwriting it
            if (networkForCell.Senders.Count <= 1)
                return true;

            foreach (var sender in networkForCell.Senders)
            {
                if (sender.GetLogicCell() == cell)
                {
                    if (sender.GetLogicValue() <= 0)
                        return false;
                    return true;
                }
            }
            return true;
        }
    }
}




