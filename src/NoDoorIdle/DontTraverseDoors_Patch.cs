using HarmonyLib;

namespace NoDoorIdle
{
    [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    class DontTraverseDoors_Patch
    {
        static bool Prepare() => !Options.Opts.CanIdleTraverseDoors;
        static void Postfix()
        {
            foreach (var def in Assets.BuildingDefs)
            {
                var go = def.BuildingComplete;
                if (go.GetComponent<Door>())
                    def.PreventIdleTraversalPastBuilding = true;
            }
        }
    }
}
