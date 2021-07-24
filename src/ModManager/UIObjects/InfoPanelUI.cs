using PeterHan.PLib.UI;
using UnityEngine.UI;

namespace ModManager
{
    class InfoPanelUI : UISource
    {
        protected override IUIComponent GetUIComponent()
        {
            return new PPanel("InfoPanel")
            {
                Direction = PanelDirection.Vertical
            }
            .AddChild(new PLabel()
            {
                Text = "Info Panel"
            })
            .AddOnRealize(go =>
            {
                var le = go.AddOrGet<LayoutElement>();
                le.preferredWidth = 400;
            });
        }
    }
}
