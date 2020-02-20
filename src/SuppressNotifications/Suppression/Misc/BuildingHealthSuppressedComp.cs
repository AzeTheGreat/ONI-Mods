using Harmony;
using KSerialization;
using UnityEngine;

namespace SuppressNotifications
{
    class BuildingHealthSuppressedComp : KMonoBehaviour, ISaveLoadable
    {
        [Serialize]
        private bool _hideDmgBar;
        public bool hideDmgBar
        {
            get => _hideDmgBar;
            set
            { 
                _hideDmgBar = value;
                ProgressBar?.gameObject.SetActive(!value);
            }
        }

        private Traverse progBarTrav;
        private ProgressBar ProgressBar { get => Traverse.Create(gameObject.GetComponent<BuildingHP>().GetSMI<BuildingHP.SMInstance>()).Field("progressBar").GetValue<ProgressBar>(); }

        protected override void OnPrefabInit()
        {
            // SMI is set in OnSpawn so this doesn't work...can't gaurantee script execution order either so hmmm....
            progBarTrav = Traverse.Create(gameObject.GetComponent<BuildingHP>().GetSMI<BuildingHP.SMInstance>()).Field("progressBar");
            Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
        }

        private void OnCopySettings(object data)
        {
            var comp = (data as GameObject).GetComponent<BuildingHealthSuppressedComp>();

            if (comp != null)
                hideDmgBar = comp.hideDmgBar;
        }

        private static readonly EventSystem.IntraObjectHandler<BuildingHealthSuppressedComp> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BuildingHealthSuppressedComp>(Handler);
        private static void Handler(BuildingHealthSuppressedComp comp, object data) => comp.OnCopySettings(data);
    }
}
