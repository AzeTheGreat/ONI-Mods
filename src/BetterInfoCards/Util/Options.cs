using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace BetterInfoCards
{
    [RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option] [Limit(0, 100)] public int InfoCardOpacity { get; set; }
        [Option] [Limit(0, 255)] public byte InfoCardBackgroundRed { get; set; }
        [Option] [Limit(0, 255)] public byte InfoCardBackgroundGreen { get; set; }
        [Option] [Limit(0, 255)] public byte InfoCardBackgroundBlue { get; set; }
        [Option] public bool HideElementCategories { get; set; }
        [Option] public bool UseBaseSelection { get; set; }
        [Option] public bool ForceFirstSelectionToHover { get; set; }
        [Option] public float TemperatureBandWidth { get; set; }
        [Option] public CardSize InfoCardSize { get; set; }

        public Options()
        {
            InfoCardOpacity = 80;
            InfoCardBackgroundRed = 73;
            InfoCardBackgroundGreen = 79;
            InfoCardBackgroundBlue = 96;
            HideElementCategories = false;
            UseBaseSelection = false;
            ForceFirstSelectionToHover = true;
            TemperatureBandWidth = 10f;

            InfoCardSize = new()
            {
                ShouldOverride = true,
                FontSizeChange = -2,
                LineSpacing = 3,
                IconSizeChange = -3,
                YPadding = 6
            };
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class CardSize
        {
            [Option] public bool ShouldOverride { get; set; }
            [Option] [Limit(-5, 5)] public int FontSizeChange { get; set; }
            [Option] [Limit(0, 20)] public int LineSpacing { get; set; }
            [Option] [Limit(-10, 10)] public int IconSizeChange { get; set; }
            [Option] [Limit(1, 20)] public int YPadding { get; set; }
        }
    }
}
