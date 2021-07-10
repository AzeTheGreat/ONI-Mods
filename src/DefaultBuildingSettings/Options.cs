using AzeLib;
using PeterHan.PLib.Options;

namespace DefaultBuildingSettings
{
    [PeterHan.PLib.Options.RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option]
        public bool VacancyOnly { get; set; }

        [Option]
        public bool SweepOnly { get; set; }

        [Option]
        public bool AutoRepairOff { get; set; }

        [Option]
        public bool OpenDoors { get; set; }

        [Option]
        public bool SwitchesOff { get; set; }

        [Option]
        [Limit(0, 100)]
        public int SmartBatteryActivateValue { get; set; }
        [Option]
        [Limit(0, 100)]
        public int SmartBatteryDeactivateValue { get; set; }

        [Option]
        [Limit(0, 100)]
        public int ReservoirActivateValue { get; set; }
        [Option]
        [Limit(0, 100)]
        public int ReservoirDeactivateValue { get; set; }

        [Option]
        [Limit(0, 100)]
        public int DeliverGenValue { get; set; }
        [Option]
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
