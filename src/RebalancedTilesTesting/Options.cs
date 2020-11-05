using AzeLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PeterHan.PLib.Options;
using System.Collections;
using System.Collections.Generic;

namespace RebalancedTilesTesting
{
    [JsonObject(MemberSerialization.OptIn)]
    [RestartRequired]
    public class Options : BaseOptions<Options>
    {
        // Needs to be static so that it is serialized correctly by POptions.
        // If instanced, the instance modified through Opts is different than the instance serialized by POptions.
        [JsonProperty] private static ConfigOptions configOptions;

        // Needs to be static to persist though Options re-creation since it's only built once.
        private static Dictionary<string, UIConfigOptions> uiConfigOptions;

        // Force accessing static members through the instance so that proper BaseOptions initialization occurs.
        public ConfigOptions ConfigOptions => configOptions;
        public Dictionary<string, UIConfigOptions> UIConfigOptions => uiConfigOptions;

        public override IEnumerable CreateOptions()
        {
            //foreach (var tileOptions in uiConfigOptions)
            //    foreach (var option in tileOptions.Value.GetOptions())
            //        yield return option;
            yield return new EmbeddedOptions();
        }

        // TODO: Only rewrite if something changed.
        public override bool ValidateSettings() => configOptions.CleanUp();

        public Options()
        {
            uiConfigOptions ??= new Dictionary<string, UIConfigOptions>();
            configOptions ??= new ConfigOptions();
        }
    }
}

