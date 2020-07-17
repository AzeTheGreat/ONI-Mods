using Harmony;
using PeterHan.PLib;
using System.Collections.Generic;

namespace CleanHUD
{
    [HarmonyPatch(typeof(ManagementMenu), "OnPrefabInit")]
    class SmallButtons
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(
                AccessTools.Method(typeof(KIconToggleMenu), nameof(KIconToggleMenu.Setup), new[] { typeof(IList<KIconToggleMenu.ToggleInfo>) }),
                AccessTools.Method(typeof(SmallButtons), nameof(SmallButtons.SetupWrapper)));
        }

        private static void SetupWrapper(IList<KIconToggleMenu.ToggleInfo> toggleInfo)
        {
            if (Options.Opts.UseSmallButtons)
            {
                foreach (var toggle in toggleInfo)
                    toggle.prefabOverride = ManagementMenu.Instance.smallPrefab;
            }

            ManagementMenu.Instance.Setup(toggleInfo);
        }
    }
}
