using HarmonyLib;

namespace DefaultBuildingSettings
{
    [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    class LoadGeneratedBuildings_Patch
    {
        static void Postfix()
        {
            foreach (var def in Assets.BuildingDefs)
            {
                var go = def.BuildingComplete;

                Generators.SetGeneratorValues(go);           
            }
        }
    }
}
