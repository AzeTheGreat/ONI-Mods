namespace BetterLogicOverlay.LogicSettingDisplay
{
    class LogicElementSensorSetting : LogicLabelSetting
    {
        [MyCmpGet] LogicElementSensor logicElementSensor;

        public override string GetSetting() => ElementLoader.elements[logicElementSensor.desiredElementIdx].GetAbbreviation();
    }
}
