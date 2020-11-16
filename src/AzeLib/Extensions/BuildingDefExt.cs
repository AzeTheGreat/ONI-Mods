namespace AzeLib.Extensions
{
    public static class BuildingDefExt
    {
        public static string GetRawName(this BuildingDef def) => STRINGS.UI.StripLinkFormatting(def.Name);
    }
}
