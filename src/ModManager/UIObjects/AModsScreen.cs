using PeterHan.PLib.UI;
using UnityEngine;

namespace ModManager
{
    public class AModsScreen
    {
        public KScreen GetDialog()
        {
            var dialog = new PDialog("ModsScreen")
            {
                Title = "Mods"
            };

            var db = dialog.Body;
            db.FlexSize = Vector2.right;
            db.Direction = PanelDirection.Horizontal;
            db.Margin = null;
            db.Spacing = 0;

            db.AddChild(new BrowserPanelUI().GetUIComponent())
                .AddChild(new InfoPanelUI().GetUIComponent());

            return dialog.Build().GetComponent<KScreen>();
        }
    }
}
