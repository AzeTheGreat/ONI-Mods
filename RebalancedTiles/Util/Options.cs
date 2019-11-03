using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using TUNING;

namespace RebalancedTiles
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        public class GenericOptions
        {
            [JsonProperty]
            public bool IsTweaked { get; set; }
            [JsonProperty]
            public int Decor { get; set; }
            [JsonProperty]
            public int DecorRadius { get; set; }
            [JsonProperty]
            public float MovementSpeed { get; set; }
            [JsonProperty]
            public float StrengthMultiplier { get; set; }
        }

        [JsonProperty]
        public GenericOptions Tile;
        [Option("Tile: Tweaked", "When true, normal tile uses custom values set in the config.")]
        public bool _IsTileTweaked { get { return Tile.IsTweaked; } set { Tile.IsTweaked = value; } }

        [JsonProperty]
        public GenericOptions BunkerTile;
        [Option("Bunker: Tweaked", "When true, bunker tile uses custom values set in the config.")]
        public bool _IsBunkerTweaked { get { return BunkerTile.IsTweaked; } set { BunkerTile.IsTweaked = value; } }

        [JsonProperty]
        public CarpetTileOptions CarpetTile;
        public class CarpetTileOptions : GenericOptions
        {
            [JsonProperty]
            public bool IsNotWall { get; set; }
            [JsonProperty]
            public bool IsCombustible { get; set; }
            [JsonProperty]
            public float CombustTemp { get; set; }
            [JsonProperty]
            public int ReedFiberCount { get; set; }
        }
        [Option("Carpet: Tweaked", "When true, carpet tile uses custom values set in the config.")]
        public bool _IsCarpetTweaked { get { return CarpetTile.IsTweaked; } set { CarpetTile.IsTweaked = value; } }
        [Option("Carpet: Not a Wall", "When true, modifies the carpet's decor distribution so that it only works as a floor.")]
        public bool _IsCarpetNotWall { get { return CarpetTile.IsNotWall; } set { CarpetTile.IsNotWall = value; } }
        [Option("Carpet: Combusts", "When true, carpet tiles will burn into normal tiles at high temperatures.")]
        public bool _IsCarpetCombustible { get { return CarpetTile.IsCombustible; } set { CarpetTile.IsCombustible = value; } }

        [JsonProperty]
        public GenericOptions GasPermeableMembrane;
        [Option("Tile: Tweaked", "When true, normal tile uses custom values set in the config.")]
        public bool _IsAirflowTweaked { get { return GasPermeableMembrane.IsTweaked; } set { GasPermeableMembrane.IsTweaked = value; } }

        [JsonProperty]
        public GenericOptions MetalTile;
        [Option("Metal: Tweaked", "When true, metal tile uses custom values set in the config.")]
        public bool _IsMetalTweaked { get { return MetalTile.IsTweaked; } set { MetalTile.IsTweaked = value; } }

        [JsonProperty]
        public GenericOptions GlassTile;
        [Option("Window: Tweaked", "When true, window tile uses custom values set in the config.")]
        public bool _IsGlassTweaked { get { return GlassTile.IsTweaked; } set { GlassTile.IsTweaked = value; } }

        [JsonProperty]
        public GenericOptions MeshTile;
        [Option("Tile: Tweaked", "When true, normal tile uses custom values set in the config.")]
        public bool _IsMeshTweaked { get { return MeshTile.IsTweaked; } set { MeshTile.IsTweaked = value; } }

        [Option("Meshed Tiles: Reduce Sunlight", "When true, sunlight's strength will be reduced when moving through airflow and mesh tiles.")]
        [JsonProperty]
        public bool DoMeshedTilesReduceSunlight { get; set; }
        [JsonProperty]
        public float MeshedTilesSunlightReduction { get; set; }

        public Options()
        {
            BunkerTile = new GenericOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.PENALTY.TIER0.amount,
                DecorRadius = BUILDINGS.DECOR.PENALTY.TIER0.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.NEUTRAL,
                StrengthMultiplier = 10f
            };
            CarpetTile = new CarpetTileOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.BONUS.TIER2.amount,
                DecorRadius = BUILDINGS.DECOR.BONUS.TIER2.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.PENALTY_1,
                StrengthMultiplier = 1f,
                IsNotWall = true,
                IsCombustible = true,
                CombustTemp = BUILDINGS.OVERHEAT_TEMPERATURES.LOW_2,
                ReedFiberCount = 1
            };
            MetalTile = new GenericOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.BONUS.TIER1.amount,
                DecorRadius = BUILDINGS.DECOR.BONUS.TIER0.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.BONUS_3,
                StrengthMultiplier = 2f
            };
            Tile = new GenericOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.BONUS.TIER0.amount,
                DecorRadius = BUILDINGS.DECOR.BONUS.TIER0.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.BONUS_2,
                StrengthMultiplier = 1.5f
            };
            GlassTile = new GenericOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.BONUS.TIER1.amount,
                DecorRadius = BUILDINGS.DECOR.BONUS.TIER0.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.BONUS_3,
                StrengthMultiplier = 0.5f
            };

            DoMeshedTilesReduceSunlight = true;
            MeshedTilesSunlightReduction = 25;
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
