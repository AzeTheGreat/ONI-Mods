using Newtonsoft.Json;
using PeterHan.PLib;

namespace RebalancedTiles
{
	public partial class Options
	{
		[JsonProperty]
		public GenericOptions Tile;
		[Option("Tile: Tweaked", "When true, Tile uses custom values set in the config.")]
		public bool _IsTileTweaked { get { return Tile.IsTweaked; } set { Tile.IsTweaked = value; } }

		[JsonProperty]
		public GenericOptions BunkerTile;
		[Option("BunkerTile: Tweaked", "When true, BunkerTile uses custom values set in the config.")]
		public bool _IsBunkerTileTweaked { get { return BunkerTile.IsTweaked; } set { BunkerTile.IsTweaked = value; } }

		[JsonProperty]
		public GenericOptions GasPermeableMembrane;
		[Option("GasPermeableMembrane: Tweaked", "When true, GasPermeableMembrane uses custom values set in the config.")]
		public bool _IsGasPermeableMembraneTweaked { get { return GasPermeableMembrane.IsTweaked; } set { GasPermeableMembrane.IsTweaked = value; } }

		[JsonProperty]
		public GenericOptions GlassTile;
		[Option("GlassTile: Tweaked", "When true, GlassTile uses custom values set in the config.")]
		public bool _IsGlassTileTweaked { get { return GlassTile.IsTweaked; } set { GlassTile.IsTweaked = value; } }

		[JsonProperty]
		public GenericOptions MeshTile;
		[Option("MeshTile: Tweaked", "When true, MeshTile uses custom values set in the config.")]
		public bool _IsMeshTileTweaked { get { return MeshTile.IsTweaked; } set { MeshTile.IsTweaked = value; } }

		[JsonProperty]
		public GenericOptions MetalTile;
		[Option("MetalTile: Tweaked", "When true, MetalTile uses custom values set in the config.")]
		public bool _IsMetalTileTweaked { get { return MetalTile.IsTweaked; } set { MetalTile.IsTweaked = value; } }

	}
}