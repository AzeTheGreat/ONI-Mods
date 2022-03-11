using HarmonyLib;
using System;
using UnityEngine;

namespace BetterInfoCards
{
    // Using patches seems unintuitive here - the calls are being intercepted, why not just modify the values from where they're manually invoked?
    // The issue is that not all calls are intercepted.  For example: EndShadowBar has a NewLine call in it, and that inner call isn't.
    // By patching the original method, it is ensured that the tweaks will always apply.
    // To help prevent unnecessary calls from being made, tweaks should be skippable by false prefixes (use ref parameters).
    public static class CardTweaker
    {
        static bool ShouldTweak => Options.Opts.InfoCardSize.ShouldOverride;

        [HarmonyPatch(typeof(HoverTextDrawer), MethodType.Constructor, new[] { typeof(HoverTextDrawer.Skin), typeof(RectTransform) })]
        class TweakShadowBarPrefab
        {
            // It is unclear where this magic number "+2" came from.
            private static readonly Vector2 border = new(Options.Opts.InfoCardSize.YPadding + 2, Options.Opts.InfoCardSize.YPadding);
            private static readonly float opacity = Options.Opts.InfoCardOpacity;

            static void Prefix(ref HoverTextDrawer.Skin skin)
            {
                if (ShouldTweak)
                    skin.shadowBarBorder = border;

                var c = skin.shadowBarWidget.color;
                skin.shadowBarWidget.color = new Color(c.r, c.g, c.b, opacity);
            }
        }

        // There's no easy way to modify this in setup.
        // HoverTextConfigurations are spawned multiple times, break reference equality, yet preserve values.
        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
        class TweakFontSize 
        {
            private static readonly int fontSizeChange = Options.Opts.InfoCardSize.FontSizeChange;
            static bool Prepare() => ShouldTweak;

            // Because this prefix can be skipped, state must be used to tell it when to undo the size change.
            static void Prefix(ref TextStyleSetting style, out bool __state)
            {
                __state = false;
                if (style)
                {
                    style.fontSize += fontSizeChange;
                    __state = true;
                } 
            }

            // Unfortunately, this will be called extraneously: once for each intercepted call, once for each real call.
            static void Postfix(ref TextStyleSetting style, bool __state)
            {
                if (__state)
                    style.fontSize -= fontSizeChange;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.NewLine))]
        class TweakNewLineHeight
        {
            private static readonly int minHeight = Options.Opts.InfoCardSize.MinHeight;
            static bool Prepare() => ShouldTweak;

            static void Prefix(ref int min_height)
            {
                min_height = minHeight;
            }
        }

        [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) })]
        class TweakIconSize
        {
            private static readonly int maxImageSize = Options.Opts.InfoCardSize.MaxImageSize;
            static bool Prepare() => ShouldTweak;

            static void Prefix(ref int image_size) => image_size = Math.Min(image_size, maxImageSize);
        }
    }
}
