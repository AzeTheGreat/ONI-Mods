using Harmony;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;


namespace ONIFixedCameraPan
{
    [HarmonyPatch(typeof(CameraController), "NormalCamUpdate")]
    public class Patch_CameraController_NormalCamUpdate
    {
        private static float maxDeltaTime = 0.1f;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo targetFieldInfo = AccessTools.Field(typeof(CameraController), nameof(CameraController.keyPanningSpeed));

            foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Ldfld && i.operand == targetFieldInfo)
                {
                    yield return i;
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(Patch_CameraController_NormalCamUpdate), nameof(Patch_CameraController_NormalCamUpdate.GetScalar)));
                    yield return new CodeInstruction(OpCodes.Mul);
                }

                else
                    yield return i;
            }
        }

        private static float GetScalar(float unscaledDeltaTime)
        {
            return Mathf.Clamp(unscaledDeltaTime, 0f, maxDeltaTime);
        }
    }
}
