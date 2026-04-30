using AzeLib;

namespace BuildMenuSearchHotkey;

public class OPTIONS : AStrings<OPTIONS>
{
    public class PREFEREXACTNAMEMATCH
    {
        public static LocString NAME = "Prefer Exact Name Match";
        public static LocString TOOLTIP = "When pressing Enter, prefer a building whose name exactly matches the search query (case-insensitive) over the fuzzy-ranked top result. Disable to always pick the top fuzzy result.";
    }
}
