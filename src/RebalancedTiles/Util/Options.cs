using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

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
        public CarpetTileOptions CarpetTile { get; set; } = new();
        [Option("STRINGS.BUILDINGS.PREFABS.GLASSTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.GLASSTILE.NAME")]
        public GlassTileOptions GlassTile { get; set; } = new();
        [Option("STRINGS.BUILDINGS.PREFABS.METALTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.METALTILE.NAME")]
        public GenericOptions MetalTile { get; set; } = new();
        [Option("STRINGS.BUILDINGS.PREFABS.MESHTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.MESHTILE.NAME")]
        public PermeableTileOptions MeshTile { get; set; } = new();
        [Option("STRINGS.BUILDINGS.PREFABS.GASPERMEABLEMEMBRANE.NAME", null, "STRINGS.BUILDINGS.PREFABS.GASPERMEABLEMEMBRANE.NAME")]
        public PermeableTileOptions GasPermeableMembrane { get; set; } = new();
        [Option("STRINGS.BUILDINGS.PREFABS.PLASTICTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.PLASTICTILE.NAME")]
        public GenericOptions InsulationTile { get; set; } = new();
        [Option("STRINGS.BUILDINGS.PREFABS.INSULATIONTILE.NAME", null, "STRINGS.BUILDINGS.PREFABS.INSULATIONTILE.NAME")]
        public GenericOptions PlasticTile { get; set; } = new();


        public bool DoMeshedTilesReduceSunlight => MeshTile.LightAbsorptionFactor > 0f || GasPermeableMembrane.LightAbsorptionFactor > 0f;

        [JsonObject(MemberSerialization.OptOut)]
        public class GenericOptions
        {
            [Option] public int? Decor { get; set; }
            [Option] public int? DecorRadius { get; set; }
            [Option] public float? StrengthMultiplier { get; set; }
            [Option] public float? MovementSpeed { get; set; }
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class CarpetTileOptions : GenericOptions
        {
            [Option] public float? CombustTemp { get; set; }
            [Option] public int? ReedFiberCount { get; set; }
            [Option] public bool IsNotWall { get; set; }
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class PermeableTileOptions : GenericOptions
        {
            [Option] public float? LightAbsorptionFactor { get; set; }
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class GlassTileOptions : GenericOptions
        {
            [Option] public float? DiamondLightAbsorptionFactor { get; set; }
            [Option] public float? GlassLightAbsorptionFactor { get; set; }
        }
    }
}
