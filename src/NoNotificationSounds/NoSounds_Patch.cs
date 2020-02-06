using Harmony;
using PeterHan.PLib;

namespace NoNotificationSounds
{
    [HarmonyPatch(typeof(NotificationScreen), "PlayDingSound")]
    public class NoSounds_Patch
    {
        static bool Prefix()
        {
            return false;
        }
    }
}
