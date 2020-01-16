using Harmony;
using PeterHan.PLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using TMPro;

namespace BetterLogicPortDisplay
{
    [HarmonyPatch(typeof(OverlayModes.Logic), "UpdateUI")]
    class GateOutputColor_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilg)
        {
            bool flag1 = false;

            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Cgt)
                    flag1 = true;

                if (i.opcode == OpCodes.Stloc_S && flag1)
                {
                    flag1 = false;
                    yield return i;

                    // Get jump and the local flag variables
                    var localFlag = i.operand;
                    Label jump = ilg.DefineLabel();

                    // If flag is true
                    yield return new CodeInstruction(OpCodes.Ldloc_S, localFlag);
                    yield return new CodeInstruction(OpCodes.Brfalse, jump);

                    // Call Helper()
                    yield return new CodeInstruction(OpCodes.Ldloc_S, (byte)4);
                    yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(
                        AccessTools.TypeByName("OverlayModes+Logic+UIInfo"),
                        "cell"));
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 6);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(
                        typeof(GateOutputColor_Patch),
                        nameof(GateOutputColor_Patch.Helper)));

                    // Set flag
                    yield return new CodeInstruction(OpCodes.Stloc, localFlag);

                    // End if
                    yield return new CodeInstruction(OpCodes.Nop) { labels = new List<Label> { jump } };
                }
                else
                    yield return i;
            }
        }

        static bool Helper(int cell, LogicCircuitNetwork networkForCell)
        {
            //var uiInfoField = Traverse.Create(__instance).GetField<object>("uiInfo");
            //var uiInfos = Traverse.Create(uiInfoField).Method("GetDataList").GetValue();

            //foreach (var info in ((IEnumerable)uiInfos))
            //{
            //    int cell = Traverse.Create(info).GetField<int>("cell");

            //    GameObject gate = Grid.Objects[cell, (int)ObjectLayer.LogicGates];
            //    var buffer = gate?.GetComponent<LogicGateBuffer>() ?? null;

            //    if (buffer != null && buffer.OutputCell == cell)
            //        updateBuffer( buffer);
            //}

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




