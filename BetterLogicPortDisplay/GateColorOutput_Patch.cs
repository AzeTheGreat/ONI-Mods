using Harmony;
using PeterHan.PLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BetterLogicPortDisplay
{
    [HarmonyPatch(typeof(OverlayModes.Logic), "UpdateUI")]
    public class GateOutputColor_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilg)
        {
            bool flag1 = false;
            //bool flag2 = false;
            //bool flag3 = false;

            //object localCircuitManager = null;
            //object localCircuitNetwork = null;

            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Cgt)
                    flag1 = true;

                //if (i.opcode == OpCodes.Ldfld && i.operand == AccessTools.Field(
                //    typeof(Game),
                //    nameof(Game.logicCircuitManager)))
                //    flag2 = true;

                //if (i.opcode == OpCodes.Callvirt && i.operand == AccessTools.Method(
                //    typeof(LogicCircuitManager),
                //    nameof(LogicCircuitManager.GetNetworkForCell)))
                //    flag3 = true;

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
                    yield return new CodeInstruction(OpCodes.Box, typeof(int));
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 6);
                    yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(
                        typeof(GateOutputColor_Patch),
                        nameof(GateOutputColor_Patch.Helper)));

                    // Set flag
                    yield return new CodeInstruction(OpCodes.Stloc, localFlag);

                    // End if
                    yield return new CodeInstruction(OpCodes.Nop) { labels = new List<Label> { jump } };
                }
                else
                    yield return i;

                //if(i.opcode == OpCodes.Ldloca_S && flag2)
                //{
                //    localCircuitManager = i.operand;
                //    flag2 = false;
                //}

                //if(i.opcode == OpCodes.Stloc_S && flag3)
                //{
                //    localCircuitNetwork = i.operand;
                //    flag3 = false;
                //}
            }
        }

        public static bool Helper(object cell, LogicCircuitNetwork networkForCell)
        {
            foreach (var sender in networkForCell.Senders)
            {
                if (sender.GetLogicCell() == (int)cell && sender.GetLogicValue() <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static void OnLoad()
        {
            PUtil.LogModInit();
        }
    }
}




