using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModManager
{
    class PresetsSelectorUI : UISource
    {
        public int ActivePresetIdx { get; set; }
        public List<Preset> Presets { get; set; } = new();

        protected override IUIComponent GetUIComponent()
        {
            return new PComboBox<Preset>()
            {
                FlexSize = Vector2.one,
                InitialItem = Presets.ElementAtOrDefault(ActivePresetIdx),
                Content = Presets,
                OnOptionSelected = OnOptionSelected
            }
            .AddOnRealize(AddPresetListener)
            .AddOnRealize(AddDelCurPresetListener);

            void AddPresetListener(GameObject go)
            {
                var target = go.AddComponent<AddNewPresetTarget>();
                target.Instance = this;
            }

            void AddDelCurPresetListener(GameObject go)
            {
                var target = go.AddComponent<DelCurPresetTarget>();
                target.Instance = this;
            }
        }

        private void OnOptionSelected(GameObject _, Preset preset)
        {
            ActivePresetIdx = Presets.IndexOf(preset);
        }

        public class Preset : IListableOption
        {
            public string Name { get; set; }

            public string GetProperName() => Name;
        }

        private class AddNewPresetTarget : MonoBehaviour, PresetsUI.IAddNewPresetTarget
        {
            public PresetsSelectorUI Instance { get; set; }

            public void OnAddNewPreset()
            {
                var count = Instance.Presets.Count;
                Instance.Presets.Add(new()
                {
                    Name = "Preset #" + (count + 1)
                });
                Instance.ActivePresetIdx = count;

                Instance.RebuildGO();
            }
        }

        private class DelCurPresetTarget : MonoBehaviour, PresetsUI.IDelCurPresetTarget
        {
            public PresetsSelectorUI Instance { get; set; }

            public void OnDelCurPreset()
            {
                Instance.Presets.Remove(
                    Instance.Presets.ElementAtOrDefault(Instance.ActivePresetIdx));
                Instance.ActivePresetIdx = 0;

                Instance.RebuildGO();
            }
        }
    }
}
