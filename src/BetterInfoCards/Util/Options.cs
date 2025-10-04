using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using UnityEngine;

namespace BetterInfoCards
{
    [RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option] [Limit(0, 100)] public int InfoCardOpacity { get; set; }
        [Option] public Color32 InfoCardBackgroundColor { get; set; }

        [JsonProperty("InfoCardBackgroundRed", NullValueHandling = NullValueHandling.Ignore)]
        public int? LegacyInfoCardBackgroundRed { get; set; }

        [JsonProperty("InfoCardBackgroundGreen", NullValueHandling = NullValueHandling.Ignore)]
        public int? LegacyInfoCardBackgroundGreen { get; set; }

        [JsonProperty("InfoCardBackgroundBlue", NullValueHandling = NullValueHandling.Ignore)]
        public int? LegacyInfoCardBackgroundBlue { get; set; }
        [Option] public bool HideElementCategories { get; set; }
        [Option] public bool UseBaseSelection { get; set; }
        [Option] public bool ForceFirstSelectionToHover { get; set; }
        [Option] public float TemperatureBandWidth { get; set; }
        [Option] public CardSize InfoCardSize { get; set; }

        public Options()
        {
            InfoCardOpacity = 80;
            InfoCardBackgroundColor = new Color32(73, 79, 96, byte.MaxValue);
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

        protected override bool ValidateSettings()
        {
            var valid = base.ValidateSettings();
            var migrated = false;

            if (LegacyInfoCardBackgroundRed.HasValue || LegacyInfoCardBackgroundGreen.HasValue || LegacyInfoCardBackgroundBlue.HasValue)
            {
                var red = (byte)Mathf.Clamp(LegacyInfoCardBackgroundRed ?? InfoCardBackgroundColor.r, 0, 255);
                var green = (byte)Mathf.Clamp(LegacyInfoCardBackgroundGreen ?? InfoCardBackgroundColor.g, 0, 255);
                var blue = (byte)Mathf.Clamp(LegacyInfoCardBackgroundBlue ?? InfoCardBackgroundColor.b, 0, 255);

                var alpha = InfoCardBackgroundColor.a == 0 ? byte.MaxValue : InfoCardBackgroundColor.a;
                InfoCardBackgroundColor = new Color32(red, green, blue, alpha);

                LegacyInfoCardBackgroundRed = null;
                LegacyInfoCardBackgroundGreen = null;
                LegacyInfoCardBackgroundBlue = null;

                migrated = true;
            }

            return valid && !migrated;
        }
    }
}
