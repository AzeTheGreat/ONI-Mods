using Harmony;

namespace NoMeteors
{
    [HarmonyPatch(typeof(SeasonManager), "SpawnBombard")]
    public class NoMeteors_Patch
    {
        static bool Prefix() => false;
    }
}
