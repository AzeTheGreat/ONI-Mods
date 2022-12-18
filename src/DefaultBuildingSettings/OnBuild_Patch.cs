using HarmonyLib;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DefaultBuildingSettings
{
    // Not an ideal approach, but the best that can be done given the game's current structure
    // TODO: Check periodically and see if the configs can just be edited as normal.

    // Prefer patching in roughly this order:
    //  Config - For single changes, requires that data be copied between GOs.
    //  OnPrefabInit - Set fields, can work on non [SerializeField]s.
    //  InitializeStates - Set default state.
    //  LoadGeneratedBuildings - Preferred if it allows mutliple to be handled, requires that data be copied between GOs.
    //  Build - Last resort.

    [HarmonyPatch]
    internal class OnBuild_Patch
    {
        // Patch the "base" BuildingDef.Build overload.
        // TODO: If needed again, break this behavior out into an attribute.
        static MethodInfo TargetMethod() => typeof(BuildingDef).GetMethods()
            .Where(x => x.Name == nameof(BuildingDef.Build))
            .OrderBy(x => x.GetParameters().Count()).FirstOrDefault();

        static void Postfix(GameObject __result)
        {
            // Early outs to reduce unnecessary testing.  If it's a door it can't be a reservoir.
            if (SetVacancyOnly(__result))
                return;
            if(OpenDoors(__result))
                return;
            Storages.SetReservoirValues(__result);
        }

        private static bool SetVacancyOnly(GameObject go)
        {
            if (!Options.Opts.VacancyOnly || go.GetComponent<SuitMarker>() is not SuitMarker suitMarker)
                return false;

            suitMarker.onlyTraverseIfUnequipAvailable = true;
            return true;
        }

        private static bool OpenDoors(GameObject go)
        {
            // Can technically fail to open if a save/load occurs after .Build is called, before the Door.isSpawned.
            if (!Options.Opts.OpenDoors || go.GetComponent<Door>() is not Door door || Door.DisplacesGas(door.doorType))
                return false;

            Schedule();
            return true;

            void Schedule() => GameScheduler.Instance.Schedule("OpenDoorAfterOnSpawn", 0f, OpenDoor, door);

            void OpenDoor(object comp)
            {
                var d = comp as Door;

                // Can't pattern match due to unity equality nonsense.
                if (d == null)
                    return;

                if (d.isSpawned)
                    d.QueueStateChange(Door.ControlState.Opened);
                else
                    Schedule();
            }
        }
    }
}
