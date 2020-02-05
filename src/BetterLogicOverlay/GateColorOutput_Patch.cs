using Harmony;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BetterLogicOverlay
{
    [HarmonyPatch(typeof(OverlayModes.Logic), "UpdateUI")]
    class GateOutputColor_Patch
    {
        static bool Prepare() => Options.Opts.FixWireOverwrite;

        // TODO: Robustify or use reflection
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilg)
        {
            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Ldc_I4_0)
                {
                    // Call Helper()
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 5);
                    yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(
                        AccessTools.TypeByName("OverlayModes+Logic+UIInfo"),
                        "cell"));
                    Debug.Log("AZE CI: " + AccessTools.Field(
                        AccessTools.TypeByName("OverlayModes+Logic+UIInfo"),
                        "cell"));
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 6);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(
                        typeof(GateOutputColor_Patch),
                        nameof(GateOutputColor_Patch.Helper)));
                }
                else
                    yield return i;
            }
        }

        static int Helper(int cell, LogicCircuitNetwork networkForCell)
        {
            // If there's only 1 sender on the network then it's impossible for another to be overwriting it
            if (networkForCell.Senders.Count <= 1)
                return 0;

            foreach (var sender in networkForCell.Senders)
            {
                if (sender.GetLogicCell() == cell)
                {
                    if (sender.GetLogicValue() <= 0)
                        return int.MaxValue;
                    return 0;
                }
            }
            return 0;
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




