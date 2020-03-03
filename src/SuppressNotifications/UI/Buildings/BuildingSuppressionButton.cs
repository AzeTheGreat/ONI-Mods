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
            return base.AreSuppressable() || (buildingHP.NeedsRepairs && !buildingHealthSuppressedComp.HideDmgBar);
        }

        internal override bool AreSuppressed()
        {
            return base.AreSuppressed() || buildingHealthSuppressedComp.HideDmgBar;
        }

        internal override string GetSuppressableString()
        {
            string suppressable = base.GetSuppressableString();

            if (buildingHP.NeedsRepairs && !buildingHealthSuppressedComp.HideDmgBar)
                suppressable += "Damage Bar";
            return suppressable;
        }

        internal override string GetSuppressedString()
        {
            string suppressed = base.GetSuppressedString();

            if(buildingHealthSuppressedComp.HideDmgBar)
                suppressed += "Damage Bar";
            return suppressed;
        }

        internal override void OnClearClick()
        {
            base.OnClearClick();
            buildingHealthSuppressedComp.SetDamageBar(false);
        }

        internal override void OnSuppressClick()
        {
            base.OnSuppressClick();
            if (buildingHP.NeedsRepairs)
                buildingHealthSuppressedComp.SetDamageBar(true);
        }
    }
}
