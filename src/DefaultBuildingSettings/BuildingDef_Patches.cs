using Harmony;
using UnityEngine;

namespace DefaultBuildingSettings
{
    // Not an ideal approach, but the best that can be done given the game's current structure
    // TODO: Check periodically and see if the configs can just be edited as normal.
    [HarmonyPatch(typeof(BuildingDef), nameof(BuildingDef.Build))]
    public class BuildingDef_Patches
    {
        static void Postfix(GameObject __result)
        {
            if (Options.Opts.VacancyOnly)
            {
                var suitMarker = __result.GetComponent<SuitMarker>();
                if (suitMarker)
                    Traverse.Create(suitMarker).Field("onlyTraverseIfUnequipAvailable").SetValue(true);
            }

            // Can technically fail if a save/load occurs after .Build is called, before the Door.isSpawned.
            if (Options.Opts.OpenDoors)
            {
                var door = __result.GetComponent<Door>();
                if (door)
                {
                    bool doesDisplaceGas = (bool)AccessTools.Method(typeof(Door), "DisplacesGas").Invoke(null, new object[] { door.doorType });
                    if (!doesDisplaceGas)
                        Schedule();
                }
    
                void OpenDoor(object comp)
                {
                    var d = comp as Door;
                    if (d.isSpawned)
                        d.QueueStateChange(Door.ControlState.Opened);
                    else
                        Schedule();
                }

                void Schedule() => GameScheduler.Instance.Schedule("OpenDoorAfterOnSpawn", 0f, OpenDoor, door);
            }
        }
    }
}
