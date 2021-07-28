using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace ModManager
{
    [HarmonyPatch(typeof(ModsScreen), nameof(ModsScreen.OnActivate))]
    [HarmonyPriority(Priority.Last)]
    public class OverrideModsScreen
    {
        static void Postfix(ModsScreen __instance)
        {
            // TODO: Remove for release.
            LogGO(__instance.gameObject);

            // Instantiate the custom mods screen.
            new AModsScreen()
            {
                ModUIExtractions = __instance.displayedMods.Select(x => new ModUIExtract(x, __instance)).ToList(),
                // Need to reinstantiate to ensure the the "prefab" doesn't get destroyed.
                DragElementPrefab = Object.Instantiate(__instance.entryPrefab.transform.Find("DragReorderIndicator").gameObject)
            }
            .GetDialog()
            .Activate();

            // This is allowed to activate so that necessary information can be extracted.
            // But it must be deactivated so that there aren't two UIs.
            __instance.Deactivate();
        }

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
