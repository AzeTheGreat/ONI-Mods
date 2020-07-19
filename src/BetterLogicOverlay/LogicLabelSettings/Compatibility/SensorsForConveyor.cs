using System.Linq;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class SolidElementSetting : LogicLabelSetting
    {
        [MyCmpGet] private TreeFilterable treeFilterable;

        public override string GetSetting()
        {
            Tag tag = treeFilterable.AcceptedTags.FirstOrDefault();

            string additionalTags = string.Empty;
            if (treeFilterable.AcceptedTags.Count > 1)
                additionalTags = " +" + (treeFilterable.AcceptedTags.Count - 1);

            return tag.GetAbbreviation() + additionalTags;
        }
    }
}
