using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BetterLogicOverlay
{
    public static class LabelPrefab
    {
        // Can't just use localPos or anything because the UI object must be parented to the canvas.
        public static readonly Vector2f offset = new Vector2f(0f, -0.04f);

        public static LocText GetLabelPrefab()
        {
            // Create custom prefab, using the power label prefab as the base.
            var prefab = Object.Instantiate(OverlayScreen.Instance.powerLabelPrefab);
            prefab.name = "LogicSettingLabel";
            prefab.raycastTarget = false;

            Object.Destroy(prefab.GetComponent<ContentSizeFitter>());   // Remove fitter so that the rect doesn't expand (it needs to be limited to a cell).
            Object.Destroy(prefab.GetComponent<ToolTip>());             // Unnecessary for this label.
            Object.Destroy(prefab.transform.GetChild(0).gameObject);    // Remove child because a separate GO doesn't work well for limiting bounds.

            // Adjust text
            prefab.font = Localization.GetFont(Localization.GetDefaultFontName());      // Use default font instead of Graystroke (lower unit readability, degree sign offset text).
            prefab.alignment = TextAlignmentOptions.Bottom;
            prefab.fontSize += 1f;
            prefab.characterSpacing = -1f;
            prefab.lineSpacing = -10f;
            prefab.enableKerning = true;
            prefab.enableWordWrapping = true;

            prefab.UpdateMeshPadding();
            return prefab;
        }
    }
}
