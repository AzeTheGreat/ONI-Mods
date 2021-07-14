using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace CleanHUD
{
    // Romen gets my full credit for locating this class and initial breakdown of the shader.
    // https://github.com/romen-h/ONI-Mods
    [HarmonyPatch(typeof(LightBufferCompositor), nameof(LightBufferCompositor.Start))]
    class ModifyBoundsWarning
    {
        static void Postfix(LightBufferCompositor __instance)
        {
            var origTex = Assets.instance.invalidAreaTex;

            var opacity = Options.Opts.InvalidAreaOpacity;
            Func<float, float> converter;
            if (Options.Opts.IsSolidInvalidArea)
                converter = x => opacity;
            else
                converter = x => Mathf.Min(x, opacity);

            var pixels = GetPixelsForced(origTex)
                .Select(x => new Color(x.r, x.g, x.b, converter(x.a)))
                .ToArray();

            var newTex = new Texture2D(origTex.width, origTex.height);
            newTex.SetPixels(pixels);
            newTex.Apply();
            __instance.material.SetTexture("_InvalidTex", newTex);
        }

        // GetPixels even if texture "can not be accessed from scripts".
        private static Color[] GetPixelsForced(Texture2D tex)
        {
            var renderTex = RenderTexture.GetTemporary(tex.width, tex.height);
            Graphics.Blit(tex, renderTex);
            
            RenderTexture.active = renderTex;
            var newTex = new Texture2D(tex.width, tex.height);
            newTex.ReadPixels(new(0, 0, tex.width, tex.height), 0, 0);

            RenderTexture.ReleaseTemporary(renderTex);
            RenderTexture.active = null;

            return newTex.GetPixels();
        }
    }

    // The shader doesn't clamp the input mouse position, it just continues to increase opacity as the cursor moves up.
    // This manually clamps the cursor world position and updates the shader before it renders.
    [HarmonyPatch(typeof(LightBufferCompositor), nameof(LightBufferCompositor.OnRenderImage))]
    class ClampWarningOpacity
    {
        static void Prefix(LightBufferCompositor __instance)
        {
            var worldCursorPos = Shader.GetGlobalVector("_WorldCursorPos");
            var topBorder = Shader.GetGlobalFloat(PropertyTextures.instance.TopBorderHeightID);

            var world = ClusterManager.Instance.activeWorld;
            // Bounds work in cellXY, not position, so adding a cell yields the correct maximum position.
            var clampedHeight = Mathf.Clamp(worldCursorPos.y, world.minimumBounds.y, world.maximumBounds.y + Grid.CellSizeInMeters - topBorder);
            // Center width is used to prevent the warning showing when the cursor moves too far horizontally.
            var centerWidth = world.WorldOffset.x + ((world.Width - 1) / 2);

            // This is a global property, but setting it only affects the instance it's set on.
            __instance.material.SetVector("_WorldCursorPos", new Vector4(centerWidth, clampedHeight));
        }
    }
}
