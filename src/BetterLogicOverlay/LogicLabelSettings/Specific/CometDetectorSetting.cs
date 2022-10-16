using STRINGS;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class CometDetectorSetting : LogicLabelSetting
    {
        [MySmiGet] private CometDetector.Instance detector;

        public override string GetSetting() => SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(detector.GetTargetCraft())?.GetRocketName() 
            ?? UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS;
    }

    class ClusterCometDetectorSetting: LogicLabelSetting
    {
        [MySmiGet] private ClusterCometDetector.Instance detector;

        public override string GetSetting() => detector.GetDetectorState() switch
        {
            ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower => UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS,
            ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject => UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.DUPEMADE,
            ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket => detector.GetClustercraftTarget()?.GetProperName(),
            _ => string.Empty,
        };
    }
}
