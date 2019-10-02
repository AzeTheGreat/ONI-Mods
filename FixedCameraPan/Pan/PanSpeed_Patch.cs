using Harmony;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FixedCameraPan.Pan
{
    [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
    class PanSpeed_Patch
    {
        static void Postfix(ref float ___keyPanningSpeed)
        {
            Options.ReadSettings();
            ___keyPanningSpeed *= Options.options.PanSpeed;
        }
    }
}
