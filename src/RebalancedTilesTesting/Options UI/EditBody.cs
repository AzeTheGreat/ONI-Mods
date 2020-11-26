using PeterHan.PLib;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RebalancedTilesTesting.OptionsUI
{
    public class EditBody : ConnectedUIComponent<OptionsScreen>
    {
        private static readonly RectOffset optionMargins = new(2, 2, 2, 2);

        private GameObject panel;
        private GameObject scrollPane;
        
        public EditBody(OptionsScreen options) : base(options) => options.editBody = this;

        public override IUIComponent GetUIComponent()
        {
            return new PScrollPane()
            {
                BackColor = PUITuning.Colors.BackgroundLight,
                Child = GetScrollPanel(default),
                ScrollHorizontal = false,
                ScrollVertical = true,
                FlexSize = Vector2.one,
                TrackSize = 8f
            }
            .AddOnRealize((GameObject realized) => scrollPane = realized);
        }

        public void SetChildren(BuildingDef def)
        {
            var childParent = panel.GetParent();
            Object.Destroy(panel);

            var options = Filter.GetOptionsForDef(def).ToList();
            GetScrollPanel(options).Build().SetParent(childParent);

            scrollPane.GetComponent<KScrollRect>().content = panel.rectTransform();
        }

        public void ResetToDefault(GameObject _)
        {

        }

        private IUIComponent GetScrollPanel(List<OptionsEntry> options)
        {
            var panel = new PGridPanel()
            {
                BackColor = Color.clear,
                Margin = new RectOffset(5, 5, 5, 5)
            }
            .AddColumn(new(0, 0.5f))
            .AddColumn(new(0, 0.5f))
            .AddOnRealize((GameObject realized) => this.panel = realized) as PGridPanel;

            if (options is null || options.Count < 1)
                return panel.AddRow(new());

            for (int i = 0; i < options.Count; i++)
            {
                var option = options[i];
                var label = new PLabel()
                {
                    Text = option.Title,
                    TextStyle = PUITuning.Fonts.TextDarkStyle
                };

                panel.AddRow(new());
                panel.AddChild(label, new(i, 0) { Alignment = TextAnchor.MiddleLeft, Margin = optionMargins });
                panel.AddChild(option, new(i, 1) { Alignment = TextAnchor.MiddleRight, Margin = optionMargins });
            }
                
            return panel;
        }
    }
}
