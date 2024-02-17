using UnityEngine;

namespace RebalancedTiles
{
    public static class GenericPatch
    {
        public static bool ShouldPatchCreateBuildingDef(Options.GenericOptions options) => options.Decor != null || options.DecorRadius != null;

        public static void CreateBuildingDef(BuildingDef __result, Options.GenericOptions options)
        {
            if(options.Decor is int decor)
                __result.BaseDecor = decor;
            if(options.DecorRadius is int radius)
                __result.BaseDecorRadius = radius;
        }

        public static bool ShouldPatchConfigureBuildingTemplate(Options.GenericOptions options) => options.MovementSpeed != null || options.StrengthMultiplier != null;

        public static void ConfigureBuildingTemplate(GameObject go, Options.GenericOptions options)
        {
            SimCellOccupier simCellOccupier = go.GetComponent<SimCellOccupier>();
            if(options.MovementSpeed is int speed)
                simCellOccupier.movementSpeedMultiplier = speed;
            if(options.StrengthMultiplier is int strength)
            simCellOccupier.strengthMultiplier = strength;
        }
    }
}
