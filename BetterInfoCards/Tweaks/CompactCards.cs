using Harmony;
using System;
using UnityEngine;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new Type[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
    class CompactCards
    {
        private static readonly int fontSizeDecrease = Options.Opts.InfoCardSize.fontSizeDecrease;

        static bool Prepare() => Options.Opts.Compactness != Options.CompactMode.Default;

        static void Prefix(ref TextStyleSetting style)
        {
            if (style != null)
                style.fontSize -= fontSizeDecrease;
        }

        static void Postfix(ref TextStyleSetting style)
        {
            if (style != null)
                style.fontSize += fontSizeDecrease;
        }
    }

    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.NewLine))]
    class CompactCards2
    {
        private static readonly int minHeight = Options.Opts.InfoCardSize.minHeight;

        static bool Prepare() => Options.Opts.Compactness != Options.CompactMode.Default;

        static void Prefix(ref int min_height)
        {
            min_height = minHeight;
        }
    }

    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int)})]
    class CompactCards4
    {
        private static readonly int maxImageSize = Options.Opts.InfoCardSize.maxImageSize;

        static bool Prepare() => Options.Opts.Compactness != Options.CompactMode.Default;

        static void Prefix(ref int image_size)
        {
            if (image_size > 16)
                image_size = maxImageSize;
        }
    }

    [HarmonyPatch(typeof(HoverTextDrawer), MethodType.Constructor, new Type[] { typeof(HoverTextDrawer.Skin), typeof(RectTransform)})]
    class CompactCards3
    {
        private static readonly Vector2 border = Options.Opts.InfoCardSize.shadowBarBorder;
        private static readonly float opacity = Options.Opts.InfoCardOpacity;

        static void Prefix(ref HoverTextDrawer.Skin skin)
        {
            if(Options.Opts.Compactness != Options.CompactMode.Default)
                skin.shadowBarBorder = border;
            skin.shadowImageColor.a = opacity;
        }
    }
}
