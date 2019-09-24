using Harmony;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace BetterDeselect
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        [Option("Separate Overlay", "Requires a separate right click to close the overlay.")]
        [JsonProperty]
        public bool ImplementOverlay { get; set; }

        [Option("Separate Cursor", "Requires a separate right click to close the cursor item.")]
        [JsonProperty]
        public bool ImplementCursor { get; set; }

        [Option("Reselect Fix", "Fixes a vanilla issue that can cause the UI to get into a weird state.")]
        [JsonProperty]
        public bool ImplementReselectFix { get; set; }

        public Options()
        {
            ImplementOverlay = true;
            ImplementCursor = true;
            ImplementReselectFix = true;
        }

        public static void Initialize()
        {
            options = POptions.ReadSettings<Options>() ?? new Options();
        }

        public static Options options;

        public static void OnLoad()
        {
            PUtil.LogModInit();
            options = new Options();
            POptions.RegisterOptions(typeof(Options));
        }

        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public static class Game_OnPrefabInit_Patch
        {
            internal static void Prefix()
            {
                Initialize();
            }
        }
    }
}
