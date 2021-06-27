using HarmonyLib;
using System;
using UnityEngine;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new Type[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
    class FontSizeDecrease_Patch
    {
        private static readonly int fontSizeDecrease = Options.Opts.InfoCardSize.fontSizeDecrease;

        static bool Prepare() => Options.Opts.Compactness != Options.CompactMode.Default;

        static void Prefix(ref TextStyleSetting style)
        {
            if (style)
                style.fontSize -= fontSizeDecrease;
        }

        static void Postfix(ref TextStyleSetting style)
        {
            if (style)
                style.fontSize += fontSizeDecrease;
        }
    }

    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.NewLine))]
    class NewLineHeight_Patch
    {
        private static readonly int minHeight = Options.Opts.InfoCardSize.minHeight;

        static bool Prepare() => Options.Opts.Compactness != Options.CompactMode.Default;

        static void Prefix(ref int min_height)
        {
            min_height = minHeight;
        }
    }

    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int)})]
    class MaxImageSize_Patch
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
    class Opacity_Patch
    {
        private static readonly Vector2 border = Options.Opts.InfoCardSize.ShadowBarBorder;
        private static readonly float opacity = Options.Opts.InfoCardOpacity;

        static void Prefix(ref HoverTextDrawer.Skin skin)
        {
            if(Options.Opts.Compactness != Options.CompactMode.Default)
                skin.shadowBarBorder = border;

            var c = skin.shadowBarWidget.color;
            skin.shadowBarWidget.color = new Color(c.r, c.g, c.b, opacity);
        }
    }
}
