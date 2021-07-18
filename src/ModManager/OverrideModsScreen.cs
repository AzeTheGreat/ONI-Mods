using HarmonyLib;

namespace ModManager
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Mods))]
    public class OverrideModsScreen
    {
        static bool Prefix()
        {
            // Instantiate the custom mods screen.
            new AModsScreen().GetDialog().Activate();
            // Don't instantiate the default UI.
            return false;
        }
    }
}
