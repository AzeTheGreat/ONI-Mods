using AzeLib;
using PeterHan.PLib.Options;
using static DefaultBuildingSettings.STRINGS.DEFAULTBUILDINGSETTINGS.OPTIONS;

namespace DefaultBuildingSettings
{
    [PeterHan.PLib.Options.RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option(VACANCYONLY.nameKey, VACANCYONLY.tooltipKey)]
        public bool VacancyOnly { get; set; }

        [Option(SWEEPONLY.nameKey, SWEEPONLY.tooltipKey)]
        public bool SweepOnly { get; set; }

        [Option(AUTOREPAIROFF.nameKey, AUTOREPAIROFF.tooltipKey)]
        public bool AutoRepairOff { get; set; }

        [Option(OPENDOOR.nameKey, OPENDOOR.tooltipKey)]
        public bool OpenDoors { get; set; }

        [Option(SWITCHESOFF.nameKey, SWITCHESOFF.tooltipKey)]
        public bool SwitchesOff { get; set; }

        [Option(ACTIVATIONRANGE.ACTIVATEVALUE.nameKey, ACTIVATIONRANGE.ACTIVATEVALUE.tooltipKey, ACTIVATIONRANGE.batteryCatKey)]
        [Limit(0, 100)]
        public int SmartBatteryActivateValue { get; set; }
        [Option(ACTIVATIONRANGE.DEACTIVATEVALUE.nameKey, ACTIVATIONRANGE.DEACTIVATEVALUE.tooltipKey, ACTIVATIONRANGE.batteryCatKey)]
        [Limit(0, 100)]
        public int SmartBatteryDeactivateValue { get; set; }

        [Option(ACTIVATIONRANGE.ACTIVATEVALUE.nameKey, ACTIVATIONRANGE.ACTIVATEVALUE.tooltipKey, ACTIVATIONRANGE.reservoirCatKey)]
        [Limit(0, 100)]
        public int ReservoirActivateValue { get; set; }
        [Option(ACTIVATIONRANGE.DEACTIVATEVALUE.nameKey, ACTIVATIONRANGE.DEACTIVATEVALUE.tooltipKey, ACTIVATIONRANGE.reservoirCatKey)]
        [Limit(0, 100)]
        public int ReservoirDeactivateValue { get; set; }

        [Option(GENERATORS.DELIVERGENVALUE.nameKey, GENERATORS.DELIVERGENVALUE.tooltipKey, GENERATORS.categoryKey)]
        [Limit(0, 100)]
        public int DeliverGenValue { get; set; }
        [Option(GENERATORS.MANUALGENVALUE.nameKey, GENERATORS.MANUALGENVALUE.tooltipKey, GENERATORS.categoryKey)]
        [Limit(0, 100)]
        public int ManualGenValue { get; set; }

        public Options()
        {
            VacancyOnly = true;
            SweepOnly = true;
            AutoRepairOff = false;
            OpenDoors = true;
            SwitchesOff = true;

            SmartBatteryActivateValue = 5;
            SmartBatteryDeactivateValue = 95;

            ReservoirActivateValue = 5;
            ReservoirDeactivateValue = 95;

            DeliverGenValue = 25;
            ManualGenValue = 25;
        }
    }
}
