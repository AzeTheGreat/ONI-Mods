using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace BetterInfoCards
{
    [RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option] public CardSize InfoCardSize { get; set; }
        [Option] public float InfoCardOpacity { get; set; }
        [Option] public float TemperatureBandWidth { get; set; }
        [Option] public bool UseBaseSelection { get; set; }
        [Option] public bool ForceFirstSelectionToHover { get; set; }
        [Option] public bool HideElementCategories { get; set; }

        public Options()
        {
            InfoCardSize = new()
            {
                ShouldOverride = true,
                FontSizeChange = -2,
                MinHeight = 16,
                MaxImageSize = 16,
                YPadding = 6
            };

            InfoCardOpacity = 0.8f;
            TemperatureBandWidth = 10f;
            UseBaseSelection = false;
            ForceFirstSelectionToHover = true;
            HideElementCategories = false;
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class CardSize
        {
            [Option] public bool ShouldOverride { get; set; }
            [Option] [Limit(-5, 5)] public int FontSizeChange { get; set; }
            [Option] [Limit(1, 20)] public int MinHeight { get; set; }
            [Option] [Limit(5, 20)] public int MaxImageSize { get; set; }
            [Option] [Limit(1, 20)] public int YPadding { get; set; }
        }
    }
}
