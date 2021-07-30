using PeterHan.PLib.UI;
using UnityEngine;
using UnityEngine.EventSystems;

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
                Text = "+",
                ToolTip = "Add a new preset.",
                OnClick = go => AExecuteEvents.ExecuteOnEntireHierarchy<IAddNewPresetTarget>(go, x => x.OnAddNewPreset())
            };

            var delButton = new PButton()
            {
                Text = "-",
                ToolTip = "Remove this preset.",
                OnClick = go => AExecuteEvents.ExecuteOnEntireHierarchy<IDelCurPresetTarget>(go, x => x.OnDelCurPreset())
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

        public interface IAddNewPresetTarget : IEventSystemHandler
        {
            void OnAddNewPreset();
        }

        public interface IDelCurPresetTarget : IEventSystemHandler
        {
            void OnDelCurPreset();
        }
    }
}
