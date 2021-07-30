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
                Content = Presets
            }
            .AddOnRealize(AddPresetListener);

            void AddPresetListener(GameObject go)
            {
                var target = go.AddComponent<AddNewPresetTarget>();
                target.Instance = this;
            }
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
    }
}
