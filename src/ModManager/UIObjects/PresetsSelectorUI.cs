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
        public bool IsRenaming { get; set; }

        protected override IUIComponent GetUIComponent() => IsRenaming ? GetRenamer() : GetSelector();

        private IUIComponent GetSelector()
        {
            return new PComboBox<Preset>()
            {
                FlexSize = Vector2.one,
                InitialItem = GetCurPreset(),
                Content = Presets,
                OnOptionSelected = OnOptionSelected
            }
            .AddOnRealize(AddListeners);

            void AddListeners(GameObject go)
            {
                var newPre = go.AddComponent<AddPresetListener>();
                var delPre = go.AddComponent<DelPresetListener>();
                var renamePre = go.AddComponent<RenamePresetListener>();
                newPre.Instance = delPre.Instance = renamePre.Instance = this;
            }
        }

        private IUIComponent GetRenamer()
        {
            return new PTextField()
            {
                FlexSize = Vector2.one,
                Text = GetCurPreset()?.Name,
                OnTextChanged = OnTextChanged
            };

            void OnTextChanged(GameObject _, string text)
            {
                IsRenaming = false;
                if (GetCurPreset() is var preset)
                    preset.Name = text;

                RebuildGO();
            }
        }

        private Preset GetCurPreset() => Presets.ElementAtOrDefault(ActivePresetIdx);

        private void OnOptionSelected(GameObject _, Preset preset)
        {
            ActivePresetIdx = Presets.IndexOf(preset);
        }

        public class Preset : IListableOption
        {
            public string Name { get; set; }

            public string GetProperName() => Name;
        }

        private class AddPresetListener : MonoBehaviour, PresetsUI.IAddPresetHandler
        {
            public PresetsSelectorUI Instance { get; set; }

            public void OnAddPreset()
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

        private class DelPresetListener : MonoBehaviour, PresetsUI.IDelPresetHandler
        {
            public PresetsSelectorUI Instance { get; set; }

            public void OnDelPreset()
            {
                Instance.Presets.Remove(
                    Instance.Presets.ElementAtOrDefault(Instance.ActivePresetIdx));
                Instance.ActivePresetIdx = 0;

                Instance.RebuildGO();
            }
        }

        private class RenamePresetListener : MonoBehaviour, PresetsUI.IRenamePresetHandler
        {
            public PresetsSelectorUI Instance { get; set; }

            public void OnRenamePreset()
            {
                Instance.IsRenaming = true;

                Instance.RebuildGO();
            }
        }
    }
}
