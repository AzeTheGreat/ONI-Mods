using Harmony;
using System;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{    
    public static class CardTweaker
    {
        static bool ShouldTweak => Options.Opts.Compactness != Options.CompactMode.Default;

        public static int GetNewLineHeight(int minHeight) => ShouldTweak ? Options.Opts.InfoCardSize.minHeight : minHeight;
        public static int GetNewIconSize(int imageSize) => ShouldTweak ? Math.Min(imageSize, Options.Opts.InfoCardSize.maxImageSize) : imageSize;

        [HarmonyPatch(typeof(HoverTextDrawer), MethodType.Constructor, new[] { typeof(HoverTextDrawer.Skin), typeof(RectTransform) })]
        class TweakShadowBarPrefab
        {
            private static readonly Vector2 border = Options.Opts.InfoCardSize.ShadowBarBorder;
            private static readonly float opacity = Options.Opts.InfoCardOpacity;

            static void Prefix(ref HoverTextDrawer.Skin skin)
            {
                if (ShouldTweak)
                    skin.shadowBarBorder = border;

                var c = skin.shadowBarWidget.color;
                skin.shadowBarWidget.color = new Color(c.r, c.g, c.b, opacity);
            }
        }

        [HarmonyPatch(typeof(HoverTextConfiguration), nameof(HoverTextConfiguration.OnSpawn))]
        class TweakFontSize
        {
            static bool Prepare() => ShouldTweak;

            static void Postfix(HoverTextConfiguration __instance)
            {
                var propertyStyles = new[] { __instance.Styles_Values };
                var pairs = new[] { __instance.Styles_BodyText, __instance.Styles_Instruction, __instance.Styles_Title, __instance.Styles_Warning }
                    .Concat(propertyStyles.SelectMany(x => new[] { x.Property, x.Property_Decimal, x.Property_Unit }));
                var styles = __instance.HoverTextStyleSettings.ToList()
                    .Concat(pairs.SelectMany(x => new[] { x.Standard, x.Selected }));

                foreach (var style in styles.Where(x => x != null).Distinct())
                    style.fontSize -= Options.Opts.InfoCardSize.fontSizeDecrease;
            }
        }
    }
}
