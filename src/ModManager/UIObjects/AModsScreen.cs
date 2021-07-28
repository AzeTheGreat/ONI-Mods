using PeterHan.PLib.UI;
using System.Collections.Generic;
using UnityEngine;

namespace ModManager
{
    public class AModsScreen
    {
        public static AModsScreen Instance { get; private set; }

        public List<ModUIExtract> ModUIExtractions { get; set; }
        public GameObject DragElementPrefab { get; set; }

        public AModsScreen()
        {
            Instance = this;
        }

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

            db.AddChild(new BrowserPanelUI().CreateUIComponent())
                .AddChild(new InfoPanelUI().CreateUIComponent());

            return dialog
                .Build()
                .LockLayoutNested()
                .GetComponent<KScreen>();
        }
    }
}
