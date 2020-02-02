using AzeLib;
using Newtonsoft.Json;
using PeterHan.PLib;

namespace DefaultBuildingSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options : BaseOptions<Options>
    {
        [Option("Vacancy Only", "If checked, atmo and jet suit checkpoint are set to Vacancy Only by default.")]
        [JsonProperty]
        public bool VacancyOnly { get; set; }

        [Option("SweepOnly", "If checked, storage receptacles are set to sweep only by default.")]
        [JsonProperty]
        public bool SweepOnly { get; set; }

        public Options()
        {
            VacancyOnly = true;
            SweepOnly = true;
        }

        public static void OnLoad()
        {
            Load();
        }
    }
}
