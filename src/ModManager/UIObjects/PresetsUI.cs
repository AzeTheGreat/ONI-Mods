using PeterHan.PLib.UI;
using UnityEngine;

namespace ModManager
{
    public class PresetsUI : UISource
    {
        protected override IUIComponent GetUIComponent()
        {
            var renameButton = new PButton()
            {
                Text = "I"
            };

            var addButton = new PButton()
            {
                Text = "+"
            };

            var delButton = new PButton()
            {
                Text = "-"
            };

            return new PPanel()
            {
                Direction = PanelDirection.Horizontal,
                Spacing = 2,
                Margin = new(2, 2, 2, 2),
                FlexSize = Vector2.one
            }
            .AddChild(new PresetsSelectorUI().CreateUIComponent())
            .AddChild(renameButton)
            .AddChild(addButton)
            .AddChild(delButton);
        }
    }
}
