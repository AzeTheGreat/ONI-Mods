using UnityEngine;

namespace RebalancedTiles
{
    public static class GenericPatch
    {
        public static void CreateBuildingDef(BuildingDef __result, Options.GenericOptions options)
        {
            __result.BaseDecor = options.Decor;
            __result.BaseDecorRadius = options.DecorRadius;
        }

        public static void ConfigureBuildingTemplate(GameObject go, Options.GenericOptions options)
        {
            SimCellOccupier simCellOccupier = go.GetComponent<SimCellOccupier>();
            simCellOccupier.movementSpeedMultiplier = options.MovementSpeed;
            simCellOccupier.strengthMultiplier = options.StrengthMultiplier;
        }
    }
}
