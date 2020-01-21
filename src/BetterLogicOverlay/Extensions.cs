using UnityEngine;

namespace BetterLogicOverlay
{
    static class Extensions
    {
        public static string GetAbbreviation(this Element element)
        {
            string abbreviation = ElementAbbreviation.abbreviations[element.id];

            if (abbreviation == string.Empty || !Options.Opts.isTranslated)
                abbreviation = element.name;
                
            return abbreviation;
        }
    }
}
