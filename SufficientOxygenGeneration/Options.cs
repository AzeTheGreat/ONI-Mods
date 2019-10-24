using Harmony;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace SufficientOxygenGeneration
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        [Option("Notification Threshold", "Set to net oxygen production in kg below which you'd like to be notified.")]
        [Limit(-99999f, 0.0001f)]
        [JsonProperty]
        public float OxygenThreshold { get; set; }

        public Options()
        {
            OxygenThreshold = -10f;
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
