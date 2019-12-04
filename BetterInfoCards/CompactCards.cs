using Harmony;
using System;
using UnityEngine;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawText), new Type[] { typeof(string), typeof(TextStyleSetting), typeof(Color), typeof(bool) })]
    class CompactCards
    {
        const int fontSizeDecrease = 2;

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
        static void Prefix(ref int min_height)
        {
            min_height = 16;
        }
    }

    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.DrawIcon), new Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int)})]
    class CompactCards4
    {
        const int maxImageSize = 16;

        static void Prefix(ref int image_size)
        {
            if (image_size > 16)
                image_size = maxImageSize;
        }
    }

    [HarmonyPatch(typeof(HoverTextDrawer), MethodType.Constructor, new Type[] { typeof(HoverTextDrawer.Skin), typeof(RectTransform)})]
    class CompactCards3
    {
        static void Prefix(ref HoverTextDrawer.Skin skin)
        {
            skin.shadowBarBorder = new Vector2(10f, 6f);
            skin.shadowImageColor.a = 0.8f;
        }
    }
}
