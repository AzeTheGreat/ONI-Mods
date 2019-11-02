using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using TUNING;

namespace RebalancedTiles
{
    [JsonObject(MemberSerialization.OptIn)]
    class Options
    {
        [Option("Meshed Tiles: Reduce Sunlight", "When true, sunlight's strength will be reduced when moving through airflow and mesh tiles.")]
        [JsonProperty]
        public bool DoMeshedTilesReduceSunlight { get; set; }

        [Option("Meshed Tiles: Transparency", "Set to the percentage of light that you would like to be blocked.  Due to limited precision, the actual reduction may be slightly different.")]
        [JsonProperty]
        [Limit(0, 100)]
        public float MeshedTilesSunlightReduction { get; set; }

        [Option("Carpet: Not a Wall", "When true, modifies the carpet's decor distribution so that it only works as a floor.")]
        [JsonProperty]
        public bool IsCarpetNotWall { get; set; }

        [Option("Bunker Tile: Tweaked", "When true, the bunker tile uses the custom values set.")]
        [JsonProperty]
        public bool IsBunkerTileTweaked { get; set; }

        [Option("Bunker Tile: Decor", "The decor value of the tile.")]
        [JsonProperty]
        [Limit(-999, 999)]
        public float BunkerTileDecor { get; set; }

        [Option("Bunker Tile: Decor Radius", "The decor radius of the tile.")]
        [JsonProperty]
        [Limit(1, 5)]
        public float BunkerTileDecorRadius { get; set; }

        public Options()
        {
            DoMeshedTilesReduceSunlight = true;
            MeshedTilesSunlightReduction = 25;

            IsCarpetNotWall = true;

            IsBunkerTileTweaked = true;
            BunkerTileDecor = BUILDINGS.DECOR.PENALTY.TIER0.amount;
            BunkerTileDecorRadius = BUILDINGS.DECOR.PENALTY.TIER0.radius;

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
