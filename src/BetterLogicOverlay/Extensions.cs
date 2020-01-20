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

        public static string GetAbbreviation(this Element element)
        {
            string abbreviation = ElementAbbreviation.abbreviations[element.id];

            if (abbreviation == string.Empty || !Options.Opts.isTranslated)
                abbreviation = element.name;
                
            return abbreviation;
        }
    }
}
