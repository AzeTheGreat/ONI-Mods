using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using STRINGS;
using UnityEngine;

namespace RebalancedTilesTesting.OptionsUI
{
    public class OptionsScreen
    {
        public EditBody editBody;
        public EditHeader editHeader;
        public SearchHeader searchHeader;
        public SearchBody searchBody;

        private static readonly RectOffset margins = new(1,1,1,1);

        public KScreen GetDialog()
        {
            var dialog = new PDialog(nameof(OptionsScreen))
            {
                Title = "Test",
                SortKey = 150f,
                DialogBackColor = PUITuning.Colors.OptionsBackground,
                DialogClosed = OnDialogClosed,
                RoundToNearestEven = true
            };

            dialog.Body.AddChild(new PGridPanel()
            .AddRow(new GridRowSpec())
            .AddRow(new GridRowSpec(600f))
            .AddColumn(new GridColumnSpec(400f))
            .AddColumn(new GridColumnSpec(200f))
            .AddChild(new SearchHeader(this).GetUIComponent(), new GridComponentSpec(0, 1) { Margin = margins } )
            .AddChild(new SearchBody(this).GetUIComponent(), new GridComponentSpec(1, 1) { Margin = margins } )
            .AddChild(new EditHeader(this).GetUIComponent(), new GridComponentSpec(0, 0) { Margin = margins } )
            .AddChild(new EditBody(this).GetUIComponent(), new GridComponentSpec(1, 0) { Margin = margins } ))
            .Margin = null;

            dialog.AddButton("ok", UI.CONFIRMDIALOG.OK, PUIStrings.TOOLTIP_OK, PUITuning.Colors.ButtonPinkStyle, null);

            return dialog.Build().GetComponent<KScreen>();
        }

        private void OnDialogClosed(string option) => POptions.WriteSettingsForAssembly(Options.Opts);
    }
}