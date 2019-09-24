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

        [Option("Separate Build Menu", "Requires a separate right click to close the build menu. Requires Restart.")]
        [JsonProperty]
        public bool ImplementBuildMenu { get; set; }

        [Option("Selection Mode", "When reselecting a held tool, or opening a category, choose what to do with the selected tool.")]
        [JsonProperty]
        public FixMode Mode { get; set; }

        public enum FixMode
        {
            [Option("Hold Selected", "Continue holding the same tool.")]
            Hold,
            [Option("Deselect Selected", "Deselect the tool.  Closest to vanilla behavior.")]
            Close
        }

        public Options()
        {
            ImplementOverlay = true;
            ImplementBuildMenu = true;
            Mode = FixMode.Hold;
        }

        public static void ReadSettings()
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
                ReadSettings();
            }
        }
    }
}
