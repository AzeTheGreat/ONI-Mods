using AzeLib.Extensions;
using KSerialization;
using UnityEngine;

namespace SuppressNotifications
{
    class BuildingHealthSuppressedComp : KMonoBehaviour, ISaveLoadable
    {
        [Serialize] private bool _hideDMGBar;
        public bool HideDmgBar { get => _hideDMGBar; }

        private BuildingHP.SMInstance smi;

        private ProgressBar ProgressBar { get
            {
                // SMI is set in OnSpawn, so this can't be cached in OnPrefabInit or OnSpawn (can't gaurantee script execution order)
                if (smi == null)
                    smi = gameObject.GetComponent<BuildingHP>().GetSMI<BuildingHP.SMInstance>();
                return smi.progressBar;
            }
        }

        public override void OnPrefabInit()
        {
            Subscribe((int)GameHashes.CopySettings, (object data) => OnCopySettings(data));
        }

        public void SetDamageBar(bool shouldHide)
        {
            _hideDMGBar = shouldHide;

            if (ProgressBar)
                ProgressBar.ToggleVisibility(shouldHide);
        }

        private void OnCopySettings(object data)
        {
            var comp = (data as GameObject).GetComponent<BuildingHealthSuppressedComp>();
            if (comp != null)
                SetDamageBar(comp.HideDmgBar);
        }
    }
}
