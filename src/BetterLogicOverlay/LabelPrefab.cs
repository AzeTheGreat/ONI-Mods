using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BetterLogicOverlay
{
    public static class LabelPrefab
    {
        // Text is bounded vertically between two automation ports.
        // These bounds are implemented based on the tallest possible text.
        // Since these are rarely used for most labels, this tweaks the bounds for better appearance for common text.

        public const float boundsHeightDelta = 0.1f;                        // Increase the bound height to fit the desired number of lines.
        public static readonly Vector2f boundsYOffset = new(0f, -0.04f);    // Shift the position to visually anchor common text to the port better.

        public static LocText GetLabelPrefab()
        {
            // Create custom prefab, using the power label prefab as the base.
            var prefab = Object.Instantiate(OverlayScreen.Instance.powerLabelPrefab);
            prefab.name = "LogicSettingLabel";
            prefab.raycastTarget = false;

            Object.Destroy(prefab.GetComponent<ContentSizeFitter>());   // Remove fitter so that the rect doesn't expand (it needs to be limited to a cell).
            Object.Destroy(prefab.GetComponent<ToolTip>());             // Unnecessary for this label.
            Object.Destroy(prefab.transform.GetChild(0).gameObject);    // Remove child because a separate GO doesn't work well for limiting bounds.

            prefab.GetComponent<RectTransform>().pivot = new(0.5f, 0f); // Anchor on the bottom to make text alignment with the port easier.

            // Adjust text
            prefab.font = Localization.GetFont(Localization.GetDefaultFontName());      // Use default font instead of Graystroke (lower unit readability, degree sign offset text).
            prefab.alignment = TextAlignmentOptions.Bottom;
            prefab.fontSize += 1f;
            prefab.characterSpacing = -1f;
            prefab.lineSpacing = -10f;
            prefab.enableKerning = true;
            prefab.enableWordWrapping = true;
            prefab.overflowMode = TextOverflowModes.Truncate;

            prefab.UpdateMeshPadding();
            return prefab;
        }
    }
}
