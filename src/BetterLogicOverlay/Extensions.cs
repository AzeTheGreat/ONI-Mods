using UnityEngine;

namespace BetterLogicOverlay
{
    static class Extensions
    {
        public static Vector3 InverseLocalScale(this RectTransform rectTransform)
        {
            Vector3 scale = rectTransform.localScale;
            return new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
        }
    }
}
