using UnityEngine;

namespace AzeLib.Extensions
{
    public static class ProgressBarExt
    {
        public static void ToggleVisibility(this ProgressBar pb, bool isVisible)
        {
            pb.GetComponent<CanvasGroup>().alpha = isVisible ? 0f : 1f;
        }
    }
}
