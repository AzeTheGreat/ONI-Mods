using PeterHan.PLib.UI;
using System.Collections.Generic;
using UnityEngine;

namespace RebalancedTilesTesting
{
    public class VirtualScrollPanel : PContainer, IDynamicSizable
	{
        public TextAnchor Alignment { get; set; }
		public PanelDirection Direction { get; set; }
		public bool DynamicSize { get; set; }
		public int Spacing { get; set; }
		public IEnumerable<IUIComponent> Children { get; set; }

		public VirtualScrollPanel() : this(null) { }
		public VirtualScrollPanel(string name) : base(name ?? nameof(VirtualScrollPanel))
		{
			Alignment = TextAnchor.MiddleCenter;
			Children = new List<IUIComponent>();
			Direction = PanelDirection.Vertical;
			DynamicSize = true;
			Spacing = 0;
		}

        public override GameObject Build()
		{
			var panel = PUIElements.CreateUI(null, Name);
			SetImage(panel);

			var lg = panel.AddComponent<BoxLayoutGroup>();
			lg.Params = new BoxLayoutParams()
			{
				Direction = Direction,
				Alignment = Alignment,
				Spacing = Spacing,
				Margin = Margin
			};

			lg.flexibleWidth = FlexSize.x;
			lg.flexibleHeight = FlexSize.y;

			var refresher = panel.AddComponent<VirtualPanelChildManager>();
			refresher.SetChildren(Children);

			InvokeRealize(panel);
			return panel;
		}
	}
}