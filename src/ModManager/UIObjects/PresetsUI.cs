using PeterHan.PLib.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModManager
{
    public class PresetsUI : UISource
    {
        public bool IsRenaming { get; set; }

        protected override IUIComponent GetUIComponent()
        {
            var renameButton = new PButton()
            {
                Text = "I",
                ToolTip = "Rename this preset.",
                OnClick = go => AExecuteEvents.ExecuteOnEntireHierarchy<IRenamePresetHandler>(go, x => x.OnRenamePreset())
            };

            var addButton = new PButton()
            {
                Text = "+",
                ToolTip = "Add a new preset.",
                OnClick = go => AExecuteEvents.ExecuteOnEntireHierarchy<IAddPresetHandler>(go, x => x.OnAddPreset())
            };

            var delButton = new PButton()
            {
                Text = "-",
                ToolTip = "Remove this preset.",
                OnClick = go => AExecuteEvents.ExecuteOnEntireHierarchy<IDelPresetHandler>(go, x => x.OnDelPreset())
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

        public interface IAddPresetHandler : IEventSystemHandler
        {
            void OnAddPreset();
        }

        public interface IDelPresetHandler : IEventSystemHandler
        {
            void OnDelPreset();
        }

        public interface IRenamePresetHandler : IEventSystemHandler
        {
            void OnRenamePreset();
        }
    }
}
