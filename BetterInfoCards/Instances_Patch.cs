using Harmony;
using System.Linq;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextScreen), "OnActivate")]
    class Initialize_Patch
    {
        static void Postfix()
        {
            WidgetModifier.Instance = new WidgetModifier();
            ModifyWidgets.Instance = new ModifyWidgets();
        }
    }

    [HarmonyPatch(typeof(HoverTextScreen), nameof(HoverTextScreen.DestroyInstance))]
    class DestroyInstance_Patch
    {
        static void Postfix()
        {
            WidgetModifier.Instance = null;
        }
    }
}
