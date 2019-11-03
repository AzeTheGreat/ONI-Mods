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

        [JsonProperty]
        public float MeshedTilesSunlightReduction { get; set; }

        [Option("Carpet Tile: Tweaked", "When true, the carpet tile uses the custom values set in the confid")]
        [JsonProperty]
        public bool IsCarpetTileTweaked { get; set; }
        [Option("Carpet: Not a Wall", "When true, modifies the carpet's decor distribution so that it only works as a floor.")]
        [JsonProperty]
        public bool IsCarpetNotWall { get; set; }
        [JsonProperty]
        public int CarpetTileDecor { get; set; }
        [JsonProperty]
        public int CarpetTileDecorRadius { get; set; }
        [JsonProperty]
        public bool CarpetTileIsCombustible { get; set; }
        [JsonProperty]
        public float CarpetTileCombustTemp { get; set; }
        [JsonProperty]
        public int CarpetTileReedFiberCount { get; set; }
        [JsonProperty]
        public float CarpetTileMovementSpeed { get; set; }
        

        [Option("Bunker Tile: Tweaked", "When true, the bunker tile uses the custom values set in the config.")]
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
            CarpetTileDecor = TUNING.BUILDINGS.DECOR.BONUS.TIER2.amount;
            CarpetTileDecorRadius = TUNING.BUILDINGS.DECOR.BONUS.TIER2.radius;
            CarpetTileIsCombustible = true;
            CarpetTileCombustTemp = TUNING.BUILDINGS.OVERHEAT_TEMPERATURES.LOW_2;
            CarpetTileReedFiberCount = 1;
            CarpetTileMovementSpeed = DUPLICANTSTATS.MOVEMENT.PENALTY_1;


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
