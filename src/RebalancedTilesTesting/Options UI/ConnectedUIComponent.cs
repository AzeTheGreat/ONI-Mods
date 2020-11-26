using PeterHan.PLib.UI;
using RebalancedTilesTesting.CustomUIComponents;

namespace RebalancedTilesTesting.OptionsUI
{
    public abstract class ConnectedUIComponent<T> : IUISource
    {
        protected readonly T link;
        public ConnectedUIComponent(T link) => this.link = link;
        public abstract IUIComponent GetUIComponent();
    }
}
