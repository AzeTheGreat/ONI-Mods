using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;
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
        [JsonProperty] public static Dictionary<string, Dictionary<string, object>> serializedValues;

        // Needs to be static to persist as Options are created since it's only built once.
        private static Dictionary<string, Dictionary<string, DefaultIntOptionsEntry>> tileOptions;

        public override IEnumerable CreateOptions()
        {
            foreach (var tileOptions in tileOptions)
                foreach (var option in tileOptions.Value)
                    yield return option.Value;
        }

        public Dictionary<string, DefaultIntOptionsEntry> RebuildAndGetOptions(BuildingDef def)
        {
            if (!def.name.Contains("Tile") || !def.ShowInBuildMenu || def.Deprecated || !TUNING.BUILDINGS.PLANORDER.Any(x => ((List<string>)x.data).Contains(def.PrefabID)))
                return null;

            if (!tileOptions.TryGetValue(def.PrefabID, out var options))
                tileOptions.Add(def.PrefabID, options = new Dictionary<string, DefaultIntOptionsEntry>());

            options.Add("BaseDecor", new DefaultIntOptionsEntry("Decor", string.Empty, (int)def.BaseDecor, def.PrefabID, "BaseDecor", def.Name));
            options.Add("BaseDecorRadius", new DefaultIntOptionsEntry("Decor Radius", string.Empty, (int)def.BaseDecorRadius, def.PrefabID, "BaseDecorRadius", def.Name));

            return options;
        }

        public Options()
        {
            tileOptions ??= new Dictionary<string, Dictionary<string, DefaultIntOptionsEntry>>();
            serializedValues ??= new Dictionary<string, Dictionary<string, object>>();
        }
    }
}

