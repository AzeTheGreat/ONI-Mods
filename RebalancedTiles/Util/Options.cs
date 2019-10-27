using Harmony;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;

namespace RebalancedTiles
{
    [JsonObject(MemberSerialization.OptIn)]
    class Options
    {
        [Option("Meshed Tiles Reduce Sunlight", "When true, sunlight's strength will be reduced when moving through airflow and mesh tiles.")]
        [JsonProperty]
        public bool DoMeshedTilesReduceSunlight { get; set; }

        [Option("Meshed Tiles Transparency", "Set to the percentage of light that you would like to be blocked.  Due to limited precision, the actual reduction may be slightly different.")]
        [JsonProperty]
        [Limit(0, 100)]
        public float MeshedTilesSunlightReduction { get; set; }

        [Option("No Carpet Walls", "When true, modifies the carpet's decor distribution so that it only works as a floor.")]
        [JsonProperty]
        public bool IsCarpetNotWall { get; set; }

        public Options()
        {
            DoMeshedTilesReduceSunlight = true;
            MeshedTilesSunlightReduction = 25;
            IsCarpetNotWall = true;
        }

        private static Options _options;
        public static Options Opts {
            get
            {
                if(_options == null)
                    _options = POptions.ReadSettings<Options>() ?? new Options();

                return _options;
            }
            set { _options = value; } }

        public static void OnLoad()
        {
            PUtil.LogModInit();
            POptions.RegisterOptions(typeof(Options));
        }
    }
}
