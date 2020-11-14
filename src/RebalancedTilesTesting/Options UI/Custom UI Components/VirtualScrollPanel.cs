using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RebalancedTilesTesting.CustomUIComponents
{
    public class VirtualScrollPanel<T> : PContainer, IDynamicSizable
	{
        public TextAnchor Alignment { get; set; }
		public PanelDirection Direction { get; set; }
		public bool DynamicSize { get; set; }
		public int Spacing { get; set; }
		public IEnumerable<T> Children { get; set; }
		public Func<T, IUIComponent> ChildFactory { get; set; }

		public VirtualScrollPanel() : this(null) { }
		public VirtualScrollPanel(string name) : base(name ?? nameof(VirtualScrollPanel<T>))
		{
			Alignment = TextAnchor.MiddleCenter;
			Direction = PanelDirection.Vertical;
			DynamicSize = true;
			Spacing = 0;
		}

        public override GameObject Build()
		{
			if (Children == null || ChildFactory == null)
				throw new InvalidOperationException("No Children or ChildFactory set.");

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
			refresher.SetChildren(Children.Cast<object>(), (x) => ChildFactory(castFunc(x)));

			InvokeRealize(panel);
			return panel;

            // Can't have generic MonoBehaviours, so cast everything to object and back...
            static T castFunc(object x) => (T)x;
		}
	}
}