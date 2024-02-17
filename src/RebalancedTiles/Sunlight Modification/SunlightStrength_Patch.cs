using AzeLib.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RebalancedTiles.Mesh_Airflow_Tiles
{
    [HarmonyPatch(typeof(Grid.LightIntensityIndexer), "get_Item")]
    class SunlightStrength_Patch
    {
        static bool Prepare() => Options.Opts.DoMeshedTilesReduceSunlight;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            return codes.Manipulator(
                i => i.OpCodeIs(OpCodes.Conv_R4),
                i => [
                    i,
                    new CodeInstruction(OpCodes.Ldarg_1),
                    CodeInstruction.Call(typeof(SunlightStrength_Patch), nameof(Splice))
                ]);
        }

        // Reference the custom sunlightModifiers grid to adjust the exposedToSunlight local var, which will modify the final light intensity.
        private static float Splice(float exposedToSunlight, int i) => Math.Max(0, exposedToSunlight - SunlightModifierGrid.sunlightModifiers[i]);
    }
}
