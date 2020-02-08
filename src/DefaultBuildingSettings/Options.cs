using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;

namespace DefaultBuildingSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    [PeterHan.PLib.Options.RestartRequired]
    public class Options : BaseOptions<Options>
    {
        [Option("Vacancy Only", "If checked, atmo and jet suit checkpoint are set to Vacancy Only by default.")]
        [JsonProperty]
        public bool VacancyOnly { get; set; }

        [Option("Sweep Only", "If checked, storage receptacles are set to sweep only by default.")]
        [JsonProperty]
        public bool SweepOnly { get; set; }

        [Option("Auto Repair Off", "If checked, buildings will not allow repairs by default.")]
        [JsonProperty]
        public bool AutoRepairOff { get; set; }

        [Option("Open Doors", "If checked, Doors that allow airflow will immediately be opened after being built.")]
        [JsonProperty]
        public bool OpenDoors { get; set; }

        public Options()
        {
            VacancyOnly = true;
            SweepOnly = true;
            AutoRepairOff = false;
            OpenDoors = true;
        }

        public static void OnLoad()
        {
            Load();
        }
    }
}
