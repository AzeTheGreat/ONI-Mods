using PeterHan.PLib.UI;
using UnityEngine;

namespace ModManager
{
    public class PresetsUI : IUISource
    {
        public IUIComponent GetUIComponent()
        {
            var textField = new PTextField()
            {
                Text = "Preset name",
                FlexSize = Vector2.one
            };

            var dropButton = new PButton()
            {
                Text = "v"
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
            .AddChild(textField)
            .AddChild(dropButton)
            .AddChild(addButton)
            .AddChild(delButton);
        }
    }
}
