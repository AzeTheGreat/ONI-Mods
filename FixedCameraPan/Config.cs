using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.IO;
using Newtonsoft.Json;

namespace ONIFixedCameraPan
{
    [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
    class Patch_CameraController_OnPrefabInit
    {
        private static float defaultMultiplier = 120f;

        static void Postfix(ref float ___keyPanningSpeed)
        {
            ___keyPanningSpeed *= GetMultiplier();
        }

        private static float GetMultiplier()
        {
            Config config = ReadConfig();

            if (config == null)
            {
                CreateConfig();
                return defaultMultiplier;
            }
            else
            {
                return config.multiplier;
            }
        }

        private static Config ReadConfig()
        {
            Config config = null;
            string directory = Path.GetDirectoryName(
                new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            var configPath = Path.Combine(directory, "config");

            try
            {
                using (var r = new StreamReader(configPath))
                {
                    var json = r.ReadToEnd();
                    config = JsonConvert.DeserializeObject<Config>(json);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error reading config file with exception: " + e.Message);
                return config;
            }

            return config;
        }

        private static void CreateConfig()
        {
            Config config = new Config();
            config.multiplier = defaultMultiplier;

            string directory = Path.GetDirectoryName(
                new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            var configPath = Path.Combine(directory, "config");

            using (var r = new StreamWriter(configPath))
            {
                var serialized = JsonConvert.SerializeObject(config, Formatting.Indented);
                r.Write(serialized);
            }
        }
    }

    class Config
    {
        public float multiplier;
    }
}
