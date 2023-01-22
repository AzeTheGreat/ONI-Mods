using AzeLib.Attributes;

namespace BetterLogicOverlay.LogicSettingDisplay
{
    class UserControlledCapacitySetting : LogicLabelSetting
    {
        [MyIntGet] IUserControlledCapacity userControlledCapacity;

        public override string GetSetting() => userControlledCapacity.UserMaxCapacity + userControlledCapacity.CapacityUnits;
    }
}
