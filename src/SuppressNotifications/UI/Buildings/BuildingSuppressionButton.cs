namespace SuppressNotifications
{
    class BuildingSuppressionButton : SuppressionButton
    {
        [MyCmpAdd]
        private CopyBuildingSettings copyBuildingSettings;
        [MyCmpAdd]
        private BuildingHealthSuppressedComp buildingHealthSuppressedComp;
        [MyCmpGet]
        private BuildingHP buildingHP;

        internal override bool AreSuppressable()
        {
            return base.AreSuppressable() || (buildingHP.NeedsRepairs && !buildingHealthSuppressedComp.hideDmgBar);
        }

        internal override bool AreSuppressed()
        {
            return base.AreSuppressed() || buildingHealthSuppressedComp.hideDmgBar;
        }

        internal override string GetSuppressableString()
        {
            string suppressable = base.GetSuppressableString();

            if (buildingHP.NeedsRepairs && !buildingHealthSuppressedComp.hideDmgBar)
                suppressable += "Damage Bar";
            return suppressable;
        }

        internal override string GetSuppressedString()
        {
            string suppressed = base.GetSuppressedString();

            if(buildingHealthSuppressedComp.hideDmgBar)
                suppressed += "Damage Bar";
            return suppressed;
        }

        internal override void OnClearClick()
        {
            base.OnClearClick();
            buildingHealthSuppressedComp.hideDmgBar = false;
        }

        internal override void OnSuppressClick()
        {
            base.OnSuppressClick();
            if (buildingHP.NeedsRepairs)
                buildingHealthSuppressedComp.hideDmgBar = true;
        }
    }
}
