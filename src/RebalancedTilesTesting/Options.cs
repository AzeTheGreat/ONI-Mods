using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PeterHan.PLib.Options;

namespace RebalancedTilesTesting
{
    [JsonObject(MemberSerialization.OptIn)]
    [ConfigFile("config.json", true)]
    public class Options
    {
        private static Options _opts;
        public static Options Opts
        {
            get => _opts ??= POptions.ReadSettingsForAssembly<Options>() ?? new();
            set => _opts = value;
        }

        [JsonProperty] public ConfigOptions configOptions = new();
    }
}
