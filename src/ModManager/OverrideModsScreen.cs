using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModManager
{
    [HarmonyPatch(typeof(ModsScreen), nameof(ModsScreen.OnActivate))]
    [HarmonyPriority(Priority.Last)]
    public class OverrideModsScreen
    {
        public static List<ModUIExtract> ModUIExtractions { get; set; }

        static void Postfix(ModsScreen __instance)
        {
            ModUIExtractions = ExtractFromMods(__instance.displayedMods);
            LogGO(__instance.gameObject);

            // Instantiate the custom mods screen.
            new AModsScreen().GetDialog().Activate();

            // This is allowed to activate so that necessary information can be extracted.
            // But it must be deactivated so that there aren't two UIs.
            __instance.Deactivate();
        }

        private static List<ModUIExtract> ExtractFromMods(List<ModsScreen.DisplayedMod> mods) =>
            mods.Select(x => new ModUIExtract(x)).ToList();


        public static void LogGO(GameObject go, int level = 0)
        {
            Debug.Log(new string(' ', level * 3) + "GO: " + go.name);

            foreach (var cmp in go.GetComponents<object>())
                Debug.Log(new string(' ', level * 3 + 1) + cmp.GetType());

            level++;
            foreach (Transform child in go.transform)
                LogGO(child.gameObject, level);
        }
    }
}
