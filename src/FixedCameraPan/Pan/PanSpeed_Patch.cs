using HarmonyLib;

namespace FixedCameraPan.Pan
{
    [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
    class PanSpeed_Patch
    {
        static void Postfix(ref float ___keyPanningSpeed)
        {
            ___keyPanningSpeed *= Options.Opts.PanSpeed / 100f;
        }
    }
}
