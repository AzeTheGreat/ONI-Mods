using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;
using UnityEngine;

namespace BetterInfoCards
{
    [JsonObject(MemberSerialization.OptIn)]
    [PeterHan.PLib.Options.RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option("Info Card Compactness", "How compact the info cards should be.")]
        [JsonProperty] public CompactMode Compactness { get; set; }

        [Option("Info Card Opacity", "Game default is 0.9.")]
        [JsonProperty] public float InfoCardOpacity { get; set; }

        [Option("Temperature Band Width", "Info cards are grouped grouped by temperature if they are close enough, this sets the maximum temperature difference at which info cards will be grouped.")]
        [JsonProperty] public float TemperatureBandWidth { get; set; }

        [Option("Restricted Selection", "Only allow selecting items that would be selectable in the base game.")]
        [JsonProperty] public bool UseBaseSelection { get; set; }

        [Option("First Selection Hover", "Limit the first selection to the hovered item, without further restricting future selections.")]
        [JsonProperty] public bool ForceFirstSelectionToHover { get; set; }

        public Options()
        {
            Compactness = CompactMode.Small;
            InfoCardOpacity = 0.8f;
            TemperatureBandWidth = 10f;
            UseBaseSelection = false;
            ForceFirstSelectionToHover = true;

            CustomCardSize = new CardSize
            {
                fontSizeDecrease = smallCardSize.fontSizeDecrease,
                minHeight = smallCardSize.minHeight,
                maxImageSize = smallCardSize.maxImageSize,
                ShadowBarBorder = smallCardSize.ShadowBarBorder
            };
        }

        [JsonProperty] private CardSize CustomCardSize { get; set; }

        public enum CompactMode
        {
            [Option("Default", "The game's default sizing")] Default,
            [Option("Small", "Recommended: small but easily readable.")] Small,
            [Option("Tiny", "Hard to read, but very compact.")] Tiny,
            [Option("Custom", "Use values defined in the config file.  Defaults to Small values.")] Custom
        }

        public CardSize InfoCardSize => Compactness switch
        {
            CompactMode.Default => defaultCardSize,
            CompactMode.Small => smallCardSize,
            CompactMode.Tiny => tinyCardSize,
            CompactMode.Custom => CustomCardSize,
            _ => throw new System.Exception("Card Size Enum is messed up."),
        };

        private static readonly CardSize defaultCardSize = new CardSize();
        private static readonly CardSize smallCardSize = new CardSize
        {
            fontSizeDecrease = 2,
            minHeight = 16,
            maxImageSize = 16,
            ShadowBarBorder = new Vector2(10f, 6f)
        };
        private static readonly CardSize tinyCardSize = new CardSize
        {
            fontSizeDecrease = 3,
            minHeight = 14,
            maxImageSize = 12,
            ShadowBarBorder = new Vector2(5f, 3f)
        };

        [JsonObject(MemberSerialization.OptIn)]
        public class CardSize
        {
            public Vector2 ShadowBarBorder
            { 
                get => new Vector2(shadowBarBorder.x, shadowBarBorder.y);
                set => shadowBarBorder = (value.x, value.y); 
            }

            [JsonProperty] public int fontSizeDecrease = 0;
            [JsonProperty] public int minHeight = 0;
            [JsonProperty] public int maxImageSize = 0;
            [JsonProperty] private (float x, float y) shadowBarBorder = (0, 0);
        }
    }
}
