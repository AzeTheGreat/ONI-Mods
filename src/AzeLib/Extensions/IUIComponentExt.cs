using PeterHan.PLib.UI;

namespace AzeLib.Extensions
{
    public static class IUIComponentExt
    {
        // Extension returns the component for call chaining and simpler initialization.
        public static IUIComponent AddOnRealize(this IUIComponent comp, PUIDelegates.OnRealize onRealize)
        {
            comp.OnRealize += onRealize;
            return comp;
        }
    }
}
