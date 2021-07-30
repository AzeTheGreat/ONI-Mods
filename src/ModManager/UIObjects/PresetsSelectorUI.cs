using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModManager
{
    class PresetsSelectorUI : UISource
    {
        public List<Preset> Presets { get; set; } = new();

        protected override IUIComponent GetUIComponent()
        {
            return new PComboBox<Preset>()
            {
                FlexSize = Vector2.one,
                InitialItem = Presets.FirstOrDefault(),
                Content = Presets
            };
        }

        public class Preset : IListableOption
        {
            public string Name { get; set; }

            public string GetProperName() => Name;
        }
    }
}
