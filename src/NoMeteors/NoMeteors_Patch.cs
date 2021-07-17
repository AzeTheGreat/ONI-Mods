using HarmonyLib;
using Klei.AI;

namespace NoMeteors
{
    [HarmonyPatch(typeof(MeteorShowerEvent.StatesInstance), nameof(MeteorShowerEvent.StatesInstance.SpawnBombard))]
    public class NoMeteors_Patch
    {
        static bool Prefix() => false;
    }
}
