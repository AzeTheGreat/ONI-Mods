using AzeLib.Extensions;

namespace BetterLogicOverlay
{
    static class Extensions
    {
        public static string GetAbbreviation(this Element element)
        {
            if (element == null)
                return null;

            string abbreviation = ElementAbbreviation.abbreviations[element.id];

            if (abbreviation == string.Empty || !Options.Opts.isTranslated)
                abbreviation = element.name;
                
            return abbreviation;
        }

        public static string GetAbbreviation(this Tag tag)
        {
            if (tag == null)
                return STRINGS.ELEMENTS.VOID.NAME;

            Element element = ElementLoader.FindElementByHash((SimHashes)tag.GetHash());

            if (!(element.GetAbbreviation() is string abbreviation))
                abbreviation = STRINGS.UI.StripLinkFormatting(TagManager.GetProperName(tag)).Truncate(5);
            return abbreviation;
        }
    }
}
