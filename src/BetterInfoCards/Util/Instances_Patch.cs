using Harmony;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextScreen), "OnActivate")]
    class Initialize_Patch
    {
        static void Postfix()
        {
            CollectHoverInfo.Instance = new CollectHoverInfo();
            ModifyHits.Instance = new ModifyHits();
        }
    }

    [HarmonyPatch(typeof(HoverTextScreen), nameof(HoverTextScreen.DestroyInstance))]
    class DestroyInstance_Patch
    {
        static void Postfix()
        {
            CollectHoverInfo.Instance = null;
            ModifyHits.Instance = null;
        }
    }
}
