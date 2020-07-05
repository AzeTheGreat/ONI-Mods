using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace NoVignette
{
    [HarmonyPatch(typeof(Vignette), "Awake")]
    public class TransparentDefaultVignette_Patch
    {
        static void Prefix(ref Image ___image) => ___image.color = Color.clear;
    }
}
