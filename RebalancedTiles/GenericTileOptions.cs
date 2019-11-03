using Newtonsoft.Json;
using PeterHan.PLib;

namespace RebalancedTiles
{
	public partial class Options
	{
        [JsonProperty]
        public TileOptions Tile;
        public partial class TileOptions : GenericOptions { }
		[Option("Tile: Tweaked", "When true, Tile uses custom values set in the config.")]
		public bool _IsTileTweaked { get { return Tile.IsTweaked; } set { Tile.IsTweaked = value; } }

        [JsonProperty]
        public BunkerTileOptions BunkerTile;
        public partial class BunkerTileOptions : GenericOptions { }
		[Option("BunkerTile: Tweaked", "When true, BunkerTile uses custom values set in the config.")]
		public bool _IsBunkerTileTweaked { get { return BunkerTile.IsTweaked; } set { BunkerTile.IsTweaked = value; } }

        [JsonProperty]
        public CarpetTileOptions CarpetTile;
        public partial class CarpetTileOptions : GenericOptions { }
		[Option("CarpetTile: Tweaked", "When true, CarpetTile uses custom values set in the config.")]
		public bool _IsCarpetTileTweaked { get { return CarpetTile.IsTweaked; } set { CarpetTile.IsTweaked = value; } }

        [JsonProperty]
        public GasPermeableMembraneOptions GasPermeableMembrane;
        public partial class GasPermeableMembraneOptions : GenericOptions { }
		[Option("GasPermeableMembrane: Tweaked", "When true, GasPermeableMembrane uses custom values set in the config.")]
		public bool _IsGasPermeableMembraneTweaked { get { return GasPermeableMembrane.IsTweaked; } set { GasPermeableMembrane.IsTweaked = value; } }

        [JsonProperty]
        public GlassTileOptions GlassTile;
        public partial class GlassTileOptions : GenericOptions { }
		[Option("GlassTile: Tweaked", "When true, GlassTile uses custom values set in the config.")]
		public bool _IsGlassTileTweaked { get { return GlassTile.IsTweaked; } set { GlassTile.IsTweaked = value; } }

        [JsonProperty]
        public MeshTileOptions MeshTile;
        public partial class MeshTileOptions : GenericOptions { }
		[Option("MeshTile: Tweaked", "When true, MeshTile uses custom values set in the config.")]
		public bool _IsMeshTileTweaked { get { return MeshTile.IsTweaked; } set { MeshTile.IsTweaked = value; } }

        [JsonProperty]
        public MetalTileOptions MetalTile;
        public partial class MetalTileOptions : GenericOptions { }
		[Option("MetalTile: Tweaked", "When true, MetalTile uses custom values set in the config.")]
		public bool _IsMetalTileTweaked { get { return MetalTile.IsTweaked; } set { MetalTile.IsTweaked = value; } }

	}
}