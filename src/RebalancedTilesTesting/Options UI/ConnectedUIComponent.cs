using PeterHan.PLib.UI;

namespace RebalancedTilesTesting.OptionsUI
{
    public abstract class ConnectedUIComponent<T>
    {
        protected readonly T link;
        public ConnectedUIComponent(T link) => this.link = link;

        public abstract IUIComponent GetUIComponent();
    }
}
