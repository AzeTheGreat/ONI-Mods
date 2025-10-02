using AzeLib.Attributes;
using Klei;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace DefaultBuildingSettings
{
    /// <summary>
    /// Applies the mod's default settings directly to building prefabs so that newly constructed
    /// instances inherit those values without requiring a Harmony hook on <see cref="BuildingDef.Build"/>.
    /// Doors still need a small helper component so their state machine opens after spawn, but the
    /// remainder of the data can be stamped onto the prefab up-front.
    ///
    /// <para>
    /// The <see cref="ApplyToBuildingPrefabsAttribute"/> marker ensures this logic runs immediately after
    /// <see cref="global::GeneratedBuildings.LoadGeneratedBuildings"/>, centralizing the wiring in
    /// <c>AzeLib</c> so other mods can share the same integration point.
    /// </para>
    /// </summary>
    internal static class BuildingPrefabDefaults
    {
        [ApplyToBuildingPrefabs]
        internal static void Apply(GameObject prefab)
        {
            if (prefab == null)
                return;

            if (SetVacancyOnly(prefab))
                return;

            if (ConfigureDoor(prefab))
                return;

            Storages.SetReservoirValues(prefab);
        }

        private static bool SetVacancyOnly(GameObject go)
        {
            if (!Options.Opts.VacancyOnly)
                return false;

            if (!go.TryGetComponent(out SuitMarker suitMarker))
                return false;

            suitMarker.onlyTraverseIfUnequipAvailable = true;
            return true;
        }

        private static bool ConfigureDoor(GameObject go)
        {
            if (!Options.Opts.OpenDoors)
                return false;

            if (!go.TryGetComponent(out Door door) || Door.DisplacesGas(door.doorType))
                return false;

            go.AddOrGet<DoorSpawnOpener>();
            return true;
        }

        [SerializationConfig(MemberSerialization.OptIn)]
        private sealed class DoorSpawnOpener : KMonoBehaviour
        {
            [MyCmpReq] private Door door;

            protected override void OnSpawn()
            {
                base.OnSpawn();

                if (!Options.Opts.OpenDoors || Door.DisplacesGas(door.doorType))
                    return;

                Schedule();
            }

            private void Schedule() => GameScheduler.Instance.Schedule("OpenDoorAfterOnSpawn", 0f, OpenDoor, door);

            private static void OpenDoor(object comp)
            {
                if (comp is not Door door)
                    return;

                if (!Options.Opts.OpenDoors || Door.DisplacesGas(door.doorType))
                    return;

                if (door.isSpawned)
                {
                    door.QueueStateChange(Door.ControlState.Opened);
                }
                else
                {
                    GameScheduler.Instance.Schedule("OpenDoorAfterOnSpawn", 0f, OpenDoor, door);
                }
            }
        }
    }
}
