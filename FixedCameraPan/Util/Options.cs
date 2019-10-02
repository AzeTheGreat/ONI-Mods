using Harmony;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace FixedCameraPan
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        [Option("Pan Speed", "Set to the framerate at which you find the speed comfortable.  At 60, the camera will always move at the same speed as it mvoes when the game is at 60 FPS.")]
        [Limit(1, 999)]
        [JsonProperty]
        public float PanSpeed { get; set; }

        public Options()
        {
            PanSpeed = 120f;
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
