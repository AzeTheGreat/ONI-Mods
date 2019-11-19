using Harmony;
using System.Linq;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
    class EditWidgets_Patch
    {
        static void Postfix()
        {
            WidgetModifier.Instance.ModifyWidgets();
        }
    }

    [HarmonyPatch(typeof(HoverTextScreen), "OnActivate")]
    class Initialize_Patch
    {
        static void Postfix()
        {
            WidgetModifier.Instance = new WidgetModifier();
            DrawnWidgets.Instance = new DrawnWidgets();
            GetWidgets_Patch.Initialize();
        }
    }

    [HarmonyPatch(typeof(HoverTextScreen), nameof(HoverTextScreen.DestroyInstance))]
    class DestroyInstance_Patch
    {
        static void Postfix()
        {
            WidgetModifier.Instance = null;
            DrawnWidgets.Instance = null;
        }
    }
}
