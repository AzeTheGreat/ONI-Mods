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
    public class Options : BaseOptions<Options>
    {
        [JsonProperty] Dictionary<string, TileOptions> genericOptions = new Dictionary<string, TileOptions>();

        public override IEnumerable CreateOptions()
        {
            foreach (var def in Assets.BuildingDefs)
            {
                if (!def.name.Contains("Tile") || !def.ShowInBuildMenu || def.Deprecated || !TUNING.BUILDINGS.PLANORDER.Any(x => ((List<string>)x.data).Contains(def.PrefabID)))
                    continue;

                if(!genericOptions.TryGetValue(def.PrefabID, out var tileOptions))
                    genericOptions.Add(def.PrefabID, tileOptions = new TileOptions());

                foreach (var option in tileOptions.RebuildOptions(def))
                    yield return option;
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TileOptions
    {
        [JsonProperty] public DefaultIntOptionsEntry decor;
        [JsonProperty] public DefaultIntOptionsEntry decorRadius;
        [JsonProperty] public DefaultIntOptionsEntry strengthMult;
        [JsonProperty] public DefaultIntOptionsEntry moveSpeedMult;

        public IEnumerable RebuildOptions(BuildingDef def)
        {
            yield return decor = new DefaultIntOptionsEntry("Decor", "t", (int)def.BaseDecor, def.Name, decor);
            yield return decorRadius = new DefaultIntOptionsEntry("Decor Radius", "t", (int)def.BaseDecorRadius, def.Name, decorRadius);

            var simCell = def.BuildingComplete.GetComponent<SimCellOccupier>();

            yield return strengthMult = new DefaultIntOptionsEntry("Tile Strength", "t", (int)simCell.strengthMultiplier, def.Name, strengthMult);
            yield return moveSpeedMult = new DefaultIntOptionsEntry("Movement Speed", "t", (int)simCell.movementSpeedMultiplier, def.Name, moveSpeedMult);

            yield return new ButtonTest("Reset", "T", def.Name)
            {
                Value = (System.Action)(() => decor.Value = decorRadius.Value = strengthMult.Value = moveSpeedMult.Value = null)
            };
        }

        private class ButtonTest : ButtonOptionsEntry
        {
            public ButtonTest(string title, string tooltip, string category = "") : base(title, tooltip, category) { }
        }
    }
}

