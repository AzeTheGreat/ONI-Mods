using Harmony;
using PeterHan.PLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.UI;

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

        // Failed alternate reflection attempt.  Issues:
        // Color not getting set on the actual image, probably some ref issue
        // Sensors don't have ILogicSender components.  Not sure how when they work in the transpiler...
        // Looks like on spawn it creates LogicPorts and adds them to the LogicCircuitNetworkManager...

        //static void Postfix(OverlayModes.Logic __instance)
        //{
        //    var uiInfoField = Traverse.Create(__instance).GetField<object>("uiInfo");
        //    var uiInfos = Traverse.Create(uiInfoField).Method("GetDataList").GetValue();

        //    foreach (var info in ((IEnumerable)uiInfos))
        //    {
        //        Debug.Log(1);
        //        var infoTrav = Traverse.Create(info);
        //        int cell = infoTrav.GetField<int>("cell");

        //        GameObject sourceGO = Grid.Objects[cell, (int)ObjectLayer.LogicGates];
        //        if(sourceGO == null)
        //            sourceGO = Grid.Objects[cell, (int)ObjectLayer.Building];
        //        Debug.Log("Source GO: " + sourceGO);
        //        Debug.Log("Test value: " + (sourceGO?.GetComponent<ILogicEventSender>()?.GetLogicValue() ?? 1));
        //        if((sourceGO?.GetComponent<ILogicEventSender>()?.GetLogicValue() ?? 1) <= 0)
        //        {
        //            Debug.Log(2);
        //            Image image = infoTrav.GetField<Image>("image");
        //            LogicModeUI logicModeUI = Assets.instance.logicModeUIData;
        //            if (image.color == logicModeUI.colourOn)
        //            {
        //                Traverse.Create(image).SetField("color", logicModeUI.colourOff);
        //                Debug.Log("Color overriden");
        //            }       
        //        }
        //    }
        //}
    }
}




