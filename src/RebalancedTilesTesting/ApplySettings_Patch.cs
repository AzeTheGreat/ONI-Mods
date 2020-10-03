using AzeLib.Extensions;
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RebalancedTilesTesting
{
    [HarmonyPatch(typeof(BuildingConfigManager), nameof(BuildingConfigManager.RegisterBuilding))]
    class ApplySettings_Patch
    {
        // Since this is a major method, don't replace the methods, instead insert custom methods just after.
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var createBuildingDef = AccessTools.Method(typeof(IBuildingConfig), nameof(IBuildingConfig.CreateBuildingDef));

            foreach (var i in instructions)
            {
                if (i.OperandIs(createBuildingDef))
                {
                    yield return i;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ApplySettings_Patch), nameof(ApplySettings_Patch.PostCreateBuildingDef)));
                }
                else
                    yield return i;
            }
        }

        private static BuildingDef PostCreateBuildingDef(BuildingDef def)
        {
            Filter.SetDef(def);
            return def;
        }
    }
}
