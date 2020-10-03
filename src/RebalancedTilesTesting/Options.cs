using AzeLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RebalancedTilesTesting
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("TEST", null, null, true)]
    [RestartRequired]
    public class Options : BaseOptions<Options>
    {
        // Needs to be static so that it is serialized correctly by POptions.
        // If instanced, the instance modified through Opts is different than the instance serialized by POptions.
        [JsonProperty] public static SerializedConfigs serializedValues;

        // Needs to be static to persist though Options re-creation since it's only built once.
        private static Dictionary<string, ConfigOptions> tileOptions;

        public override IEnumerable CreateOptions()
        {
            foreach (var tileOptions in tileOptions)
                foreach (var option in tileOptions.Value.GetOptions())
                    yield return option; 
        }

        public ConfigOptions RebuildAndGetOptions(BuildingDef def)
        {
            if (!def.name.Contains("Tile") || !def.ShowInBuildMenu || def.Deprecated || !TUNING.BUILDINGS.PLANORDER.Any(x => ((List<string>)x.data).Contains(def.PrefabID)))
                return null;

            if (!tileOptions.TryGetValue(def.PrefabID, out var options))
                tileOptions.Add(def.PrefabID, options = new ConfigOptions());

            options.AddOption(def, "BaseDecor", "Decor");
            options.AddOption(def, "BaseDecorRadius", "Decor Radius");

            return options;
        }

        // TODO: Only rewrite if something changed.
        public override bool ValidateSettings() => serializedValues.CleanUp();

        public Options()
        {
            tileOptions ??= new Dictionary<string, ConfigOptions>();
            serializedValues ??= new SerializedConfigs();
        }
    }
}

