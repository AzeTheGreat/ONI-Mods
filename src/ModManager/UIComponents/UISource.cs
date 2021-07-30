using PeterHan.PLib.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ModManager
{
    public abstract class UISource
    {
        public GameObject GO { get; private set; }

        public IUIComponent CreateUIComponent() => GetUIComponent().AddOnRealize(CacheGO);

        public void RebuildGO()
        {
            // These must be extracted before building the new GO since it will overwrite this one.
            var parent = GO.transform.parent;
            var index = GO.transform.GetSiblingIndex();

            // Get new comp, set GO, build new comp
            CreateUIComponent().Build();
            GO.transform.SetParent(parent);
            GO.transform.SetSiblingIndex(index);

            // Unlock the parent, rebuild its layout, then restore its lock state.
            var parentGO = parent.gameObject;

            if (parentGO.GetComponent<PGridLayoutGroup>())
                Debug.LogWarning("Parent GO has PGridLayoutGroup; RebuildGO does not currently support this parent type and layouts may behave unexpectedly.");

            var state = parentGO.GetLayoutLockState();
            parentGO.SetLayoutLockState(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentGO.rectTransform());
            // TODO: This doesn't relock the entire chain - need to get state of every child GO?
            parentGO.SetLayoutLockState(state);
        }

        public void DestroyGO()
        {
            if(GO != null)
            {
                // Unparent extracted GOs so they are not destroyed with the rest of the object.
                foreach (var go in GO.GetComponentsInChildren<ExtractedGO>())
                    go.transform.SetParent(null);

                Object.Destroy(GO);
                // Explicitly setting to null helps avoid any issues with lingering managed objects.
                GO = null;
            }
        }

        protected abstract IUIComponent GetUIComponent();

        private void CacheGO(GameObject go)
        {
            // Ensure that a new GO is never created without destroying the prior GO.
            DestroyGO();
            GO = go;
        }
    }
}
