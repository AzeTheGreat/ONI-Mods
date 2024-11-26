using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using TUNING;

namespace RebalancedTiles
{
    [ModInfo(null, null, true)]
    [RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option("STRINGS.BUILDINGS.PREFABS.TILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.TILE.NAME")]
        public GenericOptions Tile { get; set; } = new();

        [Option("STRINGS.BUILDINGS.PREFABS.BUNKERTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.BUNKERTILE.NAME")]
        public GenericOptions BunkerTile { get; set; } = new();

        [Option("STRINGS.BUILDINGS.PREFABS.CARPETTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.CARPETTILE.NAME")]
        public CarpetTileOptions CarpetTile { get; set; } = new()
        {
            IsNotWall = true,
            CombustTemp = BUILDINGS.OVERHEAT_TEMPERATURES.LOW_2,
            Decor = BUILDINGS.DECOR.BONUS.TIER2.amount,
            MovementSpeed = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_1,
        };

        [Option("STRINGS.BUILDINGS.PREFABS.GLASSTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.GLASSTILE.NAME")]
        public GlassTileOptions GlassTile { get; set; } = new()
        {
            StrengthMultiplier = 0.5f,
            MovementSpeed = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_2,
        };

        [Option("STRINGS.BUILDINGS.PREFABS.METALTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.METALTILE.NAME")]
        public GenericOptions MetalTile { get; set; } = new()
        {
            Decor = BUILDINGS.DECOR.BONUS.TIER0.amount
        };

        [Option("STRINGS.BUILDINGS.PREFABS.MESHTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.MESHTILE.NAME")]
        public PermeableTileOptions MeshTile { get; set; } = new()
        {
            LightAbsorptionFactor = 0.25f
        };

        [Option("STRINGS.BUILDINGS.PREFABS.GASPERMEABLEMEMBRANE.NAME", null, "STRINGS.BUILDINGS.PREFABS.GASPERMEABLEMEMBRANE.NAME")]
        public PermeableTileOptions GasPermeableMembrane { get; set; } = new()
        {
            LightAbsorptionFactor = 0.25f
        };

        [Option("STRINGS.BUILDINGS.PREFABS.PLASTICTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.PLASTICTILE.NAME")]
        public GenericOptions PlasticTile { get; set; } = new();


        [Option("STRINGS.BUILDINGS.PREFABS.INSULATIONTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.INSULATIONTILE.NAME")]
        public GenericOptions InsulationTile { get; set; } = new()
        {
            StrengthMultiplier = 3f
        };

        public bool DoMeshedTilesReduceSunlight => MeshTile.LightAbsorptionFactor > 0f || GasPermeableMembrane.LightAbsorptionFactor > 0f;

        [JsonObject(MemberSerialization.OptOut)]
        public class GenericOptions
        {
            [Option][Limit(-500, 500)] public int? Decor { get; set; }
            [Option][Limit(0, 20)] public int? DecorRadius { get; set; }
            [Option][Limit(0f, 20f)] public float? StrengthMultiplier { get; set; }
            [Option][Limit(0f, 20f)] public float? MovementSpeed { get; set; }
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class CarpetTileOptions : GenericOptions
        {
            [Option][Limit(0f, 1000f)] public float? CombustTemp { get; set; }
            [Option][Limit(0, 10)] public int? ReedFiberCount { get; set; }
            [Option] public bool IsNotWall { get; set; }
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class PermeableTileOptions : GenericOptions
        {
            [Option][Limit(0f, 1f)] public float? LightAbsorptionFactor { get; set; }
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class GlassTileOptions : GenericOptions
        {
            [Option][Limit(0f, 1f)] public float? DiamondLightAbsorptionFactor { get; set; }
            [Option][Limit(0f, 1f)] public float? GlassLightAbsorptionFactor { get; set; }
        }
    }
}
