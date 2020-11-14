using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RebalancedTilesTesting
{
    [JsonObject(MemberSerialization.OptIn)]
    [RestartRequired]
    public class Options : Serializable<Options>
    {
        private static Options _opts;
        public static Options Opts
        {
            get => _opts ??= Deserialize();
            set => _opts = value;
        }

        [JsonProperty] public ConfigOptions configOptions;
        // Needs to be static to persist though Options re-creation since it's only built once.
        public static Dictionary<string, UIConfigOptions> uiConfigOptions = new();

        public Options()
        {
            configOptions ??= new ConfigOptions();
        }

        protected override void OnSerialize() => configOptions.CleanUp();
    }
}
