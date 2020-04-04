using AzeLib;
using Harmony;
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
        public enum CompactMode
        {
            [Option("Default", "The game's default sizing")]
            Default,
            [Option("Small", "Recommended: small but easily readbale.")]
            Small,
            [Option("Tiny", "Hard to read, but very compact.")]
            Tiny,
            [Option("Custom", "Use values defined in the config file.  Defaults to Small values.")]
            Custom
        }
        
        public CardSize InfoCardSize
        {
            get
            {
                switch (Compactness)
                {
                    case CompactMode.Default:
                        return defaultCardSize;
                    case CompactMode.Small:
                        return smallCardSize;
                    case CompactMode.Tiny:
                        return tinyCardSize;
                    case CompactMode.Custom:
                        return CustomCardSize;
                    default:
                        throw new System.Exception("Card Size Enum is messed up.");
                }
            }
        }

        [JsonProperty] private CardSize CustomCardSize { get; set; }

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

        [Option("Info Card Opacity", "Game default is 0.9.")]
        [JsonProperty] public float InfoCardOpacity { get; set; }

        public Options()
        {
            Compactness = CompactMode.Small;
            InfoCardOpacity = 0.8f;

            CustomCardSize = new CardSize
            {
                fontSizeDecrease = smallCardSize.fontSizeDecrease,
                minHeight = smallCardSize.minHeight,
                maxImageSize = smallCardSize.maxImageSize,
                ShadowBarBorder = smallCardSize.ShadowBarBorder
            };
        }

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
