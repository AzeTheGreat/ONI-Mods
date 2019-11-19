using Harmony;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextScreen), "OnActivate")]
    class Initialize_Patch
    {
        static void Postfix()
        {
            ModifyWidgets.Instance = new ModifyWidgets();
        }
    }

    [HarmonyPatch(typeof(HoverTextScreen), nameof(HoverTextScreen.DestroyInstance))]
    class DestroyInstance_Patch
    {
        static void Postfix()
        {
            ModifyWidgets.Instance = null;
        }
    }
}
