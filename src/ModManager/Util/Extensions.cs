using HarmonyLib;
using PeterHan.PLib.UI;
using PeterHan.PLib.UI.Layouts;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    public static class Extensions
    {
        // Force rebuild the layout so that it's in the correct and expected state.
        // Then set to locked manually, avoiding rebuilds that would occur from .LockLayout().
        // Using the method would require locking in a precise order, this is simpler.
        public static GameObject LockLayoutNested(this GameObject go)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(go.rectTransform());
            foreach (var cmp in go.GetComponentsInChildren<AbstractLayoutGroup>())
                AccessTools.Field(cmp.GetType(), "locked").SetValue(cmp, true);
            return go;
        }

        public static IUIComponent LockLayout(this IUIComponent cmp) => cmp.AddOnRealize(LockLayout);
        public static void LockLayout(this GameObject go)
        {
            var lg = go.GetComponent<AbstractLayoutGroup>();
            lg?.LockLayout();
        }

        public static IUIComponent AddOnRealize(this IUIComponent cmp, PUIDelegates.OnRealize onRealize)
        {
            cmp.OnRealize += onRealize;
            return cmp;
        }
    }
}
